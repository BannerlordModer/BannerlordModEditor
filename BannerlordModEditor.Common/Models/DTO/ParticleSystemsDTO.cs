using System;
using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class ParticleSystemsDTO
    {
        public List<EffectDTO> Effects { get; set; } = new List<EffectDTO>();

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;

        // 便捷属性
        public int EffectsCount => Effects?.Count ?? 0;
    }

    public class EffectDTO
    {
        public string? Name { get; set; }
        public string? Guid { get; set; }
        public string? SoundCode { get; set; }
        public EmittersDTO? Emitters { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
        public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);
        public bool ShouldSerializeEmitters() => Emitters != null;

        // 类型安全的便捷属性
        public bool HasSoundCode => !string.IsNullOrEmpty(SoundCode);
        public bool HasEmitters => Emitters != null;
        public int EmittersCount => Emitters?.EmitterList?.Count ?? 0;
    }

    public class EmittersDTO
    {
        public List<EmitterDTO> EmitterList { get; set; } = new List<EmitterDTO>();

        public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
        
        // 便捷属性
        public int Count => EmitterList?.Count ?? 0;
        public bool HasEmitters => EmitterList != null && EmitterList.Count > 0;
    }

    public class EmitterDTO
    {
        public string? Name { get; set; }
        public string? Index { get; set; }
        public ParticleFlagsDTO? Flags { get; set; }
        public ParametersDTO? Parameters { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
        public bool ShouldSerializeFlags() => Flags != null;
        public bool ShouldSerializeParameters() => Parameters != null;

        // 类型安全的便捷属性
        public int? IndexInt => int.TryParse(Index, out int idx) ? idx : (int?)null;
        public bool HasFlags => Flags != null;
        public bool HasParameters => Parameters != null;
        public int FlagsCount => Flags?.FlagList?.Count ?? 0;
        public int ParametersCount => Parameters?.ParameterList?.Count ?? 0;

        // 设置方法
        public void SetIndex(int? value) => Index = value?.ToString();
    }

    public class ParticleFlagsDTO
    {
        public List<ParticleFlagDTO> FlagList { get; set; } = new List<ParticleFlagDTO>();

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
        
        // 便捷属性
        public int Count => FlagList?.Count ?? 0;
        public bool HasFlags => FlagList != null && FlagList.Count > 0;
    }

    public class ParticleFlagDTO
    {
        public string? Name { get; set; }
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);

        // 类型安全的便捷属性
        public bool? ValueBool => bool.TryParse(Value, out bool val) ? val : (bool?)null;
        public bool HasValue => !string.IsNullOrEmpty(Value);

        // 设置方法
        public void SetValueBool(bool? value) => Value = value?.ToString().ToLower();
    }

    public class ParametersDTO
    {
        public List<ParameterDTO> ParameterList { get; set; } = new List<ParameterDTO>();

        public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
        
        // 便捷属性
        public int Count => ParameterList?.Count ?? 0;
        public bool HasParameters => ParameterList != null && ParameterList.Count > 0;
    }

    public class ParameterDTO
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
        public string? Bias { get; set; }
        public string? Curve { get; set; }
        public string? Color { get; set; }
        public string? Alpha { get; set; }
        public CurveDTO? ParameterCurve { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
        public bool ShouldSerializeCurve() => !string.IsNullOrEmpty(Curve);
        public bool ShouldSerializeColor() => !string.IsNullOrEmpty(Color);
        public bool ShouldSerializeAlpha() => !string.IsNullOrEmpty(Alpha);
        public bool ShouldSerializeParameterCurve() => ParameterCurve != null;

        // 类型安全的便捷属性
        public double? ValueDouble => double.TryParse(Value, out double val) ? val : (double?)null;
        public double? BiasDouble => double.TryParse(Bias, out double bias) ? bias : (double?)null;
        public double? CurveDouble => double.TryParse(Curve, out double curve) ? curve : (double?)null;
        public double? AlphaDouble => double.TryParse(Alpha, out double alpha) ? alpha : (double?)null;

        // 设置方法
        public void SetValueDouble(double? value) => Value = value?.ToString();
        public void SetBiasDouble(double? value) => Bias = value?.ToString();
        public void SetCurveDouble(double? value) => Curve = value?.ToString();
        public void SetAlphaDouble(double? value) => Alpha = value?.ToString();
    }

    public class CurveDTO
    {
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? Default { get; set; }
        public string? CurveMultiplier { get; set; }
        public KeysDTO? Keys { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
        public bool ShouldSerializeDefault() => !string.IsNullOrEmpty(Default);
        public bool ShouldSerializeCurveMultiplier() => !string.IsNullOrEmpty(CurveMultiplier);
        public bool ShouldSerializeKeys() => Keys != null;

        // 类型安全的便捷属性
        public double? DefaultDouble => double.TryParse(Default, out double def) ? def : (double?)null;
        public double? CurveMultiplierDouble => double.TryParse(CurveMultiplier, out double multiplier) ? multiplier : (double?)null;
        public int? VersionInt => int.TryParse(Version, out int ver) ? ver : (int?)null;

        // 设置方法
        public void SetDefaultDouble(double? value) => Default = value?.ToString();
        public void SetCurveMultiplierDouble(double? value) => CurveMultiplier = value?.ToString();
        public void SetVersionInt(int? value) => Version = value?.ToString();
    }

    public class KeysDTO
    {
        public List<KeyDTO> KeyList { get; set; } = new List<KeyDTO>();

        public bool ShouldSerializeKeyList() => KeyList != null && KeyList.Count > 0;
        
        // 便捷属性
        public int Count => KeyList?.Count ?? 0;
        public bool HasKeys => KeyList != null && KeyList.Count > 0;
    }

    public class KeyDTO
    {
        public string? Value { get; set; }
        public string? Position { get; set; }
        public string? Tangent { get; set; }

        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeTangent() => !string.IsNullOrEmpty(Tangent);

        // 类型安全的便捷属性
        public double? ValueDouble => double.TryParse(Value, out double val) ? val : (double?)null;
        public double? PositionDouble => double.TryParse(Position, out double pos) ? pos : (double?)null;
        public bool HasTangent => !string.IsNullOrEmpty(Tangent);

        // 设置方法
        public void SetValueDouble(double? value) => Value = value?.ToString();
        public void SetPositionDouble(double? value) => Position = value?.ToString();
    }
}