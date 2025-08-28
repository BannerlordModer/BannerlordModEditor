# API规范文档

## 概述

本文档定义了BannerlordModEditor-CLI项目XML映射适配系统的API规范，包括核心接口、数据模型、服务契约和扩展点。

## 核心服务接口

### IFileDiscoveryService

```csharp
namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 文件发现服务接口
    /// 负责扫描XML目录，识别未适配的文件
    /// </summary>
    public interface IFileDiscoveryService
    {
        /// <summary>
        /// 查找所有未适配的XML文件
        /// </summary>
        /// <returns>未适配文件列表，按复杂度和文件名排序</returns>
        Task<List<UnadaptedFile>> FindUnadaptedFilesAsync();
        
        /// <summary>
        /// 将XML文件名转换为模型类名
        /// </summary>
        /// <param name="xmlFileName">XML文件名（不含扩展名）</param>
        /// <returns>转换后的模型类名</returns>
        string ConvertToModelName(string xmlFileName);
        
        /// <summary>
        /// 检查指定模型是否存在
        /// </summary>
        /// <param name="modelName">模型名称</param>
        /// <param name="searchDirectories">搜索目录列表</param>
        /// <returns>模型是否存在</returns>
        bool ModelExists(string modelName, string[] searchDirectories);
        
        /// <summary>
        /// 检查指定XML文件是否已适配
        /// </summary>
        /// <param name="xmlFileName">XML文件名</param>
        /// <returns>是否已适配</returns>
        bool IsFileAdapted(string xmlFileName);
        
        /// <summary>
        /// 获取模型搜索目录列表
        /// </summary>
        /// <returns>搜索目录列表</returns>
        string[] GetModelSearchDirectories();
        
        /// <summary>
        /// 确定文件复杂度
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>复杂度级别</returns>
        FileComplexity DetermineComplexity(FileInfo fileInfo);
    }
}
```

### IXmlMappingService

```csharp
namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// XML映射服务接口
    /// 负责XML数据的加载、转换和保存
    /// </summary>
    public interface IXmlMappingService
    {
        /// <summary>
        /// 加载XML文件到DO对象
        /// </summary>
        /// <typeparam name="T">DO类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns>加载的DO对象</returns>
        Task<T?> LoadToDOAsync<T>(string filePath) where T : class;
        
        /// <summary>
        /// 将DO对象保存为XML文件
        /// </summary>
        /// <typeparam name="T">DO类型</typeparam>
        /// <param name="data">DO对象</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="originalXml">原始XML内容（用于保留格式）</param>
        Task SaveFromDOAsync<T>(T data, string filePath, string? originalXml = null) where T : class;
        
        /// <summary>
        /// 执行XML往返测试
        /// </summary>
        /// <typeparam name="T">DO类型</typeparam>
        /// <param name="originalXml">原始XML内容</param>
        /// <returns>往返测试结果</returns>
        Task<XmlRoundTripResult> ExecuteRoundTripTestAsync<T>(string originalXml) where T : class;
        
        /// <summary>
        /// 验证XML结构
        /// </summary>
        /// <param name="xmlContent">XML内容</param>
        /// <param name="expectedRoot">期望的根元素名称</param>
        /// <returns>验证结果</returns>
        Task<XmlValidationResult> ValidateXmlStructureAsync(string xmlContent, string expectedRoot);
    }
}
```

### IMapperService

