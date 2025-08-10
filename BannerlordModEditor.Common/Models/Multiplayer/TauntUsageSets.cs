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
        
        public bool ShouldSerializeTauntUsageSet() => TauntUsageSet != null && TauntUsageSet.Count > 0;
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
        
        public bool ShouldSerializeTauntUsage() => TauntUsage != null && TauntUsage.Count > 0;
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
        public bool ShouldSerializeRequiresBow() => !string.IsNullOrEmpty(RequiresBow);

        [XmlAttribute("requires_on_foot")]
        public string? RequiresOnFoot { get; set; }
        public bool ShouldSerializeRequiresOnFoot() => !string.IsNullOrEmpty(RequiresOnFoot);

        [XmlAttribute("requires_shield")]
        public string? RequiresShield { get; set; }
        public bool ShouldSerializeRequiresShield() => !string.IsNullOrEmpty(RequiresShield);

        [XmlAttribute("requires_two_handed")]
        public string? RequiresTwoHanded { get; set; }
        public bool ShouldSerializeRequiresTwoHanded() => !string.IsNullOrEmpty(RequiresTwoHanded);

        [XmlAttribute("requires_crossbow")]
        public string? RequiresCrossbow { get; set; }
        public bool ShouldSerializeRequiresCrossbow() => !string.IsNullOrEmpty(RequiresCrossbow);

        [XmlAttribute("requires_empty")]
        public string? RequiresEmpty { get; set; }
        public bool ShouldSerializeRequiresEmpty() => !string.IsNullOrEmpty(RequiresEmpty);

        // Stance conditions
        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }
        public bool ShouldSerializeIsLeftStance() => !string.IsNullOrEmpty(IsLeftStance);

        [XmlAttribute("is_right_stance")]
        public string? IsRightStance { get; set; }
        public bool ShouldSerializeIsRightStance() => !string.IsNullOrEmpty(IsRightStance);

        // Unsuitability conditions
        [XmlAttribute("unsuitable_for_shield")]
        public string? UnsuitableForShield { get; set; }
        public bool ShouldSerializeUnsuitableForShield() => !string.IsNullOrEmpty(UnsuitableForShield);

        [XmlAttribute("unsuitable_for_bow")]
        public string? UnsuitableForBow { get; set; }
        public bool ShouldSerializeUnsuitableForBow() => !string.IsNullOrEmpty(UnsuitableForBow);

        [XmlAttribute("unsuitable_for_crossbow")]
        public string? UnsuitableForCrossbow { get; set; }
        public bool ShouldSerializeUnsuitableForCrossbow() => !string.IsNullOrEmpty(UnsuitableForCrossbow);

        [XmlAttribute("unsuitable_for_two_handed")]
        public string? UnsuitableForTwoHanded { get; set; }
        public bool ShouldSerializeUnsuitableForTwoHanded() => !string.IsNullOrEmpty(UnsuitableForTwoHanded);

        [XmlAttribute("unsuitable_for_empty")]
        public string? UnsuitableForEmpty { get; set; }
        public bool ShouldSerializeUnsuitableForEmpty() => !string.IsNullOrEmpty(UnsuitableForEmpty);

        [XmlAttribute("unsuitable_for_one_handed")]
        public string? UnsuitableForOneHanded { get; set; }
        public bool ShouldSerializeUnsuitableForOneHanded() => !string.IsNullOrEmpty(UnsuitableForOneHanded);

        [XmlAttribute("unsuitable_for_polearm")]
        public string? UnsuitableForPolearm { get; set; }
        public bool ShouldSerializeUnsuitableForPolearm() => !string.IsNullOrEmpty(UnsuitableForPolearm);

        // Mount conditions
        [XmlAttribute("requires_mount")]
        public string? RequiresMount { get; set; }
        public bool ShouldSerializeRequiresMount() => !string.IsNullOrEmpty(RequiresMount);

        [XmlAttribute("unsuitable_for_mount")]
        public string? UnsuitableForMount { get; set; }
        public bool ShouldSerializeUnsuitableForMount() => !string.IsNullOrEmpty(UnsuitableForMount);
    }
} 