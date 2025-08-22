using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("full_movement_sets")]
    public class FullMovementSetsDTO
    {
        [XmlElement("full_movement_set")]
        public List<FullMovementSetDTO> FullMovementSet { get; set; } = new List<FullMovementSetDTO>();
    }

    public class FullMovementSetDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("base_set")]
        public string? BaseSet { get; set; }

        [XmlElement("movement_set")]
        public List<FullMovementSetDataDTO> MovementSet { get; set; } = new List<FullMovementSetDataDTO>();
    }

    public class FullMovementSetDataDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("left_stance")]
        public string LeftStance { get; set; } = string.Empty;

        [XmlAttribute("movement_mode")]
        public string MovementMode { get; set; } = string.Empty;
    }
}