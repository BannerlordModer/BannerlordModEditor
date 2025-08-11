using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsDO
    {
        [XmlElement("effect")]
        public List<EffectDO> Effects { get; set; } = new List<EffectDO>();

        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }

    public class EffectDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("guid")]
        public string? Guid { get; set; }

        [XmlAttribute("sound_code")]
        public string? SoundCode { get; set; }

        [XmlElement("emitters")]
        public EmittersDO? Emitters { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
        public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);
        public bool ShouldSerializeEmitters() => Emitters != null;
    }

    public class EmittersDO
    {
        [XmlElement("emitter")]
        public List<EmitterDO> EmitterList { get; set; } = new List<EmitterDO>();

        public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
    }

    public class EmitterDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("_index_")]
        public string? Index { get; set; }

        [XmlElement("flags")]
        public ParticleFlagsDO? Flags { get; set; }

        [XmlElement("parameters")]
        public ParametersDO? Parameters { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
        public bool ShouldSerializeFlags() => Flags != null;
        public bool ShouldSerializeParameters() => Parameters != null;
    }

    public class ParticleFlagsDO
    {
        [XmlElement("flag")]
        public List<ParticleFlagDO> FlagList { get; set; } = new List<ParticleFlagDO>();

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class ParticleFlagDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => Value != null;
    }

    public class ParametersDO
    {
        [XmlElement("parameter")]
        public List<ParameterDO> ParameterList { get; set; } = new List<ParameterDO>();

        public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
    }

    public class ParameterDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        [XmlAttribute("base")]
        public string? Base { get; set; }

        [XmlAttribute("bias")]
        public string? Bias { get; set; }

        [XmlAttribute("curve")]
        public string? Curve { get; set; }

        [XmlElement("curve")]
        public CurveDO? ParameterCurve { get; set; }

        [XmlElement("color")]
        public ColorDO? ColorElement { get; set; }

        [XmlElement("alpha")]
        public AlphaDO? AlphaElement { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => Value != null;
        public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
        public bool ShouldSerializeCurve() => !string.IsNullOrEmpty(Curve);
        public bool ShouldSerializeParameterCurve() => ParameterCurve != null;
        public bool ShouldSerializeColorElement() => ColorElement != null;
        public bool ShouldSerializeAlphaElement() => AlphaElement != null;
    }

    public class CurveDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("version")]
        public string? Version { get; set; }

        [XmlAttribute("default")]
        public string? Default { get; set; }

        [XmlAttribute("curve_multiplier")]
        public string? CurveMultiplier { get; set; }

        [XmlElement("keys")]
        public KeysDO? Keys { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
        public bool ShouldSerializeDefault() => !string.IsNullOrEmpty(Default);
        public bool ShouldSerializeCurveMultiplier() => !string.IsNullOrEmpty(CurveMultiplier);
        public bool ShouldSerializeKeys() => Keys != null;
    }

    public class KeysDO
    {
        [XmlElement("key")]
        public List<KeyDO> KeyList { get; set; } = new List<KeyDO>();

        public bool ShouldSerializeKeyList() => KeyList != null && KeyList.Count > 0;
    }

    public class KeyDO
    {
        [XmlAttribute("time")]
        public string? Time { get; set; }

        [XmlAttribute("value")]
        public string? Value { get; set; }

        [XmlAttribute("position")]
        public string? Position { get; set; }

        [XmlAttribute("tangent")]
        public string? Tangent { get; set; }

        public bool ShouldSerializeTime() => !string.IsNullOrEmpty(Time);
        public bool ShouldSerializeValue() => Value != null;
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeTangent() => !string.IsNullOrEmpty(Tangent);
    }

    public class ColorDO
    {
        [XmlElement("keys")]
        public KeysDO? Keys { get; set; }

        public bool ShouldSerializeKeys() => Keys != null;
    }

    public class AlphaDO
    {
        [XmlElement("keys")]
        public KeysDO? Keys { get; set; }

        public bool ShouldSerializeKeys() => Keys != null;
    }
}