using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class BannerIconsRoot
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

        [XmlElement("BannerColors")]
        public BannerColors BannerColors { get; set; }

        public bool ShouldSerializeBannerColors() => BannerColors != null;
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

        [XmlElement("Icon")]
        public List<Icon> Icons { get; set; }

        public bool ShouldSerializeBackgrounds() => Backgrounds != null && Backgrounds.Count > 0;
        public bool ShouldSerializeIcons() => Icons != null && Icons.Count > 0;
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

    public class BannerColors
    {
        [XmlElement("Color")]
        public List<ColorEntry> Colors { get; set; }
    }

    public class ColorEntry
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("hex")]
        public string Hex { get; set; }

        [XmlAttribute("player_can_choose_for_background")]
        public bool PlayerCanChooseForBackground { get; set; }

        [XmlAttribute("player_can_choose_for_sigil")]
        public bool PlayerCanChooseForSigil { get; set; }

        public bool ShouldSerializePlayerCanChooseForBackground() => PlayerCanChooseForBackground;
        public bool ShouldSerializePlayerCanChooseForSigil() => PlayerCanChooseForSigil;
    }
}