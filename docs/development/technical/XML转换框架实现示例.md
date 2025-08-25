# 通用XML转换框架实现示例

## 1. 核心框架实现

### 1.1 框架入口和依赖注入配置

```csharp
namespace BannerlordModEditor.Common.Framework.Extensions
{
    /// <summary>
    /// 服务集合扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加转换框架服务
        /// </summary>
        public static IServiceCollection AddConversionFramework(
            this IServiceCollection services,
            Action<ConversionFrameworkConfiguration>? configure = null)
        {
            // 配置
            services.AddOptions();
            services.Configure<ConversionFrameworkConfiguration>(options =>
            {
                configure?.Invoke(options);
            });

            // 核心服务
            services.AddSingleton<IConversionManager, ConversionManager>();
            services.AddSingleton<IConverterRegistry, ConverterRegistry>();
            services.AddSingleton<IStrategyFactory, StrategyFactory>();
            services.AddSingleton<IPipelineFactory, PipelineFactory>();
            services.AddSingleton<IPluginManager, PluginManager>();
            services.AddSingleton<IConfigurationManager, ConfigurationManager>();
            
            // 缓存
            services.AddSingleton<IConversionCache>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<ConversionFrameworkConfiguration>>();
                var logger = provider.GetRequiredService<ILogger<MemoryConversionCache>>();
                
                var cache = new MemoryConversionCache(
                    config.Value.Performance.CacheExpiration,
                    logger);
                
                // 添加日志装饰器
                return new LoggingCacheDecorator(cache, 
                    provider.GetRequiredService<ILogger<LoggingCacheDecorator>>());
            });

            // 验证服务
            services.AddSingleton<IConversionValidator, ConversionValidator>();
            
            // 性能服务
            services.AddSingleton<IParallelConversionProcessor, ParallelConversionProcessor>();
            
            // 文件服务
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IFormatDetectionService, FormatDetectionService>();
            
            // 映射服务
            services.AddSingleton<IModelMapper, ModelMapper>();
            
            // 插件加载器
            services.AddSingleton<IPluginLoader, PluginLoader>();
            
            // 默认转换器
            services.AddSingleton<IXmlToExcelConverter, XmlToExcelConverter>();
            services.AddSingleton<IExcelToXmlConverter, ExcelToXmlConverter>();
            
            // 默认策略
            services.AddSingleton<IConversionStrategy, XmlToExcelStrategy>();
            services.AddSingleton<IConversionStrategy, ExcelToXmlStrategy>();
            
            // 管道步骤
            services.AddSingleton<IValidationStep, FileValidationStep>();
            services.AddSingleton<IPipelineStep, BackupStep>();
            services.AddSingleton<IPipelineStep, FormatDetectionStep>();
            services.AddSingleton<IConversionStep, DataTransformationStep>();
            services.AddSingleton<IPipelineStep, FileSaveStep>();
            services.AddSingleton<IValidationStep, ValidationStep>();

            return services;
        }

        /// <summary>
        /// 添加内置转换器
        /// </summary>
        public static IServiceCollection AddBuiltInConverters(this IServiceCollection services)
        {
            // XML相关转换器
            services.AddSingleton<IConverter, XmlToExcelFileConverter>();
            services.AddSingleton<IConverter, ExcelToXmlFileConverter>();
            
            // 批量转换器
            services.AddSingleton<IBatchConverter, XmlBatchConverter>();
            services.AddSingleton<IBatchConverter, ExcelBatchConverter>();
            
            return services;
        }

        /// <summary>
        /// 添加插件支持
        /// </summary>
        public static IServiceCollection AddPluginSupport(
            this IServiceCollection services,
            string pluginDirectory = "plugins")
        {
            services.Configure<ConversionFrameworkConfiguration>(options =>
            {
                options.Plugins.PluginDirectory = pluginDirectory;
                options.Plugins.AutoLoadPlugins = true;
            });

            // 插件启动
            services.AddSingleton<IPluginStartup, PluginStartup>();

            return services;
        }
    }
}
```

### 1.2 转换管理器实现

