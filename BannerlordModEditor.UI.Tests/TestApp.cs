using Avalonia;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace BannerlordModEditor.UI.Tests;

public class TestApp : Application
{
    public override void Initialize()
    {
        // 手动添加Fluent主题
        Styles.Add(new FluentTheme());
    }
} 