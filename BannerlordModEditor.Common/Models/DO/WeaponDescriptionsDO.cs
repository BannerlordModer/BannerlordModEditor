using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Models.DO
{
    /// <summary>
    /// 武器描述配置的领域对象
    /// 用于weapon_descriptions.xml文件的完整处理
    /// 包含武器制作描述、标志和可用部件
    /// </summary>
    [XmlRoot("WeaponDescriptions")]
    public class WeaponDescriptionsDO
    {
        [XmlElement("WeaponDescription")]
        public List<WeaponDescriptionDO> Descriptions { get; set; } = new List<WeaponDescriptionDO>();

        // 运行时标记
        [XmlIgnore]
        public bool HasEmptyDescriptions { get; set; } = false;

        // 业务逻辑：按武器类型索引
        [XmlIgnore]
        public Dictionary<string, List<WeaponDescriptionDO>> DescriptionsByWeaponClass { get; set; } = new Dictionary<string, List<WeaponDescriptionDO>>();

        // 业务逻辑：按使用特性索引
        [XmlIgnore]
        public Dictionary<string, List<WeaponDescriptionDO>> DescriptionsByUsageFeature { get; set; } = new Dictionary<string, List<WeaponDescriptionDO>>();

        // 业务逻辑：标志索引
        [XmlIgnore]
        public Dictionary<string, List<WeaponDescriptionDO>> DescriptionsByFlag { get; set; } = new Dictionary<string, List<WeaponDescriptionDO>>();

        // 业务逻辑：可用部件索引
        [XmlIgnore]
        public Dictionary<string, List<WeaponDescriptionDO>> DescriptionsByPiece { get; set; } = new Dictionary<string, List<WeaponDescriptionDO>>();

        // 业务逻辑方法
        public void InitializeIndexes()
        {
            DescriptionsByWeaponClass.Clear();
            DescriptionsByUsageFeature.Clear();
            DescriptionsByFlag.Clear();
            DescriptionsByPiece.Clear();

            foreach (var desc in Descriptions)
            {
                // 按武器类型索引
                if (!string.IsNullOrEmpty(desc.WeaponClass))
                {
                    if (!DescriptionsByWeaponClass.ContainsKey(desc.WeaponClass))
                    {
                        DescriptionsByWeaponClass[desc.WeaponClass] = new List<WeaponDescriptionDO>();
                    }
                    DescriptionsByWeaponClass[desc.WeaponClass].Add(desc);
                }

                // 按使用特性索引
                if (!string.IsNullOrEmpty(desc.ItemUsageFeatures))
                {
                    var features = desc.ItemUsageFeatures.Split(':');
                    foreach (var feature in features)
                    {
                        if (!string.IsNullOrEmpty(feature))
                        {
                            if (!DescriptionsByUsageFeature.ContainsKey(feature))
                            {
                                DescriptionsByUsageFeature[feature] = new List<WeaponDescriptionDO>();
                            }
                            DescriptionsByUsageFeature[feature].Add(desc);
                        }
                    }
                }

                // 按标志索引
                if (desc.WeaponFlags?.Flags != null)
                {
                    foreach (var flag in desc.WeaponFlags.Flags)
                    {
                        if (!string.IsNullOrEmpty(flag.Value))
                        {
                            if (!DescriptionsByFlag.ContainsKey(flag.Value))
                            {
                                DescriptionsByFlag[flag.Value] = new List<WeaponDescriptionDO>();
                            }
                            DescriptionsByFlag[flag.Value].Add(desc);
                        }
                    }
                }

                // 按可用部件索引
                if (desc.AvailablePieces?.Pieces != null)
                {
                    foreach (var piece in desc.AvailablePieces.Pieces)
                    {
                        if (!string.IsNullOrEmpty(piece.Id))
                        {
                            if (!DescriptionsByPiece.ContainsKey(piece.Id))
                            {
                                DescriptionsByPiece[piece.Id] = new List<WeaponDescriptionDO>();
                            }
                            DescriptionsByPiece[piece.Id].Add(desc);
                        }
                    }
                }
            }
        }

        public WeaponDescriptionDO? GetDescriptionById(string id)
        {
            return Descriptions.FirstOrDefault(d => d.Id == id);
        }

        public List<WeaponDescriptionDO> GetDescriptionsByWeaponClass(string weaponClass)
        {
            return DescriptionsByWeaponClass.GetValueOrDefault(weaponClass, new List<WeaponDescriptionDO>());
        }

        public List<WeaponDescriptionDO> GetDescriptionsByUsageFeature(string feature)
        {
            return DescriptionsByUsageFeature.GetValueOrDefault(feature, new List<WeaponDescriptionDO>());
        }

        public List<WeaponDescriptionDO> GetDescriptionsByFlag(string flag)
        {
            return DescriptionsByFlag.GetValueOrDefault(flag, new List<WeaponDescriptionDO>());
        }

        public List<WeaponDescriptionDO> GetDescriptionsByPiece(string pieceId)
        {
            return DescriptionsByPiece.GetValueOrDefault(pieceId, new List<WeaponDescriptionDO>());
        }

        public List<WeaponDescriptionDO> GetOneHandedWeapons()
        {
            return Descriptions.Where(d => d.IsOneHanded()).ToList();
        }

        public List<WeaponDescriptionDO> GetTwoHandedWeapons()
        {
            return Descriptions.Where(d => d.IsTwoHanded()).ToList();
        }

        public List<WeaponDescriptionDO> GetRangedWeapons()
        {
            return Descriptions.Where(d => d.IsRanged()).ToList();
        }

        public List<WeaponDescriptionDO> GetCraftableWeapons()
        {
            return Descriptions.Where(d => d.AvailablePieces?.Pieces?.Any() ?? false).ToList();
        }

        // 验证方法
        public bool IsValid()
        {
            if (Descriptions.Count == 0) return false; // 空文件无效
            return Descriptions.All(d => d.IsValid());
        }

        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (Descriptions.Count == 0)
                errors.Add("No weapon descriptions found");

            foreach (var desc in Descriptions)
            {
                var descErrors = desc.GetValidationErrors();
                errors.AddRange(descErrors.Select(e => $"{desc.Id}: {e}"));
            }

            return errors;
        }

        // 统计方法
        public int GetTotalWeaponCount()
        {
            return Descriptions.Count;
        }

        public int GetWeaponClassCount()
        {
            return Descriptions.Select(d => d.WeaponClass).Distinct(StringComparer.OrdinalIgnoreCase).Count();
        }

        public Dictionary<string, int> GetWeaponClassDistribution()
        {
            return Descriptions
                .GroupBy(d => d.WeaponClass ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public bool ShouldSerializeDescriptions() => Descriptions != null && Descriptions.Count > 0;
    }

    /// <summary>
    /// 单个武器描述
    /// </summary>
    public class WeaponDescriptionDO
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("weapon_class")]
        public string? WeaponClass { get; set; }

        [XmlAttribute("item_usage_features")]
        public string? ItemUsageFeatures { get; set; }

        [XmlElement("WeaponFlags")]
        public WeaponFlagsDO? WeaponFlags { get; set; }

        [XmlElement("AvailablePieces")]
        public AvailablePiecesDO? AvailablePieces { get; set; }

        // 运行时标记
        [XmlIgnore]
        public bool HasWeaponFlags { get; set; } = false;

        [XmlIgnore]
        public bool HasAvailablePieces { get; set; } = false;

        // 业务逻辑方法
        public bool IsOneHanded()
        {
            return WeaponClass?.Contains("OneHanded", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool IsTwoHanded()
        {
            return WeaponClass?.Contains("TwoHanded", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool IsRanged()
        {
            return (WeaponClass?.Contains("Bow", StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (WeaponClass?.Contains("Crossbow", StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (WeaponClass?.Contains("Thrown", StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (WeaponClass?.Contains("Javelin", StringComparison.OrdinalIgnoreCase) ?? false);
        }

        public bool IsMelee()
        {
            return !IsRanged();
        }

        public bool CanBlock()
        {
            return ItemUsageFeatures?.Contains("block", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool CanThrust()
        {
            return ItemUsageFeatures?.Contains("thrust", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool CanSwing()
        {
            return ItemUsageFeatures?.Contains("swing", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public bool HasShield()
        {
            return ItemUsageFeatures?.Contains("shield", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public List<string> GetUsageFeatures()
        {
            if (string.IsNullOrEmpty(ItemUsageFeatures))
                return new List<string>();

            return ItemUsageFeatures.Split(':', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim())
                                  .ToList();
        }

        public bool HasFlag(string flagValue)
        {
            return WeaponFlags?.Flags?.Any(f => f.Value == flagValue) ?? false;
        }

        public bool HasPiece(string pieceId)
        {
            return AvailablePieces?.Pieces?.Any(p => p.Id == pieceId) ?? false;
        }

        public List<string> GetAvailablePieceIds()
        {
            return AvailablePieces?.Pieces?
                       .Select(p => p.Id)
                       .Where(id => !string.IsNullOrEmpty(id))
                       .ToList() ?? new List<string>();
        }

        public bool IsCraftable()
        {
            return AvailablePieces?.Pieces?.Any() ?? false;
        }

        public int GetPieceCount()
        {
            return AvailablePieces?.Pieces?.Count ?? 0;
        }

        // 验证方法
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Id)) return false;
            
            // 检查ID格式（通常包含武器类型，但不强制要求点号）
            // 允许各种ID格式，包括简单的类名
            if (Id.Length < 2) return false;
            
            return true;
        }

        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Id))
                errors.Add("Weapon ID is required");

            if (Id.Length < 2)
                errors.Add("Weapon ID should be at least 2 characters long");

            if (string.IsNullOrEmpty(WeaponClass))
                errors.Add("Weapon class is recommended");

            // Weapon flags are recommended but not required
            if (WeaponFlags?.Flags == null || WeaponFlags.Flags.Count == 0)
                errors.Add("Weapon flags are recommended for better functionality");

            return errors;
        }

        // 武器类别推断
        public string GetWeaponType()
        {
            if (WeaponClass?.Contains("Sword", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Sword";
            if (WeaponClass?.Contains("Mace", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Mace";
            if (WeaponClass?.Contains("Axe", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Axe";
            if (WeaponClass?.Contains("Bow", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Bow";
            if (WeaponClass?.Contains("Crossbow", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Crossbow";
            if (WeaponClass?.Contains("Javelin", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Javelin";
            if (WeaponClass?.Contains("Dagger", StringComparison.OrdinalIgnoreCase) ?? false)
                return "Dagger";
            
            return "Unknown";
        }

        // 序列化控制方法
        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
        public bool ShouldSerializeWeaponClass() => !string.IsNullOrEmpty(WeaponClass);
        public bool ShouldSerializeItemUsageFeatures() => !string.IsNullOrEmpty(ItemUsageFeatures);
        public bool ShouldSerializeWeaponFlags() => HasWeaponFlags && WeaponFlags != null && WeaponFlags.Flags.Count > 0;
        public bool ShouldSerializeAvailablePieces() => HasAvailablePieces && AvailablePieces != null && AvailablePieces.Pieces.Count > 0;
    }

    /// <summary>
    /// 武器标志集合
    /// </summary>
    public class WeaponFlagsDO
    {
        [XmlElement("WeaponFlag")]
        public List<WeaponFlagDO> Flags { get; set; } = new List<WeaponFlagDO>();

        public bool ShouldSerializeFlags() => Flags != null && Flags.Count > 0;
    }

    /// <summary>
    /// 单个武器标志
    /// </summary>
    public class WeaponFlagDO
    {
        [XmlAttribute("value")]
        public string? Value { get; set; }

        // 常见标志值
        public const string MeleeWeapon = "MeleeWeapon";
        public const string NotUsableWithOneHand = "NotUsableWithOneHand";
        public const string TwoHandIdleOnMount = "TwoHandIdleOnMount";
        public const string HasAlternativeSwing = "HasAlternativeSwing";
        public const string CantUseOnHorseback = "CantUseOnHorseback";
        public const string Civilian = "Civilian";

        // 业务逻辑方法
        public bool IsMeleeWeapon() => Value == MeleeWeapon;
        public bool IsNotUsableWithOneHand() => Value == NotUsableWithOneHand;
        public bool RequiresTwoHandsOnMount() => Value == TwoHandIdleOnMount;
        public bool HasAlternativeSwingFlag() => Value == HasAlternativeSwing;
        public bool CantUseOnHorsebackFlag() => Value == CantUseOnHorseback;
        public bool IsCivilianFlag() => Value == Civilian;

        public bool ShouldSerializeValue() => !string.IsNullOrEmpty(Value);
    }

    /// <summary>
    /// 可用武器部件集合
    /// </summary>
    public class AvailablePiecesDO
    {
        [XmlElement("AvailablePiece")]
        public List<AvailablePieceDO> Pieces { get; set; } = new List<AvailablePieceDO>();

        // 业务逻辑方法
        public bool HasBlade() => Pieces.Any(p => p.IsBlade());
        public bool HasGuard() => Pieces.Any(p => p.IsGuard());
        public bool HasGrip() => Pieces.Any(p => p.IsGrip());
        public bool HasPommel() => Pieces.Any(p => p.IsPommel());
        public bool HasHandle() => Pieces.Any(p => p.IsHandle());
        public bool HasHead() => Pieces.Any(p => p.IsHead());

        public bool ShouldSerializePieces() => Pieces != null && Pieces.Count > 0;
    }

    /// <summary>
    /// 单个可用武器部件
    /// </summary>
    public class AvailablePieceDO
    {
        [XmlAttribute("id")]
        public string? Id { get; set; }

        // 部件类型常量
        public const string Blade = "blade";
        public const string Guard = "guard";
        public const string Grip = "grip";
        public const string Pommel = "pommel";
        public const string Handle = "handle";
        public const string Head = "head";

        // 业务逻辑方法
        public bool IsBlade() => Id == Blade;
        public bool IsGuard() => Id == Guard;
        public bool IsGrip() => Id == Grip;
        public bool IsPommel() => Id == Pommel;
        public bool IsHandle() => Id == Handle;
        public bool IsHead() => Id == Head;

        public string GetPieceType()
        {
            if (IsBlade()) return "Blade";
            if (IsGuard()) return "Guard";
            if (IsGrip()) return "Grip";
            if (IsPommel()) return "Pommel";
            if (IsHandle()) return "Handle";
            if (IsHead()) return "Head";
            
            return "Unknown";
        }

        public bool ShouldSerializeId() => !string.IsNullOrEmpty(Id);
    }
}