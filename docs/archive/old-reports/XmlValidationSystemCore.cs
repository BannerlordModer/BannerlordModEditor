using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Validation.DependencyAnalysis;
using BannerlordModEditor.Common.Validation.ImplicitValidation;

namespace BannerlordModEditor.Common.Validation.Core
{
    /// <summary>
    /// XML校验系统核心架构
    /// 基于Mount & Blade MBObjectManager的完整校验解决方案
    /// </summary>
    public interface IXmlValidationSystem
    {
        Task<ComprehensiveValidationResult> ValidateAllAsync(IEnumerable<string> xmlFiles);
        Task<ComprehensiveValidationResult> ValidateSingleAsync(string xmlPath);
        Task<ValidationReport> GenerateReportAsync(ComprehensiveValidationResult result);
        void RegisterValidator(IXmlValidator validator);
        void UnregisterValidator(IXmlValidator validator);
    }

    /// <summary>
    /// 综合校验结果
    /// </summary>
    public class ComprehensiveValidationResult
    {
        public DateTime ValidationStartTime { get; set; }
        public DateTime ValidationEndTime { get; set; }
        public Dictionary<string, ValidationResult> FileResults { get; set; } = new();
        public DependencyAnalysisResult DependencyAnalysis { get; set; }
        public List<ImplicitValidationResult> ImplicitValidationResults { get; set; } = new();
        public SchemaValidationResult SchemaValidationResult { get; set; }
        public ReferenceIntegrityResult ReferenceIntegrityResult { get; set; }
        public OverallValidationStatus OverallStatus { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Schema验证结果
    /// </summary>
    public class SchemaValidationResult
    {
        public bool IsValid { get; set; }
        public List<SchemaValidationError> Errors { get; set; } = new();
        public List<SchemaValidationWarning> Warnings { get; set; } = new();
        public string SchemaPath { get; set; }
        public List<string> ValidatedFiles { get; set; } = new();
    }

    /// <summary>
    /// Schema验证错误
    /// </summary>
    public class SchemaValidationError
    {
        public string XmlPath { get; set; }
        public string ErrorMessage { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }
        public string XPath { get; set; }
        public SchemaErrorType ErrorType { get; set; }
    }

    /// <summary>
    /// Schema验证警告
    /// </summary>
    public class SchemaValidationWarning
    {
        public string XmlPath { get; set; }
        public string WarningMessage { get; set; }
        public int LineNumber { get; set; }
        public string XPath { get; set; }
    }

    /// <summary>
    /// Schema错误类型
    /// </summary>
    public enum SchemaErrorType
    {
        ElementNotFound,
        AttributeNotFound,
        InvalidValue,
        MissingRequiredAttribute,
        TypeMismatch,
        SchemaValidationError
    }

    /// <summary>
    /// 引用完整性结果
    /// </summary>
    public class ReferenceIntegrityResult
    {
        public bool IsValid { get; set; }
        public List<ReferenceError> BrokenReferences { get; set; } = new();
        public List<ReferenceWarning> SuspiciousReferences { get; set; } = new();
        public Dictionary<string, List<string>> ObjectIndex { get; set; } = new();
        public int TotalReferences { get; set; }
        public int BrokenReferenceCount { get; set; }
    }

    /// <summary>
    /// 引用错误
    /// </summary>
    public class ReferenceError
    {
        public string SourceFile { get; set; }
        public string SourceObject { get; set; }
        public string TargetType { get; set; }
        public string TargetObject { get; set; }
        public string ReferenceString { get; set; }
        public ReferenceErrorType ErrorType { get; set; }
        public string XPath { get; set; }
    }

    /// <summary>
    /// 引用警告
    /// </summary>
    public class ReferenceWarning
    {
        public string SourceFile { get; set; }
        public string SourceObject { get; set; }
        public string TargetType { get; set; }
        public string TargetObject { get; set; }
        public string WarningMessage { get; set; }
        public ReferenceWarningType WarningType { get; set; }
    }

    /// <summary>
    /// 引用错误类型
    /// </summary>
    public enum ReferenceErrorType
    {
        ObjectNotFound,
        TypeNotFound,
        InvalidReferenceFormat,
        CircularReference,
        MissingReference
    }

    /// <summary>
    /// 引用警告类型
    /// </summary>
    public enum ReferenceWarningType
    {
        DeprecatedReference,
        SuspiciousReference,
        WeakReference,
        OptionalReference
    }

    /// <summary>
    /// 整体验证状态
    /// </summary>
    public enum OverallValidationStatus
    {
        Passed,
        PassedWithWarnings,
        Failed,
        CriticalError
    }

    /// <summary>
    /// 校验报告
    /// </summary>
    public class ValidationReport
    {
        public string ReportId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public OverallValidationStatus OverallStatus { get; set; }
        public SummaryStatistics Summary { get; set; }
        public List<FileValidationReport> FileReports { get; set; } = new();
        public List<Recommendation> Recommendations { get; set; } = new();
        public Dictionary<string, object> AdditionalData { get; set; } = new();
    }

    /// <summary>
    /// 摘要统计
    /// </summary>
    public class SummaryStatistics
    {
        public int TotalFiles { get; set; }
        public int ValidFiles { get; set; }
        public int FilesWithWarnings { get; set; }
        public int FailedFiles { get; set; }
        public int TotalErrors { get; set; }
        public int TotalWarnings { get; set; }
        public int TotalInfos { get; set; }
        public TimeSpan TotalValidationTime { get; set; }
        public Dictionary<string, int> ErrorTypes { get; set; } = new();
        public Dictionary<string, int> WarningTypes { get; set; } = new();
    }

    /// <summary>
    /// 文件校验报告
    /// </summary>
    public class FileValidationReport
    {
        public string FilePath { get; set; }
        public XmlDataType DataType { get; set; }
        public bool IsValid { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public List<ValidationError> Errors { get; set; } = new();
        public List<ValidationWarning> Warnings { get; set; } = new();
        public List<ValidationInfo> Infos { get; set; } = new();
        public TimeSpan ValidationTime { get; set; }
    }

    /// <summary>
    /// 建议
    /// </summary>
    public class Recommendation
    {
        public string RecommendationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public RecommendationPriority Priority { get; set; }
        public RecommendationCategory Category { get; set; }
        public List<string> AffectedFiles { get; set; } = new();
        public bool CanAutoFix { get; set; }
        public double Confidence { get; set; }
    }

    /// <summary>
    /// 建议优先级
    /// </summary>
    public enum RecommendationPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// 建议类别
    /// </summary>
    public enum RecommendationCategory
    {
        Performance,
        Security,
        Maintainability,
        Reliability,
        Compatibility
    }

    /// <summary>
    /// 校验设置
    /// </summary>
    public class ValidationSettings
    {
        public bool ValidateSchema { get; set; } = true;
        public bool ValidateDependencies { get; set; } = true;
        public bool ValidateImplicitRules { get; set; } = true;
        public bool ValidateReferences { get; set; } = true;
        public bool ValidateDataTypes { get; set; } = true;
        public bool ValidateValueRanges { get; set; } = true;
        public bool EnablePerformanceOptimizations { get; set; } = true;
        public bool EnableParallelValidation { get; set; } = true;
        public bool GenerateDetailedReport { get; set; } = true;
        public int MaxValidationThreads { get; set; } = Environment.ProcessorCount;
        public List<string> CustomSchemaPaths { get; set; } = new();
        public Dictionary<string, object> CustomOptions { get; set; } = new();
    }

    /// <summary>
    /// XML数据类型枚举
    /// </summary>
    public enum XmlDataType
    {
        CraftingPieces,
        Items,
        ActionTypes,
        ActionSets,
        CombatParameters,
        Skeletons,
        PhysicsMaterials,
        Monsters,
        Characters,
        Parties,
        Scenes,
        ModuleStrings,
        Sounds,
        MapIcons,
        Unknown
    }

    /// <summary>
    /// XML校验系统主实现
    /// </summary>
    public class BannerlordXmlValidationSystem : IXmlValidationSystem
    {
        private readonly List<IXmlValidator> _validators = new();
        private readonly IXmlDependencyAnalyzer _dependencyAnalyzer;
        private readonly IImplicitValidationDetector _implicitValidationDetector;
        private readonly ISchemaValidator _schemaValidator;
        private readonly IReferenceIntegrityChecker _referenceIntegrityChecker;
        private readonly IDataTypeValidator _dataTypeValidator;
        private readonly IValueRangeValidator _valueRangeValidator;
        private readonly IValidationReportGenerator _reportGenerator;
        private readonly ValidationSettings _settings;

        public BannerlordXmlValidationSystem(
            IXmlDependencyAnalyzer dependencyAnalyzer,
            IImplicitValidationDetector implicitValidationDetector,
            ISchemaValidator schemaValidator,
            IReferenceIntegrityChecker referenceIntegrityChecker,
            IDataTypeValidator dataTypeValidator,
            IValueRangeValidator valueRangeValidator,
            IValidationReportGenerator reportGenerator,
            ValidationSettings settings = null)
        {
            _dependencyAnalyzer = dependencyAnalyzer;
            _implicitValidationDetector = implicitValidationDetector;
            _schemaValidator = schemaValidator;
            _referenceIntegrityChecker = referenceIntegrityChecker;
            _dataTypeValidator = dataTypeValidator;
            _valueRangeValidator = valueRangeValidator;
            _reportGenerator = reportGenerator;
            _settings = settings ?? new ValidationSettings();

            RegisterDefaultValidators();
        }

        public async Task<ComprehensiveValidationResult> ValidateAllAsync(IEnumerable<string> xmlFiles)
        {
            var result = new ComprehensiveValidationResult
            {
                ValidationStartTime = DateTime.Now
            };

            try
            {
                var fileList = xmlFiles.ToList();
                
                // 1. 依赖关系分析
                if (_settings.ValidateDependencies)
                {
                    result.DependencyAnalysis = await AnalyzeDependenciesAsync(fileList);
                }

                // 2. Schema验证
                if (_settings.ValidateSchema)
                {
                    result.SchemaValidationResult = await ValidateSchemasAsync(fileList);
                }

                // 3. 构建对象索引
                var objectIndex = await BuildObjectIndexAsync(fileList);

                // 4. 并行验证各个文件
                if (_settings.EnableParallelValidation)
                {
                    await Parallel.ForEachAsync(fileList, async (xmlPath, cancellationToken) =>
                    {
                        var fileResult = await ValidateSingleFileInternalAsync(xmlPath, objectIndex);
                        lock (result.FileResults)
                        {
                            result.FileResults[xmlPath] = fileResult;
                        }
                    });
                }
                else
                {
                    foreach (var xmlPath in fileList)
                    {
                        var fileResult = await ValidateSingleFileInternalAsync(xmlPath, objectIndex);
                        result.FileResults[xmlPath] = fileResult;
                    }
                }

                // 5. 引用完整性检查
                if (_settings.ValidateReferences)
                {
                    result.ReferenceIntegrityResult = await CheckReferenceIntegrityAsync(fileList, objectIndex);
                }

                // 6. 计算整体状态
                result.OverallStatus = CalculateOverallStatus(result);
                result.ValidationEndTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                result.Metadata["Exception"] = ex.Message;
                result.OverallStatus = OverallValidationStatus.CriticalError;
            }

            return result;
        }

        public async Task<ComprehensiveValidationResult> ValidateSingleAsync(string xmlPath)
        {
            var result = new ComprehensiveValidationResult
            {
                ValidationStartTime = DateTime.Now
            };

            try
            {
                var fileResult = await ValidateSingleFileInternalAsync(xmlPath, new Dictionary<string, List<object>>());
                result.FileResults[xmlPath] = fileResult;
                result.OverallStatus = fileResult.IsValid ? OverallValidationStatus.Passed : OverallValidationStatus.Failed;
                result.ValidationEndTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                result.Metadata["Exception"] = ex.Message;
                result.OverallStatus = OverallValidationStatus.CriticalError;
            }

            return result;
        }

        public async Task<ValidationReport> GenerateReportAsync(ComprehensiveValidationResult result)
        {
            return await _reportGenerator.GenerateReportAsync(result);
        }

        public void RegisterValidator(IXmlValidator validator)
        {
            if (!_validators.Contains(validator))
            {
                _validators.Add(validator);
                _validators.Sort((v1, v2) => v2.Priority.CompareTo(v1.Priority));
            }
        }

        public void UnregisterValidator(IXmlValidator validator)
        {
            _validators.Remove(validator);
        }

        private async Task<DependencyAnalysisResult> AnalyzeDependenciesAsync(List<string> xmlFiles)
        {
            var loadingOrder = await _dependencyAnalyzer.DetermineLoadingOrderAsync(xmlFiles);
            var circularDependencies = _dependencyAnalyzer.FindCircularDependencies(xmlFiles);

            var analysisResult = new DependencyAnalysisResult
            {
                HasCircularDependencies = circularDependencies.Any()
            };

            // 分析每个文件的依赖关系
            foreach (var xmlFile in xmlFiles)
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(xmlFile);
                    var fileAnalysis = await _dependencyAnalyzer.AnalyzeDependenciesAsync(doc, xmlFile);
                    
                    // 合并分析结果
                    analysisResult.Dependencies.AddRange(fileAnalysis.Dependencies);
                    analysisResult.Dependents.AddRange(fileAnalysis.Dependents);
                    
                    foreach (var kvp in fileAnalysis.ObjectReferences)
                    {
                        if (!analysisResult.ObjectReferences.ContainsKey(kvp.Key))
                        {
                            analysisResult.ObjectReferences[kvp.Key] = new List<ReferenceInfo>();
                        }
                        analysisResult.ObjectReferences[kvp.Key].AddRange(kvp.Value);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error analyzing dependencies for {xmlFile}: {ex.Message}");
                }
            }

            return analysisResult;
        }

        private async Task<SchemaValidationResult> ValidateSchemasAsync(List<string> xmlFiles)
        {
            var result = new SchemaValidationResult();
            
            foreach (var xmlFile in xmlFiles)
            {
                try
                {
                    var schemaResult = await _schemaValidator.ValidateSchemaAsync(xmlFile);
                    result.ValidatedFiles.Add(xmlFile);
                    
                    if (!schemaResult.IsValid)
                    {
                        result.IsValid = false;
                        result.Errors.AddRange(schemaResult.Errors);
                    }
                    
                    result.Warnings.AddRange(schemaResult.Warnings);
                }
                catch (Exception ex)
                {
                    result.IsValid = false;
                    result.Errors.Add(new SchemaValidationError
                    {
                        XmlPath = xmlFile,
                        ErrorMessage = $"Schema validation failed: {ex.Message}",
                        ErrorType = SchemaErrorType.SchemaValidationError
                    });
                }
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }

        private async Task<Dictionary<string, List<object>>> BuildObjectIndexAsync(List<string> xmlFiles)
        {
            var objectIndex = new Dictionary<string, List<object>>();
            
            foreach (var xmlFile in xmlFiles)
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(xmlFile);
                    var objects = await ExtractObjectsFromXmlAsync(doc, xmlFile);
                    
                    var dataType = IdentifyXmlDataType(doc);
                    var typeName = dataType.ToString();
                    
                    if (!objectIndex.ContainsKey(typeName))
                    {
                        objectIndex[typeName] = new List<object>();
                    }
                    
                    objectIndex[typeName].AddRange(objects);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error indexing objects from {xmlFile}: {ex.Message}");
                }
            }

            return objectIndex;
        }

        private async Task<ReferenceIntegrityResult> CheckReferenceIntegrityAsync(List<string> xmlFiles, Dictionary<string, List<object>> objectIndex)
        {
            return await _referenceIntegrityChecker.CheckReferenceIntegrityAsync(xmlFiles, objectIndex);
        }

        private async Task<ValidationResult> ValidateSingleFileInternalAsync(string xmlPath, Dictionary<string, List<object>> objectIndex)
        {
            var result = new ValidationResult();
            var startTime = DateTime.Now;

            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);
                
                var context = new XmlValidationContext
                {
                    XmlDocument = doc,
                    XmlPath = xmlPath,
                    DataType = IdentifyXmlDataType(doc),
                    Settings = _settings
                };

                // 执行所有适用的验证器
                foreach (var validator in _validators.Where(v => v.CanValidate(context.DataType)))
                {
                    var validatorResult = await validator.ValidateAsync(context);
                    result.Errors.AddRange(validatorResult.Errors);
                    result.Warnings.AddRange(validatorResult.Warnings);
                    result.Infos.AddRange(validatorResult.Infos);
                }

                result.IsValid = !result.Errors.Any();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Message = $"Validation failed: {ex.Message}",
                    Severity = ValidationSeverity.Error
                });
            }

            result.ValidationDuration = DateTime.Now - startTime;
            return result;
        }

        private void RegisterDefaultValidators()
        {
            // 注册内置验证器
            RegisterValidator(new XmlSyntaxValidator());
            RegisterValidator(new DataTypeValidator(_dataTypeValidator));
            RegisterValidator(new ValueRangeValidator(_valueRangeValidator));
            RegisterValidator(new BusinessLogicValidator(_implicitValidationDetector));
            RegisterValidator(new ReferenceValidator(_referenceIntegrityChecker));
        }

        private OverallValidationStatus CalculateOverallStatus(ComprehensiveValidationResult result)
        {
            var totalErrors = result.FileResults.Values.Sum(r => r.Errors.Count);
            var totalWarnings = result.FileResults.Values.Sum(r => r.Warnings.Count);
            var failedFiles = result.FileResults.Values.Count(r => !r.IsValid);

            if (result.SchemaValidationResult != null && !result.SchemaValidationResult.IsValid)
            {
                return OverallValidationStatus.CriticalError;
            }

            if (result.ReferenceIntegrityResult != null && !result.ReferenceIntegrityResult.IsValid)
            {
                return OverallValidationStatus.Failed;
            }

            if (totalErrors > 0 || failedFiles > 0)
            {
                return OverallValidationStatus.Failed;
            }

            if (totalWarnings > 0)
            {
                return OverallValidationStatus.PassedWithWarnings;
            }

            return OverallValidationStatus.Passed;
        }

        private XmlDataType IdentifyXmlDataType(XmlDocument xmlDocument)
        {
            var rootElement = xmlDocument.DocumentElement;
            return rootElement?.Name switch
            {
                "CraftingPieces" => XmlDataType.CraftingPieces,
                "items" => XmlDataType.Items,
                "action_types" => XmlDataType.ActionTypes,
                "action_sets" => XmlDataType.ActionSets,
                "combat_parameters" => XmlDataType.CombatParameters,
                "skeletons" => XmlDataType.Skeletons,
                "physics_materials" => XmlDataType.PhysicsMaterials,
                "monsters" => XmlDataType.Monsters,
                "characters" => XmlDataType.Characters,
                "parties" => XmlDataType.Parties,
                "scenes" => XmlDataType.Scenes,
                "strings" => XmlDataType.ModuleStrings,
                "sounds" => XmlDataType.Sounds,
                "map_icons" => XmlDataType.MapIcons,
                _ => XmlDataType.Unknown
            };
        }

        private async Task<List<object>> ExtractObjectsFromXmlAsync(XmlDocument xmlDocument, string xmlPath)
        {
            // 这里需要实现从XML文档中提取对象的逻辑
            // 基于MBObjectManager的CreateObjectFromXmlNode方法
            var objects = new List<object>();
            
            try
            {
                var dataType = IdentifyXmlDataType(xmlDocument);
                var domainType = GetDomainObjectType(dataType);
                
                if (domainType != null)
                {
                    // 使用反序列化器提取对象
                    var deserializer = new XmlDeserializer();
                    var domainObject = await deserializer.DeserializeAsync(xmlDocument, domainType);
                    objects.Add(domainObject);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error extracting objects from {xmlPath}: {ex.Message}");
            }

            return objects;
        }

        private Type GetDomainObjectType(XmlDataType dataType)
        {
            return dataType switch
            {
                XmlDataType.CraftingPieces => typeof(CraftingPiece),
                XmlDataType.Items => typeof(ItemObject),
                XmlDataType.ActionTypes => typeof(ActionSet),
                XmlDataType.ActionSets => typeof(ActionSet),
                XmlDataType.CombatParameters => typeof(CombatParameters),
                XmlDataType.Skeletons => typeof(Skeleton),
                XmlDataType.PhysicsMaterials => typeof(PhysicsMaterial),
                XmlDataType.Monsters => typeof(Monster),
                XmlDataType.Characters => typeof(CharacterObject),
                XmlDataType.Parties => typeof(Party),
                XmlDataType.Scenes => typeof(Scene),
                _ => null
            };
        }
    }

    /// <summary>
    /// 验证错误
    /// </summary>
    public class ValidationError
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public ValidationSeverity Severity { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }
        public string XPath { get; set; }
        public string Category { get; set; }
        public Dictionary<string, object> Context { get; set; } = new();
    }

    /// <summary>
    /// 验证警告
    /// </summary>
    public class ValidationWarning
    {
        public string WarningCode { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public string XPath { get; set; }
        public string Category { get; set; }
    }

    /// <summary>
    /// 验证信息
    /// </summary>
    public class ValidationInfo
    {
        public string InfoCode { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
        public string Category { get; set; }
    }

    /// <summary>
    /// 验证严重程度
    /// </summary>
    public enum ValidationSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }
}