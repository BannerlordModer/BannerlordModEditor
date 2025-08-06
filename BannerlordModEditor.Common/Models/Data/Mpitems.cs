using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Items")]
    public class Mpitems
    {
        [XmlElement("Item")]
        public List<MpitemsItem> Items { get; set; } = new List<MpitemsItem>();
    }

    public class MpitemsItem
    {
        [XmlAttribute("multiplayer_item")]
        public bool? MultiplayerItem { get; set; }
        public bool ShouldSerializeMultiplayerItem() => MultiplayerItem.HasValue;

        [XmlAttribute("id")]
        public string Id { get; set; }
        public bool ShouldSerializeId() => Id != null;

        [XmlAttribute("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() => Name != null;

        [XmlAttribute("subtype")]
        public string Subtype { get; set; }
        public bool ShouldSerializeSubtype() => Subtype != null;

        [XmlAttribute("mesh")]
        public string Mesh { get; set; }
        public bool ShouldSerializeMesh() => Mesh != null;

        [XmlAttribute("culture")]
        public string Culture { get; set; }
        public bool ShouldSerializeCulture() => Culture != null;

        [XmlAttribute("value")]
        public int? Value { get; set; }
        public bool ShouldSerializeValue() => Value.HasValue;

        [XmlAttribute("is_merchandise")]
        public bool? IsMerchandise { get; set; }
        public bool ShouldSerializeIsMerchandise() => IsMerchandise.HasValue;

        [XmlAttribute("weight")]
        public float? Weight { get; set; }
        public bool ShouldSerializeWeight() => Weight.HasValue;

        [XmlAttribute("difficulty")]
        public int? Difficulty { get; set; }
        public bool ShouldSerializeDifficulty() => Difficulty.HasValue;

        [XmlAttribute("appearance")]
        public float? Appearance { get; set; }
        public bool ShouldSerializeAppearance() => Appearance.HasValue;

        [XmlAttribute("Type")]
        public string Type { get; set; }
        public bool ShouldSerializeType() => Type != null;

        [XmlElement("ItemComponent")]
        public MpitemsItemComponent ItemComponent { get; set; }
        public bool ShouldSerializeItemComponent() => ItemComponent != null;

        [XmlElement("Flags")]
        public MpitemsFlags Flags { get; set; }
        public bool ShouldSerializeFlags() => Flags != null;
    }

    public class MpitemsItemComponent
    {
        [XmlElement("Armor")]
        public MpitemsArmor Armor { get; set; }
        public bool ShouldSerializeArmor() => Armor != null;
    }

    public class MpitemsArmor
    {
        [XmlAttribute("head_armor")]
        public int? HeadArmor { get; set; }
        public bool ShouldSerializeHeadArmor() => HeadArmor.HasValue;

        [XmlAttribute("body_armor")]
        public int? BodyArmor { get; set; }
        public bool ShouldSerializeBodyArmor() => BodyArmor.HasValue;

        [XmlAttribute("leg_armor")]
        public int? LegArmor { get; set; }
        public bool ShouldSerializeLegArmor() => LegArmor.HasValue;

        [XmlAttribute("arm_armor")]
        public int? ArmArmor { get; set; }
        public bool ShouldSerializeArmArmor() => ArmArmor.HasValue;

        [XmlAttribute("has_gender_variations")]
        public bool? HasGenderVariations { get; set; }
        public bool ShouldSerializeHasGenderVariations() => HasGenderVariations.HasValue;

        [XmlAttribute("hair_cover_type")]
        public string HairCoverType { get; set; }
        public bool ShouldSerializeHairCoverType() => HairCoverType != null;

        [XmlAttribute("beard_cover_type")]
        public string BeardCoverType { get; set; }
        public bool ShouldSerializeBeardCoverType() => BeardCoverType != null;

        [XmlAttribute("modifier_group")]
        public string ModifierGroup { get; set; }
        public bool ShouldSerializeModifierGroup() => ModifierGroup != null;

        [XmlAttribute("material_type")]
        public string MaterialType { get; set; }
        public bool ShouldSerializeMaterialType() => MaterialType != null;

        [XmlAttribute("covers_body")]
        public bool? CoversBody { get; set; }
        public bool ShouldSerializeCoversBody() => CoversBody.HasValue;

        [XmlAttribute("covers_hands")]
        public bool? CoversHands { get; set; }
        public bool ShouldSerializeCoversHands() => CoversHands.HasValue;

        [XmlAttribute("covers_legs")]
        public bool? CoversLegs { get; set; }
        public bool ShouldSerializeCoversLegs() => CoversLegs.HasValue;
    }

    public class MpitemsFlags
    {
        [XmlAttribute("UseTeamColor")]
        public bool? UseTeamColor { get; set; }
        public bool ShouldSerializeUseTeamColor() => UseTeamColor.HasValue;

        [XmlAttribute("Civilian")]
        public bool? Civilian { get; set; }
        public bool ShouldSerializeCivilian() => Civilian.HasValue;
    }
}