```csharp
namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 对象映射服务接口
    /// 负责DO和DTO对象之间的转换
    /// </summary>
    public interface IMapperService
    {
        /// <summary>
        /// 将DO对象转换为DTO对象
        /// </summary>
        /// <typeparam name="TDO">DO类型</typeparam>
        /// <typeparam name="TDTO">DTO类型</typeparam>
        /// <param name="source">源DO对象</param>
        /// <returns>转换后的DTO对象</returns>
        TDTO? ToDTO<TDO, TDTO>(TDO? source) where TDO : class where TDTO : class;
        
        /// <summary>
        /// 将DTO对象转换为DO对象
        /// </summary>
        /// <typeparam name="TDTO">DTO类型</typeparam>
        /// <typeparam name="TDO">DO类型</typeparam>
        /// <param name="source">源DTO对象</param>
        /// <returns>转换后的DO对象</returns>
        TDO? ToDO<TDTO, TDO>(TDTO? source) where TDTO : class where TDO : class;
        
        /// <summary>
        /// 批量转换DO对象到DTO对象
        /// </summary>
        /// <typeparam name="TDO">DO类型</typeparam>
        /// <typeparam name="TDTO">DTO类型</typeparam>
        /// <param name="sourceList">源DO对象列表</param>
        /// <returns>转换后的DTO对象列表</returns>
        List<TDTO> ToDTOList<TDO, TDTO>(IEnumerable<TDO> sourceList) where TDO : class where TDTO : class;
        
        /// <summary>
        /// 批量转换DTO对象到DO对象
        /// </summary>
        /// <typeparam name="TDTO">DTO类型</typeparam>
        /// <typeparam name="TDO">DO类型</typeparam>
        /// <param name="sourceList">源DTO对象列表</param>
        /// <returns>转换后的DO对象列表</returns>
        List<TDO> ToDOList<TDTO, TDO>(IEnumerable<TDTO> sourceList) where TDTO : class where TDO : class;
        
        /// <summary>
        /// 注册自定义映射器
        /// </summary>
        /// <param name="mapperType">映射器类型</param>
        void RegisterMapper(Type mapperType);
        
        /// <summary>
        /// 获取所有注册的映射器
        /// </summary>
        /// <returns>映射器类型列表</returns>
        IEnumerable<Type> GetRegisteredMappers();
    }
}
```

## 数据模型

### UnadaptedFile

```csharp
namespace BannerlordModEditor.Common.Models
{
    /// <summary>
    /// 未适配文件信息
    /// </summary>
    public class UnadaptedFile
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        
        /// <summary>
        /// 完整文件路径
        /// </summary>
        public string FullPath { get; set; } = string.Empty;
        
        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// 期望的模型名称
        /// </summary>
        public string ExpectedModelName { get; set; } = string.Empty;
        
        /// <summary>
        /// 文件复杂度
        /// </summary>
        public FileComplexity Complexity { get; set; }
        
        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime LastModified { get; set; }
        
        /// <summary>
        /// XML根元素名称
        /// </summary>
        public string? RootElement { get; set; }
        
        /// <summary>
        /// 估计的适配工作量（人时）
        /// </summary>
        public double EstimatedEffort { get; set; }
    }
}
```

### FileComplexity

```csharp
namespace BannerlordModEditor.Common.Models
{
    /// <summary>
    /// 文件复杂度枚举
    /// </summary>
    public enum FileComplexity
    {
        /// <summary>
        /// 简单：单一根元素，少量属性
        /// </summary>
        Simple = 1,
        
        /// <summary>
        /// 中等：包含子元素，基本嵌套结构
        /// </summary>
        Medium = 2,
        
        /// <summary>
        /// 复杂：深层嵌套，多种元素类型
        /// </summary>
        Complex = 3,
        
        /// <summary>
        /// 非常复杂：大型文件，复杂的数据结构
        /// </summary>
        VeryComplex = 4
    }
}
```

### XmlRoundTripResult

```csharp
namespace BannerlordModEditor.Common.Models
{
    /// <summary>
    /// XML往返测试结果
    /// </summary>
    public class XmlRoundTripResult
    {
        /// <summary>
        /// 测试是否成功
        /// </summary>
        public bool Success { get; set; }
        
        /// <summary>
        /// 原始XML内容
        /// </summary>
        public string OriginalXml { get; set; } = string.Empty;
        
        /// <summary>
        /// 序列化后的XML内容
        /// </summary>
        public string SerializedXml { get; set; } = string.Empty;
        
        /// <summary>
        /// 差异描述
        /// </summary>
        public string? Differences { get; set; }
        
        /// <summary>
        /// 处理时间
        /// </summary>
        public TimeSpan ProcessingTime { get; set; }
        
        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }
        
        /// <summary>
        /// 警告消息列表
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
```

### XmlValidationResult

```csharp
namespace BannerlordModEditor.Common.Models
{
    /// <summary>
    /// XML验证结果
    /// </summary>
    public class XmlValidationResult
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// 验证错误列表
        /// </summary>
        public List<XmlValidationError> Errors { get; set; } = new List<XmlValidationError>();
        
        /// <summary>
        /// 验证警告列表
        /// </summary>
        public List<XmlValidationWarning> Warnings { get; set; } = new List<XmlValidationWarning>();
        
        /// <summary>
        /// XML根元素
        /// </summary>
        public string? RootElement { get; set; }
        
        /// <summary>
        /// XML命名空间
        /// </summary>
        public string? Namespace { get; set; }
        
        /// <summary>
        /// 元素总数
        /// </summary>
        public int ElementCount { get; set; }
        
        /// <summary>
        /// 属性总数
        /// </summary>
        public int AttributeCount { get; set; }
    }
}
```

