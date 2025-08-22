using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.Multiplayer;

namespace BannerlordModEditor.Common.Mappers
{
    public static class TauntUsageSetsMapper
    {
        public static TauntUsageSetsDTO ToDTO(TauntUsageSetsDO source)
        {
            if (source == null) return null;
            
            return new TauntUsageSetsDTO
            {
                TauntUsageSetList = source.TauntUsageSetList?
                    .Select(TauntUsageSetMapper.ToDTO)
                    .ToList() ?? new List<TauntUsageSetDTO>()
            };
        }

        public static TauntUsageSetsDO ToDO(TauntUsageSetsDTO source)
        {
            if (source == null) return null;
            
            return new TauntUsageSetsDO
            {
                TauntUsageSetList = source.TauntUsageSetList?
                    .Select(TauntUsageSetMapper.ToDO)
                    .ToList() ?? new List<TauntUsageSetDO>()
            };
        }
    }

    public static class TauntUsageSetMapper
    {
        public static TauntUsageSetDTO ToDTO(TauntUsageSetDO source)
        {
            if (source == null) return null;
            
            return new TauntUsageSetDTO
            {
                Id = source.Id,
                TauntUsages = source.TauntUsages?
                    .Select(TauntUsageMapper.ToDTO)
                    .ToList() ?? new List<TauntUsageDTO>()
            };
        }

        public static TauntUsageSetDO ToDO(TauntUsageSetDTO source)
        {
            if (source == null) return null;
            
            return new TauntUsageSetDO
            {
                Id = source.Id,
                TauntUsages = source.TauntUsages?
                    .Select(TauntUsageMapper.ToDO)
                    .ToList() ?? new List<TauntUsageDO>()
            };
        }
    }

    public static class TauntUsageMapper
    {
        public static TauntUsageDTO ToDTO(TauntUsageDO source)
        {
            if (source == null) return null;
            
            return new TauntUsageDTO
            {
                IsLeftStance = source.IsLeftStance,
                RequiresBow = source.RequiresBow,
                RequiresOnFoot = source.RequiresOnFoot,
                RequiresShield = source.RequiresShield,
                UnsuitableForShield = source.UnsuitableForShield,
                UnsuitableForBow = source.UnsuitableForBow,
                UnsuitableForCrossbow = source.UnsuitableForCrossbow,
                UnsuitableForTwoHanded = source.UnsuitableForTwoHanded,
                UnsuitableForEmpty = source.UnsuitableForEmpty,
                UnsuitableForOneHanded = source.UnsuitableForOneHanded,
                Action = source.Action
            };
        }

        public static TauntUsageDO ToDO(TauntUsageDTO source)
        {
            if (source == null) return null;
            
            return new TauntUsageDO
            {
                IsLeftStance = source.IsLeftStance,
                RequiresBow = source.RequiresBow,
                RequiresOnFoot = source.RequiresOnFoot,
                RequiresShield = source.RequiresShield,
                UnsuitableForShield = source.UnsuitableForShield,
                UnsuitableForBow = source.UnsuitableForBow,
                UnsuitableForCrossbow = source.UnsuitableForCrossbow,
                UnsuitableForTwoHanded = source.UnsuitableForTwoHanded,
                UnsuitableForEmpty = source.UnsuitableForEmpty,
                UnsuitableForOneHanded = source.UnsuitableForOneHanded,
                Action = source.Action
            };
        }
    }
}