using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Multiplayer;

[XmlRoot("MPClassDivisions")]
public class MPClassDivisionsDO
{
    [XmlElement("MPClassDivision")]
    public List<MPClassDivisionDO> MPClassDivisions { get; set; } = new List<MPClassDivisionDO>();

    public bool ShouldSerializeMPClassDivisions() => MPClassDivisions != null && MPClassDivisions.Count > 0;
}

public class MPClassDivisionDO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("hero")]
    public string Hero { get; set; } = string.Empty;

    [XmlAttribute("troop")]
    public string Troop { get; set; } = string.Empty;

    [XmlAttribute("hero_idle_anim")]
    public string HeroIdleAnim { get; set; } = string.Empty;

    [XmlAttribute("troop_idle_anim")]
    public string TroopIdleAnim { get; set; } = string.Empty;

    [XmlAttribute("multiplier")]
    public string Multiplier { get; set; } = string.Empty;

    [XmlAttribute("cost")]
    public string Cost { get; set; } = string.Empty;

    [XmlAttribute("casual_cost")]
    public string CasualCost { get; set; } = string.Empty;

    [XmlAttribute("icon")]
    public string Icon { get; set; } = string.Empty;

    [XmlAttribute("melee_ai")]
    public string MeleeAI { get; set; } = string.Empty;

    [XmlAttribute("ranged_ai")]
    public string RangedAI { get; set; } = string.Empty;

    [XmlAttribute("armor")]
    public string Armor { get; set; } = string.Empty;

    [XmlAttribute("movement_speed")]
    public string MovementSpeed { get; set; } = string.Empty;

    [XmlAttribute("combat_movement_speed")]
    public string CombatMovementSpeed { get; set; } = string.Empty;

    [XmlAttribute("acceleration")]
    public string Acceleration { get; set; } = string.Empty;

    [XmlElement("Perks")]
    public PerksDO Perks { get; set; } = new PerksDO();

    [XmlIgnore]
    public bool HasPerks { get; set; } = false;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeHero() => !string.IsNullOrEmpty(Hero);
    public bool ShouldSerializeTroop() => !string.IsNullOrEmpty(Troop);
    public bool ShouldSerializeHeroIdleAnim() => !string.IsNullOrEmpty(HeroIdleAnim);
    public bool ShouldSerializeTroopIdleAnim() => !string.IsNullOrEmpty(TroopIdleAnim);
    public bool ShouldSerializeMultiplier() => !string.IsNullOrEmpty(Multiplier);
    public bool ShouldSerializeCost() => !string.IsNullOrEmpty(Cost);
    public bool ShouldSerializeCasualCost() => !string.IsNullOrEmpty(CasualCost);
    public bool ShouldSerializeIcon() => !string.IsNullOrEmpty(Icon);
    public bool ShouldSerializeMeleeAI() => !string.IsNullOrEmpty(MeleeAI);
    public bool ShouldSerializeRangedAI() => !string.IsNullOrEmpty(RangedAI);
    public bool ShouldSerializeArmor() => !string.IsNullOrEmpty(Armor);
    public bool ShouldSerializeMovementSpeed() => !string.IsNullOrEmpty(MovementSpeed);
    public bool ShouldSerializeCombatMovementSpeed() => !string.IsNullOrEmpty(CombatMovementSpeed);
    public bool ShouldSerializeAcceleration() => !string.IsNullOrEmpty(Acceleration);
    public bool ShouldSerializePerks() => HasPerks && Perks != null && Perks.PerksList.Count > 0;
}

public class PerksDO
{
    [XmlElement("Perk")]
    public List<PerkDO> PerksList { get; set; } = new List<PerkDO>();

    public bool ShouldSerializePerksList() => PerksList != null && PerksList.Count > 0;
}

public class PerkDO
{
    [XmlAttribute("game_mode")]
    public string GameMode { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;

    [XmlAttribute("description")]
    public string Description { get; set; } = string.Empty;

    [XmlAttribute("icon")]
    public string Icon { get; set; } = string.Empty;

    [XmlAttribute("hero_idle_anim")]
    public string HeroIdleAnim { get; set; } = string.Empty;

    [XmlAttribute("perk_list")]
    public string PerkList { get; set; } = string.Empty;

    [XmlElement("OnSpawnEffect")]
    public List<OnSpawnEffectDO> OnSpawnEffects { get; set; } = new List<OnSpawnEffectDO>();

    [XmlElement("RandomOnSpawnEffect")]
    public List<RandomOnSpawnEffectDO> RandomOnSpawnEffects { get; set; } = new List<RandomOnSpawnEffectDO>();

    [XmlElement("Effect")]
    public List<EffectDO> Effects { get; set; } = new List<EffectDO>();

    public bool ShouldSerializeGameMode() => !string.IsNullOrEmpty(GameMode);
    public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    public bool ShouldSerializeDescription() => !string.IsNullOrEmpty(Description);
    public bool ShouldSerializeIcon() => !string.IsNullOrEmpty(Icon);
    public bool ShouldSerializeHeroIdleAnim() => !string.IsNullOrEmpty(HeroIdleAnim);
    public bool ShouldSerializePerkList() => !string.IsNullOrEmpty(PerkList);
    public bool ShouldSerializeOnSpawnEffects() => OnSpawnEffects != null && OnSpawnEffects.Count > 0;
    public bool ShouldSerializeRandomOnSpawnEffects() => RandomOnSpawnEffects != null && RandomOnSpawnEffects.Count > 0;
    public bool ShouldSerializeEffects() => Effects != null && Effects.Count > 0;
}

public class OnSpawnEffectDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;

    [XmlAttribute("slot")]
    public string Slot { get; set; } = string.Empty;

    [XmlAttribute("item")]
    public string Item { get; set; } = string.Empty;

    [XmlAttribute("target")]
    public string Target { get; set; } = string.Empty;

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeItem() => !string.IsNullOrEmpty(Item);
    public bool ShouldSerializeTarget() => !string.IsNullOrEmpty(Target);
}

public class RandomOnSpawnEffectDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("target")]
    public string Target { get; set; } = string.Empty;

    [XmlElement("Group")]
    public List<GroupDO> Groups { get; set; } = new List<GroupDO>();

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeTarget() => !string.IsNullOrEmpty(Target);
    public bool ShouldSerializeGroups() => Groups != null && Groups.Count > 0;
}

public class EffectDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

public class GroupDO
{
    [XmlElement("Item")]
    public List<ItemDO> Items { get; set; } = new List<ItemDO>();

    public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
}

public class ItemDO
{
    [XmlAttribute("slot")]
    public string Slot { get; set; } = string.Empty;

    [XmlAttribute("item")]
    public string Item { get; set; } = string.Empty;

    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeItem() => !string.IsNullOrEmpty(Item);
}