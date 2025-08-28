using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 水体预制体集合 - 数据传输对象
    /// </summary>
    [XmlRoot("WaterPrefabs")]
    public class WaterPrefabsDTO
    {
        [XmlElement("WaterPrefab")]
        public List<WaterPrefabDTO> WaterPrefabs { get; set; } = new List<WaterPrefabDTO>();

        [XmlIgnore]
        public bool HasEmptyWaterPrefabs { get; set; } = false;

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeWaterPrefabs() => WaterPrefabs != null && WaterPrefabs.Count > 0 && !HasEmptyWaterPrefabs;

        // 便捷属性
        public int WaterPrefabsCount => WaterPrefabs?.Count ?? 0;
    }

    /// <summary>
    /// 水体预制体 - 数据传输对象
    /// </summary>
    public class WaterPrefabDTO
    {
        [XmlAttribute("PrefabName")]
        public string? PrefabName { get; set; }

        [XmlAttribute("MaterialName")]
        public string? MaterialName { get; set; }

        [XmlAttribute("Thumbnail")]
        public string? Thumbnail { get; set; }

        [XmlAttribute("IsGlobal")]
        public string? IsGlobal { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializePrefabName() => PrefabName != null;
        public bool ShouldSerializeMaterialName() => MaterialName != null;
        public bool ShouldSerializeThumbnail() => Thumbnail != null;
        public bool ShouldSerializeIsGlobal() => IsGlobal != null;

        // 类型安全的便捷属性
        public bool HasPrefabName => PrefabName != null;
        public bool HasMaterialName => MaterialName != null;
        public bool HasThumbnail => Thumbnail != null;
        public bool HasIsGlobal => IsGlobal != null;
    }
}