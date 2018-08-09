using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Data;

namespace PackSizeCode
{
    class Program
    {
        public static DataTable dtCommands = new DataTable();
        static void Main(string[] args)
        {
            dtCommands.Columns.AddRange(new DataColumn[1] { new DataColumn("command") });
            var temp = parseJSON(args[0]);
            for (int i = 0; i <= temp.Count - 1; i++)
            {
                if (i < temp.Count - 1)
                {
                    if (temp[i].StartingCoordinate.Y > 0)
                    {
                        if (temp[i].Type == "Cut")
                        {
                            if (!String.IsNullOrEmpty(temp[i - 1].Type) && temp[i - 1].Type == "Cut")
                            {

                                dtCommands.Rows.Add("Move long-head 1 to " + (temp[i + 1].StartingCoordinate.X));
                                dtCommands.Rows.Add("Lower long-head 1 " + temp[i + 1].Type);
                                dtCommands.Rows.Add("Move long-head 2 to " + temp[i + 1].StartingCoordinate.X);
                                dtCommands.Rows.Add("Lower long-head 2 " + temp[i + 1].Type);
                            }
                            else
                            {
                                dtCommands.Rows.Add("Lower cross-head knife");
                                dtCommands.Rows.Add("Move cross-head to " + (temp[i].StartingCoordinate.X));
                                dtCommands.Rows.Add("Raise cross-head knife");
                            }
                        }
                        if (temp[i].Type == "Crease")
                        {
                            dtCommands.Rows.Add("Move long-head 3 to " + temp[i].StartingCoordinate.X);
                            dtCommands.Rows.Add("Lower long-head 3 knife");
                            dtCommands.Rows.Add("");
                        }
                    }
                    else
                    {
                        dtCommands.Rows.Add("Move cross-head to " + temp[i].StartingCoordinate.X);
                        dtCommands.Rows.Add("Lower cross-head knife");
                        dtCommands.Rows.Add("Move cross-head to " + (temp[i + 1].StartingCoordinate.X));
                        dtCommands.Rows.Add("Raise cross-head knife");
                    }
                }
            }
            var finalCommandListJSON = DataTableToJSON(dtCommands);
        }

        private static List<DataClasses.RootObject> parseJSON(string v)
        {
            List<DataClasses.RootObject> commands = (List<DataClasses.RootObject>)JsonConvert.DeserializeObject(v, typeof(List<DataClasses.RootObject>));
            return commands;
        }
        public static string DataTableToJSON(DataTable dt)
        {
            var jsonString = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                jsonString.Append("[" + "\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            jsonString.Append("\n");
                        }
                        else
                        {
                            jsonString.Append("\"" + dt.Rows[i][j].ToString() + "\"" + "\r\n");
                        }
                }
                return jsonString.Append("\r\n" + "]").ToString();
            }
            else
            {
                return "[]";
            }
        }
    }
}
