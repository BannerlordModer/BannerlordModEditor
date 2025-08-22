using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI;
using BannerlordModEditor.UI.Tests.Helpers;

namespace BannerlordModEditor.UI.Tests;

public class TestApp : Application
{
    public override void Initialize()
    {
        // 手动添加Fluent主题
        Styles.Add(new FluentTheme());
        
        // 手动添加应用程序资源
        Resources.Add("BoolToStringConverter", new BoolToStringConverter());
        Resources.Add("EditorContentConverter", new EditorContentConverter());
        Resources.Add("ViewLocator", new ViewLocator(TestServiceProvider.GetServiceProvider()));
    }
} 