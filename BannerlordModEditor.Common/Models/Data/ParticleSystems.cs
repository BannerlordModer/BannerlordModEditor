using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("particle_effects")]
    public class ParticleSystems
    {
        [XmlElement("effect")]
        public List<Effect> Effects { get; set; } = new List<Effect>();

        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }

    public class Effect
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("guid")]
        public string Guid { get; set; }

        [XmlAttribute("sound_code")]
        public string SoundCode { get; set; }

        [XmlElement("emitters")]
        public Emitters Emitters { get; set; }

        public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);
    }

    public class Emitters
    {
        [XmlElement("emitter")]
        public List<Emitter> EmitterList { get; set; } = new List<Emitter>();

        public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
    }

    public class Emitter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("_index_")]
        public int Index { get; set; }

        [XmlElement("flags")]
        public ParticleFlags Flags { get; set; }

        [XmlElement("parameters")]
        public Parameters Parameters { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeFlags() => Flags != null;
        public bool ShouldSerializeParameters() => Parameters != null;
    }

    public class ParticleFlags
    {
        [XmlElement("flag")]
        public List<ParticleFlag> FlagList { get; set; } = new List<ParticleFlag>();

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class ParticleFlag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    public class Parameters
    {
        [XmlElement("parameter")]
        public List<Parameter> ParameterList { get; set; } = new List<Parameter>();

        public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
    }

    public class Parameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("bias")]
        public string Bias { get; set; }

        [XmlAttribute("curve")]
        public string Curve { get; set; }

        [XmlAttribute("color")]
        public string Color { get; set; }

        [XmlAttribute("alpha")]
        public string Alpha { get; set; }

        [XmlElement("curve")]
        public Curve ParameterCurve { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
        public bool ShouldSerializeCurve() => !string.IsNullOrEmpty(Curve);
        public bool ShouldSerializeColor() => !string.IsNullOrEmpty(Color);
        public bool ShouldSerializeAlpha() => !string.IsNullOrEmpty(Alpha);
        public bool ShouldSerializeParameterCurve() => ParameterCurve != null;
    }

    public class Curve
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("default")]
        public string Default { get; set; }

        [XmlAttribute("curve_multiplier")]
        public string CurveMultiplier { get; set; }

        [XmlElement("keys")]
        public Keys Keys { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);
        public bool ShouldSerializeDefault() => !string.IsNullOrEmpty(Default);
        public bool ShouldSerializeCurveMultiplier() => !string.IsNullOrEmpty(CurveMultiplier);
        public bool ShouldSerializeKeys() => Keys != null;
    }

    public class Keys
    {
        [XmlElement("key")]
        public List<Key> KeyList { get; set; } = new List<Key>();

        public bool ShouldSerializeKeyList() => KeyList != null && KeyList.Count > 0;
    }

    public class Key
    {
        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("position")]
        public string Position { get; set; }

        [XmlAttribute("tangent")]
        public string Tangent { get; set; }

        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializePosition() => !string.IsNullOrEmpty(Position);
        public bool ShouldSerializeTangent() => !string.IsNullOrEmpty(Tangent);
    }
}