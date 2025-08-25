# 技术栈选择和配置

## 概述

本文档详细描述了Bannerlord Mod Editor项目的技术栈选择和配置。该系统采用现代化的.NET 9和Avalonia UI技术栈，结合MVVM架构模式，实现了一个跨平台、高性能的桌面应用程序。

## 核心技术栈

### .NET 9 和 C#
| 技术 | 版本 | 选择理由 | 主要用途 |
|------|------|----------|----------|
| .NET 9 | 9.0 | 最新LTS版本，性能优异，现代语言特性 | 应用程序运行时 |
| C# | 12.0 | 现代语言特性，性能优化，类型安全 | 主要开发语言 |
| LINQ | 内置 | 强大的数据查询能力 | 数据处理 |
| async/await | 内置 | 异步编程支持 | I/O操作 |

### 为什么选择.NET 9？
- **性能**: 显著的性能改进和内存优化
- **现代化**: 最新的语言特性和API
- **跨平台**: 原生支持Windows、macOS、Linux
- **生态系统**: 丰富的库和工具支持
- **LTS支持**: 长期支持版本

### Avalonia UI 11.3
| 特性 | 配置 | 选择理由 |
|------|------|----------|
| 框架 | Avalonia UI 11.3 | 跨平台UI框架，与WPF相似的API |
| 主题 | Fluent主题 | 现代化UI设计 |
| 渲染 | Skia | 高性能2D渲染 |
| 数据绑定 | XAML + 代码 | 强大的数据绑定能力 |
| 控件 | 内置 + 自定义 | 丰富的控件库 |

### 为什么选择Avalonia UI？
- **跨平台**: 单一代码库支持多平台
- **性能**: 高性能渲染引擎
- **MVVM支持**: 原生MVVM模式支持
- **XAML**: 成熟的标记语言
- **社区**: 活跃的开源社区

## MVVM框架配置

### CommunityToolkit.Mvvm 8.2
```xml
<!-- 包引用 -->
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.1" />
```

### 主要特性
- **源生成器**: 编译时生成代码，零运行时开销
- **ObservableObject**: 简化INotifyPropertyChanged实现
- **RelayCommand**: 简化ICommand实现
- **依赖注入**: 内置DI容器支持

### MVVM架构配置
```csharp
// ViewModel基类
public abstract class ViewModelBase : ObservableObject
{
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }
    
    protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        return base.SetProperty(ref field, value, propertyName);
    }
}

// 带命令的ViewModel
public partial class EditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _title = string.Empty;
    
    [RelayCommand]
    private void Save()
    {
        // 保存逻辑
    }
    
    [RelayCommand(CanExecute = nameof(CanSave))]
    private void SaveAs()
    {
        // 另存为逻辑
    }
    
    private bool CanSave() => !string.IsNullOrEmpty(Title);
}
```

## 依赖注入配置

### Microsoft.Extensions.DependencyInjection 8.0
```xml
<!-- 包引用 -->
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" />
```

### 服务注册配置
```csharp
// Program.cs 或 App.axaml.cs
public static class ServiceConfigurator
{
    public static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        
        // 注册核心服务
        services.AddSingleton<IFileDiscoveryService, FileDiscoveryService>();
        services.AddSingleton<IXmlProcessingService, XmlProcessingService>();
        services.AddSingleton<IMappingService, MappingService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IEventBus, EventBus>();
        services.AddSingleton<IEditorService, EditorService>();
        services.AddSingleton<IPluginManager, PluginManager>();
        
        // 注册编辑器工厂
        services.AddSingleton<IEditorFactory, EditorFactory>();
        
        // 注册ViewModel
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<EditorManagerViewModel>();
        services.AddTransient<AttributeEditorViewModel>();
        services.AddTransient<SkillEditorViewModel>();
        
        // 注册自定义编辑器
        RegisterCustomEditors(services);
        
        return services.BuildServiceProvider();
    }
    
    private static void RegisterCustomEditors(IServiceCollection services)
    {
        // 通过反射注册所有编辑器
        var editorTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                       t.IsSubclassOf(typeof(BaseEditorViewModel)));
        
        foreach (var editorType in editorTypes)
        {
            services.AddTransient(editorType);
        }
    }
}
```

