using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class BoneBodyTypesDTO
    {
        public List<BoneBodyTypeDTO> Items { get; set; } = new List<BoneBodyTypeDTO>();

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;

        // 便捷属性
        public int ItemsCount => Items?.Count ?? 0;
    }

    public class BoneBodyTypeDTO
    {
        public string? Type { get; set; }
        public string? Priority { get; set; }
        public string? ActivateSweep { get; set; }
        public string? UseSmallerRadiusMultWhileHoldingShield { get; set; }
        public string? DoNotScaleAccordingToAgentScale { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePriority() => !string.IsNullOrEmpty(Priority);
        public bool ShouldSerializeActivateSweep() => !string.IsNullOrEmpty(ActivateSweep);
        public bool ShouldSerializeUseSmallerRadiusMultWhileHoldingShield() => !string.IsNullOrEmpty(UseSmallerRadiusMultWhileHoldingShield);
        public bool ShouldSerializeDoNotScaleAccordingToAgentScale() => !string.IsNullOrEmpty(DoNotScaleAccordingToAgentScale);

        // 类型安全的便捷属性
        public bool HasType => !string.IsNullOrEmpty(Type);
        public int? PriorityInt => int.TryParse(Priority, out int val) ? val : (int?)null;
        public bool? ActivateSweepBool => bool.TryParse(ActivateSweep, out bool val) ? val : (bool?)null;
        public bool? UseSmallerRadiusMultWhileHoldingShieldBool => bool.TryParse(UseSmallerRadiusMultWhileHoldingShield, out bool val) ? val : (bool?)null;
        public bool? DoNotScaleAccordingToAgentScaleBool => bool.TryParse(DoNotScaleAccordingToAgentScale, out bool val) ? val : (bool?)null;

        // 设置方法
        public void SetPriorityInt(int? value) => Priority = value?.ToString();
        public void SetActivateSweepBool(bool? value) => ActivateSweep = value?.ToString().ToLower();
        public void SetUseSmallerRadiusMultWhileHoldingShieldBool(bool? value) => UseSmallerRadiusMultWhileHoldingShield = value?.ToString().ToLower();
        public void SetDoNotScaleAccordingToAgentScaleBool(bool? value) => DoNotScaleAccordingToAgentScale = value?.ToString().ToLower();
    }
}