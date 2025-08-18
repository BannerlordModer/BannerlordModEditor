using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers;

public static class SkinsMapper
{
    public static SkinsDTO ToDTO(SkinsDO source)
    {
        if (source == null) return null;

        return new SkinsDTO
        {
            Type = source.Type,
            Skins = SkinsMapper.ToDTO(source.Skins)
        };
    }

    public static SkinsDO ToDO(SkinsDTO source)
    {
        if (source == null) return null;

        return new SkinsDO
        {
            Type = source.Type,
            Skins = SkinsMapper.ToDO(source.Skins)
        };
    }

    public static SkinsContainerDTO ToDTO(SkinsContainerDO source)
    {
        if (source == null) return null;

        return new SkinsContainerDTO
        {
            SkinList = source.SkinList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<SkinDTO>()
        };
    }

    public static SkinsContainerDO ToDO(SkinsContainerDTO source)
    {
        if (source == null) return null;

        return new SkinsContainerDO
        {
            SkinList = source.SkinList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<SkinDO>()
        };
    }

    public static SkinDTO ToDTO(SkinDO source)
    {
        if (source == null) return null;

        return new SkinDTO
        {
            Id = source.Id,
            Name = source.Name,
            Race = source.Race,
            Gender = source.Gender,
            Age = source.Age,
            Skeleton = SkinsMapper.ToDTO(source.Skeleton),
            HairMeshes = SkinsMapper.ToDTO(source.HairMeshes),
            BeardMeshes = SkinsMapper.ToDTO(source.BeardMeshes),
            VoiceTypes = SkinsMapper.ToDTO(source.VoiceTypes),
            FaceTextures = SkinsMapper.ToDTO(source.FaceTextures),
            BodyMeshes = SkinsMapper.ToDTO(source.BodyMeshes),
            TattooMaterials = SkinsMapper.ToDTO(source.TattooMaterials)
        };
    }

    public static SkinDO ToDO(SkinDTO source)
    {
        if (source == null) return null;

        return new SkinDO
        {
            Id = source.Id,
            Name = source.Name,
            Race = source.Race,
            Gender = source.Gender,
            Age = source.Age,
            Skeleton = SkinsMapper.ToDO(source.Skeleton),
            HairMeshes = SkinsMapper.ToDO(source.HairMeshes),
            BeardMeshes = SkinsMapper.ToDO(source.BeardMeshes),
            VoiceTypes = SkinsMapper.ToDO(source.VoiceTypes),
            FaceTextures = SkinsMapper.ToDO(source.FaceTextures),
            BodyMeshes = SkinsMapper.ToDO(source.BodyMeshes),
            TattooMaterials = SkinsMapper.ToDO(source.TattooMaterials)
        };
    }

    public static SkinSkeletonDTO ToDTO(SkinSkeletonDO source)
    {
        if (source == null) return null;

        return new SkinSkeletonDTO
        {
            Name = source.Name,
            Scale = source.Scale
        };
    }

    public static SkinSkeletonDO ToDO(SkinSkeletonDTO source)
    {
        if (source == null) return null;

        return new SkinSkeletonDO
        {
            Name = source.Name,
            Scale = source.Scale
        };
    }

    public static HairMeshesDTO ToDTO(HairMeshesDO source)
    {
        if (source == null) return null;

        return new HairMeshesDTO
        {
            HairMeshList = source.HairMeshList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<HairMeshDTO>()
        };
    }

    public static HairMeshesDO ToDO(HairMeshesDTO source)
    {
        if (source == null) return null;

        return new HairMeshesDO
        {
            HairMeshList = source.HairMeshList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<HairMeshDO>()
        };
    }

    public static HairMeshDTO ToDTO(HairMeshDO source)
    {
        if (source == null) return null;

        return new HairMeshDTO
        {
            Id = source.Id,
            Name = source.Name,
            Mesh = source.Mesh,
            Material = source.Material,
            HairCoverType = source.HairCoverType,
            BodyName = source.BodyName
        };
    }

    public static HairMeshDO ToDO(HairMeshDTO source)
    {
        if (source == null) return null;

        return new HairMeshDO
        {
            Id = source.Id,
            Name = source.Name,
            Mesh = source.Mesh,
            Material = source.Material,
            HairCoverType = source.HairCoverType,
            BodyName = source.BodyName
        };
    }

    public static BeardMeshesDTO ToDTO(BeardMeshesDO source)
    {
        if (source == null) return null;

        return new BeardMeshesDTO
        {
            BeardMeshList = source.BeardMeshList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<BeardMeshDTO>()
        };
    }

    public static BeardMeshesDO ToDO(BeardMeshesDTO source)
    {
        if (source == null) return null;

        return new BeardMeshesDO
        {
            BeardMeshList = source.BeardMeshList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<BeardMeshDO>()
        };
    }

    public static BeardMeshDTO ToDTO(BeardMeshDO source)
    {
        if (source == null) return null;

        return new BeardMeshDTO
        {
            Id = source.Id,
            Name = source.Name,
            Mesh = source.Mesh,
            Material = source.Material,
            BodyName = source.BodyName
        };
    }

