# 通用XML转换框架架构设计文档

## 1. 概述

基于现有的TUI项目中的FormatConversionService和相关类，设计一个可扩展、高性能的通用XML转换框架。该框架将支持多种XML类型和目标格式的转换，采用模块化设计，便于扩展和维护。

## 2. 核心架构设计

### 2.1 整体架构图

```
┌─────────────────────────────────────────────────────────────────┐
│                    通用XML转换框架                                │
├─────────────────────────────────────────────────────────────────┤
│  应用层 (Application Layer)                                      │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐    │
│  │   转换管理器    │ │   配置管理器     │ │   插件管理器     │    │
│  │ConversionManager│ │ConfigManager    │ │PluginManager    │    │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘    │
├─────────────────────────────────────────────────────────────────┤
│  服务层 (Service Layer)                                          │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐    │
│  │   转换服务      │ │   验证服务       │ │   发现服务       │    │
│  │ConversionService│ │ValidationService│ │DiscoveryService │    │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘    │
├─────────────────────────────────────────────────────────────────┤
│  核心层 (Core Layer)                                              │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐    │
│  │   转换管道      │ │   策略工厂       │ │   类型注册器     │    │
│  │ConversionPipeline│ │StrategyFactory  │ │TypeRegistry     │    │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘    │
├─────────────────────────────────────────────────────────────────┤
│  策略层 (Strategy Layer)                                          │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐    │
│  │   XML策略       │ │   Excel策略     │ │   其他格式策略   │    │
│  │XmlStrategy      │ │ExcelStrategy    │ │OtherStrategies  │    │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘    │
├─────────────────────────────────────────────────────────────────┤
│  数据层 (Data Layer)                                             │
│  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐    │
│  │   模型映射器     │ │   序列化器       │ │   数据验证器     │    │
│  │ModelMapper      │ │Serializer       │ │DataValidator    │    │
│  └─────────────────┘ └─────────────────┘ └─────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

### 2.2 核心设计原则

1. **单一职责原则**: 每个组件只负责一个特定的功能
2. **开闭原则**: 对扩展开放，对修改关闭
3. **依赖倒置原则**: 依赖抽象而不是具体实现
4. **接口隔离原则**: 使用小而专一的接口
5. **策略模式**: 支持不同类型的转换策略
6. **管道模式**: 支持多步骤转换处理
7. **插件架构**: 支持动态加载转换器

## 3. 核心接口设计

### 3.1 转换器接口

```csharp
namespace BannerlordModEditor.Common.Framework.Converters
{
    /// <summary>
    /// 转换器基础接口
    /// </summary>
    public interface IConverter
    {
        string Name { get; }
        string Description { get; }
        Version Version { get; }
        IEnumerable<string> SupportedSourceFormats { get; }
        IEnumerable<string> SupportedTargetFormats { get; }
        Task<bool> CanConvertAsync(string sourceFormat, string targetFormat);
    }

    /// <summary>
    /// 通用转换器接口
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    public interface IConverter<TSource, TTarget> : IConverter
    {
        Task<ConversionResult<TSource, TTarget>> ConvertAsync(
            TSource source, 
            ConversionContext context);
    }

    /// <summary>
    /// 文件转换器接口
    /// </summary>
    public interface IFileConverter : IConverter
    {
        Task<FileConversionResult> ConvertFileAsync(
            string sourcePath, 
            string targetPath, 
            ConversionOptions options);
    }

    /// <summary>
    /// 批量转换器接口
    /// </summary>
    public interface IBatchConverter : IConverter
    {
        Task<BatchConversionResult> ConvertBatchAsync(
            IEnumerable<string> sourcePaths,
            string outputDirectory,
            ConversionOptions options);
    }
}
```

### 3.2 转换策略接口

```csharp
namespace BannerlordModEditor.Common.Framework.Strategies
{
    /// <summary>
    /// 转换策略基础接口
    /// </summary>
    public interface IConversionStrategy
    {
        string StrategyName { get; }
        string Description { get; }
        Task<bool> CanHandleAsync(ConversionContext context);
        Task<ConversionResult> ExecuteAsync(ConversionContext context);
    }

    /// <summary>
    /// XML转换策略接口
    /// </summary>
    public interface IXmlConversionStrategy : IConversionStrategy
    {
        Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlPath);
        Task<bool> ValidateXmlSchemaAsync(string xmlPath, XmlSchema schema);
    }

    /// <summary>
    /// Excel转换策略接口
    /// </summary>
    public interface IExcelConversionStrategy : IConversionStrategy
    {
        Task<ExcelAnalysisResult> AnalyzeExcelAsync(string excelPath);
        Task<WorksheetMapping> CreateMappingAsync(ExcelAnalysisResult analysis);
    }
}
```

### 3.3 转换管道接口

```csharp
namespace BannerlordModEditor.Common.Framework.Pipelines
{
    /// <summary>
    /// 转换管道步骤接口
    /// </summary>
    public interface IPipelineStep
    {
        string StepName { get; }
        int Order { get; }
        Task<StepResult> ExecuteAsync(ConversionContext context);
    }

    /// <summary>
    /// 验证步骤接口
    /// </summary>
    public interface IValidationStep : IPipelineStep
    {
        Task<ValidationResult> ValidateInputAsync(ConversionContext context);
        Task<ValidationResult> ValidateOutputAsync(ConversionContext context);
    }

    /// <summary>
    /// 转换步骤接口
    /// </summary>
    public interface IConversionStep : IPipelineStep
    {
        Task<object> TransformAsync(object input, ConversionContext context);
    }

    /// <summary>
    /// 转换管道接口
    /// </summary>
    public interface IConversionPipeline
    {
        string PipelineName { get; }
        IEnumerable<IPipelineStep> Steps { get; }
        IConversionPipeline AddStep(IPipelineStep step);
        IConversionPipeline RemoveStep(string stepName);
        Task<ConversionResult> ExecuteAsync(ConversionContext context);
    }
}
```

### 3.4 插件接口

```csharp
namespace BannerlordModEditor.Common.Framework.Plugins
{
    /// <summary>
    /// 转换插件接口
    /// </summary>
    public interface IConversionPlugin
    {
        string PluginName { get; }
        string PluginVersion { get; }
        string PluginDescription { get; }
        IEnumerable<IConverter> Converters { get; }
        IEnumerable<IConversionStrategy> Strategies { get; }
        Task InitializeAsync(IServiceProvider serviceProvider);
        Task ShutdownAsync();
    }

