# 测试故障排除指南

## 概述

本指南提供了Bannerlord Mod Editor UI测试套件的常见问题诊断和解决方案。

## 快速诊断清单

在深入排查问题之前，请先检查以下基本项目：

1. ✅ .NET 9.0 SDK已正确安装
2. ✅ 项目依赖已正确还原
3. ✅ 测试数据已同步
4. ✅ 测试配置文件存在且格式正确
5. ✅ 文件权限设置正确
6. ✅ 环境变量配置正确

## 常见错误及解决方案

### 1. 依赖注入相关错误

#### 错误：`InvalidOperationException: Unable to resolve service type`

**症状**：
```
System.InvalidOperationException: Unable to resolve service for type 'BannerlordModEditor.UI.Services.ILogService'
```

**可能原因**：
- 服务未在`TestServiceProvider`中注册
- 服务注册顺序错误
- 服务生命周期配置错误

**解决方案**：
```csharp
// 重置服务提供者
TestServiceProvider.Reset();

// 验证服务配置
var isValid = TestServiceProvider.ValidateConfiguration();
if (!isValid)
{
    throw new InvalidOperationException("TestServiceProvider配置验证失败");
}

// 获取服务
var service = TestServiceProvider.GetService<ILogService>();
```

#### 错误：`TestServiceProvider配置验证失败`

**症状**：
```
Assert.True() Failure
Expected: True
Actual: False
TestServiceProvider配置验证失败
```

**可能原因**：
- 必需服务未注册
- 服务类型不匹配
- 服务依赖关系错误

**解决方案**：
```csharp
// 检查服务注册
var services = new ServiceCollection();
services.AddSingleton<ILogService, LogService>();
services.AddSingleton<IErrorHandlerService, ErrorHandlerService>();
// ... 其他服务注册

// 验证所有必需服务
var serviceProvider = services.BuildServiceProvider();
var requiredServices = new[]
{
    typeof(ILogService),
    typeof(IErrorHandlerService),
    typeof(IValidationService),
    typeof(IDataBindingService),
    typeof(IEditorFactory)
};

foreach (var serviceType in requiredServices)
{
    var service = serviceProvider.GetService(serviceType);
    if (service == null)
    {
        throw new InvalidOperationException($"缺少必需服务: {serviceType.Name}");
    }
}
```

### 2. 测试数据相关错误

#### 错误：`FileNotFoundException: Could not find file`

**症状**：
```
System.IO.FileNotFoundException: Could not find file '/path/to/test/file.xml'
```

**可能原因**：
- 测试数据文件不存在
- 文件路径错误
- 文件权限不足

**解决方案**：
```bash
# 同步测试数据
./Sync-TestData.sh

# 验证文件存在
ls -la TestData/
ls -la BannerlordModEditor.UI.Tests/TestData/

# 检查文件权限
chmod 644 TestData/*.xml
```

#### 错误：`TestDataHelper.TestDataFileExists返回false`

**症状**：
```csharp
Assert.True() Failure
Expected: True
Actual: False
```

**可能原因**：
- 测试数据目录路径错误
- 文件同步失败
- 相对路径解析错误

**解决方案**：
```csharp
// 检查测试数据目录
var testDataDir = TestDataHelper.TestDataDirectory;
Console.WriteLine($"测试数据目录: {testDataDir}");
Console.WriteLine($"目录存在: {Directory.Exists(testDataDir)}");

// 列出所有测试数据文件
var files = TestDataHelper.ListTestDataFiles();
Console.WriteLine($"测试数据文件数量: {files.Count()}");
foreach (var file in files)
{
    Console.WriteLine($"- {file}");
}

// 检查特定文件
var testFile = "test.xml";
var exists = TestDataHelper.TestDataFileExists(testFile);
Console.WriteLine($"文件 {testFile} 存在: {exists}");
```

### 3. 并行测试相关错误

#### 错误：`测试并行执行导致资源冲突`

