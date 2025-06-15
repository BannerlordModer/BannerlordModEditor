using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models
{
    [XmlRoot("SiegeEngineTypes")]
    public class SiegeEngineTypes
    {
        [XmlElement("SiegeEngineType")]
        public List<SiegeEngineType> SiegeEngineType { get; set; } = new List<SiegeEngineType>();
    }

    public class SiegeEngineType
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("description")]
        public string Description { get; set; } = string.Empty;

        [XmlAttribute("is_constructible")]
        public string? IsConstructible { get; set; }

        [XmlAttribute("man_day_cost")]
        public string? ManDayCost { get; set; }

        [XmlAttribute("max_hit_points")]
        public string? MaxHitPoints { get; set; }

        [XmlAttribute("difficulty")]
        public string? Difficulty { get; set; }

        [XmlAttribute("is_ranged")]
        public string? IsRanged { get; set; }

        [XmlAttribute("damage")]
        public string? Damage { get; set; }

        [XmlAttribute("hit_chance")]
        public string? HitChance { get; set; }

        [XmlAttribute("is_anti_personnel")]
        public string? IsAntiPersonnel { get; set; }

        [XmlAttribute("anti_personnel_hit_chance")]
        public string? AntiPersonnelHitChance { get; set; }

        [XmlAttribute("campaign_rate_of_fire_per_day")]
        public string? CampaignRateOfFirePerDay { get; set; }
    }
} 