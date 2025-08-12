using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class CollisionInfosMapper
    {
        public static CollisionInfosRootDTO ToDTO(CollisionInfosRootDO source)
        {
            if (source == null) return null;

            return new CollisionInfosRootDTO
            {
                Type = source.Type,
                CollisionInfos = ToDTO(source.CollisionInfos)
            };
        }

        public static CollisionInfosRootDO ToDO(CollisionInfosRootDTO source)
        {
            if (source == null) return null;

            return new CollisionInfosRootDO
            {
                Type = source.Type,
                CollisionInfos = ToDO(source.CollisionInfos)
            };
        }

        private static CollisionInfosDTO ToDTO(CollisionInfosDO source)
        {
            if (source == null) return null;

            return new CollisionInfosDTO
            {
                Materials = source.Materials?.Select(ToDTO).ToList(),
                Monsters = source.Monsters?.Select(ToDTO).ToList()
            };
        }

        private static CollisionInfosDO ToDO(CollisionInfosDTO source)
        {
            if (source == null) return null;

            return new CollisionInfosDO
            {
                Materials = source.Materials?.Select(ToDO).ToList(),
                Monsters = source.Monsters?.Select(ToDO).ToList()
            };
        }

        private static CollisionMaterialDTO ToDTO(CollisionMaterialDO source)
        {
            if (source == null) return null;

            return new CollisionMaterialDTO
            {
                Id = source.Id,
                CollisionInfos = source.CollisionInfos?.Select(ToDTO).ToList()
            };
        }

        private static CollisionMaterialDO ToDO(CollisionMaterialDTO source)
        {
            if (source == null) return null;

            return new CollisionMaterialDO
            {
                Id = source.Id,
                CollisionInfos = source.CollisionInfos?.Select(ToDO).ToList()
            };
        }

        private static CollisionMonsterDTO ToDTO(CollisionMonsterDO source)
        {
            if (source == null) return null;

            return new CollisionMonsterDTO
            {
                SoundAndCollisionInfoClass = source.SoundAndCollisionInfoClass,
                CollisionInfos = source.CollisionInfos?.Select(ToDTO).ToList()
            };
        }

        private static CollisionMonsterDO ToDO(CollisionMonsterDTO source)
        {
            if (source == null) return null;

            return new CollisionMonsterDO
            {
                SoundAndCollisionInfoClass = source.SoundAndCollisionInfoClass,
                CollisionInfos = source.CollisionInfos?.Select(ToDO).ToList()
            };
        }

        private static CollisionInfoDTO ToDTO(CollisionInfoDO source)
        {
            if (source == null) return null;

            return new CollisionInfoDTO
            {
                SecondMaterial = source.SecondMaterial,
                FallbackMaterialForEffects = source.FallbackMaterialForEffects,
                CollisionEffects = source.CollisionEffects?.Select(ToDTO).ToList()
            };
        }

        private static CollisionInfoDO ToDO(CollisionInfoDTO source)
        {
            if (source == null) return null;

            return new CollisionInfoDO
            {
                SecondMaterial = source.SecondMaterial,
                FallbackMaterialForEffects = source.FallbackMaterialForEffects,
                CollisionEffects = source.CollisionEffects?.Select(ToDO).ToList()
            };
        }

        private static CollisionEffectDTO ToDTO(CollisionEffectDO source)
        {
            if (source == null) return null;

            return new CollisionEffectDTO
            {
                EffectType = source.EffectType,
                Particle = source.Particle,
                TrailParticle = source.TrailParticle,
                SplashAtlasStartIndex = source.SplashAtlasStartIndex,
                SplashAtlasEndIndex = source.SplashAtlasEndIndex,
                Decal = source.Decal,
                Sound = source.Sound,
                DoNotSendArmorParameterToSound = source.DoNotSendArmorParameterToSound,
                UpperBoundOfSizeParameterForSound = source.UpperBoundOfSizeParameterForSound,
                OneShot = source.OneShot
            };
        }

        private static CollisionEffectDO ToDO(CollisionEffectDTO source)
        {
            if (source == null) return null;

            return new CollisionEffectDO
            {
                EffectType = source.EffectType,
                Particle = source.Particle,
                TrailParticle = source.TrailParticle,
                SplashAtlasStartIndex = source.SplashAtlasStartIndex,
                SplashAtlasEndIndex = source.SplashAtlasEndIndex,
                Decal = source.Decal,
                Sound = source.Sound,
                DoNotSendArmorParameterToSound = source.DoNotSendArmorParameterToSound,
                UpperBoundOfSizeParameterForSound = source.UpperBoundOfSizeParameterForSound,
                OneShot = source.OneShot
            };
        }
    }
}