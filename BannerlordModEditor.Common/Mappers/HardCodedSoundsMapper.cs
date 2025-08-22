using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class HardCodedSoundsMapper
    {
        public static HardCodedSoundsRootDTO ToDTO(HardCodedSoundsRootDO source)
        {
            if (source == null) return null;

            return new HardCodedSoundsRootDTO
            {
                Type = source.Type,
                HardCodedSounds = HardCodedSoundsToDTO(source.HardCodedSounds)
            };
        }

        public static HardCodedSoundsRootDO ToDO(HardCodedSoundsRootDTO source)
        {
            if (source == null) return null;

            return new HardCodedSoundsRootDO
            {
                Type = source.Type,
                HardCodedSounds = HardCodedSoundsToDo(source.HardCodedSounds)
            };
        }

        public static HardCodedSoundsDTO? HardCodedSoundsToDTO(HardCodedSoundsDO? source)
        {
            if (source == null) return null;

            return new HardCodedSoundsDTO
            {
                HardCodedSound = source.HardCodedSound?.Select(HardCodedSoundToDTO).ToArray()
            };
        }

        public static HardCodedSoundsDO? HardCodedSoundsToDo(HardCodedSoundsDTO? source)
        {
            if (source == null) return null;

            return new HardCodedSoundsDO
            {
                HardCodedSound = source.HardCodedSound?.Select(HardCodedSoundToDo).ToArray()
            };
        }

        public static HardCodedSoundDTO HardCodedSoundToDTO(HardCodedSoundDO source)
        {
            if (source == null) return null;

            return new HardCodedSoundDTO
            {
                Id = source.Id,
                Path = source.Path
            };
        }

        public static HardCodedSoundDO HardCodedSoundToDo(HardCodedSoundDTO source)
        {
            if (source == null) return null;

            return new HardCodedSoundDO
            {
                Id = source.Id,
                Path = source.Path
            };
        }
    }
}