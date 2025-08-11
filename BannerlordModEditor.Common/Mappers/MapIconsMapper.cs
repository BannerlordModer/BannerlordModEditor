using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MapIconsMapper
    {
        #region DO to DTO

        public static MapIconsDTO ToDTO(MapIconsDO source)
        {
            if (source == null) return null;

            return new MapIconsDTO
            {
                Type = source.Type,
                MapIconsContainer = ToDTO(source.MapIconsContainer)
            };
        }

        public static MapIconsContainerDTO ToDTO(MapIconsContainerDO source)
        {
            if (source == null) return null;

            return new MapIconsContainerDTO
            {
                MapIconList = source.MapIconList?.Select(ToDTO).ToList() ?? new List<MapIconDTO>()
            };
        }

        public static MapIconDTO ToDTO(MapIconDO source)
        {
            if (source == null) return null;

            return new MapIconDTO
            {
                Id = source.Id,
                IdStr = source.IdStr,
                Flags = source.Flags,
                MeshName = source.MeshName,
                MeshScale = source.MeshScale,
                SoundNo = source.SoundNo,
                OffsetPos = source.OffsetPos,
                DirtName = source.DirtName
            };
        }

        #endregion

        #region DTO to DO

        public static MapIconsDO ToDO(MapIconsDTO source)
        {
            if (source == null) return null;

            return new MapIconsDO
            {
                Type = source.Type,
                MapIconsContainer = ToDO(source.MapIconsContainer)
            };
        }

        public static MapIconsContainerDO ToDO(MapIconsContainerDTO source)
        {
            if (source == null) return null;

            return new MapIconsContainerDO
            {
                MapIconList = source.MapIconList?.Select(ToDO).ToList() ?? new List<MapIconDO>()
            };
        }

        public static MapIconDO ToDO(MapIconDTO source)
        {
            if (source == null) return null;

            return new MapIconDO
            {
                Id = source.Id,
                IdStr = source.IdStr,
                Flags = source.Flags,
                MeshName = source.MeshName,
                MeshScale = source.MeshScale,
                SoundNo = source.SoundNo,
                OffsetPos = source.OffsetPos,
                DirtName = source.DirtName
            };
        }

        #endregion
    }
}