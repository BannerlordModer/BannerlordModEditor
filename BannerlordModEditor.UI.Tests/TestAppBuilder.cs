using Avalonia;
using Avalonia.Headless;
using Avalonia.Markup.Xaml;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

[assembly: AvaloniaTestApplication(typeof(BannerlordModEditor.UI.Tests.TestAppBuilder))]

namespace BannerlordModEditor.UI.Tests;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder
        .Configure<TestApp>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions())
        .WithInterFont()
        .LogToTrace();
} 