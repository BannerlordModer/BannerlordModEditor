using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 攻城器械类型集合 - 领域对象
    /// </summary>
    [XmlRoot("SiegeEngineTypes")]
    public class SiegeEnginesDO
    {
        [XmlElement("SiegeEngineType")]
        public List<SiegeEngineTypeDO> SiegeEngines { get; set; } = new List<SiegeEngineTypeDO>();

        [XmlIgnore]
        public bool HasEmptySiegeEngines { get; set; } = false;

        public bool ShouldSerializeSiegeEngines() => SiegeEngines != null && SiegeEngines.Count > 0 && !HasEmptySiegeEngines;
    }

    /// <summary>
    /// 攻城器械类型 - 领域对象
    /// </summary>
    public class SiegeEngineTypeDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("description")]
        public string? Description { get; set; }

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

        // ShouldSerialize 方法
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
        public bool ShouldSerializeIsConstructible() => !string.IsNullOrEmpty(IsConstructible);
        public bool ShouldSerializeManDayCost() => !string.IsNullOrEmpty(ManDayCost);
        public bool ShouldSerializeMaxHitPoints() => !string.IsNullOrEmpty(MaxHitPoints);
        public bool ShouldSerializeDifficulty() => !string.IsNullOrEmpty(Difficulty);
        public bool ShouldSerializeIsRanged() => !string.IsNullOrEmpty(IsRanged);
        public bool ShouldSerializeDamage() => !string.IsNullOrEmpty(Damage);
        public bool ShouldSerializeHitChance() => !string.IsNullOrEmpty(HitChance);
        public bool ShouldSerializeIsAntiPersonnel() => !string.IsNullOrEmpty(IsAntiPersonnel);
        public bool ShouldSerializeAntiPersonnelHitChance() => !string.IsNullOrEmpty(AntiPersonnelHitChance);
        public bool ShouldSerializeCampaignRateOfFirePerDay() => !string.IsNullOrEmpty(CampaignRateOfFirePerDay);
    }
}