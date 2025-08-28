using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// 水体预制体映射器
    /// </summary>
    public static class WaterPrefabsMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static WaterPrefabsDTO ToDTO(WaterPrefabsDO source)
        {
            if (source == null) return null;
            
            return new WaterPrefabsDTO
            {
                WaterPrefabs = source.WaterPrefabs?
                    .Select(WaterPrefabMapper.ToDTO)
                    .ToList() ?? new List<WaterPrefabDTO>(),
                HasEmptyWaterPrefabs = source.HasEmptyWaterPrefabs
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static WaterPrefabsDO ToDO(WaterPrefabsDTO source)
        {
            if (source == null) return null;
            
            return new WaterPrefabsDO
            {
                WaterPrefabs = source.WaterPrefabs?
                    .Select(WaterPrefabMapper.ToDO)
                    .ToList() ?? new List<WaterPrefabDO>(),
                HasEmptyWaterPrefabs = source.HasEmptyWaterPrefabs
            };
        }
    }

    /// <summary>
    /// 水体预制体映射器
    /// </summary>
    public static class WaterPrefabMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static WaterPrefabDTO ToDTO(WaterPrefabDO source)
        {
            if (source == null) return null;
            
            return new WaterPrefabDTO
            {
                PrefabName = source.PrefabName,
                MaterialName = source.MaterialName,
                Thumbnail = source.Thumbnail,
                IsGlobal = source.IsGlobal
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static WaterPrefabDO ToDO(WaterPrefabDTO source)
        {
            if (source == null) return null;
            
            return new WaterPrefabDO
            {
                PrefabName = source.PrefabName,
                MaterialName = source.MaterialName,
                Thumbnail = source.Thumbnail,
                IsGlobal = source.IsGlobal
            };
        }
    }
}