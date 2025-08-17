using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Models.DTO.Multiplayer;

namespace BannerlordModEditor.Common.Mappers;

public static class MPClassDivisionsMapper
{
    public static MPClassDivisionsDTO ToDTO(MPClassDivisionsDO source)
    {
        if (source == null) return null;

        return new MPClassDivisionsDTO
        {
            MPClassDivisions = source.MPClassDivisions?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<MPClassDivisionDTO>()
        };
    }

    public static MPClassDivisionsDO ToDO(MPClassDivisionsDTO source)
    {
        if (source == null) return null;

        return new MPClassDivisionsDO
        {
            MPClassDivisions = source.MPClassDivisions?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<MPClassDivisionDO>()
        };
    }

    public static MPClassDivisionDTO ToDTO(MPClassDivisionDO source)
    {
        if (source == null) return null;

        return new MPClassDivisionDTO
        {
            Id = source.Id,
            Hero = source.Hero,
            Troop = source.Troop,
            HeroIdleAnim = source.HeroIdleAnim,
            TroopIdleAnim = source.TroopIdleAnim,
            Multiplier = source.Multiplier,
            Cost = source.Cost,
            CasualCost = source.CasualCost,
            Icon = source.Icon,
            MeleeAI = source.MeleeAI,
            RangedAI = source.RangedAI,
            Armor = source.Armor,
            MovementSpeed = source.MovementSpeed,
            CombatMovementSpeed = source.CombatMovementSpeed,
            Acceleration = source.Acceleration,
            Perks = ToDTO(source.Perks)
        };
    }

    public static MPClassDivisionDO ToDO(MPClassDivisionDTO source)
    {
        if (source == null) return null;

        return new MPClassDivisionDO
        {
            Id = source.Id,
            Hero = source.Hero,
            Troop = source.Troop,
            HeroIdleAnim = source.HeroIdleAnim,
            TroopIdleAnim = source.TroopIdleAnim,
            Multiplier = source.Multiplier,
            Cost = source.Cost,
            CasualCost = source.CasualCost,
            Icon = source.Icon,
            MeleeAI = source.MeleeAI,
            RangedAI = source.RangedAI,
            Armor = source.Armor,
            MovementSpeed = source.MovementSpeed,
            CombatMovementSpeed = source.CombatMovementSpeed,
            Acceleration = source.Acceleration,
            Perks = ToDO(source.Perks),
            HasPerks = source.Perks != null && source.Perks.PerksList.Count > 0
        };
    }

    public static PerksDTO ToDTO(PerksDO source)
    {
        if (source == null) return null;

        return new PerksDTO
        {
            PerksList = source.PerksList?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<PerkDTO>()
        };
    }

    public static PerksDO ToDO(PerksDTO source)
    {
        if (source == null) return null;

        return new PerksDO
        {
            PerksList = source.PerksList?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<PerkDO>()
        };
    }

    public static PerkDTO ToDTO(PerkDO source)
    {
        if (source == null) return null;

        return new PerkDTO
        {
            GameMode = source.GameMode,
            Name = source.Name,
            Description = source.Description,
            Icon = source.Icon,
            HeroIdleAnim = source.HeroIdleAnim,
            PerkList = source.PerkList,
            OnSpawnEffects = source.OnSpawnEffects?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<OnSpawnEffectDTO>(),
            RandomOnSpawnEffects = source.RandomOnSpawnEffects?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<RandomOnSpawnEffectDTO>(),
            Effects = source.Effects?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<EffectDTO>()
        };
    }

    public static PerkDO ToDO(PerkDTO source)
    {
        if (source == null) return null;

        return new PerkDO
        {
            GameMode = source.GameMode,
            Name = source.Name,
            Description = source.Description,
            Icon = source.Icon,
            HeroIdleAnim = source.HeroIdleAnim,
            PerkList = source.PerkList,
            OnSpawnEffects = source.OnSpawnEffects?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<OnSpawnEffectDO>(),
            RandomOnSpawnEffects = source.RandomOnSpawnEffects?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<RandomOnSpawnEffectDO>(),
            Effects = source.Effects?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<EffectDO>()
        };
    }

    public static OnSpawnEffectDTO ToDTO(OnSpawnEffectDO source)
    {
        if (source == null) return null;

        return new OnSpawnEffectDTO
        {
            Type = source.Type,
            Value = source.Value,
            Slot = source.Slot,
            Item = source.Item,
            Target = source.Target
        };
    }

    public static OnSpawnEffectDO ToDO(OnSpawnEffectDTO source)
    {
        if (source == null) return null;

        return new OnSpawnEffectDO
        {
            Type = source.Type,
            Value = source.Value,
            Slot = source.Slot,
            Item = source.Item,
            Target = source.Target
        };
    }

    public static RandomOnSpawnEffectDTO ToDTO(RandomOnSpawnEffectDO source)
    {
        if (source == null) return null;

        return new RandomOnSpawnEffectDTO
        {
            Type = source.Type,
            Target = source.Target,
            Groups = source.Groups?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<GroupDTO>()
        };
    }

    public static RandomOnSpawnEffectDO ToDO(RandomOnSpawnEffectDTO source)
    {
        if (source == null) return null;

        return new RandomOnSpawnEffectDO
        {
            Type = source.Type,
            Target = source.Target,
            Groups = source.Groups?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<GroupDO>()
        };
    }

    public static EffectDTO ToDTO(EffectDO source)
    {
        if (source == null) return null;

        return new EffectDTO
        {
            Type = source.Type,
            Value = source.Value
        };
    }

    public static EffectDO ToDO(EffectDTO source)
    {
        if (source == null) return null;

        return new EffectDO
        {
            Type = source.Type,
            Value = source.Value
        };
    }

    public static GroupDTO ToDTO(GroupDO source)
    {
        if (source == null) return null;

        return new GroupDTO
        {
            Items = source.Items?
                .Select(MPClassDivisionsMapper.ToDTO)
                .ToList() ?? new List<ItemDTO>()
        };
    }

    public static GroupDO ToDO(GroupDTO source)
    {
        if (source == null) return null;

        return new GroupDO
        {
            Items = source.Items?
                .Select(MPClassDivisionsMapper.ToDO)
                .ToList() ?? new List<ItemDO>()
        };
    }

    public static ItemDTO ToDTO(ItemDO source)
    {
        if (source == null) return null;

        return new ItemDTO
        {
            Slot = source.Slot,
            Item = source.Item
        };
    }

    public static ItemDO ToDO(ItemDTO source)
    {
        if (source == null) return null;

        return new ItemDO
        {
            Slot = source.Slot,
            Item = source.Item
        };
    }
}