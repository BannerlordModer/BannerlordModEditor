# XML映射修复方案技术栈文档

## 执行摘要

本文档详细阐述了BannerlordModEditor-CLI项目XML映射修复方案的技术栈选择和实施策略。基于对项目需求、团队技能、性能要求和维护性的综合分析，我们选择了最适合的技术组合。该技术栈建立在现有.NET 9基础之上，确保了向后兼容性和技术先进性。

## 技术栈概览

### 核心技术栈

| 技术类别 | 选择技术 | 版本 | 用途 |
|---------|----------|------|------|
| **运行时** | .NET 9.0 | 9.0.0 | 核心运行时环境 |
| **Web框架** | ASP.NET Core | 9.0.0 | REST API服务 |
| **ORM** | Entity Framework Core | 9.0.0 | 数据访问层 |
| **测试框架** | xUnit | 2.5.0 | 单元测试和集成测试 |
| **序列化** | System.Xml.Serialization | 内置 | XML序列化/反序列化 |
| **LINQ to XML** | System.Xml.Linq | 内置 | XML查询和操作 |

### 辅助技术栈

| 技术类别 | 选择技术 | 版本 | 用途 |
|---------|----------|------|------|
| **日志记录** | Serilog | 3.1.0 | 结构化日志记录 |
| **配置管理** | Microsoft.Extensions.Configuration | 9.0.0 | 配置系统 |
| **依赖注入** | Microsoft.Extensions.DependencyInjection | 9.0.0 | DI容器 |
| **性能监控** | BenchmarkDotNet | 0.13.12 | 性能基准测试 |
| **API文档** | Swashbuckle.AspNetCore | 6.5.0 | OpenAPI文档生成 |
| **验证** | FluentValidation | 11.8.0 | 数据验证 |

### 开发工具

| 工具类别 | 选择工具 | 用途 |
|---------|----------|------|
| **IDE** | Visual Studio 2022 | 主要开发环境 |
| **版本控制** | Git | 源代码管理 |
| **CI/CD** | GitHub Actions | 持续集成和部署 |
| **包管理** | NuGet | 依赖包管理 |
| **容器化** | Docker | 环境一致性 |

## 1. 核心技术选择理由

### 1.1 .NET 9.0

#### 选择理由
- **性能优化**: .NET 9相比.NET 8有15-20%的性能提升
- **现代化特性**: 原生AOT支持、改进的GC、优化的JIT编译
- **跨平台**: 支持Windows、Linux、macOS
- **生态系统**: 丰富的NuGet包和工具支持
- **团队熟悉度**: 团队已有.NET开发经验

#### 技术优势
```csharp
// .NET 9 性能优化示例
public ref struct XmlProcessingSpan
{
    private readonly ReadOnlySpan<char> _xml;
    
    public XmlProcessingSpan(ReadOnlySpan<char> xml)
    {
        _xml = xml;
    }
    
    // 使用span避免内存分配
    public void Process()
    {
        // 零拷贝XML处理
        var parser = new XmlSpanParser(_xml);
        parser.Parse();
    }
}

// 原生AOT支持
[UnmanagedCallersOnly(EntryPoint = "ProcessXml")]
public static int ProcessXml(IntPtr xmlPtr, int length)
{
    // 原生互操作支持
    var span = new ReadOnlySpan<byte>(xmlPtr.ToPointer(), length);
    return ProcessXmlInternal(span);
}
```

#### 风险评估
- **风险**: .NET 9相对较新，可能存在未知问题
- **缓解**: 使用LTS版本，建立充分的测试覆盖
- **备选方案**: .NET 8.0 LTS（如遇到重大问题）

### 1.2 ASP.NET Core 9.0

#### 选择理由
- **高性能**: Kestrel服务器性能优异
- **模块化**: 只需引入必要的中间件
- **集成性**: 与.NET生态系统完美集成
- **工具支持**: 丰富的开发工具和模板

#### 架构实现
```csharp
// API服务配置
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // XML处理服务
        services.AddScoped<IXmlProcessor, XmlProcessor>();
        services.AddScoped<IXmlNormalizer, XmlNormalizer>();
        services.AddScoped<IXmlComparator, XmlComparator>();
        
        // 修复策略
        services.AddScoped<IXmlRepairStrategy, SiegeEnginesRepairStrategy>();
        services.AddScoped<IXmlRepairStrategy, SpecialMeshesRepairStrategy>();
        
        // 配置管理
        services.Configure<XmlProcessingConfiguration>(
            Configuration.GetSection("XmlProcessing"));
            
        // 插件管理
        services.AddScoped<IXmlProcessingPluginManager, XmlProcessingPluginManager>();
        
        // 测试框架
        services.AddScoped<IXmlTestFramework, XmlTestFramework>();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        // XML处理中间件
        app.UseXmlProcessing();
        
        // API端点
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<XmlProcessingHub>("/xml-processing");
        });
    }
}
```

