using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("action_sets")]
    public class ActionSetsDO
    {
        [XmlElement("action_set")]
        public List<ActionSetDO> ActionSets { get; set; } = new List<ActionSetDO>();

        public bool ShouldSerializeActionSets() => ActionSets != null && ActionSets.Count > 0;
    }

    public class ActionSetDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("skeleton")]
        public string? Skeleton { get; set; }

        [XmlAttribute("movement_system")]
        public string? MovementSystem { get; set; }

        [XmlAttribute("base_set")]
        public string? BaseSet { get; set; }

        [XmlElement("action")]
        public List<ActionDO> Actions { get; set; } = new List<ActionDO>();

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeSkeleton() => !string.IsNullOrEmpty(Skeleton);
        public bool ShouldSerializeMovementSystem() => !string.IsNullOrEmpty(MovementSystem);
        public bool ShouldSerializeBaseSet() => !string.IsNullOrEmpty(BaseSet);
        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;
    }

    public class ActionDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("animation")]
        public string? Animation { get; set; }

        [XmlAttribute("alternative_group")]
        public string? AlternativeGroup { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeAnimation() => !string.IsNullOrEmpty(Animation);
        public bool ShouldSerializeAlternativeGroup() => !string.IsNullOrEmpty(AlternativeGroup);
    }
}