using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class GogAchievementDataMapper
    {
        public static GogAchievementDataDTO ToDTO(GogAchievementDataDO source)
        {
            if (source == null) return null;

            return new GogAchievementDataDTO
            {
                Achievements = source.Achievements?.Select(AchievementToDTO).ToList() ?? new List<AchievementDTO>()
            };
        }

        public static GogAchievementDataDO ToDO(GogAchievementDataDTO source)
        {
            if (source == null) return null;

            return new GogAchievementDataDO
            {
                Achievements = source.Achievements?.Select(AchievementToDo).ToList() ?? new List<AchievementDO>(),
                HasAchievements = source.Achievements != null && source.Achievements.Count > 0
            };
        }

        public static AchievementDTO AchievementToDTO(AchievementDO source)
        {
            if (source == null) return null;

            return new AchievementDTO
            {
                Name = source.Name,
                Requirements = AchievementRequirementsToDTO(source.Requirements)
            };
        }

        public static AchievementDO AchievementToDo(AchievementDTO source)
        {
            if (source == null) return null;

            return new AchievementDO
            {
                Name = source.Name,
                Requirements = AchievementRequirementsToDo(source.Requirements),
                HasRequirements = source.Requirements != null && source.Requirements.Requirements.Count > 0
            };
        }

        public static AchievementRequirementsDTO AchievementRequirementsToDTO(AchievementRequirementsDO source)
        {
            if (source == null) return null;

            return new AchievementRequirementsDTO
            {
                Requirements = source.Requirements?.Select(AchievementRequirementToDTO).ToList() ?? new List<AchievementRequirementDTO>()
            };
        }

        public static AchievementRequirementsDO AchievementRequirementsToDo(AchievementRequirementsDTO source)
        {
            if (source == null) return null;

            return new AchievementRequirementsDO
            {
                Requirements = source.Requirements?.Select(AchievementRequirementToDo).ToList() ?? new List<AchievementRequirementDO>()
            };
        }

        public static AchievementRequirementDTO AchievementRequirementToDTO(AchievementRequirementDO source)
        {
            if (source == null) return null;

            return new AchievementRequirementDTO
            {
                StatName = source.StatName,
                Threshold = source.Threshold
            };
        }

        public static AchievementRequirementDO AchievementRequirementToDo(AchievementRequirementDTO source)
        {
            if (source == null) return null;

            return new AchievementRequirementDO
            {
                StatName = source.StatName,
                Threshold = source.Threshold
            };
        }
    }
}