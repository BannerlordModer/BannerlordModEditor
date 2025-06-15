using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game
{
    /// <summary>
    /// Root element for item_modifiers.xml - Contains item modifier definitions
    /// </summary>
    [XmlRoot("ItemModifiers")]
    public class ItemModifiers
    {
        [XmlElement("ItemModifier")]
        public List<ItemModifier> ItemModifier { get; set; } = new List<ItemModifier>();
    }

    /// <summary>
    /// Individual item modifier with stat effects and properties
    /// </summary>
    public class ItemModifier
    {
        [XmlAttribute("modifier_group")]
        public string ModifierGroup { get; set; } = string.Empty;

        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("loot_drop_score")]
        public string LootDropScore { get; set; } = string.Empty;

        [XmlAttribute("production_drop_score")]
        public string ProductionDropScore { get; set; } = string.Empty;

        [XmlAttribute("price_factor")]
        public string PriceFactor { get; set; } = string.Empty;

        [XmlAttribute("quality")]
        public string Quality { get; set; } = string.Empty;

        // Weapon stats
        [XmlAttribute("damage")]
        public string? Damage { get; set; }

        [XmlAttribute("speed")]
        public string? Speed { get; set; }

        [XmlAttribute("missile_speed")]
        public string? MissileSpeed { get; set; }

        [XmlAttribute("stack_count")]
        public string? StackCount { get; set; }

        // Armor stats
        [XmlAttribute("armor")]
        public string? Armor { get; set; }

        // Horse stats
        [XmlAttribute("horse_speed")]
        public string? HorseSpeed { get; set; }

        [XmlAttribute("maneuver")]
        public string? Maneuver { get; set; }

        [XmlAttribute("charge_damage")]
        public string? ChargeDamage { get; set; }

        [XmlAttribute("horse_hit_points")]
        public string? HorseHitPoints { get; set; }
    }
} 