using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class DecalTexturesAll
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("decal_textures")]
        public DecalTexturesAllContainer? DecalTextures { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeDecalTextures() => DecalTextures != null;
    }

    public class DecalTexturesAllContainer
    {
        [XmlElement("decal_texture")]
        public DecalTextureAll[]? DecalTexture { get; set; }

        public bool ShouldSerializeDecalTexture() => DecalTexture != null && DecalTexture.Length > 0;
    }

    public class DecalTextureAll
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("atlas_pos")]
        public string? AtlasPos { get; set; }

        [XmlAttribute("is_dynamic")]
        public string? IsDynamic { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeAtlasPos() => !string.IsNullOrEmpty(AtlasPos);
        public bool ShouldSerializeIsDynamic() => !string.IsNullOrEmpty(IsDynamic);
    }
}