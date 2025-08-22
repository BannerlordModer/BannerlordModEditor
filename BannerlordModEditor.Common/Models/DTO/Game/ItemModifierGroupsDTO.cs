using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Game
{
    [XmlRoot("ItemModifierGroups")]
    public class ItemModifierGroupsDTO
    {
        [XmlElement("ItemModifierGroup")]
        public List<ItemModifierGroupDTO> Groups { get; set; } = new List<ItemModifierGroupDTO>();
    }

    public class ItemModifierGroupDTO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("no_modifier_loot_score")]
        public string NoModifierLootScore { get; set; } = string.Empty;

        [XmlAttribute("no_modifier_production_score")]
        public string NoModifierProductionScore { get; set; } = string.Empty;
    }
}