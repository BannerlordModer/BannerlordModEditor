using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class ActionTypesDTO
    {
        public List<ActionTypeDTO> Actions { get; set; } = new List<ActionTypeDTO>();

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;

        // 便捷属性
        public int ActionsCount => Actions?.Count ?? 0;
    }

    public class ActionTypeDTO
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? UsageDirection { get; set; }
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