using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("WaterPrefabs")]
    public class WaterPrefabs
    {
        [XmlElement("WaterPrefab")]
        public WaterPrefab[]? WaterPrefab { get; set; }
    }

    public class WaterPrefab
    {
        [XmlAttribute("PrefabName")]
        public string? PrefabName { get; set; }

        [XmlAttribute("MaterialName")]
        public string? MaterialName { get; set; }

        [XmlAttribute("Thumbnail")]
        public string? Thumbnail { get; set; }

        [XmlAttribute("IsGlobal")]
        public string? IsGlobal { get; set; }
    }
} 