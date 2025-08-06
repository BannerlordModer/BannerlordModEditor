using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class SkinnedDecalsBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlElement("skinned_decals")]
        public SkinnedDecalsContainer SkinnedDecals { get; set; } = new SkinnedDecalsContainer();
    }

    public class SkinnedDecalsContainer
    {
        [XmlElement("skinned_decal")]
        public List<SkinnedDecal> SkinnedDecalList { get; set; } = new List<SkinnedDecal>();
    }

    public class SkinnedDecal
    {
        [XmlElement("textures")]
        public DecalTextures? Textures { get; set; }

        [XmlElement("materials")]
        public DecalMaterials? Materials { get; set; }
    }

    public class DecalTextures
    {
        [XmlElement("texture")]
        public List<SkinnedDecalTexture> TextureList { get; set; } = new List<SkinnedDecalTexture>();
    }

    public class SkinnedDecalTexture
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class DecalMaterials
    {
        [XmlElement("material")]
        public List<DecalMaterial> MaterialList { get; set; } = new List<DecalMaterial>();
    }

    public class DecalMaterial
    {
        [XmlAttribute("enum")]
        public string Enum { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 