```csharp
namespace BannerlordModEditor.Common.Framework.Managers
{
    /// <summary>
    /// 转换管理器实现
    /// </summary>
    public partial class ConversionManager : IConversionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConverterRegistry _converterRegistry;
        private readonly IStrategyFactory _strategyFactory;
        private readonly IPipelineFactory _pipelineFactory;
        private readonly IPluginManager _pluginManager;
        private readonly ILogger<ConversionManager> _logger;
        private readonly IConversionCache _cache;

        public ConversionManager(
            IServiceProvider serviceProvider,
            IConverterRegistry converterRegistry,
            IStrategyFactory strategyFactory,
            IPipelineFactory pipelineFactory,
            IPluginManager pluginManager,
            ILogger<ConversionManager> logger,
            IConversionCache cache)
        {
            _serviceProvider = serviceProvider;
            _converterRegistry = converterRegistry;
            _strategyFactory = strategyFactory;
            _pipelineFactory = pipelineFactory;
            _pluginManager = pluginManager;
            _logger = logger;
            _cache = cache;
        }

        /// <summary>
        /// 异步初始化
        /// </summary>
        public async Task InitializeAsync()
        {
            _logger.LogInformation("Initializing Conversion Framework...");

            // 加载插件
            await _pluginManager.LoadPluginsAsync();

            // 注册内置转换器
            RegisterBuiltInConverters();

            _logger.LogInformation("Conversion Framework initialized successfully");
        }

        private void RegisterBuiltInConverters()
        {
            // 从DI容器获取所有已注册的转换器
            var converters = _serviceProvider.GetServices<IConverter>();
            foreach (var converter in converters)
            {
                _converterRegistry.RegisterConverter(converter);
                _logger.LogDebug("Registered converter: {ConverterName}", converter.Name);
            }
        }

        public async Task<ConversionResult> ConvertFileAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions? options = null)
        {
            var context = new ConversionContext
            {
                SourcePath = sourcePath,
                TargetPath = targetPath,
                SourceFormat = Path.GetExtension(sourcePath).TrimStart('.').ToLowerInvariant(),
                TargetFormat = Path.GetExtension(targetPath).TrimStart('.').ToLowerInvariant(),
                Options = options ?? new ConversionOptions(),
                StartTime = DateTime.UtcNow
            };

            // 检查缓存
            var cacheKey = GenerateCacheKey(sourcePath, targetPath, options);
            if (options?.UseCache == true && await _cache.ExistsAsync(cacheKey))
            {
                var cachedResult = await _cache.GetAsync<ConversionResult>(cacheKey);
                if (cachedResult != null)
                {
                    _logger.LogInformation("Returning cached result for conversion");
                    return cachedResult;
                }
            }

            // 执行转换
            var result = await ConvertInternalAsync(context);

            // 缓存结果
            if (options?.UseCache == true && result.Success)
            {
                await _cache.SetAsync(cacheKey, result, TimeSpan.FromHours(1));
            }

            return result;
        }

        private async Task<ConversionResult> ConvertInternalAsync(ConversionContext context)
        {
            try
            {
                _logger.LogInformation("Starting conversion: {Source} -> {Target}", 
                    context.SourcePath, context.TargetPath);

                // 1. 验证转换是否支持
                var converters = _converterRegistry.GetConvertersForConversion(
                    context.SourceFormat, 
                    context.TargetFormat);

                if (!converters.Any())
                {
                    return new ConversionResult
                    {
                        Success = false,
                        Message = $"Conversion from {context.SourceFormat} to {context.TargetFormat} is not supported",
                        Errors = { "No converter found for the specified conversion" },
                        Duration = context.Duration
                    };
                }

                // 2. 选择最佳转换器
                var converter = SelectBestConverter(converters, context);

                // 3. 创建转换策略
                var strategy = await _strategyFactory.CreateStrategyAsync(context);
                if (strategy == null)
                {
                    return new ConversionResult
                    {
                        Success = false,
                        Message = "No conversion strategy available",
                        Errors = { "Cannot determine conversion strategy" },
                        Duration = context.Duration
                    };
                }

                // 4. 创建转换管道
                var pipeline = await _pipelineFactory.CreatePipelineAsync(context, strategy);

                // 5. 执行转换
                var result = await pipeline.ExecuteAsync(context);

                _logger.LogInformation("Conversion completed in {Duration}ms. Success: {Success}", 
                    context.Duration.TotalMilliseconds, result.Success);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Conversion failed");
                return new ConversionResult
                {
                    Success = false,
                    Message = "Conversion failed unexpectedly",
                    Errors = { ex.Message },
                    Duration = context.Duration
                };
            }
        }

        private IConverter SelectBestConverter(IEnumerable<IConverter> converters, ConversionContext context)
        {
            // 简单实现：选择第一个可用的转换器
            // 实际应用中可以根据优先级、性能等因素选择
            return converters.First();
        }

        private string GenerateCacheKey(string sourcePath, string targetPath, ConversionOptions? options)
        {
            var fileInfo = new FileInfo(sourcePath);
            var key = $"{sourcePath}_{targetPath}_{fileInfo.LastWriteTime.Ticks}";
            
            if (options != null)
            {
                key += $"_{options.IncludeValidation}_{options.PreserveFormatting}";
            }
            
            return key;
        }

        public async Task<BatchConversionResult> ConvertBatchAsync(
            IEnumerable<string> sourcePaths,
            string outputDirectory,
            ConversionOptions? options = null)
        {
            var processor = _serviceProvider.GetRequiredService<IParallelConversionProcessor>();
            
            var tasks = sourcePaths.Select(sourcePath =>
            {
                var fileName = Path.GetFileNameWithoutExtension(sourcePath);
                var targetExtension = options?.TargetFormat switch
                {
                    "excel" or "xlsx" => ".xlsx",
                    "xml" => ".xml",
                    _ => Path.GetExtension(sourcePath)
                };
                
                var targetPath = Path.Combine(outputDirectory, fileName + targetExtension);
                
                return new ConversionTask
                {
                    SourcePath = sourcePath,
                    TargetPath = targetPath,
                    SourceFormat = Path.GetExtension(sourcePath).TrimStart('.'),
                    TargetFormat = targetExtension.TrimStart('.')
                };
            });

            return await processor.ProcessBatchAsync(tasks, options ?? new ConversionOptions());
        }

        public async Task<FileFormatInfo> DetectFormatAsync(string filePath)
        {
            var detectionService = _serviceProvider.GetRequiredService<IFormatDetectionService>();
            return await detectionService.DetectFormatAsync(filePath);
        }

        public async Task<IEnumerable<string>> GetSupportedFormatsAsync()
        {
            var allConverters = _converterRegistry.AllConverters;
            var formats = new HashSet<string>();
            
            foreach (var converter in allConverters)
            {
                foreach (var format in converter.SupportedSourceFormats)
                {
                    formats.Add(format);
                }
                foreach (var format in converter.SupportedTargetFormats)
                {
                    formats.Add(format);
                }
            }
            
            return formats.OrderBy(f => f);
        }
    }
}
```

