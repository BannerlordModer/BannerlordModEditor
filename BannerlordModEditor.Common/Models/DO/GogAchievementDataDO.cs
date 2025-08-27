using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("Achievements")]
    public class GogAchievementDataDO
    {
        [XmlElement("Achievement")]
        public List<AchievementDO> Achievements { get; set; } = new List<AchievementDO>();

        [XmlIgnore]
        public bool HasAchievements { get; set; } = false;

        public bool ShouldSerializeAchievements() => HasAchievements && Achievements != null && Achievements.Count > 0;
    }

    public class AchievementDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Requirements")]
        public AchievementRequirementsDO? Requirements { get; set; }

        [XmlIgnore]
        public bool HasRequirements { get; set; } = false;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeRequirements() => HasRequirements && Requirements != null && Requirements.Requirements.Count > 0;
    }

    public class AchievementRequirementsDO
    {
        [XmlElement("Requirement")]
        public List<AchievementRequirementDO> Requirements { get; set; } = new List<AchievementRequirementDO>();
    }

    public class AchievementRequirementDO
    {
        [XmlAttribute("statName")]
        public string StatName { get; set; } = string.Empty;

        [XmlAttribute("threshold")]
        public string Threshold { get; set; } = string.Empty;

        public bool ShouldSerializeStatName() => !string.IsNullOrEmpty(StatName);
        public bool ShouldSerializeThreshold() => !string.IsNullOrEmpty(Threshold);
    }
}