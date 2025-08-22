using System.Reflection;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using System.Collections.Generic;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// 编辑器类型信息
/// </summary>
public class EditorTypeInfo
{
    public string EditorType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string XmlFileName { get; set; } = string.Empty;
    public Type ViewModelType { get; set; } = null!;
    public Type ViewType { get; set; } = null!;
    public bool SupportsDto { get; set; }
    public string Category { get; set; } = "General";
}

/// <summary>
/// 增强的编辑器工厂
/// </summary>
public class EnhancedEditorFactory : IEditorFactory
{
    private readonly Dictionary<string, EditorTypeInfo> _editorTypes = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IValidationService _validationService;
    private readonly IDataBindingService _dataBindingService;

    public EnhancedEditorFactory(
        IServiceProvider serviceProvider,
        IValidationService validationService,
        IDataBindingService dataBindingService)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        _dataBindingService = dataBindingService ?? throw new ArgumentNullException(nameof(dataBindingService));
        
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
        // 扫描所有程序集中的编辑器类型
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var assembly in assemblies)
        {
            try
            {
                RegisterEditorsFromAssembly(assembly);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to scan assembly {assembly.FullName}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 从程序集注册编辑器
    /// </summary>
    private void RegisterEditorsFromAssembly(Assembly assembly)
    {
        // 查找所有编辑器ViewModel类型
        var viewModelTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                       (t.Name.EndsWith("EditorViewModel") || t.Name.EndsWith("ViewModel")))
            .ToList();

        foreach (var viewModelType in viewModelTypes)
        {
            // 跳过基类
            if (viewModelType.Name == "BaseEditorViewModel" || 
                viewModelType.Name == "GenericEditorViewModel" ||
                viewModelType.Name == "SimpleEditorViewModel")
            {
                continue;
            }

            // 查找对应的View类型
            var viewTypeName = viewModelType.Name.Replace("ViewModel", "View");
            var viewType = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == viewTypeName && 
                                   typeof(BaseEditorView).IsAssignableFrom(t));

            if (viewType != null)
            {
                // 获取编辑器属性信息
                var editorTypeAttr = viewModelType.GetCustomAttribute<EditorTypeAttribute>();
                var editorInfo = new EditorTypeInfo
                {
                    ViewModelType = viewModelType,
                    ViewType = viewType,
                    EditorType = editorTypeAttr?.EditorType ?? viewModelType.Name.Replace("ViewModel", ""),
                    DisplayName = editorTypeAttr?.DisplayName ?? viewModelType.Name.Replace("ViewModel", ""),
                    Description = editorTypeAttr?.Description ?? $"编辑{viewModelType.Name.Replace("ViewModel", "")}",
                    XmlFileName = editorTypeAttr?.XmlFileName ?? $"{viewModelType.Name.Replace("ViewModel", "").Replace("Editor", "").ToLower()}.xml",
                    Category = editorTypeAttr?.Category ?? "General",
                    SupportsDto = editorTypeAttr?.SupportsDto ?? false
                };

                _editorTypes[editorInfo.EditorType] = editorInfo;
            }
        }
    }

    /// <summary>
    /// 注册编辑器类型（重载方法，简化接口实现）
    /// </summary>
    public void RegisterEditor<TViewModel, TView>(string editorType)
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        var editorInfo = new EditorTypeInfo
        {
            EditorType = editorType,
            DisplayName = editorType,
            Description = $"编辑{editorType}",
            XmlFileName = $"{editorType.ToLower()}.xml",
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
            return null;
        }

        try
        {
            ViewModelBase? viewModel = null;

            // 尝试通过服务提供器创建实例
            viewModel = _serviceProvider.GetService(editorInfo.ViewModelType) as ViewModelBase;
            
            // 如果失败，尝试直接创建
            if (viewModel == null)
            {
                viewModel = Activator.CreateInstance(editorInfo.ViewModelType) as ViewModelBase;
            }

            if (viewModel == null)
            {
                return null;
            }

            // 如果是BaseEditorViewModel类型，设置XML文件名
            if (viewModel is IBaseEditorViewModel baseEditor)
            {
                baseEditor.SetXmlFileName(xmlFileName);
            }

            // 注入服务
            InjectServices(viewModel);

            return viewModel;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to create editor view model: {ex.Message}");
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

            return view;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to create editor view: {ex.Message}");
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
        // 这里可以注入各种服务到视图模型中
        // 例如：验证服务、数据绑定服务等
        
        // 查找接受服务的属性
        var properties = viewModel.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(IValidationService) || 
                       p.PropertyType == typeof(IDataBindingService));

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(IValidationService))
            {
                property.SetValue(viewModel, _validationService);
            }
            else if (property.PropertyType == typeof(IDataBindingService))
            {
                property.SetValue(viewModel, _dataBindingService);
            }
        }
    }
}

/// <summary>
/// 编辑器类型属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class EditorTypeAttribute : Attribute
{
    public string EditorType { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string XmlFileName { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
    public bool SupportsDto { get; set; } = false;
}