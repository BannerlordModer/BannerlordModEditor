using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("item_usage_sets")]
    public class ItemUsageSets
    {
        [XmlElement("item_usage_set")]
        public List<ItemUsageSet> ItemUsageSet { get; set; } = new List<ItemUsageSet>();

        public bool ShouldSerializeItemUsageSet() => ItemUsageSet.Count > 0;
    }

    public class ItemUsageSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

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
        public ItemUsageIdles? Idles { get; set; }

        [XmlElement("movement_sets")]
        public ItemUsageMovementSets? MovementSets { get; set; }

        [XmlElement("usages")]
        public ItemUsageUsages? Usages { get; set; }

        [XmlElement("flags")]
        public ItemUsageFlags? Flags { get; set; }
    }

    public class ItemUsageIdles
    {
        [XmlElement("idle")]
        public List<ItemUsageIdle> Idle { get; set; } = new List<ItemUsageIdle>();

        public bool ShouldSerializeIdle() => Idle.Count > 0;
    }

    public class ItemUsageIdle
    {
        [XmlAttribute("action")]
        public string Action { get; set; } = string.Empty;

        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("require_free_left_hand")]
        public string? RequireFreeLeftHand { get; set; }

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }
    }

    public class ItemUsageMovementSets
    {
        [XmlElement("movement_set")]
        public List<ItemUsageMovementSet> MovementSet { get; set; } = new List<ItemUsageMovementSet>();

        public bool ShouldSerializeMovementSet() => MovementSet.Count > 0;
    }

    public class ItemUsageMovementSet
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("require_left_hand_usage_root_set")]
        public string? RequireLeftHandUsageRootSet { get; set; }
    }

    public class ItemUsageUsages
    {
        [XmlElement("usage")]
        public List<ItemUsageUsage> Usage { get; set; } = new List<ItemUsageUsage>();

        public bool ShouldSerializeUsage() => Usage.Count > 0;
    }

    public class ItemUsageUsage
    {
        [XmlAttribute("style")]
        public string Style { get; set; } = string.Empty;

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

    public class ItemUsageFlags
    {
        [XmlElement("flag")]
        public List<ItemUsageFlag> Flag { get; set; } = new List<ItemUsageFlag>();

        public bool ShouldSerializeFlag() => Flag.Count > 0;
    }

    public class ItemUsageFlag
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
    }
} 