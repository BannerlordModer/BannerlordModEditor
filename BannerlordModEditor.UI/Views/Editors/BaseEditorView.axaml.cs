using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BannerlordModEditor.UI.Views.Editors;

/// <summary>
/// 通用编辑器视图基类
/// </summary>
public partial class BaseEditorView : UserControl
{
    protected BaseEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}