**症状**：
- 测试随机失败
- 文件访问冲突
- 状态不一致

**可能原因**：
- 共享资源访问冲突
- 测试间状态污染
- 文件系统并发问题

**解决方案**：
```json
// 在test-run-settings.json中配置
{
  "parallelExecution": {
    "enabled": true,
    "maxParallelThreads": 2,
    "disableParallelizationFor": [
      "GitHubActionsEnvironmentTests",
      "CrossPlatformCompatibilityTests",
      "TestDeploymentVerificationTests"
    ]
  }
}
```

```csharp
// 在测试中确保资源隔离
public class MyTests : IDisposable
{
    private readonly string _testDirectory;
    
    public MyTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
    }
    
    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}
```

### 4. 超时相关错误

#### 错误：`测试执行超时`

**症状**：
```
测试执行时间超过阈值 (30000ms)
```

**可能原因**：
- 测试逻辑过于复杂
- I/O操作耗时过长
- 网络请求超时

**解决方案**：
```json
// 在test-run-settings.json中调整超时设置
{
  "timeout": {
    "default": 60000,
    "longRunning": 180000,
    "perTest": {
      "GitHubActionsEnvironmentTests": 90000,
      "CrossPlatformCompatibilityTests": 60000
    }
  }
}
```

```csharp
// 优化测试性能
[Fact]
public async Task SlowOperation_Should_Complete_Within_Timeout()
{
    // 设置较短的本地超时用于测试
    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    
    try
    {
        await SomeLongRunningOperation(cts.Token);
    }
    catch (OperationCanceledException)
    {
        Assert.True(false, "操作超时");
    }
}
```

### 5. 环境相关错误

#### 错误：`GitHub Actions环境检测失败`

**症状**：
```
Assert.True() Failure
Expected: True
Actual: False
GitHub Actions环境检测失败
```

**可能原因**：
- CI环境变量未设置
- 环境检测逻辑错误
- 权限不足

**解决方案**：
```csharp
// 检查环境变量
var envVars = new[]
{
    "GITHUB_ACTIONS",
    "CI",
    "GITHUB_WORKSPACE",
    "GITHUB_RUN_ID"
};

foreach (var varName in envVars)
{
    var value = Environment.GetEnvironmentVariable(varName);
    Console.WriteLine($"{varName}: {value ?? "null"}");
}

// 手动设置环境变量用于测试
Environment.SetEnvironmentVariable("GITHUB_ACTIONS", "true");
Environment.SetEnvironmentVariable("CI", "true");
```

### 6. 权限相关错误

#### 错误：`UnauthorizedAccessException`

**症状**：
```
System.UnauthorizedAccessException: Access to the path '/path/to/file' is denied
```

**可能原因**：
- 文件权限不足
- 目录权限不足
- 用户权限限制

**解决方案**：
```bash
# 检查和修复权限
ls -la /path/to/file
chmod 644 /path/to/file
chmod 755 /path/to/directory

# 检查用户权限
whoami
id -u
```

```csharp
// 在代码中处理权限错误
try
{
    File.WriteAllText(filePath, content);
}
catch (UnauthorizedAccessException ex)
{
    // 使用临时目录
    var tempPath = Path.GetTempFileName();
    File.WriteAllText(tempPath, content);
    // 使用临时文件...
}
```

### 7. 内存相关错误

#### 错误：`OutOfMemoryException`

**症状**：
```
System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown
```

**可能原因**：
- 测试数据过大
- 内存泄漏
- 并发测试内存使用过高

**解决方案**：
```json
// 在test-run-settings.json中设置内存限制
{
  "performance": {
    "maxMemoryUsage": "512MB",
    "enableMemoryMonitoring": true,
    "enableGCMonitoring": true
  }
}
```

```csharp
// 优化内存使用
[Fact]
public void LargeDataOperation_Should_Not_Overflow_Memory()
{
    // 使用流式处理而不是加载所有数据
    using var stream = File.OpenRead("large_file.xml");
    using var reader = new StreamReader(stream);
    
    // 逐行处理而不是一次性读取
    string line;
    while ((line = reader.ReadLine()) != null)
    {
        // 处理每一行
    }
}
```

