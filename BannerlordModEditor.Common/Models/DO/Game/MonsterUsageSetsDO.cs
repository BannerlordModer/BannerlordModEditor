using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Game;

[XmlRoot("monster_usage_sets")]
public class MonsterUsageSetsDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlElement("monster_usage_set")]
    public List<MonsterUsageSetDO> MonsterUsageSets { get; set; } = new List<MonsterUsageSetDO>();

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeMonsterUsageSets() => MonsterUsageSets != null && MonsterUsageSets.Count > 0;
}

public class MonsterUsageSetDO
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
    public MonsterUsageStrikesDO MonsterUsageStrikes { get; set; } = new MonsterUsageStrikesDO();

    [XmlIgnore]
    public bool HasMonsterUsageStrikes { get; set; } = false;

    public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    public bool ShouldSerializeHitObjectAction() => !string.IsNullOrEmpty(HitObjectAction);
    public bool ShouldSerializeHitObjectFallingAction() => !string.IsNullOrEmpty(HitObjectFallingAction);
    public bool ShouldSerializeRearAction() => !string.IsNullOrEmpty(RearAction);
    public bool ShouldSerializeRearDamagedAction() => !string.IsNullOrEmpty(RearDamagedAction);
    public bool ShouldSerializeLadderClimbAction() => !string.IsNullOrEmpty(LadderClimbAction);
    public bool ShouldSerializeStrikeLadderAction() => !string.IsNullOrEmpty(StrikeLadderAction);
    public bool ShouldSerializeMonsterUsageStrikes() => HasMonsterUsageStrikes && MonsterUsageStrikes != null && MonsterUsageStrikes.Strikes.Count > 0;
}

public class MonsterUsageStrikesDO
{
    [XmlElement("monster_usage_strike")]
    public List<MonsterUsageStrikeDO> Strikes { get; set; } = new List<MonsterUsageStrikeDO>();

    public bool ShouldSerializeStrikes() => Strikes != null && Strikes.Count > 0;
}

public class MonsterUsageStrikeDO
{
    private bool _isHeavy = false;
    private bool _isLeftStance = false;

    [XmlAttribute("is_heavy")]
    public string IsHeavy
    {
        get => _isHeavy ? "True" : "False";
        set => _isHeavy = value.Equals("True", StringComparison.OrdinalIgnoreCase) || value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    [XmlAttribute("is_left_stance")]
    public string IsLeftStance
    {
        get => _isLeftStance ? "True" : "False";
        set => _isLeftStance = value.Equals("True", StringComparison.OrdinalIgnoreCase) || value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    [XmlIgnore]
    public bool IsHeavyBool => _isHeavy;

    [XmlIgnore]
    public bool IsLeftStanceBool => _isLeftStance;

    [XmlAttribute("direction")]
    public string Direction { get; set; } = string.Empty;

    [XmlAttribute("body_part")]
    public string BodyPart { get; set; } = string.Empty;

    [XmlAttribute("impact")]
    public string Impact { get; set; } = string.Empty;

    [XmlAttribute("action")]
    public string Action { get; set; } = string.Empty;

    public bool ShouldSerializeDirection() => !string.IsNullOrEmpty(Direction);
    public bool ShouldSerializeBodyPart() => !string.IsNullOrEmpty(BodyPart);
    public bool ShouldSerializeImpact() => !string.IsNullOrEmpty(Impact);
    public bool ShouldSerializeAction() => !string.IsNullOrEmpty(Action);
}