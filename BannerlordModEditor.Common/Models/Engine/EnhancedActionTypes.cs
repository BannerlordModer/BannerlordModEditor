using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Models.Engine
{
    /// <summary>
    /// 优化的ActionTypes模型，能够正确区分字段为空和字段不存在
    /// 原本实现：使用简单的string类型和ShouldSerialize方法
    /// 简化实现：使用XmlModelBase基类和更精确的字段存在性追踪
    /// </summary>
    [XmlRoot("action_types")]
    public class EnhancedActionTypesList : XmlModelBase
    {
        [XmlElement("action")]
        public List<EnhancedActionType> Actions { get; set; } = new List<EnhancedActionType>();

        public bool ShouldSerializeActions() => Actions?.Count > 0;
    }

    /// <summary>
    /// 优化的ActionType模型，展示字段存在性追踪
    /// 原本实现：ActionType类，使用简单的string?和ShouldSerialize方法
    /// 简化实现：EnhancedActionType类，集成XmlModelBase并提供更好的字段存在性追踪
    /// </summary>
    public class EnhancedActionType : XmlModelBase
    {
        private string? _name;
        private string? _type;
        private string? _usageDirection;

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

        [XmlAttribute("type")]
        public string? Type
        {
            get => _type;
            set
            {
                _type = value;
                MarkPropertyExists(nameof(Type));
            }
        }

        [XmlAttribute("usage_direction")]
        public string? UsageDirection
        {
            get => _usageDirection;
            set
            {
                _usageDirection = value;
                MarkPropertyExists(nameof(UsageDirection));
            }
        }

        /// <summary>
        /// 重写ShouldSerialize方法以使用更精确的存在性检查
        /// 这是简化实现：原本使用简单的string.IsNullOrEmpty检查
        /// 现在使用PropertyExists来区分字段不存在和字段为空的情况
        /// </summary>
        public bool ShouldSerializeType() => PropertyExists(nameof(Type)) && !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeUsageDirection() => PropertyExists(nameof(UsageDirection)) && !string.IsNullOrEmpty(UsageDirection);

        /// <summary>
        /// 检查Type属性在XML中是否存在（即使为空）
        /// </summary>
        public bool TypeExistsInXml => PropertyExists(nameof(Type));

        /// <summary>
        /// 检查UsageDirection属性在XML中是否存在（即使为空）
        /// </summary>
        public bool UsageDirectionExistsInXml => PropertyExists(nameof(UsageDirection));
    }
}