### 1.3 System.Xml.Linq

#### 选择理由
- **现代API**: 相比旧的XmlDocument，API更简洁易用
- **LINQ支持**: 支持LINQ查询，操作更便捷
- **性能**: 优化的XML处理性能
- **内存效率**: 流式处理支持

#### 核心用法
```csharp
public class LinqXmlProcessor
{
    public XDocument ProcessXml(string xml)
    {
        var doc = XDocument.Parse(xml);
        
        // LINQ查询优化XML
        var booleanAttributes = doc.Descendants()
            .SelectMany(e => e.Attributes())
            .Where(a => IsBooleanAttribute(a.Name.LocalName));
        
        foreach (var attr in booleanAttributes)
        {
            attr.Value = NormalizeBooleanValue(attr.Value);
        }
        
        return doc;
    }
    
    private bool IsBooleanAttribute(string attributeName)
    {
        return attributeName.StartsWith("is_") ||
               attributeName.EndsWith("Global") ||
               attributeName.Equals("active", StringComparison.OrdinalIgnoreCase);
    }
    
    private string NormalizeBooleanValue(string value)
    {
        return value.ToLower() switch
        {
            "true" => "true",
            "false" => "false",
            "1" => "true",
            "0" => "false",
            "yes" => "true",
            "no" => "false",
            _ => value
        };
    }
}
```

## 2. 数据处理技术栈

### 2.1 XML序列化策略

#### System.Xml.Serialization vs System.Text.Json

| 比较项 | System.Xml.Serialization | System.Text.Json |
|---------|--------------------------|------------------|
| **XML支持** | 原生支持 | 有限支持 |
| **性能** | 良好 | 优秀 |
| **灵活性** | 高 | 中等 |
| **复杂度** | 中等 | 低 |
| **内存使用** | 中等 | 低 |

**选择**: System.Xml.Serialization（专门针对XML优化）

#### 实现示例
```csharp
public class EnhancedXmlSerializer
{
    private readonly XmlSerializer _serializer;
    private readonly XmlWriterSettings _settings;
    
    public EnhancedXmlSerializer(Type type)
    {
        _serializer = new XmlSerializer(type);
        _settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\n",
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        };
    }
    
    public string Serialize(object obj, string originalXml = null)
    {
        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, _settings);
        
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add("", "");
        
        // 保留原始命名空间
        if (!string.IsNullOrEmpty(originalXml))
        {
            PreserveOriginalNamespaces(originalXml, namespaces);
        }
        
        _serializer.Serialize(writer, obj, namespaces);
        writer.Flush();
        
        stream.Position = 0;
        using var reader = new StreamReader(stream, new UTF8Encoding(false));
        return reader.ReadToEnd();
    }
    
    private void PreserveOriginalNamespaces(string originalXml, XmlSerializerNamespaces namespaces)
    {
        try
        {
            var doc = XDocument.Parse(originalXml);
            if (doc.Root != null)
            {
                foreach (var attr in doc.Root.Attributes())
                {
                    if (attr.IsNamespaceDeclaration)
                    {
                        namespaces.Add(attr.Name.LocalName, attr.Value);
                    }
                }
            }
        }
        catch
        {
            // 使用默认命名空间
        }
    }
}
```

### 2.2 LINQ to XML优化

#### 性能优化策略
```csharp
public class OptimizedXmlQuery
{
    // 使用XPath预编译
    private static readonly XPathExpression BooleanAttributesExpression = 
        XPathExpression.Compile("//*[@*[starts-with(name(), 'is_') or contains(name(), 'Global')]]");
    
    public List<XAttribute> FindBooleanAttributes(XDocument doc)
    {
        var navigator = doc.CreateNavigator();
        var nodes = navigator.Select(BooleanAttributesExpression);
        
        var result = new List<XAttribute>();
        while (nodes.MoveNext())
        {
            if (nodes.Current is IHasXmlNode nodeHasNode && 
                nodeHasNode.GetNode() is XElement element)
            {
                result.AddRange(element.Attributes()
                    .Where(a => IsBooleanAttribute(a.Name.LocalName)));
            }
        }
        
        return result;
    }
    
    // 批量处理优化
    public void ProcessLargeXml(string xmlPath)
    {
        var settings = new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
            ConformanceLevel = ConformanceLevel.Document
        };
        
        using var reader = XmlReader.Create(xmlPath, settings);
        
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && 
                reader.Name == "SiegeEngineType")
            {
                ProcessSiegeEngineType(reader);
            }
        }
    }
    
    private void ProcessSiegeEngineType(XmlReader reader)
    {
        var element = XElement.ReadFrom(reader) as XElement;
        if (element != null)
        {
            // 处理单个SiegeEngineType
            var booleanAttrs = element.Attributes()
                .Where(a => IsBooleanAttribute(a.Name.LocalName));
            
            foreach (var attr in booleanAttrs)
            {
                attr.Value = NormalizeBooleanValue(attr.Value);
            }
        }
    }
}
```

