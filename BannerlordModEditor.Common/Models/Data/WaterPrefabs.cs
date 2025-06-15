using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("WaterPrefabs")]
    public class WaterPrefabsBase
    {
        [XmlElement("WaterPrefab")]
        public List<WaterPrefab> WaterPrefabList { get; set; } = new List<WaterPrefab>();
    }

    public class WaterPrefab
    {
        [XmlAttribute("PrefabName")]
        public string PrefabName { get; set; } = string.Empty;

        [XmlAttribute("MaterialName")]
        public string MaterialName { get; set; } = string.Empty;

        [XmlAttribute("Thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [XmlAttribute("IsGlobal")]
        public string IsGlobal { get; set; } = string.Empty;
    }
} 