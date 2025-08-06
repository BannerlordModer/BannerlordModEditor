using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("action_types")]
    public class ActionTypesRoot
    {
        [XmlElement("action")]
        public List<ActionTypeModel> Actions { get; set; } = new List<ActionTypeModel>();

        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;
    }

    public class ActionTypeModel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("usage_direction")]
        public string UsageDirection { get; set; }

        public bool ShouldSerializeType()
        {
            return !string.IsNullOrEmpty(Type);
        }

        public bool ShouldSerializeUsageDirection()
        {
            return !string.IsNullOrEmpty(UsageDirection);
        }
    }
}