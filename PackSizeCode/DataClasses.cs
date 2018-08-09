using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackSizeCode
{
    public class DataClasses
    {
        public class StartingCoordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class RootObject
        {
            public int InstructionNumber { get; set; }
            public string Type { get; set; }
            public StartingCoordinate StartingCoordinate { get; set; }
            public string TravelDirection { get; set; }
            public int Length { get; set; }
        }
    }
}
