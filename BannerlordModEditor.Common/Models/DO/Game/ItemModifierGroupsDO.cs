using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Game
{
    [XmlRoot("ItemModifierGroups")]
    public class ItemModifierGroupsDO
    {
        [XmlElement("ItemModifierGroup")]
        public List<ItemModifierGroupDO> Groups { get; set; } = new List<ItemModifierGroupDO>();

        [XmlIgnore]
        public bool HasGroups { get; set; } = false;

        public bool ShouldSerializeGroups() => HasGroups && Groups != null && Groups.Count > 0;
    }

    public class ItemModifierGroupDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("no_modifier_loot_score")]
        public string NoModifierLootScore { get; set; } = string.Empty;

        [XmlAttribute("no_modifier_production_score")]
        public string NoModifierProductionScore { get; set; } = string.Empty;

        public bool ShouldSerializeNoModifierLootScore() => !string.IsNullOrEmpty(NoModifierLootScore);
        public bool ShouldSerializeNoModifierProductionScore() => !string.IsNullOrEmpty(NoModifierProductionScore);
    }
}