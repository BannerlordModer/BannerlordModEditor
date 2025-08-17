using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Models.DTO.Game;

namespace BannerlordModEditor.Common.Mappers;

public static class MonsterUsageSetsMapper
{
    public static MonsterUsageSetsDTO ToDTO(MonsterUsageSetsDO source)
    {
        if (source == null) return null;

        return new MonsterUsageSetsDTO
        {
            Type = source.Type,
            MonsterUsageSets = source.MonsterUsageSets?
                .Select(MonsterUsageSetsMapper.ToDTO)
                .ToList() ?? new List<MonsterUsageSetDTO>()
        };
    }

    public static MonsterUsageSetsDO ToDO(MonsterUsageSetsDTO source)
    {
        if (source == null) return null;

        return new MonsterUsageSetsDO
        {
            Type = source.Type,
            MonsterUsageSets = source.MonsterUsageSets?
                .Select(MonsterUsageSetsMapper.ToDO)
                .ToList() ?? new List<MonsterUsageSetDO>()
        };
    }

    public static MonsterUsageSetDTO ToDTO(MonsterUsageSetDO source)
    {
        if (source == null) return null;

        return new MonsterUsageSetDTO
        {
            Id = source.Id,
            HitObjectAction = source.HitObjectAction,
            HitObjectFallingAction = source.HitObjectFallingAction,
            RearAction = source.RearAction,
            RearDamagedAction = source.RearDamagedAction,
            LadderClimbAction = source.LadderClimbAction,
            StrikeLadderAction = source.StrikeLadderAction,
            MonsterUsageStrikes = MonsterUsageStrikesMapper.ToDTO(source.MonsterUsageStrikes)
        };
    }

    public static MonsterUsageSetDO ToDO(MonsterUsageSetDTO source)
    {
        if (source == null) return null;

        return new MonsterUsageSetDO
        {
            Id = source.Id,
            HitObjectAction = source.HitObjectAction,
            HitObjectFallingAction = source.HitObjectFallingAction,
            RearAction = source.RearAction,
            RearDamagedAction = source.RearDamagedAction,
            LadderClimbAction = source.LadderClimbAction,
            StrikeLadderAction = source.StrikeLadderAction,
            MonsterUsageStrikes = MonsterUsageStrikesMapper.ToDO(source.MonsterUsageStrikes),
            HasMonsterUsageStrikes = source.MonsterUsageStrikes != null && source.MonsterUsageStrikes.Strikes.Count > 0
        };
    }
}

public static class MonsterUsageStrikesMapper
{
    public static MonsterUsageStrikesDTO ToDTO(MonsterUsageStrikesDO source)
    {
        if (source == null) return null;

        return new MonsterUsageStrikesDTO
        {
            Strikes = source.Strikes?
                .Select(MonsterUsageStrikesMapper.ToDTO)
                .ToList() ?? new List<MonsterUsageStrikeDTO>()
        };
    }

    public static MonsterUsageStrikesDO ToDO(MonsterUsageStrikesDTO source)
    {
        if (source == null) return null;

        return new MonsterUsageStrikesDO
        {
            Strikes = source.Strikes?
                .Select(MonsterUsageStrikesMapper.ToDO)
                .ToList() ?? new List<MonsterUsageStrikeDO>()
        };
    }

    public static MonsterUsageStrikeDTO ToDTO(MonsterUsageStrikeDO source)
    {
        if (source == null) return null;

        return new MonsterUsageStrikeDTO
        {
            IsHeavy = source.IsHeavy,
            IsLeftStance = source.IsLeftStance,
            Direction = source.Direction,
            BodyPart = source.BodyPart,
            Impact = source.Impact,
            Action = source.Action
        };
    }

    public static MonsterUsageStrikeDO ToDO(MonsterUsageStrikeDTO source)
    {
        if (source == null) return null;

        return new MonsterUsageStrikeDO
        {
            IsHeavy = source.IsHeavy,
            IsLeftStance = source.IsLeftStance,
            Direction = source.Direction,
            BodyPart = source.BodyPart,
            Impact = source.Impact,
            Action = source.Action
        };
    }
}