using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 攻城器械映射器
    /// </summary>
    public static class SiegeEnginesMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static SiegeEnginesDTO ToDTO(SiegeEnginesDO source)
        {
            if (source == null) return null;
            
            return new SiegeEnginesDTO
            {
                SiegeEngines = source.SiegeEngines?
                    .Select(SiegeEngineTypeMapper.ToDTO)
                    .ToList() ?? new List<SiegeEngineTypeDTO>(),
                HasEmptySiegeEngines = source.HasEmptySiegeEngines
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static SiegeEnginesDO ToDO(SiegeEnginesDTO source)
        {
            if (source == null) return null;
            
            return new SiegeEnginesDO
            {
                SiegeEngines = source.SiegeEngines?
                    .Select(SiegeEngineTypeMapper.ToDO)
                    .ToList() ?? new List<SiegeEngineTypeDO>(),
                HasEmptySiegeEngines = source.HasEmptySiegeEngines
            };
        }
    }

    /// <summary>
    /// 攻城器械类型映射器
    /// </summary>
    public static class SiegeEngineTypeMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static SiegeEngineTypeDTO ToDTO(SiegeEngineTypeDO source)
        {
            if (source == null) return null;
            
            return new SiegeEngineTypeDTO
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                IsConstructible = source.IsConstructible,
                ManDayCost = source.ManDayCost,
                MaxHitPoints = source.MaxHitPoints,
                Difficulty = source.Difficulty,
                IsRanged = source.IsRanged,
                Damage = source.Damage,
                HitChance = source.HitChance,
                IsAntiPersonnel = source.IsAntiPersonnel,
                AntiPersonnelHitChance = source.AntiPersonnelHitChance,
                CampaignRateOfFirePerDay = source.CampaignRateOfFirePerDay
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static SiegeEngineTypeDO ToDO(SiegeEngineTypeDTO source)
        {
            if (source == null) return null;
            
            return new SiegeEngineTypeDO
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                IsConstructible = source.IsConstructible,
                ManDayCost = source.ManDayCost,
                MaxHitPoints = source.MaxHitPoints,
                Difficulty = source.Difficulty,
                IsRanged = source.IsRanged,
                Damage = source.Damage,
                HitChance = source.HitChance,
                IsAntiPersonnel = source.IsAntiPersonnel,
                AntiPersonnelHitChance = source.AntiPersonnelHitChance,
                CampaignRateOfFirePerDay = source.CampaignRateOfFirePerDay
            };
        }
    }
}