### 生命周期管理
- **Singleton**: 单例模式，全局唯一实例
- **Scoped**: 作用域模式，每个作用域一个实例
- **Transient**: 瞬态模式，每次请求创建新实例

## XML处理配置

### System.Xml.Serialization
```csharp
// 增强的XML处理器
public class EnhancedXmlProcessor : IXmlProcessingService
{
    private readonly XmlSerializerSettings _settings;
    
    public EnhancedXmlProcessor()
    {
        _settings = new XmlSerializerSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            OmitXmlDeclaration = false,
            WriteEmptyNamespaces = false
        };
    }
    
    public async Task<T> LoadXmlAsync<T>(string filePath) where T : class, new()
    {
        var xml = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        return DeserializeXml<T>(xml);
    }
    
    public T DeserializeXml<T>(string xml) where T : class, new()
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xml);
        return (T)serializer.Deserialize(reader);
    }
    
    public string SerializeXml<T>(T data) where T : class, new()
    {
        var serializer = new XmlSerializer(typeof(T));
        using var writer = new StringWriter();
        serializer.Serialize(writer, data);
        return writer.ToString();
    }
}
```

### DO/DTO映射配置
```csharp
// 映射服务配置
public class MappingService : IMappingService
{
    private readonly Dictionary<(Type, Type), object> _mappers = new();
    
    public MappingService()
    {
        // 注册默认映射器
        RegisterDefaultMappers();
    }
    
    private void RegisterDefaultMappers()
    {
        // 注册项目映射器
        RegisterMapper<ItemMapper>();
        RegisterMapper<AttributeMapper>();
        RegisterMapper<SkillMapper>();
        
        // 通过反射注册所有映射器
        RegisterMappersByReflection();
    }
    
    private void RegisterMapper<T>() where T : class, new()
    {
        var mapperType = typeof(T);
        var interfaces = mapperType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>));
        
        foreach (var interfaceType in interfaces)
        {
            var genericArgs = interfaceType.GetGenericArguments();
            var key = (genericArgs[0], genericArgs[1]);
            _mappers[key] = new T();
        }
    }
}
```

## 测试框架配置

### xUnit 2.5
```xml
<!-- 测试项目包引用 -->
<PackageReference Include="xunit" Version="2.5.0" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.0" />
<PackageReference Include="xunit.extensibility.core" Version="2.5.0" />
<PackageReference Include="coverlet.collector" Version="6.0.0" />
```

### 测试配置
```csharp
// 测试基类
public class TestBase : IDisposable
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ITestOutputHelper Output;
    
    protected TestBase(ITestOutputHelper output)
    {
        Output = output;
        ServiceProvider = ServiceConfigurator.ConfigureServices();
    }
    
    protected T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    
    public virtual void Dispose()
    {
        // 清理资源
    }
}

// XML测试工具
public static class XmlTestUtils
{
    public static async Task<T> LoadTestXml<T>(string testName) where T : class, new()
    {
        var testPath = GetTestXmlPath(testName);
        var xml = await File.ReadAllTextAsync(testPath);
        var processor = new EnhancedXmlProcessor();
        return processor.DeserializeXml<T>(xml);
    }
    
    public static void AssertXmlEqual(string xml1, string xml2)
    {
        var doc1 = XDocument.Parse(xml1);
        var doc2 = XDocument.Parse(xml2);
        
        // 比较XML结构
        Assert.True(XNode.DeepEquals(doc1, doc2), "XML结构不匹配");
    }
    
    public static void AssertXmlStructureEqual(string xml1, string xml2)
    {
        var result = CompareXmlStructures(xml1, xml2);
        Assert.True(result.IsStructurallyEqual, 
            $"XML结构不匹配: {string.Join(", ", result.StructureDifferences)}");
    }
}
```

## Avalonia UI测试配置

### Avalonia.TestUtilities
```xml
<!-- UI测试包引用 -->
<PackageReference Include="Avalonia.TestUtilities" Version="11.3.0" />
<PackageReference Include="Avalonia.Controls.DataGrid" Version="11.3.0" />
```

