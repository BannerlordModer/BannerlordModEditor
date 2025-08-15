using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("action_types")]
    public class ActionTypesList
    {
        [XmlElement("action")]
        public List<ActionType> Actions { get; set; } = new List<ActionType>();
    }

    public class ActionType
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("usage_direction")]
        public string? UsageDirection { get; set; }

        public bool ShouldSerializeType() => Type != null;
        public bool ShouldSerializeUsageDirection() => UsageDirection != null;
    }
} 