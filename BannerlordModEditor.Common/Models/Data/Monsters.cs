using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    /// <summary>
    /// Represents the monsters.xml file structure containing game creature definitions
    /// </summary>
    [XmlRoot("Monsters")]
    public class Monsters
    {
        /// <summary>
        /// Collection of monster definitions
        /// </summary>
        [XmlElement("Monster")]
        public Monster[]? MonsterList { get; set; }
    }

    /// <summary>
    /// Represents a monster definition with all its attributes and properties
    /// </summary>
    public class Monster
    {
        // Basic Identity Properties
        [XmlAttribute("id")]
        [Required]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("base_monster")]
        public string? BaseMonster { get; set; }

        // Animation and Usage Properties
        [XmlAttribute("action_set")]
        public string? ActionSet { get; set; }

        [XmlAttribute("female_action_set")]
        public string? FemaleActionSet { get; set; }

        [XmlAttribute("monster_usage")]
        public string? MonsterUsage { get; set; }

        // Physical Properties
        [XmlAttribute("weight")]
        public double? Weight { get; set; }
        public bool ShouldSerializeWeight() => Weight.HasValue;

        [XmlAttribute("hit_points")]
        public int? HitPoints { get; set; }
        public bool ShouldSerializeHitPoints() => HitPoints.HasValue;

        [XmlAttribute("absorbed_damage_ratio")]
        public double? AbsorbedDamageRatio { get; set; }
        public bool ShouldSerializeAbsorbedDamageRatio() => AbsorbedDamageRatio.HasValue;

        // Movement Properties
        [XmlAttribute("walking_speed_limit")]
        public double? WalkingSpeedLimit { get; set; }
        public bool ShouldSerializeWalkingSpeedLimit() => WalkingSpeedLimit.HasValue;

        [XmlAttribute("crouch_walking_speed_limit")]
        public double? CrouchWalkingSpeedLimit { get; set; }
        public bool ShouldSerializeCrouchWalkingSpeedLimit() => CrouchWalkingSpeedLimit.HasValue;

        [XmlAttribute("jump_acceleration")]
        public double? JumpAcceleration { get; set; }
        public bool ShouldSerializeJumpAcceleration() => JumpAcceleration.HasValue;

        [XmlAttribute("jump_speed_limit")]
        public double? JumpSpeedLimit { get; set; }
        public bool ShouldSerializeJumpSpeedLimit() => JumpSpeedLimit.HasValue;

        [XmlAttribute("num_paces")]
        public int? NumPaces { get; set; }
        public bool ShouldSerializeNumPaces() => NumPaces.HasValue;

        [XmlAttribute("relative_speed_limit_for_charge")]
        public double? RelativeSpeedLimitForCharge { get; set; }
        public bool ShouldSerializeRelativeSpeedLimitForCharge() => RelativeSpeedLimitForCharge.HasValue;

        // Physical Dimensions
        [XmlAttribute("standing_chest_height")]
        public double? StandingChestHeight { get; set; }
        public bool ShouldSerializeStandingChestHeight() => StandingChestHeight.HasValue;

        [XmlAttribute("standing_pelvis_height")]
        public double? StandingPelvisHeight { get; set; }
        public bool ShouldSerializeStandingPelvisHeight() => StandingPelvisHeight.HasValue;

        [XmlAttribute("standing_eye_height")]
        public double? StandingEyeHeight { get; set; }
        public bool ShouldSerializeStandingEyeHeight() => StandingEyeHeight.HasValue;

        [XmlAttribute("crouch_eye_height")]
        public double? CrouchEyeHeight { get; set; }
        public bool ShouldSerializeCrouchEyeHeight() => CrouchEyeHeight.HasValue;

        [XmlAttribute("mounted_eye_height")]
        public double? MountedEyeHeight { get; set; }
        public bool ShouldSerializeMountedEyeHeight() => MountedEyeHeight.HasValue;

        [XmlAttribute("rider_eye_height_adder")]
        public double? RiderEyeHeightAdder { get; set; }
        public bool ShouldSerializeRiderEyeHeightAdder() => RiderEyeHeightAdder.HasValue;

        [XmlAttribute("rider_camera_height_adder")]
        public double? RiderCameraHeightAdder { get; set; }
        public bool ShouldSerializeRiderCameraHeightAdder() => RiderCameraHeightAdder.HasValue;

        [XmlAttribute("rider_body_capsule_height_adder")]
        public double? RiderBodyCapsuleHeightAdder { get; set; }
        public bool ShouldSerializeRiderBodyCapsuleHeightAdder() => RiderBodyCapsuleHeightAdder.HasValue;

        [XmlAttribute("rider_body_capsule_forward_adder")]
        public double? RiderBodyCapsuleForwardAdder { get; set; }
        public bool ShouldSerializeRiderBodyCapsuleForwardAdder() => RiderBodyCapsuleForwardAdder.HasValue;

        // Offset and Position Properties
        [XmlAttribute("eye_offset_wrt_head")]
        public string? EyeOffsetWrtHead { get; set; }

        [XmlAttribute("first_person_camera_offset_wrt_head")]
        public string? FirstPersonCameraOffsetWrtHead { get; set; }

        // Arm Properties
        [XmlAttribute("arm_length")]
        public string? ArmLength { get; set; }

        [XmlAttribute("arm_weight")]
        public string? ArmWeight { get; set; }

        // Classification
        [XmlAttribute("family_type")]
        public string? FamilyType { get; set; }

        [XmlAttribute("sound_and_collision_info_class")]
        public string? SoundAndCollisionInfoClass { get; set; }

        // Bone Definitions - Ragdoll Bones to Check for Corpses
        [XmlAttribute("ragdoll_bone_to_check_for_corpses_0")]
        public string? RagdollBoneToCheckForCorpses0 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_1")]
        public string? RagdollBoneToCheckForCorpses1 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_2")]
        public string? RagdollBoneToCheckForCorpses2 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_3")]
        public string? RagdollBoneToCheckForCorpses3 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_4")]
        public string? RagdollBoneToCheckForCorpses4 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_5")]
        public string? RagdollBoneToCheckForCorpses5 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_6")]
        public string? RagdollBoneToCheckForCorpses6 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_7")]
        public string? RagdollBoneToCheckForCorpses7 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_8")]
        public string? RagdollBoneToCheckForCorpses8 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_9")]
        public string? RagdollBoneToCheckForCorpses9 { get; set; }

        [XmlAttribute("ragdoll_bone_to_check_for_corpses_10")]
        public string? RagdollBoneToCheckForCorpses10 { get; set; }

        // Ragdoll Fall Sound Bones
        [XmlAttribute("ragdoll_fall_sound_bone_0")]
        public string? RagdollFallSoundBone0 { get; set; }

        [XmlAttribute("ragdoll_fall_sound_bone_1")]
        public string? RagdollFallSoundBone1 { get; set; }

        [XmlAttribute("ragdoll_fall_sound_bone_2")]
        public string? RagdollFallSoundBone2 { get; set; }

        [XmlAttribute("ragdoll_fall_sound_bone_3")]
        public string? RagdollFallSoundBone3 { get; set; }

        // Direction and Movement Bones
        [XmlAttribute("head_look_direction_bone")]
        public string? HeadLookDirectionBone { get; set; }

        [XmlAttribute("spine_lower_bone")]
        public string? SpineLowerBone { get; set; }

        [XmlAttribute("spine_upper_bone")]
        public string? SpineUpperBone { get; set; }

        [XmlAttribute("thorax_look_direction_bone")]
        public string? ThoraxLookDirectionBone { get; set; }

        [XmlAttribute("neck_root_bone")]
        public string? NeckRootBone { get; set; }

        [XmlAttribute("pelvis_bone")]
        public string? PelvisBone { get; set; }

        // Arm Bones
        [XmlAttribute("right_upper_arm_bone")]
        public string? RightUpperArmBone { get; set; }

        [XmlAttribute("left_upper_arm_bone")]
        public string? LeftUpperArmBone { get; set; }

        // Damage and Effect Bones
        [XmlAttribute("fall_blow_damage_bone")]
        public string? FallBlowDamageBone { get; set; }

        // Terrain Decal Bones
        [XmlAttribute("terrain_decal_bone_0")]
        public string? TerrainDecalBone0 { get; set; }

        [XmlAttribute("terrain_decal_bone_1")]
        public string? TerrainDecalBone1 { get; set; }

        // Ragdoll Stationary Check Bones
        [XmlAttribute("ragdoll_stationary_check_bone_0")]
        public string? RagdollStationaryCheckBone0 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_1")]
        public string? RagdollStationaryCheckBone1 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_2")]
        public string? RagdollStationaryCheckBone2 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_3")]
        public string? RagdollStationaryCheckBone3 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_4")]
        public string? RagdollStationaryCheckBone4 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_5")]
        public string? RagdollStationaryCheckBone5 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_6")]
        public string? RagdollStationaryCheckBone6 { get; set; }

        [XmlAttribute("ragdoll_stationary_check_bone_7")]
        public string? RagdollStationaryCheckBone7 { get; set; }

        // Move Adder Bones
        [XmlAttribute("move_adder_bone_0")]
        public string? MoveAdderBone0 { get; set; }

        [XmlAttribute("move_adder_bone_1")]
        public string? MoveAdderBone1 { get; set; }

        [XmlAttribute("move_adder_bone_2")]
        public string? MoveAdderBone2 { get; set; }

        [XmlAttribute("move_adder_bone_3")]
        public string? MoveAdderBone3 { get; set; }

        [XmlAttribute("move_adder_bone_4")]
        public string? MoveAdderBone4 { get; set; }

        [XmlAttribute("move_adder_bone_5")]
        public string? MoveAdderBone5 { get; set; }

        [XmlAttribute("move_adder_bone_6")]
        public string? MoveAdderBone6 { get; set; }

        // Splash Decal Bones
        [XmlAttribute("splash_decal_bone_0")]
        public string? SplashDecalBone0 { get; set; }

        [XmlAttribute("splash_decal_bone_1")]
        public string? SplashDecalBone1 { get; set; }

        [XmlAttribute("splash_decal_bone_2")]
        public string? SplashDecalBone2 { get; set; }

        [XmlAttribute("splash_decal_bone_3")]
        public string? SplashDecalBone3 { get; set; }

        [XmlAttribute("splash_decal_bone_4")]
        public string? SplashDecalBone4 { get; set; }

        [XmlAttribute("splash_decal_bone_5")]
        public string? SplashDecalBone5 { get; set; }

        // Blood Burst Bones
        [XmlAttribute("blood_burst_bone_0")]
        public string? BloodBurstBone0 { get; set; }

        [XmlAttribute("blood_burst_bone_1")]
        public string? BloodBurstBone1 { get; set; }

        [XmlAttribute("blood_burst_bone_2")]
        public string? BloodBurstBone2 { get; set; }

        [XmlAttribute("blood_burst_bone_3")]
        public string? BloodBurstBone3 { get; set; }

        [XmlAttribute("blood_burst_bone_4")]
        public string? BloodBurstBone4 { get; set; }

        [XmlAttribute("blood_burst_bone_5")]
        public string? BloodBurstBone5 { get; set; }

        [XmlAttribute("blood_burst_bone_6")]
        public string? BloodBurstBone6 { get; set; }

        [XmlAttribute("blood_burst_bone_7")]
        public string? BloodBurstBone7 { get; set; }

        // Hand and Item Bones
        [XmlAttribute("main_hand_bone")]
        public string? MainHandBone { get; set; }

        [XmlAttribute("off_hand_bone")]
        public string? OffHandBone { get; set; }

        [XmlAttribute("main_hand_item_bone")]
        public string? MainHandItemBone { get; set; }

        [XmlAttribute("off_hand_item_bone")]
        public string? OffHandItemBone { get; set; }

        [XmlAttribute("off_hand_item_secondary_bone")]
        public string? OffHandItemSecondaryBone { get; set; }

        [XmlAttribute("off_hand_shoulder_bone")]
        public string? OffHandShoulderBone { get; set; }

        [XmlAttribute("hand_num_bones_for_ik")]
        public int? HandNumBonesForIk { get; set; }
        public bool ShouldSerializeHandNumBonesForIk() => HandNumBonesForIk.HasValue;

        // Foot and IK Bones
        [XmlAttribute("primary_foot_bone")]
        public string? PrimaryFootBone { get; set; }

        [XmlAttribute("secondary_foot_bone")]
        public string? SecondaryFootBone { get; set; }

        [XmlAttribute("right_foot_ik_end_effector_bone")]
        public string? RightFootIkEndEffectorBone { get; set; }

        [XmlAttribute("left_foot_ik_end_effector_bone")]
        public string? LeftFootIkEndEffectorBone { get; set; }

        [XmlAttribute("right_foot_ik_tip_bone")]
        public string? RightFootIkTipBone { get; set; }

        [XmlAttribute("left_foot_ik_tip_bone")]
        public string? LeftFootIkTipBone { get; set; }

        [XmlAttribute("foot_num_bones_for_ik")]
        public int? FootNumBonesForIk { get; set; }
        public bool ShouldSerializeFootNumBonesForIk() => FootNumBonesForIk.HasValue;

        // Mount-specific Properties
        [XmlAttribute("rein_handle_left_local_pos")]
        public string? ReinHandleLeftLocalPos { get; set; }

        [XmlAttribute("rein_handle_right_local_pos")]
        public string? ReinHandleRightLocalPos { get; set; }

        [XmlAttribute("rein_skeleton")]
        public string? ReinSkeleton { get; set; }

        [XmlAttribute("rein_collision_body")]
        public string? ReinCollisionBody { get; set; }

        // Ground Slope Detection
        [XmlAttribute("front_bone_to_detect_ground_slope_index")]
        public string? FrontBoneToDetectGroundSlopeIndex { get; set; }

        [XmlAttribute("back_bone_to_detect_ground_slope_index")]
        public string? BackBoneToDetectGroundSlopeIndex { get; set; }

        // Bones to Modify on Sloping Ground
        [XmlAttribute("bones_to_modify_on_sloping_ground_0")]
        public string? BonesToModifyOnSlopingGround0 { get; set; }

        [XmlAttribute("bones_to_modify_on_sloping_ground_1")]
        public string? BonesToModifyOnSlopingGround1 { get; set; }

        [XmlAttribute("bones_to_modify_on_sloping_ground_2")]
        public string? BonesToModifyOnSlopingGround2 { get; set; }

        [XmlAttribute("bones_to_modify_on_sloping_ground_3")]
        public string? BonesToModifyOnSlopingGround3 { get; set; }

        [XmlAttribute("bones_to_modify_on_sloping_ground_4")]
        public string? BonesToModifyOnSlopingGround4 { get; set; }

        // Reference and Sitting Bones
        [XmlAttribute("body_rotation_reference_bone")]
        public string? BodyRotationReferenceBone { get; set; }

        [XmlAttribute("rider_sit_bone")]
        public string? RiderSitBone { get; set; }

        // Rein-related Bones
        [XmlAttribute("rein_handle_bone")]
        public string? ReinHandleBone { get; set; }

        [XmlAttribute("rein_collision_1_bone")]
        public string? ReinCollision1Bone { get; set; }

        [XmlAttribute("rein_collision_2_bone")]
        public string? ReinCollision2Bone { get; set; }

        [XmlAttribute("rein_head_bone")]
        public string? ReinHeadBone { get; set; }

        [XmlAttribute("rein_head_right_attachment_bone")]
        public string? ReinHeadRightAttachmentBone { get; set; }

        [XmlAttribute("rein_head_left_attachment_bone")]
        public string? ReinHeadLeftAttachmentBone { get; set; }

        [XmlAttribute("rein_right_hand_bone")]
        public string? ReinRightHandBone { get; set; }

        [XmlAttribute("rein_left_hand_bone")]
        public string? ReinLeftHandBone { get; set; }

        /// <summary>
        /// Collision capsules for the monster
        /// </summary>
        [XmlArray("Capsules")]
        [XmlArrayItem("body_capsule", typeof(MonsterCapsule))]
        [XmlArrayItem("crouched_body_capsule", typeof(MonsterCapsule))]
        public MonsterCapsule[]? Capsules { get; set; }

        /// <summary>
        /// Flags defining monster behavior and capabilities
        /// </summary>
        [XmlElement("Flags")]
        public MonsterFlags? Flags { get; set; }
    }

    /// <summary>
    /// Contains collision capsule definitions for monsters
    /// </summary>
    public class MonsterCapsules
    {
        /// <summary>
        /// Body capsule for normal standing state
        /// </summary>
        [XmlElement("body_capsule")]
        public MonsterCapsule? BodyCapsule { get; set; }

        /// <summary>
        /// Body capsule for crouching state
        /// </summary>
        [XmlElement("crouched_body_capsule")]
        public MonsterCapsule? CrouchedBodyCapsule { get; set; }
    }

    /// <summary>
    /// Represents a collision capsule with radius and position points
    /// </summary>
    public class MonsterCapsule
    {
        /// <summary>
        /// Radius of the collision capsule
        /// </summary>
        [XmlAttribute("radius")]
        public double? Radius { get; set; }
        public bool ShouldSerializeRadius() => Radius.HasValue;

        /// <summary>
        /// First position point (x, y, z coordinates)
        /// </summary>
        [XmlAttribute("pos1")]
        public string? Pos1 { get; set; }

        /// <summary>
        /// Second position point (x, y, z coordinates)
        /// </summary>
        [XmlAttribute("pos2")]
        public string? Pos2 { get; set; }
    }

    /// <summary>
    /// Contains boolean flags defining monster capabilities and behaviors
    /// </summary>
    public class MonsterFlags
    {
        // Combat Abilities
        [XmlAttribute("CanAttack")]
        public bool? CanAttack { get; set; }
        public bool ShouldSerializeCanAttack() => CanAttack.HasValue;

        [XmlAttribute("CanDefend")]
        public bool? CanDefend { get; set; }
        public bool ShouldSerializeCanDefend() => CanDefend.HasValue;

        [XmlAttribute("CanKick")]
        public bool? CanKick { get; set; }
        public bool ShouldSerializeCanKick() => CanKick.HasValue;

        [XmlAttribute("CanBeCharged")]
        public bool? CanBeCharged { get; set; }
        public bool ShouldSerializeCanBeCharged() => CanBeCharged.HasValue;

        [XmlAttribute("CanCharge")]
        public bool? CanCharge { get; set; }
        public bool ShouldSerializeCanCharge() => CanCharge.HasValue;

        // Movement Abilities
        [XmlAttribute("CanClimbLadders")]
        public bool? CanClimbLadders { get; set; }
        public bool ShouldSerializeCanClimbLadders() => CanClimbLadders.HasValue;

        [XmlAttribute("CanSprint")]
        public bool? CanSprint { get; set; }
        public bool ShouldSerializeCanSprint() => CanSprint.HasValue;

        [XmlAttribute("CanCrouch")]
        public bool? CanCrouch { get; set; }
        public bool ShouldSerializeCanCrouch() => CanCrouch.HasValue;

        [XmlAttribute("CanRetreat")]
        public bool? CanRetreat { get; set; }
        public bool ShouldSerializeCanRetreat() => CanRetreat.HasValue;

        [XmlAttribute("CanRear")]
        public bool? CanRear { get; set; }
        public bool ShouldSerializeCanRear() => CanRear.HasValue;

        [XmlAttribute("CanWander")]
        public bool? CanWander { get; set; }
        public bool ShouldSerializeCanWander() => CanWander.HasValue;

        // Group and Social Behavior
        [XmlAttribute("CanBeInGroup")]
        public bool? CanBeInGroup { get; set; }
        public bool ShouldSerializeCanBeInGroup() => CanBeInGroup.HasValue;

        [XmlAttribute("MoveAsHerd")]
        public bool? MoveAsHerd { get; set; }
        public bool ShouldSerializeMoveAsHerd() => MoveAsHerd.HasValue;

        [XmlAttribute("MoveForwardOnly")]
        public bool? MoveForwardOnly { get; set; }
        public bool ShouldSerializeMoveForwardOnly() => MoveForwardOnly.HasValue;

        // Classification and State
        [XmlAttribute("IsHumanoid")]
        public bool? IsHumanoid { get; set; }
        public bool ShouldSerializeIsHumanoid() => IsHumanoid.HasValue;

        [XmlAttribute("Mountable")]
        public bool? Mountable { get; set; }
        public bool ShouldSerializeMountable() => Mountable.HasValue;

        [XmlAttribute("CanRide")]
        public bool? CanRide { get; set; }
        public bool ShouldSerializeCanRide() => CanRide.HasValue;

        [XmlAttribute("CanWieldWeapon")]
        public bool? CanWieldWeapon { get; set; }
        public bool ShouldSerializeCanWieldWeapon() => CanWieldWeapon.HasValue;

        // Behavioral Reactions
        [XmlAttribute("RunsAwayWhenHit")]
        public bool? RunsAwayWhenHit { get; set; }
        public bool ShouldSerializeRunsAwayWhenHit() => RunsAwayWhenHit.HasValue;

        [XmlAttribute("CanGetScared")]
        public bool? CanGetScared { get; set; }
        public bool ShouldSerializeCanGetScared() => CanGetScared.HasValue;
    }
} 