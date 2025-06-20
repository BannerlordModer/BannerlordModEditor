using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("base")]
    public class CombatParametersBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("definitions")]
        public Definitions Definitions { get; set; } = new Definitions();

        [XmlElement("combat_parameters")]
        public CombatParameters CombatParameters { get; set; } = new CombatParameters();
    }

    public class Definitions
    {
        [XmlElement("def")]
        public List<Definition> Defs { get; set; } = new List<Definition>();
    }

    public class CombatParameters
    {
        [XmlElement("combat_parameter")]
        public List<CombatParameter> CombatParameterList { get; set; } = new List<CombatParameter>();
    }

    public class Definition
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("val")]
        public string Value { get; set; } = string.Empty;
    }

    public class CombatParameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("vertical_rot_limit_multiplier_up")]
        public string VerticalRotLimitMultiplierUp { get; set; } = string.Empty;

        [XmlAttribute("vertical_rot_limit_multiplier_down")]
        public string VerticalRotLimitMultiplierDown { get; set; } = string.Empty;

        [XmlAttribute("collision_check_starting_percent")]
        public string CollisionCheckStartingPercent { get; set; } = string.Empty;

        [XmlAttribute("collision_check_ending_percent")]
        public string CollisionCheckEndingPercent { get; set; } = string.Empty;

        [XmlAttribute("hit_bone_index")]
        public string HitBoneIndex { get; set; } = string.Empty;

        [XmlAttribute("shoulder_hit_bone_index")]
        public string ShoulderHitBoneIndex { get; set; } = string.Empty;

        [XmlAttribute("weapon_offset")]
        public string WeaponOffset { get; set; } = string.Empty;

        [XmlAttribute("left_rider_rot_limit")]
        public string LeftRiderRotLimit { get; set; } = string.Empty;

        [XmlAttribute("left_rider_min_rot_limit")]
        public string LeftRiderMinRotLimit { get; set; } = string.Empty;

        [XmlAttribute("right_rider_rot_limit")]
        public string RightRiderRotLimit { get; set; } = string.Empty;

        [XmlAttribute("right_rider_min_rot_limit")]
        public string RightRiderMinRotLimit { get; set; } = string.Empty;

        [XmlAttribute("rider_look_down_limit")]
        public string RiderLookDownLimit { get; set; } = string.Empty;

        [XmlAttribute("left_ladder_rot_limit")]
        public string LeftLadderRotLimit { get; set; } = string.Empty;

        [XmlAttribute("right_ladder_rot_limit")]
        public string RightLadderRotLimit { get; set; } = string.Empty;

        [XmlAttribute("look_slope_blend_factor_up_limit")]
        public string LookSlopeBlendFactorUpLimit { get; set; } = string.Empty;

        [XmlAttribute("look_slope_blend_factor_down_limit")]
        public string LookSlopeBlendFactorDownLimit { get; set; } = string.Empty;

        [XmlAttribute("look_slope_blend_speed_factor")]
        public string LookSlopeBlendSpeedFactor { get; set; } = string.Empty;

        [XmlAttribute("alternative_attack_cooldown_period")]
        public string AlternativeAttackCooldownPeriod { get; set; } = string.Empty;

        [XmlAttribute("collision_radius")]
        public string CollisionRadius { get; set; } = string.Empty;

        [XmlElement("custom_collision_capsule")]
        public CustomCollisionCapsule? CustomCollisionCapsule { get; set; }
    }

    public class CustomCollisionCapsule
    {
        [XmlAttribute("p1")]
        public string P1 { get; set; } = string.Empty;

        [XmlAttribute("p2")]
        public string P2 { get; set; } = string.Empty;

        [XmlAttribute("r")]
        public string R { get; set; } = string.Empty;
    }
} 