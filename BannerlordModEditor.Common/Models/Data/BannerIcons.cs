using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class BannerIcons
    {
        // The type attribute is present in the XML, but seems to be a generic attribute for the loader.
        // It's not essential for the data structure itself, so we can omit it if it simplifies things,
        // or add it if strict validation requires it. Let's keep it simple for now.
        // [XmlAttribute("type")]
        // public string Type { get; set; }

        [XmlElement("BannerIconData")]
        public BannerIconData? BannerIconData { get; set; }
    }

    public class BannerIconData
    {
        [XmlElement("BannerIconGroup")]
        public BannerIconGroup[]? BannerIconGroups { get; set; }

        [XmlElement("BannerColors")]
        public BannerColors? BannerColors { get; set; }
    }

    public class BannerIconGroup
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("is_pattern")]
        public bool IsPattern { get; set; }

        [XmlElement("Background", typeof(Background))]
        [XmlElement("Icon", typeof(Icon))]
        public object[]? Items { get; set; }
    }

    public class Background
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("mesh_name")]
        public string? MeshName { get; set; }

        [XmlAttribute("is_base_background")]
        public bool IsBaseBackground { get; set; }

        public bool ShouldSerializeIsBaseBackground() => IsBaseBackground;
    }

    public class Icon
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("material_name")]
        public string? MaterialName { get; set; }

        [XmlAttribute("texture_index")]
        public int TextureIndex { get; set; }

        [XmlAttribute("is_reserved")]
        public bool IsReserved { get; set; }
        
        public bool ShouldSerializeIsReserved() => IsReserved;
    }

    public class BannerColors
    {
        [XmlElement("Color")]
        public BannerColor[]? Colors { get; set; }
    }

    public class BannerColor
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }
        
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("hex")]
        public string? Hex { get; set; }
        
        [XmlAttribute("is_background_color")]
        public bool IsBackgroundColor { get; set; }
        
        public bool ShouldSerializeIsBackgroundColor() => IsBackgroundColor;
        
        [XmlAttribute("is_player_usable")]
        public bool IsPlayerUsable { get; set; }
        
        public bool ShouldSerializeIsPlayerUsable() => IsPlayerUsable;
    }
} 