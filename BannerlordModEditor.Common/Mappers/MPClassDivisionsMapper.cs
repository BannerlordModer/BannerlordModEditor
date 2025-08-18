using System.Linq;
using BannerlordModEditor.Common.Models.DO.Multiplayer;
using BannerlordModEditor.Common.Models.DTO.Multiplayer;

namespace BannerlordModEditor.Common.Mappers;

/// <summary>
/// MPClassDivisions的DO/DTO映射器
/// 处理多人游戏分类系统的领域对象和数据传输对象之间的双向转换
/// 支持大型文件处理和性能优化
/// </summary>
public static class MPClassDivisionsMapper
{
    /// <summary>
    /// 将DO转换为DTO
    /// </summary>
    public static MPClassDivisionsDTO ToDTO(MPClassDivisionsDO source)
    {
        if (source == null) return null;

        return new MPClassDivisionsDTO
        {
            MPClassDivisions = source.MPClassDivisions?
                .Select(ToDTO)
                .ToList() ?? new List<MPClassDivisionDTO>()
        };
    }

    /// <summary>
    /// 将DTO转换为DO
    /// </summary>
    public static MPClassDivisionsDO ToDO(MPClassDivisionsDTO source)
    {
        if (source == null) return null;

        var result = new MPClassDivisionsDO
        {
            MPClassDivisions = source.MPClassDivisions?
                .Select(ToDo)
                .ToList() ?? new List<MPClassDivisionDO>(),
            IsLargeFile = false, // 需要在反序列化后设置
            ProcessedChunks = 0
        };

        // 初始化索引和业务逻辑
        result.InitializeIndexes();
        
        // 更新每个分类的游戏模式标志
        foreach (var division in result.MPClassDivisions)
        {
            division.UpdateGameModeFlags();
            division.Perks?.GroupByGameMode();
        }

        return result;
    }

    /// <summary>
    /// 将单个MPClassDivisionDO转换为DTO
    /// </summary>
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

    /// <summary>
    /// 将单个MPClassDivisionDTO转换为DO
    /// </summary>
    public static MPClassDivisionDO ToDo(MPClassDivisionDTO source)
    {
        if (source == null) return null;

        var result = new MPClassDivisionDO
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
            Perks = ToDo(source.Perks),
            HasPerks = source.Perks != null && source.Perks.PerksList.Count > 0
        };

        // 更新业务逻辑标志
        result.UpdateGameModeFlags();

