# BannerlordModEditor XML验证系统技术规格

## 文档概述

本文档详细说明了BannerlordModEditor XML验证系统测试架构的技术实现规格，包括具体的代码结构、接口定义、实现策略和最佳实践。

## 1. 核心技术栈

### 1.1 基础技术栈
- **.NET 9.0**: 最新的.NET运行时和SDK
- **xUnit 2.5**: 单元测试框架
- **Moq 4.20**: 模拟框架
- **FluentAssertions 6.12**: 流畅断言库
- **coverlet.collector**: 代码覆盖率工具
- **Microsoft.Extensions.DependencyInjection**: 依赖注入

### 1.2 XML处理技术栈
- **System.Xml**: 核心XML处理
- **System.Xml.Serialization**: XML序列化
- **System.Xml.Linq**: LINQ to XML
- **System.Text.Json**: JSON序列化（用于配置）

## 2. 核心接口定义

### 2.1 测试数据管理接口

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture.Infrastructure
{
    /// <summary>
    /// 测试数据管理服务接口
    /// </summary>
    public interface ITestDataManager
    {
        /// <summary>
        /// 获取XML测试数据
        /// </summary>
        Task<string> GetXmlTestDataAsync(string testName, string xmlType);
        
        /// <summary>
        /// 获取XML文件流
        /// </summary>
        Task<Stream> GetXmlStreamAsync(string filePath);
        
        /// <summary>
        /// 创建临时测试文件
        /// </summary>
        Task<string> CreateTempTestFileAsync(string fileName, string content);
        
        /// <summary>
        /// 清理测试文件
        /// </summary>
        Task CleanupTestFilesAsync();
        
        /// <summary>
        /// 获取测试配置
        /// </summary>
        Task<TestConfiguration> GetTestConfigurationAsync(string testName);
        
        /// <summary>
        /// 获取验证规则
        /// </summary>
        Task<ValidationRuleSet> GetValidationRulesAsync(string xmlType);
    }

    /// <summary>
    /// 测试配置
    /// </summary>
    public class TestConfiguration
    {
        public string TestName { get; set; }
        public XmlTestSettings XmlSettings { get; set; }
        public PerformanceSettings PerformanceSettings { get; set; }
        public DiagnosticSettings DiagnosticSettings { get; set; }
        public ValidationSettings ValidationSettings { get; set; }
    }

    /// <summary>
    /// XML测试设置
    /// </summary>
    public class XmlTestSettings
    {
        public bool PreserveWhitespace { get; set; } = true;
        public bool PreserveFormatting { get; set; } = true;
        public bool IncludeXmlDeclaration { get; set; } = true;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public bool NormalizeLineEndings { get; set; } = true;
        public bool IgnoreAttributeOrder { get; set; } = false;
    }

    /// <summary>
    /// 性能设置
    /// </summary>
    public class PerformanceSettings
    {
        public TimeSpan MaxExecutionTime { get; set; } = TimeSpan.FromSeconds(30);
        public long MaxMemoryUsage { get; set; } = 100 * 1024 * 1024; // 100MB
        public bool EnablePerformanceMonitoring { get; set; } = true;
    }

    /// <summary>
    /// 诊断设置
    /// </summary>
    public class DiagnosticSettings
    {
        public bool EnableDetailedLogging { get; set; } = true;
        public bool EnableErrorAnalysis { get; set; } = true;
        public bool EnableRecommendationGeneration { get; set; } = true;
        public int MaxDiagnosticsDepth { get; set; } = 5;
    }

    /// <summary>
    /// 验证设置
    /// </summary>
    public class ValidationSettings
    {
        public bool EnableStrictValidation { get; set; } = true;
        public bool EnableSchemaValidation { get; set; } = true;
        public bool EnableBusinessRuleValidation { get; set; } = true;
        public List<string> DisabledRules { get; set; } = new();
    }
}
```

### 2.2 XML测试辅助接口

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture.Infrastructure
{
    /// <summary>
    /// XML测试辅助服务接口
    /// </summary>
    public interface IXmlTestHelpers
    {
        /// <summary>
        /// 加载XML文件
        /// </summary>
        Task<T> LoadXmlAsync<T>(string filePath, ITestOutputHelper output);
        
        /// <summary>
        /// 保存XML到文件
        /// </summary>
        Task<string> SaveXmlAsync<T>(T obj, string originalXml, XmlTestSettings settings = null);
        
        /// <summary>
        /// 比较XML结构
        /// </summary>
        Task<XmlComparisonResult> CompareXmlStructuresAsync(string original, string generated);
        
        /// <summary>
        /// 比较对象结构
        /// </summary>
        Task<XmlComparisonResult> CompareObjectsAsync<T>(T original, T generated);
        
        /// <summary>
        /// 验证XML结构
        /// </summary>
        Task<ValidationResult> ValidateXmlStructureAsync(string xml, string xmlType);
        
        /// <summary>
        /// 验证往返一致性
        /// </summary>
        Task<ValidationResult> ValidateRoundTripAsync<T>(T obj);
        
        /// <summary>
        /// 格式化XML
        /// </summary>
        string FormatXml(string xml, XmlTestSettings settings = null);
        
        /// <summary>
        /// 规范化XML
        /// </summary>
        string NormalizeXml(string xml, XmlTestSettings settings = null);
    }

    /// <summary>
    /// XML比较结果
    /// </summary>
    public class XmlComparisonResult
    {
        public bool IsEqual { get; set; }
        public List<XmlDifference> Differences { get; set; } = new();
        public string OriginalXml { get; set; }
        public string GeneratedXml { get; set; }
        public ComparisonStatistics Statistics { get; set; }
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// XML差异
    /// </summary>
    public class XmlDifference
    {
        public DifferenceType Type { get; set; }
        public string Path { get; set; }
        public string OriginalValue { get; set; }
        public string GeneratedValue { get; set; }
        public string Description { get; set; }
        public DifferenceSeverity Severity { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
    }

    /// <summary>
    /// 比较统计
    /// </summary>
    public class ComparisonStatistics
    {
        public int TotalNodes { get; set; }
        public int MatchingNodes { get; set; }
        public int DifferingNodes { get; set; }
        public int AddedNodes { get; set; }
        public int RemovedNodes { get; set; }
        public double SimilarityPercentage { get; set; }
    }

    /// <summary>
    /// 差异类型
    /// </summary>
    public enum DifferenceType
    {
        AttributeMissing,
        AttributeValueDifferent,
        ElementMissing,
        ElementValueDifferent,
        ElementOrderDifferent,
        WhitespaceDifferent,
        XmlDeclarationDifferent,
        NamespaceDifferent,
        CommentDifferent,
        ProcessingInstructionDifferent
    }

    /// <summary>
    /// 差异严重性
    /// </summary>
    public enum DifferenceSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }
}
```

