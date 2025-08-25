# XML适配状态检查工具技术栈

## 技术选型概述

本文档详细说明了XML适配状态检查工具的技术选型决策，包括核心框架、依赖库、工具链和基础设施的选择理由和配置。

## 核心技术栈

### 框架和运行时

| 技术 | 版本 | 选择理由 | 使用场景 |
|------|------|----------|----------|
| **.NET 9.0** | 9.0.0 | 最新稳定版本，性能优化，现代C#特性支持 | 整个应用程序运行时 |
| **C# 9.0** | 9.0 | 类型安全，模式匹配，记录类型，顶级程序 | 主要编程语言 |
| **System.CommandLine** | 2.0.0-beta4 | 现代命令行处理，支持复杂参数，自动生成帮助 | CLI界面和参数解析 |
| **System.Text.Json** | 9.0.0 | 高性能JSON处理，内置支持，无需额外依赖 | JSON序列化和配置 |

### 数据处理

| 技术 | 版本 | 选择理由 | 使用场景 |
|------|------|----------|----------|
| **System.Xml.Serialization** | 内置 | 与现有项目保持一致，成熟的XML处理 | XML序列化/反序列化 |
| **System.Xml.Linq** | 内置 | 现代XML查询和操作，LINQ支持 | XML文件分析和验证 |
| **CsvHelper** | 30.0.1 | 成熟的CSV处理库，支持复杂映射 | CSV格式报告生成 |
| **Markdig** | 0.31.0 | 高性能Markdown解析器，支持CommonMark | Markdown报告生成 |

### 用户界面

| 技术 | 版本 | 选择理由 | 使用场景 |
|------|------|----------|----------|
| **Spectre.Console** | 0.49.1 | 美观的控制台输出，支持表格、进度条、颜色丰富 | 命令行界面美化 |
| **System.CommandLine.DragonFruit** | 0.4.0-alpha | 简化命令行应用程序开发 | CLI快速开发 |

### 测试框架

| 技术 | 版本 | 选择理由 | 使用场景 |
|------|------|----------|----------|
| **xUnit** | 2.5.0 | 与现有项目保持一致，社区支持好 | 单元测试和集成测试 |
| **Moq** | 4.20.69 | 成熟的模拟框架，支持现代C#特性 | 单元测试模拟 |
| **FluentAssertions** | 6.12.0 | 流畅的断言语法，提高测试可读性 | 测试断言 |
| **coverlet.collector** | 6.0.0 | 代码覆盖率工具，集成良好 | 测试覆盖率分析 |

## 项目结构

```
BannerlordModEditor.XmlAdaptationChecker/
├── BannerlordModEditor.XmlAdaptationChecker.csproj  # 主项目文件
├── Program.cs                                       # 程序入口点
├── CheckCommand.cs                                  # 检查命令实现
├── ReportCommand.cs                                 # 报告命令实现
├── ConfigCommand.cs                                 # 配置命令实现
├── Services/                                        # 核心服务
│   ├── IXmlAdaptationChecker.cs                     # 检查服务接口
│   ├── XmlAdaptationChecker.cs                      # 检查服务实现
│   ├── IAdaptationReportGenerator.cs                # 报告生成器接口
│   ├── AdaptationReportGenerator.cs                 # 报告生成器实现
│   ├── IAdaptationAnalyzer.cs                       # 分析器接口
│   ├── AdaptationAnalyzer.cs                        # 分析器实现
│   └── AdaptationConfiguration.cs                   # 配置管理
├── Models/                                          # 数据模型
│   ├── AdaptationCheckRequest.cs                    # 检查请求模型
│   ├── AdaptationCheckResponse.cs                   # 检查响应模型
│   ├── AdaptationCheckResult.cs                     # 检查结果模型
│   ├── AdaptationStatistics.cs                      # 统计信息模型
│   └── UnadaptedFile.cs                             # 未适配文件模型
├── Formatters/                                      # 报告格式化器
│   ├── JsonReportFormatter.cs                       # JSON格式化器
│   ├── CsvReportFormatter.cs                        # CSV格式化器
│   ├── MarkdownReportFormatter.cs                   # Markdown格式化器
│   ├── HtmlReportFormatter.cs                       # HTML格式化器
│   └── ConsoleReportFormatter.cs                    # 控制台格式化器
├── Analyzers/                                       # 分析器组件
│   ├── ComplexityAnalyzer.cs                         # 复杂度分析器
│   ├── PriorityAnalyzer.cs                          # 优先级分析器
│   ├── DependencyAnalyzer.cs                        # 依赖分析器
│   └── BusinessImportanceAnalyzer.cs               # 业务重要性分析器
└── Extensions/                                      # 扩展方法
    ├── FileInfoExtensions.cs                        # 文件信息扩展
    ├── CollectionExtensions.cs                      # 集合扩展
    └── StringExtensions.cs                          # 字符串扩展
```

