using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Multiplayer
{
    /// <summary>
    /// Represents the mpbadges.xml file structure containing multiplayer badge definitions
    /// </summary>
    [XmlRoot("Badges")]
    public class BadgesDO
    {
        /// <summary>
        /// Collection of badge definitions
        /// </summary>
        [XmlElement("Badge")]
        public List<BadgeDO> BadgeList { get; set; } = new List<BadgeDO>();

        [XmlIgnore]
        public bool HasBadgeList => BadgeList != null && BadgeList.Count > 0;
    }

    /// <summary>
    /// Represents a multiplayer badge definition
    /// </summary>
    public class BadgeDO
    {
        // Basic Identity Properties
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("description")]
        public string? Description { get; set; }

        // Visibility Properties
        [XmlAttribute("is_visible_only_when_earned")]
        public string? IsVisibleOnlyWhenEarned { get; set; }

        // Time Period Properties (for conditional badges)
        [XmlAttribute("period_start")]
        public string? PeriodStart { get; set; }

        [XmlAttribute("period_end")]
        public string? PeriodEnd { get; set; }

        /// <summary>
        /// Conditions for earning the badge
        /// </summary>
        [XmlElement("Condition")]
        public List<BadgeConditionDO> Conditions { get; set; } = new List<BadgeConditionDO>();

        // Serialization control methods
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
        public bool ShouldSerializeIsVisibleOnlyWhenEarned() => !string.IsNullOrEmpty(IsVisibleOnlyWhenEarned);
        public bool ShouldSerializePeriodStart() => !string.IsNullOrEmpty(PeriodStart);
        public bool ShouldSerializePeriodEnd() => !string.IsNullOrEmpty(PeriodEnd);
        public bool ShouldSerializeConditions() => Conditions != null && Conditions.Count > 0;
    }

    /// <summary>
    /// Represents a condition for earning a badge
    /// </summary>
    public class BadgeConditionDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("group_type")]
        public string? GroupType { get; set; }

        [XmlAttribute("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Parameters for the condition
        /// </summary>
        [XmlElement("Parameter")]
        public List<BadgeParameterDO> Parameters { get; set; } = new List<BadgeParameterDO>();

        // Serialization control methods
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeGroupType() => !string.IsNullOrEmpty(GroupType);
        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
        public bool ShouldSerializeParameters() => Parameters != null && Parameters.Count > 0;
    }

    /// <summary>
    /// Represents a parameter for a badge condition
    /// </summary>
    public class BadgeParameterDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        // Serialization control methods
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }
}