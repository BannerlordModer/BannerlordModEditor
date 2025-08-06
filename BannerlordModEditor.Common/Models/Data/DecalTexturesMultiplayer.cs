using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class DecalTexturesMultiplayer
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("decal_textures")]
        public DecalTexturesMultiplayerDecalTextureList DecalTextures { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeDecalTextures() => DecalTextures != null;
    }

    public class DecalTexturesMultiplayerDecalTextureList
    {
        [XmlElement("decal_texture")]
        public List<DecalTexturesMultiplayerDecalTexture> Items { get; set; }

        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class DecalTexturesMultiplayerDecalTexture
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