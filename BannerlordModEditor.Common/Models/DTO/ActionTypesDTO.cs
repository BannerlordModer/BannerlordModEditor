using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("action_types")]
    public class ActionTypesDTO
    {
        [XmlElement("action")]
        public List<ActionTypeDTO> Actions { get; set; } = new List<ActionTypeDTO>();

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;

        // 便捷属性
        public int ActionsCount => Actions?.Count ?? 0;
    }

    public class ActionTypeDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("usage_direction")]
        public string? UsageDirection { get; set; }

        [XmlAttribute("action_stage")]
        public string? ActionStage { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeType() => Type != null;
        public bool ShouldSerializeUsageDirection() => UsageDirection != null;
        public bool ShouldSerializeActionStage() => ActionStage != null;

        // 类型安全的便捷属性
        public bool HasName => Name != null;
        public bool HasType => Type != null;
        public bool HasUsageDirection => UsageDirection != null;
        public bool HasActionStage => ActionStage != null;
    }
}