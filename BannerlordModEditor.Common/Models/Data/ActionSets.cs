using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("action_sets")]
    public class ActionSets
    {
        [XmlElement("action_set")]
        public List<ActionSet> ActionSetList { get; set; } = new List<ActionSet>();

        public bool ShouldSerializeActionSetList() => ActionSetList != null && ActionSetList.Count > 0;
    }

    public class ActionSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("skeleton")]
        public string Skeleton { get; set; }

        [XmlAttribute("movement_system")]
        public string MovementSystem { get; set; }

        [XmlAttribute("base_set")]
        public string BaseSet { get; set; }

        [XmlElement("action")]
        public List<ActionEntry> Actions { get; set; } = new List<ActionEntry>();

        public bool ShouldSerializeSkeleton() => !string.IsNullOrEmpty(Skeleton);
        public bool ShouldSerializeMovementSystem() => !string.IsNullOrEmpty(MovementSystem);
        public bool ShouldSerializeBaseSet() => !string.IsNullOrEmpty(BaseSet);
        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;
    }

    public class ActionEntry
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("animation")]
        public string Animation { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeAnimation() => !string.IsNullOrEmpty(Animation);
    }
}