using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    [XmlRoot("ItemModifiers", Namespace = "")]
    [XmlType(Namespace = "")]
    public class ItemModifiersDO
    {
        [XmlElement("ItemModifier")]
        public List<ItemModifierDO> ItemModifierList { get; set; } = new List<ItemModifierDO>();

        [XmlIgnore]
        public bool HasEmptyItemModifierList { get; set; } = false;

        public bool ShouldSerializeItemModifierList() => HasEmptyItemModifierList || (ItemModifierList != null && ItemModifierList.Count > 0);
    }

    [XmlType(Namespace = "")]
    public class ItemModifierDO
    {
        [XmlAttribute("modifier_group")]
        public string? ModifierGroup { get; set; }
        
        [XmlAttribute("id")]
        public string? Id { get; set; }
        
        [XmlAttribute("name")]
        public string? Name { get; set; }
        
        [XmlAttribute("loot_drop_score")]
        public string? LootDropScoreString
        {
            get => LootDropScore.HasValue ? LootDropScore.Value.ToString() : null;
            set => LootDropScore = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? LootDropScore { get; set; }
        
        [XmlAttribute("production_drop_score")]
        public string? ProductionDropScoreString
        {
            get => ProductionDropScore.HasValue ? ProductionDropScore.Value.ToString() : null;
            set => ProductionDropScore = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? ProductionDropScore { get; set; }
        
        [XmlAttribute("damage")]
        public string? DamageString
        {
            get => Damage.HasValue ? Damage.Value.ToString() : null;
            set => Damage = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Damage { get; set; }
        
        [XmlAttribute("speed")]
        public string? SpeedString
        {
            get => Speed.HasValue ? Speed.Value.ToString() : null;
            set => Speed = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Speed { get; set; }
        
        [XmlAttribute("missile_speed")]
        public string? MissileSpeedString
        {
            get => MissileSpeed.HasValue ? MissileSpeed.Value.ToString() : null;
            set => MissileSpeed = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? MissileSpeed { get; set; }
        
        [XmlAttribute("price_factor")]
        public string? PriceFactorString
        {
            get => PriceFactor.HasValue ? PriceFactor.Value.ToString() : null;
            set => PriceFactor = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? PriceFactor { get; set; }
        
        [XmlAttribute("quality")]
        public string? Quality { get; set; }
        
        [XmlIgnore]
        public int? HitPoints { get; set; }
        [XmlAttribute("hit_points")]
        public string? HitPointsString
        {
            get => HitPoints.HasValue ? HitPoints.Value.ToString() : null;
            set => HitPoints = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        
        [XmlAttribute("horse_speed")]
        public string? HorseSpeedString
        {
            get => HorseSpeed.HasValue ? HorseSpeed.Value.ToString() : null;
            set => HorseSpeed = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? HorseSpeed { get; set; }
        
        [XmlAttribute("stack_count")]
        public string? StackCountString
        {
            get => StackCount.HasValue ? StackCount.Value.ToString() : null;
            set => StackCount = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? StackCount { get; set; }
        
        [XmlAttribute("armor")]
        public string? ArmorString
        {
            get => Armor.HasValue ? Armor.Value.ToString() : null;
            set => Armor = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Armor { get; set; }
        
        [XmlAttribute("maneuver")]
        public string? ManeuverString
        {
            get => Maneuver.HasValue ? Maneuver.Value.ToString() : null;
            set => Maneuver = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? Maneuver { get; set; }
        
        [XmlAttribute("charge_damage")]
        public string? ChargeDamageString
        {
            get => ChargeDamage.HasValue ? ChargeDamage.Value.ToString() : null;
            set => ChargeDamage = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? ChargeDamage { get; set; }
        
        [XmlAttribute("horse_hit_points")]
        public string? HorseHitPointsString
        {
            get => HorseHitPoints.HasValue ? HorseHitPoints.Value.ToString() : null;
            set => HorseHitPoints = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? HorseHitPoints { get; set; }

        // ShouldSerialize 方法用于精确控制序列化
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrWhiteSpace(ModifierGroup);
        public bool ShouldSerializeId() => !string.IsNullOrWhiteSpace(Id);
        public bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(Name);
        public bool ShouldSerializeLootDropScoreString() => !string.IsNullOrEmpty(LootDropScoreString);
        public bool ShouldSerializeProductionDropScoreString() => !string.IsNullOrEmpty(ProductionDropScoreString);
        public bool ShouldSerializeDamageString() => !string.IsNullOrEmpty(DamageString);
        public bool ShouldSerializeSpeedString() => !string.IsNullOrEmpty(SpeedString);
        public bool ShouldSerializeMissileSpeedString() => !string.IsNullOrEmpty(MissileSpeedString);
        public bool ShouldSerializePriceFactorString() => !string.IsNullOrEmpty(PriceFactorString);
        public bool ShouldSerializeQuality() => !string.IsNullOrWhiteSpace(Quality);
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
        public bool HasQuality => !string.IsNullOrEmpty(Quality);
    }
}