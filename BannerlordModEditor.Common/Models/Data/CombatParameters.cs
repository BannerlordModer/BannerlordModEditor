using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class CombatParameters
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("definitions")]
        public Definitions Definitions { get; set; }

        [XmlArray("combat_parameters")]
        [XmlArrayItem("combat_parameter")]
        public List<BaseCombatParameter> CombatParametersList { get; set; }
        
        public bool ShouldSerializeDefinitions() => Definitions != null;
        public bool ShouldSerializeCombatParametersList() => CombatParametersList != null && CombatParametersList.Count > 0;
    }

    public class Definitions
    {
        [XmlElement("def")]
        public List<Def> Defs { get; set; }
        public bool ShouldSerializeDefs() => Defs != null && Defs.Count > 0;
    }

    public class Def
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("val")]
        public string Value { get; set; }
    }

    public class BaseCombatParameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("collision_check_starting_percent")]
        public string CollisionCheckStartingPercent { get; set; }
        public bool ShouldSerializeCollisionCheckStartingPercent() => CollisionCheckStartingPercent != null;

        [XmlAttribute("collision_damage_starting_percent")]
        public string CollisionDamageStartingPercent { get; set; }
        public bool ShouldSerializeCollisionDamageStartingPercent() => CollisionDamageStartingPercent != null;

        [XmlAttribute("collision_check_ending_percent")]
        public string CollisionCheckEndingPercent { get; set; }
        public bool ShouldSerializeCollisionCheckEndingPercent() => CollisionCheckEndingPercent != null;

        [XmlAttribute("vertical_rot_limit_multiplier_up")]
        public string VerticalRotLimitMultiplierUp { get; set; }
        public bool ShouldSerializeVerticalRotLimitMultiplierUp() => VerticalRotLimitMultiplierUp != null;

        [XmlAttribute("vertical_rot_limit_multiplier_down")]
        public string VerticalRotLimitMultiplierDown { get; set; }
        public bool ShouldSerializeVerticalRotLimitMultiplierDown() => VerticalRotLimitMultiplierDown != null;

        [XmlAttribute("left_rider_rot_limit")]
        public string LeftRiderRotLimit { get; set; }
        public bool ShouldSerializeLeftRiderRotLimit() => LeftRiderRotLimit != null;

        [XmlAttribute("left_rider_min_rot_limit")]
        public string LeftRiderMinRotLimit { get; set; }
        public bool ShouldSerializeLeftRiderMinRotLimit() => LeftRiderMinRotLimit != null;

        [XmlAttribute("right_rider_rot_limit")]
        public string RightRiderRotLimit { get; set; }
        public bool ShouldSerializeRightRiderRotLimit() => RightRiderRotLimit != null;

        [XmlAttribute("right_rider_min_rot_limit")]
        public string RightRiderMinRotLimit { get; set; }
        public bool ShouldSerializeRightRiderMinRotLimit() => RightRiderMinRotLimit != null;

        [XmlAttribute("rider_look_down_limit")]
        public string RiderLookDownLimit { get; set; }
        public bool ShouldSerializeRiderLookDownLimit() => RiderLookDownLimit != null;

        [XmlAttribute("left_ladder_rot_limit")]
        public string LeftLadderRotLimit { get; set; }
        public bool ShouldSerializeLeftLadderRotLimit() => LeftLadderRotLimit != null;

        [XmlAttribute("right_ladder_rot_limit")]
        public string RightLadderRotLimit { get; set; }
        public bool ShouldSerializeRightLadderRotLimit() => RightLadderRotLimit != null;

        [XmlAttribute("weapon_offset")]
        public string WeaponOffset { get; set; }
        public bool ShouldSerializeWeaponOffset() => WeaponOffset != null;

        [XmlAttribute("collision_radius")]
        public string CollisionRadius { get; set; }
        public bool ShouldSerializeCollisionRadius() => CollisionRadius != null;

        [XmlAttribute("alternative_attack_cooldown_period")]
        public string AlternativeAttackCooldownPeriod { get; set; }
        public bool ShouldSerializeAlternativeAttackCooldownPeriod() => AlternativeAttackCooldownPeriod != null;

        [XmlAttribute("hit_bone_index")]
        public string HitBoneIndex { get; set; }
        public bool ShouldSerializeHitBoneIndex() => HitBoneIndex != null;

        [XmlAttribute("shoulder_hit_bone_index")]
        public string ShoulderHitBoneIndex { get; set; }
        public bool ShouldSerializeShoulderHitBoneIndex() => ShoulderHitBoneIndex != null;

        [XmlAttribute("look_slope_blend_factor_up_limit")]
        public string LookSlopeBlendFactorUpLimit { get; set; }
        public bool ShouldSerializeLookSlopeBlendFactorUpLimit() => LookSlopeBlendFactorUpLimit != null;

        [XmlAttribute("look_slope_blend_factor_down_limit")]
        public string LookSlopeBlendFactorDownLimit { get; set; }
        public bool ShouldSerializeLookSlopeBlendFactorDownLimit() => LookSlopeBlendFactorDownLimit != null;

        [XmlAttribute("look_slope_blend_speed_factor")]
        public string LookSlopeBlendSpeedFactor { get; set; }
        public bool ShouldSerializeLookSlopeBlendSpeedFactor() => LookSlopeBlendSpeedFactor != null;

        [XmlElement("custom_collision_capsule")]
        public CustomCollisionCapsule CustomCollisionCapsule { get; set; }
        public bool ShouldSerializeCustomCollisionCapsule() => CustomCollisionCapsule != null;
    }

    public class CustomCollisionCapsule
    {
        [XmlAttribute("p1")]
        public string P1 { get; set; }

        [XmlAttribute("p2")]
        public string P2 { get; set; }

        [XmlAttribute("r")]
        public string R { get; set; }
    }
}