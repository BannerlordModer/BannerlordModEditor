using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class CombatParametersDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("definitions")]
        public DefinitionsDO Definitions { get; set; } = new DefinitionsDO();

        [XmlArray("combat_parameters")]
        [XmlArrayItem("combat_parameter")]
        public List<BaseCombatParameterDO> CombatParametersList { get; set; } = new List<BaseCombatParameterDO>();
        
        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeDefinitions() => Definitions != null;
        public bool ShouldSerializeCombatParametersList() => CombatParametersList != null && CombatParametersList.Count > 0;
    }

    public class DefinitionsDO
    {
        [XmlElement("def")]
        public List<DefDO> Defs { get; set; } = new List<DefDO>();
        public bool ShouldSerializeDefs() => Defs != null && Defs.Count > 0;
    }

    public class DefDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("val")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    public class BaseCombatParameterDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("collision_check_starting_percent")]
        public string? CollisionCheckStartingPercent { get; set; }

        [XmlAttribute("collision_damage_starting_percent")]
        public string? CollisionDamageStartingPercent { get; set; }

        [XmlAttribute("collision_check_ending_percent")]
        public string? CollisionCheckEndingPercent { get; set; }

        [XmlAttribute("vertical_rot_limit_multiplier_up")]
        public string? VerticalRotLimitMultiplierUp { get; set; }

        [XmlAttribute("vertical_rot_limit_multiplier_down")]
        public string? VerticalRotLimitMultiplierDown { get; set; }

        [XmlAttribute("left_rider_rot_limit")]
        public string? LeftRiderRotLimit { get; set; }

        [XmlAttribute("left_rider_min_rot_limit")]
        public string? LeftRiderMinRotLimit { get; set; }

        [XmlAttribute("right_rider_rot_limit")]
        public string? RightRiderRotLimit { get; set; }

        [XmlAttribute("right_rider_min_rot_limit")]
        public string? RightRiderMinRotLimit { get; set; }

        [XmlAttribute("rider_look_down_limit")]
        public string? RiderLookDownLimit { get; set; }

        [XmlAttribute("left_ladder_rot_limit")]
        public string? LeftLadderRotLimit { get; set; }

        [XmlAttribute("right_ladder_rot_limit")]
        public string? RightLadderRotLimit { get; set; }

        [XmlAttribute("weapon_offset")]
        public string? WeaponOffset { get; set; }

        [XmlAttribute("collision_radius")]
        public string? CollisionRadius { get; set; }

        [XmlAttribute("alternative_attack_cooldown_period")]
        public string? AlternativeAttackCooldownPeriod { get; set; }

        [XmlAttribute("hit_bone_index")]
        public string? HitBoneIndex { get; set; }

        [XmlAttribute("shoulder_hit_bone_index")]
        public string? ShoulderHitBoneIndex { get; set; }

        [XmlAttribute("look_slope_blend_factor_up_limit")]
        public string? LookSlopeBlendFactorUpLimit { get; set; }

        [XmlAttribute("look_slope_blend_factor_down_limit")]
        public string? LookSlopeBlendFactorDownLimit { get; set; }

        [XmlAttribute("look_slope_blend_speed_factor")]
        public string? LookSlopeBlendSpeedFactor { get; set; }

        [XmlElement("custom_collision_capsule")]
        public CustomCollisionCapsuleDO? CustomCollisionCapsule { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeCollisionCheckStartingPercent() => !string.IsNullOrEmpty(CollisionCheckStartingPercent);
        public bool ShouldSerializeCollisionDamageStartingPercent() => !string.IsNullOrEmpty(CollisionDamageStartingPercent);
        public bool ShouldSerializeCollisionCheckEndingPercent() => !string.IsNullOrEmpty(CollisionCheckEndingPercent);
        public bool ShouldSerializeVerticalRotLimitMultiplierUp() => !string.IsNullOrEmpty(VerticalRotLimitMultiplierUp);
        public bool ShouldSerializeVerticalRotLimitMultiplierDown() => !string.IsNullOrEmpty(VerticalRotLimitMultiplierDown);
        public bool ShouldSerializeLeftRiderRotLimit() => !string.IsNullOrEmpty(LeftRiderRotLimit);
        public bool ShouldSerializeLeftRiderMinRotLimit() => !string.IsNullOrEmpty(LeftRiderMinRotLimit);
        public bool ShouldSerializeRightRiderRotLimit() => !string.IsNullOrEmpty(RightRiderRotLimit);
        public bool ShouldSerializeRightRiderMinRotLimit() => !string.IsNullOrEmpty(RightRiderMinRotLimit);
        public bool ShouldSerializeRiderLookDownLimit() => !string.IsNullOrEmpty(RiderLookDownLimit);
        public bool ShouldSerializeLeftLadderRotLimit() => !string.IsNullOrEmpty(LeftLadderRotLimit);
        public bool ShouldSerializeRightLadderRotLimit() => !string.IsNullOrEmpty(RightLadderRotLimit);
        public bool ShouldSerializeWeaponOffset() => !string.IsNullOrEmpty(WeaponOffset);
        public bool ShouldSerializeCollisionRadius() => !string.IsNullOrEmpty(CollisionRadius);
        public bool ShouldSerializeAlternativeAttackCooldownPeriod() => !string.IsNullOrEmpty(AlternativeAttackCooldownPeriod);
        public bool ShouldSerializeHitBoneIndex() => !string.IsNullOrEmpty(HitBoneIndex);
        public bool ShouldSerializeShoulderHitBoneIndex() => !string.IsNullOrEmpty(ShoulderHitBoneIndex);
        public bool ShouldSerializeLookSlopeBlendFactorUpLimit() => !string.IsNullOrEmpty(LookSlopeBlendFactorUpLimit);
        public bool ShouldSerializeLookSlopeBlendFactorDownLimit() => !string.IsNullOrEmpty(LookSlopeBlendFactorDownLimit);
        public bool ShouldSerializeLookSlopeBlendSpeedFactor() => !string.IsNullOrEmpty(LookSlopeBlendSpeedFactor);
        public bool ShouldSerializeCustomCollisionCapsule() => CustomCollisionCapsule != null;
    }

    public class CustomCollisionCapsuleDO
    {
        [XmlAttribute("p1")]
        public string? P1 { get; set; }

        [XmlAttribute("p2")]
        public string? P2 { get; set; }

        [XmlAttribute("r")]
        public string? R { get; set; }

        public bool ShouldSerializeP1() => !string.IsNullOrEmpty(P1);
        public bool ShouldSerializeP2() => !string.IsNullOrEmpty(P2);
        public bool ShouldSerializeR() => !string.IsNullOrEmpty(R);
    }
}