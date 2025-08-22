using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class DecalSetsMapper
    {
        public static DecalSetsDTO ToDTO(DecalSetsDO source)
        {
            if (source == null) return null;

            return new DecalSetsDTO
            {
                Type = source.Type,
                DecalSets = DecalSetsContainerToDTO(source.DecalSets)
            };
        }

        public static DecalSetsDO ToDO(DecalSetsDTO source)
        {
            if (source == null) return null;

            return new DecalSetsDO
            {
                Type = source.Type,
                DecalSets = DecalSetsContainerToDo(source.DecalSets),
                HasDecalSets = source.DecalSets != null && source.DecalSets.Items.Count > 0
            };
        }

        public static DecalSetsContainerDTO DecalSetsContainerToDTO(DecalSetsContainerDO source)
        {
            if (source == null) return null;

            return new DecalSetsContainerDTO
            {
                Items = source.Items?.Select(DecalSetToDTO).ToList() ?? new List<DecalSetDTO>()
            };
        }

        public static DecalSetsContainerDO DecalSetsContainerToDo(DecalSetsContainerDTO source)
        {
            if (source == null) return null;

            return new DecalSetsContainerDO
            {
                Items = source.Items?.Select(DecalSetToDo).ToList() ?? new List<DecalSetDO>()
            };
        }

        public static DecalSetDTO DecalSetToDTO(DecalSetDO source)
        {
            if (source == null) return null;

            return new DecalSetDTO
            {
                Name = source.Name,
                TotalDecalLifeBase = source.TotalDecalLifeBase,
                VisibleDecalLifeBase = source.VisibleDecalLifeBase,
                MaximumDecalCountPerGrid = source.MaximumDecalCountPerGrid,
                MinVisibilityArea = source.MinVisibilityArea,
                AdaptiveTimeLimit = source.AdaptiveTimeLimit,
                FadeOutDelete = source.FadeOutDelete
            };
        }

        public static DecalSetDO DecalSetToDo(DecalSetDTO source)
        {
            if (source == null) return null;

            return new DecalSetDO
            {
                Name = source.Name,
                TotalDecalLifeBase = source.TotalDecalLifeBase,
                VisibleDecalLifeBase = source.VisibleDecalLifeBase,
                MaximumDecalCountPerGrid = source.MaximumDecalCountPerGrid,
                MinVisibilityArea = source.MinVisibilityArea,
                AdaptiveTimeLimit = source.AdaptiveTimeLimit,
                FadeOutDelete = source.FadeOutDelete
            };
        }
    }
}