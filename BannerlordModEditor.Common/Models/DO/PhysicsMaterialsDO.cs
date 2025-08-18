using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class PhysicsMaterialsDO
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "physics_materials";

        [XmlElement("physics_materials")]
        public PhysicsMaterialsContainerDO PhysicsMaterials { get; set; } = new PhysicsMaterialsContainerDO();

        [XmlElement("sound_and_collision_info_class_definitions")]
        public SoundAndCollisionInfoClassDefinitionsDO SoundAndCollisionInfoClassDefinitions { get; set; } = new SoundAndCollisionInfoClassDefinitionsDO();

        [XmlIgnore]
        public bool HasPhysicsMaterials { get; set; } = false;

        [XmlIgnore]
        public bool HasSoundAndCollisionInfoClassDefinitions { get; set; } = false;

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializePhysicsMaterials() => HasPhysicsMaterials && PhysicsMaterials != null && PhysicsMaterials.Materials.Count > 0;
        public bool ShouldSerializeSoundAndCollisionInfoClassDefinitions() => HasSoundAndCollisionInfoClassDefinitions && SoundAndCollisionInfoClassDefinitions != null && SoundAndCollisionInfoClassDefinitions.Definitions.Count > 0;
    }

    public class PhysicsMaterialsContainerDO
    {
        [XmlElement("physics_material")]
        public List<PhysicsMaterialDO> Materials { get; set; } = new List<PhysicsMaterialDO>();
    }

    public class PhysicsMaterialDO
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

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeStaticFriction() => !string.IsNullOrEmpty(StaticFriction);
        public bool ShouldSerializeDynamicFriction() => !string.IsNullOrEmpty(DynamicFriction);
        public bool ShouldSerializeRestitution() => !string.IsNullOrEmpty(Restitution);
        public bool ShouldSerializeSoftness() => !string.IsNullOrEmpty(Softness);
        public bool ShouldSerializeLinearDamping() => !string.IsNullOrEmpty(LinearDamping);
        public bool ShouldSerializeAngularDamping() => !string.IsNullOrEmpty(AngularDamping);
        public bool ShouldSerializeDisplayColor() => !string.IsNullOrEmpty(DisplayColor);
        public bool ShouldSerializeRainSplashesEnabled() => !string.IsNullOrEmpty(RainSplashesEnabled);
        public bool ShouldSerializeFlammable() => !string.IsNullOrEmpty(Flammable);
        public bool ShouldSerializeOverrideMaterialNameForImpactSounds() => !string.IsNullOrEmpty(OverrideMaterialNameForImpactSounds);
        public bool ShouldSerializeDontStickMissiles() => !string.IsNullOrEmpty(DontStickMissiles);
        public bool ShouldSerializeAttacksCanPassThrough() => !string.IsNullOrEmpty(AttacksCanPassThrough);
    }

    public class SoundAndCollisionInfoClassDefinitionsDO
    {
        [XmlElement("sound_and_collision_info_class_definition")]
        public List<SoundAndCollisionInfoClassDefinitionDO> Definitions { get; set; } = new List<SoundAndCollisionInfoClassDefinitionDO>();
    }

    public class SoundAndCollisionInfoClassDefinitionDO
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}