using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ParticleSystemsMapper
    {
        #region DO to DTO

        public static ParticleSystemsDTO ToDTO(ParticleSystemsDO source)
        {
            if (source == null) return null;

            return new ParticleSystemsDTO
            {
                Effects = source.Effects?.Select(ToDTO).ToList() ?? new List<EffectDTO>()
            };
        }

        public static EffectDTO ToDTO(EffectDO source)
        {
            if (source == null) return null;

            return new EffectDTO
            {
                Name = source.Name,
                Guid = source.Guid,
                SoundCode = source.SoundCode,
                Emitters = ToDTO(source.Emitters)
            };
        }

        public static EmittersDTO ToDTO(EmittersDO source)
        {
            if (source == null) return null;

            return new EmittersDTO
            {
                EmitterList = source.EmitterList?.Select(ToDTO).ToList() ?? new List<EmitterDTO>()
            };
        }

        public static EmitterDTO ToDTO(EmitterDO source)
        {
            if (source == null) return null;

            return new EmitterDTO
            {
                Name = source.Name,
                Index = source.Index,
                Flags = ToDTO(source.Flags),
                Parameters = ToDTO(source.Parameters)
            };
        }

        public static ParticleFlagsDTO ToDTO(ParticleFlagsDO source)
        {
            if (source == null) return null;

            return new ParticleFlagsDTO
            {
                FlagList = source.FlagList?.Select(ToDTO).ToList() ?? new List<ParticleFlagDTO>()
            };
        }

        public static ParticleFlagDTO ToDTO(ParticleFlagDO source)
        {
            if (source == null) return null;

            return new ParticleFlagDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static ParametersDTO ToDTO(ParametersDO source)
        {
            if (source == null) return null;

            return new ParametersDTO
            {
                ParameterList = source.ParameterList?.Select(ToDTO).ToList() ?? new List<ParameterDTO>(),
                DecalMaterials = ToDTO(source.DecalMaterials)
            };
        }

        public static ParameterDTO ToDTO(ParameterDO source)
        {
            if (source == null) return null;

            return new ParameterDTO
            {
                Name = source.Name,
                Value = source.Value,
                Base = source.Base,
                Bias = source.Bias,
                Curve = source.Curve,
                ColorElement = ToDTO(source.ColorElement),
                AlphaElement = ToDTO(source.AlphaElement),
                ParameterCurve = ToDTO(source.ParameterCurve)
            };
        }

        public static CurveDTO ToDTO(CurveDO source)
        {
            if (source == null) return null;

            return new CurveDTO
            {
                Name = source.Name,
                Version = source.Version,
                Default = source.Default,
                CurveMultiplier = source.CurveMultiplier,
                Keys = ToDTO(source.Keys)
            };
        }

        public static KeysDTO ToDTO(KeysDO source)
        {
            if (source == null) return null;

            return new KeysDTO
            {
                KeyList = source.KeyList?.Select(ToDTO).ToList() ?? new List<KeyDTO>()
            };
        }

        public static KeyDTO ToDTO(KeyDO source)
        {
            if (source == null) return null;

            return new KeyDTO
            {
                Time = source.Time,
                Value = source.Value,
                Position = source.Position,
                Tangent = source.Tangent
            };
        }

        #endregion

        #region DTO to DO

        public static ParticleSystemsDO ToDO(ParticleSystemsDTO source)
        {
            if (source == null) return null;

            return new ParticleSystemsDO
            {
                Effects = source.Effects?.Select(ToDO).ToList() ?? new List<EffectDO>()
            };
        }

        public static EffectDO ToDO(EffectDTO source)
        {
            if (source == null) return null;

            return new EffectDO
            {
                Name = source.Name,
                Guid = source.Guid,
                SoundCode = source.SoundCode,
                Emitters = ToDO(source.Emitters)
            };
        }

        public static EmittersDO ToDO(EmittersDTO source)
        {
            if (source == null) return null;

            return new EmittersDO
            {
                EmitterList = source.EmitterList?.Select(ToDO).ToList() ?? new List<EmitterDO>()
            };
        }

        public static EmitterDO ToDO(EmitterDTO source)
        {
            if (source == null) return null;

            return new EmitterDO
            {
                Name = source.Name,
                Index = source.Index,
                Flags = ToDO(source.Flags),
                Parameters = ToDO(source.Parameters)
            };
        }

        public static ParticleFlagsDO ToDO(ParticleFlagsDTO source)
        {
            if (source == null) return null;

            return new ParticleFlagsDO
            {
                FlagList = source.FlagList?.Select(ToDO).ToList() ?? new List<ParticleFlagDO>()
            };
        }

        public static ParticleFlagDO ToDO(ParticleFlagDTO source)
        {
            if (source == null) return null;

            return new ParticleFlagDO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static ParametersDO ToDO(ParametersDTO source)
        {
            if (source == null) return null;

            return new ParametersDO
            {
                ParameterList = source.ParameterList?.Select(ToDO).ToList() ?? new List<ParameterDO>(),
                DecalMaterials = ToDO(source.DecalMaterials)
            };
        }

        public static ParameterDO ToDO(ParameterDTO source)
        {
            if (source == null) return null;

            return new ParameterDO
            {
                Name = source.Name,
                Value = source.Value,
                Base = source.Base,
                Bias = source.Bias,
                Curve = source.Curve,
                ColorElement = ToDO(source.ColorElement),
                AlphaElement = ToDO(source.AlphaElement),
                ParameterCurve = ToDO(source.ParameterCurve)
            };
        }

        public static CurveDO ToDO(CurveDTO source)
        {
            if (source == null) return null;

            return new CurveDO
            {
                Name = source.Name,
                Version = source.Version,
                Default = source.Default,
                CurveMultiplier = source.CurveMultiplier,
                Keys = ToDO(source.Keys)
            };
        }

        public static KeysDO ToDO(KeysDTO source)
        {
            if (source == null) return null;

            return new KeysDO
            {
                KeyList = source.KeyList?.Select(ToDO).ToList() ?? new List<KeyDO>()
            };
        }

        public static KeyDO ToDO(KeyDTO source)
        {
            if (source == null) return null;

            return new KeyDO
            {
                Time = source.Time,
                Value = source.Value,
                Position = source.Position,
                Tangent = source.Tangent
            };
        }

        public static ColorDTO ToDTO(ColorDO source)
        {
            if (source == null) return null;

            return new ColorDTO
            {
                Keys = ToDTO(source.Keys)
            };
        }

        public static AlphaDTO ToDTO(AlphaDO source)
        {
            if (source == null) return null;

            return new AlphaDTO
            {
                Keys = ToDTO(source.Keys)
            };
        }

        public static ColorDO ToDO(ColorDTO source)
        {
            if (source == null) return null;

            return new ColorDO
            {
                Keys = ToDO(source.Keys)
            };
        }

        public static AlphaDO ToDO(AlphaDTO source)
        {
            if (source == null) return null;

            return new AlphaDO
            {
                Keys = ToDO(source.Keys)
            };
        }

        public static DecalMaterialsDTO ToDTO(DecalMaterialsDO source)
        {
            if (source == null) return null;

            return new DecalMaterialsDTO
            {
                DecalMaterialList = source.DecalMaterialList?.Select(ToDTO).ToList() ?? new List<DecalMaterialDTO>()
            };
        }

        public static DecalMaterialDTO ToDTO(DecalMaterialDO source)
        {
            if (source == null) return null;

            return new DecalMaterialDTO
            {
                Value = source.Value
            };
        }

        public static DecalMaterialsDO ToDO(DecalMaterialsDTO source)
        {
            if (source == null) return null;

            return new DecalMaterialsDO
            {
                DecalMaterialList = source.DecalMaterialList?.Select(ToDO).ToList() ?? new List<DecalMaterialDO>()
            };
        }

        public static DecalMaterialDO ToDO(DecalMaterialDTO source)
        {
            if (source == null) return null;

            return new DecalMaterialDO
            {
                Value = source.Value
            };
        }

        #endregion
    }
}