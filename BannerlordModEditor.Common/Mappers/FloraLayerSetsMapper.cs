using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    /// <summary>
    /// FloraLayerSets的DO/DTO映射器
    /// 处理植被层集合配置的领域对象和数据传输对象之间的双向转换
    /// </summary>
    public static class FloraLayerSetsMapper
    {
        /// <summary>
        /// 将DO转换为DTO
        /// </summary>
        public static FloraLayerSetsDTO ToDTO(FloraLayerSetsDO source)
        {
            if (source == null) return null;

            return new FloraLayerSetsDTO
            {
                LayerFloraSets = source.LayerFloraSets?
                    .Select(FloraLayerSetMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<FloraLayerSetDTO>()
            };
        }

        /// <summary>
        /// 将DTO转换为DO
        /// </summary>
        public static FloraLayerSetsDO ToDO(FloraLayerSetsDTO source)
        {
            if (source == null) return null;

            var result = new FloraLayerSetsDO
            {
                LayerFloraSets = source.LayerFloraSets?
                    .Select(FloraLayerSetMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<FloraLayerSetDO>(),
                HasEmptyLayerFloraSets = source.LayerFloraSets == null || source.LayerFloraSets.Count == 0
            };

            // 初始化索引
            result.InitializeIndexes();

            return result;
        }

        /// <summary>
        /// 批量转换DO列表为DTO列表
        /// </summary>
        public static List<FloraLayerSetsDTO> ToDTO(List<FloraLayerSetsDO> source)
        {
            return source?.Select(ToDTO).Where(dto => dto != null).ToList() ?? new List<FloraLayerSetsDTO>();
        }

        /// <summary>
        /// 批量转换DTO列表为DO列表
        /// </summary>
        public static List<FloraLayerSetsDO> ToDO(List<FloraLayerSetsDTO> source)
        {
            return source?.Select(FloraLayerSetsMapper.ToDO).Where(dobj => dobj != null).ToList() ?? new List<FloraLayerSetsDO>();
        }

        /// <summary>
        /// 深度复制DO
        /// </summary>
        public static FloraLayerSetsDO DeepCopy(FloraLayerSetsDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        /// <summary>
        /// 验证DO对象数据完整性
        /// </summary>
        public static bool Validate(FloraLayerSetsDO source)
        {
            if (source == null) return false;
            return source.IsValid();
        }

        /// <summary>
        /// 获取验证错误信息
        /// </summary>
        public static List<string> GetValidationErrors(FloraLayerSetsDO source)
        {
            var errors = new List<string>();

            if (source == null)
            {
                errors.Add("FloraLayerSetsDO object is null");
                return errors;
            }

            if (source.LayerFloraSets == null || source.LayerFloraSets.Count == 0)
                errors.Add("At least one flora layer set is required");

            if (source.LayerFloraSets != null)
            {
                foreach (var set in source.LayerFloraSets)
                {
                    var setErrors = FloraLayerSetMapper.GetValidationErrors(set);
                    errors.AddRange(setErrors.Select(e => $"Set '{set.Name}': {e}"));
                }
            }

            return errors;
        }

        /// <summary>
        /// 合并两个DO对象（source的值覆盖target）
        /// </summary>
        public static FloraLayerSetsDO Merge(FloraLayerSetsDO target, FloraLayerSetsDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 合并集合（按名称合并）
            if (source.LayerFloraSets != null)
            {
                if (target.LayerFloraSets == null)
                {
                    target.LayerFloraSets = new List<FloraLayerSetDO>();
                }

                foreach (var sourceSet in source.LayerFloraSets)
                {
                    var targetSet = target.LayerFloraSets
                        .FirstOrDefault(s => s.Name == sourceSet.Name);

                    if (targetSet == null)
                    {
                        // 添加新集合
                        targetSet = FloraLayerSetMapper.DeepCopy(sourceSet);
                        target.LayerFloraSets.Add(targetSet);
                    }
                    else
                    {
                        // 合并现有集合
                        FloraLayerSetMapper.Merge(targetSet, sourceSet);
                    }
                }
            }

            // 更新运行时标记
            target.HasEmptyLayerFloraSets = target.LayerFloraSets.Count == 0;

            // 重新初始化索引
            target.InitializeIndexes();

            return target;
        }
    }

    /// <summary>
    /// FloraLayerSet的映射器
    /// </summary>
    internal static class FloraLayerSetMapper
    {
        public static FloraLayerSetDTO ToDTO(FloraLayerSetDO source)
        {
            if (source == null) return null;

            return new FloraLayerSetDTO
            {
                Name = source.Name,
                LayerFloras = source.LayerFloras?
                    .Select(LayerFloraMapper.ToDTO)
                    .Where(dto => dto != null)
                    .ToList() ?? new List<LayerFloraDTO>()
            };
        }

        public static FloraLayerSetDO ToDO(FloraLayerSetDTO source)
        {
            if (source == null) return null;

            return new FloraLayerSetDO
            {
                Name = source.Name,
                LayerFloras = source.LayerFloras?
                    .Select(LayerFloraMapper.ToDO)
                    .Where(dobj => dobj != null)
                    .ToList() ?? new List<LayerFloraDO>(),
                HasEmptyLayerFloras = source.LayerFloras == null || source.LayerFloras.Count == 0
            };
        }

        public static FloraLayerSetDO DeepCopy(FloraLayerSetDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }

        public static List<string> GetValidationErrors(FloraLayerSetDO source)
        {
            return source?.GetValidationErrors() ?? new List<string> { "FloraLayerSetDO object is null" };
        }

        public static FloraLayerSetDO Merge(FloraLayerSetDO target, FloraLayerSetDO source)
        {
            if (target == null) return source;
            if (source == null) return target;

            // 合并基本属性
            if (!string.IsNullOrEmpty(source.Name))
                target.Name = source.Name;

            // 合并植被列表（完全替换）
            if (source.LayerFloras != null)
            {
                target.LayerFloras = source.LayerFloras
                    .Select(LayerFloraMapper.DeepCopy)
                    .ToList();
            }

            // 更新运行时标记
            target.HasEmptyLayerFloras = target.LayerFloras.Count == 0;

            return target;
        }
    }

    /// <summary>
    /// LayerFlora的映射器
    /// </summary>
    internal static class LayerFloraMapper
    {
        public static LayerFloraDTO ToDTO(LayerFloraDO source)
        {
            if (source == null) return null;

            return new LayerFloraDTO
            {
                Mesh = FloraLayerMeshMapper.ToDTO(source.Mesh)
            };
        }

        public static LayerFloraDO ToDO(LayerFloraDTO source)
        {
            if (source == null) return null;

            return new LayerFloraDO
            {
                Mesh = FloraLayerMeshMapper.ToDO(source.Mesh),
                HasMesh = source.Mesh != null
            };
        }

        public static LayerFloraDO DeepCopy(LayerFloraDO source)
        {
            if (source == null) return null;

            var dto = ToDTO(source);
            return ToDO(dto);
        }
    }

    /// <summary>
    /// FloraLayerMesh的映射器
    /// </summary>
    internal static class FloraLayerMeshMapper
    {
        public static FloraLayerMeshDTO ToDTO(FloraLayerMeshDO source)
        {
            if (source == null) return null;

            return new FloraLayerMeshDTO
            {
                Name = source.Name,
                Index = source.Index,
                Density = source.Density,
                SeedIndex = source.SeedIndex,
                ColonyRadius = source.ColonyRadius,
                ColonyThreshold = source.ColonyThreshold,
                SizeMin = source.SizeMin,
                SizeMax = source.SizeMax,
                AlbedoMultiplier = source.AlbedoMultiplier,
                WeightOffset = source.WeightOffset
            };
        }

        public static FloraLayerMeshDO ToDO(FloraLayerMeshDTO source)
        {
            if (source == null) return null;

            return new FloraLayerMeshDO
            {
                Name = source.Name,
                Index = source.Index,
                Density = source.Density,
                SeedIndex = source.SeedIndex,
                ColonyRadius = source.ColonyRadius,
                ColonyThreshold = source.ColonyThreshold,
                SizeMin = source.SizeMin,
                SizeMax = source.SizeMax,
                AlbedoMultiplier = source.AlbedoMultiplier,
                WeightOffset = source.WeightOffset
            };
        }

        public static FloraLayerMeshDO DeepCopy(FloraLayerMeshDO source)
        {
            if (source == null) return null;

            return new FloraLayerMeshDO
            {
                Name = source.Name,
                Index = source.Index,
                Density = source.Density,
                SeedIndex = source.SeedIndex,
                ColonyRadius = source.ColonyRadius,
                ColonyThreshold = source.ColonyThreshold,
                SizeMin = source.SizeMin,
                SizeMax = source.SizeMax,
                AlbedoMultiplier = source.AlbedoMultiplier,
                WeightOffset = source.WeightOffset
            };
        }
    }
}