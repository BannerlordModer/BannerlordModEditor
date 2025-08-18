using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO;

// ParticleSystemsMapIcon.xml - Map icon particle effects system
[XmlRoot("particle_effects")]
public class ParticleSystemsMapIconDO
{
    [XmlElement("effect")]
    public List<ParticleEffectDO> Effects { get; set; } = new List<ParticleEffectDO>();
    
    [XmlIgnore]
    public bool HasEmptyEffects { get; set; }
    
    public bool ShouldSerializeEffects() => HasEmptyEffects || (Effects != null && Effects.Count > 0);
}

public class ParticleEffectDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("guid")]
    public string? Guid { get; set; }

    [XmlArray("emitters")]
    [XmlArrayItem("emitter")]
    public List<ParticleEmitterDO> Emitters { get; set; } = new List<ParticleEmitterDO>();
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeGuid() => !string.IsNullOrEmpty(Guid);
    public bool ShouldSerializeEmitters() => Emitters != null && Emitters.Count > 0;
}

public class ParticleEmitterDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("_index_")]
    public string? Index { get; set; }

    [XmlArray("flags")]
    [XmlArrayItem("flag")]
    public List<MapIconParticleFlagDO> Flags { get; set; } = new List<MapIconParticleFlagDO>();
    
    [XmlIgnore]
    public bool HasEmptyFlags { get; set; }

    [XmlArray("parameters")]
    [XmlArrayItem("parameter")]
    public List<ParticleParameterDO> Parameters { get; set; } = new List<ParticleParameterDO>();
    
    [XmlIgnore]
    public bool HasEmptyParameters { get; set; }

    [XmlArray("curves")]
    [XmlArrayItem("curve")]
    public List<ParticleCurveDO>? Curves { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyCurves { get; set; }

    [XmlElement("material")]
    public ParticleMaterialDO? Material { get; set; }
    
    [XmlIgnore]
    public bool HasMaterial { get; set; }
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);
    public bool ShouldSerializeFlags() => HasEmptyFlags || (Flags != null && Flags.Count > 0);
    public bool ShouldSerializeParameters() => HasEmptyParameters || (Parameters != null && Parameters.Count > 0);
    public bool ShouldSerializeCurves() => HasEmptyCurves || (Curves != null && Curves.Count > 0);
    public bool ShouldSerializeMaterial() => HasMaterial && Material != null;
}

public class MapIconParticleFlagDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class ParticleParameterDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlAttribute("base")]
    public string? Base { get; set; }

    [XmlAttribute("bias")]
    public string? Bias { get; set; }
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    public bool ShouldSerializeBase() => !string.IsNullOrEmpty(Base);
    public bool ShouldSerializeBias() => !string.IsNullOrEmpty(Bias);
}

public class ParticleCurveDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlArray("points")]
    [XmlArrayItem("point")]
    public List<CurvePointDO>? Points { get; set; }
    
    [XmlIgnore]
    public bool HasEmptyPoints { get; set; }
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializePoints() => HasEmptyPoints || (Points != null && Points.Count > 0);
}

public class CurvePointDO
{
    [XmlAttribute("x")]
    public string? X { get; set; }

    [XmlAttribute("y")]
    public string? Y { get; set; }
    
    public bool ShouldSerializeX() => !string.IsNullOrEmpty(X);
    public bool ShouldSerializeY() => !string.IsNullOrEmpty(Y);
}

public class ParticleMaterialDO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlElement("texture")]
    public string? Texture { get; set; }
    
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeTexture() => !string.IsNullOrEmpty(Texture);
}