using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MusicMapper
    {
        public static MusicDTO ToDTO(MusicDO source)
        {
            if (source == null) return null;

            return new MusicDTO
            {
                Type = source.Type,
                MusicsContainer = MusicsContainerToDTO(source.MusicsContainer)
            };
        }

        public static MusicDO ToDO(MusicDTO source)
        {
            if (source == null) return null;

            return new MusicDO
            {
                Type = source.Type,
                MusicsContainer = MusicsContainerToDo(source.MusicsContainer),
                HasMusicsContainer = source.MusicsContainer != null && source.MusicsContainer.Music.Count > 0
            };
        }

        public static MusicsContainerDTO MusicsContainerToDTO(MusicsContainerDO source)
        {
            if (source == null) return null;

            return new MusicsContainerDTO
            {
                Music = source.Music?.Select(MusicTrackToDTO).ToList() ?? new List<MusicTrackDTO>()
            };
        }

        public static MusicsContainerDO MusicsContainerToDo(MusicsContainerDTO source)
        {
            if (source == null) return null;

            return new MusicsContainerDO
            {
                Music = source.Music?.Select(MusicTrackToDo).ToList() ?? new List<MusicTrackDO>()
            };
        }

        public static MusicTrackDTO MusicTrackToDTO(MusicTrackDO source)
        {
            if (source == null) return null;

            return new MusicTrackDTO
            {
                Id = source.Id,
                Name = source.Name,
                Flags = source.Flags,
                ContinueFlags = source.ContinueFlags
            };
        }

        public static MusicTrackDO MusicTrackToDo(MusicTrackDTO source)
        {
            if (source == null) return null;

            return new MusicTrackDO
            {
                Id = source.Id,
                Name = source.Name,
                Flags = source.Flags,
                ContinueFlags = source.ContinueFlags
            };
        }
    }
}