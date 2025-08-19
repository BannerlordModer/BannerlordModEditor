using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO;

[XmlRoot("base")]
public class GpuParticleSystemsDTO
{
    [XmlAttribute("type")]
    public string? Type { get; set; }

    [XmlElement("particle_systems")]
    public ParticleSystemsContainerDTO? ParticleSystems { get; set; } = new ParticleSystemsContainerDTO();

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeParticleSystems() => ParticleSystems != null && ParticleSystems.Items.Count > 0;
}

public class ParticleSystemsContainerDTO
{
    [XmlElement("particle_system")]
    public List<ParticleSystemDTO> Items { get; set; } = new List<ParticleSystemDTO>();
}

public class ParticleSystemDTO
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("material")]
    public string? Material { get; set; }

    [XmlAttribute("life")]
    public string? Life { get; set; }

    [XmlAttribute("emission_rate")]
    public string? EmissionRate { get; set; }

    [XmlAttribute("emit_velocity")]
    public string? EmitVelocity { get; set; }

    [XmlAttribute("emit_direction_randomness")]
    public string? EmitDirectionRandomness { get; set; }

    [XmlAttribute("emit_velocity_randomness")]
    public string? EmitVelocityRandomness { get; set; }

    [XmlAttribute("emit_sphere_radius")]
    public string? EmitSphereRadius { get; set; }

    [XmlAttribute("scale_t0")]
    public string? ScaleT0 { get; set; }

    [XmlAttribute("scale_t1")]
    public string? ScaleT1 { get; set; }

    [XmlAttribute("start_scale")]
    public string? StartScale { get; set; }

    [XmlAttribute("end_scale")]
    public string? EndScale { get; set; }

    [XmlAttribute("alpha_t0")]
    public string? AlphaT0 { get; set; }

    [XmlAttribute("alpha_t1")]
    public string? AlphaT1 { get; set; }

    [XmlAttribute("linear_damping")]
    public string? LinearDamping { get; set; }

    [XmlAttribute("gravity_constant")]
    public string? GravityConstant { get; set; }

    [XmlAttribute("angular_speed")]
    public string? AngularSpeed { get; set; }

    [XmlAttribute("angular_damping")]
    public string? AngularDamping { get; set; }

    [XmlAttribute("burst_length")]
    public string? BurstLength { get; set; }

    [XmlAttribute("is_burst")]
    public string? IsBurst { get; set; }

    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeMaterial() => !string.IsNullOrEmpty(Material);
    public bool ShouldSerializeLife() => !string.IsNullOrEmpty(Life);
    public bool ShouldSerializeEmissionRate() => !string.IsNullOrEmpty(EmissionRate);
    public bool ShouldSerializeEmitVelocity() => !string.IsNullOrEmpty(EmitVelocity);
    public bool ShouldSerializeEmitDirectionRandomness() => !string.IsNullOrEmpty(EmitDirectionRandomness);
    public bool ShouldSerializeEmitVelocityRandomness() => !string.IsNullOrEmpty(EmitVelocityRandomness);
    public bool ShouldSerializeEmitSphereRadius() => !string.IsNullOrEmpty(EmitSphereRadius);
    public bool ShouldSerializeScaleT0() => !string.IsNullOrEmpty(ScaleT0);
    public bool ShouldSerializeScaleT1() => !string.IsNullOrEmpty(ScaleT1);
    public bool ShouldSerializeStartScale() => !string.IsNullOrEmpty(StartScale);
    public bool ShouldSerializeEndScale() => !string.IsNullOrEmpty(EndScale);
    public bool ShouldSerializeAlphaT0() => !string.IsNullOrEmpty(AlphaT0);
    public bool ShouldSerializeAlphaT1() => !string.IsNullOrEmpty(AlphaT1);
    public bool ShouldSerializeLinearDamping() => !string.IsNullOrEmpty(LinearDamping);
    public bool ShouldSerializeGravityConstant() => !string.IsNullOrEmpty(GravityConstant);
    public bool ShouldSerializeAngularSpeed() => !string.IsNullOrEmpty(AngularSpeed);
    public bool ShouldSerializeAngularDamping() => !string.IsNullOrEmpty(AngularDamping);
    public bool ShouldSerializeBurstLength() => !string.IsNullOrEmpty(BurstLength);
    public bool ShouldSerializeIsBurst() => !string.IsNullOrEmpty(IsBurst);
}