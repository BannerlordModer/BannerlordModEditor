using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("ItemModifierGroups")]
    public class ItemModifiersGroups
    {
        [XmlElement("ItemModifierGroup")]
        public List<ItemModifierGroup> ItemModifierGroups { get; set; } = new List<ItemModifierGroup>();

        public bool ShouldSerializeItemModifierGroups() => ItemModifierGroups != null && ItemModifierGroups.Count > 0;
    }

    public class ItemModifierGroup
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("no_modifier_loot_score")]
        public int NoModifierLootScore { get; set; }

        public bool ShouldSerializeNoModifierLootScore() => true;

        [XmlAttribute("no_modifier_production_score")]
        public int NoModifierProductionScore { get; set; }

        public bool ShouldSerializeNoModifierProductionScore() => true;
    }
}