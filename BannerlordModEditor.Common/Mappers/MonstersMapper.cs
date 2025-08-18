using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DTO.Game;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MonstersMapper
    {
        public static MonstersDTO ToDTO(MonstersDO source)
        {
            if (source == null) return null;

            return new MonstersDTO
            {
                MonsterList = source.MonsterList?
                    .Select(MonstersMapper.ToDTO)
                    .ToList() ?? new List<MonsterDTO>()
            };
        }

        public static MonstersDO ToDO(MonstersDTO source)
        {
            if (source == null) return null;

            return new MonstersDO
            {
                MonsterList = source.MonsterList?
                    .Select(MonstersMapper.ToDO)
                    .ToList() ?? new List<MonsterDO>()
            };
        }

        public static MonsterDTO ToDTO(MonsterDO source)
        {
            if (source == null) return null;

            return new MonsterDTO
            {
                // Basic Identity Properties
                Id = source.Id,
                BaseMonster = source.BaseMonster,
                
                // Animation and Usage Properties
                ActionSet = source.ActionSet,
                FemaleActionSet = source.FemaleActionSet,
                MonsterUsage = source.MonsterUsage,
                
                // Physical Properties
                Weight = source.Weight,
                HitPoints = source.HitPoints,
                AbsorbedDamageRatio = source.AbsorbedDamageRatio,
                
                // Movement Properties
                WalkingSpeedLimit = source.WalkingSpeedLimit,
                CrouchWalkingSpeedLimit = source.CrouchWalkingSpeedLimit,
                JumpAcceleration = source.JumpAcceleration,
                JumpSpeedLimit = source.JumpSpeedLimit,
                NumPaces = source.NumPaces,
                RelativeSpeedLimitForCharge = source.RelativeSpeedLimitForCharge,
                
                // Physical Dimensions
                StandingChestHeight = source.StandingChestHeight,
                StandingPelvisHeight = source.StandingPelvisHeight,
                StandingEyeHeight = source.StandingEyeHeight,
                CrouchEyeHeight = source.CrouchEyeHeight,
                MountedEyeHeight = source.MountedEyeHeight,
                RiderEyeHeightAdder = source.RiderEyeHeightAdder,
                RiderCameraHeightAdder = source.RiderCameraHeightAdder,
                RiderBodyCapsuleHeightAdder = source.RiderBodyCapsuleHeightAdder,
                RiderBodyCapsuleForwardAdder = source.RiderBodyCapsuleForwardAdder,
                
                // Offset and Position Properties
                EyeOffsetWrtHead = source.EyeOffsetWrtHead,
                FirstPersonCameraOffsetWrtHead = source.FirstPersonCameraOffsetWrtHead,
                
                // Arm Properties
                ArmLength = source.ArmLength,
                ArmWeight = source.ArmWeight,
                
                // Classification
                FamilyType = source.FamilyType,
                SoundAndCollisionInfoClass = source.SoundAndCollisionInfoClass,
                
                // Bone Definitions - Ragdoll Bones to Check for Corpses
                RagdollBoneToCheckForCorpses0 = source.RagdollBoneToCheckForCorpses0,
                RagdollBoneToCheckForCorpses1 = source.RagdollBoneToCheckForCorpses1,
                RagdollBoneToCheckForCorpses2 = source.RagdollBoneToCheckForCorpses2,
                RagdollBoneToCheckForCorpses3 = source.RagdollBoneToCheckForCorpses3,
                RagdollBoneToCheckForCorpses4 = source.RagdollBoneToCheckForCorpses4,
                RagdollBoneToCheckForCorpses5 = source.RagdollBoneToCheckForCorpses5,
                RagdollBoneToCheckForCorpses6 = source.RagdollBoneToCheckForCorpses6,
                RagdollBoneToCheckForCorpses7 = source.RagdollBoneToCheckForCorpses7,
                RagdollBoneToCheckForCorpses8 = source.RagdollBoneToCheckForCorpses8,
                RagdollBoneToCheckForCorpses9 = source.RagdollBoneToCheckForCorpses9,
                RagdollBoneToCheckForCorpses10 = source.RagdollBoneToCheckForCorpses10,
                
                // Ragdoll Fall Sound Bones
                RagdollFallSoundBone0 = source.RagdollFallSoundBone0,
                RagdollFallSoundBone1 = source.RagdollFallSoundBone1,
                RagdollFallSoundBone2 = source.RagdollFallSoundBone2,
                RagdollFallSoundBone3 = source.RagdollFallSoundBone3,
                
                // Direction and Movement Bones
                HeadLookDirectionBone = source.HeadLookDirectionBone,
                SpineLowerBone = source.SpineLowerBone,
                SpineUpperBone = source.SpineUpperBone,
                ThoraxLookDirectionBone = source.ThoraxLookDirectionBone,
                NeckRootBone = source.NeckRootBone,
                PelvisBone = source.PelvisBone,
                
                // Arm Bones
                RightUpperArmBone = source.RightUpperArmBone,
                LeftUpperArmBone = source.LeftUpperArmBone,
                
                // Damage and Effect Bones
                FallBlowDamageBone = source.FallBlowDamageBone,
                
                // Terrain Decal Bones
                TerrainDecalBone0 = source.TerrainDecalBone0,
                TerrainDecalBone1 = source.TerrainDecalBone1,
                
                // Ragdoll Stationary Check Bones
                RagdollStationaryCheckBone0 = source.RagdollStationaryCheckBone0,
                RagdollStationaryCheckBone1 = source.RagdollStationaryCheckBone1,
                RagdollStationaryCheckBone2 = source.RagdollStationaryCheckBone2,
                RagdollStationaryCheckBone3 = source.RagdollStationaryCheckBone3,
                RagdollStationaryCheckBone4 = source.RagdollStationaryCheckBone4,
                RagdollStationaryCheckBone5 = source.RagdollStationaryCheckBone5,
                RagdollStationaryCheckBone6 = source.RagdollStationaryCheckBone6,
                RagdollStationaryCheckBone7 = source.RagdollStationaryCheckBone7,
                
                // Move Adder Bones
                MoveAdderBone0 = source.MoveAdderBone0,
                MoveAdderBone1 = source.MoveAdderBone1,
                MoveAdderBone2 = source.MoveAdderBone2,
                MoveAdderBone3 = source.MoveAdderBone3,
                MoveAdderBone4 = source.MoveAdderBone4,
                MoveAdderBone5 = source.MoveAdderBone5,
                MoveAdderBone6 = source.MoveAdderBone6,
                
                // Splash Decal Bones
                SplashDecalBone0 = source.SplashDecalBone0,
                SplashDecalBone1 = source.SplashDecalBone1,
                SplashDecalBone2 = source.SplashDecalBone2,
                SplashDecalBone3 = source.SplashDecalBone3,
                SplashDecalBone4 = source.SplashDecalBone4,
                SplashDecalBone5 = source.SplashDecalBone5,
                
                // Blood Burst Bones
                BloodBurstBone0 = source.BloodBurstBone0,
                BloodBurstBone1 = source.BloodBurstBone1,
                BloodBurstBone2 = source.BloodBurstBone2,
                BloodBurstBone3 = source.BloodBurstBone3,
                BloodBurstBone4 = source.BloodBurstBone4,
                BloodBurstBone5 = source.BloodBurstBone5,
                BloodBurstBone6 = source.BloodBurstBone6,
                BloodBurstBone7 = source.BloodBurstBone7,
                
                // Hand and Item Bones
                MainHandBone = source.MainHandBone,
                OffHandBone = source.OffHandBone,
                MainHandItemBone = source.MainHandItemBone,
                OffHandItemBone = source.OffHandItemBone,
                OffHandItemSecondaryBone = source.OffHandItemSecondaryBone,
                OffHandShoulderBone = source.OffHandShoulderBone,
                HandNumBonesForIk = source.HandNumBonesForIk,
                
                // Foot and IK Bones
                PrimaryFootBone = source.PrimaryFootBone,
                SecondaryFootBone = source.SecondaryFootBone,
                RightFootIkEndEffectorBone = source.RightFootIkEndEffectorBone,
                LeftFootIkEndEffectorBone = source.LeftFootIkEndEffectorBone,
                RightFootIkTipBone = source.RightFootIkTipBone,
                LeftFootIkTipBone = source.LeftFootIkTipBone,
                FootNumBonesForIk = source.FootNumBonesForIk,
                
                // Mount-specific Properties
                ReinHandleLeftLocalPos = source.ReinHandleLeftLocalPos,
                ReinHandleRightLocalPos = source.ReinHandleRightLocalPos,
                ReinSkeleton = source.ReinSkeleton,
                ReinCollisionBody = source.ReinCollisionBody,
                
                // Ground Slope Detection
                FrontBoneToDetectGroundSlopeIndex = source.FrontBoneToDetectGroundSlopeIndex,
                BackBoneToDetectGroundSlopeIndex = source.BackBoneToDetectGroundSlopeIndex,
                
                // Bones to Modify on Sloping Ground
                BonesToModifyOnSlopingGround0 = source.BonesToModifyOnSlopingGround0,
                BonesToModifyOnSlopingGround1 = source.BonesToModifyOnSlopingGround1,
                BonesToModifyOnSlopingGround2 = source.BonesToModifyOnSlopingGround2,
                BonesToModifyOnSlopingGround3 = source.BonesToModifyOnSlopingGround3,
                BonesToModifyOnSlopingGround4 = source.BonesToModifyOnSlopingGround4,
                
                // Reference and Sitting Bones
                BodyRotationReferenceBone = source.BodyRotationReferenceBone,
                RiderSitBone = source.RiderSitBone,
                
                // Rein-related Bones
                ReinHandleBone = source.ReinHandleBone,
                ReinCollision1Bone = source.ReinCollision1Bone,
                ReinCollision2Bone = source.ReinCollision2Bone,
                ReinHeadBone = source.ReinHeadBone,
                ReinHeadRightAttachmentBone = source.ReinHeadRightAttachmentBone,
                ReinHeadLeftAttachmentBone = source.ReinHeadLeftAttachmentBone,
                ReinRightHandBone = source.ReinRightHandBone,
                ReinLeftHandBone = source.ReinLeftHandBone,
                
                // Complex objects
                Capsules = MonsterCapsulesMapper.ToDTO(source.Capsules),
                Flags = MonsterFlagsMapper.ToDTO(source.Flags)
            };
        }

        public static MonsterDO ToDO(MonsterDTO source)
        {
            if (source == null) return null;

            return new MonsterDO
            {
                // Basic Identity Properties
                Id = source.Id,
                BaseMonster = source.BaseMonster,
                
                // Animation and Usage Properties
                ActionSet = source.ActionSet,
                FemaleActionSet = source.FemaleActionSet,
                MonsterUsage = source.MonsterUsage,
                
                // Physical Properties
                Weight = source.Weight,
                HitPoints = source.HitPoints,
                AbsorbedDamageRatio = source.AbsorbedDamageRatio,
                
                // Movement Properties
                WalkingSpeedLimit = source.WalkingSpeedLimit,
                CrouchWalkingSpeedLimit = source.CrouchWalkingSpeedLimit,
                JumpAcceleration = source.JumpAcceleration,
                JumpSpeedLimit = source.JumpSpeedLimit,
                NumPaces = source.NumPaces,
                RelativeSpeedLimitForCharge = source.RelativeSpeedLimitForCharge,
                
                // Physical Dimensions
                StandingChestHeight = source.StandingChestHeight,
                StandingPelvisHeight = source.StandingPelvisHeight,
                StandingEyeHeight = source.StandingEyeHeight,
                CrouchEyeHeight = source.CrouchEyeHeight,
                MountedEyeHeight = source.MountedEyeHeight,
                RiderEyeHeightAdder = source.RiderEyeHeightAdder,
                RiderCameraHeightAdder = source.RiderCameraHeightAdder,
                RiderBodyCapsuleHeightAdder = source.RiderBodyCapsuleHeightAdder,
                RiderBodyCapsuleForwardAdder = source.RiderBodyCapsuleForwardAdder,
                
                // Offset and Position Properties
                EyeOffsetWrtHead = source.EyeOffsetWrtHead,
                FirstPersonCameraOffsetWrtHead = source.FirstPersonCameraOffsetWrtHead,
                
                // Arm Properties
                ArmLength = source.ArmLength,
                ArmWeight = source.ArmWeight,
                
                // Classification
                FamilyType = source.FamilyType,
                SoundAndCollisionInfoClass = source.SoundAndCollisionInfoClass,
                
                // Bone Definitions - Ragdoll Bones to Check for Corpses
                RagdollBoneToCheckForCorpses0 = source.RagdollBoneToCheckForCorpses0,
                RagdollBoneToCheckForCorpses1 = source.RagdollBoneToCheckForCorpses1,
                RagdollBoneToCheckForCorpses2 = source.RagdollBoneToCheckForCorpses2,
                RagdollBoneToCheckForCorpses3 = source.RagdollBoneToCheckForCorpses3,
                RagdollBoneToCheckForCorpses4 = source.RagdollBoneToCheckForCorpses4,
                RagdollBoneToCheckForCorpses5 = source.RagdollBoneToCheckForCorpses5,
                RagdollBoneToCheckForCorpses6 = source.RagdollBoneToCheckForCorpses6,
                RagdollBoneToCheckForCorpses7 = source.RagdollBoneToCheckForCorpses7,
                RagdollBoneToCheckForCorpses8 = source.RagdollBoneToCheckForCorpses8,
                RagdollBoneToCheckForCorpses9 = source.RagdollBoneToCheckForCorpses9,
                RagdollBoneToCheckForCorpses10 = source.RagdollBoneToCheckForCorpses10,
                
                // Ragdoll Fall Sound Bones
                RagdollFallSoundBone0 = source.RagdollFallSoundBone0,
                RagdollFallSoundBone1 = source.RagdollFallSoundBone1,
                RagdollFallSoundBone2 = source.RagdollFallSoundBone2,
                RagdollFallSoundBone3 = source.RagdollFallSoundBone3,
                
                // Direction and Movement Bones
                HeadLookDirectionBone = source.HeadLookDirectionBone,
                SpineLowerBone = source.SpineLowerBone,
                SpineUpperBone = source.SpineUpperBone,
                ThoraxLookDirectionBone = source.ThoraxLookDirectionBone,
                NeckRootBone = source.NeckRootBone,
                PelvisBone = source.PelvisBone,
                
                // Arm Bones
                RightUpperArmBone = source.RightUpperArmBone,
                LeftUpperArmBone = source.LeftUpperArmBone,
                
                // Damage and Effect Bones
                FallBlowDamageBone = source.FallBlowDamageBone,
                
                // Terrain Decal Bones
                TerrainDecalBone0 = source.TerrainDecalBone0,
                TerrainDecalBone1 = source.TerrainDecalBone1,
                
                // Ragdoll Stationary Check Bones
                RagdollStationaryCheckBone0 = source.RagdollStationaryCheckBone0,
                RagdollStationaryCheckBone1 = source.RagdollStationaryCheckBone1,
                RagdollStationaryCheckBone2 = source.RagdollStationaryCheckBone2,
                RagdollStationaryCheckBone3 = source.RagdollStationaryCheckBone3,
                RagdollStationaryCheckBone4 = source.RagdollStationaryCheckBone4,
                RagdollStationaryCheckBone5 = source.RagdollStationaryCheckBone5,
                RagdollStationaryCheckBone6 = source.RagdollStationaryCheckBone6,
                RagdollStationaryCheckBone7 = source.RagdollStationaryCheckBone7,
                
                // Move Adder Bones
                MoveAdderBone0 = source.MoveAdderBone0,
                MoveAdderBone1 = source.MoveAdderBone1,
                MoveAdderBone2 = source.MoveAdderBone2,
                MoveAdderBone3 = source.MoveAdderBone3,
                MoveAdderBone4 = source.MoveAdderBone4,
                MoveAdderBone5 = source.MoveAdderBone5,
                MoveAdderBone6 = source.MoveAdderBone6,
                
                // Splash Decal Bones
                SplashDecalBone0 = source.SplashDecalBone0,
                SplashDecalBone1 = source.SplashDecalBone1,
                SplashDecalBone2 = source.SplashDecalBone2,
                SplashDecalBone3 = source.SplashDecalBone3,
                SplashDecalBone4 = source.SplashDecalBone4,
                SplashDecalBone5 = source.SplashDecalBone5,
                
                // Blood Burst Bones
                BloodBurstBone0 = source.BloodBurstBone0,
                BloodBurstBone1 = source.BloodBurstBone1,
                BloodBurstBone2 = source.BloodBurstBone2,
                BloodBurstBone3 = source.BloodBurstBone3,
                BloodBurstBone4 = source.BloodBurstBone4,
                BloodBurstBone5 = source.BloodBurstBone5,
                BloodBurstBone6 = source.BloodBurstBone6,
                BloodBurstBone7 = source.BloodBurstBone7,
                
                // Hand and Item Bones
                MainHandBone = source.MainHandBone,
                OffHandBone = source.OffHandBone,
                MainHandItemBone = source.MainHandItemBone,
                OffHandItemBone = source.OffHandItemBone,
                OffHandItemSecondaryBone = source.OffHandItemSecondaryBone,
                OffHandShoulderBone = source.OffHandShoulderBone,
                HandNumBonesForIk = source.HandNumBonesForIk,
                
                // Foot and IK Bones
                PrimaryFootBone = source.PrimaryFootBone,
                SecondaryFootBone = source.SecondaryFootBone,
                RightFootIkEndEffectorBone = source.RightFootIkEndEffectorBone,
                LeftFootIkEndEffectorBone = source.LeftFootIkEndEffectorBone,
                RightFootIkTipBone = source.RightFootIkTipBone,
                LeftFootIkTipBone = source.LeftFootIkTipBone,
                FootNumBonesForIk = source.FootNumBonesForIk,
                
                // Mount-specific Properties
                ReinHandleLeftLocalPos = source.ReinHandleLeftLocalPos,
                ReinHandleRightLocalPos = source.ReinHandleRightLocalPos,
                ReinSkeleton = source.ReinSkeleton,
                ReinCollisionBody = source.ReinCollisionBody,
                
                // Ground Slope Detection
                FrontBoneToDetectGroundSlopeIndex = source.FrontBoneToDetectGroundSlopeIndex,
                BackBoneToDetectGroundSlopeIndex = source.BackBoneToDetectGroundSlopeIndex,
                
                // Bones to Modify on Sloping Ground
                BonesToModifyOnSlopingGround0 = source.BonesToModifyOnSlopingGround0,
                BonesToModifyOnSlopingGround1 = source.BonesToModifyOnSlopingGround1,
                BonesToModifyOnSlopingGround2 = source.BonesToModifyOnSlopingGround2,
                BonesToModifyOnSlopingGround3 = source.BonesToModifyOnSlopingGround3,
                BonesToModifyOnSlopingGround4 = source.BonesToModifyOnSlopingGround4,
                
                // Reference and Sitting Bones
                BodyRotationReferenceBone = source.BodyRotationReferenceBone,
                RiderSitBone = source.RiderSitBone,
                
                // Rein-related Bones
                ReinHandleBone = source.ReinHandleBone,
                ReinCollision1Bone = source.ReinCollision1Bone,
                ReinCollision2Bone = source.ReinCollision2Bone,
                ReinHeadBone = source.ReinHeadBone,
                ReinHeadRightAttachmentBone = source.ReinHeadRightAttachmentBone,
                ReinHeadLeftAttachmentBone = source.ReinHeadLeftAttachmentBone,
                ReinRightHandBone = source.ReinRightHandBone,
                ReinLeftHandBone = source.ReinLeftHandBone,
                
                // Complex objects
                Capsules = MonsterCapsulesMapper.ToDO(source.Capsules),
                Flags = MonsterFlagsMapper.ToDO(source.Flags)
            };
        }
    }

    public static class MonsterCapsulesMapper
    {
        public static MonsterCapsulesDTO ToDTO(MonsterCapsulesDO source)
        {
            if (source == null) return null;

            return new MonsterCapsulesDTO
            {
                BodyCapsule = ConvertMonsterCapsule(source.BodyCapsule),
                CrouchedBodyCapsule = ConvertMonsterCapsule(source.CrouchedBodyCapsule)
            };
        }

        public static MonsterCapsulesDO ToDO(MonsterCapsulesDTO source)
        {
            if (source == null) return null;

            return new MonsterCapsulesDO
            {
                BodyCapsule = ConvertMonsterCapsule(source.BodyCapsule),
                CrouchedBodyCapsule = ConvertMonsterCapsule(source.CrouchedBodyCapsule)
            };
        }

        public static MonsterCapsuleDTO ToDTO(MonsterCapsuleDO source)
        {
            if (source == null) return null;

            return new MonsterCapsuleDTO
            {
                Radius = source.Radius,
                Pos1 = source.Pos1,
                Pos2 = source.Pos2
            };
        }

        public static MonsterCapsuleDO ToDO(MonsterCapsuleDTO source)
        {
            if (source == null) return null;

            return new MonsterCapsuleDO
            {
                Radius = source.Radius,
                Pos1 = source.Pos1,
                Pos2 = source.Pos2
            };
        }

        private static MonsterCapsuleDTO ConvertMonsterCapsule(MonsterCapsuleDO source)
        {
            if (source == null) return null;
            return ToDTO(source);
        }

        private static MonsterCapsuleDO ConvertMonsterCapsule(MonsterCapsuleDTO source)
        {
            if (source == null) return null;
            return ToDO(source);
        }
    }

    public static class MonsterFlagsMapper
    {
        public static MonsterFlagsDTO ToDTO(MonsterFlagsDO source)
        {
            if (source == null) return null;

            return new MonsterFlagsDTO
            {
                // Combat Abilities
                CanAttack = source.CanAttack,
                CanDefend = source.CanDefend,
                CanKick = source.CanKick,
                CanBeCharged = source.CanBeCharged,
                CanCharge = source.CanCharge,
                
                // Movement Abilities
                CanClimbLadders = source.CanClimbLadders,
                CanSprint = source.CanSprint,
                CanCrouch = source.CanCrouch,
                CanRetreat = source.CanRetreat,
                CanRear = source.CanRear,
                CanWander = source.CanWander,
                
                // Group and Social Behavior
                CanBeInGroup = source.CanBeInGroup,
                MoveAsHerd = source.MoveAsHerd,
                MoveForwardOnly = source.MoveForwardOnly,
                
                // Classification and State
                IsHumanoid = source.IsHumanoid,
                Mountable = source.Mountable,
                CanRide = source.CanRide,
                CanWieldWeapon = source.CanWieldWeapon,
                
                // Behavioral Reactions
                RunsAwayWhenHit = source.RunsAwayWhenHit,
                CanGetScared = source.CanGetScared
            };
        }

        public static MonsterFlagsDO ToDO(MonsterFlagsDTO source)
        {
            if (source == null) return null;

            return new MonsterFlagsDO
            {
                // Combat Abilities
                CanAttack = source.CanAttack,
                CanDefend = source.CanDefend,
                CanKick = source.CanKick,
                CanBeCharged = source.CanBeCharged,
                CanCharge = source.CanCharge,
                
                // Movement Abilities
                CanClimbLadders = source.CanClimbLadders,
                CanSprint = source.CanSprint,
                CanCrouch = source.CanCrouch,
                CanRetreat = source.CanRetreat,
                CanRear = source.CanRear,
                CanWander = source.CanWander,
                
                // Group and Social Behavior
                CanBeInGroup = source.CanBeInGroup,
                MoveAsHerd = source.MoveAsHerd,
                MoveForwardOnly = source.MoveForwardOnly,
                
                // Classification and State
                IsHumanoid = source.IsHumanoid,
                Mountable = source.Mountable,
                CanRide = source.CanRide,
                CanWieldWeapon = source.CanWieldWeapon,
                
                // Behavioral Reactions
                RunsAwayWhenHit = source.RunsAwayWhenHit,
                CanGetScared = source.CanGetScared
            };
        }
    }
}