### 1.3 策略工厂实现

```csharp
namespace BannerlordModEditor.Common.Framework.Factories
{
    /// <summary>
    /// 策略工厂
    /// </summary>
    public class StrategyFactory : IStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StrategyFactory> _logger;
        private readonly Dictionary<string, Type> _strategyTypes;

        public StrategyFactory(
            IServiceProvider serviceProvider,
            ILogger<StrategyFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            
            // 注册已知的策略类型
            _strategyTypes = new Dictionary<string, Type>
            {
                ["xml->excel"] = typeof(XmlToExcelStrategy),
                ["excel->xml"] = typeof(ExcelToXmlStrategy),
                ["xml->json"] = typeof(XmlToJsonStrategy),
                ["json->xml"] = typeof(JsonToXmlStrategy)
            };
        }

        public async Task<IConversionStrategy?> CreateStrategyAsync(ConversionContext context)
        {
            var strategyKey = $"{context.SourceFormat}->{context.TargetFormat}";
            
            if (_strategyTypes.TryGetValue(strategyKey, out var strategyType))
            {
                try
                {
                    var strategy = (IConversionStrategy)_serviceProvider.GetService(strategyType)!;
                    
                    // 验证策略是否能处理该转换
                    if (await strategy.CanHandleAsync(context))
                    {
                        _logger.LogDebug("Created strategy: {StrategyType}", strategyType.Name);
                        return strategy;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to create strategy {StrategyType}", strategyType.Name);
                }
            }

            // 尝试动态查找策略
            var dynamicStrategy = await FindDynamicStrategyAsync(context);
            if (dynamicStrategy != null)
            {
                return dynamicStrategy;
            }

            _logger.LogWarning("No strategy found for conversion: {StrategyKey}", strategyKey);
            return null;
        }

        private async Task<IConversionStrategy?> FindDynamicStrategyAsync(ConversionContext context)
        {
            // 查找所有注册的策略
            var strategies = _serviceProvider.GetServices<IConversionStrategy>();
            
            foreach (var strategy in strategies)
            {
                if (await strategy.CanHandleAsync(context))
                {
                    return strategy;
                }
            }

            return null;
        }

        public void RegisterStrategy(string key, Type strategyType)
        {
            if (!typeof(IConversionStrategy).IsAssignableFrom(strategyType))
            {
                throw new ArgumentException("Type must implement IConversionStrategy", nameof(strategyType));
            }

            _strategyTypes[key] = strategyType;
            _logger.LogDebug("Registered strategy: {Key} -> {StrategyType}", key, strategyType.Name);
        }
    }
}
```

### 1.4 管道工厂实现

```csharp
namespace BannerlordModEditor.Common.Framework.Factories
{
    /// <summary>
    /// 管道工厂
    /// </summary>
    public class PipelineFactory : IPipelineFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PipelineFactory> _logger;
        private readonly IConfigurationManager _configurationManager;

        public PipelineFactory(
            IServiceProvider serviceProvider,
            ILogger<PipelineFactory> logger,
            IConfigurationManager configurationManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configurationManager = configurationManager;
        }

        public async Task<IConversionPipeline> CreatePipelineAsync(
            ConversionContext context,
            IConversionStrategy strategy)
        {
            var pipelineName = DeterminePipelineName(context);
            var pipeline = new ConversionPipeline(pipelineName);

            // 从配置加载管道步骤
            var pipelineConfig = _configurationManager.GetFrameworkConfiguration()
                .Pipelines.TryGetValue(pipelineName, out var config) ? config : null;

            if (pipelineConfig != null)
            {
                // 使用配置的步骤
                foreach (var stepName in pipelineConfig.Steps)
                {
                    var step = CreateStep(stepName, pipelineConfig.StepSettings);
                    if (step != null)
                    {
                        pipeline.AddStep(step);
                    }
                }
            }
            else
            {
                // 使用默认步骤
                await AddDefaultStepsAsync(pipeline, context, strategy);
            }

            // 设置并行执行
            if (pipelineConfig?.ParallelExecution == true)
            {
                pipeline.EnableParallelExecution();
            }

            _logger.LogDebug("Created pipeline: {PipelineName} with {StepCount} steps", 
                pipelineName, pipeline.Steps.Count());

            return pipeline;
        }

        private string DeterminePipelineName(ConversionContext context)
        {
            return $"{context.SourceFormat}To{context.TargetFormat}Pipeline";
        }

        private async Task AddDefaultStepsAsync(
            ConversionPipeline pipeline,
            ConversionContext context,
            IConversionStrategy strategy)
        {
            // 添加标准步骤
            pipeline.AddStep(_serviceProvider.GetRequiredService<FileValidationStep>());
            
            if (context.Options.CreateBackup)
            {
                pipeline.AddStep(_serviceProvider.GetRequiredService<BackupStep>());
            }
            
            pipeline.AddStep(_serviceProvider.GetRequiredService<FormatDetectionStep>());
            pipeline.AddStep(_serviceProvider.GetRequiredService<DataTransformationStep>());
            pipeline.AddStep(_serviceProvider.GetRequiredService<FileSaveStep>());
            
            if (context.Options.IncludeValidation)
            {
                pipeline.AddStep(_serviceProvider.GetRequiredService<ValidationStep>());
            }

            // 添加策略特定的步骤
            var strategySteps = await GetStrategySpecificStepsAsync(strategy);
            foreach (var step in strategySteps)
            {
                pipeline.AddStep(step);
            }
        }

        private async Task<IEnumerable<IPipelineStep>> GetStrategySpecificStepsAsync(IConversionStrategy strategy)
        {
            // 根据策略类型返回特定的步骤
            return strategy switch
            {
                IXmlConversionStrategy => new[] { _serviceProvider.GetRequiredService<XmlValidationStep>() },
                IExcelConversionStrategy => new[] { _serviceProvider.GetRequiredService<ExcelAnalysisStep>() },
                _ => Enumerable.Empty<IPipelineStep>()
            };
        }

        private IPipelineStep? CreateStep(string stepName, Dictionary<string, object> settings)
        {
            // 根据名称创建步骤实例
            var stepType = Type.GetType($"BannerlordModEditor.Common.Framework.Pipelines.Steps.{stepName}");
            if (stepType != null)
            {
                var step = (IPipelineStep)_serviceProvider.GetService(stepType)!;
                
                // 应用设置
                if (step is IConfigurableStep configurableStep)
                {
                    configurableStep.Configure(settings);
                }
                
                return step;
            }

            _logger.LogWarning("Unknown pipeline step: {StepName}", stepName);
            return null;
        }
    }
}
```

