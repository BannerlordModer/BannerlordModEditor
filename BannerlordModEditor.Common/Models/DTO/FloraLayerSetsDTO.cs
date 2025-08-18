using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 植被层集合配置的数据传输对象
    /// 用于XML序列化/反序列化
    /// </summary>
    [XmlRoot("layer_flora_sets")]
    public class FloraLayerSetsDTO
    {
        [XmlElement("layer_flora_set")]
        public List<FloraLayerSetDTO> LayerFloraSets { get; set; } = new List<FloraLayerSetDTO>();
    }

    /// <summary>
    /// 单个植被层集合的DTO
    /// </summary>
    public class FloraLayerSetDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("layer_flora")]
        public List<LayerFloraDTO> LayerFloras { get; set; } = new List<LayerFloraDTO>();
    }

    /// <summary>
    /// 单个植被层的DTO
    /// </summary>
    public class LayerFloraDTO
    {
        [XmlElement("mesh")]
        public FloraLayerMeshDTO? Mesh { get; set; }
    }

    /// <summary>
    /// 植被网格配置的DTO
    /// </summary>
    public class FloraLayerMeshDTO
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
    }
}