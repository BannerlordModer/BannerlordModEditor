using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("particle_effects")]
    public class ParticleSystemsHardcodedMisc1
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

        [XmlElement("emitters")]
        public Emitters Emitters { get; set; }

        public bool ShouldSerializeEmitters() => Emitters != null;
    }

    public class Emitters
    {
        [XmlElement("emitter")]
        public List<Emitter> EmittersList { get; set; } = new List<Emitter>();

        public bool ShouldSerializeEmittersList() => EmittersList != null && EmittersList.Count > 0;
    }

    public class Emitter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("_index_")]
        public int? Index { get; set; }

        [XmlElement("children")]
        public Children Children { get; set; }

        [XmlElement("flags")]
        public Flags Flags { get; set; }

        [XmlElement("parameters")]
        public Parameters Parameters { get; set; }

        public bool ShouldSerializeIndex() => Index.HasValue;
        public bool ShouldSerializeChildren() => Children != null;
        public bool ShouldSerializeFlags() => Flags != null;
        public bool ShouldSerializeParameters() => Parameters != null;
    }

    public class Children
    {
        [XmlElement("emitter")]
        public List<Emitter> Emitters { get; set; } = new List<Emitter>();

        public bool ShouldSerializeEmitters() => Emitters != null && Emitters.Count > 0;
    }

    public class Flags
    {
        [XmlElement("flag")]
        public List<Flag> FlagList { get; set; } = new List<Flag>();

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
        public List<Parameter> ParameterList { get; set; } = new List<Parameter>();

        public bool ShouldSerializeParameterList() => ParameterList != null && ParameterList.Count > 0;
    }

    public class Parameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}