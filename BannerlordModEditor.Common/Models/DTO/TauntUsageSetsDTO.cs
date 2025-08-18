using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class TauntUsageSetsDTO
    {
        [XmlElement("taunt_usage_set")]
        public List<TauntUsageSetDTO> TauntUsageSetList { get; set; } = new List<TauntUsageSetDTO>();
    }

    public class TauntUsageSetDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("taunt_usage")]
        public List<TauntUsageDTO> TauntUsages { get; set; } = new List<TauntUsageDTO>();
    }

    public class TauntUsageDTO
    {
        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("requires_bow")]
        public string? RequiresBow { get; set; }

        [XmlAttribute("requires_on_foot")]
        public string? RequiresOnFoot { get; set; }

        [XmlAttribute("requires_shield")]
        public string? RequiresShield { get; set; }

        [XmlAttribute("unsuitable_for_shield")]
        public string? UnsuitableForShield { get; set; }

        [XmlAttribute("unsuitable_for_bow")]
        public string? UnsuitableForBow { get; set; }

        [XmlAttribute("unsuitable_for_crossbow")]
        public string? UnsuitableForCrossbow { get; set; }

        [XmlAttribute("unsuitable_for_two_handed")]
        public string? UnsuitableForTwoHanded { get; set; }

        [XmlAttribute("unsuitable_for_empty")]
        public string? UnsuitableForEmpty { get; set; }

        [XmlAttribute("unsuitable_for_one_handed")]
        public string? UnsuitableForOneHanded { get; set; }

        [XmlAttribute("action")]
        public string Action { get; set; } = string.Empty;
    }
}