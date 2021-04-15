using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitFlags
{
    public class DataSetItem
    {
        public Int64 FlagValue { get; set; }
        public string FlagName { get; set; }
        public string FlagDescription { get; set; }

        public DataSetItem()
        {
            FlagValue = 0;
            FlagName = "N/A";
            FlagDescription = "N/A";
        }

        public DataSetItem(Int64 value, string name, string description)
        {
            FlagValue = value;
            FlagName = name;
            FlagDescription = description;
        }
    }
}
