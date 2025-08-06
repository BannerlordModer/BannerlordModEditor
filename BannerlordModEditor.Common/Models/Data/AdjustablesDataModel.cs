using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Adjustables")]
    public class AdjustablesDataModel
    {
        [XmlElement("Adjustable")]
        public List<AdjustableDataModel> Adjustables { get; set; } = new List<AdjustableDataModel>();

        public bool ShouldSerializeAdjustables() => Adjustables != null && Adjustables.Count > 0;
    }

    public class AdjustableDataModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }
}