using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class BannerIconsMapper
    {
        public static BannerIconsDTO ToDTO(BannerIconsDO source)
        {
            if (source == null) return null;
            
            return new BannerIconsDTO
            {
                Type = source.Type,
                BannerIconData = BannerIconDataMapper.ToDTO(source.BannerIconData)
            };
        }
        
        public static BannerIconsDO ToDO(BannerIconsDTO source)
        {
            if (source == null) return null;
            
            return new BannerIconsDO
            {
                Type = source.Type,
                BannerIconData = BannerIconDataMapper.ToDO(source.BannerIconData),
                HasBannerIconData = source.BannerIconData != null
            };
        }
    }
    
    public static class BannerIconDataMapper
    {
        public static BannerIconDataDTO ToDTO(BannerIconDataDO source)
        {
            if (source == null) return null;
            
            return new BannerIconDataDTO
            {
                BannerIconGroups = source.BannerIconGroups?
                    .Select(BannerIconGroupMapper.ToDTO)
                    .ToList() ?? new List<BannerIconGroupDTO>(),
                BannerColors = BannerColorsMapper.ToDTO(source.BannerColors)
            };
        }
        
        public static BannerIconDataDO ToDO(BannerIconDataDTO source)
        {
            if (source == null) return null;
            
            return new BannerIconDataDO
            {
                BannerIconGroups = source.BannerIconGroups?
                    .Select(BannerIconGroupMapper.ToDO)
                    .ToList() ?? new List<BannerIconGroupDO>(),
                BannerColors = BannerColorsMapper.ToDO(source.BannerColors),
                HasBannerColors = source.BannerColors != null,
                // 修复：添加对空BannerIconGroups的标记
                HasEmptyBannerIconGroups = source.BannerIconGroups != null && source.BannerIconGroups.Count == 0
            };
        }
    }
    
    public static class BannerIconGroupMapper
    {
        public static BannerIconGroupDTO ToDTO(BannerIconGroupDO source)
        {
            if (source == null) return null;
            
            return new BannerIconGroupDTO
            {
                Id = source.Id,
                Name = source.Name,
                IsPattern = source.IsPattern,
                Backgrounds = source.Backgrounds?
                    .Select(BackgroundMapper.ToDTO)
                    .ToList() ?? new List<BackgroundDTO>(),
                Icons = source.Icons?
                    .Select(IconMapper.ToDTO)
                    .ToList() ?? new List<IconDTO>()
            };
        }
        
        public static BannerIconGroupDO ToDO(BannerIconGroupDTO source)
        {
            if (source == null) return null;
            
            return new BannerIconGroupDO
            {
                Id = source.Id,
                Name = source.Name,
                IsPattern = source.IsPattern,
                Backgrounds = source.Backgrounds?
                    .Select(BackgroundMapper.ToDO)
                    .ToList() ?? new List<BackgroundDO>(),
                Icons = source.Icons?
                    .Select(IconMapper.ToDO)
                    .ToList() ?? new List<IconDO>(),
                // 修复：添加对空Backgrounds和Icons的标记
                HasEmptyBackgrounds = source.Backgrounds != null && source.Backgrounds.Count == 0,
                HasEmptyIcons = source.Icons != null && source.Icons.Count == 0
            };
        }
    }
    
    public static class BackgroundMapper
    {
        public static BackgroundDTO ToDTO(BackgroundDO source)
        {
            if (source == null) return null;
            
            return new BackgroundDTO
            {
                Id = source.Id,
                MeshName = source.MeshName,
                IsBaseBackground = source.IsBaseBackground
            };
        }
        
        public static BackgroundDO ToDO(BackgroundDTO source)
        {
            if (source == null) return null;
            
            return new BackgroundDO
            {
                Id = source.Id,
                MeshName = source.MeshName,
                IsBaseBackground = source.IsBaseBackground
            };
        }
    }
    
    public static class IconMapper
    {
        public static IconDTO ToDTO(IconDO source)
        {
            if (source == null) return null;
            
            return new IconDTO
            {
                Id = source.Id,
                MaterialName = source.MaterialName,
                TextureIndex = source.TextureIndex,
                IsReserved = source.IsReserved
            };
        }
        
        public static IconDO ToDO(IconDTO source)
        {
            if (source == null) return null;
            
            return new IconDO
            {
                Id = source.Id,
                MaterialName = source.MaterialName,
                TextureIndex = source.TextureIndex,
                IsReserved = source.IsReserved
            };
        }
    }
    
    public static class BannerColorsMapper
    {
        public static BannerColorsDTO ToDTO(BannerColorsDO source)
        {
            if (source == null) return null;
            
            return new BannerColorsDTO
            {
                Colors = source.Colors?
                    .Select(ColorEntryMapper.ToDTO)
                    .ToList() ?? new List<ColorEntryDTO>()
            };
        }
        
        public static BannerColorsDO ToDO(BannerColorsDTO source)
        {
            if (source == null) return null;
            
            return new BannerColorsDO
            {
                Colors = source.Colors?
                    .Select(ColorEntryMapper.ToDO)
                    .ToList() ?? new List<ColorEntryDO>()
            };
        }
    }
    
    public static class ColorEntryMapper
    {
        public static ColorEntryDTO ToDTO(ColorEntryDO source)
        {
            if (source == null) return null;
            
            return new ColorEntryDTO
            {
                Id = source.Id,
                Hex = source.Hex,
                PlayerCanChooseForBackground = source.PlayerCanChooseForBackground,
                PlayerCanChooseForSigil = source.PlayerCanChooseForSigil
            };
        }
        
        public static ColorEntryDO ToDO(ColorEntryDTO source)
        {
            if (source == null) return null;
            
            return new ColorEntryDO
            {
                Id = source.Id,
                Hex = source.Hex,
                PlayerCanChooseForBackground = source.PlayerCanChooseForBackground,
                PlayerCanChooseForSigil = source.PlayerCanChooseForSigil
            };
        }
    }
}