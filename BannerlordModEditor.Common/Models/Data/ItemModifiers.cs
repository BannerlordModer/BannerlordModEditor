using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("ItemModifiers")]
    public class ItemModifiers
    {
        [XmlElement("ItemModifier")]
        public List<ItemModifier> ItemModifierList { get; set; } = new List<ItemModifier>();
    }

    public class ItemModifier
    {
        [XmlAttribute("modifier_group")]
        public string ModifierGroup { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("loot_drop_score")]
        public int? LootDropScore { get; set; }
        public bool ShouldSerializeLootDropScore() => LootDropScore.HasValue;

        [XmlAttribute("production_drop_score")]
        public int? ProductionDropScore { get; set; }
        public bool ShouldSerializeProductionDropScore() => ProductionDropScore.HasValue;

        [XmlAttribute("damage")]
        public int? Damage { get; set; }
        public bool ShouldSerializeDamage() => Damage.HasValue;

        [XmlAttribute("speed")]
        public int? Speed { get; set; }
        public bool ShouldSerializeSpeed() => Speed.HasValue;

        [XmlAttribute("missile_speed")]
        public int? MissileSpeed { get; set; }
        public bool ShouldSerializeMissileSpeed() => MissileSpeed.HasValue;

        [XmlAttribute("stack_count")]
        public int? StackCount { get; set; }
        public bool ShouldSerializeStackCount() => StackCount.HasValue;

        [XmlAttribute("price_factor")]
        public float? PriceFactor { get; set; }
        public bool ShouldSerializePriceFactor() => PriceFactor.HasValue;

        [XmlAttribute("quality")]
        public string Quality { get; set; }
        public bool ShouldSerializeQuality() => !string.IsNullOrEmpty(Quality);
    }
}