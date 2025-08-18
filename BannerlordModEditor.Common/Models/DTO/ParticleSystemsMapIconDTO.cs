using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO;

// ParticleSystemsMapIcon.xml - Map icon particle effects system (DTO)
[XmlRoot("particle_effects")]
public class ParticleSystemsMapIconDTO
{
    [XmlElement("effect")]
    public List<ParticleEffectDTO> Effects { get; set; } = new List<ParticleEffectDTO>();
}

public class ParticleEffectDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("guid")]
    public string? Guid { get; set; }

    [XmlArray("emitters")]
    [XmlArrayItem("emitter")]
    public List<ParticleEmitterDTO> Emitters { get; set; } = new List<ParticleEmitterDTO>();
}

public class ParticleEmitterDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("_index_")]
    public string? Index { get; set; }

    [XmlArray("flags")]
    [XmlArrayItem("flag")]
    public List<MapIconParticleFlagDTO> Flags { get; set; } = new List<MapIconParticleFlagDTO>();

    [XmlArray("parameters")]
    [XmlArrayItem("parameter")]
    public List<ParticleParameterDTO> Parameters { get; set; } = new List<ParticleParameterDTO>();

    [XmlArray("curves")]
    [XmlArrayItem("curve")]
    public List<ParticleCurveDTO>? Curves { get; set; }

    [XmlElement("material")]
    public ParticleMaterialDTO? Material { get; set; }
}

public class MapIconParticleFlagDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
}

public class ParticleParameterDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlAttribute("base")]
    public string? Base { get; set; }

    [XmlAttribute("bias")]
    public string? Bias { get; set; }
}

public class ParticleCurveDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlArray("points")]
    [XmlArrayItem("point")]
    public List<CurvePointDTO>? Points { get; set; }
}

public class CurvePointDTO
{
    [XmlAttribute("x")]
    public string? X { get; set; }

    [XmlAttribute("y")]
    public string? Y { get; set; }
}

public class ParticleMaterialDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlElement("texture")]
    public string? Texture { get; set; }
}