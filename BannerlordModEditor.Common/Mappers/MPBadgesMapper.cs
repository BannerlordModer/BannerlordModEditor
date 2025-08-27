using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MPBadgesMapper
    {
        public static MPBadgesDTO ToDTO(MPBadgesDO source)
        {
            if (source == null) return null;

            return new MPBadgesDTO
            {
                BadgeList = source.BadgeList?.Select(BadgeToDTO).ToList() ?? new List<BadgeDTO>()
            };
        }

        public static MPBadgesDO ToDO(MPBadgesDTO source)
        {
            if (source == null) return null;

            return new MPBadgesDO
            {
                BadgeList = source.BadgeList?.Select(BadgeToDo).ToList() ?? new List<BadgeDO>()
            };
        }

        public static BadgeDTO BadgeToDTO(BadgeDO source)
        {
            if (source == null) return null;

            return new BadgeDTO
            {
                Id = source.Id,
                Type = source.Type,
                GroupId = source.GroupId,
                Name = source.Name,
                Description = source.Description,
                IsVisibleOnlyWhenEarned = source.IsVisibleOnlyWhenEarned,
                PeriodStart = source.PeriodStart,
                PeriodEnd = source.PeriodEnd,
                Conditions = source.Conditions?.Select(BadgeConditionToDTO).ToList() ?? new List<BadgeConditionDTO>()
            };
        }

        public static BadgeDO BadgeToDo(BadgeDTO source)
        {
            if (source == null) return null;

            return new BadgeDO
            {
                Id = source.Id,
                Type = source.Type,
                GroupId = source.GroupId,
                Name = source.Name,
                Description = source.Description,
                IsVisibleOnlyWhenEarned = source.IsVisibleOnlyWhenEarned,
                PeriodStart = source.PeriodStart,
                PeriodEnd = source.PeriodEnd,
                Conditions = source.Conditions?.Select(BadgeConditionToDo).ToList() ?? new List<BadgeConditionDO>()
            };
        }

        public static BadgeConditionDTO BadgeConditionToDTO(BadgeConditionDO source)
        {
            if (source == null) return null;

            return new BadgeConditionDTO
            {
                Type = source.Type,
                GroupType = source.GroupType,
                Description = source.Description,
                Parameters = source.Parameters?.Select(BadgeParameterToDTO).ToList() ?? new List<BadgeParameterDTO>()
            };
        }

        public static BadgeConditionDO BadgeConditionToDo(BadgeConditionDTO source)
        {
            if (source == null) return null;

            return new BadgeConditionDO
            {
                Type = source.Type,
                GroupType = source.GroupType,
                Description = source.Description,
                Parameters = source.Parameters?.Select(BadgeParameterToDo).ToList() ?? new List<BadgeParameterDO>()
            };
        }

        public static BadgeParameterDTO BadgeParameterToDTO(BadgeParameterDO source)
        {
            if (source == null) return null;

            return new BadgeParameterDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static BadgeParameterDO BadgeParameterToDo(BadgeParameterDTO source)
        {
            if (source == null) return null;

            return new BadgeParameterDO
            {
                Name = source.Name,
                Value = source.Value
            };
        }
    }
}