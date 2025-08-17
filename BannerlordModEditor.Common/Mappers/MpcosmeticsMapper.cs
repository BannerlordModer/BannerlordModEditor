using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class MpcosmeticsMapper
    {
        public static MpcosmeticsDTO ToDTO(MpcosmeticsDO source)
        {
            if (source == null) return null;
            
            return new MpcosmeticsDTO
            {
                Cosmetics = source.Cosmetics?
                    .Select(CosmeticMapper.ToDTO)
                    .ToList() ?? new List<CosmeticDTO>()
            };
        }
        
        public static MpcosmeticsDO ToDO(MpcosmeticsDTO source)
        {
            if (source == null) return null;
            
            return new MpcosmeticsDO
            {
                Cosmetics = source.Cosmetics?
                    .Select(CosmeticMapper.ToDO)
                    .ToList() ?? new List<CosmeticDO>()
            };
        }
    }

    public static class CosmeticMapper
    {
        public static CosmeticDTO ToDTO(CosmeticDO source)
        {
            if (source == null) return null;
            
            return new CosmeticDTO
            {
                Type = source.Type,
                Id = source.Id,
                Rarity = source.Rarity,
                Cost = source.Cost,
                Replace = ReplaceMapper.ToDTO(source.Replace)
            };
        }
        
        public static CosmeticDO ToDO(CosmeticDTO source)
        {
            if (source == null) return null;
            
            return new CosmeticDO
            {
                Type = source.Type,
                Id = source.Id,
                Rarity = source.Rarity,
                Cost = source.Cost,
                Replace = ReplaceMapper.ToDO(source.Replace)
            };
        }
    }

    public static class ReplaceMapper
    {
        public static ReplaceDTO ToDTO(ReplaceDO source)
        {
            if (source == null) return null;
            
            return new ReplaceDTO
            {
                ItemlessList = source.ItemlessList?
                    .Select(ItemlessMapper.ToDTO)
                    .ToList() ?? new List<ItemlessDTO>(),
                Items = source.Items?
                    .Select(ItemMapper.ToDTO)
                    .ToList() ?? new List<ItemDTO>()
            };
        }
        
        public static ReplaceDO ToDO(ReplaceDTO source)
        {
            if (source == null) return null;
            
            return new ReplaceDO
            {
                ItemlessList = source.ItemlessList?
                    .Select(ItemlessMapper.ToDO)
                    .ToList() ?? new List<ItemlessDO>(),
                Items = source.Items?
                    .Select(ItemMapper.ToDO)
                    .ToList() ?? new List<MpcosmeticItemDO>()
            };
        }
    }

    public static class ItemMapper
    {
        public static ItemDTO ToDTO(MpcosmeticItemDO source)
        {
            if (source == null) return null;
            
            return new ItemDTO
            {
                Id = source.Id
            };
        }
        
        public static MpcosmeticItemDO ToDO(ItemDTO source)
        {
            if (source == null) return null;
            
            return new MpcosmeticItemDO
            {
                Id = source.Id
            };
        }
    }

    public static class ItemlessMapper
    {
        public static ItemlessDTO ToDTO(ItemlessDO source)
        {
            if (source == null) return null;
            
            return new ItemlessDTO
            {
                Troop = source.Troop,
                Slot = source.Slot
            };
        }
        
        public static ItemlessDO ToDO(ItemlessDTO source)
        {
            if (source == null) return null;
            
            return new ItemlessDO
            {
                Troop = source.Troop,
                Slot = source.Slot
            };
        }
    }
}