# API 规范文档

## 概述

本文档定义了修复GitHub Actions UI测试失败问题所需的API接口规范。这些接口主要用于测试数据管理、配置服务和验证服务。

## 目录

1. [测试数据访问接口](#测试数据访问接口)
2. [配置管理接口](#配置管理接口)
3. [验证服务接口](#验证服务接口)
4. [同步服务接口](#同步服务接口)
5. [监控和诊断接口](#监控和诊断接口)
6. [错误处理接口](#错误处理接口)

## 测试数据访问接口

### ITestDataRepository

```csharp
/// <summary>
/// 测试数据仓库接口
/// </summary>
public interface ITestDataRepository
{
    /// <summary>
    /// 获取指定路径的测试数据文件内容
    /// </summary>
    /// <param name="relativePath">相对于TestData目录的路径</param>
    /// <returns>文件内容字符串</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出</exception>
    /// <exception cref="UnauthorizedAccessException">访问被拒绝时抛出</exception>
    Task<string> GetTestDataAsync(string relativePath);
    
    /// <summary>
    /// 获取指定路径的测试数据文件流
    /// </summary>
    /// <param name="relativePath">相对于TestData目录的路径</param>
    /// <returns>文件流</returns>
    /// <exception cref="FileNotFoundException">文件不存在时抛出</exception>
    /// <exception cref="UnauthorizedAccessException">访问被拒绝时抛出</exception>
    Task<Stream> GetTestDataStreamAsync(string relativePath);
    
    /// <summary>
    /// 验证测试数据文件是否存在
    /// </summary>
    /// <param name="relativePath">相对于TestData目录的路径</param>
    /// <returns>文件是否存在</returns>
    bool TestDataExists(string relativePath);
    
    /// <summary>
    /// 获取所有测试数据文件列表
    /// </summary>
    /// <param name="searchPattern">搜索模式（可选）</param>
    /// <returns>文件路径列表</returns>
    Task<List<string>> GetAllTestDataFilesAsync(string searchPattern = "*.*");
    
    /// <summary>
    /// 获取测试数据目录路径
    /// </summary>
    /// <returns>测试数据目录路径</returns>
    string GetTestDataPath();
    
    /// <summary>
    /// 获取测试数据文件信息
    /// </summary>
    /// <param name="relativePath">相对于TestData目录的路径</param>
    /// <returns>文件信息</returns>
    Task<FileInfo> GetTestDataFileInfoAsync(string relativePath);
    
    /// <summary>
    /// 复制测试数据到指定位置
    /// </summary>
    /// <param name="relativePath">相对于TestData目录的路径</param>
    /// <param name="destinationPath">目标路径</param>
    /// <param name="overwrite">是否覆盖已存在的文件</param>
    /// <returns>复制是否成功</returns>
    Task<bool> CopyTestDataAsync(string relativePath, string destinationPath, bool overwrite = false);
}
```

### ITestDataManager

```csharp
/// <summary>
/// 测试数据管理器接口
/// </summary>
public interface ITestDataManager
{
    /// <summary>
    /// 验证测试数据完整性
    /// </summary>
    /// <returns>验证结果</returns>
    Task<TestDataValidationResult> ValidateTestDataAsync();
    
    /// <summary>
    /// 同步测试数据到指定项目
    /// </summary>
    /// <param name="targetProjectPath">目标项目路径</param>
    /// <param name="syncOptions">同步选项</param>
    /// <returns>同步结果</returns>
    Task<TestDataSyncResult> SyncTestDataAsync(string targetProjectPath, TestDataSyncOptions syncOptions = null);
    
    /// <summary>
    /// 生成测试数据报告
    /// </summary>
    /// <returns>测试数据报告</returns>
    Task<TestDataReport> GenerateReportAsync();
    
    /// <summary>
    /// 清理过期的测试数据
    /// </summary>
    /// <param name="cleanupOptions">清理选项</param>
    /// <returns>清理结果</returns>
    Task<TestDataCleanupResult> CleanupTestDataAsync(TestDataCleanupOptions cleanupOptions = null);
    
    /// <summary>
    /// 备份测试数据
    /// </summary>
    /// <param name="backupPath">备份路径</param>
    /// <returns>备份结果</returns>
    Task<TestDataBackupResult> BackupTestDataAsync(string backupPath);
    
    /// <summary>
    /// 从备份恢复测试数据
    /// </summary>
    /// <param name="backupPath">备份路径</param>
    /// <returns>恢复结果</returns>
    Task<TestDataRestoreResult> RestoreTestDataAsync(string backupPath);
    
    /// <summary>
    /// 获取测试数据状态
    /// </summary>
    /// <returns>测试数据状态</returns>
    Task<TestDataStatus> GetTestDataStatusAsync();
}
```

## 配置管理接口

### ITestConfigurationProvider

```csharp
/// <summary>
/// 测试配置提供者接口
/// </summary>
public interface ITestConfigurationProvider
{
    /// <summary>
    /// 获取测试配置
    /// </summary>
    /// <returns>测试配置</returns>
    TestConfiguration GetConfiguration();
    
    /// <summary>
    /// 更新测试配置
    /// </summary>
    /// <param name="configuration">新的测试配置</param>
    /// <returns>更新是否成功</returns>
    Task<bool> UpdateConfigurationAsync(TestConfiguration configuration);
    
    /// <summary>
    /// 重置测试配置到默认值
    /// </summary>
    /// <returns>重置是否成功</returns>
    Task<bool> ResetConfigurationAsync();
    
    /// <summary>
    /// 保存配置到文件
    /// </summary>
    /// <param name="configPath">配置文件路径</param>
    /// <returns>保存是否成功</returns>
    Task<bool> SaveConfigurationAsync(string configPath);
    
    /// <summary>
    /// 从文件加载配置
    /// </summary>
    /// <param name="configPath">配置文件路径</param>
    /// <returns>加载是否成功</returns>
    Task<bool> LoadConfigurationAsync(string configPath);
    
    /// <summary>
    /// 验证配置有效性
    /// </summary>
    /// <param name="configuration">要验证的配置</param>
    /// <returns>验证结果</returns>
    ConfigurationValidationResult ValidateConfiguration(TestConfiguration configuration);
}
```

### ITestConfigurationManager

```csharp
/// <summary>
/// 测试配置管理器接口
/// </summary>
public interface ITestConfigurationManager
{
    /// <summary>
    /// 获取环境特定的配置
    /// </summary>
    /// <param name="environment">环境名称</param>
    /// <returns>环境配置</returns>
    TestConfiguration GetEnvironmentConfiguration(string environment);
    
    /// <summary>
    /// 设置环境特定的配置
    /// </summary>
    /// <param name="environment">环境名称</param>
    /// <param name="configuration">配置</param>
    /// <returns>设置是否成功</returns>
    Task<bool> SetEnvironmentConfigurationAsync(string environment, TestConfiguration configuration);
    
    /// <summary>
    /// 获取所有环境配置
    /// </summary>
    /// <returns>环境配置字典</returns>
    Task<Dictionary<string, TestConfiguration>> GetAllEnvironmentConfigurationsAsync();
    
    /// <summary>
    /// 导出配置到JSON
    /// </summary>
    /// <param name="outputPath">输出路径</param>
    /// <returns>导出是否成功</returns>
    Task<bool> ExportConfigurationAsync(string outputPath);
    
    /// <summary>
    /// 从JSON导入配置
    /// </summary>
    /// <param name="inputPath">输入路径</param>
    /// <returns>导入是否成功</returns>
    Task<bool> ImportConfigurationAsync(string inputPath);
}
```

## 验证服务接口

### ITestDataValidator

```csharp
/// <summary>
/// 测试数据验证器接口
/// </summary>
public interface ITestDataValidator
{
    /// <summary>
    /// 验证XML文件结构
    /// </summary>
    /// <param name="xmlContent">XML内容</param>
    /// <param name="expectedSchema">期望的架构</param>
    /// <returns>验证结果</returns>
    Task<ValidationResult> ValidateXmlAsync(string xmlContent, string expectedSchema = null);
    
    /// <summary>
    /// 验证文件完整性
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>验证结果</returns>
    Task<ValidationResult> ValidateFileIntegrityAsync(string filePath);
    
    /// <summary>
    /// 验证测试数据目录结构
    /// </summary>
    /// <param name="directoryPath">目录路径</param>
    /// <returns>验证结果</returns>
    Task<ValidationResult> ValidateDirectoryStructureAsync(string directoryPath);
    
    /// <summary>
    /// 批量验证测试数据
    /// </summary>
    /// <param name="testDataPath">测试数据路径</param>
    /// <returns>批量验证结果</returns>
    Task<BatchValidationResult> ValidateAllTestDataAsync(string testDataPath);
    
    /// <summary>
    /// 验证特定类型的测试数据
    /// </summary>
    /// <param name="testDataPath">测试数据路径</param>
    /// <param name="dataType">数据类型</param>
    /// <returns>验证结果</returns>
    Task<ValidationResult> ValidateDataTypeAsync(string testDataPath, string dataType);
    
    /// <summary>
    /// 验证测试数据依赖关系
    /// </summary>
    /// <param name="testDataPath">测试数据路径</param>
    /// <returns>依赖关系验证结果</returns>
    Task<DependencyValidationResult> ValidateDependenciesAsync(string testDataPath);
}
```

### IXmlSchemaValidator

```csharp
/// <summary>
/// XML架构验证器接口
/// </summary>
public interface IXmlSchemaValidator
{
    /// <summary>
    /// 验证XML是否符合指定的XSD架构
    /// </summary>
    /// <param name="xmlContent">XML内容</param>
    /// <param name="xsdSchema">XSD架构</param>
    /// <returns>验证结果</returns>
    Task<XmlValidationResult> ValidateAgainstSchemaAsync(string xmlContent, string xsdSchema);
    
    /// <summary>
    /// 验证XML是否符合内置的架构
    /// </summary>
    /// <param name="xmlContent">XML内容</param>
    /// <param name="schemaType">架构类型</param>
    /// <returns>验证结果</returns>
    Task<XmlValidationResult> ValidateAgainstBuiltinSchemaAsync(string xmlContent, string schemaType);
    
    /// <summary>
    /// 获取支持的架构类型
    /// </summary>
    /// <returns>架构类型列表</returns>
    Task<List<string>> GetSupportedSchemaTypesAsync();
    
    /// <summary>
    /// 注册新的架构类型
    /// </summary>
    /// <param name="schemaType">架构类型</param>
    /// <param name="xsdSchema">XSD架构</param>
    /// <returns>注册是否成功</returns>
    Task<bool> RegisterSchemaTypeAsync(string schemaType, string xsdSchema);
}
```

## 同步服务接口

### ITestDataSyncService

```csharp
/// <summary>
/// 测试数据同步服务接口
/// </summary>
public interface ITestDataSyncService
{
    /// <summary>
    /// 同步测试数据到目标目录
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <param name="syncOptions">同步选项</param>
    /// <returns>同步结果</returns>
    Task<TestDataSyncResult> SyncAsync(string sourcePath, string targetPath, TestDataSyncOptions syncOptions = null);
    
    /// <summary>
    /// 创建符号链接
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <returns>创建是否成功</returns>
    Task<bool> CreateSymbolicLinkAsync(string sourcePath, string targetPath);
    
    /// <summary>
    /// 创建硬链接
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <returns>创建是否成功</returns>
    Task<bool> CreateHardLinkAsync(string sourcePath, string targetPath);
    
    /// <summary>
    /// 验证同步结果
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <returns>验证结果</returns>
    Task<SyncValidationResult> ValidateSyncAsync(string sourcePath, string targetPath);
    
    /// <summary>
    /// 执行双向同步
    /// </summary>
    /// <param name="path1">路径1</param>
    /// <param name="path2">路径2</param>
    /// <param name="syncOptions">同步选项</param>
    /// <returns>同步结果</returns>
    Task<TestDataSyncResult> BidirectionalSyncAsync(string path1, string path2, TestDataSyncOptions syncOptions = null);
    
    /// <summary>
    /// 获取同步状态
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <returns>同步状态</returns>
    Task<SyncStatus> GetSyncStatusAsync(string sourcePath, string targetPath);
}
```

### IFileSystemService

```csharp
/// <summary>
/// 文件系统服务接口
/// </summary>
public interface IFileSystemService
{
    /// <summary>
    /// 复制文件或目录
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <param name="overwrite">是否覆盖</param>
    /// <returns>复制是否成功</returns>
    Task<bool> CopyAsync(string sourcePath, string targetPath, bool overwrite = false);
    
    /// <summary>
    /// 移动文件或目录
    /// </summary>
    /// <param name="sourcePath">源路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <returns>移动是否成功</returns>
    Task<bool> MoveAsync(string sourcePath, string targetPath);
    
    /// <summary>
    /// 删除文件或目录
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="recursive">是否递归删除</param>
    /// <returns>删除是否成功</returns>
    Task<bool> DeleteAsync(string path, bool recursive = false);
    
    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>创建是否成功</returns>
    Task<bool> CreateDirectoryAsync(string path);
    
    /// <summary>
    /// 检查文件或目录是否存在
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>是否存在</returns>
    Task<bool> ExistsAsync(string path);
    
    /// <summary>
    /// 获取文件信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>文件信息</returns>
    Task<FileInfo> GetFileInfoAsync(string path);
    
    /// <summary>
    /// 获取目录信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>目录信息</returns>
    Task<DirectoryInfo> GetDirectoryInfoAsync(string path);
    
    /// <summary>
    /// 枚举文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="searchPattern">搜索模式</param>
    /// <param name="searchOption">搜索选项</param>
    /// <returns>文件列表</returns>
    Task<List<string>> EnumerateFilesAsync(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly);
    
    /// <summary>
    /// 枚举目录
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="searchPattern">搜索模式</param>
    /// <param name="searchOption">搜索选项</param>
    /// <returns>目录列表</returns>
    Task<List<string>> EnumerateDirectoriesAsync(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly);
}
```

## 监控和诊断接口

### ITestDiagnosticsService

```csharp
/// <summary>
/// 测试诊断服务接口
/// </summary>
public interface ITestDiagnosticsService
{
    /// <summary>
    /// 运行诊断检查
    /// </summary>
    /// <param name="diagnosticOptions">诊断选项</param>
    /// <returns>诊断结果</returns>
    Task<TestDiagnosticResult> RunDiagnosticsAsync(TestDiagnosticOptions diagnosticOptions = null);
    
    /// <summary>
    /// 获取系统信息
    /// </summary>
    /// <returns>系统信息</returns>
    Task<SystemInfo> GetSystemInfoAsync();
    
    /// <summary>
    /// 获取测试环境信息
    /// </summary>
    /// <returns>测试环境信息</returns>
    Task<TestEnvironmentInfo> GetTestEnvironmentInfoAsync();
    
    /// <summary>
    /// 收集日志文件
    /// </summary>
    /// <param name="logDirectory">日志目录</param>
    /// <returns>日志文件列表</returns>
    Task<List<string>> CollectLogsAsync(string logDirectory);
    
    /// <summary>
    /// 生成诊断报告
    /// </summary>
    /// <param name="outputPath">输出路径</param>
    /// <returns>生成是否成功</returns>
    Task<bool> GenerateDiagnosticReportAsync(string outputPath);
    
    /// <summary>
    /// 健康检查
    /// </summary>
    /// <returns>健康检查结果</returns>
    Task<HealthCheckResult> HealthCheckAsync();
}
```

### ITestMonitoringService

```csharp
/// <summary>
/// 测试监控服务接口
/// </summary>
public interface ITestMonitoringService
{
    /// <summary>
    /// 开始监控
    /// </summary>
    /// <param name="monitoringOptions">监控选项</param>
    /// <returns>监控是否成功启动</returns>
    Task<bool> StartMonitoringAsync(TestMonitoringOptions monitoringOptions = null);
    
    /// <summary>
    /// 停止监控
    /// </summary>
    /// <returns>监控是否成功停止</returns>
    Task<bool> StopMonitoringAsync();
    
    /// <summary>
    /// 获取监控状态
    /// </summary>
    /// <returns>监控状态</returns>
    Task<MonitoringStatus> GetMonitoringStatusAsync();
    
    /// <summary>
    /// 获取监控指标
    /// </summary>
    /// <returns>监控指标</returns>
    Task<MonitoringMetrics> GetMetricsAsync();
    
    /// <summary>
    /// 获取监控历史记录
    /// </summary>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>监控历史记录</returns>
    Task<List<MonitoringRecord>> GetMonitoringHistoryAsync(DateTime startTime, DateTime endTime);
    
    /// <summary>
    /// 设置警报规则
    /// </summary>
    /// <param name="alertRules">警报规则</param>
    /// <returns>设置是否成功</returns>
    Task<bool> SetAlertRulesAsync(List<AlertRule> alertRules);
    
    /// <summary>
    /// 获取警报
    /// </summary>
    /// <returns>警报列表</returns>
    Task<List<Alert>> GetAlertsAsync();
}
```

## 错误处理接口

### ITestErrorHandler

```csharp
/// <summary>
/// 测试错误处理器接口
/// </summary>
public interface ITestErrorHandler
{
    /// <summary>
    /// 处理错误
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <returns>处理结果</returns>
    Task<ErrorHandlingResult> HandleErrorAsync(ErrorInfo error);
    
    /// <summary>
    /// 处理异常
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="context">上下文信息</param>
    /// <returns>处理结果</returns>
    Task<ErrorHandlingResult> HandleExceptionAsync(Exception exception, Dictionary<string, object> context = null);
    
    /// <summary>
    /// 记录错误
    /// </summary>
    /// <param name="error">错误信息</param>
    /// <returns>记录是否成功</returns>
    Task<bool> LogErrorAsync(ErrorInfo error);
    
    /// <summary>
    /// 获取错误历史
    /// </summary>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>错误历史</returns>
    Task<List<ErrorInfo>> GetErrorHistoryAsync(DateTime startTime, DateTime endTime);
    
    /// <summary>
    /// 获取错误统计
    /// </summary>
    /// <returns>错误统计</returns>
    Task<ErrorStatistics> GetErrorStatisticsAsync();
    
    /// <summary>
    /// 清理错误日志
    /// </summary>
    /// <param name="cleanupOptions">清理选项</param>
    /// <returns>清理是否成功</returns>
    Task<bool> CleanupErrorLogsAsync(ErrorCleanupOptions cleanupOptions = null);
}
```

### ITestRecoveryService

```csharp
/// <summary>
/// 测试恢复服务接口
/// </summary>
public interface ITestRecoveryService
{
    /// <summary>
    /// 尝试恢复测试数据
    /// </summary>
    /// <param name="recoveryOptions">恢复选项</param>
    /// <returns>恢复结果</returns>
    Task<TestRecoveryResult> RecoverTestDataAsync(TestRecoveryOptions recoveryOptions = null);
    
    /// <summary>
    /// 回滚到上一个已知良好状态
    /// </summary>
    /// <returns>回滚结果</returns>
    Task<TestRecoveryResult> RollbackToLastKnownGoodStateAsync();
    
    /// <summary>
    /// 创建恢复点
    /// </summary>
    /// <param name="description">描述</param>
    /// <returns>恢复点</returns>
    Task<RecoveryPoint> CreateRecoveryPointAsync(string description);
    
    /// <summary>
    /// 恢复到指定的恢复点
    /// </summary>
    /// <param name="recoveryPointId">恢复点ID</param>
    /// <returns>恢复结果</returns>
    Task<TestRecoveryResult> RestoreToRecoveryPointAsync(string recoveryPointId);
    
    /// <summary>
    /// 获取恢复点列表
    /// </summary>
    /// <returns>恢复点列表</returns>
    Task<List<RecoveryPoint>> GetRecoveryPointsAsync();
    
    /// <summary>
    /// 删除恢复点
    /// </summary>
    /// <param name="recoveryPointId">恢复点ID</param>
    /// <returns>删除是否成功</returns>
    Task<bool> DeleteRecoveryPointAsync(string recoveryPointId);
}
```

## 数据模型

### 核心数据模型

```csharp
/// <summary>
/// 测试数据验证结果
/// </summary>
public class TestDataValidationResult
{
    /// <summary>
    /// 验证是否成功
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// 验证时间
    /// </summary>
    public DateTime ValidationTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 验证消息
    /// </summary>
    public List<string> Messages { get; set; } = new List<string>();
    
    /// <summary>
    /// 错误列表
    /// </summary>
    public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    
    /// <summary>
    /// 警告列表
    /// </summary>
    public List<ValidationWarning> Warnings { get; set; } = new List<ValidationWarning>();
    
    /// <summary>
    /// 验证的文件列表
    /// </summary>
    public List<ValidatedFile> ValidatedFiles { get; set; } = new List<ValidatedFile>();
    
    /// <summary>
    /// 统计信息
    /// </summary>
    public ValidationStatistics Statistics { get; set; } = new ValidationStatistics();
}

/// <summary>
/// 测试数据同步结果
/// </summary>
public class TestDataSyncResult
{
    /// <summary>
    /// 同步是否成功
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// 同步时间
    /// </summary>
    public DateTime SyncTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 源路径
    /// </summary>
    public string SourcePath { get; set; }
    
    /// <summary>
    /// 目标路径
    /// </summary>
    public string TargetPath { get; set; }
    
    /// <summary>
    /// 同步的文件列表
    /// </summary>
    public List<SyncedFile> SyncedFiles { get; set; } = new List<SyncedFile>();
    
    /// <summary>
    /// 失败的文件列表
    /// </summary>
    public List<SyncFailedFile> FailedFiles { get; set; } = new List<SyncFailedFile>();
    
    /// <summary>
    /// 统计信息
    /// </summary>
    public SyncStatistics Statistics { get; set; } = new SyncStatistics();
    
    /// <summary>
    /// 同步选项
    /// </summary>
    public TestDataSyncOptions SyncOptions { get; set; }
}

/// <summary>
/// 测试数据报告
/// </summary>
public class TestDataReport
{
    /// <summary>
    /// 报告生成时间
    /// </summary>
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// 报告标题
    /// </summary>
    public string Title { get; set; } = "测试数据报告";
    
    /// <summary>
    /// 报告摘要
    /// </summary>
    public ReportSummary Summary { get; set; } = new ReportSummary();
    
    /// <summary>
    /// 文件统计
    /// </summary>
    public FileStatistics FileStatistics { get; set; } = new FileStatistics();
    
    /// <summary>
    /// 验证结果
    /// </summary>
    public TestDataValidationResult ValidationResult { get; set; }
    
    /// <summary>
    /// 同步状态
    /// </summary>
    public SyncStatus SyncStatus { get; set; }
    
    /// <summary>
    /// 详细信息
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();
}
```

### 配置数据模型

```csharp
/// <summary>
/// 测试配置
/// </summary>
public class TestConfiguration
{
    /// <summary>
    /// 测试数据目录路径
    /// </summary>
    public string TestDataPath { get; set; } = "TestData";
    
    /// <summary>
    /// 是否启用符号链接
    /// </summary>
    public bool UseSymbolicLinks { get; set; } = true;
    
    /// <summary>
    /// 需要同步的文件模式
    /// </summary>
    public string[] SyncPatterns { get; set; } = 
    {
        "*.xml",
        "*.json",
        "*.txt"
    };
    
    /// <summary>
    /// 需要排除的文件模式
    /// </summary>
    public string[] ExcludePatterns { get; set; } = 
    {
        "*.tmp",
        "*.log"
    };
    
    /// <summary>
    /// 验证设置
    /// </summary>
    public ValidationSettings Validation { get; set; } = new ValidationSettings();
    
    /// <summary>
    /// 同步设置
    /// </summary>
    public SyncSettings Sync { get; set; } = new SyncSettings();
    
    /// <summary>
    /// 监控设置
    /// </summary>
    public MonitoringSettings Monitoring { get; set; } = new MonitoringSettings();
    
    /// <summary>
    /// 错误处理设置
    /// </summary>
    public ErrorHandlingSettings ErrorHandling { get; set; } = new ErrorHandlingSettings();
}

/// <summary>
/// 验证设置
/// </summary>
public class ValidationSettings
{
    /// <summary>
    /// 是否启用严格验证
    /// </summary>
    public bool StrictValidation { get; set; } = true;
    
    /// <summary>
    /// 必需的文件列表
    /// </summary>
    public string[] RequiredFiles { get; set; } = 
    {
        "action_types.xml",
        "combat_parameters.xml",
        "attributes.xml"
    };
    
    /// <summary>
    /// 文件大小限制（字节）
    /// </summary>
    public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
    
    /// <summary>
    /// 是否验证XML架构
    /// </summary>
    public bool ValidateXmlSchema { get; set; } = true;
    
    /// <summary>
    /// 是否验证文件完整性
    /// </summary>
    public bool ValidateFileIntegrity { get; set; } = true;
}

/// <summary>
/// 同步设置
/// </summary>
public class SyncSettings
{
    /// <summary>
    /// 是否启用自动同步
    /// </summary>
    public bool AutoSync { get; set; } = true;
    
    /// <summary>
    /// 同步间隔（秒）
    /// </summary>
    public int SyncInterval { get; set; } = 300; // 5分钟
    
    /// <summary>
    /// 同步重试次数
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;
    
    /// <summary>
    /// 同步超时时间（秒）
    /// </summary>
    public int SyncTimeout { get; set; } = 60;
    
    /// <summary>
    /// 同步模式
    /// </summary>
    public SyncMode SyncMode { get; set; } = SyncMode.OneWay;
}

/// <summary>
/// 同步模式
/// </summary>
public enum SyncMode
{
    /// <summary>
    /// 单向同步
    /// </summary>
    OneWay,
    
    /// <summary>
    /// 双向同步
    /// </summary>
    TwoWay,
    
    /// <summary>
    /// 镜像同步
    /// </summary>
    Mirror
}
```

### 选项和参数模型

```csharp
/// <summary>
/// 测试数据同步选项
/// </summary>
public class TestDataSyncOptions
{
    /// <summary>
    /// 是否覆盖已存在的文件
    /// </summary>
    public bool Overwrite { get; set; } = true;
    
    /// <summary>
    /// 是否递归同步子目录
    /// </summary>
    public bool Recursive { get; set; } = true;
    
    /// <summary>
    /// 是否验证同步结果
    /// </summary>
    public bool Verify { get; set; } = true;
    
    /// <summary>
    /// 是否创建备份
    /// </summary>
    public bool CreateBackup { get; set; } = false;
    
    /// <summary>
    /// 同步模式
    /// </summary>
    public SyncMode SyncMode { get; set; } = SyncMode.OneWay;
    
    /// <summary>
    /// 文件过滤模式
    /// </summary>
    public string[] IncludePatterns { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// 排除的文件模式
    /// </summary>
    public string[] ExcludePatterns { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// 超时时间（秒）
    /// </summary>
    public int Timeout { get; set; } = 60;
    
    /// <summary>
    /// 进度回调
    /// </summary>
    public Action<SyncProgress> ProgressCallback { get; set; }
}

/// <summary>
/// 测试诊断选项
/// </summary>
public class TestDiagnosticOptions
{
    /// <summary>
    /// 是否检查文件系统
    /// </summary>
    public bool CheckFileSystem { get; set; } = true;
    
    /// <summary>
    /// 是否检查权限
    /// </summary>
    public bool CheckPermissions { get; set; } = true;
    
    /// <summary>
    /// 是否检查网络连接
    /// </summary>
    public bool CheckNetwork { get; set; } = false;
    
    /// <summary>
    /// 是否检查磁盘空间
    /// </summary>
    public bool CheckDiskSpace { get; set; } = true;
    
    /// <summary>
    /// 是否检查进程状态
    /// </summary>
    public bool CheckProcessStatus { get; set; } = true;
    
    /// <summary>
    /// 是否检查依赖项
    /// </summary>
    public bool CheckDependencies { get; set; } = true;
    
    /// <summary>
    /// 是否生成详细报告
    /// </summary>
    public bool GenerateDetailedReport { get; set; } = false;
    
    /// <summary>
    /// 诊断级别
    /// </summary>
    public DiagnosticLevel Level { get; set; } = DiagnosticLevel.Standard;
}

/// <summary>
/// 诊断级别
/// </summary>
public enum DiagnosticLevel
{
    /// <summary>
    /// 基础诊断
    /// </summary>
    Basic,
    
    /// <summary>
    /// 标准诊断
    /// </summary>
    Standard,
    
    /// <summary>
    /// 详细诊断
    /// </summary>
    Detailed,
    
    /// <summary>
    /// 完整诊断
    /// </summary>
    Full
}
```

## 使用示例

### 基本使用示例

```csharp
// 创建测试数据管理器
var testDataManager = new TestDataManager();
var configuration = new TestConfiguration
{
    TestDataPath = "TestData",
    UseSymbolicLinks = true,
    Validation = new ValidationSettings
    {
        StrictValidation = true,
        RequiredFiles = new[] { "action_types.xml", "combat_parameters.xml" }
    }
};

// 验证测试数据
var validationResult = await testDataManager.ValidateTestDataAsync();
if (!validationResult.IsValid)
{
    Console.WriteLine("测试数据验证失败:");
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"- {error.Message}");
    }
}

// 同步测试数据到UI项目
var syncResult = await testDataManager.SyncTestDataAsync("BannerlordModEditor.UI.Tests");
if (syncResult.IsSuccess)
{
    Console.WriteLine($"成功同步 {syncResult.Statistics.SyncedFilesCount} 个文件");
}
else
{
    Console.WriteLine("同步失败:");
    foreach (var failedFile in syncResult.FailedFiles)
    {
        Console.WriteLine($"- {failedFile.FilePath}: {failedFile.ErrorMessage}");
    }
}
```

### 高级使用示例

```csharp
// 创建完整的测试服务
var testServiceProvider = new TestServiceProvider();
var testDataService = testServiceProvider.GetRequiredService<ITestDataService>();
var validator = testServiceProvider.GetRequiredService<ITestDataValidator>();
var syncService = testServiceProvider.GetRequiredService<ITestDataSyncService>();

// 配置和初始化
var config = await testDataService.LoadConfigurationAsync("config/test-config.json");
await testDataService.InitializeAsync(config);

// 运行完整的验证和同步流程
var validationResult = await validator.ValidateAllTestDataAsync(config.TestDataPath);
if (!validationResult.OverallSuccess)
{
    // 尝试恢复
    var recoveryService = testServiceProvider.GetRequiredService<ITestRecoveryService>();
    var recoveryResult = await recoveryService.RecoverTestDataAsync();
    
    if (recoveryResult.IsSuccess)
    {
        Console.WriteLine("测试数据恢复成功");
        validationResult = await validator.ValidateAllTestDataAsync(config.TestDataPath);
    }
}

// 同步到多个目标
var targets = new[] { "BannerlordModEditor.UI.Tests", "BannerlordModEditor.Common.Tests" };
foreach (var target in targets)
{
    var syncOptions = new TestDataSyncOptions
    {
        Overwrite = true,
        Verify = true,
        CreateBackup = true,
        ProgressCallback = progress => 
        {
            Console.WriteLine($"同步进度: {progress.PercentageComplete}%");
        }
    };
    
    var syncResult = await syncService.SyncAsync(config.TestDataPath, target, syncOptions);
    if (syncResult.IsSuccess)
    {
        Console.WriteLine($"成功同步到 {target}");
    }
}

// 生成报告
var reportService = testServiceProvider.GetRequiredService<ITestReportService>();
var report = await reportService.GenerateReportAsync();
await reportService.SaveReportAsync(report, "reports/test-data-report.html");
```

## 总结

本API规范文档提供了完整的测试数据管理接口定义，包括：

1. **测试数据访问接口**：提供统一的测试数据访问和管理功能
2. **配置管理接口**：支持环境特定的配置管理和验证
3. **验证服务接口**：提供全面的测试数据验证功能
4. **同步服务接口**：支持多种同步模式和跨平台操作
5. **监控和诊断接口**：提供实时监控和问题诊断能力
6. **错误处理接口**：提供完整的错误处理和恢复机制

这些接口为解决GitHub Actions UI测试失败问题提供了坚实的技术基础，确保测试数据的可用性、一致性和可靠性。