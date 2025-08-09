using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("base")]
    public class PhysicsMaterials
    {
        [XmlArray("physics_materials")]
        [XmlArrayItem("physics_material")]
        public List<PhysicsMaterial> PhysicsMaterialList { get; set; } = new List<PhysicsMaterial>();

        [XmlArray("sound_and_collision_info_class_definitions")]
        [XmlArrayItem("sound_and_collision_info_class_definition")]
        public List<SoundAndCollisionInfoClassDefinition> SoundAndCollisionInfoClassDefinitions { get; set; } = new List<SoundAndCollisionInfoClassDefinition>();
    }

    public class PhysicsMaterial
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("override_material_name_for_impact_sounds")]
        public string? OverrideMaterialNameForImpactSounds { get; set; }

        [XmlAttribute("dont_stick_missiles")]
        public string? DontStickMissiles { get; set; }

        [XmlAttribute("attacks_can_pass_through")]
        public string? AttacksCanPassThrough { get; set; }

        [XmlAttribute("rain_splashes_enabled")]
        public string? RainSplashesEnabled { get; set; }

        [XmlAttribute("flammable")]
        public string? Flammable { get; set; }

        public bool ShouldSerializeDontStickMissiles() => !string.IsNullOrWhiteSpace(DontStickMissiles);
        public bool ShouldSerializeRainSplashesEnabled() => !string.IsNullOrWhiteSpace(RainSplashesEnabled);
        public bool ShouldSerializeFlammable() => !string.IsNullOrWhiteSpace(Flammable);

        [XmlAttribute("static_friction")]
        public string StaticFriction { get; set; } = string.Empty;

        [XmlAttribute("dynamic_friction")]
        public string DynamicFriction { get; set; } = string.Empty;

        [XmlAttribute("restitution")]
        public string Restitution { get; set; } = string.Empty;

        [XmlAttribute("softness")]
        public string Softness { get; set; } = string.Empty;

        [XmlAttribute("linear_damping")]
        public string LinearDamping { get; set; } = string.Empty;

        [XmlAttribute("angular_damping")]
        public string AngularDamping { get; set; } = string.Empty;

        [XmlAttribute("display_color")]
        public string? DisplayColor { get; set; }
    }

    public class SoundAndCollisionInfoClassDefinition
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 