# XML-Excel互转适配系统技术栈决策和依赖

## 技术栈概述

本文档详细描述了XML-Excel互转适配系统的技术栈选择、依赖管理和配置策略。该系统基于现有的.NET 9和Avalonia UI技术栈，扩展了原有的FormatConversionService，提供了完整的类型化XML-Excel转换解决方案。

## 核心技术栈

### 1. .NET 9 和 C# 12

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| .NET 9 | 9.0 | 最新LTS版本，性能优异，现代语言特性 | 应用程序运行时 |
| C# 12 | 12.0 | 现代语言特性，性能优化，类型安全 | 主要开发语言 |
| LINQ | 内置 | 强大的数据查询能力 | 数据处理 |
| async/await | 内置 | 异步编程支持 | I/O操作 |
| Nullable引用类型 | 内置 | 编译时空值检查 | 代码质量 |

**为什么选择.NET 9？**
- **性能**: 显著的性能改进和内存优化
- **现代化**: 最新的语言特性和API
- **跨平台**: 原生支持Windows、macOS、Linux
- **生态系统**: 丰富的库和工具支持
- **LTS支持**: 长期支持版本

### 2. XML处理技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| System.Xml.Serialization | 内置 | 原生XML序列化支持 | DO/DTO序列化 |
| System.Xml.Linq | 内置 | 现代XML处理API | XML查询和操作 |
| System.Xml.Reader/Writer | 内置 | 高性能XML读写 | 流式处理 |
| System.Xml.Schema | 内置 | XML模式验证 | 数据验证 |

**XML处理策略：**
```csharp
// 高性能XML读取配置
var xmlReaderSettings = new XmlReaderSettings
{
    IgnoreWhitespace = true,
    IgnoreComments = true,
    DtdProcessing = DtdProcessing.Ignore,
    CloseInput = true,
    MaxCharactersFromEntities = 1024,
    MaxCharactersInDocument = 50 * 1024 * 1024 // 50MB限制
};

// XML写入配置
var xmlWriterSettings = new XmlWriterSettings
{
    Indent = true,
    IndentChars = "\t",
    Encoding = Encoding.UTF8,
    CheckCharacters = true,
    CloseOutput = true,
    OmitXmlDeclaration = false
};
```

### 3. Excel处理技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| ClosedXML | 0.104.0 | 无需Excel安装，功能完整 | Excel文件读写 |
| EPPlus | 7.0.0 | 备选方案，功能丰富 | 高级Excel功能 |
| NPOI | 2.6.0 | 跨平台支持 | 备选方案 |

**为什么选择ClosedXML？**
- **无依赖**: 不需要安装Excel
- **功能完整**: 支持Excel 2007+格式
- **易用性**: 简洁的API设计
- **性能**: 良好的性能表现
- **开源**: MIT许可证

**Excel处理示例：**
```csharp
// Excel读取配置
using var workbook = new XLWorkbook(excelPath);
var worksheet = workbook.Worksheets.First();

// Excel写入配置
using var workbook = new XLWorkbook();
var worksheet = workbook.Worksheets.Add("Sheet1");

// 性能优化
worksheet.Columns().AdjustToContents();
worksheet.RangeUsed().SetAutoFilter();
```

### 4. 缓存和性能优化技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| Microsoft.Extensions.Caching.Memory | 9.0.0 | 官方缓存库，功能完整 | 内存缓存 |
| System.Collections.Concurrent | 内置 | 线程安全集合 | 并发处理 |
| System.Buffers | 内置 | 内存池支持 | 内存优化 |
| System.Threading.Channels | 内置 | 生产者-消费者模式 | 异步队列 |

**缓存策略：**
```csharp
// 缓存配置
services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024 * 1024 * 100; // 100MB
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
    options.CompactionPercentage = 0.05;
});

// 对象池配置
services.AddSingleton<IObjectPool<XmlSerializer>>(provider =>
    new DefaultObjectPool<XmlSerializer>(
        new DefaultPooledObjectPolicy<XmlSerializer>(), 
        10));
```

