using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("base")]
    public class ItemHolsters
    {
        [XmlElement("item_holsters")]
        public ItemHolstersContainer ItemHolstersContainer { get; set; }
    }

    public class ItemHolstersContainer
    {
        [XmlElement("item_holster")]
        public List<ItemHolster> ItemHolsterList { get; set; } = new List<ItemHolster>();

        public bool ShouldSerializeItemHolsterList() => ItemHolsterList != null && ItemHolsterList.Count > 0;
    }

    public class ItemHolster
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("equip_action")]
        public string EquipAction { get; set; }

        [XmlAttribute("equip_action_left_stance")]
        public string EquipActionLeftStance { get; set; }

        [XmlAttribute("unequip_action")]
        public string UnequipAction { get; set; }

        [XmlAttribute("unequip_action_left_stance")]
        public string UnequipActionLeftStance { get; set; }

        [XmlAttribute("show_holster_when_drawn")]
        public string ShowHolsterWhenDrawn { get; set; }

        [XmlAttribute("group_name")]
        public string GroupName { get; set; }

        [XmlAttribute("holster_skeleton")]
        public string HolsterSkeleton { get; set; }

        [XmlAttribute("holster_bone")]
        public string HolsterBone { get; set; }

        [XmlAttribute("base_set")]
        public string BaseSet { get; set; }

        [XmlAttribute("holster_position")]
        public string HolsterPosition { get; set; }

        [XmlAttribute("holster_rotation_yaw_pitch_roll")]
        public string HolsterRotationYawPitchRoll { get; set; }

        public bool ShouldSerializeEquipAction() => !string.IsNullOrEmpty(EquipAction);
        public bool ShouldSerializeEquipActionLeftStance() => !string.IsNullOrEmpty(EquipActionLeftStance);
        public bool ShouldSerializeUnequipAction() => !string.IsNullOrEmpty(UnequipAction);
        public bool ShouldSerializeUnequipActionLeftStance() => !string.IsNullOrEmpty(UnequipActionLeftStance);
        public bool ShouldSerializeShowHolsterWhenDrawn() => !string.IsNullOrEmpty(ShowHolsterWhenDrawn);
        public bool ShouldSerializeGroupName() => !string.IsNullOrEmpty(GroupName);
        public bool ShouldSerializeHolsterSkeleton() => !string.IsNullOrEmpty(HolsterSkeleton);
        public bool ShouldSerializeHolsterBone() => !string.IsNullOrEmpty(HolsterBone);
        public bool ShouldSerializeBaseSet() => !string.IsNullOrEmpty(BaseSet);
        public bool ShouldSerializeHolsterPosition() => !string.IsNullOrEmpty(HolsterPosition);
        public bool ShouldSerializeHolsterRotationYawPitchRoll() => !string.IsNullOrEmpty(HolsterRotationYawPitchRoll);
    }
}