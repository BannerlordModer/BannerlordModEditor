using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// Root element for gog_achievement_data.xml - Contains achievement definitions
    /// </summary>
    [XmlRoot("Achievements")]
    public class Achievements
    {
        [XmlElement("Achievement")]
        public List<Achievement> Achievement { get; set; } = new List<Achievement>();
    }

    /// <summary>
    /// Individual achievement definition with requirements
    /// </summary>
    public class Achievement
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Requirements")]
        public AchievementRequirements? Requirements { get; set; }
    }

    /// <summary>
    /// Container for achievement requirements
    /// </summary>
    public class AchievementRequirements
    {
        [XmlElement("Requirement")]
        public List<AchievementRequirement> Requirement { get; set; } = new List<AchievementRequirement>();
    }

    /// <summary>
    /// Individual requirement for an achievement
    /// </summary>
    public class AchievementRequirement
    {
        [XmlAttribute("statName")]
        public string StatName { get; set; } = string.Empty;

        [XmlAttribute("threshold")]
        public string Threshold { get; set; } = string.Empty;
    }
} 