using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("item_usage_sets", Namespace = "")]
    [XmlType(Namespace = "")]
    public class ItemUsageSetsDO
    {
        [XmlElement("item_usage_set")]
        public List<ItemUsageSetDO> ItemUsageSetList { get; set; } = new List<ItemUsageSetDO>();

        [XmlIgnore]
        public bool HasEmptyItemUsageSetList { get; set; } = false;

        public bool ShouldSerializeItemUsageSetList() => HasEmptyItemUsageSetList || 
            (ItemUsageSetList != null && ItemUsageSetList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageSetDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("has_single_stance")]
        public string? HasSingleStance { get; set; }

        [XmlAttribute("base_set")]
        public string? BaseSet { get; set; }

        [XmlAttribute("switch_to_alternate_action")]
        public string? SwitchToAlternateAction { get; set; }

        [XmlAttribute("last_arrow_sound")]
        public string? LastArrowSound { get; set; }

        [XmlAttribute("equip_sound")]
        public string? EquipSound { get; set; }

        [XmlAttribute("unequip_sound")]
        public string? UnequipSound { get; set; }

        [XmlElement("idles")]
        public ItemUsageIdlesDO? Idles { get; set; }

        [XmlElement("movement_sets")]
        public ItemUsageMovementSetsDO? MovementSets { get; set; }

        [XmlElement("usages")]
        public ItemUsageUsagesDO? Usages { get; set; }

        [XmlElement("flags")]
        public ItemUsageFlagsDO? Flags { get; set; }

        // ShouldSerialize 方法用于精确控制序列化
        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeHasSingleStance() => !string.IsNullOrWhiteSpace(HasSingleStance);
        public bool ShouldSerializeBaseSet() => !string.IsNullOrWhiteSpace(BaseSet);
        public bool ShouldSerializeSwitchToAlternateAction() => !string.IsNullOrWhiteSpace(SwitchToAlternateAction);
        public bool ShouldSerializeLastArrowSound() => !string.IsNullOrWhiteSpace(LastArrowSound);
        public bool ShouldSerializeEquipSound() => !string.IsNullOrWhiteSpace(EquipSound);
        public bool ShouldSerializeUnequipSound() => !string.IsNullOrWhiteSpace(UnequipSound);
        public bool ShouldSerializeIdles() => Idles != null && Idles.IdleList.Count > 0;
        public bool ShouldSerializeMovementSets() => MovementSets != null && MovementSets.MovementSetList.Count > 0;
        public bool ShouldSerializeUsages() => Usages != null && Usages.UsageList.Count > 0;
        public bool ShouldSerializeFlags() => Flags != null && Flags.FlagList.Count > 0;

        // 便捷属性
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasHasSingleStance => !string.IsNullOrEmpty(HasSingleStance);
        public bool HasBaseSet => !string.IsNullOrEmpty(BaseSet);
        public bool HasSwitchToAlternateAction => !string.IsNullOrEmpty(SwitchToAlternateAction);
        public bool HasLastArrowSound => !string.IsNullOrEmpty(LastArrowSound);
        public bool HasEquipSound => !string.IsNullOrEmpty(EquipSound);
        public bool HasUnequipSound => !string.IsNullOrEmpty(UnequipSound);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageIdlesDO
    {
        [XmlElement("idle")]
        public List<ItemUsageIdleDO> IdleList { get; set; } = new List<ItemUsageIdleDO>();

        [XmlIgnore]
        public bool HasEmptyIdleList { get; set; } = false;

        public bool ShouldSerializeIdleList() => HasEmptyIdleList || (IdleList != null && IdleList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageIdleDO
    {
        [XmlAttribute("action")]
        public string? Action { get; set; }

        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("require_free_left_hand")]
        public string? RequireFreeLeftHand { get; set; }

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }

        public bool ShouldSerializeAction() => !string.IsNullOrWhiteSpace(Action);
        public bool ShouldSerializeIsLeftStance() => !string.IsNullOrWhiteSpace(IsLeftStance);
        public bool ShouldSerializeRequireFreeLeftHand() => !string.IsNullOrWhiteSpace(RequireFreeLeftHand);
        public bool ShouldSerializeRequireLeftHandUsageRootSet() => !string.IsNullOrWhiteSpace(RequireLeftHandUsageRootSet);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageMovementSetsDO
    {
        [XmlElement("movement_set")]
        public List<ItemUsageMovementSetDO> MovementSetList { get; set; } = new List<ItemUsageMovementSetDO>();

        [XmlIgnore]
        public bool HasEmptyMovementSetList { get; set; } = false;

        public bool ShouldSerializeMovementSetList() => HasEmptyMovementSetList || 
            (MovementSetList != null && MovementSetList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageMovementSetDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeRequireLeftHandUsageRootSet() => !string.IsNullOrWhiteSpace(RequireLeftHandUsageRootSet);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageUsagesDO
    {
        [XmlElement("usage")]
        public List<ItemUsageUsageDO> UsageList { get; set; } = new List<ItemUsageUsageDO>();

        [XmlIgnore]
        public bool HasEmptyUsageList { get; set; } = false;

        public bool ShouldSerializeUsageList() => HasEmptyUsageList || (UsageList != null && UsageList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageUsageDO
    {
        [XmlAttribute("style")]
        public string? Style { get; set; }

        [XmlAttribute("ready_action")]
        public string? ReadyAction { get; set; }

        [XmlAttribute("quick_release_action")]
        public string? QuickReleaseAction { get; set; }

        [XmlAttribute("release_action")]
        public string? ReleaseAction { get; set; }

        [XmlAttribute("quick_blocked_action")]
        public string? QuickBlockedAction { get; set; }

        [XmlAttribute("blocked_action")]
        public string? BlockedAction { get; set; }

        [XmlAttribute("is_mounted")]
        public string? IsMounted { get; set; }

        [XmlAttribute("require_free_left_hand")]
        public string? RequireFreeLeftHand { get; set; }

        [XmlAttribute("strike_type")]
        public string? StrikeType { get; set; }

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }

        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("reload_action")]
        public string? ReloadAction { get; set; }

        [XmlAttribute("ready_continue_ranged_action")]
        public string? ReadyContinueRangedAction { get; set; }

        public bool ShouldSerializeStyle() => !string.IsNullOrWhiteSpace(Style);
        public bool ShouldSerializeReadyAction() => !string.IsNullOrWhiteSpace(ReadyAction);
        public bool ShouldSerializeQuickReleaseAction() => !string.IsNullOrWhiteSpace(QuickReleaseAction);
        public bool ShouldSerializeReleaseAction() => !string.IsNullOrWhiteSpace(ReleaseAction);
        public bool ShouldSerializeQuickBlockedAction() => !string.IsNullOrWhiteSpace(QuickBlockedAction);
        public bool ShouldSerializeBlockedAction() => !string.IsNullOrWhiteSpace(BlockedAction);
        public bool ShouldSerializeIsMounted() => !string.IsNullOrWhiteSpace(IsMounted);
        public bool ShouldSerializeRequireFreeLeftHand() => !string.IsNullOrWhiteSpace(RequireFreeLeftHand);
        public bool ShouldSerializeStrikeType() => !string.IsNullOrWhiteSpace(StrikeType);
        public bool ShouldSerializeRequireLeftHandUsageRootSet() => !string.IsNullOrWhiteSpace(RequireLeftHandUsageRootSet);
        public bool ShouldSerializeIsLeftStance() => !string.IsNullOrWhiteSpace(IsLeftStance);
        public bool ShouldSerializeReloadAction() => !string.IsNullOrWhiteSpace(ReloadAction);
        public bool ShouldSerializeReadyContinueRangedAction() => !string.IsNullOrWhiteSpace(ReadyContinueRangedAction);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageFlagsDO
    {
        [XmlElement("flag")]
        public List<ItemUsageFlagDO> FlagList { get; set; } = new List<ItemUsageFlagDO>();

        [XmlIgnore]
        public bool HasEmptyFlagList { get; set; } = false;

        public bool ShouldSerializeFlagList() => HasEmptyFlagList || (FlagList != null && FlagList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class ItemUsageFlagDO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
    }
}