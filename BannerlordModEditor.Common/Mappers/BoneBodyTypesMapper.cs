using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class BoneBodyTypesMapper
    {
        #region DO to DTO

        public static BoneBodyTypesDTO ToDTO(BoneBodyTypesDO source)
        {
            if (source == null) return null;

            return new BoneBodyTypesDTO
            {
                Items = source.Items?.Select(ToDTO).ToList() ?? new List<BoneBodyTypeDTO>()
            };
        }

        public static BoneBodyTypeDTO ToDTO(BoneBodyTypeDO source)
        {
            if (source == null) return null;

            return new BoneBodyTypeDTO
            {
                Type = source.Type,
                Priority = source.Priority,
                ActivateSweep = source.ActivateSweep,
                UseSmallerRadiusMultWhileHoldingShield = source.UseSmallerRadiusMultWhileHoldingShield,
                DoNotScaleAccordingToAgentScale = source.DoNotScaleAccordingToAgentScale
            };
        }

        #endregion

        #region DTO to DO

        public static BoneBodyTypesDO ToDO(BoneBodyTypesDTO source)
        {
            if (source == null) return null;

            return new BoneBodyTypesDO
            {
                Items = source.Items?.Select(ToDO).ToList() ?? new List<BoneBodyTypeDO>()
            };
        }

        public static BoneBodyTypeDO ToDO(BoneBodyTypeDTO source)
        {
            if (source == null) return null;

            return new BoneBodyTypeDO
            {
                Type = source.Type,
                Priority = source.Priority,
                ActivateSweep = source.ActivateSweep,
                UseSmallerRadiusMultWhileHoldingShield = source.UseSmallerRadiusMultWhileHoldingShield,
                DoNotScaleAccordingToAgentScale = source.DoNotScaleAccordingToAgentScale
            };
        }

        #endregion
    }
}