### XmlValidationError

```csharp
namespace BannerlordModEditor.Common.Models
{
    /// <summary>
    /// XML验证错误
    /// </summary>
    public class XmlValidationError
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; } = string.Empty;
        
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; } = string.Empty;
        
        /// <summary>
        /// 错误位置（行号）
        /// </summary>
        public int LineNumber { get; set; }
        
        /// <summary>
        /// 错误位置（列号）
        /// </summary>
        public int ColumnNumber { get; set; }
        
        /// <summary>
        /// 错误严重程度
        /// </summary>
        public ErrorSeverity Severity { get; set; }
        
        /// <summary>
        /// 相关的XPath
        /// </summary>
        public string? XPath { get; set; }
    }
}
```

## XML处理接口

### IXmlProcessor

```csharp
namespace BannerlordModEditor.Common.Processors
{
    /// <summary>
    /// XML处理器接口
    /// 负责XML文件的预处理和后处理
    /// </summary>
    public interface IXmlProcessor
    {
        /// <summary>
        /// 预处理XML内容
        /// </summary>
        /// <param name="xmlContent">原始XML内容</param>
        /// <param name="processingOptions">处理选项</param>
        /// <returns>预处理后的XML内容</returns>
        Task<string> PreProcessAsync(string xmlContent, XmlProcessingOptions processingOptions);
        
        /// <summary>
        /// 后处理XML内容
        /// </summary>
        /// <param name="xmlContent">序列化后的XML内容</param>
        /// <param name="processingOptions">处理选项</param>
        /// <returns>后处理后的XML内容</returns>
        Task<string> PostProcessAsync(string xmlContent, XmlProcessingOptions processingOptions);
        
        /// <summary>
        /// 标准化XML格式
        /// </summary>
        /// <param name="xmlContent">XML内容</param>
        /// <returns>标准化后的XML内容</returns>
        Task<string> NormalizeAsync(string xmlContent);
        
        /// <summary>
        /// 提取XML结构信息
        /// </summary>
        /// <param name="xmlContent">XML内容</param>
        /// <returns>XML结构信息</returns>
        Task<XmlStructureInfo> ExtractStructureAsync(string xmlContent);
    }
}
```

### IXmlAnalyzer

```csharp
namespace BannerlordModEditor.Common.Analyzers
{
    /// <summary>
    /// XML分析器接口
    /// 负责分析XML结构并生成适配建议
    /// </summary>
    public interface IXmlAnalyzer
    {
        /// <summary>
        /// 分析XML文件结构
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>分析结果</returns>
        Task<XmlAnalysisResult> AnalyzeAsync(string filePath);
        
        /// <summary>
        /// 生成适配建议
        /// </summary>
        /// <param name="analysisResult">分析结果</param>
        /// <returns>适配建议</returns>
        Task<AdaptationSuggestion> GenerateSuggestionAsync(XmlAnalysisResult analysisResult);
        
        /// <summary>
        /// 估计适配工作量
        /// </summary>
        /// <param name="analysisResult">分析结果</param>
        /// <returns>工作量估计</returns>
        Task<EffortEstimate> EstimateEffortAsync(XmlAnalysisResult analysisResult);
        
        /// <summary>
        /// 验证适配结果
        /// </summary>
        /// <param name="originalXml">原始XML</param>
        /// <param name="adaptedXml">适配后的XML</param>
        /// <returns>验证结果</returns>
        Task<AdaptationValidationResult> ValidateAdaptationAsync(string originalXml, string adaptedXml);
    }
}
```

## 扩展接口

### IXmlTypeAdapter