## 3. 测试技术栈

### 3.1 xUnit测试框架

#### 选择理由
- **并行测试**: 内置并行测试支持
- **数据驱动**: 强大的Theory和InlineData支持
- **扩展性**: 丰富的扩展点
- **集成性**: 与Visual Studio和CI/CD良好集成

#### 测试架构
```csharp
public class XmlRoundTripTests
{
    private readonly IXmlProcessor _xmlProcessor;
    private readonly ITestDataManager _testDataManager;
    
    public XmlRoundTripTests()
    {
        // 依赖注入
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();
        
        _xmlProcessor = serviceProvider.GetRequiredService<IXmlProcessor>();
        _testDataManager = serviceProvider.GetRequiredService<ITestDataManager>();
    }
    
    [Theory]
    [InlineData("SiegeEngines")]
    [InlineData("SpecialMeshes")]
    [InlineData("LanguageBase")]
    [InlineData("MultiplayerScenes")]
    [InlineData("TauntUsageSets")]
    [InlineData("LanguageXml")]
    public async Task XmlRoundTrip_ShouldPreserveStructure(string xmlType)
    {
        // Arrange
        var testDataPath = $"TestData/{xmlType}.xml";
        var originalXml = await _testDataManager.LoadTestDataAsync(testDataPath);
        
        // Act
        var deserialized = _xmlProcessor.Deserialize<object>(originalXml);
        var serialized = _xmlProcessor.Serialize(deserialized, originalXml);
        
        // Assert
        var isEqual = _xmlProcessor.AreXmlsEqual(originalXml, serialized, 
            new XmlComparisonOptions { Mode = ComparisonMode.Strict });
        
        Assert.True(isEqual, 
            $"XML round-trip failed for {xmlType}. Differences found.");
    }
    
    [Theory]
    [MemberData(nameof(GetXmlTestCases))]
    public async Task XmlProcessing_ShouldHandleComplexStructures(XmlTestCase testCase)
    {
        // Arrange
        var xml = await _testDataManager.LoadTestDataAsync(testCase.DataPath);
        
        // Act
        var result = _xmlProcessor.NormalizeXml(xml, testCase.Options);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(XmlValidator.IsValidXml(result));
        
        // 验证特定处理
        if (testCase.ExpectedChanges > 0)
        {
            var changes = XmlAnalyzer.CountChanges(xml, result);
            Assert.Equal(testCase.ExpectedChanges, changes);
        }
    }
    
    public static IEnumerable<object[]> GetXmlTestCases()
    {
        yield return new object[]
        {
            new XmlTestCase
            {
                Name = "SiegeEngines Boolean Normalization",
                DataPath = "TestData/SiegeEngines.xml",
                Options = new XmlNormalizationOptions
                {
                    NormalizeBooleanValues = true,
                    NormalizeAttributeOrder = true
                },
                ExpectedChanges = 5
            }
        };
        
        // 更多测试用例...
    }
}
```

### 3.2 BenchmarkDotNet性能测试

#### 性能基准设置
```csharp
[MemoryDiagnoser]
public class XmlProcessingBenchmark
{
    private readonly IXmlProcessor _xmlProcessor;
    private readonly string _testXml;
    
    public XmlProcessingBenchmark()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();
        
        _xmlProcessor = serviceProvider.GetRequiredService<IXmlProcessor>();
        _testXml = File.ReadAllText("TestData/LargeSiegeEngines.xml");
    }
    
    [Benchmark]
    public void Serialize_Deserialize_RoundTrip()
    {
        var deserialized = _xmlProcessor.Deserialize<object>(_testXml);
        var serialized = _xmlProcessor.Serialize(deserialized, _testXml);
    }
    
    [Benchmark]
    public void Normalize_Xml()
    {
        var normalized = _xmlProcessor.NormalizeXml(_testXml, 
            new XmlNormalizationOptions
            {
                NormalizeBooleanValues = true,
                NormalizeAttributeOrder = true,
                PreserveEmptyElements = true
            });
    }
    
    [Benchmark]
    public void Compare_Xml()
    {
        var isEqual = _xmlProcessor.AreXmlsEqual(_testXml, _testXml, 
            new XmlComparisonOptions { Mode = ComparisonMode.Strict });
    }
    
    [Benchmark]
    public void Process_With_Repair_Strategy()
    {
        var repaired = _xmlProcessor.RepairXml(_testXml, "SiegeEngines");
    }
}

// 内存使用分析
public class MemoryUsageBenchmark
{
    [Benchmark]
    public void Process_Large_Xml_File()
    {
        var largeXml = File.ReadAllText("TestData/VeryLargeXml.xml");
        var processor = new XmlProcessor();
        
        for (int i = 0; i < 100; i++)
        {
            var result = processor.NormalizeXml(largeXml);
        }
    }
    
    [GlobalCleanup]
    public void Cleanup()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
```

