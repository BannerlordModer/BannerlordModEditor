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
        [DefaultValue(false)]
        public bool DontStickMissiles { get; set; }

        [XmlAttribute("attacks_can_pass_through")]
        public string? AttacksCanPassThrough { get; set; }

        [XmlAttribute("rain_splashes_enabled")]
        [DefaultValue(false)]
        public bool RainSplashesEnabled { get; set; }

        [XmlAttribute("flammable")]
        [DefaultValue(false)]
        public bool Flammable { get; set; }

        [XmlAttribute("static_friction")]
        public float StaticFriction { get; set; }

        [XmlAttribute("dynamic_friction")]
        public float DynamicFriction { get; set; }

        [XmlAttribute("restitution")]
        public float Restitution { get; set; }

        [XmlAttribute("softness")]
        [DefaultValue(0.0f)]
        public float Softness { get; set; }

        [XmlAttribute("linear_damping")]
        public float LinearDamping { get; set; }

        [XmlAttribute("angular_damping")]
        public float AngularDamping { get; set; }

        [XmlAttribute("display_color")]
        public string? DisplayColor { get; set; }
    }

    public class SoundAndCollisionInfoClassDefinition
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 