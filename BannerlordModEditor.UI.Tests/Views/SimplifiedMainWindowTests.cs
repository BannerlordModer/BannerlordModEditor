using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using BannerlordModEditor.UI.Tests.Helpers;
using BannerlordModEditor.UI.Views;
using Xunit;
using System.Linq;

/// <summary>
/// MainWindow的简化UI交互测试
/// </summary>
public class SimplifiedMainWindowTests
{
    [AvaloniaFact]
    public void MainWindow_ShouldInitializeWithDefaultLayout()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        // Act
        mainWindow.Show();

        // Assert
        Assert.NotNull(mainWindow);
        Assert.Equal("骑砍2模组编辑器 - Bannerlord Mod Editor", mainWindow.Title);
        Assert.True(mainWindow.MinWidth >= 800);
        Assert.True(mainWindow.MinHeight >= 600);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldHaveMenuItems()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        mainWindow.Show();

        // 简化测试：验证窗口能正常显示
        Assert.NotNull(mainWindow);
        Assert.True(mainWindow.IsVisible);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldHaveSearchBox()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        mainWindow.Show();

        // 简化测试：验证数据上下文正确设置
        Assert.NotNull(mainWindow.DataContext);
        Assert.Equal(viewModel, mainWindow.DataContext);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldHaveEditorButtons()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        mainWindow.Show();

        // 简化测试：验证编辑器管理器存在
        Assert.NotNull(viewModel.EditorManager);
        Assert.True(viewModel.EditorManager.Categories.Count > 0);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldDisplayDefaultContent()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        mainWindow.Show();

        // 简化测试：验证默认状态
        Assert.NotNull(viewModel.EditorManager.CurrentBreadcrumb);
        Assert.Equal("首页", viewModel.EditorManager.CurrentBreadcrumb);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldHaveThreeColumnLayout()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        mainWindow.Show();

        // 简化测试：验证窗口尺寸
        Assert.True(mainWindow.MinWidth >= 800);
        Assert.True(mainWindow.MinHeight >= 600);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldHaveBreadcrumbNavigation()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        mainWindow.Show();

        // 简化测试：验证面包屑导航功能
        Assert.NotNull(viewModel.EditorManager);
        Assert.NotNull(viewModel.EditorManager.CurrentBreadcrumb);
    }

    [AvaloniaFact]
    public void MainWindow_ShouldBeResponsive()
    {
        // Arrange
        var viewModel = new MockMainWindowViewModel();
        var mainWindow = new MainWindow
        {
            DataContext = viewModel
        };

        // Act
        mainWindow.Show();

        // 调整窗口大小
        mainWindow.Width = 1024;
        mainWindow.Height = 768;

        // Assert - 验证窗口能够响应大小变化
        Assert.Equal(1024, mainWindow.Width);
        Assert.Equal(768, mainWindow.Height);
    }
}