## 4. 配置和依赖注入

### 4.1 配置系统

#### 配置层次结构
```json
{
  "XmlProcessing": {
    "Normalization": {
      "NormalizeBooleanValues": true,
      "NormalizeAttributeOrder": true,
      "PreserveEmptyElements": true,
      "PreserveWhitespace": false,
      "PreserveComments": false,
      "ForceEmptyElementTags": true,
      "NumericTolerance": 0.0001
    },
    "Comparison": {
      "DefaultMode": "Strict",
      "IgnoreComments": true,
      "IgnoreWhitespace": true,
      "IgnoreAttributeOrder": true,
      "AllowCaseInsensitiveBooleans": true,
      "AllowNumericTolerance": true,
      "NumericTolerance": 0.0001
    },
    "Performance": {
      "EnableParallelProcessing": true,
      "MaxDegreeOfParallelism": 4,
      "MemoryLimitBytes": 536870912,
      "TimeoutMilliseconds": 30000
    },
    "Logging": {
      "LogLevel": "Information",
      "EnableDetailedLogging": false,
      "LogProcessingTime": true,
      "LogMemoryUsage": true
    }
  }
}
```

#### 配置类定义
```csharp
public class XmlProcessingConfiguration
{
    public NormalizationOptions Normalization { get; set; } = new();
    public ComparisonOptions Comparison { get; set; } = new();
    public PerformanceOptions Performance { get; set; } = new();
    public LoggingOptions Logging { get; set; } = new();
    public List<XmlTypeConfiguration> TypeConfigurations { get; set; } = new();
}

public class NormalizationOptions
{
    public bool NormalizeBooleanValues { get; set; } = true;
    public bool NormalizeAttributeOrder { get; set; } = true;
    public bool PreserveEmptyElements { get; set; } = true;
    public bool PreserveWhitespace { get; set; } = false;
    public bool PreserveComments { get; set; } = false;
    public bool ForceEmptyElementTags { get; set; } = true;
    public double NumericTolerance { get; set; } = 0.0001;
}

public class XmlTypeConfiguration
{
    public string TypeName { get; set; }
    public List<string> BooleanAttributes { get; set; } = new();
    public List<string> NumericAttributes { get; set; } = new();
    public List<string> EmptyElementNames { get; set; } = new();
    public bool PreserveWhitespace { get; set; }
    public bool PreserveComments { get; set; }
    public Dictionary<string, object> CustomSettings { get; set; } = new();
}
```

### 4.2 依赖注入配置

#### 服务注册
```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXmlProcessing(this IServiceCollection services, 
        IConfiguration configuration)
    {
        // 配置绑定
        services.Configure<XmlProcessingConfiguration>(
            configuration.GetSection("XmlProcessing"));
        
        // 核心服务
        services.AddScoped<IXmlProcessor, XmlProcessor>();
        services.AddScoped<IXmlNormalizer, XmlNormalizer>();
        services.AddScoped<IXmlComparator, XmlComparator>();
        services.AddScoped<IXmlSerializer, XmlSerializer>();
        
        // 标准化策略
        services.AddScoped<IXmlNormalizationStrategy, BooleanAttributeNormalizationStrategy>();
        services.AddScoped<IXmlNormalizationStrategy, AttributeOrderingStrategy>();
        services.AddScoped<IXmlNormalizationStrategy, EmptyElementNormalizationStrategy>();
        
        // 比较策略
        services.AddScoped<IXmlComparisonStrategy, StructuralComparisonStrategy>();
        services.AddScoped<IXmlComparisonStrategy, LogicalComparisonStrategy>();
        
        // 修复策略
        services.AddScoped<IXmlTypeRepairStrategy, SiegeEnginesRepairStrategy>();
        services.AddScoped<IXmlTypeRepairStrategy, SpecialMeshesRepairStrategy>();
        services.AddScoped<IXmlTypeRepairStrategy, LanguageBaseRepairStrategy>();
        services.AddScoped<IXmlTypeRepairStrategy, MultiplayerScenesRepairStrategy>();
        services.AddScoped<IXmlTypeRepairStrategy, TauntUsageSetsRepairStrategy>();
        services.AddScoped<IXmlTypeRepairStrategy, LanguageXmlRepairStrategy>();
        
        // 策略工厂
        services.AddScoped<IXmlNormalizationStrategyFactory, XmlNormalizationStrategyFactory>();
        services.AddScoped<IXmlComparisonStrategyFactory, XmlComparisonStrategyFactory>();
        services.AddScoped<IXmlRepairStrategyFactory, XmlRepairStrategyFactory>();
        
        // 插件管理
        services.AddScoped<IXmlProcessingPluginManager, XmlProcessingPluginManager>();
        services.AddScoped<IPluginLoader, DirectoryPluginLoader>();
        
        // 测试框架
        services.AddScoped<IXmlTestFramework, XmlTestFramework>();
        services.AddScoped<ITestDataManager, FileTestDataManager>();
        services.AddScoped<ITestReporter, ConsoleTestReporter>();
        
        // 性能监控
        services.AddScoped<IXmlPerformanceMonitor, XmlPerformanceMonitor>();
        services.AddScoped<IBenchmarkRunner, BenchmarkRunner>();
        
        // 配置提供者
        services.AddScoped<IXmlProcessingConfigurationProvider, XmlProcessingConfigurationProvider>();
        
        return services;
    }
}
```

