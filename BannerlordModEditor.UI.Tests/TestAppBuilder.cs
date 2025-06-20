using Avalonia;
using Avalonia.Headless;

[assembly: AvaloniaTestApplication(typeof(BannerlordModEditor.UI.Tests.TestAppBuilder))]

namespace BannerlordModEditor.UI.Tests;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder
        .Configure<TestApp>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
} 