### 5. 依赖注入技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| Microsoft.Extensions.DependencyInjection | 9.0.0 | 官方DI容器，性能优异 | 依赖管理 |
| Microsoft.Extensions.Options | 9.0.0 | 配置选项模式 | 配置管理 |
| Microsoft.Extensions.Logging | 9.0.0 | 结构化日志 | 日志记录 |

**DI配置示例：**
```csharp
// 服务注册
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddXmlExcelConversion(this IServiceCollection services)
    {
        // 单例服务
        services.AddSingleton<IEnhancedFormatConversionService, EnhancedFormatConversionService>();
        services.AddSingleton<IXmlExcelConverterFactory, XmlExcelConverterFactory>();
        
        // 作用域服务
        services.AddScoped<IConversionProgressService, ConversionProgressService>();
        
        // 瞬态服务
        services.AddTransient<IErrorHandler, ErrorHandler>();
        
        // 配置
        services.Configure<XmlExcelConversionOptions>(configuration.GetSection("XmlExcelConversion"));
        
        return services;
    }
}
```

## 依赖管理

### 1. 核心依赖包

```xml
<!-- 项目文件依赖配置 -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  
  <ItemGroup>
    <!-- XML处理 -->
    <PackageReference Include="System.Xml.ReaderWriter" Version="9.0.0" />
    <PackageReference Include="System.Xml.Linq" Version="9.0.0" />
    <PackageReference Include="System.Xml.Serialization" Version="9.0.0" />
    
    <!-- Excel处理 -->
    <PackageReference Include="ClosedXML" Version="0.104.0" />
    
    <!-- 缓存和性能 -->
    <PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="9.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
    <PackageReference Include="Microsoft.Extensions.Options" Version="9.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.0" />
    
    <!-- 日志和监控 -->
    <PackageReference Include="Serilog" Version="4.0.0" />
    <PackageReference Include="Serilog.Extensions.Hosting" Version="8.0.0" />
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.0" />
    <PackageReference Include="Serilog.Sinks.File" Version="6.0.0" />
    
    <!-- 测试框架 -->
    <PackageReference Include="xunit" Version="2.5.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
    <PackageReference Include="Moq" Version="4.20.0" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
  </ItemGroup>
</Project>
```

### 2. 条件依赖

```xml
<!-- 条件依赖配置 -->
<ItemGroup Condition="'$(Configuration)' == 'Debug'">
  <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.0" />
  <PackageReference Include="Serilog.Sinks.Seq" Version="8.0.0" />
</ItemGroup>

<ItemGroup Condition="'$(Configuration)' == 'Release'">
  <PackageReference Include="Serilog.Sinks.ApplicationInsights" Version="4.0.0" />
</ItemGroup>

<!-- 平台特定依赖 -->
<ItemGroup Condition="'$(RuntimeIdentifier)' == 'win-x64'">
  <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.0" />
</ItemGroup>

<ItemGroup Condition="'$(RuntimeIdentifier)' == 'linux-x64'">
  <PackageReference Include="Npgsql" Version="8.0.0" />
</ItemGroup>
```

### 3. 开发工具依赖

```xml
<!-- 开发工具依赖 -->
<ItemGroup>
  <!-- 代码分析 -->
  <PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="9.0.0" />
  <PackageReference Include="StyleCop.Analyzers" Version="1.2.0-beta.556" />
  
  <!-- 性能分析 -->
  <PackageReference Include="BenchmarkDotNet" Version="0.13.0" />
  <PackageReference Include="DotMemory.Unit" Version="2023.2.0" />
  
  <!-- 文档生成 -->
  <PackageReference Include="DocFX" Version="2.74.0" />
  
  <!-- 构建工具 -->
  <PackageReference Include="MSBuild.Sdk.Extras" Version="3.0.44" />
</ItemGroup>
```

## 性能优化技术栈

### 1. 内存优化