```csharp
namespace BannerlordModEditor.Common.Adapters
{
    /// <summary>
    /// XML类型适配器接口
    /// 用于特定XML类型的适配逻辑
    /// </summary>
    public interface IXmlTypeAdapter
    {
        /// <summary>
        /// 适配器名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 支持的XML类型列表
        /// </summary>
        IEnumerable<string> SupportedTypes { get; }
        
        /// <summary>
        /// 检查是否支持指定的XML类型
        /// </summary>
        /// <param name="xmlType">XML类型</param>
        /// <returns>是否支持</returns>
        bool SupportsType(string xmlType);
        
        /// <summary>
        /// 适配XML内容
        /// </summary>
        /// <param name="xmlContent">XML内容</param>
        /// <param name="adaptationOptions">适配选项</param>
        /// <returns>适配后的XML内容</returns>
        Task<string> AdaptAsync(string xmlContent, AdaptationOptions adaptationOptions);
        
        /// <summary>
        /// 反适配XML内容
        /// </summary>
        /// <param name="adaptedContent">适配后的内容</param>
        /// <param name="adaptationOptions">适配选项</param>
        /// <returns>原始XML内容</returns>
        Task<string> UnadaptAsync(string adaptedContent, AdaptationOptions adaptationOptions);
    }
}
```

### ICodeGenerator

```csharp
namespace BannerlordModEditor.Common.Generators
{
    /// <summary>
    /// 代码生成器接口
    /// 用于生成DO/DTO模型和映射器代码
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// 生成DO模型代码
        /// </summary>
        /// <param name="xmlStructure">XML结构信息</param>
        /// <param name="generationOptions">生成选项</param>
        /// <returns>生成的代码</returns>
        Task<string> GenerateDOAsync(XmlStructureInfo xmlStructure, CodeGenerationOptions generationOptions);
        
        /// <summary>
        /// 生成DTO模型代码
        /// </summary>
        /// <param name="xmlStructure">XML结构信息</param>
        /// <param name="generationOptions">生成选项</param>
        /// <returns>生成的代码</returns>
        Task<string> GenerateDTOAsync(XmlStructureInfo xmlStructure, CodeGenerationOptions generationOptions);
        
        /// <summary>
        /// 生成映射器代码
        /// </summary>
        /// <param name="doModel">DO模型信息</param>
        /// <param name="dtoModel">DTO模型信息</param>
        /// <param name="generationOptions">生成选项</param>
        /// <returns>生成的代码</returns>
        Task<string> GenerateMapperAsync(ModelInfo doModel, ModelInfo dtoModel, CodeGenerationOptions generationOptions);
        
        /// <summary>
        /// 生成测试代码
        /// </summary>
        /// <param name="modelInfo">模型信息</param>
        /// <param name="generationOptions">生成选项</param>
        /// <returns>生成的测试代码</returns>
        Task<string> GenerateTestAsync(ModelInfo modelInfo, CodeGenerationOptions generationOptions);
    }
}
```

## 配置接口

### IConfigurationService

```csharp
namespace BannerlordModEditor.Common.Configuration
{
    /// <summary>
    /// 配置服务接口
    /// 负责管理和应用系统配置
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="key">配置键</param>
        /// <returns>配置值</returns>
        T? GetConfiguration<T>(string key);
        
        /// <summary>
        /// 设置配置值
        /// </summary>
        /// <typeparam name="T">配置值类型</typeparam>
        /// <param name="key">配置键</param>
        /// <param name="value">配置值</param>
        void SetConfiguration<T>(string key, T value);
        
        /// <summary>
        /// 保存配置到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        Task SaveConfigurationAsync(string filePath);
        
        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <param name="filePath">文件路径</param>
        Task LoadConfigurationAsync(string filePath);
        
        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <returns>配置字典</returns>
        IDictionary<string, object> GetAllConfigurations();
        
        /// <summary>
        /// 重置配置到默认值
        /// </summary>
        void ResetToDefaults();
    }
}
```

## 事件接口

### IEventBus

```csharp
namespace BannerlordModEditor.Common.Events
{
    /// <summary>
    /// 事件总线接口
    /// 负责系统内部事件的发布和订阅
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="event">事件数据</param>
        Task PublishAsync<T>(T @event) where T : IIntegrationEvent;
        
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="handler">事件处理器</param>
        void Subscribe<T>(Func<T, Task> handler) where T : IIntegrationEvent;
        
        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="handler">事件处理器</param>
        void Unsubscribe<T>(Func<T, Task> handler) where T : IIntegrationEvent;
        
        /// <summary>
        /// 获取所有订阅者
        /// </summary>
        /// <returns>订阅者信息列表</returns>
        IEnumerable<SubscriberInfo> GetSubscribers();
    }
}
```