    /// <summary>
    /// 插件加载器接口
    /// </summary>
    public interface IPluginLoader
    {
        Task<IEnumerable<IConversionPlugin>> LoadPluginsAsync(string pluginDirectory);
        Task UnloadPluginAsync(string pluginName);
        Task<bool> IsPluginLoadedAsync(string pluginName);
    }
}
```

## 4. 核心类设计

### 4.1 转换上下文类

```csharp
namespace BannerlordModEditor.Common.Framework.Context
{
    /// <summary>
    /// 转换上下文 - 保存转换过程中的所有信息
    /// </summary>
    public class ConversionContext
    {
        public string SourceFormat { get; set; } = string.Empty;
        public string TargetFormat { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public ConversionOptions Options { get; set; } = new();
        public Dictionary<string, object> Properties { get; } = new();
        public List<ConversionMessage> Messages { get; } = new();
        public CancellationToken CancellationToken { get; set; }
        public IProgress<ConversionProgress> Progress { get; set; } = new();
        
        // 性能监控
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public TimeSpan Duration => DateTime.UtcNow - StartTime;
        
        // 类型信息
        public XmlTypeInfo? XmlTypeInfo { get; set; }
        public Type? SourceType { get; set; }
        public Type? TargetType { get; set; }
        
        // 中间数据
        public object? SourceData { get; set; }
        public object? TargetData { get; set; }
        
        // 验证结果
        public ValidationResult? ValidationResult { get; set; }
    }

    /// <summary>
    /// 转换消息
    /// </summary>
    public class ConversionMessage
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public ConversionMessageType Type { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public Exception? Exception { get; set; }
    }

    /// <summary>
    /// 转换进度
    /// </summary>
    public class ConversionProgress
    {
        public double Percentage { get; set; }
        public string Status { get; set; } = string.Empty;
        public long ProcessedItems { get; set; }
        public long TotalItems { get; set; }
        public TimeSpan EstimatedTimeRemaining { get; set; }
    }
}
```

### 4.2 转换管理器

```csharp
namespace BannerlordModEditor.Common.Framework.Managers
{
    /// <summary>
    /// 转换管理器 - 框架的核心入口点
    /// </summary>
    public class ConversionManager : IConversionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConverterRegistry _converterRegistry;
        private readonly IStrategyFactory _strategyFactory;
        private readonly IPipelineFactory _pipelineFactory;
        private readonly IPluginManager _pluginManager;
        private readonly ILogger<ConversionManager> _logger;

        public ConversionManager(
            IServiceProvider serviceProvider,
            IConverterRegistry converterRegistry,
            IStrategyFactory strategyFactory,
            IPipelineFactory pipelineFactory,
            IPluginManager pluginManager,
            ILogger<ConversionManager> logger)
        {
            _serviceProvider = serviceProvider;
            _converterRegistry = converterRegistry;
            _strategyFactory = strategyFactory;
            _pipelineFactory = pipelineFactory;
            _pluginManager = pluginManager;
            _logger = logger;
        }

        public async Task<ConversionResult> ConvertAsync<T>(
            T source,
            string targetFormat,
            ConversionOptions? options = null)
        {
            var context = new ConversionContext
            {
                SourceData = source,
                SourceType = typeof(T),
                TargetFormat = targetFormat,
                Options = options ?? new ConversionOptions()
            };

            return await ConvertInternalAsync(context);
        }

        public async Task<FileConversionResult> ConvertFileAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions? options = null)
        {
            var context = new ConversionContext
            {
                SourcePath = sourcePath,
                TargetPath = targetPath,
                SourceFormat = Path.GetExtension(sourcePath).TrimStart('.'),
                TargetFormat = Path.GetExtension(targetPath).TrimStart('.'),
                Options = options ?? new ConversionOptions()
            };

            var result = await ConvertInternalAsync(context);
            
            return new FileConversionResult
            {
                Success = result.Success,
                Message = result.Message,
                SourcePath = sourcePath,
                TargetPath = targetPath,
                Duration = result.Duration,
                Errors = result.Errors,
                Warnings = result.Warnings
            };
        }

        private async Task<ConversionResult> ConvertInternalAsync(ConversionContext context)
        {
            try
            {
                _logger.LogInformation("Starting conversion from {SourceFormat} to {TargetFormat}",
                    context.SourceFormat, context.TargetFormat);

                // 1. 选择合适的转换策略
                var strategy = await _strategyFactory.CreateStrategyAsync(context);
                if (strategy == null)
                {
                    return new ConversionResult
                    {
                        Success = false,
                        Message = $"No conversion strategy found for {context.SourceFormat} to {context.TargetFormat}",
                        Errors = { $"Unsupported conversion: {context.SourceFormat} -> {context.TargetFormat}" }
                    };
                }

                // 2. 创建转换管道
                var pipeline = await _pipelineFactory.CreatePipelineAsync(context, strategy);
                
                // 3. 执行转换
                return await pipeline.ExecuteAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Conversion failed");
                return new ConversionResult
                {
                    Success = false,
                    Message = "Conversion failed",
                    Errors = { ex.Message },
                    Duration = context.Duration
                };
            }
        }

        // 其他方法...
    }
}
```

### 4.3 类型注册器

```csharp
namespace BannerlordModEditor.Common.Framework.Registries
{
    /// <summary>
    /// 转换器注册器
    /// </summary>
    public interface IConverterRegistry
    {
        void RegisterConverter(IConverter converter);
        void UnregisterConverter(string converterName);
        IConverter? GetConverter(string converterName);
        IEnumerable<IConverter> GetConvertersForConversion(string sourceFormat, string targetFormat);
        IEnumerable<IConverter> AllConverters { get; }
    }

