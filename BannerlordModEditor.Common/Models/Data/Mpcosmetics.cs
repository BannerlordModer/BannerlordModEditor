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
        public bool ShouldSerializeReplace() => Replace != null && Replace.Items != null && Replace.Items.Count > 0;
    }

    public class Replace
    {
        [XmlElement("Item")]
        public List<Item> Items { get; set; } = new List<Item>();

        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class Item
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
}