## NuGet包依赖

### 主要依赖

```xml
<PackageReference Include="System.CommandLine" Version="2.0.0-beta4" />
<PackageReference Include="Spectre.Console" Version="0.49.1" />
<PackageReference Include="CsvHelper" Version="30.0.1" />
<PackageReference Include="Markdig" Version="0.31.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

### 开发依赖

```xml
<PackageReference Include="xunit" Version="2.5.0" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
<PackageReference Include="Moq" Version="4.20.69" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

## 配置和部署

### 配置文件

```json
{
  "AdaptationCheckOptions": {
    "DefaultXmlDirectory": "example/ModuleData",
    "DefaultModelsDirectory": "BannerlordModEditor.Common/Models",
    "DefaultOutputFormat": "Console",
    "DefaultMinimumComplexity": "Simple",
    "Verbose": false,
    "ShowProgress": true,
    "MaxParallelFiles": 4,
    "TimeoutMinutes": 30,
    "EnableCaching": true,
    "LogLevel": "Information"
  },
  "Reporting": {
    "IncludeStatistics": true,
    "IncludeRecommendations": true,
    "MaxFilesInReport": 1000,
    "GenerateCharts": false
  },
  "Analysis": {
    "ComplexityThresholds": {
      "Simple": 10240,      // 10KB
      "Medium": 102400,     // 100KB
      "Complex": 1048576    // 1MB
    },
    "PriorityRules": {
      "BusinessCritical": ["native_parameters", "managed_core_parameters"],
      "HighPriority": ["combat_parameters", "item_modifiers"],
      "MediumPriority": ["particle_systems", "sounds"],
      "LowPriority": ["credits", "strings"]
    }
  }
}
```

### 部署配置

```xml
<!-- 单文件发布配置 -->
<PropertyGroup>
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <PublishTrimmed>true</PublishTrimmed>
  <TrimMode>partial</TrimMode>
  <PublishReadyToRun>true</PublishReadyToRun>
  <IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
</PropertyGroup>
```

## 性能优化技术

### 并行处理

```csharp
// 使用Parallel.ForEach进行并行处理
var parallelOptions = new ParallelOptions
{
    MaxDegreeOfParallelism = options.MaxParallelFiles,
    CancellationToken = cancellationToken
};

await Parallel.ForEachAsync(xmlFiles, parallelOptions, async (xmlFile, ct) =>
{
    var result = await ProcessSingleFileAsync(xmlFile, ct);
    // 处理结果...
});
```

### 内存优化

```csharp
// 使用流式处理避免大文件内存占用
using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
using var reader = XmlReader.Create(fileStream, new XmlReaderSettings
{
    IgnoreWhitespace = true,
    IgnoreComments = true,
    CloseInput = true
});

// 流式XML处理
while (await reader.ReadAsync())
{
    // 处理XML节点...
}
```

### 缓存策略

```csharp
// 内存缓存配置
services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024 * 1024 * 100; // 100MB
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(5);
});

// 缓存键策略
public static class CacheKeys
{
    public static string FileExists(string modelName, string directory) 
        => $"file_exists_{modelName}_{directory.GetHashCode()}";
    
    public static string NamingConvention(string fileName) 
        => $"naming_convention_{fileName}";
    
    public static string ComplexityAnalysis(string filePath) 
        => $"complexity_{filePath}_{new FileInfo(filePath).LastWriteTimeUtc.Ticks}";
}
```

## 错误处理和恢复

### 错误处理策略

```csharp
public class AdaptationCheckErrorHandler
{
    private readonly ILogger<AdaptationCheckErrorHandler> _logger;
    
    public async Task<CheckResult> HandleFileErrorAsync(string filePath, Exception ex)
    {
        _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
        
        return new CheckResult
        {
            FilePath = filePath,
            Status = CheckStatus.Error,
            Error = new CheckError
            {
                File = filePath,
                Error = ex.Message,
                StackTrace = ex.StackTrace,
                Timestamp = DateTime.UtcNow
            }
        };
    }
    
    public async Task<CheckResult> HandleFileWithRetryAsync(string filePath, Func<Task<CheckResult>> operation, int maxRetries = 3)
    {
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                _logger.LogWarning(ex, "Attempt {Attempt} failed for file: {FilePath}. Retrying...", attempt, filePath);
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }
        }
        
        // 最终尝试失败
        return await HandleFileErrorAsync(filePath, new Exception($"Failed after {maxRetries} attempts"));
    }
}
```