    public static BeardMeshDO ToDO(BeardMeshDTO source)
    {
        if (source == null) return null;

        return new BeardMeshDO
        {
            Id = source.Id,
            Name = source.Name,
            Mesh = source.Mesh,
            Material = source.Material,
            BodyName = source.BodyName
        };
    }

    public static VoiceTypesDTO ToDTO(VoiceTypesDO source)
    {
        if (source == null) return null;

        return new VoiceTypesDTO
        {
            VoiceList = source.VoiceList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<VoiceDTO>()
        };
    }

    public static VoiceTypesDO ToDO(VoiceTypesDTO source)
    {
        if (source == null) return null;

        return new VoiceTypesDO
        {
            VoiceList = source.VoiceList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<VoiceDO>()
        };
    }

    public static VoiceDTO ToDTO(VoiceDO source)
    {
        if (source == null) return null;

        return new VoiceDTO
        {
            Id = source.Id,
            Name = source.Name,
            SoundPrefix = source.SoundPrefix,
            Pitch = source.Pitch
        };
    }

    public static VoiceDO ToDO(VoiceDTO source)
    {
        if (source == null) return null;

        return new VoiceDO
        {
            Id = source.Id,
            Name = source.Name,
            SoundPrefix = source.SoundPrefix,
            Pitch = source.Pitch
        };
    }

    public static FaceTexturesDTO ToDTO(FaceTexturesDO source)
    {
        if (source == null) return null;

        return new FaceTexturesDTO
        {
            FaceTextureList = source.FaceTextureList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<FaceTextureDTO>()
        };
    }

    public static FaceTexturesDO ToDO(FaceTexturesDTO source)
    {
        if (source == null) return null;

        return new FaceTexturesDO
        {
            FaceTextureList = source.FaceTextureList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<FaceTextureDO>()
        };
    }

    public static FaceTextureDTO ToDTO(FaceTextureDO source)
    {
        if (source == null) return null;

        return new FaceTextureDTO
        {
            Id = source.Id,
            Name = source.Name,
            Texture = source.Texture,
            NormalMap = source.NormalMap,
            SpecularMap = source.SpecularMap
        };
    }

    public static FaceTextureDO ToDO(FaceTextureDTO source)
    {
        if (source == null) return null;

        return new FaceTextureDO
        {
            Id = source.Id,
            Name = source.Name,
            Texture = source.Texture,
            NormalMap = source.NormalMap,
            SpecularMap = source.SpecularMap
        };
    }

    public static BodyMeshesDTO ToDTO(BodyMeshesDO source)
    {
        if (source == null) return null;

        return new BodyMeshesDTO
        {
            BodyMeshList = source.BodyMeshList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<BodyMeshDTO>()
        };
    }

    public static BodyMeshesDO ToDO(BodyMeshesDTO source)
    {
        if (source == null) return null;

        return new BodyMeshesDO
        {
            BodyMeshList = source.BodyMeshList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<BodyMeshDO>()
        };
    }

    public static BodyMeshDTO ToDTO(BodyMeshDO source)
    {
        if (source == null) return null;

        return new BodyMeshDTO
        {
            Id = source.Id,
            Name = source.Name,
            Mesh = source.Mesh,
            Material = source.Material,
            BodyPart = source.BodyPart,
            Weight = source.Weight,
            Build = source.Build
        };
    }

    public static BodyMeshDO ToDO(BodyMeshDTO source)
    {
        if (source == null) return null;

        return new BodyMeshDO
        {
            Id = source.Id,
            Name = source.Name,
            Mesh = source.Mesh,
            Material = source.Material,
            BodyPart = source.BodyPart,
            Weight = source.Weight,
            Build = source.Build
        };
    }

    public static TattooMaterialsDTO ToDTO(TattooMaterialsDO source)
    {
        if (source == null) return null;

        return new TattooMaterialsDTO
        {
            TattooMaterialList = source.TattooMaterialList?
                .Select(SkinsMapper.ToDTO)
                .ToList() ?? new List<TattooMaterialDTO>()
        };
    }

    public static TattooMaterialsDO ToDO(TattooMaterialsDTO source)
    {
        if (source == null) return null;

        return new TattooMaterialsDO
        {
            TattooMaterialList = source.TattooMaterialList?
                .Select(SkinsMapper.ToDO)
                .ToList() ?? new List<TattooMaterialDO>()
        };
    }

    public static TattooMaterialDTO ToDTO(TattooMaterialDO source)
    {
        if (source == null) return null;

        return new TattooMaterialDTO
        {
            Id = source.Id,
            Name = source.Name,
            Texture = source.Texture,
            ColorMask = source.ColorMask,
            AlphaTexture = source.AlphaTexture
        };
    }

    public static TattooMaterialDO ToDO(TattooMaterialDTO source)
    {
        if (source == null) return null;

        return new TattooMaterialDO
        {
            Id = source.Id,
            Name = source.Name,
            Texture = source.Texture,
            ColorMask = source.ColorMask,
            AlphaTexture = source.AlphaTexture
        };
    }
}