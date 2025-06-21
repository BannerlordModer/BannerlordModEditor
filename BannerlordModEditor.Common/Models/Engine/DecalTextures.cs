using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine
{
    [XmlRoot("base")]
    public class DecalTexturesRoot
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("decal_textures")]
        public DecalTextures? DecalTextures { get; set; }
    }

    public class DecalTextures
    {
        [XmlElement("decal_texture")]
        public DecalTexture[]? DecalTexture { get; set; }
    }

    public class DecalTexture
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("atlas_pos")]
        public string? AtlasPos { get; set; }

        [XmlAttribute("is_dynamic")]
        public string? IsDynamic { get; set; }
    }
} 