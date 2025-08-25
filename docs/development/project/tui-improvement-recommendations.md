# TUI项目XML编辑功能改进建议

## 概述

基于对BannerlordModEditor.TUI项目的分析，本文档提出了具体的改进建议，以充分利用Common层现有的XML模型系统，将TUI从通用XML转换工具升级为专业的Bannerlord模组编辑器。

## 改进优先级

### 🚨 优先级1：核心集成（必须实现）

#### 1.1 集成Common层XML模型系统

**目标**: 让TUI能够识别和处理特定的Bannerlord XML类型

**实现步骤**:

1. **添加项目引用**
   ```xml
   <!-- 在BannerlordModEditor.TUI.csproj中添加 -->
   <ProjectReference Include="..\BannerlordModEditor.Common\BannerlordModEditor.Common.csproj" />
   ```

2. **集成FileDiscoveryService**
   ```csharp
   // 在FormatConversionService中添加
   private readonly IFileDiscoveryService _fileDiscoveryService;
   
   public FormatConversionService(IFileDiscoveryService fileDiscoveryService)
   {
       _fileDiscoveryService = fileDiscoveryService;
   }
   ```

3. **实现XML类型检测**
   ```csharp
   public async Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlFilePath)
   {
       var analysisResult = await _fileDiscoveryService.AnalyzeXmlFileAsync(xmlFilePath);
       return new XmlTypeInfo
       {
           XmlType = analysisResult.XmlType,
           ModelType = analysisResult.ModelType,
           Namespace = analysisResult.Namespace,
           Description = analysisResult.Description
       };
   }
   ```

4. **添加类型化转换方法**
   ```csharp
   public async Task<ConversionResult> ConvertTypedXmlAsync<T>(string xmlFilePath, string excelFilePath)
       where T : class
   {
       var loader = new GenericXmlLoader<T>();
       var data = await loader.LoadAsync(xmlFilePath);
       
       // 使用类型化的Excel转换逻辑
       return await ConvertToExcelAsync(data, excelFilePath);
   }
   ```

#### 1.2 增强用户界面

**目标**: 为用户提供类型识别和选择功能

**实现步骤**:

1. **添加XML类型检测UI**
   ```csharp
   // 在MainViewModel中添加
   public XmlFileInfo CurrentXmlInfo { get; private set; }
   
   public async Task DetectXmlTypeAsync()
   {
       if (!string.IsNullOrEmpty(SourceFilePath))
       {
           CurrentXmlInfo = await _conversionService.DetectXmlTypeAsync(SourceFilePath);
           OnPropertyChanged(nameof(CurrentXmlInfo));
       }
   }
   ```

2. **更新TUI界面**
   ```csharp
   // 在MainWindow中添加类型信息显示
   var typeInfoLabel = new Label("")
   {
       X = 1,
       Y = 8,
       Width = Dim.Fill(),
       Height = 1
   };
   
   // 绑定到ViewModel的CurrentXmlInfo属性
   typeInfoLabel.Text = () => $"检测到的类型: {ViewModel.CurrentXmlInfo?.DisplayName ?? "未知"}";
   ```

### ⚡ 优先级2：功能增强（重要）

#### 2.1 实现类型化编辑界面

**目标**: 为不同XML类型提供专门的编辑界面

**实现策略**:

1. **创建类型化视图模型**
   ```csharp
   public abstract class TypedXmlViewModel<T> : ViewModelBase
       where T : class
   {
       protected readonly GenericXmlLoader<T> Loader;
       public T Data { get; private set; }
       
       public TypedXmlViewModel(GenericXmlLoader<T> loader)
       {
           Loader = loader;
       }
       
       public async Task LoadAsync(string filePath)
       {
           Data = await Loader.LoadAsync(filePath);
           OnPropertyChanged(nameof(Data));
       }
   }
   ```

2. **实现特定类型的编辑器**
   ```csharp
   // 例如：ActionTypes编辑器
   public class ActionTypesViewModel : TypedXmlViewModel<ActionTypesDO>
   {
       public ActionTypesViewModel(GenericXmlLoader<ActionTypesDO> loader) 
           : base(loader) { }
       
       // ActionTypes特定的属性和方法
   }
   ```

3. **动态界面生成**
   ```csharp
   public View CreateTypedEditor(Type xmlType)
   {
       // 根据XML类型动态创建编辑界面
       var editorType = typeof(TypedEditor<>).MakeGenericType(xmlType);
       return (View)Activator.CreateInstance(editorType);
   }
   ```

#### 2.2 添加验证和错误处理

**目标**: 提供类型特定的验证和错误处理

**实现步骤**:

1. **集成Common层验证规则**
   ```csharp
   public class XmlValidationService
   {
       private readonly IFileDiscoveryService _fileDiscoveryService;
       
       public async Task<ValidationResult> ValidateXmlAsync<T>(string xmlFilePath)
           where T : class
       {
           var loader = new GenericXmlLoader<T>();
           var data = await loader.LoadAsync(xmlFilePath);
           
           // 使用Common层的验证逻辑
           return await ValidateDataAsync(data);
       }
   }
   ```

2. **实时验证反馈**
   ```csharp
   // 在TUI界面中添加验证状态显示
   var validationStatus = new Label("")
   {
       X = 1,
       Y = 12,
       Width = Dim.Fill(),
       Height = 1
   };
   
   validationStatus.Text = () => 
   {
       var status = ViewModel.ValidationStatus;
       return status.IsValid ? "✅ 验证通过" : $"❌ {status.ErrorMessage}";
   };
   ```

#### 2.3 增强转换功能

**目标**: 提供更强大的转换选项

**实现步骤**:

1. **批量转换**
   ```csharp
   public async Task<BatchConversionResult> ConvertBatchAsync<BatchConversionRequest>(request)
   {
       var results = new List<ConversionResult>();
       
       foreach (var file in request.Files)
       {
           var result = await ConvertTypedXmlAsync(file.SourcePath, file.TargetPath);
           results.Add(result);
       }
       
       return new BatchConversionResult
       {
           Results = results,
           TotalFiles = request.Files.Count,
           SuccessCount = results.Count(r => r.Success)
       };
   }
   ```

2. **模板支持**
   ```csharp
   public class XmlTemplateService
   {
       public async Task<string> CreateTemplateAsync<T>(string outputPath)
           where T : class, new()
       {
           var template = new T();
           var loader = new GenericXmlLoader<T>();
           await loader.SaveAsync(template, outputPath);
           return outputPath;
       }
   }
   ```

### 🔧 优先级3：生态完善（可选）

#### 3.1 高级功能

**目标**: 提供专业级的编辑功能

**实现策略**:

1. **XML依赖关系管理**
   ```csharp
   public class XmlDependencyService
   {
       public async Task<DependencyGraph> AnalyzeDependenciesAsync(string xmlFilePath)
       {
           // 分析XML文件之间的依赖关系
           // 例如：Items引用ItemCategories，Skills引用CharacterAttributes等
       }
   }
   ```

2. **版本控制集成**
   ```csharp
   public class XmlVersionControlService
   {
       public async Task<VersionInfo> GetVersionInfoAsync(string xmlFilePath)
       {
           // 获取XML文件的版本信息
           // 支持与Git集成
       }
   }
   ```

3. **插件系统**
   ```csharp
   public interface IXmlEditorPlugin
   {
       string SupportedXmlType { get; }
       View CreateEditor(object data);
       Task<bool> ValidateAsync(object data);
   }
   ```

#### 3.2 性能优化

**目标**: 提升大文件处理性能

**实现策略**:

1. **缓存机制**
   ```csharp
   public class XmlCacheService
   {
       private readonly ConcurrentDictionary<string, CachedXml> _cache;
       
       public async Task<T> GetOrLoadAsync<T>(string filePath)
           where T : class
       {
           return await _cache.GetOrAdd(filePath, async key =>
           {
               var loader = new GenericXmlLoader<T>();
               var data = await loader.LoadAsync(key);
               return new CachedXml(data, DateTime.UtcNow);
           }).Data;
       }
   }
   ```

2. **流式处理**
   ```csharp
   public async Task ProcessLargeXmlAsync<T>(string filePath, Action<T> processor)
   {
       // 使用流式XML处理，避免内存问题
       // 适用于特别大的XML文件
   }
   ```

## 实施计划

### 阶段1：核心集成（2-3周）
- [ ] 集成Common层XML模型系统
- [ ] 实现XML类型检测
- [ ] 添加类型化转换功能
- [ ] 更新用户界面

### 阶段2：功能增强（3-4周）
- [ ] 实现类型化编辑界面
- [ ] 添加验证和错误处理
- [ ] 增强转换功能
- [ ] 完善测试覆盖

### 阶段3：生态完善（4-6周）
- [ ] 实现高级功能
- [ ] 性能优化
- [ ] 插件系统
- [ ] 文档完善

## 技术架构改进

### 当前架构问题
```
TUI项目
├── 通用XML处理（独立系统）
└── 基础转换功能
    ↓ 未连接
Common层
├── 102种XML模型
├── 类型化处理系统
└── 完整的验证框架
```

### 目标架构
```
TUI项目
├── 通用XML处理（保持）
├── 类型化XML处理（新增）
├── XML类型检测（新增）
├── 验证和错误处理（新增）
└── 用户界面增强（改进）
    ↓ 集成
Common层
├── 102种XML模型
├── 类型化处理系统
└── 完整的验证框架
```

