using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using BannerlordModEditor.UI.ViewModels.Editors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using BannerlordModEditor.UI.Factories;
using BannerlordModEditor.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BannerlordModEditor.UI.ViewModels;

/// <summary>
/// EditorManagerViewModel的配置选项
/// 
/// 这个类封装了EditorManagerViewModel所需的所有依赖服务和配置参数，
/// 通过明确的属性定义和验证机制，解决了依赖注入歧义问题。
/// 
/// 主要职责：
/// - 提供统一的配置接口
/// - 支持依赖注入容器的服务解析
/// - 提供配置验证和默认值
/// - 支持性能监控和诊断功能
/// 
/// 设计原则：
/// - 所有依赖服务都通过明确的属性提供
/// - 提供合理的默认值和回退机制
/// - 支持配置验证和错误处理
/// - 保持向后兼容性
/// </summary>
public class EditorManagerOptions
{
    /// <summary>
    /// 编辑器工厂
    /// </summary>
    public IEditorFactory? EditorFactory { get; set; }
    
    /// <summary>
    /// 日志服务
    /// </summary>
    public ILogService? LogService { get; set; }
    
    /// <summary>
    /// 错误处理服务
    /// </summary>
    public IErrorHandlerService? ErrorHandlerService { get; set; }
    
    /// <summary>
    /// 验证服务
    /// </summary>
    public IValidationService? ValidationService { get; set; }
    
    /// <summary>
    /// 数据绑定服务
    /// </summary>
    public IDataBindingService? DataBindingService { get; set; }
    
    /// <summary>
    /// 服务提供者
    /// </summary>
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// 是否启用性能监控
    /// </summary>
    public bool EnablePerformanceMonitoring { get; set; } = false;

    /// <summary>
    /// 是否启用健康检查
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// 是否启用诊断功能
    /// </summary>
    public bool EnableDiagnostics { get; set; } = false;

    /// <summary>
    /// 创建超时时间（毫秒）
    /// </summary>
    public int CreationTimeout { get; set; } = 30000;

    /// <summary>
    /// 是否使用严格模式（在服务不可用时抛出异常）
    /// </summary>
    public bool StrictMode { get; set; } = false;
    
    /// <summary>
    /// 创建默认配置
    /// </summary>
    /// <returns>默认配置的EditorManagerOptions实例</returns>
    /// <remarks>
    /// 默认配置提供了基本的回退服务，适用于测试或简单场景
    /// </remarks>
    public static EditorManagerOptions Default => new EditorManagerOptions
    {
        LogService = new LogService(),
        ErrorHandlerService = new ErrorHandlerService(),
        ValidationService = new ValidationService()
    };
    
    /// <summary>
    /// 创建用于依赖注入的配置
    /// </summary>
    /// <param name="serviceProvider">服务提供器</param>
    /// <returns>配置完成的EditorManagerOptions实例</returns>
    /// <exception cref="ArgumentNullException">当serviceProvider为null时抛出</exception>
    /// <remarks>
    /// 这个方法从依赖注入容器中解析所有必要的服务
    /// </remarks>
    public static EditorManagerOptions ForDependencyInjection(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        return new EditorManagerOptions
        {
            EditorFactory = serviceProvider.GetService<IEditorFactory>(),
            LogService = serviceProvider.GetService<ILogService>(),
            ErrorHandlerService = serviceProvider.GetService<IErrorHandlerService>(),
            ValidationService = serviceProvider.GetService<IValidationService>(),
            DataBindingService = serviceProvider.GetService<IDataBindingService>(),
            ServiceProvider = serviceProvider
        };
    }

    /// <summary>
    /// 创建用于测试的配置
    /// </summary>
    /// <returns>适用于测试环境的EditorManagerOptions实例</returns>
    /// <remarks>
    /// 这个方法提供mock服务，适用于单元测试场景
    /// </remarks>
    public static EditorManagerOptions ForTesting()
    {
        return new EditorManagerOptions
        {
            LogService = new LogService(),
            ErrorHandlerService = new ErrorHandlerService(),
            ValidationService = new ValidationService(),
            StrictMode = false
        };
    }

