using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers;

public static class ParticleSystemsMapIconMapper
{
    public static ParticleSystemsMapIconDTO ToDTO(ParticleSystemsMapIconDO source)
    {
        if (source == null) return null;

        return new ParticleSystemsMapIconDTO
        {
            Effects = source.Effects?
                .Select(ParticleSystemsMapIconMapper.ToDTO)
                .ToList() ?? new List<ParticleEffectDTO>()
        };
    }

    public static ParticleSystemsMapIconDO ToDO(ParticleSystemsMapIconDTO source)
    {
        if (source == null) return null;

        return new ParticleSystemsMapIconDO
        {
            Effects = source.Effects?
                .Select(ParticleSystemsMapIconMapper.ToDO)
                .ToList() ?? new List<ParticleEffectDO>()
        };
    }

    public static ParticleEffectDTO ToDTO(ParticleEffectDO source)
    {
        if (source == null) return null;

        return new ParticleEffectDTO
        {
            Name = source.Name,
            Guid = source.Guid,
            Emitters = source.Emitters?
                .Select(ParticleSystemsMapIconMapper.ToDTO)
                .ToList() ?? new List<ParticleEmitterDTO>()
        };
    }

    public static ParticleEffectDO ToDO(ParticleEffectDTO source)
    {
        if (source == null) return null;

        return new ParticleEffectDO
        {
            Name = source.Name,
            Guid = source.Guid,
            Emitters = source.Emitters?
                .Select(ParticleSystemsMapIconMapper.ToDO)
                .ToList() ?? new List<ParticleEmitterDO>()
        };
    }

    public static ParticleEmitterDTO ToDTO(ParticleEmitterDO source)
    {
        if (source == null) return null;

        return new ParticleEmitterDTO
        {
            Name = source.Name,
            Index = source.Index,
            Flags = source.Flags?
                .Select(ParticleSystemsMapIconMapper.ToDTO)
                .ToList() ?? new List<MapIconParticleFlagDTO>(),
            Parameters = source.Parameters?
                .Select(ParticleSystemsMapIconMapper.ToDTO)
                .ToList() ?? new List<ParticleParameterDTO>(),
            Curves = source.Curves?
                .Select(ParticleSystemsMapIconMapper.ToDTO)
                .ToList(),
            Material = ParticleSystemsMapIconMapper.ToDTO(source.Material)
        };
    }

    public static ParticleEmitterDO ToDO(ParticleEmitterDTO source)
    {
        if (source == null) return null;

        return new ParticleEmitterDO
        {
            Name = source.Name,
            Index = source.Index,
            Flags = source.Flags?
                .Select(ParticleSystemsMapIconMapper.ToDO)
                .ToList() ?? new List<MapIconParticleFlagDO>(),
            Parameters = source.Parameters?
                .Select(ParticleSystemsMapIconMapper.ToDO)
                .ToList() ?? new List<ParticleParameterDO>(),
            Curves = source.Curves?
                .Select(ParticleSystemsMapIconMapper.ToDO)
                .ToList(),
            Material = ParticleSystemsMapIconMapper.ToDO(source.Material)
        };
    }

    public static MapIconParticleFlagDTO ToDTO(MapIconParticleFlagDO source)
    {
        if (source == null) return null;

        return new MapIconParticleFlagDTO
        {
            Name = source.Name,
            Value = source.Value
        };
    }

    public static MapIconParticleFlagDO ToDO(MapIconParticleFlagDTO source)
    {
        if (source == null) return null;

        return new MapIconParticleFlagDO
        {
            Name = source.Name,
            Value = source.Value
        };
    }

    public static ParticleParameterDTO ToDTO(ParticleParameterDO source)
    {
        if (source == null) return null;

        return new ParticleParameterDTO
        {
            Name = source.Name,
            Value = source.Value,
            Base = source.Base,
            Bias = source.Bias
        };
    }

    public static ParticleParameterDO ToDO(ParticleParameterDTO source)
    {
        if (source == null) return null;

        return new ParticleParameterDO
        {
            Name = source.Name,
            Value = source.Value,
            Base = source.Base,
            Bias = source.Bias
        };
    }

    public static ParticleCurveDTO ToDTO(ParticleCurveDO source)
    {
        if (source == null) return null;

        return new ParticleCurveDTO
        {
            Name = source.Name,
            Points = source.Points?
                .Select(ParticleSystemsMapIconMapper.ToDTO)
                .ToList()
        };
    }

    public static ParticleCurveDO ToDO(ParticleCurveDTO source)
    {
        if (source == null) return null;

        return new ParticleCurveDO
        {
            Name = source.Name,
            Points = source.Points?
                .Select(ParticleSystemsMapIconMapper.ToDO)
                .ToList()
        };
    }

    public static CurvePointDTO ToDTO(CurvePointDO source)
    {
        if (source == null) return null;

        return new CurvePointDTO
        {
            X = source.X,
            Y = source.Y
        };
    }

    public static CurvePointDO ToDO(CurvePointDTO source)
    {
        if (source == null) return null;

        return new CurvePointDO
        {
            X = source.X,
            Y = source.Y
        };
    }

    public static ParticleMaterialDTO ToDTO(ParticleMaterialDO source)
    {
        if (source == null) return null;

        return new ParticleMaterialDTO
        {
            Name = source.Name,
            Texture = source.Texture
        };
    }

    public static ParticleMaterialDO ToDO(ParticleMaterialDTO source)
    {
        if (source == null) return null;

        return new ParticleMaterialDO
        {
            Name = source.Name,
            Texture = source.Texture
        };
    }
}