### UI测试配置
```csharp
// UI测试基类
public class UiTestBase : TestBase
{
    protected AppBuilder AppBuilder;
    protected Window Window;
    
    protected UiTestBase(ITestOutputHelper output) : base(output)
    {
        AppBuilder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
    
    protected async Task<T> CreateWindow<T>(Action<T> configure = null) 
        where T : Window, new()
    {
        Window = new T();
        configure?.Invoke((T)Window);
        
        await AppBuilder.Start(AppMain);
        
        return (T)Window;
    }
    
    private void AppMain(Application app, string[] args)
    {
        app.Styles.Add(new FluentTheme());
        app.Resources.Add("Services", ServiceProvider);
        
        Window.Show();
        app.Run(Window);
    }
}

// ViewModel测试
public class EditorViewModelTests : UiTestBase
{
    [Fact]
    public async Task LoadXmlFile_ShouldLoadData()
    {
        // Arrange
        var viewModel = new AttributeEditorViewModel(
            GetService<IXmlProcessingService>(),
            GetService<IMappingService>());
        
        // Act
        await viewModel.LoadXmlFileAsync("test_attributes.xml");
        
        // Assert
        Assert.False(viewModel.IsLoading);
        Assert.NotEmpty(viewModel.Attributes);
        Assert.False(viewModel.HasUnsavedChanges);
    }
}
```

## 构建和部署配置

### 项目配置
```xml
<!-- BannerlordModEditor.UI.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <AvaloniaUseCompiledBindingsByDefault>false</AvaloniaUseCompiledBindingsByDefault>
    <AssemblyName>BannerlordModEditor</AssemblyName>
    <WarningsAsErrors />
    <NoWarn>$(NoWarn);CS8618;CS8603;CS8604;CS8602;CS8600;CS8625</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Avalonia" Version="11.3.0" />
    <PackageReference Include="Avalonia.Desktop" Version="11.3.0" />
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.3.0" />
    <PackageReference Include="Avalonia.Fonts.Inter" Version="11.3.0" />
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.1" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Http" Version="8.0.0" />
    <PackageReference Include="Velopack" Version="0.0.1298" />
  </ItemGroup>
  
  <!-- Linux支持 -->
  <ItemGroup Condition="'$([MSBuild]::IsOSPlatform(Linux))' == 'true'">
    <PackageReference Include="Avalonia.FreeDesktop" Version="11.3.0" />
  </ItemGroup>
  
  <!-- 调试工具 -->
  <ItemGroup Condition="'$(Configuration)' == 'Debug'">
    <PackageReference Include="Avalonia.Diagnostics" Version="11.3.0" />
  </ItemGroup>
</Project>
```

### 发布配置
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
</PropertyGroup>
```

### Velopack配置
```csharp
// Program.cs
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        VelopackApp.Build()
            .WithFirstRun(v => v.Notify("Welcome to Bannerlord Mod Editor!"))
            .Run();
            
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}
```

## 开发工具配置

### 代码分析配置
```xml
<!-- 代码分析规则 -->
<PropertyGroup>
  <EnableNETAnalyzers>true</EnableNETAnalyzers>
  <AnalysisLevel>latest</AnalysisLevel>
  <AnalysisMode>All</AnalysisMode>
  <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
</PropertyGroup>

<!-- 代码风格 -->
<PropertyGroup>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
  <WarningsAsErrors>CS8600,CS8602,CS8603,CS8604,CS8618,CS8625</WarningsAsErrors>
</PropertyGroup>
```

### 调试配置
```xml
<!-- 调试配置 -->
<PropertyGroup Condition="'$(Configuration)' == 'Debug'">
  <DefineConstants>DEBUG;TRACE</DefineConstants>
  <DebugType>full</DebugType>
  <DebugSymbols>true</DebugSymbols>
  <Optimize>false</Optimize>
</PropertyGroup>

<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <DefineConstants>TRACE</DefineConstants>
  <DebugType>embedded</DebugType>
  <Optimize>true</Optimize>
