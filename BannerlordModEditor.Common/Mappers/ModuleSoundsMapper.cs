using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Audio;
using BannerlordModEditor.Common.Models.DTO.Audio;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ModuleSoundsMapper
    {
        public static ModuleSoundsDTO ToDTO(ModuleSoundsDO source)
        {
            if (source == null) return null;

            return new ModuleSoundsDTO
            {
                Type = source.Type,
                ModuleSounds = ModuleSoundsContainerToDTO(source.ModuleSounds)
            };
        }

        public static ModuleSoundsDO ToDO(ModuleSoundsDTO source)
        {
            if (source == null) return null;

            return new ModuleSoundsDO
            {
                Type = source.Type,
                ModuleSounds = ModuleSoundsContainerToDo(source.ModuleSounds),
                HasModuleSounds = source.ModuleSounds != null && source.ModuleSounds.Sounds.Count > 0
            };
        }

        public static ModuleSoundsContainerDTO ModuleSoundsContainerToDTO(ModuleSoundsContainerDO source)
        {
            if (source == null) return null;

            return new ModuleSoundsContainerDTO
            {
                Sounds = source.Sounds?.Select(ModuleSoundToDTO).ToList() ?? new List<ModuleSoundDTO>()
            };
        }

        public static ModuleSoundsContainerDO ModuleSoundsContainerToDo(ModuleSoundsContainerDTO source)
        {
            if (source == null) return null;

            return new ModuleSoundsContainerDO
            {
                Sounds = source.Sounds?.Select(ModuleSoundToDo).ToList() ?? new List<ModuleSoundDO>()
            };
        }

        public static ModuleSoundDTO ModuleSoundToDTO(ModuleSoundDO source)
        {
            if (source == null) return null;

            return new ModuleSoundDTO
            {
                Name = source.Name,
                Is2D = source.Is2D,
                SoundCategory = source.SoundCategory,
                Path = source.Path,
                MinPitchMultiplier = source.MinPitchMultiplier,
                MaxPitchMultiplier = source.MaxPitchMultiplier,
                Variations = source.Variations?.Select(SoundVariationToDTO).ToList() ?? new List<SoundVariationDTO>()
            };
        }

        public static ModuleSoundDO ModuleSoundToDo(ModuleSoundDTO source)
        {
            if (source == null) return null;

            return new ModuleSoundDO
            {
                Name = source.Name,
                Is2D = source.Is2D,
                SoundCategory = source.SoundCategory,
                Path = source.Path,
                MinPitchMultiplier = source.MinPitchMultiplier,
                MaxPitchMultiplier = source.MaxPitchMultiplier,
                Variations = source.Variations?.Select(SoundVariationToDo).ToList() ?? new List<SoundVariationDO>(),
                HasVariations = source.Variations != null && source.Variations.Count > 0
            };
        }

        public static SoundVariationDTO SoundVariationToDTO(SoundVariationDO source)
        {
            if (source == null) return null;

            return new SoundVariationDTO
            {
                Path = source.Path,
                Weight = source.Weight
            };
        }

        public static SoundVariationDO SoundVariationToDo(SoundVariationDTO source)
        {
            if (source == null) return null;

            return new SoundVariationDO
            {
                Path = source.Path,
                Weight = source.Weight
            };
        }
    }
}