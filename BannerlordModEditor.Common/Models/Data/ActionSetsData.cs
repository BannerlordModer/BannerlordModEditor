using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("action_sets")]
    public class ActionSetsData
    {
        [XmlElement("action_set")]
        public List<ActionSet> ActionSets { get; set; }

        public bool ShouldSerializeActionSets() => ActionSets != null && ActionSets.Count > 0;
    }

    public class ActionSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("skeleton")]
        public string Skeleton { get; set; }

        [XmlAttribute("movement_system")]
        public string MovementSystem { get; set; }

        [XmlElement("action")]
        public List<Action> Actions { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeSkeleton() => !string.IsNullOrEmpty(Skeleton);
        public bool ShouldSerializeMovementSystem() => !string.IsNullOrEmpty(MovementSystem);
        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;
    }

    public class Action
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("animation")]
        public string Animation { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeAnimation() => !string.IsNullOrEmpty(Animation);
    }
}