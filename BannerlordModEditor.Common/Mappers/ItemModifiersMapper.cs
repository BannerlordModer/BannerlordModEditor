using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ItemModifiersMapper
    {
        public static ItemModifiersDTO ToDTO(ItemModifiersDO source)
        {
            if (source == null) return null;
            
            return new ItemModifiersDTO
            {
                ItemModifierList = source.ItemModifierList?
                    .Select(ToDTO)
                    .ToList() ?? new List<ItemModifierDTO>()
            };
        }
        
        public static ItemModifiersDO ToDO(ItemModifiersDTO source)
        {
            if (source == null) return null;
            
            return new ItemModifiersDO
            {
                ItemModifierList = source.ItemModifierList?
                    .Select(ToDo)
                    .ToList() ?? new List<ItemModifierDO>()
            };
        }
        
        public static ItemModifierDTO ToDTO(ItemModifierDO source)
        {
            if (source == null) return null;
            
            return new ItemModifierDTO
            {
                ModifierGroup = source.ModifierGroup,
                Id = source.Id,
                Name = source.Name,
                LootDropScoreString = source.LootDropScoreString,
                ProductionDropScoreString = source.ProductionDropScoreString,
                DamageString = source.DamageString,
                SpeedString = source.SpeedString,
                MissileSpeedString = source.MissileSpeedString,
                PriceFactorString = source.PriceFactorString,
                Quality = source.Quality,
                HitPointsString = source.HitPointsString,
                HorseSpeedString = source.HorseSpeedString,
                StackCountString = source.StackCountString,
                ArmorString = source.ArmorString,
                ManeuverString = source.ManeuverString,
                ChargeDamageString = source.ChargeDamageString,
                HorseHitPointsString = source.HorseHitPointsString
            };
        }
        
        public static ItemModifierDO ToDo(ItemModifierDTO source)
        {
            if (source == null) return null;
            
            return new ItemModifierDO
            {
                ModifierGroup = source.ModifierGroup,
                Id = source.Id,
                Name = source.Name,
                LootDropScoreString = source.LootDropScoreString,
                ProductionDropScoreString = source.ProductionDropScoreString,
                DamageString = source.DamageString,
                SpeedString = source.SpeedString,
                MissileSpeedString = source.MissileSpeedString,
                PriceFactorString = source.PriceFactorString,
                Quality = source.Quality,
                HitPointsString = source.HitPointsString,
                HorseSpeedString = source.HorseSpeedString,
                StackCountString = source.StackCountString,
                ArmorString = source.ArmorString,
                ManeuverString = source.ManeuverString,
                ChargeDamageString = source.ChargeDamageString,
                HorseHitPointsString = source.HorseHitPointsString
            };
        }
    }
}