        return result;
    }

    /// <summary>
    /// 将PerksDO转换为DTO
    /// </summary>
    public static PerksDTO ToDTO(PerksDO source)
    {
        if (source == null) return null;

        return new PerksDTO
        {
            PerksList = source.PerksList?
                .Select(ToDTO)
                .ToList() ?? new List<PerkDTO>()
        };
    }

    /// <summary>
    /// 将PerksDTO转换为DO
    /// </summary>
    public static PerksDO ToDo(PerksDTO source)
    {
        if (source == null) return null;

        var result = new PerksDO
        {
            PerksList = source.PerksList?
                .Select(ToDo)
                .ToList() ?? new List<PerkDO>()
        };

        // 初始化游戏模式分组
        result.GroupByGameMode();

        return result;
    }

    /// <summary>
    /// 将单个PerkDO转换为DTO
    /// </summary>
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
                .Select(ToDTO)
                .ToList() ?? new List<OnSpawnEffectDTO>(),
            RandomOnSpawnEffects = source.RandomOnSpawnEffects?
                .Select(ToDTO)
                .ToList() ?? new List<RandomOnSpawnEffectDTO>(),
            Effects = source.Effects?
                .Select(ToDTO)
                .ToList() ?? new List<EffectDTO>()
        };
    }

    /// <summary>
    /// 将单个PerkDTO转换为DO
    /// </summary>
    public static PerkDO ToDo(PerkDTO source)
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
                .Select(ToDo)
                .ToList() ?? new List<OnSpawnEffectDO>(),
            RandomOnSpawnEffects = source.RandomOnSpawnEffects?
                .Select(ToDo)
                .ToList() ?? new List<RandomOnSpawnEffectDO>(),
            Effects = source.Effects?
                .Select(ToDo)
                .ToList() ?? new List<EffectDO>()
        };
    }

    /// <summary>
    /// 将OnSpawnEffectDO转换为DTO
    /// </summary>
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

    /// <summary>
    /// 将OnSpawnEffectDTO转换为DO
    /// </summary>
    public static OnSpawnEffectDO ToDo(OnSpawnEffectDTO source)
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

    /// <summary>
    /// 将RandomOnSpawnEffectDO转换为DTO
    /// </summary>
    public static RandomOnSpawnEffectDTO ToDTO(RandomOnSpawnEffectDO source)
    {
        if (source == null) return null;

        return new RandomOnSpawnEffectDTO
        {
            Type = source.Type,
            Target = source.Target,
            Groups = source.Groups?
                .Select(ToDTO)
                .ToList() ?? new List<GroupDTO>()
        };
    }

    /// <summary>
    /// 将RandomOnSpawnEffectDTO转换为DO
    /// </summary>
    public static RandomOnSpawnEffectDO ToDo(RandomOnSpawnEffectDTO source)
    {
        if (source == null) return null;

        return new RandomOnSpawnEffectDO
        {
            Type = source.Type,
            Target = source.Target,
            Groups = source.Groups?
                .Select(ToDo)
                .ToList() ?? new List<GroupDO>()
        };
    }

    /// <summary>
    /// 将EffectDO转换为DTO
    /// </summary>
    public static EffectDTO ToDTO(EffectDO source)
    {
        if (source == null) return null;

        return new EffectDTO
        {
            Type = source.Type,
            Value = source.Value
        };
    }

    /// <summary>
    /// 将EffectDTO转换为DO
    /// </summary>
    public static EffectDO ToDo(EffectDTO source)
    {
        if (source == null) return null;

        return new EffectDO
        {
            Type = source.Type,
            Value = source.Value
        };
    }

    /// <summary>
    /// 将GroupDO转换为DTO
    /// </summary>
    public static GroupDTO ToDTO(GroupDO source)
    {
        if (source == null) return null;

        return new GroupDTO
        {
            Items = source.Items?
                .Select(ToDTO)
                .ToList() ?? new List<ItemDTO>()
        };
    }

    /// <summary>
    /// 将GroupDTO转换为DO
    /// </summary>
    public static GroupDO ToDo(GroupDTO source)
    {
        if (source == null) return null;

        return new GroupDO
        {
            Items = source.Items?
                .Select(ToDo)
                .ToList() ?? new List<ItemDO>()
        };
    }

    /// <summary>
    /// 将ItemDO转换为DTO
    /// </summary>
    public static ItemDTO ToDTO(ItemDO source)
    {
        if (source == null) return null;

        return new ItemDTO
        {
            Slot = source.Slot,
            Item = source.Item
        };
    }

    /// <summary>
    /// 将ItemDTO转换为DO
    /// </summary>
    public static ItemDO ToDo(ItemDTO source)
    {
        if (source == null) return null;

        return new ItemDO
        {
            Slot = source.Slot,
            Item = source.Item
        };
    }

    /// <summary>
    /// 批量处理大型文件的特殊映射方法
    /// </summary>
    public static void ProcessLargeFileChunk(MPClassDivisionsDO target, List<MPClassDivisionDTO> chunk, int chunkIndex)
    {
        if (target == null || chunk == null) return;

        var divisions = chunk.Select(ToDo).ToList();
        target.MPClassDivisions.AddRange(divisions);
        target.ProcessedChunks = chunkIndex + 1;

        // 增量更新索引
        foreach (var division in divisions)
        {
            var culture = target.ExtractCultureFromId(division.Id);
            if (!target.ClassDivisionsByCulture.ContainsKey(culture))
            {
                target.ClassDivisionsByCulture[culture] = new List<MPClassDivisionDO>();
            }
            target.ClassDivisionsByCulture[culture].Add(division);

            foreach (var perk in division.Perks?.PerksList ?? new List<PerkDO>())
            {
                var gameMode = perk.GameMode;
                if (!string.IsNullOrEmpty(gameMode))
                {
                    if (!target.ClassDivisionsByGameMode.ContainsKey(gameMode))
                    {
                        target.ClassDivisionsByGameMode[gameMode] = new List<MPClassDivisionDO>();
                    }
                    if (!target.ClassDivisionsByGameMode[gameMode].Contains(division))
                    {
                        target.ClassDivisionsByGameMode[gameMode].Add(division);
                    }
                }
            }
        }
    }
}