using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class GpuParticleSystems
    {
        [XmlElement("particle_systems")]
        public ParticleSystemList ParticleSystems { get; set; }

        public class ParticleSystemList
        {
            [XmlElement("particle_system")]
            public List<ParticleSystem> Items { get; set; }
        }

        public class ParticleSystem
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("material")]
            public string Material { get; set; }

            [XmlAttribute("life")]
            public float Life { get; set; }

            [XmlAttribute("emission_rate")]
            public float EmissionRate { get; set; }

            [XmlAttribute("emit_velocity")]
            public string EmitVelocity { get; set; }

            [XmlAttribute("emit_direction_randomness")]
            public string EmitDirectionRandomness { get; set; }

            [XmlAttribute("emit_velocity_randomness")]
            public float EmitVelocityRandomness { get; set; }

            [XmlAttribute("emit_sphere_radius")]
            public float EmitSphereRadius { get; set; }

            [XmlAttribute("scale_t0")]
            public float ScaleT0 { get; set; }

            [XmlAttribute("scale_t1")]
            public float ScaleT1 { get; set; }

            [XmlAttribute("start_scale")]
            public float StartScale { get; set; }

            [XmlAttribute("end_scale")]
            public float EndScale { get; set; }

            [XmlAttribute("alpha_t0")]
            public float AlphaT0 { get; set; }

            [XmlAttribute("alpha_t1")]
            public float AlphaT1 { get; set; }

            [XmlAttribute("linear_damping")]
            public float LinearDamping { get; set; }

            [XmlAttribute("gravity_constant")]
            public float GravityConstant { get; set; }

            [XmlAttribute("angular_speed")]
            public float AngularSpeed { get; set; }

            [XmlAttribute("angular_damping")]
            public float AngularDamping { get; set; }

            [XmlAttribute("burst_length")]
            public float BurstLength { get; set; }

            [XmlAttribute("is_burst")]
            public bool IsBurst { get; set; }
        }
    }
}