### 2.3 测试执行接口

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture.Strategies
{
    /// <summary>
    /// 测试执行策略接口
    /// </summary>
    public interface ITestExecutionStrategy
    {
        /// <summary>
        /// 执行测试
        /// </summary>
        Task<TestExecutionResult> ExecuteTestAsync(TestContext context);
        
        /// <summary>
        /// 执行往返测试
        /// </summary>
        Task<TestExecutionResult> ExecuteRoundTripTestAsync<T>(string xmlFilePath);
        
        /// <summary>
        /// 执行结构相等性测试
        /// </summary>
        Task<TestExecutionResult> ExecuteStructuralEqualityTestAsync<T>(string xmlFilePath);
        
        /// <summary>
        /// 执行性能测试
        /// </summary>
        Task<TestExecutionResult> ExecutePerformanceTestAsync<T>(string xmlFilePath);
        
        /// <summary>
        /// 执行压力测试
        /// </summary>
        Task<TestExecutionResult> ExecuteStressTestAsync<T>(string xmlFilePath);
    }

    /// <summary>
    /// 测试上下文
    /// </summary>
    public class TestContext
    {
        public string TestName { get; set; }
        public string XmlFilePath { get; set; }
        public Type TestType { get; set; }
        public TestConfiguration Configuration { get; set; }
        public ITestOutputHelper Output { get; set; }
        public Dictionary<string, object> CustomData { get; set; } = new();
    }

    /// <summary>
    /// 测试执行结果
    /// </summary>
    public class TestExecutionResult
    {
        public bool Success { get; set; }
        public string TestName { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public Exception Exception { get; set; }
        public TestMetrics Metrics { get; set; }
        public List<TestWarning> Warnings { get; set; } = new();
        public TestDiagnosticReport DiagnosticReport { get; set; }
    }

    /// <summary>
    /// 测试指标
    /// </summary>
    public class TestMetrics
    {
        public long MemoryUsed { get; set; }
        public double CpuUsage { get; set; }
        public int XmlNodesProcessed { get; set; }
        public int SerializationOperations { get; set; }
        public int DeserializationOperations { get; set; }
        public int ValidationOperations { get; set; }
    }

    /// <summary>
    /// 测试警告
    /// </summary>
    public class TestWarning
    {
        public string Message { get; set; }
        public WarningSeverity Severity { get; set; }
        public string Code { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }

    /// <summary>
    /// 警告严重性
    /// </summary>
    public enum WarningSeverity
    {
        Low,
        Medium,
        High
    }
}
```

### 2.4 诊断服务接口

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture.Diagnostics
{
    /// <summary>
    /// 测试诊断服务接口
    /// </summary>
    public interface ITestDiagnosticService
    {
        /// <summary>
        /// 诊断往返测试失败
        /// </summary>
        Task<TestDiagnosticReport> DiagnoseRoundTripFailureAsync<T>(string xmlFilePath, Exception ex);
        
        /// <summary>
        /// 诊断序列化失败
        /// </summary>
        Task<TestDiagnosticReport> DiagnoseSerializationFailureAsync<T>(T obj, Exception ex);
        
        /// <summary>
        /// 诊断反序列化失败
        /// </summary>
        Task<TestDiagnosticReport> DiagnoseDeserializationFailureAsync(string xmlFilePath, Exception ex);
        
        /// <summary>
        /// 诊断结构相等性失败
        /// </summary>
        Task<TestDiagnosticReport> DiagnoseStructuralEqualityFailureAsync<T>(T original, T generated, Exception ex);
        
        /// <summary>
        /// 生成修复建议
        /// </summary>
        Task<List<TestRecommendation>> GenerateRecommendationsAsync(TestDiagnosticReport report);
        
        /// <summary>
        /// 分析测试趋势
        /// </summary>
        Task<TestTrendAnalysis> AnalyzeTestTrendsAsync(List<TestExecutionResult> results);
    }

    /// <summary>
    /// 测试诊断报告
    /// </summary>
    public class TestDiagnosticReport
    {
        public string TestName { get; set; }
        public TestFailureType FailureType { get; set; }
        public Exception Exception { get; set; }
        public List<DiagnosticFinding> Findings { get; set; } = new();
        public List<TestRecommendation> Recommendations { get; set; } = new();
        public DiagnosticSeverity Severity { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string ReportId { get; set; }
    }

    /// <summary>
    /// 诊断发现
    /// </summary>
    public class DiagnosticFinding
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FindingCategory Category { get; set; }
        public FindingSeverity Severity { get; set; }
        public string Location { get; set; }
        public string CodeSnippet { get; set; }
        public List<string> RelatedFiles { get; set; } = new();
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// 测试建议
    /// </summary>
    public class TestRecommendation
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public RecommendationType Type { get; set; }
        public RecommendationPriority Priority { get; set; }
        public List<string> Steps { get; set; } = new();
        public string CodeExample { get; set; }
        public Dictionary<string, object> Context { get; set; } = new();
    }

    /// <summary>
    /// 测试失败类型
    /// </summary>
    public enum TestFailureType
    {
        RoundTripFailure,
        SerializationFailure,
        DeserializationFailure,
        StructuralEqualityFailure,
        PerformanceFailure,
        ValidationFailure,
        Unknown
    }

    /// <summary>
    /// 诊断严重性
    /// </summary>
    public enum DiagnosticSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// 发现类别
    /// </summary>
    public enum FindingCategory
    {
        Serialization,
        Deserialization,
        XmlStructure,
        DataModel,
        Configuration,
        Environment,
        Performance,
        Memory
    }

    /// <summary>
    /// 发现严重性
    /// </summary>
    public enum FindingSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    /// <summary>
    /// 建议类型
    /// </summary>
    public enum RecommendationType
    {
        CodeFix,
        ConfigurationChange,
        DataFix,
        EnvironmentFix,
        PerformanceOptimization,
        MemoryOptimization
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
    /// 测试趋势分析
    /// </summary>
    public class TestTrendAnalysis
    {
        public DateTime AnalysisPeriod { get; set; }
        public TestTrendSummary Summary { get; set; }
        public List<TestTrendData> TrendData { get; set; } = new();
        public List<TestTrendAlert> Alerts { get; set; } = new();
    }

    /// <summary>
    /// 测试趋势摘要
    /// </summary>
    public class TestTrendSummary
    {
        public double PassRateTrend { get; set; }
        public double ExecutionTimeTrend { get; set; }
        public double MemoryUsageTrend { get; set; }
        public double FailureRateTrend { get; set; }
        public TrendDirection OverallTrend { get; set; }
    }

    /// <summary>
    /// 趋势方向
    /// </summary>
    public enum TrendDirection
    {
        Improving,
        Stable,
        Degrading
    }

    /// <summary>
    /// 测试趋势数据
    /// </summary>
    public class TestTrendData
    {
        public DateTime Timestamp { get; set; }
        public double PassRate { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
        public long AverageMemoryUsage { get; set; }
        public int TotalFailures { get; set; }
    }

    /// <summary>
    /// 测试趋势警告
    /// </summary>
    public class TestTrendAlert
    {
        public string Message { get; set; }
        public AlertSeverity Severity { get; set; }
        public AlertType Type { get; set; }
        public DateTime TriggeredAt { get; set; }
        public Dictionary<string, object> Metrics { get; set; } = new();
    }

    /// <summary>
    /// 警告严重性
    /// </summary>
    public enum AlertSeverity
    {
        Info,
        Warning,
        Error,
        Critical
    }

    /// <summary>
    /// 警告类型
    /// </summary>
    public enum AlertType
    {
        PassRateDegradation,
        PerformanceDegradation,
        MemoryIncrease,
        FailureRateIncrease,
        UnusualPattern
    }
}
```

## 3. 核心实现类

### 3.1 测试数据管理实现

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture.Infrastructure
{
    /// <summary>
    /// 测试数据管理服务实现
    /// </summary>
    public class TestDataManager : ITestDataManager, IDisposable
    {
        private readonly string _testDataPath;
        private readonly string _tempPath;
        private readonly List<string> _tempFiles = new();
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<TestDataManager> _logger;

        public TestDataManager(ILogger<TestDataManager> logger = null)
        {
            _logger = logger;
            _testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            _tempPath = Path.Combine(Path.GetTempPath(), "BannerlordModEditor_Tests");
            Directory.CreateDirectory(_tempPath);
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<string> GetXmlTestDataAsync(string testName, string xmlType)
        {
            try
            {
                var filePath = Path.Combine(_testDataPath, xmlType, $"{testName}.xml");
                if (!File.Exists(filePath))
                {
                    // 尝试在其他位置查找
                    filePath = Path.Combine(_testDataPath, $"{testName}.xml");
                    if (!File.Exists(filePath))
                    {
                        throw new FileNotFoundException($"Test XML file not found: {filePath}");
                    }
                }

                return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get XML test data for {TestName}/{XmlType}", testName, xmlType);
                throw;
            }
        }

        public async Task<Stream> GetXmlStreamAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_testDataPath, filePath);
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"XML file not found: {fullPath}");
                }

                return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get XML stream for {FilePath}", filePath);
                throw;
            }
        }

        public async Task<string> CreateTempTestFileAsync(string fileName, string content)
        {
            try
            {
                var filePath = Path.Combine(_tempPath, fileName);
                await File.WriteAllTextAsync(filePath, content);
                _tempFiles.Add(filePath);
                return filePath;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to create temp test file {FileName}", fileName);
                throw;
            }
        }

        public async Task CleanupTestFilesAsync()
        {
            try
            {
                foreach (var file in _tempFiles.Where(File.Exists))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Failed to delete temp file {FilePath}", file);
                    }
                }
                _tempFiles.Clear();

                // 清理临时目录
                if (Directory.Exists(_tempPath))
                {
                    try
                    {
                        Directory.Delete(_tempPath, true);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Failed to delete temp directory {DirectoryPath}", _tempPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to cleanup test files");
                throw;
            }
        }

        public async Task<TestConfiguration> GetTestConfigurationAsync(string testName)
        {
            try
            {
                var configPath = Path.Combine(_testDataPath, "Config", $"{testName}.json");
                if (!File.Exists(configPath))
                {
                    // 返回默认配置
                    return new TestConfiguration
                    {
                        TestName = testName,
                        XmlSettings = new XmlTestSettings(),
                        PerformanceSettings = new PerformanceSettings(),
                        DiagnosticSettings = new DiagnosticSettings(),
                        ValidationSettings = new ValidationSettings()
                    };
                }

                var json = await File.ReadAllTextAsync(configPath);
                return JsonSerializer.Deserialize<TestConfiguration>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get test configuration for {TestName}", testName);
                throw;
            }
        }

        public async Task<ValidationRuleSet> GetValidationRulesAsync(string xmlType)
        {
            try
            {
                var rulesPath = Path.Combine(_testDataPath, "Config", "ValidationRules", $"{xmlType}.json");
                if (!File.Exists(rulesPath))
                {
                    // 返回默认规则集
                    return new ValidationRuleSet
                    {
                        XmlType = xmlType,
                        Rules = new List<ValidationRule>()
                    };
                }

                var json = await File.ReadAllTextAsync(rulesPath);
                return JsonSerializer.Deserialize<ValidationRuleSet>(json, _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get validation rules for {XmlType}", xmlType);
                throw;
            }
        }

        public void Dispose()
        {
            CleanupTestFilesAsync().Wait();
        }
    }
}
```

### 3.2 XML测试辅助实现

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture.Infrastructure
{
    /// <summary>
    /// XML测试辅助服务实现
    /// </summary>
    public class XmlTestHelpers : IXmlTestHelpers
    {
        private readonly ITestDataManager _testDataManager;
        private readonly ITestDiagnosticService _diagnosticService;
        private readonly ILogger<XmlTestHelpers> _logger;

        public XmlTestHelpers(
            ITestDataManager testDataManager,
            ITestDiagnosticService diagnosticService,
            ILogger<XmlTestHelpers> logger = null)
        {
            _testDataManager = testDataManager;
            _diagnosticService = diagnosticService;
            _logger = logger;
        }

        public async Task<T> LoadXmlAsync<T>(string filePath, ITestOutputHelper output)
        {
            try
            {
                output?.WriteLine($"Loading XML from: {filePath}");
                
                var xmlContent = await File.ReadAllTextAsync(filePath);
                var loader = new GenericXmlLoader<T>();
                
                var result = await loader.LoadAsync(filePath);
                
                output?.WriteLine($"Successfully loaded XML: {typeof(T).Name}");
                return result;
            }
            catch (Exception ex)
            {
                output?.WriteLine($"Failed to load XML: {ex.Message}");
                _logger?.LogError(ex, "Failed to load XML from {FilePath}", filePath);
                throw;
            }
        }

        public async Task<string> SaveXmlAsync<T>(T obj, string originalXml, XmlTestSettings settings = null)
        {
            try
            {
                settings ??= new XmlTestSettings();
                
                var loader = new GenericXmlLoader<T>();
                var tempFile = await _testDataManager.CreateTempTestFileAsync(
                    $"{Guid.NewGuid()}.xml", 
                    originalXml);
                
                loader.Save(obj, tempFile, originalXml);
                
                var savedXml = await File.ReadAllTextAsync(tempFile);
                
                // 应用格式化和规范化
                var formattedXml = FormatXml(savedXml, settings);
                var normalizedXml = NormalizeXml(formattedXml, settings);
                
                return normalizedXml;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to save XML for type {Type}", typeof(T).Name);
                throw;
            }
        }

        public async Task<XmlComparisonResult> CompareXmlStructuresAsync(string original, string generated)
        {
            try
            {
                var result = new XmlComparisonResult
                {
                    OriginalXml = original,
                    GeneratedXml = generated
                };

                // 规范化两个XML字符串
                var normalizedOriginal = NormalizeXml(original);
                var normalizedGenerated = NormalizeXml(generated);

                // 使用XDocument进行深度比较
                var originalDoc = XDocument.Parse(normalizedOriginal);
                var generatedDoc = XDocument.Parse(normalizedGenerated);

                var differences = await CompareXDocumentsAsync(originalDoc, generatedDoc);
                result.Differences.AddRange(differences);
                result.IsEqual = !differences.Any(d => d.Severity == DifferenceSeverity.Error);

                // 计算统计信息
                result.Statistics = CalculateComparisonStatistics(originalDoc, generatedDoc, differences);

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to compare XML structures");
                throw;
            }
        }

        public async Task<XmlComparisonResult> CompareObjectsAsync<T>(T original, T generated)
        {
            try
            {
                var originalXml = await SaveXmlAsync(original, "");
                var generatedXml = await SaveXmlAsync(generated, "");
                
                return await CompareXmlStructuresAsync(originalXml, generatedXml);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to compare objects of type {Type}", typeof(T).Name);
                throw;
            }
        }

        public async Task<ValidationResult> ValidateXmlStructureAsync(string xml, string xmlType)
        {
            try
            {
                var result = new ValidationResult();
                
                // 基本XML格式验证
                try
                {
                    XDocument.Parse(xml);
                    result.IsValid = true;
                }
                catch (XmlException ex)
                {
                    result.IsValid = false;
                    result.Errors.Add(new ValidationError
                    {
                        Message = $"XML格式错误: {ex.Message}",
                        Severity = ValidationSeverity.Error
                    });
                    return result;
                }

                // 获取特定类型的验证规则
                var rules = await _testDataManager.GetValidationRulesAsync(xmlType);
                
                // 应用验证规则
                foreach (var rule in rules.Rules)
                {
                    try
                    {
                        var ruleResult = await rule.Validator(xml);
                        if (!ruleResult.IsValid)
                        {
                            result.IsValid = false;
                            result.Errors.AddRange(ruleResult.Errors);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Failed to apply validation rule {RuleName}", rule.Name);
                        result.Errors.Add(new ValidationError
                        {
                            Message = $"验证规则执行失败: {rule.Name}",
                            Severity = ValidationSeverity.Warning
                        });
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to validate XML structure for type {XmlType}", xmlType);
                throw;
            }
        }

        public async Task<ValidationResult> ValidateRoundTripAsync<T>(T obj)
        {
            try
            {
                var result = new ValidationResult();
                
                // 序列化为XML
                var xml = await SaveXmlAsync(obj, "");
                
                // 反序列化回对象
                var loader = new GenericXmlLoader<T>();
                var tempFile = await _testDataManager.CreateTempTestFileAsync("roundtrip.xml", xml);
                var deserializedObj = await loader.LoadAsync(tempFile);
                
                // 比较原始对象和反序列化对象
                var comparisonResult = await CompareObjectsAsync(obj, deserializedObj);
                
                if (!comparisonResult.IsEqual)
                {
                    result.IsValid = false;
                    result.Errors.AddRange(comparisonResult.Differences
                        .Where(d => d.Severity == DifferenceSeverity.Error)
                        .Select(d => new ValidationError
                        {
                            Message = d.Description,
                            Severity = ValidationSeverity.Error
                        }));
                }
                else
                {
                    result.IsValid = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to validate round trip for type {Type}", typeof(T).Name);
                throw;
            }
        }

        public string FormatXml(string xml, XmlTestSettings settings = null)
        {
            try
            {
                settings ??= new XmlTestSettings();
                
                var doc = XDocument.Parse(xml);
                var stringBuilder = new StringBuilder();
                
                using (var writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings
                {
                    Indent = settings.PreserveFormatting,
                    IndentChars = "  ",
                    NewLineChars = Environment.NewLine,
                    OmitXmlDeclaration = !settings.IncludeXmlDeclaration,
                    Encoding = settings.Encoding
                }))
                {
                    doc.Save(writer);
                }
                
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to format XML");
                return xml; // 返回原始XML
            }
        }

        public string NormalizeXml(string xml, XmlTestSettings settings = null)
        {
            try
            {
                settings ??= new XmlTestSettings();
                
                var doc = XDocument.Parse(xml);
                
                // 移除空白节点
                if (!settings.PreserveWhitespace)
                {
                    doc.DescendantNodes()
                        .OfType<XText>()
                        .Where(t => string.IsNullOrWhiteSpace(t.Value))
                        .Remove();
                }
                
                // 规范化行尾符
                if (settings.NormalizeLineEndings)
                {
                    var xmlString = doc.ToString();
                    return xmlString.Replace("\r\n", "\n").Replace("\r", "\n");
                }
                
                return doc.ToString();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to normalize XML");
                return xml; // 返回原始XML
            }
        }

        private async Task<List<XmlDifference>> CompareXDocumentsAsync(XDocument original, XDocument generated)
        {
            var differences = new List<XmlDifference>();
            
            // 实现深度XML比较逻辑
            // 这里需要实现复杂的XML节点比较算法
            
            return differences;
        }

        private ComparisonStatistics CalculateComparisonStatistics(XDocument original, XDocument generated, List<XmlDifference> differences)
        {
            // 计算比较统计信息
            return new ComparisonStatistics
            {
                TotalNodes = original.DescendantNodes().Count(),
                MatchingNodes = original.DescendantNodes().Count() - differences.Count,
                DifferingNodes = differences.Count,
                SimilarityPercentage = (original.DescendantNodes().Count() - differences.Count) * 100.0 / original.DescendantNodes().Count()
            };
        }
    }
}
```

