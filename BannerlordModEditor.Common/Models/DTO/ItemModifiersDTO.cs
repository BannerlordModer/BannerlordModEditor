using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO
{
    [XmlRoot("ItemModifiers")]
    public class ItemModifiersDTO
    {
        [XmlElement("ItemModifier")]
        public List<ItemModifierDTO> ItemModifierList { get; set; } = new List<ItemModifierDTO>();

        public bool ShouldSerializeItemModifierList() => ItemModifierList != null && ItemModifierList.Count > 0;

        // 便捷属性
        public int Count => ItemModifierList?.Count ?? 0;
        public bool HasItemModifiers => ItemModifierList != null && ItemModifierList.Count > 0;
    }

    public class ItemModifierDTO
    {
        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }
        
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("name")]
        public string? Name { get; set; }
        
        [XmlAttribute("loot_drop_score")]
        public string? LootDropScoreString { get; set; }
        
        [XmlAttribute("production_drop_score")]
        public string? ProductionDropScoreString { get; set; }
        
        [XmlAttribute("damage")]
        public string? DamageString { get; set; }
        
        [XmlAttribute("speed")]
        public string? SpeedString { get; set; }
        
        [XmlAttribute("missile_speed")]
        public string? MissileSpeedString { get; set; }
        
        [XmlAttribute("price_factor")]
        public string? PriceFactorString { get; set; }
        
        [XmlAttribute("quality")]
        public string? Quality { get; set; }
        
        [XmlAttribute("hit_points")]
        public string? HitPointsString { get; set; }
        
        [XmlAttribute("horse_speed")]
        public string? HorseSpeedString { get; set; }
        
        [XmlAttribute("stack_count")]
        public string? StackCountString { get; set; }
        
        [XmlAttribute("armor")]
        public string? ArmorString { get; set; }
        
        [XmlAttribute("maneuver")]
        public string? ManeuverString { get; set; }
        
        [XmlAttribute("charge_damage")]
        public string? ChargeDamageString { get; set; }
        
        [XmlAttribute("horse_hit_points")]
        public string? HorseHitPointsString { get; set; }

        // ShouldSerialize方法
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrEmpty(ModifierGroup);
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeLootDropScoreString() => !string.IsNullOrEmpty(LootDropScoreString);
        public bool ShouldSerializeProductionDropScoreString() => !string.IsNullOrEmpty(ProductionDropScoreString);
        public bool ShouldSerializeDamageString() => !string.IsNullOrEmpty(DamageString);
        public bool ShouldSerializeSpeedString() => !string.IsNullOrEmpty(SpeedString);
        public bool ShouldSerializeMissileSpeedString() => !string.IsNullOrEmpty(MissileSpeedString);
        public bool ShouldSerializePriceFactorString() => !string.IsNullOrEmpty(PriceFactorString);
        public bool ShouldSerializeQuality() => !string.IsNullOrEmpty(Quality);
        public bool ShouldSerializeHitPointsString() => !string.IsNullOrEmpty(HitPointsString);
        public bool ShouldSerializeHorseSpeedString() => !string.IsNullOrEmpty(HorseSpeedString);
        public bool ShouldSerializeStackCountString() => !string.IsNullOrEmpty(StackCountString);
        public bool ShouldSerializeArmorString() => !string.IsNullOrEmpty(ArmorString);
        public bool ShouldSerializeManeuverString() => !string.IsNullOrEmpty(ManeuverString);
        public bool ShouldSerializeChargeDamageString() => !string.IsNullOrEmpty(ChargeDamageString);
        public bool ShouldSerializeHorseHitPointsString() => !string.IsNullOrEmpty(HorseHitPointsString);

        // 便捷属性
        public bool HasModifierGroup => !string.IsNullOrEmpty(ModifierGroup);
        public bool HasId => !string.IsNullOrEmpty(Id);
        public bool HasName => !string.IsNullOrEmpty(Name);
        public bool HasLootDropScore => !string.IsNullOrEmpty(LootDropScoreString);
        public bool HasProductionDropScore => !string.IsNullOrEmpty(ProductionDropScoreString);
        public bool HasDamage => !string.IsNullOrEmpty(DamageString);
        public bool HasSpeed => !string.IsNullOrEmpty(SpeedString);
        public bool HasMissileSpeed => !string.IsNullOrEmpty(MissileSpeedString);
        public bool HasPriceFactor => !string.IsNullOrEmpty(PriceFactorString);
        public bool HasQuality => !string.IsNullOrEmpty(Quality);
        public bool HasHitPoints => !string.IsNullOrEmpty(HitPointsString);
        public bool HasHorseSpeed => !string.IsNullOrEmpty(HorseSpeedString);
        public bool HasStackCount => !string.IsNullOrEmpty(StackCountString);
        public bool HasArmor => !string.IsNullOrEmpty(ArmorString);
        public bool HasManeuver => !string.IsNullOrEmpty(ManeuverString);
        public bool HasChargeDamage => !string.IsNullOrEmpty(ChargeDamageString);
        public bool HasHorseHitPoints => !string.IsNullOrEmpty(HorseHitPointsString);

        // 类型安全的便捷属性
        public int? LootDropScoreInt => int.TryParse(LootDropScoreString, out int val) ? val : (int?)null;
        public int? ProductionDropScoreInt => int.TryParse(ProductionDropScoreString, out int val) ? val : (int?)null;
        public int? DamageInt => int.TryParse(DamageString, out int val) ? val : (int?)null;
        public int? SpeedInt => int.TryParse(SpeedString, out int val) ? val : (int?)null;
        public int? MissileSpeedInt => int.TryParse(MissileSpeedString, out int val) ? val : (int?)null;
        public float? PriceFactorFloat => float.TryParse(PriceFactorString, out float val) ? val : (float?)null;
        public int? HitPointsInt => int.TryParse(HitPointsString, out int val) ? val : (int?)null;
        public float? HorseSpeedFloat => float.TryParse(HorseSpeedString, out float val) ? val : (float?)null;
        public int? StackCountInt => int.TryParse(StackCountString, out int val) ? val : (int?)null;
        public int? ArmorInt => int.TryParse(ArmorString, out int val) ? val : (int?)null;
        public float? ManeuverFloat => float.TryParse(ManeuverString, out float val) ? val : (float?)null;
        public float? ChargeDamageFloat => float.TryParse(ChargeDamageString, out float val) ? val : (float?)null;
        public float? HorseHitPointsFloat => float.TryParse(HorseHitPointsString, out float val) ? val : (float?)null;

        // 设置方法
        public void SetLootDropScoreInt(int? value) => LootDropScoreString = value?.ToString();
        public void SetProductionDropScoreInt(int? value) => ProductionDropScoreString = value?.ToString();
        public void SetDamageInt(int? value) => DamageString = value?.ToString();
        public void SetSpeedInt(int? value) => SpeedString = value?.ToString();
        public void SetMissileSpeedInt(int? value) => MissileSpeedString = value?.ToString();
        public void SetPriceFactorFloat(float? value) => PriceFactorString = value?.ToString();
        public void SetHitPointsInt(int? value) => HitPointsString = value?.ToString();
        public void SetHorseSpeedFloat(float? value) => HorseSpeedString = value?.ToString();
        public void SetStackCountInt(int? value) => StackCountString = value?.ToString();
        public void SetArmorInt(int? value) => ArmorString = value?.ToString();
        public void SetManeuverFloat(float? value) => ManeuverString = value?.ToString();
        public void SetChargeDamageFloat(float? value) => ChargeDamageString = value?.ToString();
        public void SetHorseHitPointsFloat(float? value) => HorseHitPointsString = value?.ToString();
    }
}