## 2. 具体转换器实现

### 2.1 XML到Excel转换器

```csharp
namespace BannerlordModEditor.Common.Framework.Converters
{
    /// <summary>
    /// XML到Excel文件转换器
    /// </summary>
    public class XmlToExcelFileConverter : IFileConverter
    {
        public string Name => "XmlToExcel";
        public string Description => "Convert XML files to Excel format";
        public Version Version => new Version(1, 0, 0);
        public IEnumerable<string> SupportedSourceFormats => new[] { "xml" };
        public IEnumerable<string> SupportedTargetFormats => new[] { "xlsx", "xls" };

        private readonly IXmlTypeDetectionService _xmlTypeDetection;
        private readonly IModelMapper _modelMapper;
        private readonly ILogger<XmlToExcelFileConverter> _logger;

        public XmlToExcelFileConverter(
            IXmlTypeDetectionService xmlTypeDetection,
            IModelMapper modelMapper,
            ILogger<XmlToExcelFileConverter> logger)
        {
            _xmlTypeDetection = xmlTypeDetection;
            _modelMapper = modelMapper;
            _logger = logger;
        }

        public async Task<bool> CanConvertAsync(string sourceFormat, string targetFormat)
        {
            return SupportedSourceFormats.Contains(sourceFormat.ToLowerInvariant()) &&
                   SupportedTargetFormats.Contains(targetFormat.ToLowerInvariant());
        }

        public async Task<FileConversionResult> ConvertFileAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            var startTime = DateTime.UtcNow;
            var result = new FileConversionResult
            {
                SourcePath = sourcePath,
                TargetPath = targetPath
            };

            try
            {
                _logger.LogInformation("Starting XML to Excel conversion: {Source} -> {Target}", 
                    sourcePath, targetPath);

                // 1. 检测XML类型
                var xmlTypeInfo = await _xmlTypeDetection.DetectXmlTypeAsync(sourcePath);
                
                // 2. 根据类型选择转换方法
                if (xmlTypeInfo.IsAdapted && xmlTypeInfo.ModelType != null)
                {
                    // 类型化XML转换
                    result = await ConvertTypedXmlToExcelAsync(sourcePath, targetPath, xmlTypeInfo, options);
                }
                else
                {
                    // 通用XML转换
                    result = await ConvertGenericXmlToExcelAsync(sourcePath, targetPath, options);
                }

                result.Duration = DateTime.UtcNow - startTime;
                
                _logger.LogInformation("XML to Excel conversion completed in {Duration}ms", 
                    result.Duration.TotalMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "XML to Excel conversion failed");
                result.Duration = DateTime.UtcNow - startTime;
                result.Success = false;
                result.Message = "Conversion failed";
                result.Errors = { ex.Message };
                return result;
            }
        }

        private async Task<FileConversionResult> ConvertTypedXmlToExcelAsync(
            string sourcePath,
            string targetPath,
            XmlTypeInfo xmlTypeInfo,
            ConversionOptions options)
        {
            var result = new FileConversionResult();

            // 获取模型类型
            var modelType = Type.GetType(xmlTypeInfo.ModelType);
            if (modelType == null)
            {
                result.Success = false;
                result.Message = $"Cannot load model type: {xmlTypeInfo.ModelType}";
                result.Errors = { result.Message };
                return result;
            }

            // 创建通用加载器
            var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(modelType);
            var loader = Activator.CreateInstance(loaderType) as dynamic;
            
            if (loader == null)
            {
                result.Success = false;
                result.Message = "Cannot create XML loader";
                result.Errors = { result.Message };
                return result;
            }

            // 加载XML数据
            var xmlData = await loader.LoadAsync(sourcePath);
            
            // 转换为Excel
            var excelData = await _modelMapper.MapTypedXmlToExcelAsync(xmlData, modelType, options);
            
            // 保存Excel文件
            excelData.SaveAs(targetPath);
            
            result.Success = true;
            result.Message = "Typed XML to Excel conversion completed successfully";
            result.RecordsProcessed = await CountRecordsAsync(xmlData);
            
            return result;
        }

        private async Task<FileConversionResult> ConvertGenericXmlToExcelAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            var result = new FileConversionResult();

            // 加载XML文档
            var xmlDoc = XDocument.Load(sourcePath);
            var rootElement = xmlDoc.Root;
            
            if (rootElement == null)
            {
                result.Success = false;
                result.Message = "XML document has no root element";
                result.Errors = { result.Message };
                return result;
            }

            // 创建Excel工作簿
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(rootElement.Name.LocalName);

            // 收集所有可能的列名
            var columnNames = new HashSet<string>();
            var rowData = new List<Dictionary<string, string>>();

            // 提取数据
            var recordElements = rootElement.Elements();
            foreach (var recordElement in recordElements)
            {
                var rowDict = new Dictionary<string, string>();
                ExtractElementData(recordElement, rowDict, "", options);
                
                foreach (var key in rowDict.Keys)
                {
                    columnNames.Add(key);
                }
                
                rowData.Add(rowDict);
            }

            // 添加表头
            var sortedColumns = columnNames.OrderBy(c => c).ToList();
            for (int i = 0; i < sortedColumns.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = sortedColumns[i];
                
                // 设置表头样式
                worksheet.Cell(1, i + 1).Style
                    .Font.SetBold()
                    .Fill.SetBackgroundColor(XLColor.LightGray);
            }

            // 添加数据行
            for (int rowIndex = 0; rowIndex < rowData.Count; rowIndex++)
            {
                var rowDict = rowData[rowIndex];
                for (int colIndex = 0; colIndex < sortedColumns.Count; colIndex++)
                {
                    var columnName = sortedColumns[colIndex];
                    if (rowDict.TryGetValue(columnName, out var value))
                    {
                        worksheet.Cell(rowIndex + 2, colIndex + 1).Value = value;
                    }
                }
            }

            // 自动调整列宽
            worksheet.Columns().AdjustToContents();
            
            // 冻结首行
            worksheet.SheetView.Freeze(1, 0);

            // 保存文件
            workbook.SaveAs(targetPath);

            result.Success = true;
            result.Message = "Generic XML to Excel conversion completed successfully";
            result.RecordsProcessed = rowData.Count;

            return result;
        }

        private void ExtractElementData(
            XElement element,
            Dictionary<string, string> data,
            string parentPath,
            ConversionOptions options)
        {
            if (element.HasElements)
            {
                // 处理嵌套元素
                foreach (var childElement in element.Elements())
                {
                    var childPath = string.IsNullOrEmpty(parentPath) 
                        ? childElement.Name.LocalName 
                        : $"{parentPath}{options.NestedElementSeparator}{childElement.Name.LocalName}";
                    
                    if (childElement.HasElements)
                    {
                        ExtractElementData(childElement, data, childPath, options);
                    }
                    else
                    {
                        var key = options.FlattenNestedElements ? childPath : childElement.Name.LocalName;
                        data[key] = childElement.Value;
                    }
                }
            }
            else
            {
                // 叶子节点
                var key = string.IsNullOrEmpty(parentPath) ? element.Name.LocalName : parentPath;
                data[key] = element.Value;
            }
        }

        private async Task<int> CountRecordsAsync(object xmlData)
        {
            // 使用反射获取记录数
            var dataType = xmlData.GetType();
            
            // 查找可能包含记录列表的属性
            var listProperties = dataType.GetProperties()
                .Where(p => typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType))
                .ToList();

            foreach (var prop in listProperties)
            {
                var value = prop.GetValue(xmlData);
                if (value is IEnumerable<object> list)
                {
                    return list.Count();
                }
            }

            return 1; // 如果没有找到列表属性，假设为单条记录
        }
    }
}
```

