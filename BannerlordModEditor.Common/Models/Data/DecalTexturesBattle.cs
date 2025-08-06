using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class DecalTexturesBattle
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("decal_textures")]
        public DecalTexturesContainer DecalTextures { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeDecalTextures() => DecalTextures != null;
    }

    public class DecalTexturesContainer
    {
        [XmlElement("decal_texture")]
        public List<DecalTexture> DecalTextureList { get; set; }

        public bool ShouldSerializeDecalTextureList() => DecalTextureList != null && DecalTextureList.Count > 0;
    }

    public class DecalTexture
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("atlas_pos")]
        public string AtlasPos { get; set; }

        [XmlAttribute("is_dynamic")]
        public string IsDynamic { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeAtlasPos() => !string.IsNullOrEmpty(AtlasPos);
        public bool ShouldSerializeIsDynamic() => !string.IsNullOrEmpty(IsDynamic);
    }
}