using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Models.DTO.Multiplayer;

namespace BannerlordModEditor.Common.Mappers
{
    public static class BadgesMapper
    {
        public static BadgesDTO ToDTO(BadgesDO source)
        {
            if (source == null) return null;

            return new BadgesDTO
            {
                BadgeList = source.BadgeList?
                    .Select(BadgesMapper.ToDTO)
                    .ToList() ?? new List<BadgeDTO>()
            };
        }

        public static BadgesDO ToDO(BadgesDTO source)
        {
            if (source == null) return null;

            return new BadgesDO
            {
                BadgeList = source.BadgeList?
                    .Select(BadgesMapper.ToDO)
                    .ToList() ?? new List<BadgeDO>()
            };
        }

        public static BadgeDTO ToDTO(BadgeDO source)
        {
            if (source == null) return null;

            return new BadgeDTO
            {
                // Basic Identity Properties
                Id = source.Id,
                Type = source.Type,
                Name = source.Name,
                Description = source.Description,
                
                // Visibility Properties
                IsVisibleOnlyWhenEarned = source.IsVisibleOnlyWhenEarned,
                
                // Time Period Properties
                PeriodStart = source.PeriodStart,
                PeriodEnd = source.PeriodEnd,
                
                // Conditions
                Conditions = source.Conditions?
                    .Select(BadgesMapper.ToDTO)
                    .ToList() ?? new List<BadgeConditionDTO>()
            };
        }

        public static BadgeDO ToDO(BadgeDTO source)
        {
            if (source == null) return null;

            return new BadgeDO
            {
                // Basic Identity Properties
                Id = source.Id,
                Type = source.Type,
                Name = source.Name,
                Description = source.Description,
                
                // Visibility Properties
                IsVisibleOnlyWhenEarned = source.IsVisibleOnlyWhenEarned,
                
                // Time Period Properties
                PeriodStart = source.PeriodStart,
                PeriodEnd = source.PeriodEnd,
                
                // Conditions
                Conditions = source.Conditions?
                    .Select(BadgesMapper.ToDO)
                    .ToList() ?? new List<BadgeConditionDO>()
            };
        }

        public static BadgeConditionDTO ToDTO(BadgeConditionDO source)
        {
            if (source == null) return null;

            return new BadgeConditionDTO
            {
                Type = source.Type,
                GroupType = source.GroupType,
                Description = source.Description,
                Parameters = source.Parameters?
                    .Select(BadgesMapper.ToDTO)
                    .ToList() ?? new List<BadgeParameterDTO>()
            };
        }

        public static BadgeConditionDO ToDO(BadgeConditionDTO source)
        {
            if (source == null) return null;

            return new BadgeConditionDO
            {
                Type = source.Type,
                GroupType = source.GroupType,
                Description = source.Description,
                Parameters = source.Parameters?
                    .Select(BadgesMapper.ToDO)
                    .ToList() ?? new List<BadgeParameterDO>()
            };
        }

        public static BadgeParameterDTO ToDTO(BadgeParameterDO source)
        {
            if (source == null) return null;

            return new BadgeParameterDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static BadgeParameterDO ToDO(BadgeParameterDTO source)
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