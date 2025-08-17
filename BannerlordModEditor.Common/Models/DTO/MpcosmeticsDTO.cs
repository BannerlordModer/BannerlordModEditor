using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("Cosmetics")]
    public class MpcosmeticsDTO
    {
        [XmlElement("Cosmetic")]
        public List<CosmeticDTO> Cosmetics { get; set; } = new List<CosmeticDTO>();

        public bool ShouldSerializeCosmetics() => Cosmetics != null && Cosmetics.Count > 0;
    }

    public class CosmeticDTO
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
        public ReplaceDTO? Replace { get; set; }

        public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeRarity() => !string.IsNullOrEmpty(Rarity);
        public bool ShouldSerializeCost() => !string.IsNullOrEmpty(Cost);
        public bool ShouldSerializeReplace() => Replace != null;
    }

    public class ReplaceDTO
    {
        [XmlElement("Itemless")]
        public List<ItemlessDTO> ItemlessList { get; set; } = new List<ItemlessDTO>();
        
        [XmlElement("Item")]
        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();

        public bool ShouldSerializeItemlessList() => ItemlessList != null && ItemlessList.Count > 0;
        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
    }

    public class ItemDTO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
    
    public class ItemlessDTO
    {
        [XmlAttribute("troop")]
        public string? Troop { get; set; }
        
        [XmlAttribute("slot")]
        public string? Slot { get; set; }

        public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
        public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    }
}