## 5. 日志和监控

### 5.1 Serilog配置

#### 日志配置
```csharp
public static class SerilogConfiguration
{
    public static LoggerConfiguration CreateLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "logs/xml-processing-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Seq(
                serverUrl: configuration["Seq:ServerUrl"],
                apiKey: configuration["Seq:ApiKey"])
            .CreateLogger();
    }
}

// 结构化日志记录
public class XmlProcessingLogger
{
    private readonly ILogger _logger;
    
    public XmlProcessingLogger(ILogger<XmlProcessingLogger> logger)
    {
        _logger = logger;
    }
    
    public void LogProcessingStart(string xmlType, string operation)
    {
        _logger.Information("XML processing started: {XmlType} - {Operation}", 
            xmlType, operation);
    }
    
    public void LogProcessingComplete(string xmlType, string operation, 
        TimeSpan duration, long memoryUsed)
    {
        _logger.Information("XML processing completed: {XmlType} - {Operation} - Duration: {Duration}ms - Memory: {Memory}bytes", 
            xmlType, operation, duration.TotalMilliseconds, memoryUsed);
    }
    
    public void LogProcessingError(string xmlType, string operation, 
        Exception exception, Dictionary<string, object> context)
    {
        _logger.Error(exception, "XML processing error: {XmlType} - {Operation} - Context: {@Context}", 
            xmlType, operation, context);
    }
    
    public void LogPerformanceMetrics(XmlPerformanceMetrics metrics)
    {
        _logger.Information("Performance metrics: {@Metrics}", metrics);
    }
}

public class XmlPerformanceMetrics
{
    public string Operation { get; set; }
    public string XmlType { get; set; }
    public TimeSpan Duration { get; set; }
    public long MemoryUsed { get; set; }
    public int XmlSize { get; set; }
    public int ProcessedElements { get; set; }
    public int ProcessedAttributes { get; set; }
    public Dictionary<string, object> AdditionalMetrics { get; set; } = new();
}
```

### 5.2 性能监控

#### 性能计数器
```csharp
public class XmlPerformanceMonitor
{
    private readonly ILogger<XmlPerformanceMonitor> _logger;
    private readonly ConcurrentDictionary<string, PerformanceCounter> _counters;
    
    public XmlPerformanceMonitor(ILogger<XmlPerformanceMonitor> logger)
    {
        _logger = logger;
        _counters = new ConcurrentDictionary<string, PerformanceCounter>();
    }
    
    public void TrackProcessingTime(string operation, TimeSpan duration)
    {
        var counter = _counters.GetOrAdd(operation, 
            key => new PerformanceCounter("XML Processing", key, false));
        
        counter.IncrementBy((long)duration.TotalMilliseconds);
        
        _logger.LogDebug("Processing time tracked: {Operation} - {Duration}ms", 
            operation, duration.TotalMilliseconds);
    }
    
    public void TrackMemoryUsage(string operation, long bytes)
    {
        var counter = _counters.GetOrAdd($"{operation}_Memory", 
            key => new PerformanceCounter("XML Processing", key, false));
        
        counter.IncrementBy(bytes);
        
        _logger.LogDebug("Memory usage tracked: {Operation} - {Bytes}bytes", 
            operation, bytes);
    }
    
    public PerformanceReport GenerateReport()
    {
        var report = new PerformanceReport
        {
            GeneratedAt = DateTime.UtcNow,
            Metrics = new Dictionary<string, PerformanceMetric>()
        };
        
        foreach (var counter in _counters)
        {
            report.Metrics[counter.Key] = new PerformanceMetric
            {
                Name = counter.Key,
                Value = counter.NextValue(),
                Type = counter.CounterType
            };
        }
        
        return report;
    }
}

public class PerformanceReport
{
    public DateTime GeneratedAt { get; set; }
    public Dictionary<string, PerformanceMetric> Metrics { get; set; }
}

public class PerformanceMetric
{
    public string Name { get; set; }
    public float Value { get; set; }
    public PerformanceCounterType Type { get; set; }
}
```

## 6. 部署和CI/CD

### 6.1 Docker配置

#### Dockerfile
```dockerfile
# 基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# 构建镜像
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BannerlordModEditor-CLI.csproj", "."]
RUN dotnet restore "BannerlordModEditor-CLI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "BannerlordModEditor-CLI.csproj" -c Release -o /app/build

# 发布镜像
FROM build AS publish
RUN dotnet publish "BannerlordModEditor-CLI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 最终镜像
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 健康检查
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "BannerlordModEditor-CLI.dll"]
```

