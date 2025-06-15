using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Map
{
    /// <summary>
    /// Root element for map_icons.xml - Contains map icon definitions
    /// </summary>
    [XmlRoot("base")]
    public class MapIconsBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "map_icon";

        [XmlElement("map_icons")]
        public MapIconsContainer MapIcons { get; set; } = new MapIconsContainer();
    }

    /// <summary>
    /// Container for all map icons
    /// </summary>
    public class MapIconsContainer
    {
        [XmlElement("map_icon")]
        public List<MapIcon> MapIcon { get; set; } = new List<MapIcon>();
    }

    /// <summary>
    /// Individual map icon definition with visual and audio properties
    /// </summary>
    public class MapIcon
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("id_str")]
        public string IdStr { get; set; } = string.Empty;

        [XmlAttribute("flags")]
        public string Flags { get; set; } = string.Empty;

        [XmlAttribute("mesh_name")]
        public string MeshName { get; set; } = string.Empty;

        [XmlAttribute("mesh_scale")]
        public string MeshScale { get; set; } = string.Empty;

        [XmlAttribute("sound_no")]
        public string SoundNo { get; set; } = string.Empty;

        [XmlAttribute("offset_pos")]
        public string OffsetPos { get; set; } = string.Empty;

        [XmlAttribute("dirt_name")]
        public string? DirtName { get; set; }
    }
} 