using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 水体预制体集合 - 领域对象
    /// </summary>
    [XmlRoot("WaterPrefabs")]
    public class WaterPrefabsDO
    {
        [XmlElement("WaterPrefab")]
        public List<WaterPrefabDO> WaterPrefabs { get; set; } = new List<WaterPrefabDO>();

        [XmlIgnore]
        public bool HasEmptyWaterPrefabs { get; set; } = false;

        public bool ShouldSerializeWaterPrefabs() => WaterPrefabs != null && WaterPrefabs.Count > 0 && !HasEmptyWaterPrefabs;
    }

    /// <summary>
    /// 水体预制体 - 领域对象
    /// </summary>
    public class WaterPrefabDO
    {
        [XmlAttribute("PrefabName")]
        public string? PrefabName { get; set; }

        [XmlAttribute("MaterialName")]
        public string? MaterialName { get; set; }

        [XmlAttribute("Thumbnail")]
        public string? Thumbnail { get; set; }

        [XmlAttribute("IsGlobal")]
        public string? IsGlobal { get; set; }

        // ShouldSerialize 方法
        public bool ShouldSerializePrefabName() => !string.IsNullOrEmpty(PrefabName);
        public bool ShouldSerializeMaterialName() => !string.IsNullOrEmpty(MaterialName);
        public bool ShouldSerializeThumbnail() => !string.IsNullOrEmpty(Thumbnail);
        public bool ShouldSerializeIsGlobal() => !string.IsNullOrEmpty(IsGlobal);
    }
}