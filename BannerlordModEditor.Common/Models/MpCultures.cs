using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("BasicCultures")]
    public class BasicCultures
    {
        [XmlElement("Culture")]
        public List<Culture> Culture { get; set; } = new List<Culture>();
    }

    public class Culture
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("is_main_culture")]
        public string? IsMainCulture { get; set; }

        [XmlAttribute("default_face_key")]
        public string DefaultFaceKey { get; set; } = string.Empty;

        [XmlAttribute("color")]
        public string? Color { get; set; }

        [XmlAttribute("color2")]
        public string? Color2 { get; set; }

        [XmlAttribute("cloth_alternative_color1")]
        public string? ClothAlternativeColor1 { get; set; }

        [XmlAttribute("cloth_alternative_color2")]
        public string? ClothAlternativeColor2 { get; set; }

        [XmlAttribute("banner_background_color1")]
        public string? BannerBackgroundColor1 { get; set; }

        [XmlAttribute("banner_foreground_color1")]
        public string? BannerForegroundColor1 { get; set; }

        [XmlAttribute("banner_background_color2")]
        public string? BannerBackgroundColor2 { get; set; }

        [XmlAttribute("banner_foreground_color2")]
        public string? BannerForegroundColor2 { get; set; }

        [XmlAttribute("faction_banner_key")]
        public string? FactionBannerKey { get; set; }
    }
} 