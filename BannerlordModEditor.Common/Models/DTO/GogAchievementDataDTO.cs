using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("Achievements")]
    public class GogAchievementDataDTO
    {
        [XmlElement("Achievement")]
        public List<AchievementDTO> Achievements { get; set; } = new List<AchievementDTO>();
    }

    public class AchievementDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Requirements")]
        public AchievementRequirementsDTO? Requirements { get; set; }
    }

    public class AchievementRequirementsDTO
    {
        [XmlElement("Requirement")]
        public List<AchievementRequirementDTO> Requirements { get; set; } = new List<AchievementRequirementDTO>();
    }

    public class AchievementRequirementDTO
    {
        [XmlAttribute("statName")]
        public string StatName { get; set; } = string.Empty;

        [XmlAttribute("threshold")]
        public string Threshold { get; set; } = string.Empty;
    }
}