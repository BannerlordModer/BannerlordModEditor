using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsGeneral
    {
        [XmlElement("effect")]
        public List<Effect> Effects { get; set; }

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

        public bool ShouldSerializeSoundCode() => !string.IsNullOrEmpty(SoundCode);

        [XmlElement("emitters")]
        public Emitters Emitters { get; set; }

        public bool ShouldSerializeEmitters() => Emitters != null;
    }

    public class Emitters
    {
        [XmlElement("emitter")]
        public List<Emitter> EmitterList { get; set; }

        public bool ShouldSerializeEmitterList() => EmitterList != null && EmitterList.Count > 0;
    }

    public class Emitter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("_index_")]
        public int Index { get; set; }

        [XmlElement("flags")]
        public Flags Flags { get; set; }

        [XmlElement("parameters")]
        public Parameters Parameters { get; set; }

        public bool ShouldSerializeFlags() => Flags != null;
        public bool ShouldSerializeParameters() => Parameters != null;
    }

    public class Flags
    {
        [XmlElement("flag")]
        public List<Flag> FlagList { get; set; }

        public bool ShouldSerializeFlagList() => FlagList != null && FlagList.Count > 0;
    }

    public class Flag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class Parameters
    {
        [XmlElement("parameter")]
        public List<Parameter> ParameterList { get; set; }

        public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
    }

    public class Parameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("base")]
        public string Base { get; set; }

        [XmlAttribute("bias")]
        public string Bias { get; set; }

        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);

        [XmlElement("curve")]
        public Curve Curve { get; set; }

        [XmlElement("color")]
        public ColorElement Color { get; set; }

        [XmlElement("alpha")]
        public AlphaElement Alpha { get; set; }

        public bool ShouldSerializeCurve() => Curve != null;
        public bool ShouldSerializeColor() => Color != null;
        public bool ShouldSerializeAlpha() => Alpha != null;
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

        public bool ShouldSerializeKeys() => Keys != null;
    }

    public class Keys
    {
        [XmlElement("key")]
        public List<Key> KeyList { get; set; }

        public bool ShouldSerializeKeyList() => KeyList != null && KeyList.Count > 0;
    }

    public class Key
    {
        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("tangent")]
        public string Tangent { get; set; }
    }

    public class ColorElement
    {
        [XmlElement("keys")]
        public Keys Keys { get; set; }

        public bool ShouldSerializeKeys() => Keys != null;
    }

    public class AlphaElement
    {
        [XmlElement("keys")]
        public Keys Keys { get; set; }

        public bool ShouldSerializeKeys() => Keys != null;
    }
}