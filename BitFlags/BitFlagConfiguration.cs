using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace BitFlags
{
    public class BitFlagConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("BitFlagCollection", IsRequired = false)]
        public BitFlagGroups BitFlagGroups
        {
            get { return this["BitFlagCollection"] as BitFlagGroups; }
        }

    }

    [ConfigurationCollection(typeof(BitFlagGroups), AddItemName = "BitFlagGroup")]
    public class BitFlagGroups : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BitFlagGroup();
        }

        public BitFlagGroup this[int index]
        {
            get { return base.BaseGet(index) as BitFlagGroup; }
        }

        public BitFlagGroup this[string key]
        {
            get { return (BitFlagGroup)base.BaseGet(key); }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BitFlagGroup)(element)).Name;
        }
    }

    [ConfigurationCollection(typeof(BitFlags), AddItemName = "BitFlag")]
    public class BitFlags : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BitFlag();
        }

        public BitFlag this[int index]
        {
            get { return base.BaseGet(index) as BitFlag; }
        }

        /*
        public BitFlagElement this[string key]
        {
            get { return (BitFlagElement)base.BaseGet(key); }
        }
        */

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BitFlag)(element)).Name;
        }
    }


    public class BitFlagGroup : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("BitFlags", IsRequired = false)]
        public BitFlags Flags
        {
            get { return this["BitFlags"] as BitFlags; }
        }

    }

    public class BitFlag : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public Int64 Value
        {
            get { return (Int64)this["value"]; }
            set { this["value"] = value; }
        }
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("description")]
        public string Description
        {
            get { return (string)this["description"]; }
            set { this["description"] = value; }
        }

    }
}
