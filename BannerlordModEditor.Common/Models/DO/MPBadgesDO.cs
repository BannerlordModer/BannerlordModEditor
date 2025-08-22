using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// Represents the mpbadges.xml file structure containing multiplayer badge definitions
    /// </summary>
    [XmlRoot("Badges")]
    public class MPBadgesDO
    {
        /// <summary>
        /// Collection of badge definitions
        /// </summary>
        [XmlElement("Badge")]
        public List<BadgeDO> BadgeList { get; set; } = new List<BadgeDO>();

        public bool ShouldSerializeBadgeList() => BadgeList != null && BadgeList.Count > 0;
    }

    /// <summary>
    /// Represents a multiplayer badge definition with conditions for earning
    /// </summary>
    public class BadgeDO
    {
        /// <summary>
        /// Unique identifier for the badge
        /// </summary>
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Type of badge (Custom, Conditional, etc.)
        /// </summary>
        [XmlAttribute("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Group identifier for badge categorization
        /// </summary>
        [XmlAttribute("group_id")]
        public string? GroupId { get; set; }

        /// <summary>
        /// Display name of the badge (may include localization keys)
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Description of the badge (may include localization keys)
        /// </summary>
        [XmlAttribute("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Whether the badge is only visible when earned
        /// </summary>
        [XmlAttribute("is_visible_only_when_earned")]
        public string? IsVisibleOnlyWhenEarned { get; set; }

        /// <summary>
        /// Start time for time-limited badges (format: MM/dd/yyyy HH:mm:ss)
        /// </summary>
        [XmlAttribute("period_start")]
        public string? PeriodStart { get; set; }

        /// <summary>
        /// End time for time-limited badges (format: MM/dd/yyyy HH:mm:ss)
        /// </summary>
        [XmlAttribute("period_end")]
        public string? PeriodEnd { get; set; }

        /// <summary>
        /// Collection of conditions that must be met to earn this badge
        /// </summary>
        [XmlElement("Condition")]
        public List<BadgeConditionDO> Conditions { get; set; } = new List<BadgeConditionDO>();

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeGroupId() => !string.IsNullOrEmpty(GroupId);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
        public bool ShouldSerializeIsVisibleOnlyWhenEarned() => !string.IsNullOrEmpty(IsVisibleOnlyWhenEarned);
        public bool ShouldSerializePeriodStart() => !string.IsNullOrEmpty(PeriodStart);
        public bool ShouldSerializePeriodEnd() => !string.IsNullOrEmpty(PeriodEnd);
        public bool ShouldSerializeConditions() => Conditions != null && Conditions.Count > 0;
    }

    /// <summary>
    /// Represents a condition that must be met for earning a badge
    /// </summary>
    public class BadgeConditionDO
    {
        /// <summary>
        /// Type of condition (PlayerDataNumeric, BadgeOwnerKill, etc.)
        /// </summary>
        [XmlAttribute("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Group type for condition evaluation (Solo, Party, etc.)
        /// </summary>
        [XmlAttribute("group_type")]
        public string? GroupType { get; set; }

        /// <summary>
        /// Description of the condition (may include localization keys)
        /// </summary>
        [XmlAttribute("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Collection of parameters that define the specific requirements
        /// </summary>
        [XmlElement("Parameter")]
        public List<BadgeParameterDO> Parameters { get; set; } = new List<BadgeParameterDO>();

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeGroupType() => !string.IsNullOrEmpty(GroupType);
        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
        public bool ShouldSerializeParameters() => Parameters != null && Parameters.Count > 0;
    }

    /// <summary>
    /// Represents a parameter within a badge condition
    /// </summary>
    public class BadgeParameterDO
    {
        /// <summary>
        /// Name of the parameter (property, min_value, is_best, etc.)
        /// </summary>
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }
}