    /// <summary>
    /// 转换器注册器实现
    /// </summary>
    public class ConverterRegistry : IConverterRegistry
    {
        private readonly Dictionary<string, IConverter> _converters = new();
        private readonly Dictionary<(string Source, string Target), List<IConverter>> _conversionMap = new();

        public void RegisterConverter(IConverter converter)
        {
            _converters[converter.Name] = converter;

            // 更新转换映射
            foreach (var sourceFormat in converter.SupportedSourceFormats)
            {
                foreach (var targetFormat in converter.SupportedTargetFormats)
                {
                    var key = (sourceFormat, targetFormat);
                    if (!_conversionMap.ContainsKey(key))
                    {
                        _conversionMap[key] = new List<IConverter>();
                    }
                    _conversionMap[key].Add(converter);
                }
            }
        }

        public IEnumerable<IConverter> GetConvertersForConversion(string sourceFormat, string targetFormat)
        {
            var key = (sourceFormat, targetFormat);
            return _conversionMap.TryGetValue(key, out var converters) 
                ? converters 
                : Enumerable.Empty<IConverter>();
        }

        // 其他方法实现...
    }
}
```

## 5. 转换策略设计

### 5.1 XML到Excel策略

```csharp
namespace BannerlordModEditor.Common.Framework.Strategies.XmlToExcel
{
    /// <summary>
    /// XML到Excel转换策略
    /// </summary>
    public class XmlToExcelStrategy : IXmlConversionStrategy
    {
        public string StrategyName => "XmlToExcel";
        public string Description => "Convert XML files to Excel format";

        private readonly IXmlTypeDetectionService _xmlTypeDetection;
        private readonly IModelMapper _modelMapper;
        private readonly ILogger<XmlToExcelStrategy> _logger;

        public XmlToExcelStrategy(
            IXmlTypeDetectionService xmlTypeDetection,
            IModelMapper modelMapper,
            ILogger<XmlToExcelStrategy> logger)
        {
            _xmlTypeDetection = xmlTypeDetection;
            _modelMapper = modelMapper;
            _logger = logger;
        }

        public async Task<bool> CanHandleAsync(ConversionContext context)
        {
            return context.SourceFormat.Equals("xml", StringComparison.OrdinalIgnoreCase) &&
                   context.TargetFormat.Equals("xlsx", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<ConversionResult> ExecuteAsync(ConversionContext context)
        {
            try
            {
                // 1. 检测XML类型
                var xmlTypeInfo = await _xmlTypeDetection.DetectXmlTypeAsync(context.SourcePath!);
                context.XmlTypeInfo = xmlTypeInfo;

                // 2. 加载XML数据
                var loadDataResult = await LoadXmlDataAsync(context);
                if (!loadDataResult.Success)
                {
                    return loadDataResult;
                }

                // 3. 转换为Excel
                var excelData = await ConvertToExcelAsync(context);

                // 4. 保存Excel文件
                await SaveExcelAsync(excelData, context.TargetPath!);

                return new ConversionResult
                {
                    Success = true,
                    Message = $"Successfully converted XML to Excel",
                    Duration = context.Duration
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "XML to Excel conversion failed");
                return new ConversionResult
                {
                    Success = false,
                    Message = "Conversion failed",
                    Errors = { ex.Message },
                    Duration = context.Duration
                };
            }
        }

        private async Task<ConversionResult> LoadXmlDataAsync(ConversionContext context)
        {
            // 根据XML类型选择适当的加载器
            if (context.XmlTypeInfo?.IsAdapted == true)
            {
                // 使用类型化加载器
                var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(
                    Type.GetType(context.XmlTypeInfo.ModelType)!);
                var loader = Activator.CreateInstance(loaderType) as dynamic;
                
                var data = await loader.LoadAsync(context.SourcePath);
                context.SourceData = data;
                
                return new ConversionResult { Success = true };
            }
            else
            {
                // 使用通用XML加载器
                var xmlDoc = XDocument.Load(context.SourcePath);
                context.SourceData = xmlDoc;
                
                return new ConversionResult { Success = true };
            }
        }

        private async Task<object> ConvertToExcelAsync(ConversionContext context)
        {
            // 实现XML到Excel的转换逻辑
            // 可以使用ClosedXML或其他Excel库
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");

            if (context.SourceData is XDocument xmlDoc)
            {
                // 通用XML转换
                await ConvertGenericXmlToExcel(xmlDoc, worksheet);
            }
            else
            {
                // 类型化XML转换
                await ConvertTypedXmlToExcel(context.SourceData!, worksheet);
            }

            return workbook;
        }

        // 其他辅助方法...
    }
}
```

### 5.2 Excel到XML策略

```csharp
namespace BannerlordModEditor.Common.Framework.Strategies.ExcelToXml
{
    /// <summary>
    /// Excel到XML转换策略
    /// </summary>
    public class ExcelToXmlStrategy : IExcelConversionStrategy
    {
        public string StrategyName => "ExcelToXml";
        public string Description => "Convert Excel files to XML format";

        private readonly IModelMapper _modelMapper;
        private readonly ILogger<ExcelToXmlStrategy> _logger;

        public ExcelToXmlStrategy(
            IModelMapper modelMapper,
            ILogger<ExcelToXmlStrategy> logger)
        {
            _modelMapper = modelMapper;
            _logger = logger;
        }

