using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data;

// Skins.xml - Character skin definitions and visual appearance system
[XmlRoot("base")]
public class SkinsBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "skin";

    [XmlElement("skins")]
    public SkinsContainer Skins { get; set; } = new SkinsContainer();
}

public class SkinsContainer
{
    [XmlElement("skin")]
    public List<Skin> Skin { get; set; } = new List<Skin>();
}

public class Skin
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("race")]
    public string? Race { get; set; }

    [XmlAttribute("gender")]
    public string? Gender { get; set; }

    [XmlAttribute("age")]
    public string? Age { get; set; }

    [XmlElement("skeleton")]
    public SkinSkeleton? Skeleton { get; set; }

    [XmlElement("hair_meshes")]
    public HairMeshes? HairMeshes { get; set; }

    [XmlElement("beard_meshes")]
    public BeardMeshes? BeardMeshes { get; set; }

    [XmlElement("voice_types")]
    public VoiceTypes? VoiceTypes { get; set; }

    [XmlElement("face_textures")]
    public FaceTextures? FaceTextures { get; set; }

    [XmlElement("body_meshes")]
    public BodyMeshes? BodyMeshes { get; set; }

    [XmlElement("tattoo_materials")]
    public TattooMaterials? TattooMaterials { get; set; }
}

public class SkinSkeleton
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("scale")]
    public string? Scale { get; set; }
}

public class HairMeshes
{
    [XmlElement("hair_mesh")]
    public List<HairMesh> HairMesh { get; set; } = new List<HairMesh>();
}

public class HairMesh
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("mesh")]
    public string? Mesh { get; set; }

    [XmlAttribute("material")]
    public string? Material { get; set; }

    [XmlAttribute("hair_cover_type")]
    public string? HairCoverType { get; set; }

    [XmlAttribute("body_name")]
    public string? BodyName { get; set; }
}

public class BeardMeshes
{
    [XmlElement("beard_mesh")]
    public List<BeardMesh> BeardMesh { get; set; } = new List<BeardMesh>();
}

public class BeardMesh
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("mesh")]
    public string? Mesh { get; set; }

    [XmlAttribute("material")]
    public string? Material { get; set; }

    [XmlAttribute("body_name")]
    public string? BodyName { get; set; }
}

public class VoiceTypes
{
    [XmlElement("voice")]
    public List<Voice> Voice { get; set; } = new List<Voice>();
}

public class Voice
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("sound_prefix")]
    public string? SoundPrefix { get; set; }

    [XmlAttribute("pitch")]
    public string? Pitch { get; set; }
}

public class FaceTextures
{
    [XmlElement("face_texture")]
    public List<FaceTexture> FaceTexture { get; set; } = new List<FaceTexture>();
}

public class FaceTexture
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("texture")]
    public string? Texture { get; set; }

    [XmlAttribute("normal_map")]
    public string? NormalMap { get; set; }

    [XmlAttribute("specular_map")]
    public string? SpecularMap { get; set; }
}

public class BodyMeshes
{
    [XmlElement("body_mesh")]
    public List<BodyMesh> BodyMesh { get; set; } = new List<BodyMesh>();
}

public class BodyMesh
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("mesh")]
    public string? Mesh { get; set; }

    [XmlAttribute("material")]
    public string? Material { get; set; }

    [XmlAttribute("body_part")]
    public string? BodyPart { get; set; }

    [XmlAttribute("weight")]
    public string? Weight { get; set; }

    [XmlAttribute("build")]
    public string? Build { get; set; }
}

public class TattooMaterials
{
    [XmlElement("tattoo_material")]
    public List<TattooMaterial> TattooMaterial { get; set; } = new List<TattooMaterial>();
}

public class TattooMaterial
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("texture")]
    public string? Texture { get; set; }

    [XmlAttribute("color_mask")]
    public string? ColorMask { get; set; }

    [XmlAttribute("alpha_texture")]
    public string? AlphaTexture { get; set; }
} 