using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class PhysicsMaterialsMapper
    {
        public static PhysicsMaterialsDTO ToDTO(PhysicsMaterialsDO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialsDTO
            {
                Type = source.Type,
                PhysicsMaterials = PhysicsMaterialsContainerToDTO(source.PhysicsMaterials),
                SoundAndCollisionInfoClassDefinitions = SoundAndCollisionInfoClassDefinitionsToDTO(source.SoundAndCollisionInfoClassDefinitions)
            };
        }

        public static PhysicsMaterialsDO ToDO(PhysicsMaterialsDTO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialsDO
            {
                Type = source.Type,
                PhysicsMaterials = PhysicsMaterialsContainerToDo(source.PhysicsMaterials),
                SoundAndCollisionInfoClassDefinitions = SoundAndCollisionInfoClassDefinitionsToDo(source.SoundAndCollisionInfoClassDefinitions),
                HasPhysicsMaterials = source.PhysicsMaterials != null && source.PhysicsMaterials.Materials.Count > 0,
                HasSoundAndCollisionInfoClassDefinitions = source.SoundAndCollisionInfoClassDefinitions != null && source.SoundAndCollisionInfoClassDefinitions.Definitions.Count > 0
            };
        }

        public static PhysicsMaterialsContainerDTO PhysicsMaterialsContainerToDTO(PhysicsMaterialsContainerDO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialsContainerDTO
            {
                Materials = source.Materials?.Select(PhysicsMaterialToDTO).ToList() ?? new List<PhysicsMaterialDTO>()
            };
        }

        public static PhysicsMaterialsContainerDO PhysicsMaterialsContainerToDo(PhysicsMaterialsContainerDTO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialsContainerDO
            {
                Materials = source.Materials?.Select(PhysicsMaterialToDo).ToList() ?? new List<PhysicsMaterialDO>()
            };
        }

        public static PhysicsMaterialDTO PhysicsMaterialToDTO(PhysicsMaterialDO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialDTO
            {
                Id = source.Id,
                StaticFriction = source.StaticFriction,
                DynamicFriction = source.DynamicFriction,
                Restitution = source.Restitution,
                Softness = source.Softness,
                LinearDamping = source.LinearDamping,
                AngularDamping = source.AngularDamping,
                DisplayColor = source.DisplayColor,
                RainSplashesEnabled = source.RainSplashesEnabled,
                Flammable = source.Flammable,
                OverrideMaterialNameForImpactSounds = source.OverrideMaterialNameForImpactSounds,
                DontStickMissiles = source.DontStickMissiles,
                AttacksCanPassThrough = source.AttacksCanPassThrough
            };
        }

        public static PhysicsMaterialDO PhysicsMaterialToDo(PhysicsMaterialDTO source)
        {
            if (source == null) return null;

            return new PhysicsMaterialDO
            {
                Id = source.Id,
                StaticFriction = source.StaticFriction,
                DynamicFriction = source.DynamicFriction,
                Restitution = source.Restitution,
                Softness = source.Softness,
                LinearDamping = source.LinearDamping,
                AngularDamping = source.AngularDamping,
                DisplayColor = source.DisplayColor,
                RainSplashesEnabled = source.RainSplashesEnabled,
                Flammable = source.Flammable,
                OverrideMaterialNameForImpactSounds = source.OverrideMaterialNameForImpactSounds,
                DontStickMissiles = source.DontStickMissiles,
                AttacksCanPassThrough = source.AttacksCanPassThrough
            };
        }

        public static SoundAndCollisionInfoClassDefinitionsDTO SoundAndCollisionInfoClassDefinitionsToDTO(SoundAndCollisionInfoClassDefinitionsDO source)
        {
            if (source == null) return null;

            return new SoundAndCollisionInfoClassDefinitionsDTO
            {
                Definitions = source.Definitions?.Select(SoundAndCollisionInfoClassDefinitionToDTO).ToList() ?? new List<SoundAndCollisionInfoClassDefinitionDTO>()
            };
        }

        public static SoundAndCollisionInfoClassDefinitionsDO SoundAndCollisionInfoClassDefinitionsToDo(SoundAndCollisionInfoClassDefinitionsDTO source)
        {
            if (source == null) return null;

            return new SoundAndCollisionInfoClassDefinitionsDO
            {
                Definitions = source.Definitions?.Select(SoundAndCollisionInfoClassDefinitionToDo).ToList() ?? new List<SoundAndCollisionInfoClassDefinitionDO>()
            };
        }

        public static SoundAndCollisionInfoClassDefinitionDTO SoundAndCollisionInfoClassDefinitionToDTO(SoundAndCollisionInfoClassDefinitionDO source)
        {
            if (source == null) return null;

            return new SoundAndCollisionInfoClassDefinitionDTO
            {
                Name = source.Name
            };
        }

        public static SoundAndCollisionInfoClassDefinitionDO SoundAndCollisionInfoClassDefinitionToDo(SoundAndCollisionInfoClassDefinitionDTO source)
        {
            if (source == null) return null;

            return new SoundAndCollisionInfoClassDefinitionDO
            {
                Name = source.Name
            };
        }
    }
}