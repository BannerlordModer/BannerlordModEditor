using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Multiplayer
{
    /// <summary>
    /// Root element for taunt_usage_sets.xml - Contains taunt usage definitions
    /// </summary>
    [XmlRoot("taunt_usage_sets")]
    public class TauntUsageSets
    {
        [XmlElement("taunt_usage_set")]
        public List<TauntUsageSet> TauntUsageSet { get; set; } = new List<TauntUsageSet>();
    }

    /// <summary>
    /// Collection of taunt usages for a specific taunt action
    /// </summary>
    public class TauntUsageSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("taunt_usage")]
        public List<TauntUsage> TauntUsage { get; set; } = new List<TauntUsage>();
    }

    /// <summary>
    /// Individual taunt usage with conditions and action
    /// </summary>
    public class TauntUsage
    {
        [XmlAttribute("action")]
        public string Action { get; set; } = string.Empty;

        // Requirement conditions
        [XmlAttribute("requires_bow")]
        public string? RequiresBow { get; set; }

        [XmlAttribute("requires_on_foot")]
        public string? RequiresOnFoot { get; set; }

        [XmlAttribute("requires_shield")]
        public string? RequiresShield { get; set; }

        [XmlAttribute("requires_two_handed")]
        public string? RequiresTwoHanded { get; set; }

        [XmlAttribute("requires_crossbow")]
        public string? RequiresCrossbow { get; set; }

        [XmlAttribute("requires_empty")]
        public string? RequiresEmpty { get; set; }

        // Stance conditions
        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("is_right_stance")]
        public string? IsRightStance { get; set; }

        // Unsuitability conditions
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

        [XmlAttribute("unsuitable_for_polearm")]
        public string? UnsuitableForPolearm { get; set; }

        // Mount conditions
        [XmlAttribute("requires_mount")]
        public string? RequiresMount { get; set; }

        [XmlAttribute("unsuitable_for_mount")]
        public string? UnsuitableForMount { get; set; }
    }
} 