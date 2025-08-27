using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("full_movement_sets")]
    public class FullMovementSetsDO
    {
        [XmlElement("full_movement_set")]
        public List<FullMovementSetDO> FullMovementSet { get; set; } = new List<FullMovementSetDO>();

        public bool ShouldSerializeFullMovementSet() => FullMovementSet.Count > 0;
    }

    public class FullMovementSetDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("base_set")]
        public string? BaseSet { get; set; }

        [XmlElement("movement_set")]
        public List<FullMovementSetDataDO> MovementSet { get; set; } = new List<FullMovementSetDataDO>();

        public bool ShouldSerializeMovementSet() => MovementSet.Count > 0;
        public bool ShouldSerializeBaseSet() => !string.IsNullOrEmpty(BaseSet);
    }

    public class FullMovementSetDataDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("left_stance")]
        public string LeftStance { get; set; } = string.Empty;

        [XmlAttribute("movement_mode")]
        public string MovementMode { get; set; } = string.Empty;
    }
}