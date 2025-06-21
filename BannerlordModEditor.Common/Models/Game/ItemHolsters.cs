using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game
{
    [XmlRoot("base")]
    public class ItemHolstersRoot
    {
        [XmlElement("item_holsters")]
        public ItemHolsters? ItemHolsters { get; set; }
    }

    public class ItemHolsters
    {
        [XmlElement("item_holster")]
        public ItemHolster[]? ItemHolster { get; set; }
    }

    public class ItemHolster
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("equip_action")]
        public string? EquipAction { get; set; }

        [XmlAttribute("equip_action_left_stance")]
        public string? EquipActionLeftStance { get; set; }

        [XmlAttribute("unequip_action")]
        public string? UnequipAction { get; set; }

        [XmlAttribute("unequip_action_left_stance")]
        public string? UnequipActionLeftStance { get; set; }

        [XmlAttribute("show_holster_when_drawn")]
        public string? ShowHolsterWhenDrawn { get; set; }

        [XmlAttribute("group_name")]
        public string? GroupName { get; set; }

        [XmlAttribute("holster_skeleton")]
        public string? HolsterSkeleton { get; set; }

        [XmlAttribute("holster_bone")]
        public string? HolsterBone { get; set; }

        [XmlAttribute("base_set")]
        public string? BaseSet { get; set; }

        [XmlAttribute("holster_position")]
        public string? HolsterPosition { get; set; }

        [XmlAttribute("holster_rotation_yaw_pitch_roll")]
        public string? HolsterRotationYawPitchRoll { get; set; }
    }
} 