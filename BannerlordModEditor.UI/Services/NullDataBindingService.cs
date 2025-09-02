using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BannerlordModEditor.UI.Services;

/// <summary>
/// 空数据绑定服务实现，用于当不使用数据绑定服务时的依赖注入
/// </summary>
public class NullDataBindingService : IDataBindingService
{
    /// <summary>
    /// 创建双向数据绑定
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TTarget">目标类型</typeparam>
    /// <param name="source">源对象</param>
    /// <param name="sourceProperty">源属性</param>
    /// <param name="target">目标对象</param>
    /// <param name="targetProperty">目标属性</param>
    /// <param name="twoWay">是否双向绑定</param>
    /// <returns>可释放对象</returns>
    public IDisposable CreateBinding<TSource, TTarget>(
        ObservableObject source,
        string sourceProperty,
        ObservableObject target,
        string targetProperty,
        bool twoWay = true)
    {
        return new EmptyDisposable();
    }

    /// <summary>
    /// 创建集合绑定
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="target">目标集合</param>
    /// <returns>可释放对象</returns>
    public IDisposable CreateCollectionBinding<T>(
        ObservableCollection<T> source,
        ObservableCollection<T> target)
    {
        return new EmptyDisposable();
    }

    /// <summary>
    /// 创建验证绑定
    /// </summary>
    /// <param name="source">源对象</param>
    /// <param name="propertyName">属性名称</param>
    /// <param name="validationCallback">验证回调</param>
    /// <returns>可释放对象</returns>
    public IDisposable CreateValidationBinding(
        ObservableObject source,
        string propertyName,
        Action<List<string>> validationCallback)
    {
        return new EmptyDisposable();
    }

    /// <summary>
    /// 批量更新属性
    /// </summary>
    /// <param name="obj">要更新的对象</param>
    /// <param name="updateAction">更新操作</param>
    public void BatchUpdate(ObservableObject obj, Action updateAction)
    {
        updateAction?.Invoke();
    }

    /// <summary>
    /// 空的可释放对象实现
    /// </summary>
    private class EmptyDisposable : IDisposable
    {
        public void Dispose()
        {
            // 空实现
        }
    }
}