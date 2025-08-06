using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Achievements")]
    public class GogAchievementData
    {
        [XmlElement("Achievement")]
        public List<GogAchievement> Achievements { get; set; } = new List<GogAchievement>();
    }

    public class GogAchievement
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Requirements")]
        public GogAchievementRequirements Requirements { get; set; }
    }

    public class GogAchievementRequirements
    {
        [XmlElement("Requirement")]
        public List<GogAchievementRequirement> RequirementList { get; set; } = new List<GogAchievementRequirement>();
    }

    public class GogAchievementRequirement
    {
        [XmlAttribute("statName")]
        public string StatName { get; set; }

        [XmlAttribute("threshold")]
        public int Threshold { get; set; }
    }
}