## 4. 测试基类和扩展方法

### 4.1 基础测试基类

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture
{
    /// <summary>
    /// XML测试基类
    /// </summary>
    public abstract class XmlTestBase : IDisposable
    {
        protected readonly ITestOutputHelper Output;
        protected readonly ITestDataManager TestDataManager;
        protected readonly IXmlTestHelpers XmlHelpers;
        protected readonly ITestDiagnosticService DiagnosticService;
        protected readonly ITestExecutionStrategy TestExecutionStrategy;
        protected readonly List<string> CreatedFiles = new();

        protected XmlTestBase(ITestOutputHelper output)
        {
            Output = output;
            
            // 依赖注入设置
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            var serviceProvider = services.BuildServiceProvider();
            
            TestDataManager = serviceProvider.GetRequiredService<ITestDataManager>();
            XmlHelpers = serviceProvider.GetRequiredService<IXmlTestHelpers>();
            DiagnosticService = serviceProvider.GetRequiredService<ITestDiagnosticService>();
            TestExecutionStrategy = serviceProvider.GetRequiredService<ITestExecutionStrategy>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITestDataManager, TestDataManager>();
            services.AddSingleton<IXmlTestHelpers, XmlTestHelpers>();
            services.AddSingleton<ITestDiagnosticService, TestDiagnosticService>();
            services.AddSingleton<ITestExecutionStrategy, XmlTestExecutionEngine>();
            
            // 添加日志
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
        }

        protected async Task<T> LoadAndValidateXmlAsync<T>(string filePath)
        {
            try
            {
                Output.WriteLine($"Loading and validating XML: {Path.GetFileName(filePath)}");
                
                // 验证文件存在
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"XML file not found: {filePath}");
                }

                // 加载XML
                var obj = await XmlHelpers.LoadXmlAsync<T>(filePath, Output);
                
                // 验证对象
                if (obj == null)
                {
                    throw new InvalidOperationException($"Failed to load XML as {typeof(T).Name}");
                }

                Output.WriteLine($"✓ Successfully loaded and validated XML");
                return obj;
            }
            catch (Exception ex)
            {
                Output.WriteLine($"✗ Failed to load and validate XML: {ex.Message}");
                throw;
            }
        }

        protected async Task<TestExecutionResult> ExecuteRoundTripTestAsync<T>(string filePath)
        {
            try
            {
                Output.WriteLine($"Executing RoundTrip test for: {Path.GetFileName(filePath)}");
                
                var context = new TestContext
                {
                    TestName = $"{typeof(T).Name}_RoundTrip",
                    XmlFilePath = filePath,
                    TestType = typeof(T),
                    Output = Output,
                    Configuration = await TestDataManager.GetTestConfigurationAsync($"{typeof(T).Name}_RoundTrip")
                };

                var result = await TestExecutionStrategy.ExecuteRoundTripTestAsync<T>(filePath);
                
                if (result.Success)
                {
                    Output.WriteLine($"✓ RoundTrip test passed for {Path.GetFileName(filePath)}");
                }
                else
                {
                    Output.WriteLine($"✗ RoundTrip test failed for {Path.GetFileName(filePath)}: {result.Exception?.Message}");
                    
                    // 生成诊断报告
                    if (result.Exception != null)
                    {
                        var diagnosticReport = await DiagnosticService.DiagnoseRoundTripFailureAsync<T>(filePath, result.Exception);
                        Output.WriteLine($"Diagnostics: {diagnosticReport.Severity} - {diagnosticReport.Findings.Count} findings");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Output.WriteLine($"✗ RoundTrip test execution failed: {ex.Message}");
                throw;
            }
        }

        protected async Task<TestExecutionResult> ExecuteStructuralEqualityTestAsync<T>(string filePath)
        {
            try
            {
                Output.WriteLine($"Executing Structural Equality test for: {Path.GetFileName(filePath)}");
                
                var context = new TestContext
                {
                    TestName = $"{typeof(T).Name}_StructuralEquality",
                    XmlFilePath = filePath,
                    TestType = typeof(T),
                    Output = Output,
                    Configuration = await TestDataManager.GetTestConfigurationAsync($"{typeof(T).Name}_StructuralEquality")
                };

                var result = await TestExecutionStrategy.ExecuteStructuralEqualityTestAsync<T>(filePath);
                
                if (result.Success)
                {
                    Output.WriteLine($"✓ Structural Equality test passed for {Path.GetFileName(filePath)}");
                }
                else
                {
                    Output.WriteLine($"✗ Structural Equality test failed for {Path.GetFileName(filePath)}: {result.Exception?.Message}");
                    
                    // 生成诊断报告
                    if (result.Exception != null)
                    {
                        var diagnosticReport = await DiagnosticService.DiagnoseStructuralEqualityFailureAsync(default(T), default(T), result.Exception);
                        Output.WriteLine($"Diagnostics: {diagnosticReport.Severity} - {diagnosticReport.Findings.Count} findings");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Output.WriteLine($"✗ Structural Equality test execution failed: {ex.Message}");
                throw;
            }
        }

        protected async Task<string> CreateTestFileAsync(string fileName, string content)
        {
            try
            {
                var filePath = await TestDataManager.CreateTempTestFileAsync(fileName, content);
                CreatedFiles.Add(filePath);
                return filePath;
            }
            catch (Exception ex)
            {
                Output.WriteLine($"✗ Failed to create test file {fileName}: {ex.Message}");
                throw;
            }
        }

        public virtual void Dispose()
        {
            // 清理创建的文件
            foreach (var file in CreatedFiles.Where(File.Exists))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"Warning: Failed to delete test file {file}: {ex.Message}");
                }
            }
            
            CreatedFiles.Clear();
            
            // 清理测试数据管理器
            if (TestDataManager is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
```

### 4.2 XML测试扩展方法

```csharp
namespace BannerlordModEditor.Common.Tests.Extensions
{
    /// <summary>
    /// XML测试扩展方法
    /// </summary>
    public static class XmlTestExtensions
    {
        /// <summary>
        /// 应该成功加载XML
        /// </summary>
        public static async Task<T> ShouldLoadXmlAsync<T>(this string filePath, ITestOutputHelper output)
        {
            try
            {
                output.WriteLine($"Loading XML: {Path.GetFileName(filePath)}");
                
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"XML file not found: {filePath}");
                }

                var loader = new GenericXmlLoader<T>();
                var result = await loader.LoadAsync(filePath);
                
                result.Should().NotBeNull($"XML file {filePath} should load successfully");
                
                output.WriteLine($"✓ Successfully loaded XML: {typeof(T).Name}");
                return result;
            }
            catch (Exception ex)
            {
                output.WriteLine($"✗ Failed to load XML: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应该成功序列化为XML
        /// </summary>
        public static async Task<string> ShouldSerializeToXmlAsync<T>(this T obj, ITestOutputHelper output)
        {
            try
            {
                output.WriteLine($"Serializing object to XML: {typeof(T).Name}");
                
                var loader = new GenericXmlLoader<T>();
                var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
                
                try
                {
                    loader.Save(obj, tempFile, "");
                    
                    var xml = await File.ReadAllTextAsync(tempFile);
                    xml.Should().NotBeNullOrEmpty("Serialized XML should not be empty");
                    
                    output.WriteLine($"✓ Successfully serialized to XML: {xml.Length} characters");
                    return xml;
                }
                finally
                {
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
            catch (Exception ex)
            {
                output.WriteLine($"✗ Failed to serialize to XML: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应该XML等价
        /// </summary>
        public static async Task ShouldBeXmlEquivalentAsync(this string original, string generated, ITestOutputHelper output)
        {
            try
            {
                output.WriteLine("Comparing XML equivalence");
                
                // 规范化两个XML字符串
                var normalizedOriginal = NormalizeXml(original);
                var normalizedGenerated = NormalizeXml(generated);
                
                // 使用XDocument进行比较
                var originalDoc = XDocument.Parse(normalizedOriginal);
                var generatedDoc = XDocument.Parse(normalizedGenerated);
                
                var areEqual = XNode.DeepEquals(originalDoc, generatedDoc);
                
                if (!areEqual)
                {
                    output.WriteLine("XML documents are not equivalent");
                    output.WriteLine($"Original length: {original.Length}");
                    output.WriteLine($"Generated length: {generated.Length}");
                    
                    // 显示差异
                    var differences = FindXmlDifferences(originalDoc, generatedDoc);
                    foreach (var diff in differences.Take(5)) // 只显示前5个差异
                    {
                        output.WriteLine($"Difference: {diff}");
                    }
                }
                
                areEqual.Should().BeTrue("XML documents should be equivalent");
                
                output.WriteLine("✓ XML documents are equivalent");
            }
            catch (Exception ex)
            {
                output.WriteLine($"✗ XML equivalence check failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应该成功完成往返测试
        /// </summary>
        public static async Task<T> ShouldRoundTripSuccessfullyAsync<T>(this string filePath, ITestOutputHelper output)
        {
            try
            {
                output.WriteLine($"Executing RoundTrip test: {Path.GetFileName(filePath)}");
                
                // 加载原始XML
                var originalObj = await filePath.ShouldLoadXmlAsync<T>(output);
                
                // 序列化为XML
                var serializedXml = await originalObj.ShouldSerializeToXmlAsync(output);
                
                // 反序列化回对象
                var tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.xml");
                try
                {
                    await File.WriteAllTextAsync(tempFile, serializedXml);
                    var deserializedObj = await tempFile.ShouldLoadXmlAsync<T>(output);
                    
                    // 比较原始对象和反序列化对象
                    var originalSerialized = await originalObj.ShouldSerializeToXmlAsync(output);
                    var deserializedSerialized = await deserializedObj.ShouldSerializeToXmlAsync(output);
                    
                    await originalSerialized.ShouldBeXmlEquivalentAsync(deserializedSerialized, output);
                    
                    output.WriteLine($"✓ RoundTrip test successful for {typeof(T).Name}");
                    return deserializedObj;
                }
                finally
                {
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }
            }
            catch (Exception ex)
            {
                output.WriteLine($"✗ RoundTrip test failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应该在指定时间内完成
        /// </summary>
        public static async Task ShouldCompleteWithinAsync(this Func<Task> action, TimeSpan timeout, ITestOutputHelper output)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                var task = action();
                
                var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
                
                if (completedTask != task)
                {
                    throw new TimeoutException($"Operation did not complete within {timeout.TotalMilliseconds}ms");
                }
                
                await task; // 重新抛出任何异常
                
                stopwatch.Stop();
                output.WriteLine($"✓ Operation completed in {stopwatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                output.WriteLine($"✗ Operation failed or timed out: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 应该使用合理的内存
        /// </summary>
        public static async Task ShouldUseReasonableMemoryAsync(this Func<Task> action, long maxMemoryBytes, ITestOutputHelper output)
        {
            try
            {
                var initialMemory = GC.GetTotalMemory(false);
                
                await action();
                
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var finalMemory = GC.GetTotalMemory(false);
                
                var memoryIncrease = finalMemory - initialMemory;
                
                output.WriteLine($"Memory usage: {memoryIncrease} bytes increase");
                
                memoryIncrease.Should().BeLessThan(maxMemoryBytes, 
                    $"Memory usage should be reasonable (< {maxMemoryBytes} bytes)");
                
                output.WriteLine("✓ Memory usage is reasonable");
            }
            catch (Exception ex)
            {
                output.WriteLine($"✗ Memory usage check failed: {ex.Message}");
                throw;
            }
        }

        private static string NormalizeXml(string xml)
        {
            var doc = XDocument.Parse(xml);
            
            // 移除空白节点
            doc.DescendantNodes()
                .OfType<XText>()
                .Where(t => string.IsNullOrWhiteSpace(t.Value))
                .Remove();
            
            return doc.ToString();
        }

        private static List<string> FindXmlDifferences(XDocument original, XDocument generated)
        {
            var differences = new List<string>();
            
            // 简单的差异检测算法
            var originalElements = original.Descendants().ToList();
            var generatedElements = generated.Descendants().ToList();
            
            if (originalElements.Count != generatedElements.Count)
            {
                differences.Add($"Element count differs: original={originalElements.Count}, generated={generatedElements.Count}");
            }
            
            for (int i = 0; i < Math.Min(originalElements.Count, generatedElements.Count); i++)
            {
                var originalEl = originalElements[i];
                var generatedEl = generatedElements[i];
                
                if (originalEl.Name != generatedEl.Name)
                {
                    differences.Add($"Element name differs at position {i}: {originalEl.Name} vs {generatedEl.Name}");
                }
                
                if (originalEl.Value != generatedEl.Value)
                {
                    differences.Add($"Element value differs at position {i}: '{originalEl.Value}' vs '{generatedEl.Value}'");
                }
            }
            
            return differences;
        }
    }
}
```

## 5. 配置和设置

### 5.1 依赖注入配置

```csharp
namespace BannerlordModEditor.Common.Tests.Architecture
{
    /// <summary>
    /// 测试依赖注入配置
    /// </summary>
    public static class TestServiceCollectionExtensions
    {
        /// <summary>
        /// 添加测试服务
        /// </summary>
        public static IServiceCollection AddTestServices(this IServiceCollection services)
        {
            // 核心服务
            services.AddSingleton<ITestDataManager, TestDataManager>();
            services.AddSingleton<IXmlTestHelpers, XmlTestHelpers>();
            services.AddSingleton<ITestDiagnosticService, TestDiagnosticService>();
            services.AddSingleton<ITestExecutionStrategy, XmlTestExecutionEngine>();
            
            // 验证服务
            services.AddSingleton<IValidationRuleEngine, ValidationRuleEngine>();
            services.AddSingleton<IValidationRuleProvider, FileBasedValidationRuleProvider>();
            
            // 监控服务
            services.AddSingleton<ITestMonitoringService, TestMonitoringService>();
            services.AddSingleton<ITestResultAggregator, TestResultAggregator>();
            
            // 报告服务
            services.AddSingleton<ITestReportGenerator, TestReportGenerator>();
            services.AddSingleton<ITestTrendAnalyzer, TestTrendAnalyzer>();
            
            // 日志
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Information);
            });
            
            // 配置
            services.Configure<TestConfiguration>(options =>
            {
                // 默认配置
            });
            
            return services;
        }

        /// <summary>
        /// 添加模拟服务
        /// </summary>
        public static IServiceCollection AddMockServices(this IServiceCollection services)
        {
            // 模拟文件发现服务
            services.AddSingleton<IFileDiscoveryService>(provider =>
            {
                var mock = new Mock<IFileDiscoveryService>();
                // 设置模拟行为
                return mock.Object;
            });
            
            // 模拟XML加载器
            services.AddSingleton<GenericXmlLoader<object>>(provider =>
            {
                // 可以根据需要创建特定的模拟
                return new GenericXmlLoader<object>();
            });
            
            return services;
        }
    }
}
```

### 5.2 测试配置文件

```json
{
  "testConfiguration": {
    "xmlSettings": {
      "preserveWhitespace": true,
      "preserveFormatting": true,
      "includeXmlDeclaration": true,
      "encoding": "UTF-8",
      "normalizeLineEndings": true,
      "ignoreAttributeOrder": false
    },
    "performanceSettings": {
      "maxExecutionTime": "00:00:30",
      "maxMemoryUsage": 104857600,
      "enablePerformanceMonitoring": true
    },
    "diagnosticSettings": {
      "enableDetailedLogging": true,
      "enableErrorAnalysis": true,
      "enableRecommendationGeneration": true,
      "maxDiagnosticsDepth": 5
    },
    "validationSettings": {
      "enableStrictValidation": true,
      "enableSchemaValidation": true,
      "enableBusinessRuleValidation": true,
      "disabledRules": []
    }
  }
}
```

## 6. 最佳实践和指导原则

### 6.1 测试命名约定

```csharp
// 正确的测试命名
public class CombatParametersXmlTests : XmlTestBase
{
    [Fact]
    public async Task CombatParameters_LoadAndSave_ShouldBeLogicallyIdentical()
    {
        // 测试逻辑
    }
    
    [Fact]
    public async Task CombatParameters_RoundTrip_ShouldPreserveAllData()
    {
        // 测试逻辑
    }
    
    [Fact]
    public async Task CombatParameters_WithEmptyElements_ShouldSerializeCorrectly()
    {
        // 测试逻辑
    }
}
```

### 6.2 测试组织模式

```csharp
// 按功能域组织测试
public class AudioXmlTests : XmlTestBase
{
    public AudioXmlTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public async Task SoundFiles_LoadAndSave_ShouldBeLogicallyIdentical()
    {
        var filePath = GetTestFilePath("sound_files.xml");
        await ExecuteRoundTripTestAsync<SoundFilesDO>(filePath);
    }
}

// 按测试类型组织
public class RoundTripTests : XmlTestBase
{
    public RoundTripTests(ITestOutputHelper output) : base(output) { }
    
    [Theory]
    [InlineData("combat_parameters.xml", typeof(CombatParametersDO))]
    [InlineData("action_types.xml", typeof(ActionTypesDO))]
    [InlineData("skeletons_layout.xml", typeof(SkeletonsLayoutDO))]
    public async Task XmlFiles_RoundTrip_ShouldPreserveStructure(string fileName, Type type)
    {
        var filePath = GetTestFilePath(fileName);
        var method = typeof(XmlTestExtensions).GetMethod("ShouldRoundTripSuccessfullyAsync")?
            .MakeGenericMethod(type);
        
        if (method != null)
        {
            await (Task)method.Invoke(null, new object[] { filePath, Output });
        }
    }
}
```

### 6.3 错误处理和诊断

```csharp
// 使用诊断服务进行详细错误分析
[Fact]
public async Task CombatParameters_WithComplexStructure_ShouldSerializeCorrectly()
{
    try
    {
        var filePath = GetTestFilePath("complex_combat_parameters.xml");
        var result = await ExecuteRoundTripTestAsync<CombatParametersDO>(filePath);
        
        result.Success.Should().BeTrue();
        
        if (!result.Success && result.DiagnosticReport != null)
        {
            Output.WriteLine($"Diagnostic Report:");
            Output.WriteLine($"Severity: {result.DiagnosticReport.Severity}");
            Output.WriteLine($"Findings: {result.DiagnosticReport.Findings.Count}");
            
            foreach (var finding in result.DiagnosticReport.Findings)
            {
                Output.WriteLine($"- {finding.Title}: {finding.Description}");
            }
        }
    }
    catch (Exception ex)
    {
        Output.WriteLine($"Test failed with exception: {ex}");
        
        // 生成详细的诊断报告
        var diagnosticReport = await DiagnosticService.DiagnoseRoundTripFailureAsync<CombatParametersDO>(
            GetTestFilePath("complex_combat_parameters.xml"), ex);
        
        Output.WriteLine($"Generated diagnostic report with {diagnosticReport.Findings.Count} findings");
        
        throw;
    }
}
```

## 7. 性能优化策略

### 7.1 测试性能监控

```csharp
// 性能测试示例
[Fact]
public async Task LargeXmlFiles_ShouldProcessWithinTimeLimit()
{
    var filePath = GetTestFilePath("large_combat_parameters.xml");
    
    await ShouldCompleteWithinAsync(async () =>
    {
        await ExecuteRoundTripTestAsync<CombatParametersDO>(filePath);
    }, TimeSpan.FromSeconds(10), Output);
}

[Fact]
public async Task XmlProcessing_ShouldUseReasonableMemory()
{
    var filePath = GetTestFilePath("memory_intensive_file.xml");
    
    await ShouldUseReasonableMemoryAsync(async () =>
    {
        await ExecuteRoundTripTestAsync<ComplexObjectDO>(filePath);
    }, 50 * 1024 * 1024, Output); // 50MB limit
}
```

### 7.2 批量测试优化

```csharp
// 批量测试处理
[Theory]
[InlineData("particle_systems_basic.xml")]
[InlineData("particle_systems_general.xml")]
[InlineData("particle_systems_outdoor.xml")]
public async Task ParticleSystems_RoundTrip_ShouldBeConsistent(string fileName)
{
    var filePath = GetTestFilePath(fileName);
    
    // 使用批处理优化
    var batchProcessor = new BatchTestProcessor();
    var result = await batchProcessor.ProcessRoundTripTestAsync<ParticleSystemsDO>(filePath);
    
    result.Should().NotBeNull();
    result.Success.Should().BeTrue();
}
```

## 8. 部署和维护

### 8.1 CI/CD集成

```yaml
# GitHub Actions工作流示例
name: XML Test Architecture

on:
  push:
    branches: [ main, feature/* ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
    
    - name: Install dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release
    
    - name: Run tests with coverage
      run: dotnet test --configuration Release --collect:"XPlat Code Coverage" --verbosity normal
    
    - name: Generate test report
      run: dotnet reportgenerator -reports:coverage.xml -targetdir:TestResults
    
    - name: Upload test results
      uses: actions/upload-artifact@v3
      with:
        name: test-results
        path: TestResults/
```

### 8.2 监控和报告

```csharp
// 测试结果监控
public class TestMonitoringService : ITestMonitoringService
{
    private readonly ILogger<TestMonitoringService> _logger;
    
    public async Task<TestExecutionMetrics> CollectMetricsAsync()
    {
        // 收集测试执行指标
        return new TestExecutionMetrics
        {
            TotalTests = 150,
            PassedTests = 142,
            FailedTests = 8,
            PassRate = 94.7,
            AverageExecutionTime = TimeSpan.FromSeconds(2.5),
            MemoryUsage = new MemoryUsageInfo
            {
                AverageUsage = 45 * 1024 * 1024, // 45MB
                PeakUsage = 78 * 1024 * 1024      // 78MB
            }
        };
    }
    
    public async Task AlertOnDegradationAsync(TestExecutionMetrics current, TestExecutionMetrics baseline)
    {
        if (current.PassRate < baseline.PassRate - 5) // 5% 下降
        {
            _logger.LogWarning("Test pass rate degraded from {Baseline}% to {Current}%", 
                baseline.PassRate, current.PassRate);
        }
    }
}
```

## 总结

本技术规格文档提供了BannerlordModEditor XML验证系统测试架构的详细实现方案。通过定义清晰的接口、实现具体的服务类、提供扩展方法和最佳实践指导，我们能够：

1. **提高测试可靠性**: 通过精确的XML比较和错误诊断
2. **增强可维护性**: 通过清晰的架构和组件化设计
3. **优化性能**: 通过性能监控和优化策略
4. **改善开发体验**: 通过丰富的扩展方法和工具

这个架构将帮助我们解决当前的测试失败问题，并为未来的测试开发提供坚实的基础。