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
        public bool ShouldSerializeModifierGroup() => !string.IsNullOrEmpty(ModifierGroup);

        [XmlAttribute("id")]
        public string Id { get; set; }
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);

        [XmlAttribute("name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        [XmlAttribute("loot_drop_score")]
        public string LootDropScoreString
        {
            get => LootDropScore.HasValue ? LootDropScore.Value.ToString() : null;
            set => LootDropScore = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? LootDropScore { get; set; }
        public bool ShouldSerializeLootDropScoreString() => !string.IsNullOrEmpty(LootDropScoreString);

        [XmlAttribute("production_drop_score")]
        public string ProductionDropScoreString
        {
            get => ProductionDropScore.HasValue ? ProductionDropScore.Value.ToString() : null;
            set => ProductionDropScore = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? ProductionDropScore { get; set; }
        public bool ShouldSerializeProductionDropScoreString() => !string.IsNullOrEmpty(ProductionDropScoreString);

        [XmlAttribute("damage")]
        public string DamageString
        {
            get => Damage.HasValue ? Damage.Value.ToString() : null;
            set => Damage = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Damage { get; set; }
        public bool ShouldSerializeDamageString() => !string.IsNullOrEmpty(DamageString);

        [XmlAttribute("speed")]
        public string SpeedString
        {
            get => Speed.HasValue ? Speed.Value.ToString() : null;
            set => Speed = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Speed { get; set; }
        public bool ShouldSerializeSpeedString() => !string.IsNullOrEmpty(SpeedString);

        [XmlAttribute("price_factor")]
        public string PriceFactorString
        {
            get => PriceFactor.HasValue ? PriceFactor.Value.ToString() : null;
            set => PriceFactor = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? PriceFactor { get; set; }
        public bool ShouldSerializePriceFactorString() => !string.IsNullOrEmpty(PriceFactorString);

        
        [XmlAttribute("missile_speed")]
        public string MissileSpeedString
        {
            get => MissileSpeed.HasValue ? MissileSpeed.Value.ToString() : null;
            set => MissileSpeed = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? MissileSpeed { get; set; }
        public bool ShouldSerializeMissileSpeedString() => !string.IsNullOrEmpty(MissileSpeedString);
        [XmlIgnore]
        public int? HitPoints { get; set; }
        [XmlAttribute("hit_points")]
        public string HitPointsString
        {
            get => HitPoints.HasValue ? HitPoints.Value.ToString() : null;
            set => HitPoints = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        public bool ShouldSerializeHitPointsString() => !string.IsNullOrEmpty(HitPointsString);

        [XmlAttribute("horse_speed")]
        public string HorseSpeedString
        {
            get => HorseSpeed.HasValue ? HorseSpeed.Value.ToString() : null;
            set => HorseSpeed = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        public bool ShouldSerializeHorseSpeedString() => !string.IsNullOrEmpty(HorseSpeedString);

        [XmlAttribute("stack_count")]
        public string StackCountString
        {
            get => StackCount.HasValue ? StackCount.Value.ToString() : null;
            set => StackCount = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        public bool ShouldSerializeStackCountString() => !string.IsNullOrEmpty(StackCountString);

        [XmlAttribute("armor")]
        public string ArmorString
        {
            get => Armor.HasValue ? Armor.Value.ToString() : null;
            set => Armor = string.IsNullOrEmpty(value) ? (int?)null : int.Parse(value);
        }
        [XmlIgnore]
        public int? Armor { get; set; }
        public bool ShouldSerializeArmorString() => !string.IsNullOrEmpty(ArmorString);

        
        [XmlIgnore]
        public float? HorseSpeed { get; set; }

        [XmlAttribute("maneuver")]
        public string ManeuverString
        {
            get => Maneuver.HasValue ? Maneuver.Value.ToString() : null;
            set => Maneuver = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? Maneuver { get; set; }
        public bool ShouldSerializeManeuverString() => !string.IsNullOrEmpty(ManeuverString);

        [XmlAttribute("charge_damage")]
        public string ChargeDamageString
        {
            get => ChargeDamage.HasValue ? ChargeDamage.Value.ToString() : null;
            set => ChargeDamage = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? ChargeDamage { get; set; }
        public bool ShouldSerializeChargeDamageString() => !string.IsNullOrEmpty(ChargeDamageString);

        [XmlAttribute("horse_hit_points")]
        public string HorseHitPointsString
        {
            get => HorseHitPoints.HasValue ? HorseHitPoints.Value.ToString() : null;
            set => HorseHitPoints = string.IsNullOrEmpty(value) ? (float?)null : float.Parse(value);
        }
        [XmlIgnore]
        public float? HorseHitPoints { get; set; }
        public bool ShouldSerializeHorseHitPointsString() => !string.IsNullOrEmpty(HorseHitPointsString);

        [XmlAttribute("quality")]
        public string Quality { get; set; }
        public bool ShouldSerializeQuality() => !string.IsNullOrEmpty(Quality);

        
        [XmlIgnore]
        public int? StackCount { get; set; }
    }
}