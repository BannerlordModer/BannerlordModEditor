using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class BannerIcons
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("BannerIconData")]
        public BannerIconData BannerIconData { get; set; }
    }

    public class BannerIconData
    {
        [XmlElement("BannerIconGroup")]
        public List<BannerIconGroup> BannerIconGroups { get; set; }
    }

    public class BannerIconGroup
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("is_pattern")]
        public bool IsPattern { get; set; }

        [XmlElement("Background")]
        public List<Background> Backgrounds { get; set; }

        public bool ShouldSerializeBackgrounds() => IsPattern && Backgrounds != null && Backgrounds.Count > 0;

        [XmlElement("Icon")]
        public List<Icon> Icons { get; set; }

        public bool ShouldSerializeIcons() => !IsPattern && Icons != null && Icons.Count > 0;
    }

    public class Background
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("mesh_name")]
        public string MeshName { get; set; }

        [XmlAttribute("is_base_background")]
        public bool IsBaseBackground { get; set; }

        public bool ShouldSerializeIsBaseBackground() => IsBaseBackground;
    }

    public class Icon
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("material_name")]
        public string MaterialName { get; set; }

        [XmlAttribute("texture_index")]
        public int TextureIndex { get; set; }

        [XmlAttribute("is_reserved")]
        public bool IsReserved { get; set; }

        public bool ShouldSerializeIsReserved() => IsReserved;
    }
}