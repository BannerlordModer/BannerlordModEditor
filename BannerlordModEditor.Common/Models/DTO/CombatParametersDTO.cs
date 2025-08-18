using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class CombatParametersDTO
    {
        public string? Type { get; set; }

        public DefinitionsDTO Definitions { get; set; } = new DefinitionsDTO();

        public List<BaseCombatParameterDTO> CombatParametersList { get; set; } = new List<BaseCombatParameterDTO>();
        
        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeType() => Type != null;
        public bool ShouldSerializeDefinitions() => Definitions != null;
        public bool ShouldSerializeCombatParametersList() => CombatParametersList != null && CombatParametersList.Count > 0;

        // 便捷属性
        public bool HasType => Type != null;
        public int CombatParametersCount => CombatParametersList?.Count ?? 0;
    }

    public class DefinitionsDTO
    {
        public List<DefDTO> Defs { get; set; } = new List<DefDTO>();
        
        // ShouldSerialize方法
        public bool ShouldSerializeDefs() => Defs != null && Defs.Count > 0;

        // 便捷属性
        public int DefsCount => Defs?.Count ?? 0;
    }

    public class DefDTO
    {
        public string? Name { get; set; }
        public string? Value { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeName() => Name != null;
        public bool ShouldSerializeValue() => Value != null;

        // 便捷属性
        public bool HasName => Name != null;
        public bool HasValue => Value != null;

        // 类型安全的便捷属性
        public float? ValueFloat => float.TryParse(Value, out float val) ? val : (float?)null;
        public int? ValueInt => int.TryParse(Value, out int val) ? val : (int?)null;

        // 设置方法
        public void SetValueFloat(float? value) => Value = value?.ToString();
        public void SetValueInt(int? value) => Value = value?.ToString();
    }

    public class BaseCombatParameterDTO
    {
        public string? Id { get; set; }
        public string? CollisionCheckStartingPercent { get; set; }
        public string? CollisionDamageStartingPercent { get; set; }
        public string? CollisionCheckEndingPercent { get; set; }
        public string? VerticalRotLimitMultiplierUp { get; set; }
        public string? VerticalRotLimitMultiplierDown { get; set; }
        public string? LeftRiderRotLimit { get; set; }
        public string? LeftRiderMinRotLimit { get; set; }
        public string? RightRiderRotLimit { get; set; }
        public string? RightRiderMinRotLimit { get; set; }
        public string? RiderLookDownLimit { get; set; }
        public string? LeftLadderRotLimit { get; set; }
        public string? RightLadderRotLimit { get; set; }
        public string? WeaponOffset { get; set; }
        public string? CollisionRadius { get; set; }
        public string? AlternativeAttackCooldownPeriod { get; set; }
        public string? HitBoneIndex { get; set; }
        public string? ShoulderHitBoneIndex { get; set; }
        public string? LookSlopeBlendFactorUpLimit { get; set; }
        public string? LookSlopeBlendFactorDownLimit { get; set; }
        public string? LookSlopeBlendSpeedFactor { get; set; }
        public CustomCollisionCapsuleDTO? CustomCollisionCapsule { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeId() => Id != null;
        public bool ShouldSerializeCollisionCheckStartingPercent() => CollisionCheckStartingPercent != null;
        public bool ShouldSerializeCollisionDamageStartingPercent() => CollisionDamageStartingPercent != null;
        public bool ShouldSerializeCollisionCheckEndingPercent() => CollisionCheckEndingPercent != null;
        public bool ShouldSerializeVerticalRotLimitMultiplierUp() => VerticalRotLimitMultiplierUp != null;
        public bool ShouldSerializeVerticalRotLimitMultiplierDown() => VerticalRotLimitMultiplierDown != null;
        public bool ShouldSerializeLeftRiderRotLimit() => LeftRiderRotLimit != null;
        public bool ShouldSerializeLeftRiderMinRotLimit() => LeftRiderMinRotLimit != null;
        public bool ShouldSerializeRightRiderRotLimit() => RightRiderRotLimit != null;
        public bool ShouldSerializeRightRiderMinRotLimit() => RightRiderMinRotLimit != null;
        public bool ShouldSerializeRiderLookDownLimit() => RiderLookDownLimit != null;
        public bool ShouldSerializeLeftLadderRotLimit() => LeftLadderRotLimit != null;
        public bool ShouldSerializeRightLadderRotLimit() => RightLadderRotLimit != null;
        public bool ShouldSerializeWeaponOffset() => WeaponOffset != null;
        public bool ShouldSerializeCollisionRadius() => CollisionRadius != null;
        public bool ShouldSerializeAlternativeAttackCooldownPeriod() => AlternativeAttackCooldownPeriod != null;
        public bool ShouldSerializeHitBoneIndex() => HitBoneIndex != null;
        public bool ShouldSerializeShoulderHitBoneIndex() => ShoulderHitBoneIndex != null;
        public bool ShouldSerializeLookSlopeBlendFactorUpLimit() => LookSlopeBlendFactorUpLimit != null;
        public bool ShouldSerializeLookSlopeBlendFactorDownLimit() => LookSlopeBlendFactorDownLimit != null;
        public bool ShouldSerializeLookSlopeBlendSpeedFactor() => LookSlopeBlendSpeedFactor != null;
        public bool ShouldSerializeCustomCollisionCapsule() => CustomCollisionCapsule != null;

        // 便捷属性
        public bool HasId => Id != null;
        public bool HasCollisionCheckStartingPercent => CollisionCheckStartingPercent != null;
        public bool HasCollisionDamageStartingPercent => CollisionDamageStartingPercent != null;
        public bool HasCollisionCheckEndingPercent => CollisionCheckEndingPercent != null;
        public bool HasVerticalRotLimitMultiplierUp => VerticalRotLimitMultiplierUp != null;
        public bool HasVerticalRotLimitMultiplierDown => VerticalRotLimitMultiplierDown != null;
        public bool HasLeftRiderRotLimit => LeftRiderRotLimit != null;
        public bool HasLeftRiderMinRotLimit => LeftRiderMinRotLimit != null;
        public bool HasRightRiderRotLimit => RightRiderRotLimit != null;
        public bool HasRightRiderMinRotLimit => RightRiderMinRotLimit != null;
        public bool HasRiderLookDownLimit => RiderLookDownLimit != null;
        public bool HasLeftLadderRotLimit => LeftLadderRotLimit != null;
        public bool HasRightLadderRotLimit => RightLadderRotLimit != null;
        public bool HasWeaponOffset => WeaponOffset != null;
        public bool HasCollisionRadius => CollisionRadius != null;
        public bool HasAlternativeAttackCooldownPeriod => AlternativeAttackCooldownPeriod != null;
        public bool HasHitBoneIndex => HitBoneIndex != null;
        public bool HasShoulderHitBoneIndex => ShoulderHitBoneIndex != null;
        public bool HasLookSlopeBlendFactorUpLimit => LookSlopeBlendFactorUpLimit != null;
        public bool HasLookSlopeBlendFactorDownLimit => LookSlopeBlendFactorDownLimit != null;
        public bool HasLookSlopeBlendSpeedFactor => LookSlopeBlendSpeedFactor != null;
        public bool HasCustomCollisionCapsule => CustomCollisionCapsule != null;

        // 类型安全的便捷属性
        public float? CollisionCheckStartingPercentFloat => float.TryParse(CollisionCheckStartingPercent, out float val) ? val : (float?)null;
        public float? CollisionDamageStartingPercentFloat => float.TryParse(CollisionDamageStartingPercent, out float val) ? val : (float?)null;
        public float? CollisionCheckEndingPercentFloat => float.TryParse(CollisionCheckEndingPercent, out float val) ? val : (float?)null;
        public float? VerticalRotLimitMultiplierUpFloat => float.TryParse(VerticalRotLimitMultiplierUp, out float val) ? val : (float?)null;
        public float? VerticalRotLimitMultiplierDownFloat => float.TryParse(VerticalRotLimitMultiplierDown, out float val) ? val : (float?)null;
        public float? LeftRiderRotLimitFloat => float.TryParse(LeftRiderRotLimit, out float val) ? val : (float?)null;
        public float? LeftRiderMinRotLimitFloat => float.TryParse(LeftRiderMinRotLimit, out float val) ? val : (float?)null;
        public float? RightRiderRotLimitFloat => float.TryParse(RightRiderRotLimit, out float val) ? val : (float?)null;
        public float? RightRiderMinRotLimitFloat => float.TryParse(RightRiderMinRotLimit, out float val) ? val : (float?)null;
        public float? RiderLookDownLimitFloat => float.TryParse(RiderLookDownLimit, out float val) ? val : (float?)null;
        public float? LeftLadderRotLimitFloat => float.TryParse(LeftLadderRotLimit, out float val) ? val : (float?)null;
        public float? RightLadderRotLimitFloat => float.TryParse(RightLadderRotLimit, out float val) ? val : (float?)null;
        public float? WeaponOffsetFloat => float.TryParse(WeaponOffset, out float val) ? val : (float?)null;
        public float? CollisionRadiusFloat => float.TryParse(CollisionRadius, out float val) ? val : (float?)null;
        public float? AlternativeAttackCooldownPeriodFloat => float.TryParse(AlternativeAttackCooldownPeriod, out float val) ? val : (float?)null;
        public int? HitBoneIndexInt => int.TryParse(HitBoneIndex, out int val) ? val : (int?)null;
        public int? ShoulderHitBoneIndexInt => int.TryParse(ShoulderHitBoneIndex, out int val) ? val : (int?)null;
        public float? LookSlopeBlendFactorUpLimitFloat => float.TryParse(LookSlopeBlendFactorUpLimit, out float val) ? val : (float?)null;
        public float? LookSlopeBlendFactorDownLimitFloat => float.TryParse(LookSlopeBlendFactorDownLimit, out float val) ? val : (float?)null;
        public float? LookSlopeBlendSpeedFactorFloat => float.TryParse(LookSlopeBlendSpeedFactor, out float val) ? val : (float?)null;

        // 设置方法
        public void SetCollisionCheckStartingPercentFloat(float? value) => CollisionCheckStartingPercent = value?.ToString();
        public void SetCollisionDamageStartingPercentFloat(float? value) => CollisionDamageStartingPercent = value?.ToString();
        public void SetCollisionCheckEndingPercentFloat(float? value) => CollisionCheckEndingPercent = value?.ToString();
        public void SetVerticalRotLimitMultiplierUpFloat(float? value) => VerticalRotLimitMultiplierUp = value?.ToString();
        public void SetVerticalRotLimitMultiplierDownFloat(float? value) => VerticalRotLimitMultiplierDown = value?.ToString();
        public void SetLeftRiderRotLimitFloat(float? value) => LeftRiderRotLimit = value?.ToString();
        public void SetLeftRiderMinRotLimitFloat(float? value) => LeftRiderMinRotLimit = value?.ToString();
        public void SetRightRiderRotLimitFloat(float? value) => RightRiderRotLimit = value?.ToString();
        public void SetRightRiderMinRotLimitFloat(float? value) => RightRiderMinRotLimit = value?.ToString();
        public void SetRiderLookDownLimitFloat(float? value) => RiderLookDownLimit = value?.ToString();
        public void SetLeftLadderRotLimitFloat(float? value) => LeftLadderRotLimit = value?.ToString();
        public void SetRightLadderRotLimitFloat(float? value) => RightLadderRotLimit = value?.ToString();
        public void SetWeaponOffsetFloat(float? value) => WeaponOffset = value?.ToString();
        public void SetCollisionRadiusFloat(float? value) => CollisionRadius = value?.ToString();
        public void SetAlternativeAttackCooldownPeriodFloat(float? value) => AlternativeAttackCooldownPeriod = value?.ToString();
        public void SetHitBoneIndexInt(int? value) => HitBoneIndex = value?.ToString();
        public void SetShoulderHitBoneIndexInt(int? value) => ShoulderHitBoneIndex = value?.ToString();
        public void SetLookSlopeBlendFactorUpLimitFloat(float? value) => LookSlopeBlendFactorUpLimit = value?.ToString();
        public void SetLookSlopeBlendFactorDownLimitFloat(float? value) => LookSlopeBlendFactorDownLimit = value?.ToString();
        public void SetLookSlopeBlendSpeedFactorFloat(float? value) => LookSlopeBlendSpeedFactor = value?.ToString();
    }

    public class CustomCollisionCapsuleDTO
    {
        public string? P1 { get; set; }
        public string? P2 { get; set; }
        public string? R { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeP1() => P1 != null;
        public bool ShouldSerializeP2() => P2 != null;
        public bool ShouldSerializeR() => R != null;

        // 便捷属性
        public bool HasP1 => P1 != null;
        public bool HasP2 => P2 != null;
        public bool HasR => R != null;

        // 类型安全的便捷属性
        public float? P1Float => float.TryParse(P1, out float val) ? val : (float?)null;
        public float? P2Float => float.TryParse(P2, out float val) ? val : (float?)null;
        public float? RFloat => float.TryParse(R, out float val) ? val : (float?)null;

        // 设置方法
        public void SetP1Float(float? value) => P1 = value?.ToString();
        public void SetP2Float(float? value) => P2 = value?.ToString();
        public void SetRFloat(float? value) => R = value?.ToString();
    }
}