### 2.2 Excel到XML转换器

```csharp
namespace BannerlordModEditor.Common.Framework.Converters
{
    /// <summary>
    /// Excel到XML文件转换器
    /// </summary>
    public class ExcelToXmlFileConverter : IFileConverter
    {
        public string Name => "ExcelToXml";
        public string Description => "Convert Excel files to XML format";
        public Version Version => new Version(1, 0, 0);
        public IEnumerable<string> SupportedSourceFormats => new[] { "xlsx", "xls" };
        public IEnumerable<string> SupportedTargetFormats => new[] { "xml" };

        private readonly IModelMapper _modelMapper;
        private readonly ILogger<ExcelToXmlFileConverter> _logger;

        public ExcelToXmlFileConverter(
            IModelMapper modelMapper,
            ILogger<ExcelToXmlFileConverter> logger)
        {
            _modelMapper = modelMapper;
            _logger = logger;
        }

        public async Task<bool> CanConvertAsync(string sourceFormat, string targetFormat)
        {
            return SupportedSourceFormats.Contains(sourceFormat.ToLowerInvariant()) &&
                   SupportedTargetFormats.Contains(targetFormat.ToLowerInvariant());
        }

        public async Task<FileConversionResult> ConvertFileAsync(
            string sourcePath,
            string targetPath,
            ConversionOptions options)
        {
            var startTime = DateTime.UtcNow;
            var result = new FileConversionResult
            {
                SourcePath = sourcePath,
                TargetPath = targetPath
            };

            try
            {
                _logger.LogInformation("Starting Excel to XML conversion: {Source} -> {Target}", 
                    sourcePath, targetPath);

                // 分析Excel结构
                using var workbook = new XLWorkbook(sourcePath);
                var worksheet = workbook.Worksheets.First();
                
                var analysisResult = new ExcelAnalysisResult
                {
                    ColumnNames = worksheet.FirstRowUsed()
                        .Cells()
                        .Select(c => c.Value.ToString())
                        .ToList(),
                    RowCount = worksheet.RowsUsed().Count() - 1,
                    WorksheetName = worksheet.Name
                };

                // 根据选项选择转换方法
                if (options.XmlType != null)
                {
                    // 类型化XML转换
                    result = await ConvertExcelToTypedXmlAsync(worksheet, targetPath, analysisResult, options);
                }
                else
                {
                    // 通用XML转换
                    result = await ConvertExcelToGenericXmlAsync(worksheet, targetPath, analysisResult, options);
                }

                result.Duration = DateTime.UtcNow - startTime;
                
                _logger.LogInformation("Excel to XML conversion completed in {Duration}ms", 
                    result.Duration.TotalMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel to XML conversion failed");
                result.Duration = DateTime.UtcNow - startTime;
                result.Success = false;
                result.Message = "Conversion failed";
                result.Errors = { ex.Message };
                return result;
            }
        }

        private async Task<FileConversionResult> ConvertExcelToGenericXmlAsync(
            IXLWorksheet worksheet,
            string targetPath,
            ExcelAnalysisResult analysisResult,
            ConversionOptions options)
        {
            var result = new FileConversionResult();

            // 创建XML文档
            var xmlDoc = new XDocument();
            var rootName = options.RootElementName ?? worksheet.Name;
            var rootElement = new XElement(rootName);
            xmlDoc.Add(rootElement);

            var rowElementName = options.RowElementName ?? "Record";
            var recordsProcessed = 0;

            // 处理数据行
            var dataRows = worksheet.RowsUsed().Skip(1); // 跳过标题行
            
            foreach (var row in dataRows)
            {
                var rowElement = new XElement(rowElementName);
                
                for (int i = 0; i < analysisResult.ColumnNames.Count; i++)
                {
                    var columnName = analysisResult.ColumnNames[i];
                    var cellValue = row.Cell(i + 1).Value.ToString() ?? "";
                    
                    // 处理嵌套元素
                    if (options.FlattenNestedElements && columnName.Contains(options.NestedElementSeparator))
                    {
                        CreateNestedElements(rowElement, columnName, cellValue, options.NestedElementSeparator);
                    }
                    else
                    {
                        var elementName = SanitizeXmlName(columnName);
                        var element = new XElement(elementName, cellValue);
                        rowElement.Add(element);
                    }
                }
                
                rootElement.Add(rowElement);
                recordsProcessed++;
            }

            // 保存XML文件
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = Encoding.UTF8,
                NewLineChars = "\n"
            };

            using var writer = XmlWriter.Create(targetPath, settings);
            xmlDoc.Save(writer);

            result.Success = true;
            result.Message = "Excel to generic XML conversion completed successfully";
            result.RecordsProcessed = recordsProcessed;

            return result;
        }

        private async Task<FileConversionResult> ConvertExcelToTypedXmlAsync(
            IXLWorksheet worksheet,
            string targetPath,
            ExcelAnalysisResult analysisResult,
            ConversionOptions options)
        {
            var result = new FileConversionResult();

            // 获取XML类型信息
            var xmlTypeInfo = await GetXmlTypeInfoAsync(options.XmlType!);
            if (xmlTypeInfo == null || !xmlTypeInfo.IsAdapted)
            {
                result.Success = false;
                result.Message = $"Unsupported XML type: {options.XmlType}";
                result.Errors = { result.Message };
                return result;
            }

            // 转换为类型化XML
            var xmlData = await _modelMapper.MapExcelToTypedXmlAsync(
                worksheet, 
                xmlTypeInfo, 
                analysisResult, 
                options);

            // 保存XML文件
            var modelType = Type.GetType(xmlTypeInfo.ModelType);
            if (modelType == null)
            {
                result.Success = false;
                result.Message = $"Cannot load model type: {xmlTypeInfo.ModelType}";
                result.Errors = { result.Message };
                return result;
            }

            var loaderType = typeof(GenericXmlLoader<>).MakeGenericType(modelType);
            var loader = Activator.CreateInstance(loaderType) as dynamic;
            
            if (loader == null)
            {
                result.Success = false;
                result.Message = "Cannot create XML loader";
                result.Errors = { result.Message };
                return result;
            }

            await loader.SaveAsync(xmlData, targetPath);

            result.Success = true;
            result.Message = "Excel to typed XML conversion completed successfully";
            result.RecordsProcessed = await CountRecordsAsync(xmlData);

            return result;
        }

        private void CreateNestedElements(
            XElement parent,
            string nestedPath,
            string value,
            string separator)
        {
            var parts = nestedPath.Split(separator);
            XElement currentElement = parent;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var elementName = SanitizeXmlName(part);

                if (i == parts.Length - 1)
                {
                    // 最后一个部分，设置值
                    currentElement.Add(new XElement(elementName, value));
                }
                else
                {
                    // 中间部分，查找或创建元素
                    var existingElement = currentElement.Element(elementName);
                    if (existingElement == null)
                    {
                        existingElement = new XElement(elementName);
                        currentElement.Add(existingElement);
                    }
                    currentElement = existingElement;
                }
            }
        }

        private string SanitizeXmlName(string name)
        {
            // 移除无效字符
            var invalidChars = new[] { ' ', '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            var sanitized = name;
            
            foreach (var invalidChar in invalidChars)
            {
                sanitized = sanitized.Replace(invalidChar, '_');
            }

            // 确保以字母或下划线开头
            if (sanitized.Length > 0 && !char.IsLetter(sanitized[0]) && sanitized[0] != '_')
            {
                sanitized = "_" + sanitized;
            }

            return string.IsNullOrEmpty(sanitized) ? "element" : sanitized;
        }

        private async Task<XmlTypeInfo?> GetXmlTypeInfoAsync(string xmlType)
        {
            // 这里应该从服务获取XML类型信息
            // 简化实现
            return new XmlTypeInfo
            {
                XmlType = xmlType,
                IsAdapted = true,
                ModelType = $"BannerlordModEditor.Common.Models.DO.{xmlType}DO, BannerlordModEditor.Common"
            };
        }

        private async Task<int> CountRecordsAsync(object xmlData)
        {
            // 使用反射获取记录数
            var dataType = xmlData.GetType();
            
            var listProperties = dataType.GetProperties()
                .Where(p => typeof(IEnumerable<object>).IsAssignableFrom(p.PropertyType))
                .ToList();

            foreach (var prop in listProperties)
            {
                var value = prop.GetValue(xmlData);
                if (value is IEnumerable<object> list)
                {
                    return list.Count();
                }
            }

            return 1;
        }
    }
}
```

