using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Engine;

// gpu_particle_systems.xml - GPU particle system definitions
[XmlRoot("base")]
public class GpuParticleSystemsBase
{
    [XmlAttribute("type")]
    public string Type { get; set; } = "particle_system";

    [XmlElement("particle_systems")]
    public ParticleSystemsContainer ParticleSystems { get; set; } = new ParticleSystemsContainer();
}

public class ParticleSystemsContainer
{
    [XmlElement("particle_system")]
    public List<ParticleSystem> ParticleSystem { get; set; } = new List<ParticleSystem>();
}

public class ParticleSystem
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
} 