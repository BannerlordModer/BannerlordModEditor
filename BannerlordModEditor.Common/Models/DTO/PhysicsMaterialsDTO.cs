using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("base")]
    public class PhysicsMaterialsDTO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "physics_materials";

        [XmlElement("physics_materials")]
        public PhysicsMaterialsContainerDTO PhysicsMaterials { get; set; } = new PhysicsMaterialsContainerDTO();

        [XmlElement("sound_and_collision_info_class_definitions")]
        public SoundAndCollisionInfoClassDefinitionsDTO SoundAndCollisionInfoClassDefinitions { get; set; } = new SoundAndCollisionInfoClassDefinitionsDTO();
    }

    public class PhysicsMaterialsContainerDTO
    {
        [XmlElement("physics_material")]
        public List<PhysicsMaterialDTO> Materials { get; set; } = new List<PhysicsMaterialDTO>();
    }

    public class PhysicsMaterialDTO
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
        public string Softness { get; set; } = string.Empty;

        [XmlAttribute("linear_damping")]
        public string LinearDamping { get; set; } = string.Empty;

        [XmlAttribute("angular_damping")]
        public string AngularDamping { get; set; } = string.Empty;

        [XmlAttribute("display_color")]
        public string DisplayColor { get; set; } = string.Empty;

        [XmlAttribute("rain_splashes_enabled")]
        public string RainSplashesEnabled { get; set; } = string.Empty;

        [XmlAttribute("flammable")]
        public string Flammable { get; set; } = string.Empty;

        [XmlAttribute("override_material_name_for_impact_sounds")]
        public string OverrideMaterialNameForImpactSounds { get; set; } = string.Empty;

        [XmlAttribute("dont_stick_missiles")]
        public string DontStickMissiles { get; set; } = string.Empty;

        [XmlAttribute("attacks_can_pass_through")]
        public string AttacksCanPassThrough { get; set; } = string.Empty;
    }

    public class SoundAndCollisionInfoClassDefinitionsDTO
    {
        [XmlElement("sound_and_collision_info_class_definition")]
        public List<SoundAndCollisionInfoClassDefinitionDTO> Definitions { get; set; } = new List<SoundAndCollisionInfoClassDefinitionDTO>();
    }

    public class SoundAndCollisionInfoClassDefinitionDTO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
}