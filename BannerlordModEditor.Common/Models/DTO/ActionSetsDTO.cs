using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class ActionSetsDTO
    {
        public List<ActionSetDTO> ActionSets { get; set; } = new List<ActionSetDTO>();

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeActionSets() => ActionSets != null && ActionSets.Count > 0;

        // 便捷属性
        public int ActionSetsCount => ActionSets?.Count ?? 0;
    }

    public class ActionSetDTO
    {
        public string? Id { get; set; }
        public string? Skeleton { get; set; }
        public string? MovementSystem { get; set; }
        public string? BaseSet { get; set; }
        public List<ActionDTO> Actions { get; set; } = new List<ActionDTO>();

        // ShouldSerialize方法
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeSkeleton() => !string.IsNullOrEmpty(Skeleton);
        public bool ShouldSerializeMovementSystem() => !string.IsNullOrEmpty(MovementSystem);
        public bool ShouldSerializeBaseSet() => !string.IsNullOrEmpty(BaseSet);
        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;

        // 类型安全的便捷属性
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasSkeleton => !string.IsNullOrEmpty(Skeleton);
        public bool HasMovementSystem => !string.IsNullOrEmpty(MovementSystem);
        public bool HasBaseSet => !string.IsNullOrEmpty(BaseSet);
        public bool HasActions => Actions != null && Actions.Count > 0;
        public int ActionsCount => Actions?.Count ?? 0;
    }

    public class ActionDTO
    {
        public string? Type { get; set; }
        public string? Animation { get; set; }
        public string? AlternativeGroup { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeAnimation() => !string.IsNullOrEmpty(Animation);
        public bool ShouldSerializeAlternativeGroup() => !string.IsNullOrEmpty(AlternativeGroup);

        // 类型安全的便捷属性
        public bool HasType => !string.IsNullOrEmpty(Type);
        public bool HasAnimation => !string.IsNullOrEmpty(Animation);
        public bool HasAlternativeGroup => !string.IsNullOrEmpty(AlternativeGroup);
    }
}