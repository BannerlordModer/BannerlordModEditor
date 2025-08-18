using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO;

// Skins.xml - Character skin definitions and visual appearance system
[XmlRoot("base")]
public class SkinsDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "skin";

    [XmlElement("skins")]
    public SkinsContainerDO Skins { get; set; } = new SkinsContainerDO();
    
    [XmlIgnore]
    public bool HasSkins { get; set; }
    
    public bool ShouldSerializeSkins() => HasSkins;
}

public class SkinsContainerDO
{
    [XmlElement("skin")]
    public List<SkinDO> SkinList { get; set; } = new List<SkinDO>();
}

public class SkinDO
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
    public SkinSkeletonDO? Skeleton { get; set; }
    
    [XmlIgnore]
    public bool HasSkeleton { get; set; }

    [XmlElement("hair_meshes")]
    public HairMeshesDO? HairMeshes { get; set; }
    
    [XmlIgnore]
    public bool HasHairMeshes { get; set; }

    [XmlElement("beard_meshes")]
    public BeardMeshesDO? BeardMeshes { get; set; }
    
    [XmlIgnore]
    public bool HasBeardMeshes { get; set; }

    [XmlElement("voice_types")]
    public VoiceTypesDO? VoiceTypes { get; set; }
    
    [XmlIgnore]
    public bool HasVoiceTypes { get; set; }

    [XmlElement("face_textures")]
    public FaceTexturesDO? FaceTextures { get; set; }
    
    [XmlIgnore]
    public bool HasFaceTextures { get; set; }

    [XmlElement("body_meshes")]
    public BodyMeshesDO? BodyMeshes { get; set; }
    
    [XmlIgnore]
    public bool HasBodyMeshes { get; set; }

    [XmlElement("tattoo_materials")]
    public TattooMaterialsDO? TattooMaterials { get; set; }
    
    [XmlIgnore]
    public bool HasTattooMaterials { get; set; }
    
    public bool ShouldSerializeSkeleton() => HasSkeleton;
    public bool ShouldSerializeHairMeshes() => HasHairMeshes;
    public bool ShouldSerializeBeardMeshes() => HasBeardMeshes;
    public bool ShouldSerializeVoiceTypes() => HasVoiceTypes;
    public bool ShouldSerializeFaceTextures() => HasFaceTextures;
    public bool ShouldSerializeBodyMeshes() => HasBodyMeshes;
    public bool ShouldSerializeTattooMaterials() => HasTattooMaterials;
}

public class SkinSkeletonDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("scale")]
    public string? Scale { get; set; }
}

public class HairMeshesDO
{
    [XmlElement("hair_mesh")]
    public List<HairMeshDO> HairMeshList { get; set; } = new List<HairMeshDO>();
    
    [XmlIgnore]
    public bool HasHairMeshes { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyHairMeshes { get; set; }
    
    public bool ShouldSerializeHairMeshList() => HasEmptyHairMeshes || (HairMeshList != null && HairMeshList.Count > 0);
}

public class HairMeshDO
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

public class BeardMeshesDO
{
    [XmlElement("beard_mesh")]
    public List<BeardMeshDO> BeardMeshList { get; set; } = new List<BeardMeshDO>();
    
    [XmlIgnore]
    public bool HasBeardMeshes { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyBeardMeshes { get; set; }
    
    public bool ShouldSerializeBeardMeshList() => HasEmptyBeardMeshes || (BeardMeshList != null && BeardMeshList.Count > 0);
}

public class BeardMeshDO
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

public class VoiceTypesDO
{
    [XmlElement("voice")]
    public List<VoiceDO> VoiceList { get; set; } = new List<VoiceDO>();
    
    [XmlIgnore]
    public bool HasVoiceTypes { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyVoiceTypes { get; set; }
    
    public bool ShouldSerializeVoiceList() => HasEmptyVoiceTypes || (VoiceList != null && VoiceList.Count > 0);
}

public class VoiceDO
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

public class FaceTexturesDO
{
    [XmlElement("face_texture")]
    public List<FaceTextureDO> FaceTextureList { get; set; } = new List<FaceTextureDO>();
    
    [XmlIgnore]
    public bool HasFaceTextures { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyFaceTextures { get; set; }
    
    public bool ShouldSerializeFaceTextureList() => HasEmptyFaceTextures || (FaceTextureList != null && FaceTextureList.Count > 0);
}

public class FaceTextureDO
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

public class BodyMeshesDO
{
    [XmlElement("body_mesh")]
    public List<BodyMeshDO> BodyMeshList { get; set; } = new List<BodyMeshDO>();
    
    [XmlIgnore]
    public bool HasBodyMeshes { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyBodyMeshes { get; set; }
    
    public bool ShouldSerializeBodyMeshList() => HasEmptyBodyMeshes || (BodyMeshList != null && BodyMeshList.Count > 0);
}

public class BodyMeshDO
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

public class TattooMaterialsDO
{
    [XmlElement("tattoo_material")]
    public List<TattooMaterialDO> TattooMaterialList { get; set; } = new List<TattooMaterialDO>();
    
    [XmlIgnore]
    public bool HasTattooMaterials { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyTattooMaterials { get; set; }
    
    public bool ShouldSerializeTattooMaterialList() => HasEmptyTattooMaterials || (TattooMaterialList != null && TattooMaterialList.Count > 0);
}

public class TattooMaterialDO
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