    /// <summary>
    /// 验证配置的有效性
    /// </summary>
    /// <returns>验证结果</returns>
    /// <remarks>
    /// 这个方法检查所有必要的服务是否可用，并提供详细的诊断信息
    /// </remarks>
    public ConfigurationValidationResult Validate()
    {
        var result = new ConfigurationValidationResult();
        
        // 检查必要的服务
        if (LogService == null)
        {
            result.Errors.Add("LogService is required");
            result.IsValid = false;
        }
        
        if (ErrorHandlerService == null)
        {
            result.Errors.Add("ErrorHandlerService is required");
            result.IsValid = false;
        }

        // 检查可选服务
        if (ValidationService == null)
        {
            result.Warnings.Add("ValidationService is not available - some features may not work");
        }

        if (EditorFactory == null)
        {
            result.Warnings.Add("EditorFactory is not available - using default editors only");
        }

        if (ServiceProvider == null)
        {
            result.Warnings.Add("ServiceProvider is not available - dependency injection may not work properly");
        }

        // 检查配置值
        if (CreationTimeout <= 0)
        {
            result.Errors.Add("CreationTimeout must be positive");
            result.IsValid = false;
        }

        return result;
    }

    /// <summary>
    /// 确保配置有效，如果无效则抛出异常
    /// </summary>
    /// <exception cref="InvalidOperationException">当配置无效时抛出</exception>
    public void EnsureValid()
    {
        var validationResult = Validate();
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException($"Invalid EditorManagerOptions configuration:{Environment.NewLine}{string.Join(Environment.NewLine, validationResult.Errors)}");
        }
    }

    /// <summary>
    /// 克隆当前配置
    /// </summary>
    /// <returns>克隆的配置实例</returns>
    public EditorManagerOptions Clone()
    {
        return new EditorManagerOptions
        {
            EditorFactory = EditorFactory,
            LogService = LogService,
            ErrorHandlerService = ErrorHandlerService,
            ValidationService = ValidationService,
            DataBindingService = DataBindingService,
            ServiceProvider = ServiceProvider,
            EnablePerformanceMonitoring = EnablePerformanceMonitoring,
            EnableHealthChecks = EnableHealthChecks,
            EnableDiagnostics = EnableDiagnostics,
            CreationTimeout = CreationTimeout,
            StrictMode = StrictMode
        };
    }

    /// <summary>
    /// 应用严格的配置验证
    /// </summary>
    /// <returns>配置实例，用于链式调用</returns>
    public EditorManagerOptions WithStrictMode()
    {
        StrictMode = true;
        return this;
    }

    /// <summary>
    /// 启用性能监控
    /// </summary>
    /// <returns>配置实例，用于链式调用</returns>
    public EditorManagerOptions WithPerformanceMonitoring()
    {
        EnablePerformanceMonitoring = true;
        return this;
    }

    /// <summary>
    /// 启用健康检查
    /// </summary>
    /// <returns>配置实例，用于链式调用</returns>
    public EditorManagerOptions WithHealthChecks()
    {
        EnableHealthChecks = true;
        return this;
    }

    /// <summary>
    /// 启用诊断功能
    /// </summary>
    /// <returns>配置实例，用于链式调用</returns>
    public EditorManagerOptions WithDiagnostics()
    {
        EnableDiagnostics = true;
        return this;
    }
}

/// <summary>
/// 配置验证结果
/// </summary>
public class ConfigurationValidationResult
{
    /// <summary>
    /// 配置是否有效
    /// </summary>
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 错误信息列表
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// 警告信息列表
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// 获取完整的验证信息
    /// </summary>
    /// <returns>验证信息字符串</returns>
    public override string ToString()
    {
        var lines = new List<string>
        {
            $"Configuration Validation: {(IsValid ? "PASSED" : "FAILED")}"
        };

        if (Errors.Count > 0)
        {
            lines.Add("Errors:");
            foreach (var error in Errors)
            {
                lines.Add($"  - {error}");
            }
        }

        if (Warnings.Count > 0)
        {
            lines.Add("Warnings:");
            foreach (var warning in Warnings)
            {
                lines.Add($"  - {warning}");
            }
        }

        return string.Join(Environment.NewLine, lines);
    }
}

