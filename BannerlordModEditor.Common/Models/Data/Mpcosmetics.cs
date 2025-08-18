using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("Cosmetics")]
    public class Mpcosmetics
    {
        [XmlElement("Cosmetic")]
        public List<Cosmetic> Cosmetics { get; set; } = new List<Cosmetic>();

        public bool ShouldSerializeCosmetics() => Cosmetics != null && Cosmetics.Count > 0;
    }

    public class Cosmetic
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("rarity")]
        public string Rarity { get; set; }

        [XmlAttribute("cost")]
        public string Cost { get; set; }

        [XmlElement("Replace")]
        public Replace Replace { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeRarity() => !string.IsNullOrEmpty(Rarity);
        public bool ShouldSerializeCost() => !string.IsNullOrEmpty(Cost);
        
        // 简化实现：总是序列化Replace元素，即使为null，以匹配原始XML结构
        // 原本实现：Replace只在非null时才序列化
        // 简化实现：总是序列化Replace元素，确保XML结构一致性
        public bool ShouldSerializeReplace() => true;
    }

    public class Replace
    {
        // 简化实现：调整属性顺序以匹配原始XML中的元素顺序
        // 原本实现：Item在Itemless之前
        // 简化实现：Itemless在Item之前，以匹配实际XML结构
        [XmlElement("Itemless")]
        public List<Itemless> ItemlessList { get; set; } = new List<Itemless>();
        
        [XmlElement("Item")]
        public List<Item> Items { get; set; } = new List<Item>();

        public bool ShouldSerializeItemlessList() => ItemlessList != null && ItemlessList.Count > 0;
        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class Item
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
    
    public class Itemless
    {
        [XmlAttribute("troop")]
        public string Troop { get; set; }
        
        [XmlAttribute("slot")]
        public string Slot { get; set; }

        public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
        public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    }
}