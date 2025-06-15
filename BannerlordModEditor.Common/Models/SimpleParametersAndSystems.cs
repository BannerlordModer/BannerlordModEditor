using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    // managed_campaign_parameters.xml
    [XmlRoot("base")]
    public class ManagedCampaignParametersBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "campaign_parameters";

        [XmlElement("managed_campaign_parameters")]
        public ManagedCampaignParametersContainer ManagedCampaignParameters { get; set; } = new ManagedCampaignParametersContainer();
    }

    public class ManagedCampaignParametersContainer
    {
        [XmlElement("managed_campaign_parameter")]
        public List<ManagedCampaignParameter> ManagedCampaignParameter { get; set; } = new List<ManagedCampaignParameter>();
    }

    public class ManagedCampaignParameter
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("value")]
        public string Value { get; set; } = string.Empty;
    }

    // gpu_particle_systems.xml
    [XmlRoot("base")]
    public class GpuParticleSystemsBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "particle_system";

        [XmlElement("particle_systems")]
        public GpuParticleSystemsContainer ParticleSystems { get; set; } = new GpuParticleSystemsContainer();
    }

    public class GpuParticleSystemsContainer
    {
        [XmlElement("particle_system")]
        public List<GpuParticleSystem> ParticleSystem { get; set; } = new List<GpuParticleSystem>();
    }

    public class GpuParticleSystem
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("material")]
        public string Material { get; set; } = string.Empty;

        [XmlAttribute("life")]
        public string Life { get; set; } = string.Empty;

        [XmlAttribute("emission_rate")]
        public string EmissionRate { get; set; } = string.Empty;

        [XmlAttribute("emit_velocity")]
        public string EmitVelocity { get; set; } = string.Empty;

        [XmlAttribute("emit_direction_randomness")]
        public string EmitDirectionRandomness { get; set; } = string.Empty;

        [XmlAttribute("emit_velocity_randomness")]
        public string EmitVelocityRandomness { get; set; } = string.Empty;

        [XmlAttribute("emit_sphere_radius")]
        public string EmitSphereRadius { get; set; } = string.Empty;

        [XmlAttribute("scale_t0")]
        public string ScaleT0 { get; set; } = string.Empty;

        [XmlAttribute("scale_t1")]
        public string ScaleT1 { get; set; } = string.Empty;

        [XmlAttribute("start_scale")]
        public string StartScale { get; set; } = string.Empty;

        [XmlAttribute("end_scale")]
        public string EndScale { get; set; } = string.Empty;

        [XmlAttribute("alpha_t0")]
        public string AlphaT0 { get; set; } = string.Empty;

        [XmlAttribute("alpha_t1")]
        public string AlphaT1 { get; set; } = string.Empty;

        [XmlAttribute("linear_damping")]
        public string LinearDamping { get; set; } = string.Empty;

        [XmlAttribute("gravity_constant")]
        public string GravityConstant { get; set; } = string.Empty;

        [XmlAttribute("angular_speed")]
        public string AngularSpeed { get; set; } = string.Empty;

        [XmlAttribute("angular_damping")]
        public string AngularDamping { get; set; } = string.Empty;

        [XmlAttribute("burst_length")]
        public string BurstLength { get; set; } = string.Empty;

        [XmlAttribute("is_burst")]
        public string IsBurst { get; set; } = string.Empty;
    }

    // special_meshes.xml
    [XmlRoot("base")]
    public class SpecialMeshesBase
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "special_meshes";

        [XmlElement("meshes")]
        public SpecialMeshesContainer Meshes { get; set; } = new SpecialMeshesContainer();
    }

    public class SpecialMeshesContainer
    {
        [XmlElement("mesh")]
        public List<SpecialMesh> Mesh { get; set; } = new List<SpecialMesh>();
    }

    public class SpecialMesh
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("types")]
        public SpecialMeshTypes? Types { get; set; }
    }

    public class SpecialMeshTypes
    {
        [XmlElement("type")]
        public List<SpecialMeshType> Type { get; set; } = new List<SpecialMeshType>();
    }

    public class SpecialMeshType
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 