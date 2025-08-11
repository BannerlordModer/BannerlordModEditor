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

        // ShouldSerialize方法
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeUsageDirection() => !string.IsNullOrEmpty(UsageDirection);

        // 类型安全的便捷属性
        public bool HasName => !string.IsNullOrEmpty(Name);
        public bool HasType => !string.IsNullOrEmpty(Type);
        public bool HasUsageDirection => !string.IsNullOrEmpty(UsageDirection);
    }
}