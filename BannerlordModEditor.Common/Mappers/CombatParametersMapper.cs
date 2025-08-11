using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class CombatParametersMapper
    {
        #region DO to DTO

        public static CombatParametersDTO ToDTO(CombatParametersDO source)
        {
            if (source == null) return null;

            return new CombatParametersDTO
            {
                Type = source.Type,
                Definitions = ToDTO(source.Definitions),
                CombatParametersList = source.CombatParametersList?.Select(ToDTO).ToList() ?? new List<BaseCombatParameterDTO>()
            };
        }

        public static DefinitionsDTO ToDTO(DefinitionsDO source)
        {
            if (source == null) return null;

            return new DefinitionsDTO
            {
                Defs = source.Defs?.Select(ToDTO).ToList() ?? new List<DefDTO>()
            };
        }

        public static DefDTO ToDTO(DefDO source)
        {
            if (source == null) return null;

            return new DefDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static BaseCombatParameterDTO ToDTO(BaseCombatParameterDO source)
        {
            if (source == null) return null;

            return new BaseCombatParameterDTO
            {
                Id = source.Id,
                CollisionCheckStartingPercent = source.CollisionCheckStartingPercent,
                CollisionDamageStartingPercent = source.CollisionDamageStartingPercent,
                CollisionCheckEndingPercent = source.CollisionCheckEndingPercent,
                VerticalRotLimitMultiplierUp = source.VerticalRotLimitMultiplierUp,
                VerticalRotLimitMultiplierDown = source.VerticalRotLimitMultiplierDown,
                LeftRiderRotLimit = source.LeftRiderRotLimit,
                LeftRiderMinRotLimit = source.LeftRiderMinRotLimit,
                RightRiderRotLimit = source.RightRiderRotLimit,
                RightRiderMinRotLimit = source.RightRiderMinRotLimit,
                RiderLookDownLimit = source.RiderLookDownLimit,
                LeftLadderRotLimit = source.LeftLadderRotLimit,
                RightLadderRotLimit = source.RightLadderRotLimit,
                WeaponOffset = source.WeaponOffset,
                CollisionRadius = source.CollisionRadius,
                AlternativeAttackCooldownPeriod = source.AlternativeAttackCooldownPeriod,
                HitBoneIndex = source.HitBoneIndex,
                ShoulderHitBoneIndex = source.ShoulderHitBoneIndex,
                LookSlopeBlendFactorUpLimit = source.LookSlopeBlendFactorUpLimit,
                LookSlopeBlendFactorDownLimit = source.LookSlopeBlendFactorDownLimit,
                LookSlopeBlendSpeedFactor = source.LookSlopeBlendSpeedFactor,
                CustomCollisionCapsule = ToDTO(source.CustomCollisionCapsule)
            };
        }

        public static CustomCollisionCapsuleDTO ToDTO(CustomCollisionCapsuleDO source)
        {
            if (source == null) return null;

            return new CustomCollisionCapsuleDTO
            {
                P1 = source.P1,
                P2 = source.P2,
                R = source.R
            };
        }

        #endregion

        #region DTO to DO

        public static CombatParametersDO ToDO(CombatParametersDTO source)
        {
            if (source == null) return null;

            return new CombatParametersDO
            {
                Type = source.Type,
                Definitions = ToDO(source.Definitions),
                CombatParametersList = source.CombatParametersList?.Select(ToDO).ToList() ?? new List<BaseCombatParameterDO>()
            };
        }

        public static DefinitionsDO ToDO(DefinitionsDTO source)
        {
            if (source == null) return null;

            return new DefinitionsDO
            {
                Defs = source.Defs?.Select(ToDO).ToList() ?? new List<DefDO>()
            };
        }

        public static DefDO ToDO(DefDTO source)
        {
            if (source == null) return null;

            return new DefDO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static BaseCombatParameterDO ToDO(BaseCombatParameterDTO source)
        {
            if (source == null) return null;

            return new BaseCombatParameterDO
            {
                Id = source.Id,
                CollisionCheckStartingPercent = source.CollisionCheckStartingPercent,
                CollisionDamageStartingPercent = source.CollisionDamageStartingPercent,
                CollisionCheckEndingPercent = source.CollisionCheckEndingPercent,
                VerticalRotLimitMultiplierUp = source.VerticalRotLimitMultiplierUp,
                VerticalRotLimitMultiplierDown = source.VerticalRotLimitMultiplierDown,
                LeftRiderRotLimit = source.LeftRiderRotLimit,
                LeftRiderMinRotLimit = source.LeftRiderMinRotLimit,
                RightRiderRotLimit = source.RightRiderRotLimit,
                RightRiderMinRotLimit = source.RightRiderMinRotLimit,
                RiderLookDownLimit = source.RiderLookDownLimit,
                LeftLadderRotLimit = source.LeftLadderRotLimit,
                RightLadderRotLimit = source.RightLadderRotLimit,
                WeaponOffset = source.WeaponOffset,
                CollisionRadius = source.CollisionRadius,
                AlternativeAttackCooldownPeriod = source.AlternativeAttackCooldownPeriod,
                HitBoneIndex = source.HitBoneIndex,
                ShoulderHitBoneIndex = source.ShoulderHitBoneIndex,
                LookSlopeBlendFactorUpLimit = source.LookSlopeBlendFactorUpLimit,
                LookSlopeBlendFactorDownLimit = source.LookSlopeBlendFactorDownLimit,
                LookSlopeBlendSpeedFactor = source.LookSlopeBlendSpeedFactor,
                CustomCollisionCapsule = ToDO(source.CustomCollisionCapsule)
            };
        }

        public static CustomCollisionCapsuleDO ToDO(CustomCollisionCapsuleDTO source)
        {
            if (source == null) return null;

            return new CustomCollisionCapsuleDO
            {
                P1 = source.P1,
                P2 = source.P2,
                R = source.R
            };
        }

        #endregion
    }
}