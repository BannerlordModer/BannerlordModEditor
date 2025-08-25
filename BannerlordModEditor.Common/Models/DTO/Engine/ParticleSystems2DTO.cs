using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Engine
{
    [XmlRoot("particle_systems")]
    public class ParticleSystems2DTO
    {
        [XmlElement("particle_system")]
        public List<ParticleSystem2DTO> ParticleSystems { get; set; } = new List<ParticleSystem2DTO>();
    }

    public class ParticleSystem2DTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("version")]
        public string Version { get; set; } = string.Empty;

        [XmlElement("emitter")]
        public Emitter2DTO Emitter { get; set; } = new Emitter2DTO();
    }

    public class Emitter2DTO
    {
        [XmlElement("flags")]
        public Flags2DTO Flags { get; set; } = new Flags2DTO();

        [XmlElement("properties")]
        public Properties2DTO Properties { get; set; } = new Properties2DTO();
    }

    public class Flags2DTO
    {
        [XmlElement("flag")]
        public List<Flag2DTO> Flags { get; set; } = new List<Flag2DTO>();
    }

    public class Flag2DTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }

    public class Properties2DTO
    {
        [XmlElement("property")]
        public List<Property2DTO> Properties { get; set; } = new List<Property2DTO>();
    }

    public class Property2DTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }
}