        public async Task<bool> CanHandleAsync(ConversionContext context)
        {
            return context.SourceFormat.Equals("xlsx", StringComparison.OrdinalIgnoreCase) &&
                   context.TargetFormat.Equals("xml", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<ConversionResult> ExecuteAsync(ConversionContext context)
        {
            try
            {
                // 1. 分析Excel结构
                var analysisResult = await AnalyzeExcelAsync(context.SourcePath!);
                
                // 2. 创建映射
                var mapping = await CreateMappingAsync(analysisResult);
                
                // 3. 转换为XML
                var xmlData = await ConvertToXmlAsync(context, mapping);
                
                // 4. 保存XML文件
                await SaveXmlAsync(xmlData, context.TargetPath!);

                return new ConversionResult
                {
                    Success = true,
                    Message = $"Successfully converted Excel to XML",
                    Duration = context.Duration
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel to XML conversion failed");
                return new ConversionResult
                {
                    Success = false,
                    Message = "Conversion failed",
                    Errors = { ex.Message },
                    Duration = context.Duration
                };
            }
        }

        public async Task<ExcelAnalysisResult> AnalyzeExcelAsync(string excelPath)
        {
            using var workbook = new XLWorkbook(excelPath);
            var worksheet = workbook.Worksheets.First();

            var result = new ExcelAnalysisResult
            {
                ColumnNames = worksheet.FirstRowUsed()
                    .Cells()
                    .Select(c => c.Value.ToString())
                    .ToList(),
                RowCount = worksheet.RowsUsed().Count() - 1,
                WorksheetName = worksheet.Name
            };

            return await Task.FromResult(result);
        }

        public async Task<WorksheetMapping> CreateMappingAsync(ExcelAnalysisResult analysis)
        {
            // 创建列名到XML元素的映射
            var mapping = new WorksheetMapping();
            
            foreach (var columnName in analysis.ColumnNames)
            {
                mapping.AddColumnMapping(columnName, columnName.ToXmlName());
            }

            return await Task.FromResult(mapping);
        }

        private async Task<object> ConvertToXmlAsync(ConversionContext context, WorksheetMapping mapping)
        {
            using var workbook = new XLWorkbook(context.SourcePath);
            var worksheet = workbook.Worksheets.First();

            if (context.XmlTypeInfo?.IsAdapted == true)
            {
                // 转换为类型化XML
                return await ConvertToTypedXmlAsync(worksheet, mapping, context);
            }
            else
            {
                // 转换为通用XML
                return await ConvertToGenericXmlAsync(worksheet, mapping, context);
            }
        }

        // 其他辅助方法...
    }
}
```

## 6. 转换管道设计

### 6.1 预定义管道步骤

```csharp
namespace BannerlordModEditor.Common.Framework.Pipelines.Steps
{
    /// <summary>
    /// 文件验证步骤
    /// </summary>
    public class FileValidationStep : IValidationStep
    {
        public string StepName => "FileValidation";
        public int Order => 1;

        public async Task<StepResult> ExecuteAsync(ConversionContext context)
        {
            var result = new StepResult { Success = true };

            // 验证源文件
            if (!File.Exists(context.SourcePath))
            {
                result.Success = false;
                result.Errors.Add($"Source file not found: {context.SourcePath}");
                return result;
            }

            // 验证目标目录
            var targetDir = Path.GetDirectoryName(context.TargetPath);
            if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
            {
                try
                {
                    Directory.CreateDirectory(targetDir);
                }
                catch (Exception ex)
                {
                    result.Success = false;
                    result.Errors.Add($"Cannot create target directory: {ex.Message}");
                    return result;
                }
            }

            // 验证文件权限
            try
            {
                using var stream = File.OpenRead(context.SourcePath);
            }
            catch (UnauthorizedAccessException)
            {
                result.Success = false;
                result.Errors.Add($"Access denied to source file: {context.SourcePath}");
                return result;
            }

            return await Task.FromResult(result);
        }

        public Task<ValidationResult> ValidateInputAsync(ConversionContext context)
        {
            var result = new ValidationResult();
            
            if (!File.Exists(context.SourcePath))
            {
                result.IsValid = false;
                result.Errors.Add(new ValidationError
                {
                    Message = $"Source file not found: {context.SourcePath}",
                    ErrorType = ValidationErrorType.FileNotFound
                });
            }

            return Task.FromResult(result);
        }

        public Task<ValidationResult> ValidateOutputAsync(ConversionContext context)
        {
            return Task.FromResult(new ValidationResult { IsValid = true });
        }
    }

    /// <summary>
    /// 备份步骤
    /// </summary>
    public class BackupStep : IPipelineStep
    {
        public string StepName => "Backup";
        public int Order => 2;

        public async Task<StepResult> ExecuteAsync(ConversionContext context)
        {
            if (!context.Options.CreateBackup)
            {
                return new StepResult { Success = true };
            }

            var result = new StepResult { Success = true };

            try
            {
                var backupPath = $"{context.TargetPath}.backup_{DateTime.UtcNow:yyyyMMddHHmmss}";
                File.Copy(context.TargetPath, backupPath, true);
                
                context.Messages.Add(new ConversionMessage
                {
                    Type = ConversionMessageType.Info,
                    Message = $"Backup created: {backupPath}"
                });
            }
            catch (Exception ex)
            {
                result.Warnings.Add($"Failed to create backup: {ex.Message}");
            }

            return await Task.FromResult(result);
        }
    }

    /// <summary>
    /// 格式检测步骤
    /// </summary>
    public class FormatDetectionStep : IPipelineStep
    {
        private readonly IFormatDetectionService _formatDetection;

        public FormatDetectionStep(IFormatDetectionService formatDetection)
        {
            _formatDetection = formatDetection;
        }

        public string StepName => "FormatDetection";
        public int Order => 3;

        public async Task<StepResult> ExecuteAsync(ConversionContext context)
        {
            var result = new StepResult { Success = true };

            // 检测源文件格式
            var sourceFormatInfo = await _formatDetection.DetectFormatAsync(context.SourcePath);
            if (!sourceFormatInfo.IsSupported)
            {
                result.Success = false;
                result.Errors.Add($"Unsupported source format: {sourceFormatInfo.Format}");
                return result;
            }

            context.Properties["SourceFormatInfo"] = sourceFormatInfo;

            // 如果是XML，检测类型
            if (sourceFormatInfo.Format == "xml")
            {
                var xmlTypeInfo = await _formatDetection.DetectXmlTypeAsync(context.SourcePath);
                context.XmlTypeInfo = xmlTypeInfo;
            }

            return result;
        }
    }

    /// <summary>
    /// 数据转换步骤
    /// </summary>
    public class DataTransformationStep : IConversionStep
    {
        private readonly IModelMapper _modelMapper;

        public DataTransformationStep(IModelMapper modelMapper)
        {
            _modelMapper = modelMapper;
        }

        public string StepName => "DataTransformation";
        public int Order => 4;

        public async Task<object> TransformAsync(object input, ConversionContext context)
        {
            // 根据上下文选择适当的转换器
            if (context.SourceFormat == "xml" && context.TargetFormat == "xlsx")
            {
                return await _modelMapper.MapXmlToExcelAsync(input, context);
            }
            else if (context.SourceFormat == "xlsx" && context.TargetFormat == "xml")
            {
                return await _modelMapper.MapExcelToXmlAsync(input, context);
            }
            else
            {
                throw new NotSupportedException(
                    $"Conversion from {context.SourceFormat} to {context.TargetFormat} is not supported");
            }
        }
    }

    /// <summary>
    /// 文件保存步骤
    /// </summary>
    public class FileSaveStep : IPipelineStep
    {
        public string StepName => "FileSave";
        public int Order => 5;

        public async Task<StepResult> ExecuteAsync(ConversionContext context)
        {
            var result = new StepResult { Success = true };

            try
            {
                if (context.TargetData is XLWorkbook excelWorkbook)
                {
                    excelWorkbook.SaveAs(context.TargetPath);
                }
                else if (context.TargetData is XDocument xmlDocument)
                {
                    var settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "\t",
                        Encoding = Encoding.UTF8
                    };

                    using var writer = XmlWriter.Create(context.TargetPath, settings);
                    xmlDocument.Save(writer);
                }
                else
                {
                    result.Success = false;
                    result.Errors.Add("Unsupported target data type");
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add($"Failed to save file: {ex.Message}");
            }

            return await Task.FromResult(result);
        }
    }

    /// <summary>
    /// 验证步骤
    /// </summary>
    public class ValidationStep : IValidationStep
    {
        private readonly IConversionValidator _validator;

        public ValidationStep(IConversionValidator validator)
        {
            _validator = validator;
        }

        public string StepName => "Validation";
        public int Order => 6;

        public async Task<StepResult> ExecuteAsync(ConversionContext context)
        {
            var result = new StepResult { Success = true };

            if (context.Options.IncludeValidation)
            {
                var validationResult = await _validator.ValidateConversionAsync(
                    context.SourcePath,
                    context.TargetPath,
                    context);

                context.ValidationResult = validationResult;

                if (!validationResult.IsValid)
                {
                    result.Success = false;
                    result.Errors.AddRange(validationResult.Errors.Select(e => e.Message));
                }

                result.Warnings.AddRange(validationResult.Warnings.Select(w => w.Message));
            }

            return await Task.FromResult(result);
        }

        public Task<ValidationResult> ValidateInputAsync(ConversionContext context)
        {
            return Task.FromResult(new ValidationResult { IsValid = true });
        }

        public Task<ValidationResult> ValidateOutputAsync(ConversionContext context)
        {
            return _validator.ValidateConversionAsync(
                context.SourcePath,
                context.TargetPath,
                context);
        }
    }
}
```

## 7. 插件系统设计

### 7.1 插件基类

```csharp
namespace BannerlordModEditor.Common.Framework.Plugins
{
    /// <summary>
    /// 转换插件基类
    /// </summary>
    public abstract class ConversionPluginBase : IConversionPlugin
    {
        public abstract string PluginName { get; }
        public abstract string PluginVersion { get; }
        public abstract string PluginDescription { get; }
        
        protected List<IConverter> ConvertersList { get; } = new();
        protected List<IConversionStrategy> StrategiesList { get; } = new();

        public virtual IEnumerable<IConverter> Converters => ConvertersList;
        public virtual IEnumerable<IConversionStrategy> Strategies => StrategiesList;

        public virtual Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // 子类可以重写此方法进行初始化
            return Task.CompletedTask;
        }

        public virtual Task ShutdownAsync()
        {
            // 子类可以重写此方法进行清理
            return Task.CompletedTask;
        }

        protected void AddConverter(IConverter converter)
        {
            ConvertersList.Add(converter);
        }

        protected void AddStrategy(IConversionStrategy strategy)
        {
            StrategiesList.Add(strategy);
        }
    }

    /// <summary>
    /// 插件加载器实现
    /// </summary>
    public class PluginLoader : IPluginLoader
    {
        private readonly ILogger<PluginLoader> _logger;
        private readonly Dictionary<string, IConversionPlugin> _loadedPlugins = new();

        public PluginLoader(ILogger<PluginLoader> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<IConversionPlugin>> LoadPluginsAsync(string pluginDirectory)
        {
            if (!Directory.Exists(pluginDirectory))
            {
                _logger.LogWarning("Plugin directory not found: {PluginDirectory}", pluginDirectory);
                return Enumerable.Empty<IConversionPlugin>();
            }

            var plugins = new List<IConversionPlugin>();

            foreach (var dllFile in Directory.GetFiles(pluginDirectory, "*.dll"))
            {
                try
                {
                    var plugin = await LoadPluginFromAssemblyAsync(dllFile);
                    if (plugin != null)
                    {
                        plugins.Add(plugin);
                        _loadedPlugins[plugin.PluginName] = plugin;
                        _logger.LogInformation("Loaded plugin: {PluginName} v{PluginVersion}", 
                            plugin.PluginName, plugin.PluginVersion);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load plugin from {DllFile}", dllFile);
                }
            }

            return plugins;
        }

        private async Task<IConversionPlugin?> LoadPluginFromAssemblyAsync(string dllPath)
        {
            var assembly = Assembly.LoadFrom(dllPath);
            
            // 查找实现IConversionPlugin的类型
            var pluginTypes = assembly.GetTypes()
                .Where(t => typeof(IConversionPlugin).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();

            if (pluginTypes.Count == 0)
            {
                _logger.LogWarning("No plugin types found in {DllPath}", dllPath);
                return null;
            }

            if (pluginTypes.Count > 1)
            {
                _logger.LogWarning("Multiple plugin types found in {DllPath}, using first one", dllPath);
            }

            var pluginType = pluginTypes.First();
            var plugin = (IConversionPlugin)Activator.CreateInstance(pluginType)!;

            // 初始化插件
            await plugin.InitializeAsync(CreateServiceProviderForPlugin(assembly));

            return plugin;
        }

        private IServiceProvider CreateServiceProviderForPlugin(Assembly assembly)
        {
            // 为插件创建服务提供者
            var services = new ServiceCollection();
            
            // 插件可以注册自己的服务
            var pluginStartupTypes = assembly.GetTypes()
                .Where(t => typeof(IPluginStartup).IsAssignableFrom(t))
                .ToList();

            foreach (var startupType in pluginStartupTypes)
            {
                var startup = (IPluginStartup)Activator.CreateInstance(startupType)!;
                startup.ConfigureServices(services);
            }

            return services.BuildServiceProvider();
        }

        public async Task UnloadPluginAsync(string pluginName)
        {
            if (_loadedPlugins.TryGetValue(pluginName, out var plugin))
            {
                await plugin.ShutdownAsync();
                _loadedPlugins.Remove(pluginName);
                _logger.LogInformation("Unloaded plugin: {PluginName}", pluginName);
            }
        }

        public Task<bool> IsPluginLoadedAsync(string pluginName)
        {
            return Task.FromResult(_loadedPlugins.ContainsKey(pluginName));
        }
    }

    /// <summary>
    /// 插件启动接口
    /// </summary>
    public interface IPluginStartup
    {
        void ConfigureServices(IServiceCollection services);
    }
}
```

### 7.2 示例插件

```csharp
namespace BannerlordModEditor.Common.Framework.Plugins.Samples
{
    /// <summary>
    /// JSON转换插件示例
    /// </summary>
    public class JsonConversionPlugin : ConversionPluginBase
    {
        public override string PluginName => "JsonConverter";
        public override string PluginVersion => "1.0.0";
        public override string PluginDescription => "Provides JSON conversion capabilities";

        public override Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // 注册JSON转换器
            AddConverter(new JsonToXmlConverter(serviceProvider));
            AddConverter(new XmlToJsonConverter(serviceProvider));
            
            // 注册JSON策略
            AddStrategy(new JsonConversionStrategy(serviceProvider));
            
            return base.InitializeAsync(serviceProvider);
        }
    }

    /// <summary>
    /// JSON到XML转换器
    /// </summary>
    public class JsonToXmlConverter : IFileConverter
    {
        public string Name => "JsonToXml";
        public string Description => "Convert JSON files to XML format";
        public Version Version => new Version(1, 0, 0);
        public IEnumerable<string> SupportedSourceFormats => new[] { "json" };
        public IEnumerable<string> SupportedTargetFormats => new[] { "xml" };

        private readonly IServiceProvider _serviceProvider;

        public JsonToXmlConverter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> CanConvertAsync(string sourceFormat, string targetFormat)
        {
            return sourceFormat.Equals("json", StringComparison.OrdinalIgnoreCase) &&
                   targetFormat.Equals("xml", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<FileConversionResult> ConvertFileAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            try
            {
                var json = await File.ReadAllTextAsync(sourcePath);
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);
                
                var xml = ConvertJsonToXml(jsonObject, Path.GetFileNameWithoutExtension(sourcePath));
                
                await File.WriteAllTextAsync(targetPath, xml);
                
                return new FileConversionResult
                {
                    Success = true,
                    Message = "JSON to XML conversion completed successfully",
                    SourcePath = sourcePath,
                    TargetPath = targetPath
                };
            }
            catch (Exception ex)
            {
                return new FileConversionResult
                {
                    Success = false,
                    Message = "JSON to XML conversion failed",
                    Errors = { ex.Message },
                    SourcePath = sourcePath,
                    TargetPath = targetPath
                };
            }
        }

        private string ConvertJsonToXml(JsonElement jsonElement, string rootName)
        {
            var xml = new XDocument();
            var root = new XElement(rootName);
            xml.Add(root);
            
            ConvertJsonElementToXml(jsonElement, root);
            
            return xml.ToString();
        }

        private void ConvertJsonElementToXml(JsonElement jsonElement, XElement parentElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in jsonElement.EnumerateObject())
                    {
                        var element = new XElement(property.Name.ToXmlName());
                        parentElement.Add(element);
                        ConvertJsonElementToXml(property.Value, element);
                    }
                    break;
                    
                case JsonValueKind.Array:
                    foreach (var item in jsonElement.EnumerateArray())
                    {
                        var element = new XElement("Item");
                        parentElement.Add(element);
                        ConvertJsonElementToXml(item, element);
                    }
                    break;
                    
                default:
                    parentElement.Value = jsonElement.ToString();
                    break;
            }
        }
    }
}
```

## 8. 配置系统设计

### 8.1 配置模型

```csharp
namespace BannerlordModEditor.Common.Framework.Configuration
{
    /// <summary>
    /// 框架配置
    /// </summary>
    public class ConversionFrameworkConfiguration
    {
        public const string SectionName = "ConversionFramework";

