using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("base")]
    public class PhysicsMaterialsBase
    {
        [XmlElement("physics_materials")]
        public PhysicsMaterialsContainer PhysicsMaterials { get; set; } = new PhysicsMaterialsContainer();
    }

    public class PhysicsMaterialsContainer
    {
        [XmlElement("physics_material")]
        public List<PhysicsMaterial> PhysicsMaterial { get; set; } = new List<PhysicsMaterial>();
    }

    public class PhysicsMaterial
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("static_friction")]
        public string StaticFriction { get; set; } = string.Empty;

        [XmlAttribute("dynamic_friction")]
        public string DynamicFriction { get; set; } = string.Empty;

        [XmlAttribute("restitution")]
        public string Restitution { get; set; } = string.Empty;

        [XmlAttribute("softness")]
        public string? Softness { get; set; }

        [XmlAttribute("linear_damping")]
        public string LinearDamping { get; set; } = string.Empty;

        [XmlAttribute("angular_damping")]
        public string AngularDamping { get; set; } = string.Empty;

        [XmlAttribute("display_color")]
        public string DisplayColor { get; set; } = string.Empty;

        [XmlAttribute("rain_splashes_enabled")]
        public string? RainSplashesEnabled { get; set; }

        [XmlAttribute("flammable")]
        public string? Flammable { get; set; }

        [XmlAttribute("override_material_name_for_impact_sounds")]
        public string? OverrideMaterialNameForImpactSounds { get; set; }

        [XmlAttribute("dont_stick_missiles")]
        public string? DontStickMissiles { get; set; }

        [XmlAttribute("attacks_can_pass_through")]
        public string? AttacksCanPassThrough { get; set; }
    }
} 