#### docker-compose.yml
```yaml
version: '3.8'

services:
  xml-processor:
    build: .
    ports:
      - "8080:80"
      - "8443:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80;https://+:443
      - ConnectionStrings__DefaultConnection=Server=db;Database=XmlProcessing;User=sa;Password=your_password;
    volumes:
      - ./logs:/app/logs
      - ./TestData:/app/TestData
    depends_on:
      - db
      - seq
    networks:
      - xml-network

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=your_password
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
    volumes:
      - sql_data:/var/opt/mssql
    networks:
      - xml-network

  seq:
    image: datalust/seq:latest
    environment:
      - ACCEPT_EULA=Y
    ports:
      - "5341:80"
    volumes:
      - seq_data:/data
    networks:
      - xml-network

volumes:
  sql_data:
  seq_data:

networks:
  xml-network:
    driver: bridge
```

### 6.2 GitHub Actions配置

#### CI/CD Pipeline
```yaml
name: XML Processing CI/CD

on:
  push:
    branches: [ feature/cli-development ]
  pull_request:
    branches: [ feature/cli-development ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        env:
          SA_PASSWORD: your_password
          ACCEPT_EULA: Y
        ports:
          - 1433:1433
        options: --health-cmd "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P your_password -Q 'SELECT 1'" --health-interval 10s --health-timeout 3s --health-retries 3
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 9.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build solution
      run: dotnet build --configuration Release
    
    - name: Run unit tests
      run: dotnet test BannerlordModEditor.Common.Tests --configuration Release --logger "console;verbosity=detailed"
    
    - name: Run integration tests
      run: dotnet test BannerlordModEditor.Cli.IntegrationTests --configuration Release --logger "console;verbosity=detailed"
    
    - name: Run performance benchmarks
      run: dotnet test BannerlordModEditor.Common.Tests --configuration Release --logger "console;verbosity=detailed" --filter "FullyQualifiedName~Benchmark"
    
    - name: Generate coverage report
      run: |
        dotnet test --configuration Release --collect:"XPlat Code Coverage"
        dotnet reportgenerator -reports:coverage.xml -targetdir:coverage-report
    
    - name: Upload coverage to Codecov
      uses: codecov/codecov-action@v3
      with:
        file: ./coverage.xml
    
    - name: Build and push Docker image
      if: github.ref == 'refs/heads/feature/cli-development'
      run: |
        echo ${{ secrets.DOCKER_PASSWORD }} | docker login -u ${{ secrets.DOCKER_USERNAME }} --password-stdin
        docker build -t bannerlord-mod-editor:latest .
        docker push bannerlord-mod-editor:latest
    
    - name: Deploy to staging
      if: github.ref == 'refs/heads/feature/cli-development'
      run: |
        # 部署到测试环境
        echo "Deploying to staging environment..."
    
    - name: Run smoke tests
      if: github.ref == 'refs/heads/feature/cli-development'
      run: |
        # 运行冒烟测试
        echo "Running smoke tests..."
```

## 7. 安全考虑

### 7.1 认证和授权