### 8. 网络相关错误

#### 错误：`网络请求超时或失败`

**症状**：
```
System.Net.Http.HttpRequestException: Connection timeout
```

**可能原因**：
- 网络连接问题
- 防火墙阻止
- DNS解析失败

**解决方案**：
```csharp
// 配置HTTP客户端超时
var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

// 使用重试机制
var policy = Policy
    .Handle<HttpRequestException>()
    .Or<TaskCanceledException>()
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
```

## 调试技巧

### 1. 启用详细日志

```json
{
  "logging": {
    "level": "Debug",
    "enableConsoleOutput": true,
    "enableFileOutput": true,
    "logFile": "debug-test.log"
  }
}
```

### 2. 使用断点调试

```csharp
[Fact]
public void Debuggable_Test()
{
    // 设置断点
    var serviceProvider = TestServiceProvider.GetServiceProvider();
    var service = serviceProvider.GetService<ILogService>();
    
    // 检查变量状态
    Assert.NotNull(service);
    Console.WriteLine($"服务类型: {service.GetType().Name}");
}
```

### 3. 条件断点

```csharp
[Fact]
public void Conditional_Debug_Test()
{
    for (int i = 0; i < 100; i++)
    {
        var result = SomeOperation(i);
        
        // 只在特定条件下触发断点
        if (result == null)
        {
            Debugger.Break();
        }
    }
}
```

### 4. 内存分析

```csharp
[Fact]
public void Memory_Analysis_Test()
{
    var initialMemory = GC.GetTotalMemory(false);
    
    // 执行操作
    var result = SomeMemoryIntensiveOperation();
    
    var finalMemory = GC.GetTotalMemory(false);
    var memoryIncrease = finalMemory - initialMemory;
    
    Console.WriteLine($"内存使用增加: {memoryIncrease / 1024}KB");
    
    // 强制垃圾回收
    GC.Collect();
    GC.WaitForPendingFinalizers();
    
    var afterGCMemory = GC.GetTotalMemory(false);
    Console.WriteLine($"GC后内存使用: {afterGCMemory / 1024}KB");
}
```

## 性能优化

### 1. 测试缓存

```json
{
  "optimizations": {
    "enableTestCaching": true,
    "enableAssemblyCaching": true,
    "cacheDirectory": ".test-cache"
  }
}
```

### 2. 并行执行优化

```csharp
// 使用集合级别的并行控制
[assembly: CollectionBehavior(DisableTestParallelization = false)]
[assembly: TestCollectionOrderer("BannerlordModEditor.UI.Tests.Helpers.PriorityOrderer")]

public class PriorityOrderer : ITestCollectionOrderer
{
    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
    {
        return testCollections.OrderBy(collection => 
        {
            var name = collection.DisplayName;
            if (name.Contains("Integration")) return 1;
            if (name.Contains("Environment")) return 2;
            return 0;
        });
    }
}
```

### 3. 懒加载

```csharp
public class LazyTests
{
    private readonly Lazy<IServiceProvider> _lazyServiceProvider;
    
    public LazyTests()
    {
        _lazyServiceProvider = new Lazy<IServiceProvider>(() => 
            TestServiceProvider.GetServiceProvider());
    }
    
    [Fact]
    public void Lazy_Service_Resolution()
    {
        var serviceProvider = _lazyServiceProvider.Value;
        var service = serviceProvider.GetService<ILogService>();
        Assert.NotNull(service);
    }
}
```

## 环境特定问题

### 1. Windows环境

#### 问题：路径分隔符不一致

**解决方案**：
```csharp
public static string NormalizePath(string path)
{
    if (string.IsNullOrEmpty(path)) return path;
    return path.Replace('\\', Path.DirectorySeparatorChar)
               .Replace('/', Path.DirectorySeparatorChar);
}
```