</PropertyGroup>
```

## 性能优化配置

### 内存优化
```csharp
// 对象池配置
public static class ObjectPoolConfigurator
{
    public static void ConfigurePools(IServiceCollection services)
    {
        // 配置XML处理器对象池
        services.AddSingleton<ObjectPool<XmlSerializer>>(provider => 
            new DefaultObjectPool<XmlSerializer>(
                new DefaultPooledObjectPolicy<XmlSerializer>(), 
                10));
        
        // 配置映射器对象池
        services.AddSingleton<ObjectPool<IMapper>>(provider => 
            new DefaultObjectPool<IMapper>(
                new DefaultPooledObjectPolicy<IMapper>(), 
                20));
    }
}

// 缓存配置
public static class CacheConfigurator
{
    public static void ConfigureCache(IServiceCollection services)
    {
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1024 * 1024 * 100; // 100MB
            options.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
        });
        
        services.AddDistributedMemoryCache();
    }
}
```

### 异步配置
```csharp
// 异步处理配置
public static class AsyncConfigurator
{
    public static void ConfigureAsync(IServiceCollection services)
    {
        // 配置异步HTTP客户端
        services.AddHttpClient("XmlProcessor", client =>
        {
            client.BaseAddress = new Uri("https://api.example.com/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        
        // 配置异步任务调度器
        services.AddSingleton<TaskScheduler>(provider =>
            new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler);
    }
}
```

## 安全配置

### 安全最佳实践
```csharp
// 安全配置
public static class SecurityConfigurator
{
    public static void ConfigureSecurity(IServiceCollection services)
    {
        // 配置XML安全设置
        services.AddSingleton<XmlReaderSettings>(provider => 
            new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                MaxCharactersFromEntities = 1024,
                MaxCharactersInDocument = 1024 * 1024 * 10, // 10MB
                IgnoreWhitespace = true,
                IgnoreComments = true
            });
        
        // 配置文件路径验证
        services.AddSingleton<IPathValidator, PathValidator>();
    }
}

// 路径验证器
public class PathValidator : IPathValidator
{
    public bool IsValidPath(string path)
    {
        try
        {
            var fullPath = Path.GetFullPath(path);
            var root = Path.GetPathRoot(fullPath);
            
            // 防止目录遍历攻击
            return !fullPath.Contains("..") && 
                   !fullPath.Contains("\\\\") && 
                   !string.IsNullOrEmpty(root);
        }
        catch
        {
            return false;
        }
    }
}
```

## 监控和诊断配置

### 日志配置
```csharp
// 日志配置
public static class LoggingConfigurator
{
    public static void ConfigureLogging(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.AddFile("logs/app.log", minimumLevel: LogLevel.Information);
            
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddFilter("Microsoft", LogLevel.Warning);
            builder.AddFilter("System", LogLevel.Warning);
        });
    }
}
```

### 性能监控
```csharp
// 性能监控配置
public static class MonitoringConfigurator
{
    public static void ConfigureMonitoring(IServiceCollection services)
    {
        // 性能计数器
        services.AddSingleton<PerformanceCounter>();
        
        // 内存监控
        services.AddSingleton<MemoryMonitor>();
        
        // 诊断服务
        services.AddSingleton<IDiagnosticService, DiagnosticService>();
    }
}

// 性能计数器
public class PerformanceCounter
{
    private readonly Stopwatch _stopwatch = new();
    private long _operations;
    private long _totalTime;
    
    public void StartOperation()
    {
        _stopwatch.Restart();
    }
    
    public void EndOperation()
    {
        _stopwatch.Stop();
        Interlocked.Increment(ref _operations);
        Interlocked.Add(ref _totalTime, _stopwatch.ElapsedTicks);
    }
    
    public double AverageTime => _operations > 0 ? 
        TimeSpan.FromTicks(_totalTime / _operations).TotalMilliseconds : 0;
}
```

## 总结

本技术栈配置提供了Bannerlord Mod Editor项目的完整技术解决方案：

1. **现代化技术栈**: .NET 9 + Avalonia UI + MVVM
2. **高性能**: 异步处理、对象池、缓存优化
3. **可测试性**: 完整的单元测试和UI测试框架
4. **可扩展性**: 插件化架构、依赖注入
5. **安全性**: 输入验证、路径安全、XML安全
6. **可维护性**: 代码分析、日志记录、性能监控

这个技术栈选择为项目提供了坚实的技术基础，支持快速开发和长期维护。