using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Mappers
{
    public static class ItemUsageSetsMapper
    {
        public static ItemUsageSetsDTO ToDTO(ItemUsageSetsDO source)
        {
            if (source == null) return null;

            return new ItemUsageSetsDTO
            {
                ItemUsageSetList = source.ItemUsageSetList?
                    .Select(ToDTO)
                    .ToList() ?? new List<ItemUsageSetDTO>()
            };
        }

        public static ItemUsageSetsDO ToDO(ItemUsageSetsDTO source)
        {
            if (source == null) return null;

            return new ItemUsageSetsDO
            {
                ItemUsageSetList = source.ItemUsageSetList?
                    .Select(ToDO)
                    .ToList() ?? new List<ItemUsageSetDO>()
            };
        }

        public static ItemUsageSetDTO ToDTO(ItemUsageSetDO source)
        {
            if (source == null) return null;

            return new ItemUsageSetDTO
            {
                Id = source.Id,
                HasSingleStance = source.HasSingleStance,
                BaseSet = source.BaseSet,
                SwitchToAlternateAction = source.SwitchToAlternateAction,
                LastArrowSound = source.LastArrowSound,
                EquipSound = source.EquipSound,
                UnequipSound = source.UnequipSound,
                // 修复嵌套Mapper调用 - 使用当前类的方法
                Idles = ToDTO(source.Idles),
                MovementSets = ToDTO(source.MovementSets),
                Usages = ToDTO(source.Usages),
                Flags = ToDTO(source.Flags)
            };
        }

        public static ItemUsageSetDO ToDO(ItemUsageSetDTO source)
        {
            if (source == null) return null;

            return new ItemUsageSetDO
            {
                Id = source.Id,
                HasSingleStance = source.HasSingleStance,
                BaseSet = source.BaseSet,
                SwitchToAlternateAction = source.SwitchToAlternateAction,
                LastArrowSound = source.LastArrowSound,
                EquipSound = source.EquipSound,
                UnequipSound = source.UnequipSound,
                // 修复嵌套Mapper调用 - 使用当前类的方法
                Idles = ToDO(source.Idles),
                MovementSets = ToDO(source.MovementSets),
                Usages = ToDO(source.Usages),
                Flags = ToDO(source.Flags)
            };
        }

        public static ItemUsageIdlesDTO? ToDTO(ItemUsageIdlesDO? source)
        {
            if (source == null) return null;

            return new ItemUsageIdlesDTO
            {
                // 修复嵌套Mapper调用
                IdleList = source.IdleList?
                    .Select(ToDTO)
                    .ToList() ?? new List<ItemUsageIdleDTO>()
            };
        }

        public static ItemUsageIdlesDO? ToDO(ItemUsageIdlesDTO? source)
        {
            if (source == null) return null;

            return new ItemUsageIdlesDO
            {
                // 修复嵌套Mapper调用
                IdleList = source.IdleList?
                    .Select(ToDO)
                    .ToList() ?? new List<ItemUsageIdleDO>()
            };
        }

        public static ItemUsageIdleDTO ToDTO(ItemUsageIdleDO source)
        {
            if (source == null) return null;

            return new ItemUsageIdleDTO
            {
                Action = source.Action,
                IsLeftStance = source.IsLeftStance,
                RequireFreeLeftHand = source.RequireFreeLeftHand,
                RequireLeftHandUsageRootSet = source.RequireLeftHandUsageRootSet
            };
        }

        public static ItemUsageIdleDO ToDO(ItemUsageIdleDTO source)
        {
            if (source == null) return null;

            return new ItemUsageIdleDO
            {
                Action = source.Action,
                IsLeftStance = source.IsLeftStance,
                RequireFreeLeftHand = source.RequireFreeLeftHand,
                RequireLeftHandUsageRootSet = source.RequireLeftHandUsageRootSet
            };
        }

        public static ItemUsageMovementSetsDTO? ToDTO(ItemUsageMovementSetsDO? source)
        {
            if (source == null) return null;

            return new ItemUsageMovementSetsDTO
            {
                MovementSetList = source.MovementSetList?
                    .Select(ToDTO)
                    .ToList() ?? new List<ItemUsageMovementSetDTO>()
            };
        }

        public static ItemUsageMovementSetsDO? ToDO(ItemUsageMovementSetsDTO? source)
        {
            if (source == null) return null;

            return new ItemUsageMovementSetsDO
            {
                MovementSetList = source.MovementSetList?
                    .Select(ToDO)
                    .ToList() ?? new List<ItemUsageMovementSetDO>()
            };
        }

        public static ItemUsageMovementSetDTO ToDTO(ItemUsageMovementSetDO source)
        {
            if (source == null) return null;

            return new ItemUsageMovementSetDTO
            {
                Id = source.Id,
                RequireLeftHandUsageRootSet = source.RequireLeftHandUsageRootSet
            };
        }

        public static ItemUsageMovementSetDO ToDO(ItemUsageMovementSetDTO source)
        {
            if (source == null) return null;

            return new ItemUsageMovementSetDO
            {
                Id = source.Id,
                RequireLeftHandUsageRootSet = source.RequireLeftHandUsageRootSet
            };
        }

        public static ItemUsageUsagesDTO? ToDTO(ItemUsageUsagesDO? source)
        {
            if (source == null) return null;

            return new ItemUsageUsagesDTO
            {
                UsageList = source.UsageList?
                    .Select(ToDTO)
                    .ToList() ?? new List<ItemUsageUsageDTO>()
            };
        }

        public static ItemUsageUsagesDO? ToDO(ItemUsageUsagesDTO? source)
        {
            if (source == null) return null;

            return new ItemUsageUsagesDO
            {
                UsageList = source.UsageList?
                    .Select(ToDO)
                    .ToList() ?? new List<ItemUsageUsageDO>()
            };
        }

        public static ItemUsageUsageDTO ToDTO(ItemUsageUsageDO source)
        {
            if (source == null) return null;

            return new ItemUsageUsageDTO
            {
                Style = source.Style,
                ReadyAction = source.ReadyAction,
                QuickReleaseAction = source.QuickReleaseAction,
                ReleaseAction = source.ReleaseAction,
                QuickBlockedAction = source.QuickBlockedAction,
                BlockedAction = source.BlockedAction,
                IsMounted = source.IsMounted,
                RequireFreeLeftHand = source.RequireFreeLeftHand,
                StrikeType = source.StrikeType,
                RequireLeftHandUsageRootSet = source.RequireLeftHandUsageRootSet,
                IsLeftStance = source.IsLeftStance,
                ReloadAction = source.ReloadAction,
                ReadyContinueRangedAction = source.ReadyContinueRangedAction
            };
        }

        public static ItemUsageUsageDO ToDO(ItemUsageUsageDTO source)
        {
            if (source == null) return null;

            return new ItemUsageUsageDO
            {
                Style = source.Style,
                ReadyAction = source.ReadyAction,
                QuickReleaseAction = source.QuickReleaseAction,
                ReleaseAction = source.ReleaseAction,
                QuickBlockedAction = source.QuickBlockedAction,
                BlockedAction = source.BlockedAction,
                IsMounted = source.IsMounted,
                RequireFreeLeftHand = source.RequireFreeLeftHand,
                StrikeType = source.StrikeType,
                RequireLeftHandUsageRootSet = source.RequireLeftHandUsageRootSet,
                IsLeftStance = source.IsLeftStance,
                ReloadAction = source.ReloadAction,
                ReadyContinueRangedAction = source.ReadyContinueRangedAction
            };
        }

        public static ItemUsageFlagsDTO? ToDTO(ItemUsageFlagsDO? source)
        {
            if (source == null) return null;

            return new ItemUsageFlagsDTO
            {
                FlagList = source.FlagList?
                    .Select(ToDTO)
                    .ToList() ?? new List<ItemUsageFlagDTO>()
            };
        }

        public static ItemUsageFlagsDO? ToDO(ItemUsageFlagsDTO? source)
        {
            if (source == null) return null;

            return new ItemUsageFlagsDO
            {
                FlagList = source.FlagList?
                    .Select(ToDO)
                    .ToList() ?? new List<ItemUsageFlagDO>()
            };
        }

        public static ItemUsageFlagDTO ToDTO(ItemUsageFlagDO source)
        {
            if (source == null) return null;

            return new ItemUsageFlagDTO
            {
                Name = source.Name
            };
        }

        public static ItemUsageFlagDO ToDO(ItemUsageFlagDTO source)
        {
            if (source == null) return null;

            return new ItemUsageFlagDO
            {
                Name = source.Name
            };
        }
    }
}