### 2. Linux环境

#### 问题：文件权限问题

**解决方案**：
```bash
# 设置执行权限
chmod +x *.sh
chmod 755 TestData/

# 检查权限
ls -la
```

### 3. macOS环境

#### 问题：Case Sensitivity

**解决方案**：
```csharp
// 使用StringComparison.OrdinalIgnoreCase进行文件比较
var files = Directory.GetFiles(directory, "test.xml", SearchOption.AllDirectories);
var foundFile = files.FirstOrDefault(f => 
    f.Equals("test.xml", StringComparison.OrdinalIgnoreCase));
```

## CI/CD特定问题

### 1. GitHub Actions

#### 问题：测试数据同步失败

**解决方案**：
```yaml
- name: Sync test data
  run: |
    chmod +x ./Sync-TestData.sh
    ./Sync-TestData.sh
  env:
    GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

### 2. Azure DevOps

#### 问题：环境变量传递

**解决方案**：
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'test'
    projects: '**/*Tests.csproj'
    arguments: '--configuration Release --environment Testing'
  env:
    TEST_MODE: 'true'
    LOG_LEVEL: 'Debug'
```

## 最佳实践

### 1. 测试隔离

```csharp
public class IsolatedTests : IDisposable
{
    private readonly string _testRoot;
    private readonly IServiceProvider _serviceProvider;
    
    public IsolatedTests()
    {
        _testRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testRoot);
        
        _serviceProvider = CreateTestServiceProvider();
    }
    
    public void Dispose()
    {
        if (Directory.Exists(_testRoot))
        {
            Directory.Delete(_testRoot, true);
        }
    }
    
    private IServiceProvider CreateTestServiceProvider()
    {
        var services = new ServiceCollection();
        // 注册服务
        return services.BuildServiceProvider();
    }
}
```

### 2. 异常处理

```csharp
[Fact]
public async Task Robust_Error_Handling()
{
    try
    {
        await SomeRiskyOperation();
    }
    catch (SpecificException ex)
    {
        // 记录详细信息
        Console.WriteLine($"特定异常: {ex.Message}");
        Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
        
        // 验证异常属性
        Assert.NotNull(ex.InnerException);
        Assert.Contains("expected", ex.Message);
    }
    catch (Exception ex)
    {
        Assert.Fail($"未预期的异常类型: {ex.GetType().Name}");
    }
}
```

### 3. 断言优化

```csharp
[Fact]
public void Meaningful_Assertions()
{
    var result = SomeOperation();
    
    // 提供有意义的错误消息
    Assert.True(result.IsValid, 
        $"操作结果无效。错误: {result.ErrorMessage}");
    
    Assert.Equal("expected", result.Value, 
        $"值不匹配。期望: expected, 实际: {result.Value}");
    
    Assert.NotNull(result.Items, 
        $"结果项集合为null");
    
    Assert.NotEmpty(result.Items, 
        $"结果项集合为空");
}
```

## 获取帮助

### 1. 日志分析

```bash
# 查看详细日志
tail -f test-results.log

# 搜索错误
grep -i error test-results.log

# 分析性能
grep "Performance Metric" test-results.log
```

### 2. 社区资源

- [GitHub Issues](https://github.com/your-repo/issues)
- [Stack Overflow](https://stackoverflow.com/questions/tagged/bannerlord-mod-editor)
- [.NET Testing Documentation](https://docs.microsoft.com/en-us/dotnet/core/testing/)

### 3. 联系信息

- 项目维护者：[your-email@example.com]
- 技术支持：[support@example.com]
- 社区论坛：[forum.example.com]

## 总结

本故障排除指南提供了Bannerlord Mod Editor UI测试套件的常见问题诊断和解决方案。通过遵循本指南中的建议，您应该能够快速识别和解决大多数测试相关的问题。

记住，测试问题的诊断通常需要耐心和系统性。从基本的检查开始，然后逐步深入到更复杂的调试技术。祝您测试顺利！