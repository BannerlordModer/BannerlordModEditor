using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class CollisionInfosRootDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("collision_infos")]
        public CollisionInfosDO? CollisionInfos { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null;
    }

    public class CollisionInfosDO
    {
        [XmlElement("material")]
        public List<CollisionMaterialDO>? Materials { get; set; }

        [XmlElement("monster")]
        public List<CollisionMonsterDO>? Monsters { get; set; }

        public bool ShouldSerializeMaterials() => Materials != null && Materials.Count > 0;
        public bool ShouldSerializeMonsters() => Monsters != null && Monsters.Count > 0;
    }

    public class CollisionMaterialDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlElement("collision_info")]
        public List<CollisionInfoDO>? CollisionInfos { get; set; }

        public bool ShouldSerializeId() => Id != null;
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionMonsterDO
    {
        [XmlAttribute("sound_and_collision_info_class")]
        public string? SoundAndCollisionInfoClass { get; set; }

        [XmlElement("collision_info")]
        public List<CollisionInfoDO>? CollisionInfos { get; set; }

        public bool ShouldSerializeSoundAndCollisionInfoClass() => SoundAndCollisionInfoClass != null;
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionInfoDO
    {
        [XmlAttribute("second_material")]
        public string? SecondMaterial { get; set; }
        
        [XmlAttribute("fallback_material_for_effects")]
        public string? FallbackMaterialForEffects { get; set; }

        [XmlElement("collision_effect")]
        public List<CollisionEffectDO>? CollisionEffects { get; set; }

        public bool ShouldSerializeSecondMaterial() => SecondMaterial != null;
        public bool ShouldSerializeFallbackMaterialForEffects() => FallbackMaterialForEffects != null;
        public bool ShouldSerializeCollisionEffects() => CollisionEffects != null && CollisionEffects.Count > 0;
    }

    public class CollisionEffectDO
    {
        [XmlAttribute("effect_type")]
        public string? EffectType { get; set; }

        [XmlAttribute("particle")]
        public string? Particle { get; set; }

        [XmlAttribute("trail_particle")]
        public string? TrailParticle { get; set; }

        [XmlAttribute("splash_atlas_start_index")]
        public string? SplashAtlasStartIndex { get; set; }

        [XmlAttribute("splash_atlas_end_index")]
        public string? SplashAtlasEndIndex { get; set; }

        [XmlAttribute("decal")]
        public string? Decal { get; set; }

        [XmlAttribute("sound")]
        public string? Sound { get; set; }

        [XmlAttribute("do_not_send_armor_parameter_to_sound")]
        public string? DoNotSendArmorParameterToSound { get; set; }

        [XmlAttribute("upper_bound_of_size_parameter_for_sound")]
        public string? UpperBoundOfSizeParameterForSound { get; set; }
        
        [XmlAttribute("one_shot")]
        public string? OneShot { get; set; }

        public bool ShouldSerializeEffectType() => EffectType != null;
        public bool ShouldSerializeParticle() => Particle != null;
        public bool ShouldSerializeTrailParticle() => TrailParticle != null;
        public bool ShouldSerializeSplashAtlasStartIndex() => SplashAtlasStartIndex != null;
        public bool ShouldSerializeSplashAtlasEndIndex() => SplashAtlasEndIndex != null;
        public bool ShouldSerializeDecal() => Decal != null;
        public bool ShouldSerializeSound() => Sound != null;
        public bool ShouldSerializeDoNotSendArmorParameterToSound() => DoNotSendArmorParameterToSound != null;
        public bool ShouldSerializeUpperBoundOfSizeParameterForSound() => UpperBoundOfSizeParameterForSound != null;
        public bool ShouldSerializeOneShot() => OneShot != null;
    }
}