using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Adjustables")]
    public class Adjustables
    {
        [XmlElement("Adjustable")]
        public List<Adjustable> AdjustableList { get; set; } = new List<Adjustable>();

        public bool ShouldSerializeAdjustableList() => AdjustableList != null && AdjustableList.Count > 0;
    }

    public class Adjustable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }
}
