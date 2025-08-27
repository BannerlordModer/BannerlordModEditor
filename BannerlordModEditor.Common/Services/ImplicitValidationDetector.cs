using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 隐式校验逻辑检测器
    /// 基于Mount & Blade源码分析，实现XML数据的隐式校验规则检测
    /// </summary>
    public class ImplicitValidationDetector
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly XmlDependencyAnalyzer _dependencyAnalyzer;
        
        // 基于mountblade-code分析的隐式校验规则
        private readonly Dictionary<string, List<ImplicitValidationRule>> _validationRules = new();

        public ImplicitValidationDetector(
            IFileDiscoveryService fileDiscoveryService,
            XmlDependencyAnalyzer dependencyAnalyzer)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _dependencyAnalyzer = dependencyAnalyzer;
            
            InitializeValidationRules();
        }

        /// <summary>
        /// 初始化验证规则
        /// </summary>
        private void InitializeValidationRules()
        {
            // 基础对象验证规则
            InitializeBasicObjectRules();
            
            // 角色系统验证规则
            InitializeCharacterRules();
            
            // 物品系统验证规则
            InitializeItemRules();
            
            // 战斗系统验证规则
            InitializeCombatRules();
            
            // 制作系统验证规则
            InitializeCraftingRules();
            
            // 引用完整性验证规则
            InitializeReferenceRules();
            
            // 数值范围验证规则
            InitializeValueRangeRules();
        }

        /// <summary>
        /// 初始化基础对象验证规则
        /// </summary>
        private void InitializeBasicObjectRules()
        {
            // 通用ID验证
            _validationRules["*"] = new List<ImplicitValidationRule>
            {
                new ImplicitValidationRule
                {
                    Name = "ID_Unique_Required",
                    Description = "所有对象必须具有唯一的ID",
                    Severity = ValidationSeverity.Error,
                    Validator = (doc, context) =>
                    {
                        var ids = doc.XPathSelectElements("//*[@id]")
                            .Select(e => e.Attribute("id")?.Value)
                            .Where(id => !string.IsNullOrEmpty(id))
                            .ToList();
                        
                        var duplicates = ids.GroupBy(id => id)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key)
                            .ToList();
                        
                        return duplicates.Select(duplicate => new ValidationResult
                        {
                            RuleName = "ID_Unique_Required",
                            Message = $"重复的ID: {duplicate}",
                            Severity = ValidationSeverity.Error,
                            Suggestion = "修改重复的ID以确保唯一性"
                        }).ToList();
                    }
                },
                new ImplicitValidationRule
                {
                    Name = "ID_Format_Valid",
                    Description = "ID必须符合命名规范",
                    Severity = ValidationSeverity.Warning,
                    Validator = (doc, context) =>
                    {
                        var invalidIds = doc.XPathSelectElements("//*[@id]")
                            .Select(e => e.Attribute("id")?.Value)
                            .Where(id => !string.IsNullOrEmpty(id) && !IsValidIdFormat(id))
                            .ToList();
                        
                        return invalidIds.Select(invalidId => new ValidationResult
                        {
                            RuleName = "ID_Format_Valid",
                            Message = $"ID格式无效: {invalidId}",
                            Severity = ValidationSeverity.Warning,
                            Suggestion = "ID应该使用小写字母、数字和下划线，避免特殊字符"
                        }).ToList();
                    }
                }
            };
        }

        /// <summary>
        /// 初始化角色系统验证规则
        /// </summary>
        private void InitializeCharacterRules()
        {
            _validationRules["characters"] = new List<ImplicitValidationRule>
            {
                new ImplicitValidationRule
                {
                    Name = "Character_Level_Valid",
                    Description = "角色等级必须在1-62范围内",
                    Severity = ValidationSeverity.Error,
                    Validator = (doc, context) =>
                    {
                        var invalidLevels = doc.XPathSelectElements("//Character[@level]")
                            .Where(e => !int.TryParse(e.Attribute("level")?.Value, out int level) || level < 1 || level > 62)
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        return invalidLevels.Select(characterId => new ValidationResult
                        {
                            RuleName = "Character_Level_Valid",
                            Message = $"角色 {characterId} 的等级无效",
                            Severity = ValidationSeverity.Error,
                            Suggestion = "角色等级必须在1-62范围内"
                        }).ToList();
                    }
                },
                new ImplicitValidationRule
                {
                    Name = "Character_Occupation_Valid",
                    Description = "角色职业必须是有效的枚举值",
                    Severity = ValidationSeverity.Error,
                    Validator = (doc, context) =>
                    {
                        var validOccupations = new[] { "NotAssigned", "Soldier", "Merchant", "Gangster", "Bandit", "CaravanGuard", "MerchantCartGuard", "VeteranMercenary", "Mercenary", "CaravanMaster", "CaravanGuard", "CaravanMaster", "GangLeader", "BanditChief", "MerchantGuildMaster", "Artisan", "Elder", "Headman", "Warrior", "Slave", "Lord", "Lady" };
                        
                        var invalidOccupations = doc.XPathSelectElements("//Character[@occupation]")
                            .Where(e => !validOccupations.Contains(e.Attribute("occupation")?.Value))
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        return invalidOccupations.Select(characterId => new ValidationResult
                        {
                            RuleName = "Character_Occupation_Valid",
                            Message = $"角色 {characterId} 的职业无效",
                            Severity = ValidationSeverity.Error,
                            Suggestion = $"职业必须是以下值之一: {string.Join(", ", validOccupations)}"
                        }).ToList();
                    }
                }
            };
        }

        /// <summary>
        /// 初始化物品系统验证规则
        /// </summary>
        private void InitializeItemRules()
        {
            _validationRules["items"] = new List<ImplicitValidationRule>
            {
                new ImplicitValidationRule
                {
                    Name = "Item_Weight_Valid",
                    Description = "物品重量必须在0-1000范围内",
                    Severity = ValidationSeverity.Warning,
                    Validator = (doc, context) =>
                    {
                        var invalidWeights = doc.XPathSelectElements("//Item[@weight]")
                            .Where(e => !float.TryParse(e.Attribute("weight")?.Value, out float weight) || weight < 0 || weight > 1000)
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        return invalidWeights.Select(itemId => new ValidationResult
                        {
                            RuleName = "Item_Weight_Valid",
                            Message = $"物品 {itemId} 的重量无效",
                            Severity = ValidationSeverity.Warning,
                            Suggestion = "物品重量应该在0-1000范围内"
                        }).ToList();
                    }
                },
                new ImplicitValidationRule
                {
                    Name = "Item_Value_Valid",
                    Description = "物品价值必须是非负数",
                    Severity = ValidationSeverity.Error,
                    Validator = (doc, context) =>
                    {
                        var invalidValues = doc.XPathSelectElements("//Item[@value]")
                            .Where(e => !int.TryParse(e.Attribute("value")?.Value, out int value) || value < 0)
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        return invalidValues.Select(itemId => new ValidationResult
                        {
                            RuleName = "Item_Value_Valid",
                            Message = $"物品 {itemId} 的价值无效",
                            Severity = ValidationSeverity.Error,
                            Suggestion = "物品价值必须是非负数"
                        }).ToList();
                    }
                },
                new ImplicitValidationRule
                {
                    Name = "Item_Type_Valid",
                    Description = "物品类型必须是有效的枚举值",
                    Severity = ValidationSeverity.Error,
                    Validator = (doc, context) =>
                    {
                        var validItemTypes = new[] { "Goods", "Animal", "Horse", "OneHandedWeapon", "TwoHandedWeapon", "Polearm", "Arrow", "Bolt", "Shield", "Bow", "Crossbow", "ThrowingKnife", "ThrowingAxe", "Stone", "Bullets", "Armor", "HeadArmor", "BodyArmor", "LegArmor", "HandArmor", "Pistol", "Musket", "Banner", "Book", "HorseHarness", "CartridgeBag", "ChestArmor", "Gloves", "Boots", "Mask" };
                        
                        var invalidTypes = doc.XPathSelectElements("//Item[@type]")
                            .Where(e => !validItemTypes.Contains(e.Attribute("type")?.Value))
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        return invalidTypes.Select(itemId => new ValidationResult
                        {
                            RuleName = "Item_Type_Valid",
                            Message = $"物品 {itemId} 的类型无效",
                            Severity = ValidationSeverity.Error,
                            Suggestion = $"物品类型必须是以下值之一: {string.Join(", ", validItemTypes)}"
                        }).ToList();
                    }
                }
            };
        }

        /// <summary>
        /// 初始化战斗系统验证规则
        /// </summary>
        private void InitializeCombatRules()
        {
            _validationRules["combat_parameters"] = new List<ImplicitValidationRule>
            {
                new ImplicitValidationRule
                {
                    Name = "Combat_Parameter_Valid",
                    Description = "战斗参数必须在有效范围内",
                    Severity = ValidationSeverity.Warning,
                    Validator = (doc, context) =>
                    {
                        var results = new List<ValidationResult>();
                        
                        // 检查伤害倍数
                        var damageMultipliers = doc.XPathSelectElements("//combat_parameter[@damage_multiplier]")
                            .Where(e => !float.TryParse(e.Attribute("damage_multiplier")?.Value, out float multiplier) || multiplier < 0 || multiplier > 10)
                            .Select(e => e.Attribute("name")?.Value ?? "Unknown")
                            .ToList();
                        
                        foreach (var param in damageMultipliers)
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Combat_Parameter_Valid",
                                Message = $"战斗参数 {param} 的伤害倍数无效",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "伤害倍数应该在0-10范围内"
                            });
                        }
                        
                        // 检查速度倍数
                        var speedMultipliers = doc.XPathSelectElements("//combat_parameter[@speed_multiplier]")
                            .Where(e => !float.TryParse(e.Attribute("speed_multiplier")?.Value, out float multiplier) || multiplier < 0 || multiplier > 10)
                            .Select(e => e.Attribute("name")?.Value ?? "Unknown")
                            .ToList();
                        
                        foreach (var param in speedMultipliers)
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Combat_Parameter_Valid",
                                Message = $"战斗参数 {param} 的速度倍数无效",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "速度倍数应该在0-10范围内"
                            });
                        }
                        
                        return results;
                    }
                }
            };
        }

        /// <summary>
        /// 初始化制作系统验证规则
        /// </summary>
        private void InitializeCraftingRules()
        {
            _validationRules["crafting_pieces"] = new List<ImplicitValidationRule>
            {
                new ImplicitValidationRule
                {
                    Name = "Crafting_Piece_Difficulty_Valid",
                    Description = "制作部件难度必须在0-300范围内",
                    Severity = ValidationSeverity.Warning,
                    Validator = (doc, context) =>
                    {
                        var invalidDifficulties = doc.XPathSelectElements("//CraftingPiece[@difficulty]")
                            .Where(e => !int.TryParse(e.Attribute("difficulty")?.Value, out int difficulty) || difficulty < 0 || difficulty > 300)
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        return invalidDifficulties.Select(pieceId => new ValidationResult
                        {
                            RuleName = "Crafting_Piece_Difficulty_Valid",
                            Message = $"制作部件 {pieceId} 的难度无效",
                            Severity = ValidationSeverity.Warning,
                            Suggestion = "制作部件难度应该在0-300范围内"
                        }).ToList();
                    }
                },
                new ImplicitValidationRule
                {
                    Name = "Crafting_Piece_Stat_Valid",
                    Description = "制作部件属性值必须在有效范围内",
                    Severity = ValidationSeverity.Warning,
                    Validator = (doc, context) =>
                    {
                        var results = new List<ValidationResult>();
                        
                        // 检查伤害
                        var damages = doc.XPathSelectElements("//CraftingPiece[@damage]")
                            .Where(e => !int.TryParse(e.Attribute("damage")?.Value, out int damage) || damage < 0 || damage > 1000)
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        foreach (var pieceId in damages)
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Crafting_Piece_Stat_Valid",
                                Message = $"制作部件 {pieceId} 的伤害值无效",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "伤害值应该在0-1000范围内"
                            });
                        }
                        
                        // 检查速度
                        var speeds = doc.XPathSelectElements("//CraftingPiece[@speed]")
                            .Where(e => !int.TryParse(e.Attribute("speed")?.Value, out int speed) || speed < 0 || speed > 200)
                            .Select(e => e.Attribute("id")?.Value ?? "Unknown")
                            .ToList();
                        
                        foreach (var pieceId in speeds)
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Crafting_Piece_Stat_Valid",
                                Message = $"制作部件 {pieceId} 的速度值无效",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "速度值应该在0-200范围内"
                            });
                        }
                        
                        return results;
                    }
                }
            };
        }

        /// <summary>
        /// 初始化引用完整性验证规则
        /// </summary>
        private void InitializeReferenceRules()
        {
            _validationRules["*"] = _validationRules.GetValueOrDefault("*") ?? new List<ImplicitValidationRule>();
            
            _validationRules["*"].Add(new ImplicitValidationRule
            {
                Name = "Reference_Integrity_Valid",
                Description = "所有引用必须指向存在的对象",
                Severity = ValidationSeverity.Error,
                Validator = (doc, context) =>
                {
                    var results = new List<ValidationResult>();
                    
                    // 检查物品引用
                    var itemReferences = doc.XPathSelectElements("//*[@item]")
                        .Select(e => new
                        {
                            Element = e,
                            Reference = e.Attribute("item")?.Value,
                            ElementId = e.Attribute("id")?.Value ?? e.Parent?.Attribute("id")?.Value ?? "Unknown"
                        })
                        .Where(r => !string.IsNullOrEmpty(r.Reference))
                        .ToList();
                    
                    foreach (var reference in itemReferences)
                    {
                        if (!context.AvailableItems.Contains(reference.Reference))
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Reference_Integrity_Valid",
                                Message = $"元素 {reference.ElementId} 引用了不存在的物品: {reference.Reference}",
                                Severity = ValidationSeverity.Error,
                                Suggestion = "确保引用的物品在items.xml中存在"
                            });
                        }
                    }
                    
                    // 检查角色引用
                    var characterReferences = doc.XPathSelectElements("//*[@character]")
                        .Select(e => new
                        {
                            Element = e,
                            Reference = e.Attribute("character")?.Value,
                            ElementId = e.Attribute("id")?.Value ?? e.Parent?.Attribute("id")?.Value ?? "Unknown"
                        })
                        .Where(r => !string.IsNullOrEmpty(r.Reference))
                        .ToList();
                    
                    foreach (var reference in characterReferences)
                    {
                        if (!context.AvailableCharacters.Contains(reference.Reference))
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Reference_Integrity_Valid",
                                Message = $"元素 {reference.ElementId} 引用了不存在的角色: {reference.Reference}",
                                Severity = ValidationSeverity.Error,
                                Suggestion = "确保引用的角色在characters.xml中存在"
                            });
                        }
                    }
                    
                    return results;
                }
            });
        }

        /// <summary>
        /// 初始化数值范围验证规则
        /// </summary>
        private void InitializeValueRangeRules()
        {
            _validationRules["*"] = _validationRules.GetValueOrDefault("*") ?? new List<ImplicitValidationRule>();
            
            _validationRules["*"].Add(new ImplicitValidationRule
            {
                Name = "Value_Range_Valid",
                Description = "数值必须在有效范围内",
                Severity = ValidationSeverity.Warning,
                Validator = (doc, context) =>
                {
                    var results = new List<ValidationResult>();
                    
                    // 检查所有数值属性
                    var numericAttributes = doc.XPathSelectElements("//*[@*[number(.) = number(.)]]")
                        .SelectMany(e => e.Attributes())
                        .Where(a => float.TryParse(a.Value, out _))
                        .ToList();
                    
                    foreach (var attr in numericAttributes)
                    {
                        var value = float.Parse(attr.Value);
                        var elementId = attr.Parent.Attribute("id")?.Value ?? "Unknown";
                        
                        // 检查常见数值范围
                        if (attr.Name == "weight" && (value < 0 || value > 1000))
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Value_Range_Valid",
                                Message = $"元素 {elementId} 的重量值 {value} 超出范围",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "重量值应该在0-1000范围内"
                            });
                        }
                        else if (attr.Name == "value" && value < 0)
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Value_Range_Valid",
                                Message = $"元素 {elementId} 的价值值 {value} 为负数",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "价值值应该为非负数"
                            });
                        }
                        else if (attr.Name == "damage" && (value < 0 || value > 10000))
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Value_Range_Valid",
                                Message = $"元素 {elementId} 的伤害值 {value} 超出范围",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "伤害值应该在0-10000范围内"
                            });
                        }
                        else if (attr.Name == "speed" && (value < 0 || value > 500))
                        {
                            results.Add(new ValidationResult
                            {
                                RuleName = "Value_Range_Valid",
                                Message = $"元素 {elementId} 的速度值 {value} 超出范围",
                                Severity = ValidationSeverity.Warning,
                                Suggestion = "速度值应该在0-500范围内"
                            });
                        }
                    }
                    
                    return results;
                }
            });
        }

        /// <summary>
        /// 验证指定的XML文件
        /// </summary>
        public ImplicitValidationResult ValidateXmlFile(string xmlFilePath, ValidationContext context)
        {
            var result = new ImplicitValidationResult
            {
                FilePath = xmlFilePath,
                FileName = System.IO.Path.GetFileName(xmlFilePath)
            };
            
            try
            {
                var doc = XDocument.Load(xmlFilePath);
                var fileBaseName = System.IO.Path.GetFileNameWithoutExtension(xmlFilePath);
                
                // 获取适用的验证规则
                var rules = GetApplicableRules(fileBaseName);
                
                // 执行验证
                foreach (var rule in rules)
                {
                    try
                    {
                        var ruleResults = rule.Validator(doc, context);
                        result.Results.AddRange(ruleResults);
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"执行规则 {rule.Name} 时发生错误: {ex.Message}");
                    }
                }
                
                // 计算验证结果统计
                result.ErrorCount = result.Results.Count(r => r.Severity == ValidationSeverity.Error);
                result.WarningCount = result.Results.Count(r => r.Severity == ValidationSeverity.Warning);
                result.InfoCount = result.Results.Count(r => r.Severity == ValidationSeverity.Info);
                
                result.IsValid = result.ErrorCount == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"加载XML文件时发生错误: {ex.Message}");
                result.IsValid = false;
            }
            
            return result;
        }

        /// <summary>
        /// 获取适用的验证规则
        /// </summary>
        private List<ImplicitValidationRule> GetApplicableRules(string fileBaseName)
        {
            var rules = new List<ImplicitValidationRule>();
            
            // 添加通用规则
            if (_validationRules.ContainsKey("*"))
            {
                rules.AddRange(_validationRules["*"]);
            }
            
            // 添加特定文件的规则
            if (_validationRules.ContainsKey(fileBaseName))
            {
                rules.AddRange(_validationRules[fileBaseName]);
            }
            
            return rules;
        }

        /// <summary>
        /// 验证ID格式是否有效
        /// </summary>
        private bool IsValidIdFormat(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;
            
            // ID应该只包含小写字母、数字和下划线
            return id.All(c => char.IsLetterOrDigit(c) || c == '_') && 
                   id.Any(char.IsLetter);
        }

        /// <summary>
        /// 验证整个模块目录
        /// </summary>
        public ImplicitValidationBatchResult ValidateModule(string moduleDataPath)
        {
            var batchResult = new ImplicitValidationBatchResult();
            
            try
            {
                // 首先分析依赖关系
                var dependencyResult = _dependencyAnalyzer.AnalyzeDependencies(moduleDataPath);
                
                // 构建验证上下文
                var context = new ValidationContext();
                
                // 收集所有可用的对象ID
                CollectAvailableObjects(moduleDataPath, context);
                
                // 获取所有XML文件
                var xmlFiles = _fileDiscoveryService.GetAllXmlFiles(moduleDataPath);
                
                // 验证每个文件
                foreach (var xmlFile in xmlFiles)
                {
                    var fileResult = ValidateXmlFile(xmlFile, context);
                    batchResult.FileResults.Add(fileResult);
                }
                
                // 计算汇总统计
                batchResult.TotalFiles = batchResult.FileResults.Count;
                batchResult.TotalErrors = batchResult.FileResults.Sum(r => r.ErrorCount);
                batchResult.TotalWarnings = batchResult.FileResults.Sum(r => r.WarningCount);
                batchResult.TotalInfos = batchResult.FileResults.Sum(r => r.InfoCount);
                batchResult.IsValid = batchResult.TotalErrors == 0;
                
                // 添加依赖分析结果
                batchResult.DependencyAnalysisResult = dependencyResult;
            }
            catch (Exception ex)
            {
                batchResult.Errors.Add($"验证过程中发生错误: {ex.Message}");
                batchResult.IsValid = false;
            }
            
            return batchResult;
        }

        /// <summary>
        /// 收集所有可用的对象ID
        /// </summary>
        public void CollectAvailableObjects(string moduleDataPath, ValidationContext context)
        {
            try
            {
                var xmlFiles = _fileDiscoveryService.GetAllXmlFiles(moduleDataPath);
                
                foreach (var xmlFile in xmlFiles)
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(xmlFile);
                    
                    try
                    {
                        var doc = XDocument.Load(xmlFile);
                        
                        // 收集物品ID
                        if (fileName == "items")
                        {
                            var itemIds = doc.XPathSelectElements("//Item[@id]")
                                .Select(e => e.Attribute("id")?.Value)
                                .Where(id => !string.IsNullOrEmpty(id))
                                .Cast<string>()
                                .ToList();
                            context.AvailableItems.AddRange(itemIds);
                        }
                        
                        // 收集角色ID
                        if (fileName == "characters")
                        {
                            var characterIds = doc.XPathSelectElements("//Character[@id]")
                                .Select(e => e.Attribute("id")?.Value)
                                .Where(id => !string.IsNullOrEmpty(id))
                                .Cast<string>()
                                .ToList();
                            context.AvailableCharacters.AddRange(characterIds);
                        }
                        
                        // 收集文化ID
                        if (fileName == "cultures")
                        {
                            var cultureIds = doc.XPathSelectElements("//Culture[@id]")
                                .Select(e => e.Attribute("id")?.Value)
                                .Where(id => !string.IsNullOrEmpty(id))
                                .Cast<string>()
                                .ToList();
                            context.AvailableCultures.AddRange(cultureIds);
                        }
                        
                        // 收集技能ID
                        if (fileName == "skills")
                        {
                            var skillIds = doc.XPathSelectElements("//Skill[@id]")
                                .Select(e => e.Attribute("id")?.Value)
                                .Where(id => !string.IsNullOrEmpty(id))
                                .Cast<string>()
                                .ToList();
                            context.AvailableSkills.AddRange(skillIds);
                        }
                        
                        // 收集制作部件ID
                        if (fileName == "crafting_pieces")
                        {
                            var craftingIds = doc.XPathSelectElements("//CraftingPiece[@id]")
                                .Select(e => e.Attribute("id")?.Value)
                                .Where(id => !string.IsNullOrEmpty(id))
                                .Cast<string>()
                                .ToList();
                            context.AvailableCraftingPieces.AddRange(craftingIds);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"收集对象ID时发生错误 {xmlFile}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"收集可用对象时发生错误: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 隐式验证规则
    /// </summary>
    public class ImplicitValidationRule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ValidationSeverity Severity { get; set; }
        public Func<XDocument, ValidationContext, List<ValidationResult>> Validator { get; set; }
    }

    /// <summary>
    /// 验证严重程度
    /// </summary>
    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        public string RuleName { get; set; }
        public string Message { get; set; }
        public ValidationSeverity Severity { get; set; }
        public string Suggestion { get; set; }
    }

    /// <summary>
    /// 验证上下文
    /// </summary>
    public class ValidationContext
    {
        public List<string> AvailableItems { get; set; } = new();
        public List<string> AvailableCharacters { get; set; } = new();
        public List<string> AvailableCultures { get; set; } = new();
        public List<string> AvailableSkills { get; set; } = new();
        public List<string> AvailableCraftingPieces { get; set; } = new();
    }

    /// <summary>
    /// 隐式验证结果
    /// </summary>
    public class ImplicitValidationResult
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public List<ValidationResult> Results { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 批量验证结果
    /// </summary>
    public class ImplicitValidationBatchResult
    {
        public int TotalFiles { get; set; }
        public int TotalErrors { get; set; }
        public int TotalWarnings { get; set; }
        public int TotalInfos { get; set; }
        public List<ImplicitValidationResult> FileResults { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public XmlDependencyAnalysisResult DependencyAnalysisResult { get; set; }
        public bool IsValid { get; set; }
    }
}