## 3. 使用示例

### 3.1 控制台应用程序示例

```csharp
using BannerlordModEditor.Common.Framework;
using BannerlordModEditor.Common.Framework.Extensions;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. 配置服务
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddConsole());
        services.AddConversionFramework(options =>
        {
            options.Performance.MaxDegreeOfParallelism = 4;
            options.Performance.EnableCaching = true;
            options.Plugins.AutoLoadPlugins = true;
        });
        services.AddBuiltInConverters();

        // 2. 构建服务提供者
        var serviceProvider = services.BuildServiceProvider();

        // 3. 初始化框架
        var conversionManager = serviceProvider.GetRequiredService<IConversionManager>();
        await conversionManager.InitializeAsync();

        // 4. 显示支持的格式
        var supportedFormats = await conversionManager.GetSupportedFormatsAsync();
        Console.WriteLine("支持的格式:");
        foreach (var format in supportedFormats)
        {
            Console.WriteLine($"- {format}");
        }

        // 5. 执行单个文件转换
        if (args.Length >= 2)
        {
            var sourceFile = args[0];
            var targetFile = args[1];
            
            Console.WriteLine($"\n转换文件: {sourceFile} -> {targetFile}");
            
            var result = await conversionManager.ConvertFileAsync(
                sourceFile,
                targetFile,
                new ConversionOptions
                {
                    CreateBackup = true,
                    IncludeValidation = true,
                    PreserveFormatting = true,
                    UseCache = true
                });

            if (result.Success)
            {
                Console.WriteLine("转换成功!");
                Console.WriteLine($"处理记录数: {result.RecordsProcessed}");
                Console.WriteLine($"耗时: {result.Duration.TotalMilliseconds}ms");
            }
            else
            {
                Console.WriteLine("转换失败:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error}");
                }
            }
        }

        // 6. 批量转换示例
        if (args.Length >= 3 && args[2] == "batch")
        {
            var sourceDir = args[0];
            var outputDir = args[1];
            
            Console.WriteLine($"\n批量转换: {sourceDir} -> {outputDir}");
            
            var xmlFiles = Directory.GetFiles(sourceDir, "*.xml");
            var batchResult = await conversionManager.ConvertBatchAsync(
                xmlFiles,
                outputDir,
                new ConversionOptions
                {
                    TargetFormat = "xlsx",
                    UseCache = true
                });

            Console.WriteLine($"批量转换完成: {batchResult.SuccessfulTasks}/{batchResult.TotalTasks} 成功");
            Console.WriteLine($"成功率: {batchResult.SuccessRate:F1}%");
            Console.WriteLine($"总耗时: {batchResult.Duration.TotalSeconds:F1}秒");
        }

        // 7. 格式检测示例
        if (args.Length >= 2 && args[2] == "detect")
        {
            var fileToDetect = args[0];
            
            Console.WriteLine($"\n检测文件格式: {fileToDetect}");
            
            var formatInfo = await conversionManager.DetectFormatAsync(fileToDetect);
            
            Console.WriteLine($"格式: {formatInfo.FormatType}");
            Console.WriteLine($"是否支持: {formatInfo.IsSupported}");
            Console.WriteLine($"描述: {formatInfo.FormatDescription}");
            
            if (formatInfo.ColumnNames.Any())
            {
                Console.WriteLine("列名:");
                foreach (var column in formatInfo.ColumnNames)
                {
                    Console.WriteLine($"- {column}");
                }
            }
        }
    }
}
```

