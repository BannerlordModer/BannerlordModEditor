using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("base")]
    public class ItemHolstersDO
    {
        [XmlElement("item_holsters")]
        public ItemHolstersContainerDO ItemHolstersContainer { get; set; }
        
        // 标记是否应该序列化空的item_holsters元素
        [XmlIgnore]
        public bool HasEmptyItemHolsters { get; set; } = false;

        public bool ShouldSerializeItemHolstersContainer() => HasEmptyItemHolsters || ItemHolstersContainer != null;
    }

    public class ItemHolstersContainerDO
    {
        [XmlElement("item_holster")]
        public List<ItemHolsterDO> ItemHolsterList { get; set; } = new List<ItemHolsterDO>();

        public bool ShouldSerializeItemHolsterList() => ItemHolsterList != null && ItemHolsterList.Count > 0;
    }

    public class ItemHolsterDO
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

        public bool ShouldSerializeEquipAction() => EquipAction != null;
        public bool ShouldSerializeEquipActionLeftStance() => EquipActionLeftStance != null;
        public bool ShouldSerializeUnequipAction() => UnequipAction != null;
        public bool ShouldSerializeUnequipActionLeftStance() => UnequipActionLeftStance != null;
        public bool ShouldSerializeShowHolsterWhenDrawn() => ShowHolsterWhenDrawn != null;
        public bool ShouldSerializeGroupName() => GroupName != null;
        public bool ShouldSerializeHolsterSkeleton() => HolsterSkeleton != null;
        public bool ShouldSerializeHolsterBone() => HolsterBone != null;
        public bool ShouldSerializeBaseSet() => BaseSet != null;
        public bool ShouldSerializeHolsterPosition() => HolsterPosition != null;
        public bool ShouldSerializeHolsterRotationYawPitchRoll() => HolsterRotationYawPitchRoll != null;
    }
}