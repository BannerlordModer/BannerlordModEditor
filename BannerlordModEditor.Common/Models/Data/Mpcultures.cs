using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("BasicCultures")]
    public class Mpcultures
    {
        [XmlElement("Culture")]
        public List<Culture> Cultures { get; set; } = new List<Culture>();
    }

    public class Culture
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("is_main_culture")]
        public bool? IsMainCulture { get; set; }
        public bool ShouldSerializeIsMainCulture() => IsMainCulture.HasValue;

        [XmlAttribute("default_face_key")]
        public string DefaultFaceKey { get; set; }

        [XmlAttribute("color")]
        public string Color { get; set; }
        public bool ShouldSerializeColor() => !string.IsNullOrEmpty(Color);

        [XmlAttribute("color2")]
        public string Color2 { get; set; }
        public bool ShouldSerializeColor2() => !string.IsNullOrEmpty(Color2);

        [XmlAttribute("cloth_alternative_color1")]
        public string ClothAlternativeColor1 { get; set; }
        public bool ShouldSerializeClothAlternativeColor1() => !string.IsNullOrEmpty(ClothAlternativeColor1);

        [XmlAttribute("cloth_alternative_color2")]
        public string ClothAlternativeColor2 { get; set; }
        public bool ShouldSerializeClothAlternativeColor2() => !string.IsNullOrEmpty(ClothAlternativeColor2);

        [XmlAttribute("banner_background_color1")]
        public string BannerBackgroundColor1 { get; set; }
        public bool ShouldSerializeBannerBackgroundColor1() => !string.IsNullOrEmpty(BannerBackgroundColor1);

        [XmlAttribute("banner_foreground_color1")]
        public string BannerForegroundColor1 { get; set; }
        public bool ShouldSerializeBannerForegroundColor1() => !string.IsNullOrEmpty(BannerForegroundColor1);

        [XmlAttribute("banner_background_color2")]
        public string BannerBackgroundColor2 { get; set; }
        public bool ShouldSerializeBannerBackgroundColor2() => !string.IsNullOrEmpty(BannerBackgroundColor2);

        [XmlAttribute("banner_foreground_color2")]
        public string BannerForegroundColor2 { get; set; }
        public bool ShouldSerializeBannerForegroundColor2() => !string.IsNullOrEmpty(BannerForegroundColor2);

        [XmlAttribute("faction_banner_key")]
        public string FactionBannerKey { get; set; }
        public bool ShouldSerializeFactionBannerKey() => !string.IsNullOrEmpty(FactionBannerKey);
    }
}