| 技术 | 用途 | 实现方式 |
|------|------|----------|
| 对象池 | 减少GC压力 | 实现XmlSerializer和XLWorkbook对象池 |
| 内存缓存 | 减少重复计算 | 使用MemoryCache缓存转换结果 |
| 流式处理 | 减少内存占用 | 大文件分片流式处理 |
| 值类型优化 | 减少堆分配 | 使用struct和Span<T> |

**对象池实现：**
```csharp
public class XmlSerializerPool : ObjectPool<XmlSerializer>
{
    private readonly ConcurrentDictionary<Type, XmlSerializer> _serializers = new();
    
    protected override XmlSerializer Create()
    {
        throw new NotImplementedException("使用GetSerializer方法");
    }
    
    public XmlSerializer GetSerializer(Type type)
    {
        return _serializers.GetOrAdd(type, t => new XmlSerializer(t));
    }
    
    protected override bool Return(XmlSerializer obj)
    {
        // XmlSerializer是线程安全的，不需要返回
        return true;
    }
}
```

### 2. 并发处理技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| System.Threading.Channels | 内置 | 高性能生产者-消费者模式 | 异步队列处理 |
| System.Threading.Tasks.Dataflow | 内置 | 数据流编程 | 并行数据管道 |
| System.Collections.Concurrent | 内置 | 线程安全集合 | 并发数据结构 |
| System.Threading.SemaphoreSlim | 内置 | 信号量限制 | 并发控制 |

**并发处理示例：**
```csharp
// 并发限制处理
public class ConcurrentProcessor
{
    private readonly SemaphoreSlim _semaphore;
    private readonly Channel<ConversionTask> _channel;
    
    public ConcurrentProcessor(int maxConcurrency)
    {
        _semaphore = new SemaphoreSlim(maxConcurrency);
        _channel = Channel.CreateUnbounded<ConversionTask>();
    }
    
    public async Task ProcessAsync(IEnumerable<ConversionTask> tasks)
    {
        var processingTasks = tasks.Select(task => ProcessSingleTaskAsync(task));
        await Task.WhenAll(processingTasks);
    }
    
    private async Task ProcessSingleTaskAsync(ConversionTask task)
    {
        await _semaphore.WaitAsync();
        try
        {
            await ProcessTaskInternalAsync(task);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
```

### 3. 异步编程技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| Task-based Async Pattern | 内置 | 现代异步编程模式 | 异步操作 |
| ValueTask | 内置 | 减少堆分配 | 高性能异步 |
| IAsyncEnumerable | 内置 | 异步流处理 | 大数据集处理 |
| CancellationToken | 内置 | 异步操作取消 | 任务管理 |

**异步处理示例：**
```csharp
// 异步转换管道
public class AsyncConversionPipeline
{
    public async IAsyncEnumerable<ConversionResult> ConvertAsync(
        IEnumerable<ConversionTask> tasks,
        CancellationToken cancellationToken = default)
    {
        foreach (var task in tasks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var result = await ConvertSingleTaskAsync(task, cancellationToken);
            yield return result;
        }
    }
    
    private async Task<ConversionResult> ConvertSingleTaskAsync(
        ConversionTask task,
        CancellationToken cancellationToken)
    {
        return await Task.Run(() => ProcessTask(task), cancellationToken);
    }
}
```

## 监控和诊断技术栈

### 1. 日志记录技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| Serilog | 4.0.0 | 结构化日志，高性能 | 应用日志 |
| Microsoft.Extensions.Logging | 9.0.0 | 官方日志框架 | 集成日志 |
| Seq | 2023.3 | 日志聚合和分析 | 日志管理 |

**日志配置示例：**
```csharp
// 日志配置
public static class LoggingConfigurator
{
    public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/xml-excel-converter-.txt", 
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .WriteTo.Seq("http://localhost:5341"));
    }
}
```

### 2. 性能监控技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| System.Diagnostics | 内置 | 内置性能计数器 | 性能监控 |
| BenchmarkDotNet | 0.13.0 | 性能基准测试 | 性能分析 |
| DotMemory | 2023.2 | 内存分析 | 内存优化 |

