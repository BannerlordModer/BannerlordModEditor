using Avalonia.Controls;
using Avalonia;
using System.Collections.Generic;
using System.Linq;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 测试辅助扩展方法
/// </summary>
public static class TestExtensions
{
    /// <summary>
    /// 查找指定类型的所有子控件
    /// </summary>
    public static IEnumerable<T> FindVisuals<T>(this Control control) where T : Control
    {
        if (control == null)
            yield break;

        if (control is T result)
            yield return result;

        if (control is ContentControl contentControl && contentControl.Content is T contentResult)
            yield return contentResult;

        if (control is ItemsControl itemsControl)
        {
            for (int i = 0; i < itemsControl.ItemCount; i++)
            {
                var item = itemsControl.Items[i];
                if (item is T itemResult)
                    yield return itemResult;
            }
        }

        // 简化实现，只检查逻辑子元素
        foreach (var child in GetLogicalChildren(control))
        {
            if (child is Control childControl)
            {
                foreach (var descendant in FindVisuals<T>(childControl))
                {
                    yield return descendant;
                }
            }
        }
    }

    /// <summary>
    /// 查找符合指定条件的子控件
    /// </summary>
    public static T? Find<T>(this Control control, System.Func<T, bool> condition) where T : Control
    {
        return control.FindVisuals<T>().FirstOrDefault(condition);
    }

    /// <summary>
    /// 获取控件的逻辑子元素
    /// </summary>
    private static IEnumerable<Control> GetLogicalChildren(Control control)
    {
        if (control == null)
            yield break;

        // 使用LogicalChildren来获取子控件
        if (control is ContentControl contentControl && contentControl.Content is Control contentChild)
        {
            yield return contentChild;
        }

        if (control is ItemsControl itemsControl)
        {
            foreach (var item in itemsControl.Items)
            {
                if (item is Control itemControl)
                {
                    yield return itemControl;
                }
            }
        }
    }
}