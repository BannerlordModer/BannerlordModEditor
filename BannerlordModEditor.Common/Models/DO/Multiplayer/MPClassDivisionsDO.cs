using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO.Multiplayer;

/// <summary>
/// 多人游戏分类系统的领域对象
/// 用于MPClassDivisions.xml文件的完整处理
/// 包含大型文件处理和性能优化功能
/// </summary>
[XmlRoot("MPClassDivisions")]
public class MPClassDivisionsDO
{
    [XmlElement("MPClassDivision")]
    public List<MPClassDivisionDO> MPClassDivisions { get; set; } = new List<MPClassDivisionDO>();

    // 性能优化：大型文件处理标记
    [XmlIgnore]
    public bool IsLargeFile { get; set; } = false;
    
    [XmlIgnore]
    public int ProcessedChunks { get; set; } = 0;

    // 业务逻辑：按文化分类
    [XmlIgnore]
    public Dictionary<string, List<MPClassDivisionDO>> ClassDivisionsByCulture { get; set; } = new Dictionary<string, List<MPClassDivisionDO>>();

    // 业务逻辑：按游戏模式分类
    [XmlIgnore]
    public Dictionary<string, List<MPClassDivisionDO>> ClassDivisionsByGameMode { get; set; } = new Dictionary<string, List<MPClassDivisionDO>>();

    // 业务逻辑方法
    public void InitializeIndexes()
    {
        ClassDivisionsByCulture.Clear();
        ClassDivisionsByGameMode.Clear();

        foreach (var division in MPClassDivisions)
        {
            // 按文化分类（从ID中提取）
            var culture = ExtractCultureFromId(division.Id);
            if (!ClassDivisionsByCulture.ContainsKey(culture))
            {
                ClassDivisionsByCulture[culture] = new List<MPClassDivisionDO>();
            }
            ClassDivisionsByCulture[culture].Add(division);

            // 按游戏模式分类
            foreach (var perk in division.Perks?.PerksList ?? new List<PerkDO>())
            {
                var gameMode = perk.GameMode;
                if (!string.IsNullOrEmpty(gameMode))
                {
                    if (!ClassDivisionsByGameMode.ContainsKey(gameMode))
                    {
                        ClassDivisionsByGameMode[gameMode] = new List<MPClassDivisionDO>();
                    }
                    if (!ClassDivisionsByGameMode[gameMode].Contains(division))
                    {
                        ClassDivisionsByGameMode[gameMode].Add(division);
                    }
                }
            }
        }
    }

    public List<MPClassDivisionDO> GetClassDivisionsByCulture(string culture)
    {
        return ClassDivisionsByCulture.GetValueOrDefault(culture, new List<MPClassDivisionDO>());
    }

    public List<MPClassDivisionDO> GetClassDivisionsByGameMode(string gameMode)
    {
        return ClassDivisionsByGameMode.GetValueOrDefault(gameMode, new List<MPClassDivisionDO>());
    }

    public string ExtractCultureFromId(string id)
    {
        if (string.IsNullOrEmpty(id)) return "unknown";
        
        var parts = id.Split('_');
        return parts.Length > 2 ? parts[2] : "unknown";
    }

    public bool ShouldSerializeMPClassDivisions() => MPClassDivisions != null && MPClassDivisions.Count > 0;
}

/// <summary>
/// 单个多人游戏分类的领域对象
/// 包含完整的多人游戏分类属性和业务逻辑
/// </summary>
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

    // 运行时标记
    [XmlIgnore]
    public bool HasPerks { get; set; } = false;

    [XmlIgnore]
    public bool IsSkirmishMode { get; set; } = false;

    [XmlIgnore]
    public bool IsCaptainMode { get; set; } = false;

    // 业务逻辑方法
    public string GetCulture()
    {
        var parts = Id.Split('_');
        return parts.Length > 2 ? parts[2] : "unknown";
    }

    public string GetClassType()
    {
        var parts = Id.Split('_');
        return parts.Length > 3 ? string.Join("_", parts.Skip(3)) : "unknown";
    }

    public bool HasValidCost()
    {
        return decimal.TryParse(Cost, out var cost) && cost > 0;
    }

    public bool HasValidMultiplier()
    {
        return decimal.TryParse(Multiplier, out var multiplier) && multiplier > 0;
    }

    public double GetMovementSpeedValue()
    {
        if (double.TryParse(MovementSpeed, out var speed))
            return speed;
        return 1.0;
    }

    public double GetCombatMovementSpeedValue()
    {
        if (double.TryParse(CombatMovementSpeed, out var speed))
            return speed;
        return 1.0;
    }

    public int GetArmorValue()
    {
        if (int.TryParse(Armor, out var armor))
            return armor;
        return 0;
    }

    public void UpdateGameModeFlags()
    {
        IsSkirmishMode = false;
        IsCaptainMode = false;

        if (Perks?.PerksList == null) return;

        foreach (var perk in Perks.PerksList)
        {
            if (perk.GameMode == "skirmish")
                IsSkirmishMode = true;
            else if (perk.GameMode == "captain")
                IsCaptainMode = true;
        }
    }

    // 游戏平衡性检查
    public bool IsBalanced()
    {
        if (!HasValidCost()) return false;
        
        var costValue = decimal.Parse(Cost);
        var armorValue = GetArmorValue();
        var moveSpeed = GetMovementSpeedValue();
        
        // 简单的平衡性检查：高成本应该有相应的属性
        if (costValue > 150 && armorValue < 10) return false;
        if (costValue > 100 && moveSpeed < 0.8) return false;
        
        return true;
    }

    // 序列化控制方法
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

