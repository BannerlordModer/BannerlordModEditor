using System;
using System.Collections.Generic;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;
using BannerlordModEditor.UI.ViewModels;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// 编辑器工厂接口
/// </summary>
public interface IEditorFactory
{
    /// <summary>
    /// 创建编辑器视图模型
    /// </summary>
    ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName);

    /// <summary>
    /// 创建编辑器视图
    /// </summary>
    BaseEditorView? CreateEditorView(string editorType);

    /// <summary>
    /// 注册编辑器类型
    /// </summary>
    void RegisterEditor<TViewModel, TView>(string editorType)
        where TViewModel : ViewModelBase
        where TView : BaseEditorView;

    /// <summary>
    /// 获取所有注册的编辑器类型
    /// </summary>
    IEnumerable<string> GetRegisteredEditorTypes();
}

/// <summary>
/// 编辑器工厂实现
/// </summary>
public class EditorFactory : IEditorFactory
{
    private readonly Dictionary<string, Type> _viewModelTypes = new();
    private readonly Dictionary<string, Type> _viewTypes = new();
    private readonly IServiceProvider _serviceProvider;

    public EditorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        RegisterDefaultEditors();
    }

    /// <summary>
    /// 注册默认编辑器
    /// </summary>
    private void RegisterDefaultEditors()
    {
        // 注册现有编辑器
        RegisterEditor<AttributeEditorViewModel, AttributeEditorView>("AttributeEditor");
        RegisterEditor<SkillEditorViewModel, SkillEditorView>("SkillEditor");
        RegisterEditor<BoneBodyTypeEditorViewModel, BoneBodyTypeEditorView>("BoneBodyTypeEditor");
        RegisterEditor<CraftingPieceEditorViewModel, CraftingPieceEditorView>("CraftingPieceEditor");
        RegisterEditor<ItemModifierEditorViewModel, ItemModifierEditorView>("ItemModifierEditor");
        
        // 注册通用数据网格编辑器
        RegisterGenericEditors();
    }

    /// <summary>
    /// 注册通用编辑器
    /// </summary>
    private void RegisterGenericEditors()
    {
        // 为常用的XML类型注册通用数据网格编辑器
        // 注意：这些编辑器需要在运行时通过反射创建
    }

    /// <summary>
    /// 注册编辑器类型
    /// </summary>
    public void RegisterEditor<TViewModel, TView>(string editorType)
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        _viewModelTypes[editorType] = typeof(TViewModel);
        _viewTypes[editorType] = typeof(TView);
    }

    /// <summary>
    /// 创建编辑器视图模型
    /// </summary>
    public ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName)
    {
        if (!_viewModelTypes.TryGetValue(editorType, out var viewModelType))
        {
            return null;
        }

        try
        {
            ViewModelBase? viewModel = null;

            // 尝试通过服务提供器创建实例
            viewModel = _serviceProvider.GetService(viewModelType) as ViewModelBase;
            
            // 如果失败，尝试直接创建
            if (viewModel == null)
            {
                viewModel = Activator.CreateInstance(viewModelType) as ViewModelBase;
            }

            if (viewModel == null)
            {
                return null;
            }

            // 如果是BaseEditorViewModel类型，设置XML文件名
            var baseEditorType = viewModel.GetType().GetInterface("IBaseEditorViewModel");
            if (baseEditorType != null)
            {
                // 尝试调用SetXmlFileName方法
                var setMethod = viewModelType.GetMethod("SetXmlFileName");
                if (setMethod != null)
                {
                    setMethod.Invoke(viewModel, new object[] { xmlFileName });
                }
                else
                {
                    // 尝试设置XmlFileName属性
                    var xmlFileNameProperty = viewModelType.GetProperty("XmlFileName");
                    if (xmlFileNameProperty != null && xmlFileNameProperty.CanWrite)
                    {
                        xmlFileNameProperty.SetValue(viewModel, xmlFileName);
                    }
                }
            }

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
        if (!_viewTypes.TryGetValue(editorType, out var viewType))
        {
            return null;
        }

        try
        {
            // 尝试通过服务提供器创建实例
            var view = _serviceProvider.GetService(viewType) as BaseEditorView;
            
            // 如果失败，尝试直接创建
            if (view == null)
            {
                view = Activator.CreateInstance(viewType) as BaseEditorView;
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
        return _viewModelTypes.Keys;
    }
}

/// <summary>
/// 编辑器注册扩展方法
/// </summary>
public static class EditorFactoryExtensions
{
    /// <summary>
    /// 批量注册编辑器
    /// </summary>
    public static void RegisterEditors(this IEditorFactory factory, 
        Dictionary<string, (Type ViewModelType, Type ViewType)> editors)
    {
        foreach (var editor in editors)
        {
            var method = typeof(EditorFactoryExtensions)
                .GetMethod(nameof(RegisterEditorInternal))?
                .MakeGenericMethod(editor.Value.ViewModelType, editor.Value.ViewType);
            
            method?.Invoke(factory, new object[] { factory, editor.Key });
        }
    }

    private static void RegisterEditorInternal<TViewModel, TView>(IEditorFactory factory, string editorType)
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        factory.RegisterEditor<TViewModel, TView>(editorType);
    }
}