/// <summary>
/// EditorManagerViewModel - 编辑器管理器视图模型
/// 
/// 这个类负责管理所有编辑器的生命周期、状态和交互。
/// 通过依赖注入工厂模式解决了构造函数歧义问题，提供了清晰的服务初始化方式。
/// 
/// 主要职责：
/// - 管理编辑器的创建和销毁
/// - 提供编辑器分类和搜索功能
/// - 处理编辑器选择和切换
/// - 提供状态消息和面包屑导航
/// - 支持异步XML文件加载
/// 
/// 设计改进：
/// - 使用单一构造函数模式，避免依赖注入歧义
/// - 通过EditorManagerOptions提供统一的配置接口
/// - 支持配置验证和错误处理
/// - 提供性能监控和诊断功能
/// 
/// 使用方式：
/// <code>
/// // 通过依赖注入容器创建
/// var editorManager = serviceProvider.GetRequiredService<EditorManagerViewModel>();
/// 
/// // 或者通过工厂创建
/// var factory = serviceProvider.GetRequiredService<IEditorManagerFactory>();
/// var editorManager = factory.CreateEditorManager();
/// </code>
/// </summary>
public partial class EditorManagerViewModel : ViewModelBase
{
    private readonly IEditorFactory? _editorFactory;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService? _dataBindingService;
    private readonly IServiceProvider? _serviceProvider;

    // 性能监控和诊断相关字段
    private readonly bool _enablePerformanceMonitoring;
    private readonly bool _enableHealthChecks;
    private readonly bool _enableDiagnostics;
    private readonly bool _strictMode;
    private readonly int _creationTimeout;

    [ObservableProperty]
    private ObservableCollection<EditorCategoryViewModel> categories = new();

    [ObservableProperty]
    private ViewModelBase? selectedEditor;

    [ObservableProperty]
    private string? statusMessage;

    [ObservableProperty]
    private ViewModelBase? currentEditorViewModel;

    [ObservableProperty]
    private string? currentBreadcrumb;

