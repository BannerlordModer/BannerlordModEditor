using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Multiplayer
{
    /// <summary>
    /// Represents the mpbadges.xml file structure containing multiplayer badge definitions
    /// </summary>
    [XmlRoot("Badges")]
    public class BadgesDTO
    {
        /// <summary>
        /// Collection of badge definitions
        /// </summary>
        [XmlElement("Badge")]
        public List<BadgeDTO> BadgeList { get; set; } = new List<BadgeDTO>();
    }

    /// <summary>
    /// Represents a multiplayer badge definition
    /// </summary>
    public class BadgeDTO
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
        public List<BadgeConditionDTO> Conditions { get; set; } = new List<BadgeConditionDTO>();
    }

    /// <summary>
    /// Represents a condition for earning a badge
    /// </summary>
    public class BadgeConditionDTO
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
        public List<BadgeParameterDTO> Parameters { get; set; } = new List<BadgeParameterDTO>();
    }

    /// <summary>
    /// Represents a parameter for a badge condition
    /// </summary>
    public class BadgeParameterDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }
    }
}