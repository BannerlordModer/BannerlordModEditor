using System.Reflection;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// 统一的编辑器工厂实现
/// </summary>
public class UnifiedEditorFactory : IEditorFactory
{
    private readonly Dictionary<string, EditorTypeInfo> _editorTypes = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService _dataBindingService;
    private readonly ILogService _logService;
    private readonly IErrorHandlerService _errorHandlerService;

    public UnifiedEditorFactory(
        IServiceProvider serviceProvider,
        IValidationService validationService,
        IDataBindingService dataBindingService,
        ILogService logService,
        IErrorHandlerService errorHandlerService)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _dataBindingService = dataBindingService ?? throw new ArgumentNullException(nameof(dataBindingService));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        _errorHandlerService = errorHandlerService ?? throw new ArgumentNullException(nameof(errorHandlerService));
        
        RegisterDefaultEditors();
        RegisterEditorsByReflection();
    }

    /// <summary>
    /// 注册默认编辑器
    /// </summary>
    private void RegisterDefaultEditors()
    {
        RegisterEditor<AttributeEditorViewModel, AttributeEditorView>("AttributeEditor", 
            "属性编辑器", "编辑角色属性", "attributes.xml", "Character");
        
        RegisterEditor<SkillEditorViewModel, SkillEditorView>("SkillEditor", 
            "技能编辑器", "编辑角色技能", "skills.xml", "Character");
        
        RegisterEditor<BoneBodyTypeEditorViewModel, BoneBodyTypeEditorView>("BoneBodyTypeEditor", 
            "骨骼类型编辑器", "编辑骨骼类型", "bone_body_types.xml", "Character");
        
        RegisterEditor<CraftingPieceEditorViewModel, CraftingPieceEditorView>("CraftingPieceEditor", 
            "制作件编辑器", "编辑制作件", "crafting_pieces.xml", "Crafting");
        
        RegisterEditor<ItemModifierEditorViewModel, ItemModifierEditorView>("ItemModifierEditor", 
            "物品修饰器编辑器", "编辑物品修饰器", "item_modifiers.xml", "Items");
    }

    /// <summary>
    /// 通过反射注册编辑器
    /// </summary>
    private void RegisterEditorsByReflection()
    {
        try
        {
            var viewModelAssembly = typeof(AttributeEditorViewModel).Assembly;
            var viewAssembly = typeof(AttributeEditorView).Assembly;

            var viewModelTypes = viewModelAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel"))
                .Where(t => t.GetCustomAttribute<EditorTypeAttribute>() != null);

            foreach (var viewModelType in viewModelTypes)
            {
                var editorAttribute = viewModelType.GetCustomAttribute<EditorTypeAttribute>();
                if (editorAttribute != null)
                {
                    var viewTypeName = viewModelType.Name.Replace("ViewModel", "View");
                    var viewType = viewAssembly.GetTypes()
                        .FirstOrDefault(t => t.Name == viewTypeName);

                    if (viewType != null)
                    {
                        var method = GetType().GetMethod(nameof(RegisterEditor), 2, 
                            new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string) });
                        
                        var genericMethod = method?.MakeGenericMethod(viewModelType, viewType);
                        genericMethod?.Invoke(this, new object[] 
                        { 
                            editorAttribute.EditorType, 
                            editorAttribute.DisplayName, 
                            editorAttribute.Description, 
                            editorAttribute.XmlFileName, 
                            editorAttribute.Category 
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, "Failed to register editors by reflection");
        }
    }

    /// <summary>
    /// 注册编辑器类型
    /// </summary>
    public void RegisterEditor<TViewModel, TView>(string editorType)
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        var editorInfo = new EditorTypeInfo
        {
            EditorType = editorType,
            ViewModelType = typeof(TViewModel),
            ViewType = typeof(TView),
            Category = "General",
            SupportsDto = false
        };

        _editorTypes[editorType] = editorInfo;
    }

    /// <summary>
    /// 注册编辑器类型（增强方法）
    /// </summary>
    public void RegisterEditor<TViewModel, TView>(string editorType, 
        string displayName, string description, string xmlFileName, string category = "General")
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        var editorInfo = new EditorTypeInfo
        {
            EditorType = editorType,
            DisplayName = displayName,
            Description = description,
            XmlFileName = xmlFileName,
            ViewModelType = typeof(TViewModel),
            ViewType = typeof(TView),
            Category = category,
            SupportsDto = false // 默认不支持DTO，可以通过属性设置
        };

        _editorTypes[editorType] = editorInfo;
    }

    /// <summary>
    /// 创建编辑器视图模型
    /// </summary>
    public ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName)
    {
        if (!_editorTypes.TryGetValue(editorType, out var editorInfo))
        {
            _logService.LogWarning($"Editor type not found: {editorType}", "EditorFactory");
            return null;
        }

        try
        {
            // 尝试通过服务提供器创建实例
            var viewModel = _serviceProvider.GetService(editorInfo.ViewModelType) as ViewModelBase;
            
            // 如果失败，尝试直接创建
            if (viewModel == null)
            {
                viewModel = Activator.CreateInstance(editorInfo.ViewModelType) as ViewModelBase;
            }

            if (viewModel == null)
            {
                _logService.LogError($"Failed to create view model instance for type: {editorInfo.ViewModelType.Name}", "EditorFactory");
                return null;
            }

            // 注入服务
            InjectServices(viewModel);

            return viewModel;
        }
        catch (TargetInvocationException ex)
        {
            _logService.LogException(ex, $"Constructor exception for editor: {editorType}");
            return null;
        }
        catch (TypeLoadException ex)
        {
            _logService.LogException(ex, $"Type load exception for editor: {editorType}");
            return null;
        }
        catch (MissingMethodException ex)
        {
            _logService.LogException(ex, $"Missing constructor for editor: {editorType}");
            return null;
        }
        catch (InvalidOperationException ex)
        {
            _logService.LogException(ex, $"Invalid operation creating editor: {editorType}");
            return null;
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Unexpected error creating editor view model: {editorType}");
            return null;
        }
    }

    /// <summary>
    /// 创建编辑器视图
    /// </summary>
    public BaseEditorView? CreateEditorView(string editorType)
    {
        if (!_editorTypes.TryGetValue(editorType, out var editorInfo))
        {
            _logService.LogWarning($"Editor type not found: {editorType}", "EditorFactory");
            return null;
        }

        try
        {
            // 尝试通过服务提供器创建实例
            var view = _serviceProvider.GetService(editorInfo.ViewType) as BaseEditorView;
            
            // 如果失败，尝试直接创建
            if (view == null)
            {
                view = Activator.CreateInstance(editorInfo.ViewType) as BaseEditorView;
            }

            if (view == null)
            {
                _logService.LogError($"Failed to create view instance for type: {editorInfo.ViewType.Name}", "EditorFactory");
                return null;
            }

            return view;
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Failed to create editor view: {editorType}");
            return null;
        }
    }

    /// <summary>
    /// 获取所有注册的编辑器类型
    /// </summary>
    public IEnumerable<string> GetRegisteredEditorTypes()
    {
        return _editorTypes.Keys;
    }

    /// <summary>
    /// 获取编辑器类型信息
    /// </summary>
    public EditorTypeInfo? GetEditorTypeInfo(string editorType)
    {
        return _editorTypes.TryGetValue(editorType, out var info) ? info : null;
    }

    /// <summary>
    /// 按类别获取编辑器类型
    /// </summary>
    public IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category)
    {
        return _editorTypes.Values.Where(e => e.Category == category);
    }

    /// <summary>
    /// 获取所有类别
    /// </summary>
    public IEnumerable<string> GetCategories()
    {
        return _editorTypes.Values.Select(e => e.Category).Distinct();
    }

    /// <summary>
    /// 注入服务到视图模型
    /// </summary>
    private void InjectServices(ViewModelBase viewModel)
    {
        try
        {
            // 查找接受服务的属性
            var properties = viewModel.GetType().GetProperties()
                .Where(p => p.CanWrite && p.PropertyType == typeof(ILogService));

            foreach (var property in properties)
            {
                try
                {
                    property.SetValue(viewModel, _logService);
                }
                catch (TargetException ex)
                {
                    _logService.LogWarning($"Failed to inject service {property.Name} into {viewModel.GetType().Name}: {ex.Message}", "EditorFactory");
                }
            }

            // 注入其他服务
            var validationProperties = viewModel.GetType().GetProperties()
                .Where(p => p.CanWrite && p.PropertyType == typeof(IValidationService));

            foreach (var property in validationProperties)
            {
                try
                {
                    property.SetValue(viewModel, _validationService);
                }
                catch (TargetException ex)
                {
                    _logService.LogWarning($"Failed to inject validation service into {viewModel.GetType().Name}: {ex.Message}", "EditorFactory");
                }
            }

            var errorHandlerProperties = viewModel.GetType().GetProperties()
                .Where(p => p.CanWrite && p.PropertyType == typeof(IErrorHandlerService));

            foreach (var property in errorHandlerProperties)
            {
                try
                {
                    property.SetValue(viewModel, _errorHandlerService);
                }
                catch (TargetException ex)
                {
                    _logService.LogWarning($"Failed to inject error handler service into {viewModel.GetType().Name}: {ex.Message}", "EditorFactory");
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            _logService.LogException(ex, $"Failed to get properties for view model: {viewModel.GetType().Name}");
        }
        catch (Exception ex)
        {
            _logService.LogException(ex, $"Unexpected error injecting services into view model: {viewModel.GetType().Name}");
        }
    }

    /// <summary>
    /// 获取所有编辑器实例
    /// </summary>
    public IEnumerable<ViewModelBase> GetAllEditors()
    {
        var editors = new List<ViewModelBase>();
        
        foreach (var editorInfo in _editorTypes.Values)
        {
            try
            {
                var editor = CreateEditorViewModel(editorInfo.EditorType, editorInfo.XmlFileName);
                if (editor != null)
                {
                    editors.Add(editor);
                }
            }
            catch (Exception ex)
            {
                _logService.LogException(ex, $"Failed to create editor instance for {editorInfo.EditorType}");
            }
        }
        
        return editors;
    }
}