using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public class ItemHolstersMapper
    {
        public static ItemHolstersDTO ToDTO(ItemHolstersDO source)
        {
            if (source == null)
                return null;

            return new ItemHolstersDTO
            {
                ItemHolstersContainer = ToDTO(source.ItemHolstersContainer)
            };
        }

        public static ItemHolstersContainerDTO ToDTO(ItemHolstersContainerDO source)
        {
            if (source == null)
                return null;

            return new ItemHolstersContainerDTO
            {
                ItemHolsterList = source.ItemHolsterList?.Select(ToDTO).ToList() ?? new List<ItemHolsterDTO>()
            };
        }

        public static ItemHolsterDTO ToDTO(ItemHolsterDO source)
        {
            if (source == null)
                return null;

            return new ItemHolsterDTO
            {
                Id = source.Id,
                EquipAction = source.EquipAction,
                EquipActionLeftStance = source.EquipActionLeftStance,
                UnequipAction = source.UnequipAction,
                UnequipActionLeftStance = source.UnequipActionLeftStance,
                ShowHolsterWhenDrawn = source.ShowHolsterWhenDrawn,
                GroupName = source.GroupName,
                HolsterSkeleton = source.HolsterSkeleton,
                HolsterBone = source.HolsterBone,
                BaseSet = source.BaseSet,
                HolsterPosition = source.HolsterPosition,
                HolsterRotationYawPitchRoll = source.HolsterRotationYawPitchRoll
            };
        }

        public static ItemHolstersDO ToDO(ItemHolstersDTO source)
        {
            if (source == null)
                return null;

            return new ItemHolstersDO
            {
                ItemHolstersContainer = ToDO(source.ItemHolstersContainer)
            };
        }

        public static ItemHolstersContainerDO ToDO(ItemHolstersContainerDTO source)
        {
            if (source == null)
                return null;

            return new ItemHolstersContainerDO
            {
                ItemHolsterList = source.ItemHolsterList?.Select(ToDo).ToList() ?? new List<ItemHolsterDO>()
            };
        }

        public static ItemHolsterDO ToDo(ItemHolsterDTO source)
        {
            if (source == null)
                return null;

            return new ItemHolsterDO
            {
                Id = source.Id,
                EquipAction = source.EquipAction,
                EquipActionLeftStance = source.EquipActionLeftStance,
                UnequipAction = source.UnequipAction,
                UnequipActionLeftStance = source.UnequipActionLeftStance,
                ShowHolsterWhenDrawn = source.ShowHolsterWhenDrawn,
                GroupName = source.GroupName,
                HolsterSkeleton = source.HolsterSkeleton,
                HolsterBone = source.HolsterBone,
                BaseSet = source.BaseSet,
                HolsterPosition = source.HolsterPosition,
                HolsterRotationYawPitchRoll = source.HolsterRotationYawPitchRoll
            };
        }
    }
}