        // 插件配置
        public PluginConfiguration Plugins { get; set; } = new();
        
        // 转换器配置
        public Dictionary<string, ConverterConfiguration> Converters { get; set; } = new();
        
        // 策略配置
        public Dictionary<string, StrategyConfiguration> Strategies { get; set; } = new();
        
        // 管道配置
        public Dictionary<string, PipelineConfiguration> Pipelines { get; set; } = new();
        
        // 性能配置
        public PerformanceConfiguration Performance { get; set; } = new();
        
        // 日志配置
        public LoggingConfiguration Logging { get; set; } = new();
    }

    /// <summary>
    /// 插件配置
    /// </summary>
    public class PluginConfiguration
    {
        public string PluginDirectory { get; set; } = "plugins";
        public bool AutoLoadPlugins { get; set; } = true;
        public List<string> DisabledPlugins { get; set; } = new();
        public Dictionary<string, object> PluginSettings { get; set; } = new();
    }

    /// <summary>
    /// 转换器配置
    /// </summary>
    public class ConverterConfiguration
    {
        public bool Enabled { get; set; } = true;
        public int Priority { get; set; } = 0;
        public Dictionary<string, object> Settings { get; set; } = new();
    }

    /// <summary>
    /// 策略配置
    /// </summary>
    public class StrategyConfiguration
    {
        public bool Enabled { get; set; } = true;
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    /// <summary>
    /// 管道配置
    /// </summary>
    public class PipelineConfiguration
    {
        public List<string> Steps { get; set; } = new();
        public Dictionary<string, object> StepSettings { get; set; } = new();
        public bool ParallelExecution { get; set; } = false;
    }