## 监控和日志

### 日志配置

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    },
    "Console": {
      "LogLevel": {
        "Default": "Information"
      }
    },
    "File": {
      "LogLevel": {
        "Default": "Warning"
      },
      "Path": "logs/adaptation-checker.log",
      "MaxFileSize": "10MB",
      "MaxFiles": 5
    }
  }
}
```

### 性能监控

```csharp
public class PerformanceMonitor
{
    private readonly ILogger<PerformanceMonitor> _logger;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    
    public void StartOperation(string operationName)
    {
        _stopwatch.Restart();
        _logger.LogInformation("Starting operation: {OperationName}", operationName);
    }
    
    public void EndOperation(string operationName)
    {
        _stopwatch.Stop();
        _logger.LogInformation("Completed operation: {OperationName} in {Duration}ms", 
            operationName, _stopwatch.ElapsedMilliseconds);
    }
    
    public async Task<T> MeasureAsync<T>(string operationName, Func<Task<T>> operation)
    {
        StartOperation(operationName);
        try
        {
            return await operation();
        }
        finally
        {
            EndOperation(operationName);
        }
    }
}
```

## 安全考虑

### 输入验证

```csharp
public class InputValidator
{
    public static void ValidateDirectoryPath(string path, string paramName)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Directory path cannot be empty", paramName);
        
        if (path.Contains(".."))
            throw new ArgumentException("Directory path cannot contain parent directory references", paramName);
        
        if (Path.IsPathRooted(path) && !path.StartsWith(Path.GetFullPath(path)))
            throw new ArgumentException("Invalid directory path", paramName);
    }
    
    public static void ValidateXmlFile(string filePath)
    {
        if (!filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("File must be an XML file", nameof(filePath));
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException("XML file not found", filePath);
    }
}
```

### 文件系统安全

```csharp
public class SecureFileAccess
{
    public static FileStream OpenFileReadSafely(string filePath)
    {
        // 验证文件路径
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);
        
        // 检查文件大小限制
        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Length > 100 * 1024 * 1024) // 100MB
            throw new IOException("File size exceeds limit");
        
        // 安全打开文件
        return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
    }
}
```

## 扩展性设计

### 插件架构

```csharp
public interface IReportFormatter
{
    bool CanFormat(ReportFormat format);
    Task<string> FormatReportAsync(AdaptationCheckResult result);
    string FileExtension { get; }
}

public interface IComplexityAnalyzer
{
    bool CanAnalyze(string filePath);
    Task<AdaptationComplexity> AnalyzeComplexityAsync(string filePath);
}

// 插件注册
public class PluginRegistry
{
    private readonly List<IReportFormatter> _formatters = new();
    private readonly List<IComplexityAnalyzer> _analyzers = new();
    
    public void RegisterFormatter(IReportFormatter formatter)
    {
        _formatters.Add(formatter);
    }
    
    public void RegisterAnalyzer(IComplexityAnalyzer analyzer)
    {
        _analyzers.Add(analyzer);
    }
    
    public IReportFormatter GetFormatter(ReportFormat format)
    {
        return _formatters.FirstOrDefault(f => f.CanFormat(format)) 
            ?? throw new NotSupportedException($"Format {format} is not supported");
    }
}
```

## 开发工作流

### 本地开发

```bash
# 克隆项目
git clone <repository-url>
cd BannerlordModEditor-XML-Adaptation

# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行测试
dotnet test

# 运行检查工具
dotnet run --project BannerlordModEditor.XmlAdaptationChecker -- check --xml-dir "example/ModuleData"
```

### CI/CD Pipeline

```yaml
# .github/workflows/build.yml
name: Build and Test
on: [push, pull_request]
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0.x'
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal
    - name: Publish
      run: dotnet publish -c Release -o publish
```

## 结论

XML适配状态检查工具的技术选型基于以下原则：

1. **一致性**: 与现有BannerlordModEditor项目保持技术栈一致
2. **性能**: 优先选择高性能的库和优化策略
3. **可维护性**: 选择成熟、文档完善的库
4. **扩展性**: 采用插件化架构支持未来扩展
5. **安全性**: 实现输入验证和安全访问控制
6. **开发体验**: 提供良好的开发工具和调试支持

这个技术栈组合确保了工具的高效性、可靠性和可扩展性，能够满足XML适配状态检查的各种需求。