## 代码示例

### 集成后的FormatConversionService

```csharp
public class FormatConversionService : IFormatConversionService
{
    private readonly IFileDiscoveryService _fileDiscoveryService;
    private readonly IXmlValidationService _validationService;
    private readonly IXmlTemplateService _templateService;

    public FormatConversionService(
        IFileDiscoveryService fileDiscoveryService,
        IXmlValidationService validationService,
        IXmlTemplateService templateService)
    {
        _fileDiscoveryService = fileDiscoveryService;
        _validationService = validationService;
        _templateService = templateService;
    }

    // 新增：XML类型检测
    public async Task<XmlTypeInfo> DetectXmlTypeAsync(string xmlFilePath)
    {
        var analysisResult = await _fileDiscoveryService.AnalyzeXmlFileAsync(xmlFilePath);
        return new XmlTypeInfo
        {
            XmlType = analysisResult.XmlType,
            ModelType = analysisResult.ModelType,
            Namespace = analysisResult.Namespace,
            Description = analysisResult.Description,
            IsSupported = analysisResult.IsAdapted
        };
    }

    // 新增：类型化转换
    public async Task<ConversionResult> ConvertTypedXmlAsync<T>(string xmlFilePath, string excelFilePath)
        where T : class
    {
        try
        {
            // 检测XML类型
            var typeInfo = await DetectXmlTypeAsync(xmlFilePath);
            if (!typeInfo.IsSupported)
            {
                return ConversionResult.Failure($"不支持的XML类型: {typeInfo.XmlType}");
            }

            // 验证XML
            var validationResult = await _validationService.ValidateXmlAsync<T>(xmlFilePath);
            if (!validationResult.IsValid)
            {
                return ConversionResult.Failure(validationResult.ErrorMessage);
            }

            // 类型化转换
            var loader = new GenericXmlLoader<T>();
            var data = await loader.LoadAsync(xmlFilePath);

            // 转换为Excel
            var excelData = ConvertToExcelData(data);
            await SaveExcelAsync(excelFilePath, excelData);

            return ConversionResult.Success($"成功转换 {typeInfo.XmlType} XML文件", 
                validationResult.RecordCount);
        }
        catch (Exception ex)
        {
            return ConversionResult.Failure($"转换失败: {ex.Message}", ex);
        }
    }

    // 保持原有的通用转换方法
    public async Task<ConversionResult> ExcelToXmlAsync(string excelFilePath, string xmlFilePath)
    {
        // 原有实现保持不变
    }

    public async Task<ConversionResult> XmlToExcelAsync(string xmlFilePath, string excelFilePath)
    {
        // 可以先尝试类型化转换，失败后回退到通用转换
        try
        {
            var typeInfo = await DetectXmlTypeAsync(xmlFilePath);
            if (typeInfo.IsSupported)
            {
                var method = typeof(FormatConversionService)
                    .GetMethod(nameof(ConvertTypedXmlAsync))
                    .MakeGenericMethod(Type.GetType(typeInfo.ModelType));
                
                return (ConversionResult)await method.Invoke(this, new object[] { xmlFilePath, excelFilePath });
            }
        }
        catch
        {
            // 回退到通用转换
        }

        // 原有通用转换逻辑
        return await ConvertGenericXmlToExcelAsync(xmlFilePath, excelFilePath);
    }
}
```

## 测试策略

### 新增测试类型
1. **XML类型检测测试**
2. **类型化转换测试**
3. **验证服务测试**
4. **集成测试**

### 测试覆盖率目标
- 单元测试覆盖率：90%+
- 集成测试覆盖率：80%+
- 端到端测试覆盖率：70%+

## 风险评估

### 技术风险
- **Common层依赖**: 增加了对Common层的依赖，需要处理版本兼容性
- **复杂性增加**: 类型化处理会增加系统复杂性
- **性能影响**: 需要监控性能影响

### 缓解策略
- **渐进式集成**: 分阶段集成，确保每阶段都经过充分测试
- **向后兼容**: 保持原有通用转换功能的完整性
- **性能监控**: 添加性能监控和优化机制

## 总结

通过实施这些改进建议，BannerlordModEditor.TUI项目将从：

**当前状态**: 通用XML转换工具
- 支持基础的XML↔Excel转换
- 缺乏Bannerlord特定功能
- 无法利用Common层的强大功能

**升级为**: 专业Bannerlord模组编辑器
- 支持102种XML类型的专门处理
- 完整的验证和错误处理
- 类型化的编辑界面
- 与Common层深度集成

这将使TUI项目成为真正专业的Bannerlord模组开发工具，大大提升开发效率和用户体验。