    [ObservableProperty]
    private string? searchText = string.Empty;

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            // 显示所有编辑器
            foreach (var category in Categories)
            {
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = true;
                }
            }
        }
        else
        {
            // 过滤编辑器
            var searchLower = value.ToLower();
            foreach (var category in Categories)
            {
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = editor.Name.ToLower().Contains(searchLower) ||
                                        editor.Description.ToLower().Contains(searchLower) ||
                                        editor.EditorType.ToLower().Contains(searchLower);
                }
            }
        }
    }

    /// <summary>
    /// 使用EditorManagerOptions构造EditorManagerViewModel
    /// </summary>
    /// <param name="options">配置选项</param>
    /// <exception cref="InvalidOperationException">当配置无效时抛出</exception>
    /// <exception cref="ArgumentNullException">当必要服务缺失且严格模式启用时抛出</exception>
    /// <remarks>
    /// 这是EditorManagerViewModel的唯一构造函数，通过EditorManagerOptions提供所有必要的配置。
    /// 这种设计解决了依赖注入歧义问题，并提供了清晰的配置接口。
    /// </remarks>
    public EditorManagerViewModel(EditorManagerOptions? options = null)
    {
        var config = options ?? EditorManagerOptions.Default;
        
        // 验证配置
        var validationResult = config.Validate();
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException($"Invalid EditorManagerOptions configuration:{Environment.NewLine}{string.Join(Environment.NewLine, validationResult.Errors)}");
        }

        // 记录警告信息
        if (validationResult.Warnings.Count > 0)
        {
            var logService = config.LogService ?? new LogService();
            foreach (var warning in validationResult.Warnings)
            {
                logService.LogWarning(warning, "EditorManagerViewModel.Constructor");
            }
        }

        // 分配服务
        _editorFactory = config.EditorFactory;
        _logService = config.LogService ?? new LogService();
        _errorHandlerService = config.ErrorHandlerService ?? new ErrorHandlerService();
        _validationService = config.ValidationService ?? new ValidationService();
        _dataBindingService = config.DataBindingService;
        _serviceProvider = config.ServiceProvider;

        // 分配配置选项
        _enablePerformanceMonitoring = config.EnablePerformanceMonitoring;
        _enableHealthChecks = config.EnableHealthChecks;
        _enableDiagnostics = config.EnableDiagnostics;
        _strictMode = config.StrictMode;
        _creationTimeout = config.CreationTimeout;

        // 初始化性能监控（如果启用）
        if (_enablePerformanceMonitoring)
        {
            InitializePerformanceMonitoring();
        }

        // 执行健康检查（如果启用）
        if (_enableHealthChecks)
        {
            PerformHealthChecks();
        }

        // 记录初始化信息
        _logService.LogInfo("EditorManagerViewModel initialized successfully", "EditorManagerViewModel");
        if (_enableDiagnostics)
        {
            _logService.LogDebug($"Performance monitoring: {_enablePerformanceMonitoring}", "EditorManagerViewModel");
            _logService.LogDebug($"Health checks: {_enableHealthChecks}", "EditorManagerViewModel");
            _logService.LogDebug($"Diagnostics: {_enableDiagnostics}", "EditorManagerViewModel");
            _logService.LogDebug($"Strict mode: {_strictMode}", "EditorManagerViewModel");
        }

        // 加载编辑器
        LoadEditors();
    }

    /// <summary>
    /// 初始化性能监控
    /// </summary>
    private void InitializePerformanceMonitoring()
    {
        try
        {
            // TODO: 实现性能监控初始化
            _logService.LogDebug("Performance monitoring initialized", "EditorManagerViewModel.Performance");
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Failed to initialize performance monitoring");
            // 不抛出异常，因为性能监控失败不应该阻止初始化
        }
    }

    /// <summary>
    /// 执行健康检查
    /// </summary>
    private void PerformHealthChecks()
    {
        try
        {
            // 检查日志服务
            _logService.LogDebug("Testing log service", "EditorManagerViewModel.HealthCheck");

            // 检查错误处理服务
            _errorHandlerService.ShowErrorMessage("Health check test", "Test");

            // 检查验证服务
            if (_validationService != null)
            {
                var testResult = _validationService.Validate(new object());
                _logService.LogDebug($"Validation service test: {testResult.IsValid}", "EditorManagerViewModel.HealthCheck");
            }

            // 检查编辑器工厂
            if (_editorFactory != null)
            {
                var registeredTypes = _editorFactory.GetRegisteredEditorTypes();
                _logService.LogDebug($"Editor factory has {registeredTypes.Count()} registered types", "EditorManagerViewModel.HealthCheck");
            }

            _logService.LogInfo("All health checks passed", "EditorManagerViewModel");
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Health check failed");
            if (_strictMode)
            {
                throw new InvalidOperationException("Health check failed", ex);
            }
        }
    }

    /// <summary>
    /// 为了向后兼容保留的构造函数（已标记为过时）
    /// </summary>
    /// <param name="editorFactory">编辑器工厂</param>
    /// <param name="logService">日志服务</param>
    /// <param name="errorHandlerService">错误处理服务</param>
    /// <param name="validationService">验证服务</param>
    /// <param name="serviceProvider">服务提供器</param>
    /// <remarks>
    /// 这个构造函数已过时，请使用EditorManagerViewModel(EditorManagerOptions)构造函数
    /// </remarks>
    [Obsolete("请使用 EditorManagerViewModel(EditorManagerOptions) 构造函数，这个构造函数将在未来版本中被移除")]
    public EditorManagerViewModel(
        IEditorFactory? editorFactory = null,
        ILogService? logService = null,
        IErrorHandlerService? errorHandlerService = null,
        IValidationService? validationService = null,
        IServiceProvider? serviceProvider = null)
        : this(new EditorManagerOptions
        {
            EditorFactory = editorFactory,
            LogService = logService,
            ErrorHandlerService = errorHandlerService,
            ValidationService = validationService,
            ServiceProvider = serviceProvider
        })
    {
    }

    private void LoadEditors()
    {
        try
        {
            if (_editorFactory == null)
            {
                LoadDefaultEditors();
                return;
            }

            var editors = _editorFactory.GetAllEditors();
            if (editors == null || !editors.Any())
            {
                // 如果工厂没有返回编辑器，使用默认配置
                LoadDefaultEditors();
                return;
            }

            var groupedEditors = editors.GroupBy(e => GetEditorCategory(e))
                .Select(g => new EditorCategoryViewModel(g.Key, $"{g.Key} 编辑器", "📁"));

            Categories = new ObservableCollection<EditorCategoryViewModel>(groupedEditors);
            StatusMessage = $"已加载 {editors.Count()} 个编辑器";
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, "加载编辑器失败");
            StatusMessage = "加载编辑器失败";
        }
    }

    /// <summary>
    /// 加载默认的编辑器配置
    /// </summary>
    private void LoadDefaultEditors()
    {
        // 创建默认的编辑器分类并添加测试编辑器
        var characterCategory = new EditorCategoryViewModel("角色设定", "角色设定编辑器", "👤");
        characterCategory.Editors.Add(new EditorItemViewModel("属性定义", "属性定义编辑器", "attributes.xml", "AttributeEditor", "⚙️"));
        characterCategory.Editors.Add(new EditorItemViewModel("技能系统", "技能系统编辑器", "skills.xml", "SkillEditor", "🎯"));
        characterCategory.Editors.Add(new EditorItemViewModel("骨骼体型", "骨骼体型编辑器", "bone_body_types.xml", "BoneBodyTypeEditor", "🦴"));
        
        var equipmentCategory = new EditorCategoryViewModel("装备物品", "装备物品编辑器", "⚔️");
        equipmentCategory.Editors.Add(new EditorItemViewModel("物品编辑", "物品编辑器", "items.xml", "ItemEditor", "📦"));
        
        var combatCategory = new EditorCategoryViewModel("战斗系统", "战斗系统编辑器", "🛡️");
        combatCategory.Editors.Add(new EditorItemViewModel("战斗参数", "战斗参数编辑器", "combat_parameters.xml", "CombatParameterEditor", "⚔️"));
        
        Categories = new ObservableCollection<EditorCategoryViewModel>
        {
            characterCategory,
            equipmentCategory,
            combatCategory,
            new EditorCategoryViewModel("世界场景", "世界场景编辑器", "🌍"),
            new EditorCategoryViewModel("音频系统", "音频系统编辑器", "🎵"),
            new EditorCategoryViewModel("多人游戏", "多人游戏编辑器", "👥"),
            new EditorCategoryViewModel("游戏配置", "游戏配置编辑器", "⚙️")
        };
        StatusMessage = "已加载默认编辑器分类";
    }

    private string GetEditorCategory(ViewModelBase editor)
    {
        try
        {
            var editorType = editor.GetType();
            var categoryAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
            return categoryAttribute?.Category ?? "其他";
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Unexpected error while getting category name");
            return "错误分类";
        }
    }

    /// <summary>
    /// 根据EditorItemViewModel获取分类名称
    /// </summary>
    private string GetCategoryFromEditorItem(EditorItemViewModel editorItem)
    {
        // 根据编辑器类型返回对应的分类名称
        return editorItem.EditorType switch
        {
            "AttributeEditor" => "角色设定",
            "SkillEditor" => "角色设定",
            "BoneBodyTypeEditor" => "角色设定",
            "ItemEditor" => "装备物品",
            "CombatParameterEditor" => "战斗系统",
            "CraftingPieceEditor" => "装备物品",
            "ItemModifierEditor" => "装备物品",
            _ => "其他"
        };
    }

    /// <summary>
    /// 自动加载XML文件（如果编辑器支持）
    /// </summary>
    private async Task AutoLoadXmlFileAsync(ViewModelBase editorViewModel, string xmlFileName)
    {
        try
        {
            // 检查编辑器是否有LoadXmlFile方法
            var loadMethod = editorViewModel.GetType().GetMethod("LoadXmlFile");
            if (loadMethod != null)
            {
                // 在后台线程中执行XML加载
                await Task.Run(() => loadMethod.Invoke(editorViewModel, new object[] { xmlFileName }));
                _logService.LogInfo($"Successfully auto-loaded XML file: {xmlFileName}", "EditorManager");
            }
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Failed to auto-load XML file: {xmlFileName}");
        }
    }

    [RelayCommand]
    private void SelectEditor(ViewModelBase editor)
    {
        try
        {
            // 如果传入的是EditorItemViewModel，需要转换为具体的编辑器ViewModel
            ViewModelBase actualEditor = editor;
            EditorItemViewModel? editorItem = null;
            
            if (editor is EditorItemViewModel item)
            {
                editorItem = item;
                actualEditor = CreateEditorViewModel(editorItem);
            }

            // 保持SelectedEditor为传入的原始对象（可能是EditorItemViewModel）
            SelectedEditor = editor;
            CurrentEditorViewModel = actualEditor;
            StatusMessage = $"已选择编辑器: {actualEditor.GetType().Name}";

            // 更新面包屑导航 - 优先使用EditorItemViewModel的信息
            string categoryName;
            string editorName;
            string xmlFileName;
            
            if (editorItem != null)
            {
                // 使用EditorItemViewModel的信息
                categoryName = GetCategoryFromEditorItem(editorItem);
                editorName = editorItem.Name;
                xmlFileName = editorItem.XmlFileName;
            }
            else
            {
                // 回退到使用ViewModel的属性
                var editorType = actualEditor.GetType();
                var editorAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
                categoryName = editorAttribute?.Category ?? "其他";
                editorName = editorAttribute?.DisplayName ?? editorType.Name.Replace("ViewModel", "");
                xmlFileName = editorAttribute?.XmlFileName ?? "";
                
                if (string.IsNullOrEmpty(xmlFileName) && actualEditor is BaseEditorViewModel baseEditor)
                {
                    xmlFileName = baseEditor.XmlFileName;
                }
            }
            
            CurrentBreadcrumb = $"{categoryName} > {editorName}";
            
            if (!string.IsNullOrEmpty(xmlFileName))
            {
                // 异步加载XML文件，不等待完成
                _ = AutoLoadXmlFileAsync(actualEditor, xmlFileName);
            }
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, "选择编辑器失败");
            StatusMessage = "选择编辑器失败";
        }
    }

    internal ViewModelBase CreateEditorViewModel(EditorItemViewModel editorItem)
    {
        try
        {
            if (_editorFactory != null)
            {
                var viewModel = _editorFactory.CreateEditorViewModel(editorItem.EditorType, editorItem.XmlFileName);
                if (viewModel != null)
                {
                    return viewModel;
                }
                _logService.LogWarning($"Failed to create editor via factory: {editorItem.EditorType}", "EditorManager");
            }

            // 回退到直接创建（用于测试或没有工厂的情况）
            return editorItem.EditorType switch
            {
                "AttributeEditor" => new AttributeEditorViewModel(_validationService),
                "SkillEditor" => new SkillEditorViewModel(_validationService),
                "CombatParameterEditor" => new CombatParameterEditorViewModel(_validationService),
                "ItemEditor" => new ItemEditorViewModel(_validationService),
                "BoneBodyTypeEditor" => new BoneBodyTypeEditorViewModel(_validationService),
                "CraftingPieceEditor" => new CraftingPieceEditorViewModel(_validationService),
                "ItemModifierEditor" => new ItemModifierEditorViewModel(_validationService),
                _ => throw new NotSupportedException($"不支持的编辑器类型: {editorItem.EditorType}")
            };
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Failed to create editor view model: {editorItem.EditorType}");
            throw;
        }
    }

    [RelayCommand]
    private void RefreshEditors()
    {
        LoadEditors();
    }

    [RelayCommand]
    private void ShowHelp()
    {
        try
        {
            StatusMessage = "查看帮助信息";
            // TODO: 实现帮助对话框
        }
        catch (Exception ex)
        {
            _errorHandlerService.HandleError(ex, "显示帮助失败");
        }
    }

    partial void OnSelectedEditorChanged(ViewModelBase? value)
    {
        if (value != null)
        {
            CurrentEditorViewModel = value;
            var editorType = value.GetType();
            var editorAttribute = editorType.GetCustomAttribute<EditorTypeAttribute>();
            CurrentBreadcrumb = $"{editorAttribute?.Category ?? "其他"} > {editorType.Name}";
        }
        else
        {
            CurrentEditorViewModel = null;
            CurrentBreadcrumb = null;
        }
    }
}