    /// <summary>
    /// 性能配置
    /// </summary>
    public class PerformanceConfiguration
    {
        public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
        public int BufferSize { get; set; } = 8192;
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(30);
        public bool EnableCaching { get; set; } = true;
        public int CacheSize { get; set; } = 1000;
        public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromHours(1);
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LoggingConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        public bool EnableDetailedLogging { get; set; } = false;
        public string LogFilePath { get; set; } = "logs/conversion.log";
        public int MaxLogFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
        public int MaxLogFiles { get; set; } = 5;
    }
}
```

### 8.2 配置管理器

```csharp
namespace BannerlordModEditor.Common.Framework.Configuration
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigurationManager> _logger;
        private ConversionFrameworkConfiguration? _frameworkConfig;

        public ConfigurationManager(
            IConfiguration configuration,
            ILogger<ConfigurationManager> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public ConversionFrameworkConfiguration GetFrameworkConfiguration()
        {
            if (_frameworkConfig == null)
            {
                _frameworkConfig = _configuration
                    .GetSection(ConversionFrameworkConfiguration.SectionName)
                    .Get<ConversionFrameworkConfiguration>() ?? new ConversionFrameworkConfiguration();
            }

            return _frameworkConfig;
        }

        public T GetConverterConfiguration<T>(string converterName) where T : new()
        {
            var config = GetFrameworkConfiguration();
            
            if (config.Converters.TryGetValue(converterName, out var converterConfig))
            {
                return converterConfig.Settings.ToObject<T>() ?? new T();
            }

            return new T();
        }

        public T GetStrategyConfiguration<T>(string strategyName) where T : new()
        {
            var config = GetFrameworkConfiguration();
            
            if (config.Strategies.TryGetValue(strategyName, out var strategyConfig))
            {
                return strategyConfig.Parameters.ToObject<T>() ?? new T();
            }

            return new T();
        }

        public async Task SaveConfigurationAsync()
        {
            try
            {
                var configPath = Path.Combine(AppContext.BaseDirectory, "config", "conversion.json");
                var directory = Path.GetDirectoryName(configPath);
                
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(GetFrameworkConfiguration(), new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await File.WriteAllTextAsync(configPath, json);
                
                _logger.LogInformation("Configuration saved to {ConfigPath}", configPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save configuration");
            }
        }

        public async Task ReloadConfigurationAsync()
        {
            try
            {
                var configPath = Path.Combine(AppContext.BaseDirectory, "config", "conversion.json");
                
                if (File.Exists(configPath))
                {
                    var json = await File.ReadAllTextAsync(configPath);
                    _frameworkConfig = JsonSerializer.Deserialize<ConversionFrameworkConfiguration>(json);
                    
                    _logger.LogInformation("Configuration reloaded from {ConfigPath}", configPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reload configuration");
            }
        }
    }
}
```

## 9. 性能优化设计

### 9.1 缓存系统

```csharp
namespace BannerlordModEditor.Common.Framework.Caching
{
    /// <summary>
    /// 转换缓存接口
    /// </summary>
    public interface IConversionCache
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task ClearAsync();
        Task<bool> ExistsAsync(string key);
    }

    /// <summary>
    /// 内存缓存实现
    /// </summary>
    public class MemoryConversionCache : IConversionCache
    {
        private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
        private readonly TimeSpan _defaultExpiration;
        private readonly Timer _cleanupTimer;
        private readonly ILogger<MemoryConversionCache> _logger;

        public MemoryConversionCache(
            TimeSpan defaultExpiration,
            ILogger<MemoryConversionCache> logger)
        {
            _defaultExpiration = defaultExpiration;
            _logger = logger;
            
            // 定期清理过期缓存
            _cleanupTimer = new Timer(CleanupExpiredEntries, null, 
                TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                if (entry.Expiration > DateTime.UtcNow)
                {
                    return (T?)entry.Value;
                }
                else
                {
                    // 移除过期条目
                    await RemoveAsync(key);
                }
            }

            return default;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var entry = new CacheEntry
            {
                Value = value,
                Expiration = DateTime.UtcNow.Add(expiration ?? _defaultExpiration)
            };

            _cache[key] = entry;
            
            _logger.LogDebug("Cache entry added: {Key}", key);
            
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.TryRemove(key, out _);
            _logger.LogDebug("Cache entry removed: {Key}", key);
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            _cache.Clear();
            _logger.LogInformation("Cache cleared");
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string key)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                if (entry.Expiration > DateTime.UtcNow)
                {
                    return Task.FromResult(true);
                }
                else
                {
                    return RemoveAsync(key).ContinueWith(_ => false);
                }
            }

            return Task.FromResult(false);
        }

        private void CleanupExpiredEntries(object? state)
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _cache
                .Where(kvp => kvp.Value.Expiration <= now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
            }

            if (expiredKeys.Count > 0)
            {
                _logger.LogDebug("Cleaned up {Count} expired cache entries", expiredKeys.Count);
            }
        }

        private class CacheEntry
        {
            public object Value { get; set; } = null!;
            public DateTime Expiration { get; set; }
        }
    }

