using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("Cosmetics")]
    public class MpcosmeticsDO
    {
        [XmlElement("Cosmetic")]
        public List<CosmeticDO> Cosmetics { get; set; } = new List<CosmeticDO>();
        
        [XmlIgnore]
        public bool HasEmptyCosmetics { get; set; } = false;

        public bool ShouldSerializeCosmetics() => HasEmptyCosmetics || (Cosmetics != null && Cosmetics.Count > 0);
    }

    public class CosmeticDO
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("rarity")]
        public string? Rarity { get; set; }

        [XmlAttribute("cost")]
        public string? Cost { get; set; }

        [XmlElement("Replace")]
        public ReplaceDO? Replace { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeRarity() => !string.IsNullOrEmpty(Rarity);
        public bool ShouldSerializeCost() => !string.IsNullOrEmpty(Cost);
        public bool ShouldSerializeReplace() => Replace != null;
    }

    public class ReplaceDO
    {
        [XmlElement("Itemless")]
        public List<ItemlessDO> ItemlessList { get; set; } = new List<ItemlessDO>();
        
        [XmlElement("Item")]
        public List<MpcosmeticItemDO> Items { get; set; } = new List<MpcosmeticItemDO>();

        public bool ShouldSerializeItemlessList() => ItemlessList != null && ItemlessList.Count > 0;
        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class MpcosmeticItemDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
    
    public class ItemlessDO
    {
        [XmlAttribute("troop")]
        public string? Troop { get; set; }
        
        [XmlAttribute("slot")]
        public string? Slot { get; set; }

        public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
        public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    }
}