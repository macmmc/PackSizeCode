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
                if (temp[i].Type == "Cut")
                {
                    dtCommands.Rows.Add("Move cross-head to " + temp[i].StartingCoordinate.X);
                    dtCommands.Rows.Add("Lower cross-head knife");
                    if (temp[i].InstructionNumber < 21)
                    {
                        dtCommands.Rows.Add("Move cross-head to " + (temp[i + 1].StartingCoordinate.Y));
                    }
                    else
                    {
                        dtCommands.Rows.Add("Move cross-head to 0");
                    }
                    dtCommands.Rows.Add("Raise cross-head knife");
                }
                else if (temp[i].Type == "Crease")
                {
                    dtCommands.Rows.Add("Move long-head to " + temp[i].StartingCoordinate.X);
                    dtCommands.Rows.Add("Lower long-head knife");
                }
            }
            var JSON = DataTableToJSON(dtCommands);
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
                jsonString.Append("[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                        jsonString.Append("\"" + dt.Rows[i][j].ToString() + "\"" + "\r\n");
                }
                return jsonString.Append("]").ToString();
            }
            else
            {
                return "[]";
            }
        }
    }
}