/// <summary>
/// 天赋容器
/// </summary>
public class PerksDO
{
    [XmlElement("Perk")]
    public List<PerkDO> PerksList { get; set; } = new List<PerkDO>();

    // 业务逻辑：按游戏模式分组
    [XmlIgnore]
    public Dictionary<string, List<PerkDO>> PerksByGameMode { get; set; } = new Dictionary<string, List<PerkDO>>();

    public void GroupByGameMode()
    {
        PerksByGameMode.Clear();
        
        foreach (var perk in PerksList)
        {
            var gameMode = perk.GameMode;
            if (string.IsNullOrEmpty(gameMode)) continue;
            
            if (!PerksByGameMode.ContainsKey(gameMode))
            {
                PerksByGameMode[gameMode] = new List<PerkDO>();
            }
            PerksByGameMode[gameMode].Add(perk);
        }
    }

    public List<PerkDO> GetPerksByGameMode(string gameMode)
    {
        return PerksByGameMode.GetValueOrDefault(gameMode, new List<PerkDO>());
    }

    public bool ShouldSerializePerksList() => PerksList != null && PerksList.Count > 0;
}

/// <summary>
/// 单个天赋的领域对象
/// </summary>
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

    // 业务逻辑方法
    public bool IsSkirmishPerk() => GameMode == "skirmish";
    
    public bool IsCaptainPerk() => GameMode == "captain";
    
    public bool IsAllGameModePerk() => GameMode == "all";

    public bool HasArmorEffects()
    {
        return OnSpawnEffects.Any(e => e.Type == "ArmorOnSpawn");
    }

    public bool HasEquipmentEffects()
    {
        return OnSpawnEffects.Any(e => e.Type == "AlternativeEquipmentOnSpawn");
    }

    public List<OnSpawnEffectDO> GetEffectsByType(string type)
    {
        return OnSpawnEffects.Where(e => e.Type == type).ToList();
    }

    public List<RandomOnSpawnEffectDO> GetRandomEffectsByType(string type)
    {
        return RandomOnSpawnEffects.Where(e => e.Type == type).ToList();
    }

    // 序列化控制方法
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

/// <summary>
/// 生成时效果
/// </summary>
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

    // 效果类型常量
    public const string ArmorOnSpawnType = "ArmorOnSpawn";
    public const string AlternativeEquipmentOnSpawnType = "AlternativeEquipmentOnSpawn";

    // 业务逻辑方法
    public bool IsArmorEffect() => Type == ArmorOnSpawnType;
    
    public bool IsEquipmentEffect() => Type == AlternativeEquipmentOnSpawnType;

    public int GetArmorValue()
    {
        if (int.TryParse(Value, out var armor))
            return armor;
        return 0;
    }

    public bool TargetsPlayer() => Target == "Player";
    
    public bool TargetsTroops() => Target == "Troops";

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeItem() => !string.IsNullOrEmpty(Item);
    public bool ShouldSerializeTarget() => !string.IsNullOrEmpty(Target);
}

/// <summary>
/// 随机生成时效果
/// </summary>
public class RandomOnSpawnEffectDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("target")]
    public string Target { get; set; } = string.Empty;

    [XmlElement("Group")]
    public List<GroupDO> Groups { get; set; } = new List<GroupDO>();

    // 业务逻辑方法
    public bool TargetsPlayer() => Target == "Player";
    
    public bool TargetsTroops() => Target == "Troops";

    public List<ItemDO> GetAllItems()
    {
        return Groups.SelectMany(g => g.Items).ToList();
    }

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeTarget() => !string.IsNullOrEmpty(Target);
    public bool ShouldSerializeGroups() => Groups != null && Groups.Count > 0;
}

/// <summary>
/// 效果
/// </summary>
public class EffectDO
{
    [XmlAttribute("type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute("value")]
    public string Value { get; set; } = string.Empty;

    public bool ShouldSerializeType() => !string.IsNullOrEmpty(Type);
    public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
}

/// <summary>
/// 物品组
/// </summary>
public class GroupDO
{
    [XmlElement("Item")]
    public List<ItemDO> Items { get; set; } = new List<ItemDO>();

    public bool ShouldSerializeItems() => Items != null && Items.Count > 0;
}

/// <summary>
/// 单个物品
/// </summary>
public class ItemDO
{
    [XmlAttribute("slot")]
    public string Slot { get; set; } = string.Empty;

    [XmlAttribute("item")]
    public string Item { get; set; } = string.Empty;

    // 插槽类型常量
    public const string BodySlot = "Body";
    public const string CapeSlot = "Cape";
    public const string HeadSlot = "Head";
    public const string LegSlot = "Leg";
    public const string WeaponSlot = "Weapon";

    // 业务逻辑方法
    public bool IsBodyArmor() => Slot == BodySlot;
    
    public bool IsCape() => Slot == CapeSlot;
    
    public bool IsHeadArmor() => Slot == HeadSlot;
    
    public bool IsLegArmor() => Slot == LegSlot;
    
    public bool IsWeapon() => Slot == WeaponSlot;

    public bool ShouldSerializeSlot() => !string.IsNullOrEmpty(Slot);
    public bool ShouldSerializeItem() => !string.IsNullOrEmpty(Item);
}