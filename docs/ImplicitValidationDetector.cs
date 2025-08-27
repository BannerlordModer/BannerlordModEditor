using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace BannerlordModEditor.Common.Validation.ImplicitValidation
{
    /// <summary>
    /// 隐式校验逻辑检测器
    /// 基于Mount & Blade源码中的隐式规则实现校验
    /// </summary>
    public interface IImplicitValidationDetector
    {
        Task<ImplicitValidationResult> DetectImplicitRulesAsync(XmlDocument xmlDocument, string xmlPath);
        Task<List<ImplicitValidationRule>> ExtractRulesFromSourceAsync(Type objectType);
        bool ValidateAgainstImplicitRules(object domainObject, List<ImplicitValidationRule> rules);
    }

    /// <summary>
    /// 隐式校验结果
    /// </summary>
    public class ImplicitValidationResult
    {
        public List<ImplicitValidationRule> DetectedRules { get; set; } = new();
        public List<ImplicitValidationViolation> Violations { get; set; } = new();
        public List<ImplicitValidationSuggestion> Suggestions { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// 隐式校验规则
    /// </summary>
    public class ImplicitValidationRule
    {
        public string RuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RuleCategory Category { get; set; }
        public RuleSeverity Severity { get; set; }
        public Type SourceType { get; set; }
        public MethodInfo SourceMethod { get; set; }
        public Predicate<object> ValidationPredicate { get; set; }
        public Func<object, string> ErrorMessageBuilder { get; set; }
        public bool IsCustomRule { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    /// <summary>
    /// 隐式校验违规
    /// </summary>
    public class ImplicitValidationViolation
    {
        public ImplicitValidationRule Rule { get; set; }
        public object ViolatingObject { get; set; }
        public string Message { get; set; }
        public XmlNode SourceNode { get; set; }
        public ViolationLocation Location { get; set; }
        public Dictionary<string, object> Context { get; set; } = new();
    }

    /// <summary>
    /// 校验建议
    /// </summary>
    public class ImplicitValidationSuggestion
    {
        public string SuggestionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SuggestionType Type { get; set; }
        public Func<XmlNode, object> FixAction { get; set; }
        public bool CanAutoFix { get; set; }
        public double Confidence { get; set; }
    }

    /// <summary>
    /// 规则类别
    /// </summary>
    public enum RuleCategory
    {
        BusinessLogic,      // 业务逻辑规则
        DataIntegrity,      // 数据完整性规则
        Performance,        // 性能相关规则
        Security,           // 安全相关规则
        Consistency,        // 一致性规则
        Convention          // 约定规则
    }

    /// <summary>
    /// 规则严重程度
    /// </summary>
    public enum RuleSeverity
    {
        Informational,      // 信息性
        Warning,            // 警告
        Error,              // 错误
        Critical            // 严重错误
    }

    /// <summary>
    /// 建议类型
    /// </summary>
    public enum SuggestionType
    {
        FixMissingValue,    // 修复缺失值
        AdjustRange,        // 调整范围
        NormalizeFormat,    // 标准化格式
        RemoveRedundancy,   // 移除冗余
        OptimizeStructure   // 优化结构
    }

    /// <summary>
    /// 违规位置
    /// </summary>
    public class ViolationLocation
    {
        public string XPath { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }
        public string XmlPath { get; set; }
    }

    /// <summary>
    /// 基于MBObjectManager的隐式校验检测器
    /// </summary>
    public class MbImplicitValidationDetector : IImplicitValidationDetector
    {
        private readonly IReflectionHelper _reflectionHelper;
        private readonly IXmlDeserializer _xmlDeserializer;
        private readonly Dictionary<Type, List<ImplicitValidationRule>> _ruleCache = new();

        public MbImplicitValidationDetector(
            IReflectionHelper reflectionHelper,
            IXmlDeserializer xmlDeserializer)
        {
            _reflectionHelper = reflectionHelper;
            _xmlDeserializer = xmlDeserializer;
        }

        public async Task<ImplicitValidationResult> DetectImplicitRulesAsync(XmlDocument xmlDocument, string xmlPath)
        {
            var result = new ImplicitValidationResult();
            
            try
            {
                // 识别XML类型
                var dataType = IdentifyXmlDataType(xmlDocument);
                
                // 获取对应的领域对象类型
                var domainType = GetDomainObjectType(dataType);
                
                if (domainType != null)
                {
                    // 提取隐式规则
                    var rules = await ExtractRulesFromSourceAsync(domainType);
                    result.DetectedRules.AddRange(rules);
                    
                    // 反序列化XML为对象
                    var domainObject = await _xmlDeserializer.DeserializeAsync(xmlDocument, domainType);
                    
                    // 验证隐式规则
                    ValidateAgainstImplicitRules(domainObject, rules);
                    
                    // 检测违规
                    var violations = DetectViolations(domainObject, rules, xmlDocument);
                    result.Violations.AddRange(violations);
                    
                    // 生成建议
                    var suggestions = GenerateSuggestions(violations);
                    result.Suggestions.AddRange(suggestions);
                }
            }
            catch (Exception ex)
            {
                result.Metadata["Error"] = ex.Message;
            }
            
            return result;
        }

        public async Task<List<ImplicitValidationRule>> ExtractRulesFromSourceAsync(Type objectType)
        {
            if (_ruleCache.ContainsKey(objectType))
            {
                return _ruleCache[objectType];
            }

            var rules = new List<ImplicitValidationRule>();
            
            // 从MBObjectBase提取基础规则
            await ExtractMbObjectBaseRules(objectType, rules);
            
            // 从特定类型提取规则
            await ExtractTypeSpecificRules(objectType, rules);
            
            // 从属性和字段提取规则
            await ExtractPropertyRules(objectType, rules);
            
            // 从方法提取规则
            await ExtractMethodRules(objectType, rules);
            
            _ruleCache[objectType] = rules;
            return rules;
        }

        public bool ValidateAgainstImplicitRules(object domainObject, List<ImplicitValidationRule> rules)
        {
            var isValid = true;
            
            foreach (var rule in rules)
            {
                try
                {
                    if (!rule.ValidationPredicate(domainObject))
                    {
                        isValid = false;
                        // 记录违规信息
                        var violation = new ImplicitValidationViolation
                        {
                            Rule = rule,
                            ViolatingObject = domainObject,
                            Message = rule.ErrorMessageBuilder(domainObject)
                        };
                        
                        // 处理违规...
                    }
                }
                catch (Exception ex)
                {
                    // 处理校验异常
                    System.Diagnostics.Debug.WriteLine($"Rule validation failed: {ex.Message}");
                }
            }
            
            return isValid;
        }

        private async Task ExtractMbObjectBaseRules(Type objectType, List<ImplicitValidationRule> rules)
        {
            // MBObjectBase的隐式规则
            rules.Add(new ImplicitValidationRule
            {
                RuleId = "MBObjectBase_StringIdRequired",
                Name = "String ID Required",
                Description = "MBObjectBase requires a non-empty StringId",
                Category = RuleCategory.DataIntegrity,
                Severity = RuleSeverity.Error,
                SourceType = objectType,
                ValidationPredicate = obj => 
                {
                    var stringIdProperty = objectType.GetProperty("StringId");
                    var value = stringIdProperty?.GetValue(obj);
                    return !string.IsNullOrEmpty(value as string);
                },
                ErrorMessageBuilder = obj => "StringId cannot be null or empty"
            });

            rules.Add(new ImplicitValidationRule
            {
                RuleId = "MBObjectBase_IsReadyValidation",
                Name = "Is Ready Validation",
                Description = "Object should be marked as ready after initialization",
                Category = RuleCategory.BusinessLogic,
                Severity = RuleSeverity.Warning,
                SourceType = objectType,
                ValidationPredicate = obj =>
                {
                    var isReadyProperty = objectType.GetProperty("IsReady");
                    var value = isReadyProperty?.GetValue(obj);
                    return value is bool ready && ready;
                },
                ErrorMessageBuilder = obj => "Object is not marked as ready"
            });
        }

        private async Task ExtractTypeSpecificRules(Type objectType, List<ImplicitValidationRule> rules)
        {
            // CraftingPiece特定规则
            if (objectType == typeof(CraftingPiece))
            {
                rules.AddRange(await GetCraftingPieceRules());
            }
            
            // ItemObject特定规则
            if (objectType == typeof(ItemObject))
            {
                rules.AddRange(await GetItemObjectRules());
            }
            
            // 可以添加更多类型的特定规则
        }

        private async Task<List<ImplicitValidationRule>> GetCraftingPieceRules()
        {
            var rules = new List<ImplicitValidationRule>();
            
            // CraftingPiece的PieceType必须有效
            rules.Add(new ImplicitValidationRule
            {
                RuleId = "CraftingPiece_ValidPieceType",
                Name = "Valid Piece Type",
                Description = "CraftingPiece must have a valid PieceType",
                Category = RuleCategory.DataIntegrity,
                Severity = RuleSeverity.Error,
                SourceType = typeof(CraftingPiece),
                ValidationPredicate = obj =>
                {
                    var piece = obj as CraftingPiece;
                    return piece != null && piece.PieceType != default;
                },
                ErrorMessageBuilder = obj => "CraftingPiece must have a valid PieceType"
            });
            
            // CraftingPiece的长度必须为正数
            rules.Add(new ImplicitValidationRule
            {
                RuleId = "CraftingPiece_PositiveLength",
                Name = "Positive Length",
                Description = "CraftingPiece length must be positive",
                Category = RuleCategory.DataIntegrity,
                Severity = RuleSeverity.Error,
                SourceType = typeof(CraftingPiece),
                ValidationPredicate = obj =>
                {
                    var piece = obj as CraftingPiece;
                    return piece != null && piece.Length > 0;
                },
                ErrorMessageBuilder = obj => "CraftingPiece length must be positive"
            });
            
            return rules;
        }

        private async Task<List<ImplicitValidationRule>> GetItemObjectRules()
        {
            var rules = new List<ImplicitValidationRule>();
            
            // ItemObject的ItemComponent不能为null
            rules.Add(new ImplicitValidationRule
            {
                RuleId = "ItemObject_ItemComponentRequired",
                Name = "Item Component Required",
                Description = "ItemObject must have an ItemComponent",
                Category = RuleCategory.DataIntegrity,
                Severity = RuleSeverity.Error,
                SourceType = typeof(ItemObject),
                ValidationPredicate = obj =>
                {
                    var item = obj as ItemObject;
                    return item != null && item.ItemComponent != null;
                },
                ErrorMessageBuilder = obj => "ItemObject must have an ItemComponent"
            });
            
            return rules;
        }

        private async Task ExtractPropertyRules(Type objectType, List<ImplicitValidationRule> rules)
        {
            var properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var property in properties)
            {
                // 检查属性的自定义验证特性
                var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
                
                foreach (var attribute in validationAttributes)
                {
                    rules.Add(new ImplicitValidationRule
                    {
                        RuleId = $"{objectType.Name}_{property.Name}_{attribute.GetType().Name}",
                        Name = $"{property.Name} {attribute.GetType().Name}",
                        Description = attribute.ErrorMessage,
                        Category = RuleCategory.DataIntegrity,
                        Severity = GetSeverityFromAttribute(attribute),
                        SourceType = objectType,
                        ValidationPredicate = obj =>
                        {
                            var value = property.GetValue(obj);
                            return attribute.IsValid(value);
                        },
                        ErrorMessageBuilder = obj => 
                        {
                            var value = property.GetValue(obj);
                            return attribute.FormatErrorMessage(property.Name);
                        }
                    });
                }
                
                // 检查可为值类型的空值
                if (property.PropertyType.IsValueType && 
                    Nullable.GetUnderlyingType(property.PropertyType) != null)
                {
                    rules.Add(new ImplicitValidationRule
                    {
                        RuleId = $"{objectType.Name}_{property.Name}_NullableCheck",
                        Name = $"{property.Name} Nullable Check",
                        Description = $"Nullable {property.Name} should have a value",
                        Category = RuleCategory.DataIntegrity,
                        Severity = RuleSeverity.Warning,
                        SourceType = objectType,
                        ValidationPredicate = obj => property.GetValue(obj) != null,
                        ErrorMessageBuilder = obj => $"{property.Name} should not be null"
                    });
                }
            }
        }

        private async Task ExtractMethodRules(Type objectType, List<ImplicitValidationRule> rules)
        {
            var methods = objectType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var method in methods)
            {
                // 检查以"ShouldSerialize"开头的方法
                if (method.Name.StartsWith("ShouldSerialize") && method.ReturnType == typeof(bool))
                {
                    var propertyName = method.Name.Substring("ShouldSerialize".Length);
                    
                    rules.Add(new ImplicitValidationRule
                    {
                        RuleId = $"{objectType.Name}_{propertyName}_ShouldSerialize",
                        Name = $"{propertyName} Should Serialize",
                        Description = $"Conditional serialization for {propertyName}",
                        Category = RuleCategory.Convention,
                        Severity = RuleSeverity.Informational,
                        SourceType = objectType,
                        SourceMethod = method,
                        ValidationPredicate = obj =>
                        {
                            var shouldSerialize = (bool)method.Invoke(obj, null);
                            var property = objectType.GetProperty(propertyName);
                            var value = property?.GetValue(obj);
                            
                            // 如果ShouldSerialize返回false，值应该为null或默认值
                            if (!shouldSerialize)
                            {
                                return value == null || value.Equals(GetDefaultValue(property.PropertyType));
                            }
                            
                            return true;
                        },
                        ErrorMessageBuilder = obj => 
                        {
                            var shouldSerialize = (bool)method.Invoke(obj, null);
                            return $"{propertyName} should {(shouldSerialize ? "not " : "")}be serialized";
                        }
                    });
                }
            }
        }

        private List<ImplicitValidationViolation> DetectViolations(object domainObject, List<ImplicitValidationRule> rules, XmlDocument xmlDocument)
        {
            var violations = new List<ImplicitValidationViolation>();
            
            foreach (var rule in rules)
            {
                try
                {
                    if (!rule.ValidationPredicate(domainObject))
                    {
                        var violation = new ImplicitValidationViolation
                        {
                            Rule = rule,
                            ViolatingObject = domainObject,
                            Message = rule.ErrorMessageBuilder(domainObject),
                            SourceNode = FindSourceNode(xmlDocument, domainObject)
                        };
                        
                        violations.Add(violation);
                    }
                }
                catch (Exception ex)
                {
                    violations.Add(new ImplicitValidationViolation
                    {
                        Rule = rule,
                        ViolatingObject = domainObject,
                        Message = $"Rule validation failed: {ex.Message}"
                    });
                }
            }
            
            return violations;
        }

        private List<ImplicitValidationSuggestion> GenerateSuggestions(List<ImplicitValidationViolation> violations)
        {
            var suggestions = new List<ImplicitValidationSuggestion>();
            
            foreach (var violation in violations)
            {
                var suggestion = CreateSuggestionForViolation(violation);
                if (suggestion != null)
                {
                    suggestions.Add(suggestion);
                }
            }
            
            return suggestions;
        }

        private ImplicitValidationSuggestion CreateSuggestionForViolation(ImplicitValidationViolation violation)
        {
            // 根据违规类型生成修复建议
            switch (violation.Rule.RuleId)
            {
                case "MBObjectBase_StringIdRequired":
                    return new ImplicitValidationSuggestion
                    {
                        SuggestionId = "GenerateStringId",
                        Title = "Generate String ID",
                        Description = "Generate a unique StringId for the object",
                        Type = SuggestionType.FixMissingValue,
                        CanAutoFix = true,
                        Confidence = 0.9
                    };
                    
                case "CraftingPiece_PositiveLength":
                    return new ImplicitValidationSuggestion
                    {
                        SuggestionId = "SetPositiveLength",
                        Title = "Set Positive Length",
                        Description = "Set a positive length value for the CraftingPiece",
                        Type = SuggestionType.AdjustRange,
                        CanAutoFix = true,
                        Confidence = 0.8
                    };
                    
                default:
                    return null;
            }
        }

        private XmlNode FindSourceNode(XmlDocument xmlDocument, object domainObject)
        {
            // 根据领域对象查找对应的XML节点
            // 这里需要实现具体的查找逻辑
            return xmlDocument.DocumentElement;
        }

        private object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private RuleSeverity GetSeverityFromAttribute(ValidationAttribute attribute)
        {
            if (attribute is RequiredAttribute)
                return RuleSeverity.Error;
            
            if (attribute is RangeAttribute)
                return RuleSeverity.Warning;
            
            return RuleSeverity.Informational;
        }

        private XmlDataType IdentifyXmlDataType(XmlDocument xmlDocument)
        {
            var rootElement = xmlDocument.DocumentElement;
            return rootElement?.Name switch
            {
                "CraftingPieces" => XmlDataType.CraftingPieces,
                "items" => XmlDataType.Items,
                "action_types" => XmlDataType.ActionTypes,
                "combat_parameters" => XmlDataType.CombatParameters,
                _ => XmlDataType.Unknown
            };
        }

        private Type GetDomainObjectType(XmlDataType dataType)
        {
            return dataType switch
            {
                XmlDataType.CraftingPieces => typeof(CraftingPiece),
                XmlDataType.Items => typeof(ItemObject),
                XmlDataType.ActionTypes => typeof(ActionSet),
                XmlDataType.CombatParameters => typeof(CombatParameters),
                _ => null
            };
        }
    }

    /// <summary>
    /// XML数据类型枚举
    /// </summary>
    public enum XmlDataType
    {
        CraftingPieces,
        Items,
        ActionTypes,
        CombatParameters,
        Unknown
    }

    /// <summary>
    /// 验证特性基类
    /// </summary>
    public abstract class ValidationAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public abstract bool IsValid(object value);
        public abstract string FormatErrorMessage(string name);
    }

    /// <summary>
    /// 必填验证特性
    /// </summary>
    public class RequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            
            if (value is string str)
                return !string.IsNullOrEmpty(str);
            
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} is required";
        }
    }

    /// <summary>
    /// 范围验证特性
    /// </summary>
    public class RangeAttribute : ValidationAttribute
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            
            if (double.TryParse(value.ToString(), out double numericValue))
            {
                return numericValue >= Minimum && numericValue <= Maximum;
            }
            
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be between {Minimum} and {Maximum}";
        }
    }
}