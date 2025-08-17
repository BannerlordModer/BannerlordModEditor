using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DTO.Game;

[XmlRoot("monster_usage_sets")]
public class MonsterUsageSetsDTO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlElement("monster_usage_set")]
    public List<MonsterUsageSetDTO> MonsterUsageSets { get; set; } = new List<MonsterUsageSetDTO>();
}

public class MonsterUsageSetDTO
{
    [XmlAttribute("id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("hit_object_action")]
    public string HitObjectAction { get; set; } = string.Empty;

    [XmlAttribute("hit_object_falling_action")]
    public string HitObjectFallingAction { get; set; } = string.Empty;

    [XmlAttribute("rear_action")]
    public string RearAction { get; set; } = string.Empty;

    [XmlAttribute("rear_damaged_action")]
    public string RearDamagedAction { get; set; } = string.Empty;

    [XmlAttribute("ladder_climb_action")]
    public string LadderClimbAction { get; set; } = string.Empty;

    [XmlAttribute("strike_ladder_action")]
    public string StrikeLadderAction { get; set; } = string.Empty;

    [XmlElement("monster_usage_strikes")]
    public MonsterUsageStrikesDTO MonsterUsageStrikes { get; set; } = new MonsterUsageStrikesDTO();
}

public class MonsterUsageStrikesDTO
{
    [XmlElement("monster_usage_strike")]
    public List<MonsterUsageStrikeDTO> Strikes { get; set; } = new List<MonsterUsageStrikeDTO>();
}

public class MonsterUsageStrikeDTO
{
    [XmlAttribute("is_heavy")]
    public string IsHeavy { get; set; } = string.Empty;

    [XmlAttribute("is_left_stance")]
    public string IsLeftStance { get; set; } = string.Empty;

    [XmlAttribute("direction")]
    public string Direction { get; set; } = string.Empty;

    [XmlAttribute("body_part")]
    public string BodyPart { get; set; } = string.Empty;

    [XmlAttribute("impact")]
    public string Impact { get; set; } = string.Empty;

    [XmlAttribute("action")]
    public string Action { get; set; } = string.Empty;
}