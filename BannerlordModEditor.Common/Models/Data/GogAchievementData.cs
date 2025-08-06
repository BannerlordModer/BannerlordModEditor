using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// GOG成就数据模型
    /// 原本实现：无
    /// 简化实现：使用增强的XML映射类系统实现字段存在性追踪
    /// </summary>
    [XmlRoot("Achievements")]
    public class GogAchievements : XmlModelBase
    {
        private List<GogAchievement> _achievements = new();

        [XmlElement("Achievement")]
        public List<GogAchievement> Achievements
        {
            get => _achievements;
            set
            {
                _achievements = value ?? new List<GogAchievement>();
                MarkPropertyExists(nameof(Achievements));
            }
        }

        public bool ShouldSerializeAchievements() => PropertyExists(nameof(Achievements));
    }

    /// <summary>
    /// 成就模型
    /// </summary>
    public class GogAchievement : XmlModelBase
    {
        private string? _name;
        private GogRequirements? _requirements;

        [XmlAttribute("name")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkPropertyExists(nameof(Name));
            }
        }

        [XmlArray("Requirements")]
        [XmlArrayItem("Requirement")]
        public GogRequirements? Requirements
        {
            get => _requirements;
            set
            {
                _requirements = value;
                MarkPropertyExists(nameof(Requirements));
            }
        }

        /// <summary>
        /// 检查Name属性在XML中是否存在
        /// </summary>
        public bool NameExistsInXml => PropertyExists(nameof(Name));

        /// <summary>
        /// 检查Requirements属性在XML中是否存在
        /// </summary>
        public bool RequirementsExistsInXml => PropertyExists(nameof(Requirements));
    }

    /// <summary>
    /// 要求集合模型
    /// </summary>
    [XmlRoot("Requirements")]
    public class GogRequirements : List<GogRequirement>
    {
        public GogRequirements()
        {
        }

        public GogRequirements(IEnumerable<GogRequirement> collection) : base(collection)
        {
        }
    }

    /// <summary>
    /// 要求模型
    /// </summary>
    public class GogRequirement : XmlModelBase
    {
        private string? _statName;
        private string? _threshold;

        [XmlAttribute("statName")]
        public string? StatName
        {
            get => _statName;
            set
            {
                _statName = value;
                MarkPropertyExists(nameof(StatName));
            }
        }

        [XmlAttribute("threshold")]
        public string? Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                MarkPropertyExists(nameof(Threshold));
            }
        }

        /// <summary>
        /// 检查StatName属性在XML中是否存在
        /// </summary>
        public bool StatNameExistsInXml => PropertyExists(nameof(StatName));

        /// <summary>
        /// 检查Threshold属性在XML中是否存在
        /// </summary>
        public bool ThresholdExistsInXml => PropertyExists(nameof(Threshold));
    }
}