**性能监控实现：**
```csharp
// 性能监控
public class PerformanceMonitor
{
    private readonly ConcurrentDictionary<string, PerformanceMetrics> _metrics = new();
    
    public void RecordMetric(string name, double value)
    {
        _metrics.AddOrUpdate(name, 
            key => new PerformanceMetrics { Name = key, Values = { value } },
            (key, existing) =>
            {
                existing.Values.Add(value);
                existing.LastUpdated = DateTime.UtcNow;
                return existing;
            });
    }
    
    public PerformanceReport GenerateReport()
    {
        return new PerformanceReport
        {
            Metrics = _metrics.Values.ToList(),
            GeneratedAt = DateTime.UtcNow
        };
    }
}
```

### 3. 健康检查技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| Microsoft.Extensions.Diagnostics.HealthChecks | 9.0.0 | 官方健康检查框架 | 健康监控 |
| AspNetCore.HealthChecks | 8.0.0 | 丰富的健康检查扩展 | 系统监控 |

**健康检查配置：**
```csharp
// 健康检查配置
public static class HealthCheckConfigurator
{
    public static IHealthChecksBuilder AddXmlExcelConverterHealthChecks(
        this IHealthChecksBuilder builder)
    {
        return builder
            .AddCheck<XmlConverterHealthCheck>("xml-converter")
            .AddCheck<ExcelConverterHealthCheck>("excel-converter")
            .AddCheck<FileSystemHealthCheck>("file-system")
            .AddCheck<MemoryHealthCheck>("memory")
            .AddCheck<PluginSystemHealthCheck>("plugin-system");
    }
}
```

## 测试技术栈

### 1. 单元测试技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| xUnit | 2.5.0 | 现代测试框架 | 单元测试 |
| Moq | 4.20.0 | 强大的Mock框架 | 模拟依赖 |
| FluentAssertions | 6.12.0 | 流畅的断言API | 断言验证 |

**单元测试示例：**
```csharp
// 单元测试
public class XmlExcelConverterTests
{
    private readonly Mock<IFileDiscoveryService> _fileDiscoveryMock;
    private readonly Mock<IValidationService> _validationMock;
    private readonly EnhancedFormatConversionService _service;
    
    public XmlExcelConverterTests()
    {
        _fileDiscoveryMock = new Mock<IFileDiscoveryService>();
        _validationMock = new Mock<IValidationService>();
        _service = new EnhancedFormatConversionService(
            _fileDiscoveryMock.Object,
            _validationMock.Object);
    }
    
    [Fact]
    public async Task XmlToExcelAsync_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var xmlPath = "test.xml";
        var excelPath = "test.xlsx";
        
        _fileDiscoveryMock.Setup(x => x.FileExists(xmlPath)).Returns(true);
        _validationMock.Setup(x => x.ValidateXmlFileAsync(xmlPath))
            .ReturnsAsync(new ValidationResult { IsValid = true });
        
        // Act
        var result = await _service.XmlToExcelAsync(xmlPath, excelPath);
        
        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }
}
```

### 2. 集成测试技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| TestContainers | 3.7.0 | 容器化测试 | 集成测试 |
| Microsoft.AspNetCore.Mvc.Testing | 9.0.0 | 集成测试框架 | API测试 |
| FluentAssertions | 6.12.0 | 流畅的断言API | 断言验证 |

**集成测试示例：**
```csharp
// 集成测试
public class XmlExcelConverterIntegrationTests : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;
    
    public XmlExcelConverterIntegrationTests(TestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task EndToEndXmlToExcelConversion_CompletesSuccessfully()
    {
        // Arrange
        var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item id=""1"" name=""Test Item"" />
</root>";
        
        var xmlPath = Path.GetTempFileName() + ".xml";
        var excelPath = Path.GetTempFileName() + ".xlsx";
        
        await File.WriteAllTextAsync(xmlPath, xmlContent);
        
        try
        {
            // Act
            var result = await _fixture.Service.XmlToExcelAsync(xmlPath, excelPath);
            
            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            File.Exists(excelPath).Should().BeTrue();
            
            // 验证Excel内容
            using var workbook = new XLWorkbook(excelPath);
            var worksheet = workbook.Worksheets.First();
            var rowCount = worksheet.RowsUsed().Count();
            rowCount.Should().BeGreaterThan(1);
        }
        finally
        {
            // 清理
            if (File.Exists(xmlPath)) File.Delete(xmlPath);
            if (File.Exists(excelPath)) File.Delete(excelPath);
        }
    }
}
```

