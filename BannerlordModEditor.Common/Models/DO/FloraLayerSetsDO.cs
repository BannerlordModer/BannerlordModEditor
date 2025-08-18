using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 植被层集合配置的领域对象
    /// 用于flora_layer_sets.xml文件的完整处理
    /// 包含地形植被分布和密度设置
    /// </summary>
    [XmlRoot("layer_flora_sets")]
    public class FloraLayerSetsDO
    {
        [XmlElement("layer_flora_set")]
        public List<FloraLayerSetDO> LayerFloraSets { get; set; } = new List<FloraLayerSetDO>();

        // 运行时标记
        [XmlIgnore]
        public bool HasEmptyLayerFloraSets { get; set; } = false;

        // 业务逻辑：按名称索引
        [XmlIgnore]
        public Dictionary<string, FloraLayerSetDO> SetsByName { get; set; } = new Dictionary<string, FloraLayerSetDO>();

        // 业务逻辑：按网格名称索引
        [XmlIgnore]
        public Dictionary<string, List<FloraLayerSetDO>> SetsByMeshName { get; set; } = new Dictionary<string, List<FloraLayerSetDO>>();

        // 业务逻辑：密度分布索引
        [XmlIgnore]
        public Dictionary<float, List<FloraLayerSetDO>> SetsByDensity { get; set; } = new Dictionary<float, List<FloraLayerSetDO>>();

        // 业务逻辑方法
        public void InitializeIndexes()
        {
            SetsByName.Clear();
            SetsByMeshName.Clear();
            SetsByDensity.Clear();

            foreach (var set in LayerFloraSets)
            {
                // 按名称索引
                if (!string.IsNullOrEmpty(set.Name))
                {
                    SetsByName[set.Name] = set;
                }

                // 按网格名称索引
                if (set.LayerFloras != null)
                {
                    foreach (var flora in set.LayerFloras)
                    {
                        if (flora.Mesh != null && !string.IsNullOrEmpty(flora.Mesh.Name))
                        {
                            if (!SetsByMeshName.ContainsKey(flora.Mesh.Name))
                            {
                                SetsByMeshName[flora.Mesh.Name] = new List<FloraLayerSetDO>();
                            }
                            SetsByMeshName[flora.Mesh.Name].Add(set);
                        }

                        // 按密度索引（分组）
                        if (flora.Mesh != null)
                        {
                            var densityGroup = (float)Math.Floor(flora.Mesh.Density * 10) / 10; // 按0.1分组
                            if (!SetsByDensity.ContainsKey(densityGroup))
                            {
                                SetsByDensity[densityGroup] = new List<FloraLayerSetDO>();
                            }
                            if (!SetsByDensity[densityGroup].Contains(set))
                            {
                                SetsByDensity[densityGroup].Add(set);
                            }
                        }
                    }
                }
            }
        }

        public FloraLayerSetDO? GetSetByName(string name)
        {
            return SetsByName.GetValueOrDefault(name);
        }

        public List<FloraLayerSetDO> GetSetsByMeshName(string meshName)
        {
            return SetsByMeshName.GetValueOrDefault(meshName, new List<FloraLayerSetDO>());
        }

        public List<FloraLayerSetDO> GetSetsByDensityRange(float minDensity, float maxDensity)
        {
            return SetsByDensity
                .Where(kv => kv.Key >= minDensity && kv.Key <= maxDensity)
                .SelectMany(kv => kv.Value)
                .Distinct()
                .ToList();
        }

        public List<FloraLayerSetDO> GetHighDensitySets(float threshold = 0.8f)
        {
            return GetSetsByDensityRange(threshold, 1.0f);
        }

        public List<FloraLayerSetDO> GetLowDensitySets(float threshold = 0.2f)
        {
            return GetSetsByDensityRange(0.0f, threshold);
        }

        // 验证方法
        public bool IsValid()
        {
            return LayerFloraSets.All(s => s.IsValid());
        }

        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (LayerFloraSets.Count == 0)
                errors.Add("No flora layer sets found");

            foreach (var set in LayerFloraSets)
            {
                var setErrors = set.GetValidationErrors();
                errors.AddRange(setErrors.Select(e => $"{set.Name}: {e}"));
            }

            return errors;
        }

        // 统计方法
        public int GetTotalSetCount()
        {
            return LayerFloraSets.Count;
        }

        public int GetTotalFloraCount()
        {
            return LayerFloraSets.Sum(s => s.LayerFloras?.Count ?? 0);
        }

        public Dictionary<string, int> GetMeshUsageCount()
        {
            var usage = new Dictionary<string, int>();
            
            foreach (var set in LayerFloraSets)
            {
                if (set.LayerFloras != null)
                {
                    foreach (var flora in set.LayerFloras)
                    {
                        if (flora.Mesh != null && !string.IsNullOrEmpty(flora.Mesh.Name))
                        {
                            usage[flora.Mesh.Name] = usage.GetValueOrDefault(flora.Mesh.Name) + 1;
                        }
                    }
                }
            }

            return usage;
        }

        public double GetAverageDensity()
        {
            var allDensities = LayerFloraSets
                .SelectMany(s => s.LayerFloras ?? new List<LayerFloraDO>())
                .Where(f => f.Mesh != null)
                .Select(f => f.Mesh.Density)
                .ToList();

            return allDensities.Count > 0 ? allDensities.Average() : 0;
        }

        public bool ShouldSerializeLayerFloraSets() => LayerFloraSets != null && LayerFloraSets.Count > 0;
    }

    /// <summary>
    /// 单个植被层集合
    /// </summary>
    public class FloraLayerSetDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("layer_flora")]
        public List<LayerFloraDO> LayerFloras { get; set; } = new List<LayerFloraDO>();

        // 运行时标记
        [XmlIgnore]
        public bool HasEmptyLayerFloras { get; set; } = false;

        // 业务逻辑方法
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name)) return false;
            
            if (LayerFloras == null || LayerFloras.Count == 0) return false;

            return LayerFloras.All(f => f.IsValid());
        }

        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Name))
                errors.Add("Flora layer set name is required");

            if (LayerFloras == null || LayerFloras.Count == 0)
                errors.Add("At least one layer flora is required");

            if (LayerFloras != null)
            {
                foreach (var flora in LayerFloras)
                {
                    var floraErrors = flora.GetValidationErrors();
                    errors.AddRange(floraErrors.Select(e => $"  {e}"));
                }
            }

            return errors;
        }

        public int GetTotalMeshCount()
        {
            return LayerFloras?.Count ?? 0;
        }

        public double GetAverageDensity()
        {
            var densities = LayerFloras?
                .Where(f => f.Mesh != null)
                .Select(f => f.Mesh.Density)
                .ToList() ?? new List<float>();

            return densities.Count > 0 ? densities.Average() : 0;
        }

        public double GetTotalColonyRadius()
        {
            return LayerFloras?
                .Where(f => f.Mesh != null)
                .Sum(f => f.Mesh.ColonyRadius) ?? 0;
        }

        public List<string> GetUniqueMeshNames()
        {
            return LayerFloras?
                .Where(f => f.Mesh != null && !string.IsNullOrEmpty(f.Mesh.Name))
                .Select(f => f.Mesh.Name)
                .Distinct()
                .ToList() ?? new List<string>();
        }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeLayerFloras() => LayerFloras != null && LayerFloras.Count > 0;
    }

    /// <summary>
    /// 单个植被层
    /// </summary>
    public class LayerFloraDO
    {
        [XmlElement("mesh")]
        public FloraLayerMeshDO? Mesh { get; set; }

        // 运行时标记
        [XmlIgnore]
        public bool HasMesh { get; set; } = false;

        // 业务逻辑方法
        public bool IsValid()
        {
            return Mesh != null && Mesh.IsValid();
        }

        public List<string> GetValidationErrors()
        {
            if (Mesh == null)
                return new List<string> { "Mesh is required" };

            return Mesh.GetValidationErrors().Select(e => $"Mesh: {e}").ToList();
        }

        public bool ShouldSerializeMesh() => HasMesh && Mesh != null;
    }

    /// <summary>
    /// 植被网格配置
    /// </summary>
    public class FloraLayerMeshDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("density")]
        public float Density { get; set; }

        [XmlAttribute("seed_index")]
        public int SeedIndex { get; set; }

        [XmlAttribute("colony_radius")]
        public float ColonyRadius { get; set; }

        [XmlAttribute("colony_threshold")]
        public float ColonyThreshold { get; set; }

        [XmlAttribute("size_min")]
        public string? SizeMin { get; set; }

        [XmlAttribute("size_max")]
        public string? SizeMax { get; set; }

        [XmlAttribute("albedo_multiplier")]
        public string? AlbedoMultiplier { get; set; }

        [XmlAttribute("weight_offset")]
        public float WeightOffset { get; set; }

        // 业务逻辑方法
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Name)) return false;
            if (Density < 0 || Density > 1) return false;
            if (Index < 0) return false;
            if (ColonyRadius < 0) return false;
            
            return true;
        }

        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Name))
                errors.Add("Mesh name is required");

            if (Density < 0 || Density > 1)
                errors.Add($"Density must be between 0 and 1, got {Density}");

            if (Index < 0)
                errors.Add($"Index must be non-negative, got {Index}");

            if (ColonyRadius < 0)
                errors.Add($"Colony radius must be non-negative, got {ColonyRadius}");

            return errors;
        }

        public bool IsHighDensity(float threshold = 0.7f)
        {
            return Density >= threshold;
        }

        public bool IsLowDensity(float threshold = 0.3f)
        {
            return Density <= threshold;
        }

        public double? GetSizeMinValue()
        {
            if (double.TryParse(SizeMin, out var value))
                return value;
            return null;
        }

        public double? GetSizeMaxValue()
        {
            if (double.TryParse(SizeMax, out var value))
                return value;
            return null;
        }

        public double? GetAlbedoMultiplierValue()
        {
            if (double.TryParse(AlbedoMultiplier, out var value))
                return value;
            return null;
        }

        public bool HasSizeRange()
        {
            return !string.IsNullOrEmpty(SizeMin) && !string.IsNullOrEmpty(SizeMax);
        }

        public bool IsColonyForming()
        {
            return ColonyRadius > 0 && ColonyThreshold > 0;
        }

        // 序列化控制方法
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIndex() => true;
        public bool ShouldSerializeDensity() => true;
        public bool ShouldSerializeSeedIndex() => true;
        public bool ShouldSerializeColonyRadius() => true;
        public bool ShouldSerializeColonyThreshold() => true;
        public bool ShouldSerializeSizeMin() => !string.IsNullOrEmpty(SizeMin);
        public bool ShouldSerializeSizeMax() => !string.IsNullOrEmpty(SizeMax);
        public bool ShouldSerializeAlbedoMultiplier() => !string.IsNullOrEmpty(AlbedoMultiplier);
        public bool ShouldSerializeWeightOffset() => true;
    }
}