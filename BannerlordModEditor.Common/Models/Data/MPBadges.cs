using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// Represents the mpbadges.xml file structure containing multiplayer badge definitions
    /// </summary>
    [XmlRoot("Badges")]
    public class MPBadges
    {
        /// <summary>
        /// Collection of badge definitions
        /// </summary>
        [XmlElement("Badge")]
        public Badge[]? Badges { get; set; }
    }

    /// <summary>
    /// Represents a multiplayer badge definition with conditions for earning
    /// </summary>
    public class Badge
    {
        /// <summary>
        /// Unique identifier for the badge
        /// </summary>
        [XmlAttribute("id")]
        [Required]
        public string? Id { get; set; }

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

        [XmlElement("Condition")]
        public BadgeCondition[]? Conditions { get; set; }
    }

    /// <summary>
    /// Represents a condition that must be met for earning a badge
    /// </summary>
    public class BadgeCondition
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
        
        [XmlElement("Parameter")]
        public BadgeParameter[]? Parameters { get; set; }
    }

    /// <summary>
    /// Represents a parameter within a badge condition
    /// </summary>
    public class BadgeParameter
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
    }
} 