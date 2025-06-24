using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("full_movement_sets")]
    public class FullMovementSets
    {
        [XmlElement("full_movement_set")]
        public List<FullMovementSet> FullMovementSet { get; set; } = new List<FullMovementSet>();

        public bool ShouldSerializeFullMovementSet() => FullMovementSet.Count > 0;
    }

    public class FullMovementSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("base_set")]
        public string? BaseSet { get; set; }

        [XmlElement("movement_set")]
        public List<FullMovementSetData> MovementSet { get; set; } = new List<FullMovementSetData>();

        public bool ShouldSerializeMovementSet() => MovementSet.Count > 0;
    }

    public class FullMovementSetData
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("left_stance")]
        public string LeftStance { get; set; } = string.Empty;

        [XmlAttribute("movement_mode")]
        public string MovementMode { get; set; } = string.Empty;
    }
} 