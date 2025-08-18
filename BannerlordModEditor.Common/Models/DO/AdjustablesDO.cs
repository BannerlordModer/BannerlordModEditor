using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("Adjustables")]
    public class AdjustablesDO
    {
        [XmlElement("Adjustable")]
        public List<AdjustableDO> Adjustables { get; set; } = new List<AdjustableDO>();

        public bool ShouldSerializeAdjustables() => Adjustables != null && Adjustables.Count > 0;
    }

    public class AdjustableDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }
}