### 3.2 ASP.NET Core API 示例

```csharp
[ApiController]
[Route("api/[controller]")]
public class ConversionController : ControllerBase
{
    private readonly IConversionManager _conversionManager;
    private readonly ILogger<ConversionController> _logger;

    public ConversionController(
        IConversionManager conversionManager,
        ILogger<ConversionController> logger)
    {
        _conversionManager = conversionManager;
        _logger = logger;
    }

    [HttpPost("convert")]
    public async Task<IActionResult> ConvertFile([FromBody] ConversionRequest request)
    {
        try
        {
            var result = await _conversionManager.ConvertFileAsync(
                request.SourcePath,
                request.TargetPath,
                request.Options);

            if (result.Success)
            {
                return Ok(new ConversionResponse
                {
                    Success = true,
                    Message = result.Message,
                    RecordsProcessed = result.RecordsProcessed,
                    DurationMs = result.Duration.TotalMilliseconds,
                    Warnings = result.Warnings
                });
            }
            else
            {
                return BadRequest(new ConversionResponse
                {
                    Success = false,
                    Message = result.Message,
                    Errors = result.Errors
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Conversion API error");
            return StatusCode(500, new ConversionResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = { ex.Message }
            });
        }
    }

    [HttpPost("convert/batch")]
    public async Task<IActionResult> ConvertBatch([FromBody] BatchConversionRequest request)
    {
        try
        {
            var result = await _conversionManager.ConvertBatchAsync(
                request.SourcePaths,
                request.OutputDirectory,
                request.Options);

            return Ok(new BatchConversionResponse
            {
                Success = true,
                TotalTasks = result.TotalTasks,
                SuccessfulTasks = result.SuccessfulTasks,
                FailedTasks = result.FailedTasks,
                SuccessRate = result.SuccessRate,
                DurationMs = result.Duration.TotalMilliseconds,
                Results = result.Results.Select(r => new ConversionResponse
                {
                    Success = r.Success,
                    Message = r.Message,
                    RecordsProcessed = r.RecordsProcessed,
                    DurationMs = r.Duration.TotalMilliseconds,
                    Errors = r.Errors,
                    Warnings = r.Warnings
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Batch conversion API error");
            return StatusCode(500, new ConversionResponse
            {
                Success = false,
                Message = "Internal server error",
                Errors = { ex.Message }
            });
        }
    }

    [HttpGet("formats")]
    public async Task<IActionResult> GetSupportedFormats()
    {
        var formats = await _conversionManager.GetSupportedFormatsAsync();
        return Ok(formats);
    }

    [HttpPost("detect")]
    public async Task<IActionResult> DetectFormat([FromBody] FormatDetectionRequest request)
    {
        var formatInfo = await _conversionManager.DetectFormatAsync(request.FilePath);
        return Ok(formatInfo);
    }
}

public class ConversionRequest
{
    public string SourcePath { get; set; } = string.Empty;
    public string TargetPath { get; set; } = string.Empty;
    public ConversionOptions Options { get; set; } = new();
}

public class BatchConversionRequest
{
    public List<string> SourcePaths { get; set; } = new();
    public string OutputDirectory { get; set; } = string.Empty;
    public ConversionOptions Options { get; set; } = new();
}

public class FormatDetectionRequest
{
    public string FilePath { get; set; } = string.Empty;
}

public class ConversionResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int RecordsProcessed { get; set; }
    public double DurationMs { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
}

public class BatchConversionResponse : ConversionResponse
{
    public int TotalTasks { get; set; }
    public int SuccessfulTasks { get; set; }
    public int FailedTasks { get; set; }
    public double SuccessRate { get; set; }
    public List<ConversionResponse> Results { get; set; } = new();
}
```

