using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Game;

// item_modifiers_groups.xml - Item modifier group configurations for loot and production scoring
[XmlRoot("ItemModifierGroups")]
public class ItemModifierGroups
{
    [XmlElement("ItemModifierGroup")]
    public List<ItemModifierGroup> ItemModifierGroup { get; set; } = new();
}

public class ItemModifierGroup
{
    [XmlAttribute("id")]
    public string? Id { get; set; }

    [XmlAttribute("no_modifier_loot_score")]
    public string? NoModifierLootScore { get; set; }

    [XmlAttribute("no_modifier_production_score")]
    public string? NoModifierProductionScore { get; set; }
} 