#### JWT配置
```csharp
public class JwtConfiguration
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
}

public class JwtService
{
    private readonly JwtConfiguration _config;
    
    public JwtService(IOptions<JwtConfiguration> config)
    {
        _config = config.Value;
    }
    
    public string GenerateToken(string username, IList<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_config.ExpirationMinutes),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public bool ValidateToken(string token, out ClaimsPrincipal principal)
    {
        principal = null;
        
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.Key);
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config.Issuer,
                ValidateAudience = true,
                ValidAudience = _config.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            principal = tokenHandler.ValidateToken(token, validationParameters);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

### 7.2 输入验证

#### FluentValidation配置
```csharp
public class XmlProcessingRequestValidator : AbstractValidator<XmlProcessingRequest>
{
    public XmlProcessingRequestValidator()
    {
        RuleFor(x => x.Xml)
            .NotEmpty()
            .Must(BeValidXml)
            .WithMessage("Invalid XML format");
        
        RuleFor(x => x.Options)
            .NotNull();
        
        RuleFor(x => x.XmlType)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.XmlType));
    }
    
    private bool BeValidXml(string xml)
    {
        try
        {
            XDocument.Parse(xml);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class XmlComparisonRequestValidator : AbstractValidator<XmlComparisonRequest>
{
    public XmlComparisonRequestValidator()
    {
        RuleFor(x => x.Xml1)
            .NotEmpty()
            .Must(BeValidXml);
        
        RuleFor(x => x.Xml2)
            .NotEmpty()
            .Must(BeValidXml);
        
        RuleFor(x => x.Options)
            .NotNull();
    }
    
    private bool BeValidXml(string xml)
    {
        try
        {
            XDocument.Parse(xml);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
```

## 8. 性能优化策略

### 8.1 内存优化

#### 对象池模式
```csharp
public class XmlProcessorPool
{
    private readonly ConcurrentBag<IXmlProcessor> _processors;
    private readonly Func<IXmlProcessor> _processorFactory;
    private readonly int _maxPoolSize;
    
    public XmlProcessorPool(Func<IXmlProcessor> processorFactory, int maxPoolSize = 10)
    {
        _processorFactory = processorFactory;
        _maxPoolSize = maxPoolSize;
        _processors = new ConcurrentBag<IXmlProcessor>();
    }
    
    public IXmlProcessor GetProcessor()
    {
        if (_processors.TryTake(out var processor))
        {
            return processor;
        }
        
        return _processorFactory();
    }
    
    public void ReturnProcessor(IXmlProcessor processor)
    {
        if (_processors.Count < _maxPoolSize)
        {
            _processors.Add(processor);
        }
    }
}

public class XmlProcessingScope : IDisposable
{
    private readonly XmlProcessorPool _pool;
    private readonly IXmlProcessor _processor;
    
    public XmlProcessingScope(XmlProcessorPool pool)
    {
        _pool = pool;
        _processor = pool.GetProcessor();
    }
    
    public IXmlProcessor Processor => _processor;
    
    public void Dispose()
    {
        _pool.ReturnProcessor(_processor);
    }
}

// 使用示例
public class XmlProcessingService
{
    private readonly XmlProcessorPool _processorPool;
    
    public XmlProcessingService(XmlProcessorPool processorPool)
    {
        _processorPool = processorPool;
    }
    
    public async Task<string> ProcessXmlAsync(string xml)
    {
        using var scope = new XmlProcessingScope(_processorPool);
        var processor = scope.Processor;
        
        return await Task.Run(() => processor.NormalizeXml(xml));
    }
}
```

### 8.2 并发处理

#### 并行处理策略
```csharp
public class ParallelXmlProcessor
{
    private readonly IXmlProcessor _xmlProcessor;
    private readonly ParallelOptions _parallelOptions;
    
    public ParallelXmlProcessor(IXmlProcessor xmlProcessor, int maxDegreeOfParallelism = 4)
    {
        _xmlProcessor = xmlProcessor;
        _parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism
        };
    }
    
    public async Task<List<XmlProcessingResult>> ProcessBatchAsync(List<XmlProcessingRequest> requests)
    {
        var results = new ConcurrentBag<XmlProcessingResult>();
        
        await Task.Run(() => Parallel.ForEach(requests, _parallelOptions, request =>
        {
            try
            {
                var result = _xmlProcessor.NormalizeXml(request.Xml, request.Options);
                results.Add(new XmlProcessingResult
                {
                    Success = true,
                    ProcessedXml = result,
                    RequestId = request.Id
                });
            }
            catch (Exception ex)
            {
                results.Add(new XmlProcessingResult
                {
                    Success = false,
                    Error = ex.Message,
                    RequestId = request.Id
                });
            }
        }));
        
        return results.ToList();
    }
}
```

## 9. 监控和告警

### 9.1 应用程序监控

#### 健康检查
```csharp
public class XmlProcessingHealthCheck : IHealthCheck
{
    private readonly IXmlProcessor _xmlProcessor;
    private readonly IXmlProcessingConfigurationProvider _configProvider;
    
    public XmlProcessingHealthCheck(IXmlProcessor xmlProcessor, 
        IXmlProcessingConfigurationProvider configProvider)
    {
        _xmlProcessor = xmlProcessor;
        _configProvider = configProvider;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // 检查配置是否加载
            var config = _configProvider.GetConfiguration();
            if (config == null)
            {
                return HealthCheckResult.Unhealthy("Configuration not loaded");
            }
            
            // 检查XML处理器是否正常工作
            var testXml = "<test><item active=\"true\"/></test>";
            var result = _xmlProcessor.NormalizeXml(testXml, 
                new XmlNormalizationOptions { NormalizeBooleanValues = true });
            
            if (string.IsNullOrEmpty(result))
            {
                return HealthCheckResult.Unhealthy("XML processor not working");
            }
            
            // 检查内存使用情况
            var memoryInfo = GC.GetGCMemoryInfo();
            if (memoryInfo.MemoryLoadBytes > 0.9 * memoryInfo.TotalAvailableMemoryBytes)
            {
                return HealthCheckResult.Degraded("High memory usage detected");
            }
            
            return HealthCheckResult.Healthy("XML processing system is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Health check failed: {ex.Message}", ex);
        }
    }
}
```

### 9.2 指标收集

#### Prometheus指标
```csharp
public class XmlProcessingMetrics
{
    private readonly Counter _xmlProcessingCounter;
    private readonly Histogram _xmlProcessingDuration;
    private readonly Gauge _xmlProcessingMemory;
    
    public XmlProcessingMetrics(IMetricFactory metricFactory)
    {
        _xmlProcessingCounter = metricFactory.CreateCounter(
            "xml_processing_total",
            "Total number of XML processing operations",
            new[] { "operation", "xml_type", "status" });
        
        _xmlProcessingDuration = metricFactory.CreateHistogram(
            "xml_processing_duration_seconds",
            "Duration of XML processing operations",
            new[] { "operation", "xml_type" });
        
        _xmlProcessingMemory = metricFactory.CreateGauge(
            "xml_processing_memory_bytes",
            "Memory usage during XML processing",
            new[] { "operation", "xml_type" });
    }
    
    public void RecordProcessing(string operation, string xmlType, bool success)
    {
        _xmlProcessingCounter.Inc(success ? 1 : 0, operation, xmlType, success ? "success" : "failure");
    }
    
    public void RecordDuration(string operation, string xmlType, TimeSpan duration)
    {
        _xmlProcessingDuration.Observe(duration.TotalSeconds, operation, xmlType);
    }
    
    public void RecordMemoryUsage(string operation, string xmlType, long bytes)
    {
        _xmlProcessingMemory.Set(bytes, operation, xmlType);
    }
}
```

## 10. 故障排除和调试

### 10.1 诊断工具

#### 诊断收集器
```csharp
public class XmlProcessingDiagnostics
{
    private readonly ILogger<XmlProcessingDiagnostics> _logger;
    private readonly DiagnosticCollector _collector;
    
    public XmlProcessingDiagnostics(ILogger<XmlProcessingDiagnostics> logger)
    {
        _logger = logger;
        _collector = new DiagnosticCollector();
    }
    
    public async Task<DiagnosticReport> CollectDiagnosticsAsync()
    {
        var report = new DiagnosticReport
        {
            CollectedAt = DateTime.UtcNow,
            SystemInfo = CollectSystemInfo(),
            PerformanceInfo = CollectPerformanceInfo(),
            ConfigurationInfo = CollectConfigurationInfo(),
            ErrorInfo = CollectErrorInfo()
        };
        
        return report;
    }
    
    private SystemInfo CollectSystemInfo()
    {
        return new SystemInfo
        {
            OSVersion = Environment.OSVersion.ToString(),
            MachineName = Environment.MachineName,
            ProcessorCount = Environment.ProcessorCount,
            WorkingSet = Environment.WorkingSet,
            Is64BitProcess = Environment.Is64BitProcess,
            FrameworkVersion = RuntimeInformation.FrameworkDescription
        };
    }
    
    private PerformanceInfo CollectPerformanceInfo()
    {
        var process = Process.GetCurrentProcess();
        
        return new PerformanceInfo
        {
            ProcessId = process.Id,
            ThreadCount = process.Threads.Count,
            HandleCount = process.HandleCount,
            PrivateMemorySize = process.PrivateMemorySize64,
            VirtualMemorySize = process.VirtualMemorySize64,
            WorkingSet = process.WorkingSet64,
            PeakWorkingSet = process.PeakWorkingSet64,
            CpuUsage = GetCpuUsage()
        };
    }
    
    private double GetCpuUsage()
    {
        var startTime = DateTime.UtcNow;
        var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
        
        Thread.Sleep(1000);
        
        var endTime = DateTime.UtcNow;
        var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
        
        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;
        
        return cpuUsedMs / (Environment.ProcessorCount * totalMsPassed) * 100;
    }
}

public class DiagnosticReport
{
    public DateTime CollectedAt { get; set; }
    public SystemInfo SystemInfo { get; set; }
    public PerformanceInfo PerformanceInfo { get; set; }
    public ConfigurationInfo ConfigurationInfo { get; set; }
    public ErrorInfo ErrorInfo { get; set; }
}
```

## 11. 总结

### 11.1 技术栈优势

**性能优势：**
- .NET 9提供卓越的性能表现
- 并行处理和内存优化
- 高效的XML处理库

**可维护性：**
- 清晰的架构分层
- 依赖注入和配置管理
- 完善的日志和监控

**可扩展性：**
- 插件化架构设计
- 配置驱动的处理策略
- 模块化的测试框架

**可靠性：**
- 完整的错误处理机制
- 健康检查和监控
- 全面的测试覆盖

### 11.2 实施建议

1. **分阶段实施**: 按照优先级分阶段实施各个组件
2. **持续监控**: 建立完善的监控和告警机制
3. **性能优化**: 定期进行性能测试和优化
4. **文档更新**: 保持技术文档的及时更新

### 11.3 未来展望

**技术演进：**
- .NET 10的新特性
- 更高效的XML处理技术
- 云原生架构支持

**功能扩展：**
- 更多XML类型支持
- 高级分析功能
- 机器学习集成

---

**文档版本**: 1.0  
**创建日期**: 2025-08-27  
**最后更新**: 2025-08-27  
**下次更新**: 技术栈变更时更新