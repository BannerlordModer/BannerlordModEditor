using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.ViewModels.Editors;
using BannerlordModEditor.UI.Views.Editors;

namespace BannerlordModEditor.UI.Factories;

/// <summary>
/// 空编辑器工厂实现，用于当不使用编辑器工厂时的依赖注入
///
/// 这个类提供了 IEditorFactory 的空实现，确保在最小化配置下
/// EditorManagerFactory 仍然可以正常工作。
/// </summary>
public class NullEditorFactory : IEditorFactory
{
    /// <summary>
    /// 创建编辑器视图模型
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <param name="xmlFileName">XML文件名</param>
    /// <returns>null，因为空工厂不创建任何编辑器</returns>
    public ViewModelBase? CreateEditorViewModel(string editorType, string xmlFileName)
    {
        return null;
    }

    /// <summary>
    /// 创建编辑器视图
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <returns>null，因为空工厂不创建任何编辑器</returns>
    public BaseEditorView? CreateEditorView(string editorType)
    {
        return null;
    }

    /// <summary>
    /// 注册编辑器类型
    /// </summary>
    /// <typeparam name="TViewModel">视图模型类型</typeparam>
    /// <typeparam name="TView">视图类型</typeparam>
    /// <param name="editorType">编辑器类型名称</param>
    public void RegisterEditor<TViewModel, TView>(string editorType)
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        // 空实现，不注册任何编辑器
    }

    /// <summary>
    /// 获取所有注册的编辑器类型
    /// </summary>
    /// <returns>空的编辑器类型列表</returns>
    public IEnumerable<string> GetRegisteredEditorTypes()
    {
        return Array.Empty<string>();
    }

    /// <summary>
    /// 获取编辑器类型信息
    /// </summary>
    /// <param name="editorType">编辑器类型</param>
    /// <returns>null，因为空工厂不提供任何编辑器信息</returns>
    public EditorTypeInfo? GetEditorTypeInfo(string editorType)
    {
        return null;
    }

    /// <summary>
    /// 按类别获取编辑器类型
    /// </summary>
    /// <param name="category">类别名称</param>
    /// <returns>空的编辑器类型列表</returns>
    public IEnumerable<EditorTypeInfo> GetEditorsByCategory(string category)
    {
        return Array.Empty<EditorTypeInfo>();
    }

    /// <summary>
    /// 获取所有类别
    /// </summary>
    /// <returns>空的类别列表</returns>
    public IEnumerable<string> GetCategories()
    {
        return Array.Empty<string>();
    }

    /// <summary>
    /// 注册编辑器类型（增强方法）
    /// </summary>
    /// <typeparam name="TViewModel">视图模型类型</typeparam>
    /// <typeparam name="TView">视图类型</typeparam>
    /// <param name="editorType">编辑器类型名称</param>
    /// <param name="displayName">显示名称</param>
    /// <param name="description">描述</param>
    /// <param name="xmlFileName">XML文件名</param>
    /// <param name="category">类别</param>
    public void RegisterEditor<TViewModel, TView>(string editorType,
        string displayName, string description, string xmlFileName, string category = "General")
        where TViewModel : ViewModelBase
        where TView : BaseEditorView
    {
        // 空实现，不注册任何编辑器
    }

    /// <summary>
    /// 获取所有编辑器实例
    /// </summary>
    /// <returns>空的编辑器实例列表</returns>
    public IEnumerable<ViewModelBase> GetAllEditors()
    {
        return Array.Empty<ViewModelBase>();
    }
}