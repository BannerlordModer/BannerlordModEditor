using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class SkinnedDecals
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlArray("skinned_decals")]
        [XmlArrayItem("skinned_decal")]
        public SkinnedDecal[]? Decals { get; set; }
    }

    public class SkinnedDecal
    {
        [XmlArray("textures")]
        [XmlArrayItem("texture")]
        public DecalTexture[]? Textures { get; set; }

        [XmlArray("materials")]
        [XmlArrayItem("material")]
        public DecalMaterial[]? Materials { get; set; }
    }

    public class DecalTexture
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }
    }

    public class DecalMaterial
    {
        [XmlAttribute("enum")]
        public string? Enum { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }
    }
} 