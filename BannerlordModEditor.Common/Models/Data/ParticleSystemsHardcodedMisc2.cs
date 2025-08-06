using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsHardcodedMisc2
    {
        [XmlElement("effect")]
        public List<Effect> Effects { get; set; }
        public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
    }

    public class Effect
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("guid")]
        public string Guid { get; set; }
        public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);

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
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("_index_")]
        public int? Index { get; set; }
        public bool ShouldSerializeIndex() => Index.HasValue;

        [XmlElement("flags")]
        public Flags Flags { get; set; }
        public bool ShouldSerializeFlags() => Flags != null;

        [XmlElement("parameters")]
        public Parameters Parameters { get; set; }
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
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("value")]
        public string Value { get; set; }
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
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
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("value")]
        public string Value { get; set; }
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);

        [XmlAttribute("base")]
        public string Base { get; set; }
        public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);

        [XmlAttribute("bias")]
        public string Bias { get; set; }
        public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);

        [XmlElement("curve")]
        public Curve Curve { get; set; }
        public bool ShouldSerializeCurve() => Curve != null;

        [XmlElement("color")]
        public ColorElement Color { get; set; }
        public bool ShouldSerializeColor() => Color != null;

        [XmlElement("alpha")]
        public AlphaElement Alpha { get; set; }
        public bool ShouldSerializeAlpha() => Alpha != null;
    }

    public class Curve
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("version")]
        public string Version { get; set; }
        public bool ShouldSerializeVersion() => !string.IsNullOrEmpty(Version);

        [XmlAttribute("default")]
        public string Default { get; set; }
        public bool ShouldSerializeDefault() => !string.IsNullOrEmpty(Default);

        [XmlAttribute("curve_multiplier")]
        public string CurveMultiplier { get; set; }
        public bool ShouldSerializeCurveMultiplier() => !string.IsNullOrEmpty(CurveMultiplier);

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
        public bool ShouldSerializeTime() => !string.IsNullOrEmpty(Time);

        [XmlAttribute("value")]
        public string Value { get; set; }
        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);

        [XmlAttribute("tangent")]
        public string Tangent { get; set; }
        public bool ShouldSerializeTangent() => !string.IsNullOrEmpty(Tangent);
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