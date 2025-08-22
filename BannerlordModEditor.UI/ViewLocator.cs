using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace BannerlordModEditor.UI;

public class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider? _serviceProvider;

    public ViewLocator()
    {
        // 如果需要服务提供器，可以通过构造函数注入
    }

    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        
        // 首先尝试通过命名约定找到视图
        var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        // 如果找不到，尝试通过编辑器工厂创建视图
        if (_serviceProvider != null)
        {
            var editorFactory = _serviceProvider.GetService<IEditorFactory>();
            if (editorFactory != null)
            {
                var editorType = param.GetType().Name.Replace("ViewModel", "", StringComparison.Ordinal);
                var view = editorFactory.CreateEditorView(editorType);
                if (view != null)
                {
                    return view;
                }
            }
        }
        
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
