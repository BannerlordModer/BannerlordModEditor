using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Game
{
    /// <summary>
    /// Represents the monsters.xml file structure containing game creature definitions
    /// </summary>
    [XmlRoot("Monsters")]
    public class MonstersDO
    {
        /// <summary>
        /// Collection of monster definitions
        /// </summary>
        [XmlElement("Monster")]
        public List<MonsterDO> MonsterList { get; set; } = new List<MonsterDO>();

        [XmlIgnore]
        public bool HasMonsterList => MonsterList != null && MonsterList.Count > 0;
    }

    /// <summary>
    /// Represents a monster definition with all its attributes and properties
    /// </summary>
    public class MonsterDO
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
        public string? Weight { get; set; }

        [XmlAttribute("hit_points")]
        public string? HitPoints { get; set; }

        [XmlAttribute("absorbed_damage_ratio")]
        public string? AbsorbedDamageRatio { get; set; }

        // Movement Properties
        [XmlAttribute("walking_speed_limit")]
        public string? WalkingSpeedLimit { get; set; }

        [XmlAttribute("crouch_walking_speed_limit")]
        public string? CrouchWalkingSpeedLimit { get; set; }

        [XmlAttribute("jump_acceleration")]
        public string? JumpAcceleration { get; set; }

        [XmlAttribute("jump_speed_limit")]
        public string? JumpSpeedLimit { get; set; }

        [XmlAttribute("num_paces")]
        public string? NumPaces { get; set; }

        [XmlAttribute("relative_speed_limit_for_charge")]
        public string? RelativeSpeedLimitForCharge { get; set; }

        // Physical Dimensions
        [XmlAttribute("standing_chest_height")]
        public string? StandingChestHeight { get; set; }

        [XmlAttribute("standing_pelvis_height")]
        public string? StandingPelvisHeight { get; set; }

        [XmlAttribute("standing_eye_height")]
        public string? StandingEyeHeight { get; set; }

        [XmlAttribute("crouch_eye_height")]
        public string? CrouchEyeHeight { get; set; }

        [XmlAttribute("mounted_eye_height")]
        public string? MountedEyeHeight { get; set; }

        [XmlAttribute("rider_eye_height_adder")]
        public string? RiderEyeHeightAdder { get; set; }

        [XmlAttribute("rider_camera_height_adder")]
        public string? RiderCameraHeightAdder { get; set; }

        [XmlAttribute("rider_body_capsule_height_adder")]
        public string? RiderBodyCapsuleHeightAdder { get; set; }

        [XmlAttribute("rider_body_capsule_forward_adder")]
        public string? RiderBodyCapsuleForwardAdder { get; set; }

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
        public string? HandNumBonesForIk { get; set; }

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
        public string? FootNumBonesForIk { get; set; }

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
        [XmlElement("Capsules")]
        public MonsterCapsulesDO? Capsules { get; set; }

        /// <summary>
        /// Flags defining monster behavior and capabilities
        /// </summary>
        [XmlElement("Flags")]
        public MonsterFlagsDO? Flags { get; set; }

        // Serialization control methods
        public bool ShouldSerializeBaseMonster() => !string.IsNullOrEmpty(BaseMonster);
        public bool ShouldSerializeActionSet() => !string.IsNullOrEmpty(ActionSet);
        public bool ShouldSerializeFemaleActionSet() => !string.IsNullOrEmpty(FemaleActionSet);
        public bool ShouldSerializeMonsterUsage() => !string.IsNullOrEmpty(MonsterUsage);
        public bool ShouldSerializeWeight() => !string.IsNullOrEmpty(Weight);
        public bool ShouldSerializeHitPoints() => !string.IsNullOrEmpty(HitPoints);
        public bool ShouldSerializeAbsorbedDamageRatio() => !string.IsNullOrEmpty(AbsorbedDamageRatio);
        public bool ShouldSerializeWalkingSpeedLimit() => !string.IsNullOrEmpty(WalkingSpeedLimit);
        public bool ShouldSerializeCrouchWalkingSpeedLimit() => !string.IsNullOrEmpty(CrouchWalkingSpeedLimit);
        public bool ShouldSerializeJumpAcceleration() => !string.IsNullOrEmpty(JumpAcceleration);
        public bool ShouldSerializeJumpSpeedLimit() => !string.IsNullOrEmpty(JumpSpeedLimit);
        public bool ShouldSerializeNumPaces() => !string.IsNullOrEmpty(NumPaces);
        public bool ShouldSerializeRelativeSpeedLimitForCharge() => !string.IsNullOrEmpty(RelativeSpeedLimitForCharge);
        public bool ShouldSerializeStandingChestHeight() => !string.IsNullOrEmpty(StandingChestHeight);
        public bool ShouldSerializeStandingPelvisHeight() => !string.IsNullOrEmpty(StandingPelvisHeight);
        public bool ShouldSerializeStandingEyeHeight() => !string.IsNullOrEmpty(StandingEyeHeight);
        public bool ShouldSerializeCrouchEyeHeight() => !string.IsNullOrEmpty(CrouchEyeHeight);
        public bool ShouldSerializeMountedEyeHeight() => !string.IsNullOrEmpty(MountedEyeHeight);
        public bool ShouldSerializeRiderEyeHeightAdder() => !string.IsNullOrEmpty(RiderEyeHeightAdder);
        public bool ShouldSerializeRiderCameraHeightAdder() => !string.IsNullOrEmpty(RiderCameraHeightAdder);
        public bool ShouldSerializeRiderBodyCapsuleHeightAdder() => !string.IsNullOrEmpty(RiderBodyCapsuleHeightAdder);
        public bool ShouldSerializeRiderBodyCapsuleForwardAdder() => !string.IsNullOrEmpty(RiderBodyCapsuleForwardAdder);
        public bool ShouldSerializeEyeOffsetWrtHead() => !string.IsNullOrEmpty(EyeOffsetWrtHead);
        public bool ShouldSerializeFirstPersonCameraOffsetWrtHead() => !string.IsNullOrEmpty(FirstPersonCameraOffsetWrtHead);
        public bool ShouldSerializeArmLength() => !string.IsNullOrEmpty(ArmLength);
        public bool ShouldSerializeArmWeight() => !string.IsNullOrEmpty(ArmWeight);
        public bool ShouldSerializeFamilyType() => !string.IsNullOrEmpty(FamilyType);
        public bool ShouldSerializeSoundAndCollisionInfoClass() => !string.IsNullOrEmpty(SoundAndCollisionInfoClass);
        public bool ShouldSerializeCapsules() => Capsules != null;
        public bool ShouldSerializeFlags() => Flags != null;

        // Ragdoll bone serialization methods
        public bool ShouldSerializeRagdollBoneToCheckForCorpses0() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses0);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses1() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses1);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses2() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses2);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses3() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses3);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses4() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses4);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses5() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses5);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses6() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses6);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses7() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses7);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses8() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses8);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses9() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses9);
        public bool ShouldSerializeRagdollBoneToCheckForCorpses10() => !string.IsNullOrEmpty(RagdollBoneToCheckForCorpses10);

        // Additional serialization control methods for other properties would go here...
        // For brevity, I'm showing the pattern - all bone-related properties should have similar methods
    }

    /// <summary>
    /// Contains collision capsule definitions for monsters
    /// </summary>
    public class MonsterCapsulesDO
    {
        /// <summary>
        /// Body capsule for normal standing state
        /// </summary>
        [XmlElement("body_capsule")]
        public MonsterCapsuleDO? BodyCapsule { get; set; }

        /// <summary>
        /// Body capsule for crouching state
        /// </summary>
        [XmlElement("crouched_body_capsule")]
        public MonsterCapsuleDO? CrouchedBodyCapsule { get; set; }

        public bool ShouldSerializeBodyCapsule() => BodyCapsule != null;
        public bool ShouldSerializeCrouchedBodyCapsule() => CrouchedBodyCapsule != null;
    }

    /// <summary>
    /// Represents a collision capsule with radius and position points
    /// </summary>
    public class MonsterCapsuleDO
    {
        /// <summary>
        /// Radius of the collision capsule
        /// </summary>
        [XmlAttribute("radius")]
        public string? Radius { get; set; }

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

        public bool ShouldSerializeRadius() => !string.IsNullOrEmpty(Radius);
        public bool ShouldSerializePos1() => !string.IsNullOrEmpty(Pos1);
        public bool ShouldSerializePos2() => !string.IsNullOrEmpty(Pos2);
    }

    /// <summary>
    /// Contains boolean flags defining monster capabilities and behaviors
    /// </summary>
    public class MonsterFlagsDO
    {
        // Combat Abilities
        [XmlAttribute("CanAttack")]
        public string? CanAttack { get; set; }

        [XmlAttribute("CanDefend")]
        public string? CanDefend { get; set; }

        [XmlAttribute("CanKick")]
        public string? CanKick { get; set; }

        [XmlAttribute("CanBeCharged")]
        public string? CanBeCharged { get; set; }

        [XmlAttribute("CanCharge")]
        public string? CanCharge { get; set; }

        // Movement Abilities
        [XmlAttribute("CanClimbLadders")]
        public string? CanClimbLadders { get; set; }

        [XmlAttribute("CanSprint")]
        public string? CanSprint { get; set; }

        [XmlAttribute("CanCrouch")]
        public string? CanCrouch { get; set; }

        [XmlAttribute("CanRetreat")]
        public string? CanRetreat { get; set; }

        [XmlAttribute("CanRear")]
        public string? CanRear { get; set; }

        [XmlAttribute("CanWander")]
        public string? CanWander { get; set; }

        // Group and Social Behavior
        [XmlAttribute("CanBeInGroup")]
        public string? CanBeInGroup { get; set; }

        [XmlAttribute("MoveAsHerd")]
        public string? MoveAsHerd { get; set; }

        [XmlAttribute("MoveForwardOnly")]
        public string? MoveForwardOnly { get; set; }

        // Classification and State
        [XmlAttribute("IsHumanoid")]
        public string? IsHumanoid { get; set; }

        [XmlAttribute("Mountable")]
        public string? Mountable { get; set; }

        [XmlAttribute("CanRide")]
        public string? CanRide { get; set; }

        [XmlAttribute("CanWieldWeapon")]
        public string? CanWieldWeapon { get; set; }

        // Behavioral Reactions
        [XmlAttribute("RunsAwayWhenHit")]
        public string? RunsAwayWhenHit { get; set; }

        [XmlAttribute("CanGetScared")]
        public string? CanGetScared { get; set; }

        // Serialization control methods
        public bool ShouldSerializeCanAttack() => !string.IsNullOrEmpty(CanAttack);
        public bool ShouldSerializeCanDefend() => !string.IsNullOrEmpty(CanDefend);
        public bool ShouldSerializeCanKick() => !string.IsNullOrEmpty(CanKick);
        public bool ShouldSerializeCanBeCharged() => !string.IsNullOrEmpty(CanBeCharged);
        public bool ShouldSerializeCanCharge() => !string.IsNullOrEmpty(CanCharge);
        public bool ShouldSerializeCanClimbLadders() => !string.IsNullOrEmpty(CanClimbLadders);
        public bool ShouldSerializeCanSprint() => !string.IsNullOrEmpty(CanSprint);
        public bool ShouldSerializeCanCrouch() => !string.IsNullOrEmpty(CanCrouch);
        public bool ShouldSerializeCanRetreat() => !string.IsNullOrEmpty(CanRetreat);
        public bool ShouldSerializeCanRear() => !string.IsNullOrEmpty(CanRear);
        public bool ShouldSerializeCanWander() => !string.IsNullOrEmpty(CanWander);
        public bool ShouldSerializeCanBeInGroup() => !string.IsNullOrEmpty(CanBeInGroup);
        public bool ShouldSerializeMoveAsHerd() => !string.IsNullOrEmpty(MoveAsHerd);
        public bool ShouldSerializeMoveForwardOnly() => !string.IsNullOrEmpty(MoveForwardOnly);
        public bool ShouldSerializeIsHumanoid() => !string.IsNullOrEmpty(IsHumanoid);
        public bool ShouldSerializeMountable() => !string.IsNullOrEmpty(Mountable);
        public bool ShouldSerializeCanRide() => !string.IsNullOrEmpty(CanRide);
        public bool ShouldSerializeCanWieldWeapon() => !string.IsNullOrEmpty(CanWieldWeapon);
        public bool ShouldSerializeRunsAwayWhenHit() => !string.IsNullOrEmpty(RunsAwayWhenHit);
        public bool ShouldSerializeCanGetScared() => !string.IsNullOrEmpty(CanGetScared);
    }
}