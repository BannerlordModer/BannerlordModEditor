using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// XML校验系统核心
    /// 提供统一的XML文件验证接口，整合依赖关系分析和隐式校验逻辑
    /// </summary>
    public class XmlValidationSystemCore
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly XmlDependencyAnalyzer _dependencyAnalyzer;
        private readonly ImplicitValidationDetector _validationDetector;
        
        public XmlValidationSystemCore(
            IFileDiscoveryService fileDiscoveryService,
            XmlDependencyAnalyzer dependencyAnalyzer,
            ImplicitValidationDetector validationDetector)
        {
            _fileDiscoveryService = fileDiscoveryService;
            _dependencyAnalyzer = dependencyAnalyzer;
            _validationDetector = validationDetector;
        }

        /// <summary>
        /// 执行完整的XML验证流程
        /// </summary>
        public async Task<XmlValidationResult> ValidateModuleAsync(string moduleDataPath)
        {
            return await Task.Run(() => ValidateModule(moduleDataPath));
        }

        /// <summary>
        /// 执行完整的XML验证流程（同步版本）
        /// </summary>
        public XmlValidationResult ValidateModule(string moduleDataPath)
        {
            var result = new XmlValidationResult
            {
                ModuleDataPath = moduleDataPath,
                ValidationTimestamp = DateTime.Now
            };

            try
            {
                // 1. 分析依赖关系
                result.DependencyAnalysis = _dependencyAnalyzer.AnalyzeDependencies(moduleDataPath);
                
                // 2. 执行隐式校验
                result.ImplicitValidation = _validationDetector.ValidateModule(moduleDataPath);
                
                // 3. 执行Schema验证
                result.SchemaValidation = ValidateSchemas(moduleDataPath);
                
                // 4. 生成汇总报告
                GenerateSummaryReport(result);
                
                // 5. 生成修复建议
                GenerateFixSuggestions(result);
                
                result.IsValid = result.TotalErrors == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"验证过程中发生错误: {ex.Message}");
                result.IsValid = false;
            }

            return result;
        }

        /// <summary>
        /// 验证单个XML文件
        /// </summary>
        public XmlFileValidationResult ValidateSingleFile(string xmlFilePath)
        {
            var result = new XmlFileValidationResult
            {
                FilePath = xmlFilePath,
                FileName = System.IO.Path.GetFileName(xmlFilePath),
                ValidationTimestamp = DateTime.Now
            };

            try
            {
                var moduleDataPath = System.IO.Path.GetDirectoryName(xmlFilePath);
                
                // 1. 分析文件依赖关系
                var dependencyResult = _dependencyAnalyzer.AnalyzeDependencies(moduleDataPath);
                var fileDependency = dependencyResult.FileResults.FirstOrDefault(f => f.FilePath == xmlFilePath);
                
                if (fileDependency != null)
                {
                    result.DependencyAnalysis = fileDependency;
                }
                
                // 2. 构建验证上下文
                var context = new ValidationContext();
                _validationDetector.CollectAvailableObjects(moduleDataPath, context);
                
                // 3. 执行隐式校验
                result.ImplicitValidation = _validationDetector.ValidateXmlFile(xmlFilePath, context);
                
                // 4. 执行Schema验证
                result.SchemaValidation = ValidateFileSchema(xmlFilePath);
                
                result.IsValid = result.TotalErrors == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"验证文件时发生错误: {ex.Message}");
                result.IsValid = false;
            }

            return result;
        }

        /// <summary>
        /// 执行Schema验证
        /// </summary>
        private SchemaValidationResult ValidateSchemas(string moduleDataPath)
        {
            var result = new SchemaValidationResult();
            
            try
            {
                var xmlFiles = _fileDiscoveryService.GetAllXmlFiles(moduleDataPath);
                
                foreach (var xmlFile in xmlFiles)
                {
                    var fileResult = ValidateFileSchema(xmlFile);
                    result.FileResults.Add(fileResult);
                }
                
                result.TotalFiles = result.FileResults.Count;
                result.TotalErrors = result.FileResults.Sum(r => r.ErrorCount);
                result.IsValid = result.TotalErrors == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Schema验证过程中发生错误: {ex.Message}");
                result.IsValid = false;
            }
            
            return result;
        }

        /// <summary>
        /// 验证单个文件的Schema
        /// </summary>
        private SchemaFileValidationResult ValidateFileSchema(string xmlFilePath)
        {
            var result = new SchemaFileValidationResult
            {
                FilePath = xmlFilePath,
                FileName = System.IO.Path.GetFileName(xmlFilePath)
            };

            try
            {
                var settings = new System.Xml.XmlReaderSettings
                {
                    ValidationType = System.Xml.ValidationType.Schema,
                    ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings
                };

                // 添加事件处理器
                settings.ValidationEventHandler += (sender, e) =>
                {
                    var severity = e.Severity == System.Xml.Schema.XmlSeverityType.Error 
                        ? ValidationSeverity.Error 
                        : ValidationSeverity.Warning;
                    
                    result.Results.Add(new SchemaValidationErrorDetail
                    {
                        Message = e.Message,
                        Severity = severity,
                        LineNumber = e.Exception?.LineNumber ?? 0,
                        LinePosition = e.Exception?.LinePosition ?? 0
                    });
                };

                // 尝试验证
                using (var reader = System.Xml.XmlReader.Create(xmlFilePath, settings))
                {
                    while (reader.Read())
                    {
                        // 读取整个文档以触发验证
                    }
                }
                
                result.ErrorCount = result.Results.Count(r => r.Severity == ValidationSeverity.Error);
                result.WarningCount = result.Results.Count(r => r.Severity == ValidationSeverity.Warning);
                result.IsValid = result.ErrorCount == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Schema验证时发生错误: {ex.Message}");
                result.IsValid = false;
            }

            return result;
        }

        /// <summary>
        /// 生成汇总报告
        /// </summary>
        private void GenerateSummaryReport(XmlValidationResult result)
        {
            result.TotalFiles = result.ImplicitValidation.TotalFiles;
            result.TotalErrors = result.ImplicitValidation.TotalErrors + 
                                result.SchemaValidation.TotalErrors +
                                result.DependencyAnalysis.FileResults.Count(f => !f.IsValid);
            result.TotalWarnings = result.ImplicitValidation.TotalWarnings + 
                                   result.SchemaValidation.FileResults.Sum(f => f.WarningCount);
            result.TotalInfos = result.ImplicitValidation.TotalInfos;
            
            // 汇总所有错误信息
            result.Errors.Clear(); // 清空可能的重复错误
            
            // 汇总依赖分析错误
            foreach (var fileResult in result.DependencyAnalysis.FileResults)
            {
                if (fileResult.Errors.Count > 0)
                {
                    result.Errors.AddRange(fileResult.Errors.Select(e => $"{fileResult.FileName}: {e}"));
                }
            }
            
            // 汇总隐式验证错误
            foreach (var fileResult in result.ImplicitValidation.FileResults)
            {
                if (fileResult.Errors.Count > 0)
                {
                    result.Errors.AddRange(fileResult.Errors.Select(e => $"{fileResult.FileName}: {e}"));
                }
            }
            
            // 汇总Schema验证错误
            foreach (var fileResult in result.SchemaValidation.FileResults)
            {
                if (fileResult.Errors.Count > 0)
                {
                    result.Errors.AddRange(fileResult.Errors.Select(e => $"{fileResult.FileName}: {e}"));
                }
            }

            // 添加关键问题摘要
            if (result.DependencyAnalysis.CircularDependencies.Count > 0)
            {
                result.Summary.Issues.Add($"发现 {result.DependencyAnalysis.CircularDependencies.Count} 个循环依赖");
            }
            
            if (result.TotalErrors > 0)
            {
                result.Summary.Issues.Add($"发现 {result.TotalErrors} 个错误需要修复");
            }
            
            if (result.TotalWarnings > 0)
            {
                result.Summary.Issues.Add($"发现 {result.TotalWarnings} 个警告建议关注");
            }
        }

        /// <summary>
        /// 生成修复建议
        /// </summary>
        private void GenerateFixSuggestions(XmlValidationResult result)
        {
            // 循环依赖修复建议
            if (result.DependencyAnalysis.CircularDependencies.Count > 0)
            {
                result.FixSuggestions.Add(new FixSuggestion
                {
                    Category = "循环依赖",
                    Priority = FixPriority.High,
                    Description = "检测到循环依赖，可能导致加载失败",
                    Steps = result.DependencyAnalysis.CircularDependencies.Select(c => 
                        $"移除循环: {string.Join(" -> ", c.Cycle)}").ToList()
                });
            }

            // 缺失依赖修复建议
            var missingDeps = result.DependencyAnalysis.FileResults
                .SelectMany(f => f.MissingDependencies)
                .Distinct()
                .ToList();
            
            if (missingDeps.Count > 0)
            {
                result.FixSuggestions.Add(new FixSuggestion
                {
                    Category = "缺失依赖",
                    Priority = FixPriority.High,
                    Description = "检测到缺失的依赖文件",
                    Steps = missingDeps.Select(dep => 
                        $"创建缺失的文件: {dep}.xml 或移除相关引用").ToList()
                });
            }

            // 隐式校验错误修复建议
            var implicitErrors = result.ImplicitValidation.FileResults
                .SelectMany(f => f.Results.Where(r => r.Severity == ValidationSeverity.Error))
                .GroupBy(r => r.RuleName)
                .ToList();
            
            foreach (var errorGroup in implicitErrors)
            {
                result.FixSuggestions.Add(new FixSuggestion
                {
                    Category = "隐式校验错误",
                    Priority = FixPriority.Medium,
                    Description = $"修复 {errorGroup.Key} 相关的错误",
                    Steps = errorGroup.Select(e => e.Suggestion).Distinct().ToList()
                });
            }

            // Schema验证错误修复建议
            var schemaErrors = result.SchemaValidation.FileResults
                .SelectMany(f => f.Results.Where(r => r.Severity == ValidationSeverity.Error))
                .ToList();
            
            if (schemaErrors.Count > 0)
            {
                result.FixSuggestions.Add(new FixSuggestion
                {
                    Category = "Schema验证",
                    Priority = FixPriority.Medium,
                    Description = "修复XML Schema验证错误",
                    Steps = schemaErrors.Select(e => 
                        $"文件 {e.FilePath} 第 {e.LineNumber} 行: {e.Message}").ToList()
                });
            }
        }

        /// <summary>
        /// 获取推荐的加载顺序
        /// </summary>
        public List<string> GetRecommendedLoadOrder(string moduleDataPath)
        {
            var dependencyResult = _dependencyAnalyzer.AnalyzeDependencies(moduleDataPath);
            return dependencyResult.LoadOrder;
        }

        /// <summary>
        /// 获取文件依赖关系图
        /// </summary>
        public Dictionary<string, List<string>> GetDependencyGraph(string moduleDataPath)
        {
            var dependencyResult = _dependencyAnalyzer.AnalyzeDependencies(moduleDataPath);
            var graph = new Dictionary<string, List<string>>();
            
            foreach (var fileResult in dependencyResult.FileResults)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(fileResult.FileName);
                graph[fileName] = fileResult.AllDependencies;
            }
            
            return graph;
        }

        /// <summary>
        /// 验证特定规则
        /// </summary>
        public List<ValidationResult> ValidateSpecificRule(string moduleDataPath, string ruleName)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext();
            _validationDetector.CollectAvailableObjects(moduleDataPath, context);
            
            var xmlFiles = _fileDiscoveryService.GetAllXmlFiles(moduleDataPath);
            
            foreach (var xmlFile in xmlFiles)
            {
                try
                {
                    var doc = System.Xml.Linq.XDocument.Load(xmlFile);
                    var fileBaseName = System.IO.Path.GetFileNameWithoutExtension(xmlFile);
                    
                    // 查找匹配的规则
                    var rule = FindValidationRule(fileBaseName, ruleName);
                    if (rule != null)
                    {
                        var ruleResults = rule.Validator(doc, context);
                        results.AddRange(ruleResults);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"验证规则时发生错误 {xmlFile}: {ex.Message}");
                }
            }
            
            return results;
        }

        /// <summary>
        /// 查找指定的验证规则
        /// </summary>
        private ImplicitValidationRule FindValidationRule(string fileBaseName, string ruleName)
        {
            // 这里需要从ImplicitValidationDetector中获取规则
            // 暂时返回null，实际实现需要暴露规则查找接口
            return null;
        }
    }

    /// <summary>
    /// XML验证结果
    /// </summary>
    public class XmlValidationResult
    {
        public string ModuleDataPath { get; set; }
        public DateTime ValidationTimestamp { get; set; }
        public int TotalFiles { get; set; }
        public int TotalErrors { get; set; }
        public int TotalWarnings { get; set; }
        public int TotalInfos { get; set; }
        public XmlDependencyAnalysisResult DependencyAnalysis { get; set; }
        public ImplicitValidationBatchResult ImplicitValidation { get; set; }
        public SchemaValidationResult SchemaValidation { get; set; }
        public ValidationSummary Summary { get; set; } = new();
        public List<FixSuggestion> FixSuggestions { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 单个文件验证结果
    /// </summary>
    public class XmlFileValidationResult
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime ValidationTimestamp { get; set; }
        public XmlFileDependencyResult DependencyAnalysis { get; set; }
        public ImplicitValidationResult ImplicitValidation { get; set; }
        public SchemaFileValidationResult SchemaValidation { get; set; }
        public List<string> Errors { get; set; } = new();
        public int TotalErrors => Errors.Count + 
                                 (ImplicitValidation?.ErrorCount ?? 0) + 
                                 (SchemaValidation?.ErrorCount ?? 0);
        public int TotalWarnings => (ImplicitValidation?.WarningCount ?? 0) + 
                                    (SchemaValidation?.WarningCount ?? 0);
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// Schema验证结果
    /// </summary>
    public class SchemaValidationResult
    {
        public int TotalFiles { get; set; }
        public int TotalErrors { get; set; }
        public List<SchemaFileValidationResult> FileResults { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 单个文件Schema验证结果
    /// </summary>
    public class SchemaFileValidationResult
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public List<SchemaValidationErrorDetail> Results { get; set; } = new();
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// Schema验证结果详情
    /// </summary>
    public class SchemaValidationErrorDetail
    {
        public string Message { get; set; }
        public ValidationSeverity Severity { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }
        public string FilePath { get; set; }
    }

    /// <summary>
    /// 验证摘要
    /// </summary>
    public class ValidationSummary
    {
        public List<string> Issues { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public string Status { get; set; }
    }

    /// <summary>
    /// 修复建议
    /// </summary>
    public class FixSuggestion
    {
        public string Category { get; set; }
        public FixPriority Priority { get; set; }
        public string Description { get; set; }
        public List<string> Steps { get; set; } = new();
    }

    /// <summary>
    /// 修复优先级
    /// </summary>
    public enum FixPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
}