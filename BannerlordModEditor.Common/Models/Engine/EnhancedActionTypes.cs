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
       private bool _nameExists;
       private string? _type;
       private bool _typeExists;
       private string? _usageDirection;
       private bool _usageDirectionExists;

       [XmlAttribute("name")]
       public string? Name
       {
           get => _nameExists ? _name : null;
           set
           {
               _name = value;
               _nameExists = true;
               MarkPropertyExists(nameof(Name));
           }
       }

       [XmlAttribute("type")]
       public string? Type
       {
           get => _typeExists ? _type : null;
           set
           {
               _type = value;
               _typeExists = value != null; // Only mark as existing if not null
               if (value != null)
               {
                   MarkPropertyExists(nameof(Type));
               }
           }
       }

       [XmlAttribute("usage_direction")]
       public string? UsageDirection
       {
           get => _usageDirectionExists ? _usageDirection : null;
           set
           {
               _usageDirection = value;
               _usageDirectionExists = value != null; // Only mark as existing if not null
               if (value != null)
               {
                   MarkPropertyExists(nameof(UsageDirection));
               }
               }
       }

       public bool ShouldSerializeName() => _nameExists && !string.IsNullOrEmpty(_name);
       public bool ShouldSerializeType() => _typeExists && !string.IsNullOrEmpty(_type);
       public bool ShouldSerializeUsageDirection() => _usageDirectionExists && !string.IsNullOrEmpty(_usageDirection);

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