### 3. 性能测试技术栈

| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| BenchmarkDotNet | 0.13.0 | 性能基准测试 | 性能测试 |
| NBomber | 3.0.0 | 负载测试 | 压力测试 |
| DotTrace | 2023.2 | 性能分析 | 性能调优 |

**性能测试示例：**
```csharp
// 性能测试
[MemoryDiagnoser]
public class XmlExcelConverterPerformanceTests
{
    private readonly EnhancedFormatConversionService _service;
    
    public XmlExcelConverterPerformanceTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddXmlExcelConversion()
            .BuildServiceProvider();
        
        _service = serviceProvider.GetRequiredService<EnhancedFormatConversionService>();
    }
    
    [Benchmark]
    public async Task XmlToExcel_SmallFile()
    {
        var xmlPath = "test_small.xml";
        var excelPath = "test_small.xlsx";
        
        await _service.XmlToExcelAsync(xmlPath, excelPath);
    }
    
    [Benchmark]
    public async Task XmlToExcel_LargeFile()
    {
        var xmlPath = "test_large.xml";
        var excelPath = "test_large.xlsx";
        
        await _service.XmlToExcelAsync(xmlPath, excelPath);
    }
}
```

## 配置管理

### 1. 应用配置

```json
{
  "XmlExcelConversion": {
    "MaxParallelism": 4,
    "ChunkSizeThreshold": 52428800,
    "DefaultChunkSize": 10000,
    "MemoryThreshold": 80.0,
    "EnableCaching": true,
    "CacheExpirationMinutes": 30,
    "MaxCacheSize": 100,
    "ContinueOnError": false,
    "EnableRecovery": true,
    "MaxRetryAttempts": 3,
    "EnableValidation": true,
    "ValidationLevel": "Standard",
    "EnablePlugins": true,
    "PluginDirectory": "plugins",
    "LogLevel": "Information",
    "EnableDetailedLogging": false
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/xml-excel-converter-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7
        }
      }
    ]
  }
}
```

### 2. 环境配置

```bash
# 开发环境配置
export ASPNETCORE_ENVIRONMENT=Development
export XML_EXCEL_PLUGIN_DIRECTORY=./plugins
export XML_EXCEL_CACHE_SIZE=100
export XML_EXCEL_MAX_PARALLELISM=4

# 生产环境配置
export ASPNETCORE_ENVIRONMENT=Production
export XML_EXCEL_PLUGIN_DIRECTORY=/opt/plugins
export XML_EXCEL_CACHE_SIZE=500
export XML_EXCEL_MAX_PARALLELISM=8
```

### 3. 启动配置

```csharp
// 启动配置
public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddXmlExcelConversion();
                services.AddHostedService<ConversionBackgroundService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            })
            .Build();
        
        await host.RunAsync();
    }
}
```

## 部署和打包

### 1. 发布配置

```xml
<!-- 发布配置 -->
<PropertyGroup>
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <PublishTrimmed>true</PublishTrimmed>
  <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
  <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
  <DebugType>embedded</DebugType>
  <Optimize>true</Optimize>
  
  <!-- 平台特定配置 -->
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
  <PlatformTarget>x64</PlatformTarget>
</PropertyGroup>
```

### 2. Docker配置

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BannerlordModEditor.TUI.csproj", "."]
RUN dotnet restore "BannerlordModEditor.TUI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BannerlordModEditor.TUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BannerlordModEditor.TUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BannerlordModEditor.TUI.dll"]
```

### 3. Kubernetes配置

```yaml
# Kubernetes配置
apiVersion: apps/v1
kind: Deployment
metadata:
  name: xml-excel-converter
