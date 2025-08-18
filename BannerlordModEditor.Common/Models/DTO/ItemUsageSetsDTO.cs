using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("item_usage_sets", Namespace = "")]
    [XmlType(Namespace = "")]
    public class ItemUsageSetsDTO
    {
        [XmlElement("item_usage_set")]
        public List<ItemUsageSetDTO> ItemUsageSetList { get; set; } = new List<ItemUsageSetDTO>();
    }

    [XmlType(Namespace = "")]
    public class ItemUsageSetDTO
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
        public ItemUsageIdlesDTO? Idles { get; set; }

        [XmlElement("movement_sets")]
        public ItemUsageMovementSetsDTO? MovementSets { get; set; }

        [XmlElement("usages")]
        public ItemUsageUsagesDTO? Usages { get; set; }

        [XmlElement("flags")]
        public ItemUsageFlagsDTO? Flags { get; set; }
    }

    [XmlType(Namespace = "")]
    public class ItemUsageIdlesDTO
    {
        [XmlElement("idle")]
        public List<ItemUsageIdleDTO> IdleList { get; set; } = new List<ItemUsageIdleDTO>();
    }

    [XmlType(Namespace = "")]
    public class ItemUsageIdleDTO
    {
        [XmlAttribute("action")]
        public string? Action { get; set; }

        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("require_free_left_hand")]
        public string? RequireFreeLeftHand { get; set; }

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }
    }

    [XmlType(Namespace = "")]
    public class ItemUsageMovementSetsDTO
    {
        [XmlElement("movement_set")]
        public List<ItemUsageMovementSetDTO> MovementSetList { get; set; } = new List<ItemUsageMovementSetDTO>();
    }

    [XmlType(Namespace = "")]
    public class ItemUsageMovementSetDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }
    }

    [XmlType(Namespace = "")]
    public class ItemUsageUsagesDTO
    {
        [XmlElement("usage")]
        public List<ItemUsageUsageDTO> UsageList { get; set; } = new List<ItemUsageUsageDTO>();
    }

    [XmlType(Namespace = "")]
    public class ItemUsageUsageDTO
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
    }

    [XmlType(Namespace = "")]
    public class ItemUsageFlagsDTO
    {
        [XmlElement("flag")]
        public List<ItemUsageFlagDTO> FlagList { get; set; } = new List<ItemUsageFlagDTO>();
    }

    [XmlType(Namespace = "")]
    public class ItemUsageFlagDTO
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
    }
}