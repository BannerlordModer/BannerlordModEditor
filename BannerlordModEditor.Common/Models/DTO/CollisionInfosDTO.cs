using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class CollisionInfosRootDTO
    {
        public string? Type { get; set; }
        public CollisionInfosDTO? CollisionInfos { get; set; }

        public bool ShouldSerializeType() => Type != null;
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null;
    }

    public class CollisionInfosDTO
    {
        public List<CollisionMaterialDTO>? Materials { get; set; }
        public List<CollisionMonsterDTO>? Monsters { get; set; }

        public bool ShouldSerializeMaterials() => Materials != null && Materials.Count > 0;
        public bool ShouldSerializeMonsters() => Monsters != null && Monsters.Count > 0;
    }

    public class CollisionMaterialDTO
    {
        public string? Id { get; set; }
        public List<CollisionInfoDTO>? CollisionInfos { get; set; }

        public bool ShouldSerializeId() => Id != null;
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionMonsterDTO
    {
        public string? SoundAndCollisionInfoClass { get; set; }
        public List<CollisionInfoDTO>? CollisionInfos { get; set; }

        public bool ShouldSerializeSoundAndCollisionInfoClass() => SoundAndCollisionInfoClass != null;
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionInfoDTO
    {
        public string? SecondMaterial { get; set; }
        public string? FallbackMaterialForEffects { get; set; }
        public List<CollisionEffectDTO>? CollisionEffects { get; set; }

        public bool ShouldSerializeSecondMaterial() => SecondMaterial != null;
        public bool ShouldSerializeFallbackMaterialForEffects() => FallbackMaterialForEffects != null;
        public bool ShouldSerializeCollisionEffects() => CollisionEffects != null && CollisionEffects.Count > 0;
    }

    public class CollisionEffectDTO
    {
        public string? EffectType { get; set; }
        public string? Particle { get; set; }
        public string? TrailParticle { get; set; }
        public string? SplashAtlasStartIndex { get; set; }
        public string? SplashAtlasEndIndex { get; set; }
        public string? Decal { get; set; }
        public string? Sound { get; set; }
        public string? DoNotSendArmorParameterToSound { get; set; }
        public string? UpperBoundOfSizeParameterForSound { get; set; }
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