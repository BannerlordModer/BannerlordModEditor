using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("action_types")]
    public class ActionTypesDO
    {
        [XmlElement("action")]
        public List<ActionTypeDO> Actions { get; set; } = new List<ActionTypeDO>();

        [XmlIgnore]
        public bool HasEmptyActions { get; set; } = false;

        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0 && !HasEmptyActions;
    }

    public class ActionTypeDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("usage_direction")]
        public string? UsageDirection { get; set; }

        [XmlAttribute("action_stage")]
        public string? ActionStage { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeUsageDirection() => !string.IsNullOrEmpty(UsageDirection);
        public bool ShouldSerializeActionStage() => !string.IsNullOrEmpty(ActionStage);
    }
}