    /// <summary>
    /// 缓存装饰器基类
    /// </summary>
    public abstract class CacheDecoratorBase : IConversionCache
    {
        protected readonly IConversionCache InnerCache;

        protected CacheDecoratorBase(IConversionCache innerCache)
        {
            InnerCache = innerCache;
        }

        public virtual Task<T?> GetAsync<T>(string key)
        {
            return InnerCache.GetAsync<T>(key);
        }

        public virtual Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            return InnerCache.SetAsync(key, value, expiration);
        }

        public virtual Task RemoveAsync(string key)
        {
            return InnerCache.RemoveAsync(key);
        }

        public virtual Task ClearAsync()
        {
            return InnerCache.ClearAsync();
        }

        public virtual Task<bool> ExistsAsync(string key)
        {
            return InnerCache.ExistsAsync(key);
        }
    }

    /// <summary>
    /// 日志缓存装饰器
    /// </summary>
    public class LoggingCacheDecorator : CacheDecoratorBase
    {
        private readonly ILogger<LoggingCacheDecorator> _logger;

        public LoggingCacheDecorator(
            IConversionCache innerCache,
            ILogger<LoggingCacheDecorator> logger) : base(innerCache)
        {
            _logger = logger;
        }

        public override async Task<T?> GetAsync<T>(string key)
        {
            _logger.LogDebug("Cache get: {Key}", key);
            var result = await InnerCache.GetAsync<T>(key);
            _logger.LogDebug("Cache get result: {Key}, Found: {Found}", key, result != null);
            return result;
        }

