using Avalonia;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using BannerlordModEditor.UI.ViewModels;

namespace BannerlordModEditor.UI.Tests;

public class TestApp : Application
{
    public override void Initialize()
    {
        // 手动添加Fluent主题
        Styles.Add(new FluentTheme());
        
        // 添加应用程序资源 - 包括测试需要的转换器
        Resources.Add("BoolToStringConverter", new BoolToStringConverter());
    }
} 