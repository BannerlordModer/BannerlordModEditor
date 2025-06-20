using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Headless;

namespace BannerlordModEditor.UI.Tests;

public class BasicUITests
{
    [AvaloniaFact]
    public void Should_Type_Text_Into_TextBox()
    {
        // 设置控件：
        var textBox = new TextBox();
        var window = new Window { Content = textBox };

        // 打开窗口：
        window.Show();

        // 焦点在文本框上：
        textBox.Focus();

        // 模拟文本输入：
        window.KeyTextInput("Hello World");

        // 断言：
        Assert.Equal("Hello World", textBox.Text);
    }

    [AvaloniaFact]
    public void Should_Create_Window()
    {
        var window = new Window
        {
            Title = "Test Window",
            Width = 400,
            Height = 300
        };

        window.Show();

        Assert.Equal("Test Window", window.Title);
        Assert.Equal(400, window.Width);
        Assert.Equal(300, window.Height);
    }

    [AvaloniaFact]
    public void Should_Handle_Button_Click()
    {
        var clicked = false;
        var button = new Button { Content = "Click Me" };
        button.Click += (sender, e) => clicked = true;

        var window = new Window { Content = button };
        window.Show();

        // 模拟按钮点击
        button.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Button.ClickEvent));

        Assert.True(clicked);
    }

    [AvaloniaFact]
    public void Should_Set_TextBlock_Content()
    {
        var textBlock = new TextBlock { Text = "Hello Avalonia" };
        var window = new Window { Content = textBlock };
        
        window.Show();

        Assert.Equal("Hello Avalonia", textBlock.Text);
    }
} 