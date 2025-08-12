using System.Collections.Generic;

namespace BannerlordModEditor.Common.Models.DTO
{
    public class CollisionInfosRootDTO
    {
        public string? Type { get; set; }
        public CollisionInfosDTO? CollisionInfos { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
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

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionMonsterDTO
    {
        public string? SoundAndCollisionInfoClass { get; set; }
        public List<CollisionInfoDTO>? CollisionInfos { get; set; }

        public bool ShouldSerializeSoundAndCollisionInfoClass() => !string.IsNullOrEmpty(SoundAndCollisionInfoClass);
        public bool ShouldSerializeCollisionInfos() => CollisionInfos != null && CollisionInfos.Count > 0;
    }

    public class CollisionInfoDTO
    {
        public string? SecondMaterial { get; set; }
        public string? FallbackMaterialForEffects { get; set; }
        public List<CollisionEffectDTO>? CollisionEffects { get; set; }

        public bool ShouldSerializeSecondMaterial() => !string.IsNullOrEmpty(SecondMaterial);
        public bool ShouldSerializeFallbackMaterialForEffects() => !string.IsNullOrEmpty(FallbackMaterialForEffects);
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

        public bool ShouldSerializeEffectType() => !string.IsNullOrEmpty(EffectType);
        public bool ShouldSerializeParticle() => !string.IsNullOrEmpty(Particle);
        public bool ShouldSerializeTrailParticle() => !string.IsNullOrEmpty(TrailParticle);
        public bool ShouldSerializeSplashAtlasStartIndex() => !string.IsNullOrEmpty(SplashAtlasStartIndex);
        public bool ShouldSerializeSplashAtlasEndIndex() => !string.IsNullOrEmpty(SplashAtlasEndIndex);
        public bool ShouldSerializeDecal() => !string.IsNullOrEmpty(Decal);
        public bool ShouldSerializeSound() => !string.IsNullOrEmpty(Sound);
        public bool ShouldSerializeDoNotSendArmorParameterToSound() => !string.IsNullOrEmpty(DoNotSendArmorParameterToSound);
        public bool ShouldSerializeUpperBoundOfSizeParameterForSound() => !string.IsNullOrEmpty(UpperBoundOfSizeParameterForSound);
        public bool ShouldSerializeOneShot() => !string.IsNullOrEmpty(OneShot);
    }
}