## 诊断接口

### IDiagnosticService

```csharp
namespace BannerlordModEditor.Common.Diagnostics
{
    /// <summary>
    /// 诊断服务接口
    /// 负责系统健康检查和性能监控
    /// </summary>
    public interface IDiagnosticService
    {
        /// <summary>
        /// 执行系统健康检查
        /// </summary>
        /// <returns>健康检查结果</returns>
        Task<HealthCheckResult> PerformHealthCheckAsync();
        
        /// <summary>
        /// 获取系统性能指标
        /// </summary>
        /// <returns>性能指标</returns>
        Task<PerformanceMetrics> GetPerformanceMetricsAsync();
        
        /// <summary>
        /// 获取系统诊断信息
        /// </summary>
        /// <returns>诊断信息</returns>
        Task<SystemDiagnostics> GetSystemDiagnosticsAsync();
        
        /// <summary>
        /// 生成诊断报告
        /// </summary>
        /// <param name="reportOptions">报告选项</param>
        /// <returns>诊断报告</returns>
        Task<DiagnosticReport> GenerateReportAsync(DiagnosticReportOptions reportOptions);
    }
}
```

## 使用示例

### 基本使用

```csharp
// 初始化服务
var fileDiscoveryService = new FileDiscoveryService("example/ModuleData");
var mappingService = new XmlMappingService();
var mapperService = new MapperService();

// 发现未适配文件
var unadaptedFiles = await fileDiscoveryService.FindUnadaptedFilesAsync();

foreach (var file in unadaptedFiles)
{
    // 加载XML到DO对象
    var combatParams = await mappingService.LoadToDOAsync<CombatParametersDO>(file.FullPath);
    
    // 处理业务逻辑
    if (combatParams != null)
    {
        // 修改数据
        combatParams.Type = "modified_type";
        
        // 保存回XML文件
        await mappingService.SaveFromDOAsync(combatParams, file.FullPath);
    }
}
```

### 高级使用

```csharp
// 执行往返测试
var originalXml = File.ReadAllText("test.xml");
var roundTripResult = await mappingService.ExecuteRoundTripTestAsync<CombatParametersDO>(originalXml);

if (!roundTripResult.Success)
{
    Console.WriteLine($"往返测试失败: {roundTripResult.ErrorMessage}");
    Console.WriteLine($"差异: {roundTripResult.Differences}");
}

// 批量处理
var batchProcessor = new BatchXmlProcessor(fileDiscoveryService, mappingService);
var batchResult = await batchProcessor.ProcessBatchAsync(unadaptedFiles.Take(10));

Console.WriteLine($"成功处理: {batchResult.SuccessCount}");
Console.WriteLine($"失败处理: {batchResult.FailureCount}");
```

## 扩展点

### 自定义映射器

```csharp
public class CustomCombatParametersMapper
{
    public static CombatParametersDTO ToDTO(CombatParametersDO source)
    {
        // 自定义映射逻辑
        return new CombatParametersDTO
        {
            // 映射属性
        };
    }
    
    public static CombatParametersDO ToDO(CombatParametersDTO source)
    {
        // 自定义映射逻辑
        return new CombatParametersDO
        {
            // 映射属性
        };
    }
}
```

### 自定义XML处理器

```csharp
public class CustomXmlProcessor : IXmlProcessor
{
    public async Task<string> PreProcessAsync(string xmlContent, XmlProcessingOptions options)
    {
        // 自定义预处理逻辑
        return processedXml;
    }
    
    public async Task<string> PostProcessAsync(string xmlContent, XmlProcessingOptions options)
    {
        // 自定义后处理逻辑
        return processedXml;
    }
}
```

## 版本兼容性

### API版本策略

- **v1.0**: 初始版本，支持基本的XML映射适配
- **v1.1**: 添加批量处理功能
- **v1.2**: 添加代码生成功能
- **v2.0**: 重构为插件架构，支持自定义适配器

### 向后兼容性

- 所有API变更都通过版本化接口处理
- 旧版本API至少支持2个主要版本
- 提供迁移指南和工具

## 总结

本API规范定义了XML映射适配系统的完整接口契约，涵盖了文件发现、XML处理、对象映射、代码生成等核心功能。该设计具有良好的扩展性和可维护性，能够满足当前和未来的功能需求。