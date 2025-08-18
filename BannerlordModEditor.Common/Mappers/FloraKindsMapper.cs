using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class FloraKindsMapper
    {
        #region DO to DTO

        public static FloraKindsDTO ToDTO(FloraKindsDO source)
        {
            if (source == null) return null;

            return new FloraKindsDTO
            {
                FloraKindsList = source.FloraKindsList?.Select(ToDTO).ToList() ?? new List<FloraKindDTO>()
            };
        }

        public static FloraKindDTO ToDTO(FloraKindDO source)
        {
            if (source == null) return null;

            return new FloraKindDTO
            {
                Name = source.Name,
                ViewDistance = source.ViewDistance,
                Flags = ToDTO(source.Flags),
                SeasonalKinds = source.SeasonalKinds?.Select(ToDTO).ToList() ?? new List<SeasonalKindDTO>()
            };
        }

        public static FloraFlagsDTO ToDTO(FloraFlagsDO source)
        {
            if (source == null) return null;

            return new FloraFlagsDTO
            {
                FlagsList = source.FlagsList?.Select(ToDTO).ToList() ?? new List<FloraFlagDTO>()
            };
        }

        public static FloraFlagDTO ToDTO(FloraFlagDO source)
        {
            if (source == null) return null;

            return new FloraFlagDTO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static SeasonalKindDTO ToDTO(SeasonalKindDO source)
        {
            if (source == null) return null;

            return new SeasonalKindDTO
            {
                Season = source.Season,
                FloraVariations = ToDTO(source.FloraVariations)
            };
        }

        public static FloraVariationsDTO ToDTO(FloraVariationsDO source)
        {
            if (source == null) return null;

            return new FloraVariationsDTO
            {
                FloraVariationList = source.FloraVariationList?.Select(ToDTO).ToList() ?? new List<FloraVariationDTO>()
            };
        }

        public static FloraVariationDTO ToDTO(FloraVariationDO source)
        {
            if (source == null) return null;

            return new FloraVariationDTO
            {
                Name = source.Name,
                BodyName = source.BodyName,
                DensityMultiplier = source.DensityMultiplier,
                BbRadius = source.BbRadius,
                Meshes = source.Meshes?.Select(ToDTO).ToList() ?? new List<MeshDTO>()
            };
        }

        public static MeshDTO ToDTO(MeshDO source)
        {
            if (source == null) return null;

            return new MeshDTO
            {
                Name = source.Name,
                Material = source.Material
            };
        }

        #endregion

        #region DTO to DO

        public static FloraKindsDO ToDO(FloraKindsDTO source)
        {
            if (source == null) return null;

            return new FloraKindsDO
            {
                FloraKindsList = source.FloraKindsList?.Select(TODO).ToList() ?? new List<FloraKindDO>()
            };
        }

        public static FloraKindDO TODO(FloraKindDTO source)
        {
            if (source == null) return null;

            return new FloraKindDO
            {
                Name = source.Name,
                ViewDistance = source.ViewDistance,
                Flags = TODO(source.Flags),
                HasFlags = source.Flags != null && source.Flags.FlagsList.Count > 0,
                SeasonalKinds = source.SeasonalKinds?.Select(TODO).ToList() ?? new List<SeasonalKindDO>()
            };
        }

        public static FloraFlagsDO TODO(FloraFlagsDTO source)
        {
            if (source == null) return null;

            return new FloraFlagsDO
            {
                FlagsList = source.FlagsList?.Select(TODO).ToList() ?? new List<FloraFlagDO>()
            };
        }

        public static FloraFlagDO TODO(FloraFlagDTO source)
        {
            if (source == null) return null;

            return new FloraFlagDO
            {
                Name = source.Name,
                Value = source.Value
            };
        }

        public static SeasonalKindDO TODO(SeasonalKindDTO source)
        {
            if (source == null) return null;

            return new SeasonalKindDO
            {
                Season = source.Season,
                FloraVariations = TODO(source.FloraVariations),
                HasFloraVariations = source.FloraVariations != null && source.FloraVariations.FloraVariationList.Count > 0
            };
        }

        public static FloraVariationsDO TODO(FloraVariationsDTO source)
        {
            if (source == null) return null;

            return new FloraVariationsDO
            {
                FloraVariationList = source.FloraVariationList?.Select(TODO).ToList() ?? new List<FloraVariationDO>()
            };
        }

        public static FloraVariationDO TODO(FloraVariationDTO source)
        {
            if (source == null) return null;

            return new FloraVariationDO
            {
                Name = source.Name,
                BodyName = source.BodyName,
                DensityMultiplier = source.DensityMultiplier,
                BbRadius = source.BbRadius,
                Meshes = source.Meshes?.Select(TODO).ToList() ?? new List<MeshDO>()
            };
        }

        public static MeshDO TODO(MeshDTO source)
        {
            if (source == null) return null;

            return new MeshDO
            {
                Name = source.Name,
                Material = source.Material
            };
        }

        #endregion
    }
}