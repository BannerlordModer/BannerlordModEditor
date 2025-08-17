using BannerlordModEditor.Common.Models.DO.Audio;
using BannerlordModEditor.Common.Models.DTO.Audio;

namespace BannerlordModEditor.Common.Mappers;

public static class VoiceDefinitionsMapper
{
    public static VoiceDefinitionsDTO ToDTO(VoiceDefinitionsDO source)
    {
        if (source == null) return null;

        return new VoiceDefinitionsDTO
        {
            VoiceTypeDeclarations = VoiceTypeDeclarationsMapper.ToDTO(source.VoiceTypeDeclarations),
            VoiceDefinitions = source.VoiceDefinitions?
                .Select(VoiceDefinitionsMapper.ToDTO)
                .ToList() ?? new List<VoiceDefinitionDTO>()
        };
    }

    public static VoiceDefinitionsDO ToDO(VoiceDefinitionsDTO source)
    {
        if (source == null) return null;

        return new VoiceDefinitionsDO
        {
            VoiceTypeDeclarations = VoiceTypeDeclarationsMapper.ToDO(source.VoiceTypeDeclarations),
            VoiceDefinitions = source.VoiceDefinitions?
                .Select(VoiceDefinitionsMapper.ToDO)
                .ToList() ?? new List<VoiceDefinitionDO>(),
            HasVoiceTypeDeclarations = source.VoiceTypeDeclarations != null && source.VoiceTypeDeclarations.VoiceTypes.Count > 0
        };
    }

    public static VoiceDefinitionDTO ToDTO(VoiceDefinitionDO source)
    {
        if (source == null) return null;

        return new VoiceDefinitionDTO
        {
            Name = source.Name,
            SoundAndCollisionInfoClass = source.SoundAndCollisionInfoClass,
            OnlyForNpcs = source.OnlyForNpcs,
            MinPitchMultiplier = source.MinPitchMultiplier,
            MaxPitchMultiplier = source.MaxPitchMultiplier,
            Voices = source.Voices?
                .Select(VoiceMapper.ToDTO)
                .ToList() ?? new List<VoiceDTO>()
        };
    }

    public static VoiceDefinitionDO ToDO(VoiceDefinitionDTO source)
    {
        if (source == null) return null;

        return new VoiceDefinitionDO
        {
            Name = source.Name,
            SoundAndCollisionInfoClass = source.SoundAndCollisionInfoClass,
            OnlyForNpcs = source.OnlyForNpcs,
            MinPitchMultiplier = source.MinPitchMultiplier,
            MaxPitchMultiplier = source.MaxPitchMultiplier,
            Voices = source.Voices?
                .Select(VoiceMapper.ToDO)
                .ToList() ?? new List<VoiceDO>()
        };
    }
}

public static class VoiceTypeDeclarationsMapper
{
    public static VoiceTypeDeclarationsDTO ToDTO(VoiceTypeDeclarationsDO source)
    {
        if (source == null) return null;

        return new VoiceTypeDeclarationsDTO
        {
            VoiceTypes = source.VoiceTypes?
                .Select(VoiceTypeDeclarationsMapper.ToDTO)
                .ToList() ?? new List<VoiceTypeDTO>()
        };
    }

    public static VoiceTypeDeclarationsDO ToDO(VoiceTypeDeclarationsDTO source)
    {
        if (source == null) return null;

        return new VoiceTypeDeclarationsDO
        {
            VoiceTypes = source.VoiceTypes?
                .Select(VoiceTypeDeclarationsMapper.ToDO)
                .ToList() ?? new List<VoiceTypeDO>()
        };
    }

    public static VoiceTypeDTO ToDTO(VoiceTypeDO source)
    {
        if (source == null) return null;

        return new VoiceTypeDTO
        {
            Name = source.Name
        };
    }

    public static VoiceTypeDO ToDO(VoiceTypeDTO source)
    {
        if (source == null) return null;

        return new VoiceTypeDO
        {
            Name = source.Name
        };
    }
}

public static class VoiceMapper
{
    public static VoiceDTO ToDTO(VoiceDO source)
    {
        if (source == null) return null;

        return new VoiceDTO
        {
            Type = source.Type,
            Path = source.Path,
            FaceAnim = source.FaceAnim
        };
    }

    public static VoiceDO ToDO(VoiceDTO source)
    {
        if (source == null) return null;

        return new VoiceDO
        {
            Type = source.Type,
            Path = source.Path,
            FaceAnim = source.FaceAnim
        };
    }
}