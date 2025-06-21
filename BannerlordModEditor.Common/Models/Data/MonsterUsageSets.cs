using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.Data
{
    [XmlRoot("monster_usage_sets")]
    public class MonsterUsageSets
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlElement("monster_usage_set")]
        public MonsterUsageSet[]? MonsterUsageSet { get; set; }
    }

    public class MonsterUsageSet
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        [XmlAttribute("hit_object_action")]
        public string? HitObjectAction { get; set; }

        [XmlAttribute("hit_object_falling_action")]
        public string? HitObjectFallingAction { get; set; }

        [XmlAttribute("rear_action")]
        public string? RearAction { get; set; }

        [XmlAttribute("rear_damaged_action")]
        public string? RearDamagedAction { get; set; }

        [XmlAttribute("ladder_climb_action")]
        public string? LadderClimbAction { get; set; }

        [XmlAttribute("strike_ladder_action")]
        public string? StrikeLadderAction { get; set; }

        [XmlElement("monster_usage_strikes")]
        public MonsterUsageStrikes? MonsterUsageStrikes { get; set; }

        [XmlElement("monster_usage_movements")]
        public MonsterUsageMovements? MonsterUsageMovements { get; set; }

        [XmlElement("monster_usage_upper_body_movements")]
        public MonsterUsageUpperBodyMovements? MonsterUsageUpperBodyMovements { get; set; }

        [XmlElement("monster_usage_jumps")]
        public MonsterUsageJumps? MonsterUsageJumps { get; set; }

        [XmlElement("monster_usage_falls")]
        public MonsterUsageFalls? MonsterUsageFalls { get; set; }
    }

    public class MonsterUsageStrikes
    {
        [XmlElement("monster_usage_strike")]
        public MonsterUsageStrike[]? MonsterUsageStrike { get; set; }
    }

    public class MonsterUsageStrike
    {
        [XmlAttribute("is_heavy")]
        public string? IsHeavy { get; set; }

        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("direction")]
        public string? Direction { get; set; }

        [XmlAttribute("body_part")]
        public string? BodyPart { get; set; }

        [XmlAttribute("impact")]
        public string? Impact { get; set; }

        [XmlAttribute("action")]
        public string? Action { get; set; }
    }

    public class MonsterUsageMovements
    {
        [XmlElement("monster_usage_movement")]
        public MonsterUsageMovement[]? MonsterUsageMovement { get; set; }
    }

    public class MonsterUsageMovement
    {
        [XmlAttribute("is_left_foot")]
        public string? IsLeftFoot { get; set; }

        [XmlAttribute("pace")]
        public string? Pace { get; set; }

        [XmlAttribute("direction")]
        public string? Direction { get; set; }

        [XmlAttribute("turn_direction")]
        public string? TurnDirection { get; set; }

        [XmlAttribute("action")]
        public string? Action { get; set; }
    }

    public class MonsterUsageUpperBodyMovements
    {
        [XmlElement("monster_usage_upper_body_movement")]
        public MonsterUsageUpperBodyMovement[]? MonsterUsageUpperBodyMovement { get; set; }
    }

    public class MonsterUsageUpperBodyMovement
    {
        [XmlAttribute("pace")]
        public string? Pace { get; set; }

        [XmlAttribute("direction")]
        public string? Direction { get; set; }

        [XmlAttribute("action")]
        public string? Action { get; set; }
    }

    public class MonsterUsageJumps
    {
        [XmlElement("monster_usage_jump")]
        public MonsterUsageJump[]? MonsterUsageJump { get; set; }
    }

    public class MonsterUsageJump
    {
        [XmlAttribute("jump_state")]
        public string? JumpState { get; set; }

        [XmlAttribute("direction")]
        public string? Direction { get; set; }

        [XmlAttribute("action")]
        public string? Action { get; set; }

        [XmlAttribute("is_hard")]
        public string? IsHard { get; set; }
    }

    public class MonsterUsageFalls
    {
        [XmlElement("monster_usage_fall")]
        public MonsterUsageFall[]? MonsterUsageFall { get; set; }
    }

    public class MonsterUsageFall
    {
        [XmlAttribute("is_heavy")]
        public string? IsHeavy { get; set; }

        [XmlAttribute("is_left_stance")]
        public string? IsLeftStance { get; set; }

        [XmlAttribute("direction")]
        public string? Direction { get; set; }

        [XmlAttribute("body_part")]
        public string? BodyPart { get; set; }

        [XmlAttribute("death_type")]
        public string? DeathType { get; set; }

        [XmlAttribute("action")]
        public string? Action { get; set; }
    }
} 