## 4. 性能优化建议

### 4.1 使用缓存

```csharp
// 启用缓存可以显著提高重复转换的性能
var options = new ConversionOptions
{
    UseCache = true,
    CacheExpiration = TimeSpan.FromHours(1)
};
```

### 4.2 批量处理

```csharp
// 批量处理文件比逐个处理更高效
var batchResult = await conversionManager.ConvertBatchAsync(
    sourceFiles,
    outputDirectory,
    new ConversionOptions
    {
        MaxDegreeOfParallelism = Environment.ProcessorCount
    });
```

### 4.3 流式处理大文件

```csharp
// 对于大文件，考虑使用流式处理
public class StreamingXmlConverter : IConverter
{
    public async Task<ConversionResult> ConvertLargeFileAsync(
        string sourcePath,
        string targetPath)
    {
        using var sourceStream = File.OpenRead(sourcePath);
        using var targetStream = File.Create(targetPath);
        
        // 使用XmlReader和XmlWriter进行流式处理
        using var reader = XmlReader.Create(sourceStream);
        using var writer = XmlWriter.Create(targetStream, new XmlWriterSettings
        {
            Indent = true,
            Encoding = Encoding.UTF8
        });
        
        // 流式转换逻辑...
    }
}
```

## 5. 扩展框架

### 5.1 添加新的转换器

```csharp
// 1. 实现转换器接口
public class JsonToCsvConverter : IFileConverter
{
    public string Name => "JsonToCsv";
    public string Description => "Convert JSON files to CSV format";
    public IEnumerable<string> SupportedSourceFormats => new[] { "json" };
    public IEnumerable<string> SupportedTargetFormats => new[] { "csv" };
    
    // 实现转换逻辑...
}

// 2. 注册转换器
services.AddSingleton<IConverter, JsonToCsvConverter>();
```

### 5.2 创建自定义插件

```csharp
// 1. 创建插件类库项目
// 2. 实现插件接口
public class CustomConversionPlugin : ConversionPluginBase
{
    public override string PluginName => "CustomConverters";
    public override string PluginVersion => "1.0.0";
    
    public override Task InitializeAsync(IServiceProvider serviceProvider)
    {
        AddConverter(new JsonToYamlConverter());
        AddConverter(new YamlToJsonConverter());
        return Task.CompletedTask;
    }
}

// 3. 将DLL放入plugins目录
```

## 6. 总结

这个通用XML转换框架提供了：

1. **灵活的架构**: 支持多种转换格式和策略
2. **高性能**: 缓存、并行处理等优化机制
3. **可扩展性**: 插件系统支持动态添加新功能
4. **易用性**: 简洁的API和配置选项
5. **健壮性**: 完善的错误处理和验证机制

通过这个框架，可以轻松实现各种XML转换需求，并根据业务需要进行扩展。