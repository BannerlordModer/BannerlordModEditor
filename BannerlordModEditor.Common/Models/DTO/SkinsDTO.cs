using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO;

// Skins.xml - Character skin definitions and visual appearance system
[XmlRoot("base")]
public class SkinsDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "skin";

    [XmlElement("skins")]
    public SkinsContainerDTO Skins { get; set; } = new SkinsContainerDTO();
}

public class SkinsContainerDTO
{
    [XmlElement("skin")]
    public List<SkinDTO> SkinList { get; set; } = new List<SkinDTO>();
}

public class SkinDTO
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
    public SkinSkeletonDTO? Skeleton { get; set; }

    [XmlElement("hair_meshes")]
    public HairMeshesDTO? HairMeshes { get; set; }

    [XmlElement("beard_meshes")]
    public BeardMeshesDTO? BeardMeshes { get; set; }

    [XmlElement("voice_types")]
    public VoiceTypesDTO? VoiceTypes { get; set; }

    [XmlElement("face_textures")]
    public FaceTexturesDTO? FaceTextures { get; set; }

    [XmlElement("body_meshes")]
    public BodyMeshesDTO? BodyMeshes { get; set; }

    [XmlElement("tattoo_materials")]
    public TattooMaterialsDTO? TattooMaterials { get; set; }
}

public class SkinSkeletonDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("scale")]
    public string? Scale { get; set; }
}

public class HairMeshesDTO
{
    [XmlElement("hair_mesh")]
    public List<HairMeshDTO> HairMeshList { get; set; } = new List<HairMeshDTO>();
}

public class HairMeshDTO
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

public class BeardMeshesDTO
{
    [XmlElement("beard_mesh")]
    public List<BeardMeshDTO> BeardMeshList { get; set; } = new List<BeardMeshDTO>();
}

public class BeardMeshDTO
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

public class VoiceTypesDTO
{
    [XmlElement("voice")]
    public List<VoiceDTO> VoiceList { get; set; } = new List<VoiceDTO>();
}

public class VoiceDTO
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

public class FaceTexturesDTO
{
    [XmlElement("face_texture")]
    public List<FaceTextureDTO> FaceTextureList { get; set; } = new List<FaceTextureDTO>();
}

public class FaceTextureDTO
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

public class BodyMeshesDTO
{
    [XmlElement("body_mesh")]
    public List<BodyMeshDTO> BodyMeshList { get; set; } = new List<BodyMeshDTO>();
}

public class BodyMeshDTO
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

public class TattooMaterialsDTO
{
    [XmlElement("tattoo_material")]
    public List<TattooMaterialDTO> TattooMaterialList { get; set; } = new List<TattooMaterialDTO>();
}

public class TattooMaterialDTO
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