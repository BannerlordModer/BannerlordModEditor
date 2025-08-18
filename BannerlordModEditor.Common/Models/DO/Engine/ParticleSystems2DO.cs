using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Engine
{
    [XmlRoot("particle_systems")]
    public class ParticleSystems2DO
    {
        [XmlElement("particle_system")]
        public List<ParticleSystem2DO> ParticleSystems { get; set; } = new List<ParticleSystem2DO>();

        [XmlIgnore]
        public bool HasParticleSystems { get; set; } = false;

        public bool ShouldSerializeParticleSystems() => HasParticleSystems && ParticleSystems != null && ParticleSystems.Count > 0;
    }

    public class ParticleSystem2DO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("version")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("emitter")]
        public Emitter2DO Emitter { get; set; } = new Emitter2DO();

        [XmlIgnore]
        public bool HasEmitter { get; set; } = false;

        public bool ShouldSerializeEmitter() => HasEmitter && Emitter != null;
    }

    public class Emitter2DO
    {
        [XmlElement("flags")]
        public Flags2DO Flags { get; set; } = new Flags2DO();

        [XmlElement("properties")]
        public Properties2DO Properties { get; set; } = new Properties2DO();

        [XmlIgnore]
        public bool HasFlags { get; set; } = false;

        [XmlIgnore]
        public bool HasProperties { get; set; } = false;

        public bool ShouldSerializeFlags() => HasFlags && Flags != null && Flags.Flags.Count > 0;
        public bool ShouldSerializeProperties() => HasProperties && Properties != null && Properties.Properties.Count > 0;
    }

    public class Flags2DO
    {
        [XmlElement("flag")]
        public List<Flag2DO> Flags { get; set; } = new List<Flag2DO>();
    }

    public class Flag2DO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }

    public class Properties2DO
    {
        [XmlElement("property")]
        public List<Property2DO> Properties { get; set; } = new List<Property2DO>();
    }

    public class Property2DO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}