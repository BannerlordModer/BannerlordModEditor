using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsBasic
    {
        [XmlElement("effect")]
        public List<Effect> Effects { get; set; } = new List<Effect>();
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
    }

    public class Flags
    {
        [XmlElement("flag")]
        public List<Flag> FlagList { get; set; } = new List<Flag>();
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
        public List<Parameter> ParameterList { get; set; } = new List<Parameter>();
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

        [XmlElement("curve")]
        public Curve Curve { get; set; }

        [XmlElement("color")]
        public ColorKeys Color { get; set; }

        [XmlElement("alpha")]
        public AlphaKeys Alpha { get; set; }

        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
        public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
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
        public CurveKeys Keys { get; set; }
    }

    public class CurveKeys
    {
        [XmlElement("key")]
        public List<CurveKey> KeyList { get; set; } = new List<CurveKey>();
    }

    public class CurveKey
    {
        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlAttribute("tangent")]
        public string Tangent { get; set; }
    }

    public class ColorKeys
    {
        [XmlElement("keys")]
        public ColorKeyList Keys { get; set; }
    }

    public class ColorKeyList
    {
        [XmlElement("key")]
        public List<ColorKey> KeyList { get; set; } = new List<ColorKey>();
    }

    public class ColorKey
    {
        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    public class AlphaKeys
    {
        [XmlElement("keys")]
        public AlphaKeyList Keys { get; set; }
    }

    public class AlphaKeyList
    {
        [XmlElement("key")]
        public List<AlphaKey> KeyList { get; set; } = new List<AlphaKey>();
    }

    public class AlphaKey
    {
        [XmlAttribute("time")]
        public string Time { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}