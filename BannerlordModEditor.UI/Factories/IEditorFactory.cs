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

    /// <summary>
    /// 获取编辑器类型信息
    /// </summary>
    EditorTypeInfo? GetEditorTypeInfo(string editorType);

    /// <summary>
    /// 按类别获取编辑器类型
    /// </summary>
    IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category);

    /// <summary>
    /// 获取所有类别
    /// </summary>
    IEnumerable<string> GetCategories();

    /// <summary>
    /// 注册编辑器类型（增强方法）
    /// </summary>
    void RegisterEditor<TViewModel, TView>(string editorType, 
        string displayName, string description, string xmlFileName, string category = "General")
        where TViewModel : ViewModelBase
        where TView : BaseEditorView;

    /// <summary>
    /// 获取所有编辑器实例
    /// </summary>
    IEnumerable<ViewModelBase> GetAllEditors();
}