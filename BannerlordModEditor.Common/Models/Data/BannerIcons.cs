using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class BannerIconsBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("BannerIconData")]
        public BannerIconData BannerIconData { get; set; } = new BannerIconData();
    }

    public class BannerIconData
    {
        [XmlElement("BannerIconGroup")]
        public List<BannerIconGroup> BannerIconGroupList { get; set; } = new List<BannerIconGroup>();

        [XmlElement("BannerColors")]
        public BannerColors BannerColors { get; set; } = new BannerColors();
    }

    public class BannerIconGroup
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("is_pattern")]
        public string IsPattern { get; set; } = string.Empty;

        [XmlElement("Background")]
        public List<Background> BackgroundList { get; set; } = new List<Background>();

        [XmlElement("Icon")]
        public List<BannerIcon> IconList { get; set; } = new List<BannerIcon>();
    }

    public class Background
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("mesh_name")]
        public string MeshName { get; set; } = string.Empty;

        [XmlAttribute("is_base_background")]
        public string? IsBaseBackground { get; set; }
    }

    public class BannerIcon
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("material_name")]
        public string MaterialName { get; set; } = string.Empty;

        [XmlAttribute("texture_index")]
        public string TextureIndex { get; set; } = string.Empty;

        [XmlAttribute("is_reserved")]
        public string? IsReserved { get; set; }
    }

    public class BannerColors
    {
        [XmlElement("Color")]
        public List<BannerColor> ColorList { get; set; } = new List<BannerColor>();
    }

    public class BannerColor
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("hex")]
        public string Hex { get; set; } = string.Empty;

        [XmlAttribute("player_can_choose_for_background")]
        public string? PlayerCanChooseForBackground { get; set; }

        [XmlAttribute("player_can_choose_for_sigil")]
        public string? PlayerCanChooseForSigil { get; set; }
    }
} 