spec:
  replicas: 3
  selector:
    matchLabels:
      app: xml-excel-converter
  template:
    metadata:
      labels:
        app: xml-excel-converter
    spec:
      containers:
      - name: xml-excel-converter
        image: xml-excel-converter:latest
        ports:
        - containerPort: 80
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: XML_EXCEL_MAX_PARALLELISM
          value: "8"
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /health
            port: 80
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /ready
            port: 80
          initialDelaySeconds: 5
          periodSeconds: 5
```

## 安全考虑

### 1. 输入验证

```csharp
// 输入验证
public class InputValidator
{
    public static bool IsValidFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;
        
        try
        {
            var fullPath = Path.GetFullPath(path);
            var root = Path.GetPathRoot(fullPath);
            
            return !fullPath.Contains("..") && 
                   !fullPath.Contains("\\\\") && 
                   !string.IsNullOrEmpty(root);
        }
        catch
        {
            return false;
        }
    }
    
    public static bool IsValidXmlType(string xmlType)
    {
        return !string.IsNullOrWhiteSpace(xmlType) && 
               xmlType.All(char.IsLetterOrDigit) &&
               xmlType.Length <= 50;
    }
}
```

### 2. 文件系统安全

```csharp
// 文件系统安全
public class FileSystemSecurity
{
    public static bool HasAccessPermission(string path, FileAccess access)
    {
        try
        {
            using var stream = new FileStream(path, FileMode.Open, access, FileShare.None);
            stream.Dispose();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
    }
    
    public static bool IsPathSafe(string path)
    {
        try
        {
            var fullPath = Path.GetFullPath(path);
            return fullPath.StartsWith(Path.GetPathRoot(fullPath));
        }
        catch
        {
            return false;
        }
    }
}
```

### 3. 异常处理安全

```csharp
// 安全异常处理
public class SecureExceptionHandler
{
    public static ErrorResult HandleException(Exception ex)
    {
        var errorResult = new ErrorResult
        {
            ErrorId = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow,
            Exception = ex
        };
        
        // 记录安全信息
        if (ex is UnauthorizedAccessException)
        {
            errorResult.UserMessage = "访问被拒绝，请检查权限";
            errorResult.Severity = ErrorSeverity.High;
        }
        else if (ex is FileNotFoundException)
        {
            errorResult.UserMessage = "文件未找到";
            errorResult.Severity = ErrorSeverity.Medium;
        }
        else
        {
            errorResult.UserMessage = "处理过程中发生错误";
            errorResult.Severity = ErrorSeverity.Medium;
        }
        
        // 不暴露敏感信息
        errorResult.ErrorMessage = errorResult.UserMessage;
        
        return errorResult;
    }
}
```

## 总结

本技术栈决策为XML-Excel互转适配系统提供了完整的技术解决方案：

### 核心优势
1. **现代化技术栈**: 基于.NET 9和现代C#特性
2. **高性能**: 优化的XML和Excel处理，缓存和对象池
3. **可扩展**: 插件化架构和依赖注入
4. **可靠性**: 完善的错误处理和监控
5. **可测试**: 全面的测试框架和工具
6. **可部署**: 支持多种部署方式和容器化

### 关键技术选择
- **XML处理**: System.Xml.Serialization + System.Xml.Linq
- **Excel处理**: ClosedXML
- **缓存**: Microsoft.Extensions.Caching.Memory
- **依赖注入**: Microsoft.Extensions.DependencyInjection
- **日志**: Serilog
- **测试**: xUnit + Moq + FluentAssertions
- **性能**: BenchmarkDotNet

### 架构模式
- **类型化转换**: 基于DO/DTO架构
- **工厂模式**: 动态转换器选择
- **策略模式**: 不同的转换策略
- **观察者模式**: 进度和错误通知
- **对象池模式**: 性能优化

这个技术栈选择为系统提供了高性能、可靠性和可维护性，能够满足大规模XML-Excel转换的需求。