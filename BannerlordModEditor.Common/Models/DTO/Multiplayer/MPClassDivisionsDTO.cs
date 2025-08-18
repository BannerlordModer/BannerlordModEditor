using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Multiplayer;

[XmlRoot("MPClassDivisions")]
public class MPClassDivisionsDTO
{
    [XmlElement("MPClassDivision")]
    public List<MPClassDivisionDTO> MPClassDivisions { get; set; } = new List<MPClassDivisionDTO>();
}

public class MPClassDivisionDTO
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
    public PerksDTO Perks { get; set; } = new PerksDTO();
}

public class PerksDTO
{
    [XmlElement("Perk")]
    public List<PerkDTO> PerksList { get; set; } = new List<PerkDTO>();
}

public class PerkDTO
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
    public List<OnSpawnEffectDTO> OnSpawnEffects { get; set; } = new List<OnSpawnEffectDTO>();

    [XmlElement("RandomOnSpawnEffect")]
    public List<RandomOnSpawnEffectDTO> RandomOnSpawnEffects { get; set; } = new List<RandomOnSpawnEffectDTO>();

    [XmlElement("Effect")]
    public List<EffectDTO> Effects { get; set; } = new List<EffectDTO>();
}

public class OnSpawnEffectDTO
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
}

public class RandomOnSpawnEffectDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("target")]
    public string Target { get; set; } = string.Empty;

    [XmlElement("Group")]
    public List<GroupDTO> Groups { get; set; } = new List<GroupDTO>();
}

public class EffectDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;
}

public class GroupDTO
{
    [XmlElement("Item")]
    public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
}

public class ItemDTO
{
    [XmlAttribute("slot")]
    public string Slot { get; set; } = string.Empty;

    [XmlAttribute("item")]
    public string Item { get; set; } = string.Empty;
}