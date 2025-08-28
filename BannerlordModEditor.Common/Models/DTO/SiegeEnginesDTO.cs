using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    /// <summary>
    /// 攻城器械类型集合 - 数据传输对象
    /// </summary>
    [XmlRoot("SiegeEngineTypes")]
    public class SiegeEnginesDTO
    {
        [XmlElement("SiegeEngineType")]
        public List<SiegeEngineTypeDTO> SiegeEngines { get; set; } = new List<SiegeEngineTypeDTO>();

        [XmlIgnore]
        public bool HasEmptySiegeEngines { get; set; } = false;

        // ShouldSerialize方法（对应DO层）
        public bool ShouldSerializeSiegeEngines() => SiegeEngines != null && SiegeEngines.Count > 0 && !HasEmptySiegeEngines;

        // 便捷属性
        public int SiegeEnginesCount => SiegeEngines?.Count ?? 0;
    }

    /// <summary>
    /// 攻城器械类型 - 数据传输对象
    /// </summary>
    public class SiegeEngineTypeDTO
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

        // ShouldSerialize方法 - 修复：与DO层保持一致，使用!string.IsNullOrEmpty()
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

        // 类型安全的便捷属性
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasName => !string.IsNullOrEmpty(Name);
        public bool HasDescription => !string.IsNullOrEmpty(Description);
        public bool HasIsConstructible => !string.IsNullOrEmpty(IsConstructible);
        public bool HasManDayCost => !string.IsNullOrEmpty(ManDayCost);
        public bool HasMaxHitPoints => !string.IsNullOrEmpty(MaxHitPoints);
        public bool HasDifficulty => !string.IsNullOrEmpty(Difficulty);
        public bool HasIsRanged => !string.IsNullOrEmpty(IsRanged);
        public bool HasDamage => !string.IsNullOrEmpty(Damage);
        public bool HasHitChance => !string.IsNullOrEmpty(HitChance);
        public bool HasIsAntiPersonnel => !string.IsNullOrEmpty(IsAntiPersonnel);
        public bool HasAntiPersonnelHitChance => !string.IsNullOrEmpty(AntiPersonnelHitChance);
        public bool HasCampaignRateOfFirePerDay => !string.IsNullOrEmpty(CampaignRateOfFirePerDay);
    }
}