        public override async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            _logger.LogDebug("Cache set: {Key}", key);
            await InnerCache.SetAsync(key, value, expiration);
        }
    }
}
```

### 9.2 并行处理

```csharp
namespace BannerlordModEditor.Common.Framework.Parallel
{
    /// <summary>
    /// 并行转换处理器
    /// </summary>
    public class ParallelConversionProcessor : IParallelConversionProcessor
    {
        private readonly IConversionManager _conversionManager;
        private readonly ILogger<ParallelConversionProcessor> _logger;
        private readonly ParallelOptions _parallelOptions;

        public ParallelConversionProcessor(
            IConversionManager conversionManager,
            ILogger<ParallelConversionProcessor> logger,
            int maxDegreeOfParallelism)
        {
            _conversionManager = conversionManager;
            _logger = logger;
            _parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
        }

        public async Task<BatchConversionResult> ProcessBatchAsync(
            IEnumerable<ConversionTask> tasks,
            ConversionOptions options)
        {
            var taskList = tasks.ToList();
            var results = new ConcurrentBag<ConversionResult>();
            var progress = new Progress<ConversionProgress>();
            
            progress.ProgressChanged += (sender, progressInfo) =>
            {
                _logger.LogInformation("Batch progress: {Percentage}%", progressInfo.Percentage);
            };

            await Parallel.ForEachAsync(taskList, _parallelOptions, async (task, ct) =>
            {
                try
                {
                    var result = await _conversionManager.ConvertFileAsync(
                        task.SourcePath,
                        task.TargetPath,
                        options);
                    
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Batch conversion failed for {SourcePath}", task.SourcePath);
                    results.Add(new ConversionResult
                    {
                        Success = false,
                        Message = "Conversion failed",
                        Errors = { ex.Message }
                    });
                }
            });

            return new BatchConversionResult
            {
                TotalTasks = taskList.Count,
                SuccessfulTasks = results.Count(r => r.Success),
                FailedTasks = results.Count(r => !r.Success),
                Results = results.ToList(),
                Duration = TimeSpan.Zero // 需要计算总耗时
            };
        }
    }

    /// <summary>
    /// 转换任务
    /// </summary>
    public class ConversionTask
    {
        public string SourcePath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public string SourceFormat { get; set; } = string.Empty;
        public string TargetFormat { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// 批量转换结果
    /// </summary>
    public class BatchConversionResult
    {
        public int TotalTasks { get; set; }
        public int SuccessfulTasks { get; set; }
        public int FailedTasks { get; set; }
        public List<ConversionResult> Results { get; set; } = new();
        public TimeSpan Duration { get; set; }
        public double SuccessRate => TotalTasks > 0 ? (double)SuccessfulTasks / TotalTasks * 100 : 0;
    }
}
```

## 10. 使用示例

### 10.1 基本使用

```csharp
// 1. 配置服务
var services = new ServiceCollection();
services.AddConversionFramework();

// 2. 构建服务提供者
var serviceProvider = services.BuildServiceProvider();

// 3. 获取转换管理器
var conversionManager = serviceProvider.GetRequiredService<IConversionManager>();

// 4. 执行转换
var result = await conversionManager.ConvertFileAsync(
    "data/input.xml",
    "output/output.xlsx",
    new ConversionOptions
    {
        CreateBackup = true,
        IncludeValidation = true,
        PreserveFormatting = true
    });

if (result.Success)
{
    Console.WriteLine("转换成功！");
    Console.WriteLine($"处理了 {result.RecordsProcessed} 条记录");
    Console.WriteLine($"耗时: {result.Duration.TotalMilliseconds}ms");
}
else
{
    Console.WriteLine("转换失败：");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"- {error}");
    }
}
```

### 10.2 自定义管道

```csharp
// 创建自定义管道
var pipeline = new ConversionPipeline("CustomPipeline");
pipeline.AddStep(new FileValidationStep())
        .AddStep(new CustomPreprocessingStep())
        .AddStep(new DataTransformationStep())
        .AddStep(new CustomPostprocessingStep())
        .AddStep(new FileSaveStep());

// 执行管道
var context = new ConversionContext
{
    SourcePath = "input.xml",
    TargetPath = "output.xlsx",
    Options = new ConversionOptions()
};

var result = await pipeline.ExecuteAsync(context);
```

### 10.3 插件开发

```csharp
// 开发自定义插件
public class CustomXmlConverterPlugin : ConversionPluginBase
{
    public override string PluginName => "CustomXmlConverter";
    public override string PluginVersion => "1.0.0";
    public override string PluginDescription => "Custom XML conversion logic";

    public override async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        // 注册自定义转换器
        AddConverter(new CustomXmlConverter());
        
        // 注册自定义策略
        AddStrategy(new CustomXmlStrategy(serviceProvider));
        
        await base.InitializeAsync(serviceProvider);
    }
}
```

## 11. 总结

本设计文档提出了一个完整的通用XML转换框架架构，具有以下特点：

1. **高度可扩展**: 通过插件系统和策略模式，支持动态添加新的转换器
2. **模块化设计**: 各组件职责明确，便于维护和测试
3. **管道模式**: 支持灵活的转换流程定制
4. **性能优化**: 包含缓存、并行处理等性能优化机制
5. **完善的配置系统**: 支持灵活的配置管理
6. **健壮的错误处理**: 全面的错误处理和日志记录

该架构可以很好地满足Bannerlord Mod Editor的XML转换需求，同时也为未来的扩展提供了良好的基础。