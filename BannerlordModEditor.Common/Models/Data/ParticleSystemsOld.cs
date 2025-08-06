using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ParticleSystemsOld
    {
        [XmlElement("particle_systems")]
        public ParticleSystems ParticleSystems { get; set; }
    }

    public class ParticleSystems
    {
        [XmlElement("particle_system")]
        public List<ParticleSystem> ParticleSystemList { get; set; }
    }

    public class ParticleSystem
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("low_quality_system")]
        public string LowQualitySystem { get; set; }

        [XmlAttribute("low_quality_system_distance")]
        public string LowQualitySystemDistance { get; set; }

        [XmlElement("emitters")]
        public Emitters Emitters { get; set; }

        public bool ShouldSerializeLowQualitySystem()
        {
            return !string.IsNullOrEmpty(LowQualitySystem);
        }

        public bool ShouldSerializeLowQualitySystemDistance()
        {
            return !string.IsNullOrEmpty(LowQualitySystemDistance);
        }
    }

    public class Emitters
    {
        [XmlElement("emitter")]
        public List<Emitter> EmitterList { get; set; }
    }

    public class Emitter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("sample_mesh")]
        public string SampleMesh { get; set; }

        [XmlAttribute("not_visible_on_low_config")]
        public string NotVisibleOnLowConfig { get; set; }

        [XmlAttribute("synch_with_master")]
        public string SynchWithMaster { get; set; }

        [XmlAttribute("fadeout_distance_level")]
        public string FadeoutDistanceLevel { get; set; }

        [XmlAttribute("emitter_type")]
        public string EmitterType { get; set; }

        [XmlAttribute("collision_event")]
        public string CollisionEvent { get; set; }

        [XmlAttribute("material")]
        public string Material { get; set; }

        [XmlAttribute("decal_material")]
        public string DecalMaterial { get; set; }

        [XmlAttribute("decal_min_scale")]
        public string DecalMinScale { get; set; }

        [XmlAttribute("decal_max_scale")]
        public string DecalMaxScale { get; set; }

        [XmlAttribute("decal_randomize_rotation")]
        public string DecalRandomizeRotation { get; set; }

        [XmlAttribute("creates_deletable_decals")]
        public string CreatesDeletableDecals { get; set; }

        [XmlAttribute("billboard")]
        public string Billboard { get; set; }

        [XmlAttribute("particles_per_sec")]
        public string ParticlesPerSec { get; set; }

        [XmlAttribute("burst_length")]
        public string BurstLength { get; set; }

        [XmlAttribute("particle_life")]
        public string ParticleLife { get; set; }

        [XmlAttribute("particle_life_deviation")]
        public string ParticleLifeDeviation { get; set; }

        [XmlAttribute("particle_size_deviation")]
        public string ParticleSizeDeviation { get; set; }

        [XmlAttribute("gravity")]
        public string Gravity { get; set; }

        [XmlAttribute("gravity_weight")]
        public string GravityWeight { get; set; }

        [XmlAttribute("turbulance_size")]
        public string TurbulanceSize { get; set; }

        [XmlAttribute("turbulance_strength")]
        public string TurbulanceStrength { get; set; }

        [XmlAttribute("starting_scale_time")]
        public string StartingScaleTime { get; set; }

        [XmlAttribute("starting_scale")]
        public string StartingScale { get; set; }

        [XmlAttribute("starting_color_time")]
        public string StartingColorTime { get; set; }

        [XmlAttribute("starting_alpha_time")]
        public string StartingAlphaTime { get; set; }

        [XmlAttribute("starting_color")]
        public string StartingColor { get; set; }

        [XmlAttribute("starting_alpha")]
        public string StartingAlpha { get; set; }

        [XmlAttribute("ending_scale_time")]
        public string EndingScaleTime { get; set; }

        [XmlAttribute("ending_scale")]
        public string EndingScale { get; set; }

        [XmlAttribute("ending_color_time")]
        public string EndingColorTime { get; set; }

        [XmlAttribute("ending_alpha_time")]
        public string EndingAlphaTime { get; set; }

        [XmlAttribute("ending_color")]
        public string EndingColor { get; set; }

        [XmlAttribute("ending_alpha")]
        public string EndingAlpha { get; set; }

        [XmlAttribute("emit_size")]
        public string EmitSize { get; set; }

        [XmlAttribute("emit_impulse")]
        public string EmitImpulse { get; set; }

        [XmlAttribute("emit_randomization")]
        public string EmitRandomization { get; set; }

        [XmlAttribute("damping")]
        public string Damping { get; set; }

        [XmlAttribute("rotation_speed")]
        public string RotationSpeed { get; set; }

        [XmlAttribute("rotational_damping")]
        public string RotationalDamping { get; set; }

        [XmlAttribute("scale_with_velocity_multiplier")]
        public string ScaleWithVelocityMultiplier { get; set; }

        [XmlAttribute("scale_with_emitters_velocity")]
        public string ScaleWithEmittersVelocity { get; set; }

        [XmlAttribute("activation_delay")]
        public string ActivationDelay { get; set; }

        [XmlAttribute("always_emit")]
        public string AlwaysEmit { get; set; }

        [XmlAttribute("global_emit_dir")]
        public string GlobalEmitDir { get; set; }

        [XmlAttribute("turbulance_2d")]
        public string Turbulance2D { get; set; }

        [XmlAttribute("randomize_size")]
        public string RandomizeSize { get; set; }

        [XmlAttribute("gradient_normals")]
        public string GradientNormals { get; set; }

        [XmlAttribute("rain_culling")]
        public string RainCulling { get; set; }

        [XmlAttribute("rain_splash")]
        public string RainSplash { get; set; }

        [XmlAttribute("scale_with_velocity")]
        public string ScaleWithVelocity { get; set; }

        [XmlAttribute("randomize_rotation")]
        public string RandomizeRotation { get; set; }

        [XmlAttribute("orderby_distance")]
        public string OrderByDistance { get; set; }

        [XmlAttribute("animated_sprite")]
        public string AnimatedSprite { get; set; }

        [XmlAttribute("sprite_count")]
        public string SpriteCount { get; set; }

        [XmlAttribute("texture_atlas_texture_counts")]
        public string TextureAtlasTextureCounts { get; set; }

        [XmlAttribute("sprite_frame_rate")]
        public string SpriteFrameRate { get; set; }

        [XmlAttribute("collide_with_environment")]
        public string CollideWithEnvironment { get; set; }

        [XmlAttribute("use_texture_atlas")]
        public string UseTextureAtlas { get; set; }

        [XmlAttribute("emit_on_terrain")]
        public string EmitOnTerrain { get; set; }

        [XmlAttribute("emit_water_level")]
        public string EmitWaterLevel { get; set; }

        [XmlElement("children")]
        public Children Children { get; set; }

        // 可选字段序列化控制（示例，实际可根据需要补充）
        public bool ShouldSerializeDecalMaterial() => !string.IsNullOrEmpty(DecalMaterial);
        public bool ShouldSerializeDecalMinScale() => !string.IsNullOrEmpty(DecalMinScale);
        public bool ShouldSerializeDecalMaxScale() => !string.IsNullOrEmpty(DecalMaxScale);
        public bool ShouldSerializeSpriteCount() => !string.IsNullOrEmpty(SpriteCount);
        public bool ShouldSerializeTextureAtlasTextureCounts() => !string.IsNullOrEmpty(TextureAtlasTextureCounts);
        public bool ShouldSerializeSpriteFrameRate() => !string.IsNullOrEmpty(SpriteFrameRate);
        public bool ShouldSerializeCollideWithEnvironment() => !string.IsNullOrEmpty(CollideWithEnvironment);
        public bool ShouldSerializeUseTextureAtlas() => !string.IsNullOrEmpty(UseTextureAtlas);
        public bool ShouldSerializeEmitOnTerrain() => !string.IsNullOrEmpty(EmitOnTerrain);
        public bool ShouldSerializeEmitWaterLevel() => !string.IsNullOrEmpty(EmitWaterLevel);
        public bool ShouldSerializeOrderByDistance() => !string.IsNullOrEmpty(OrderByDistance);
        public bool ShouldSerializeAnimatedSprite() => !string.IsNullOrEmpty(AnimatedSprite);
    }

    public class Children
    {
        // 目前仅空节点
    }
}