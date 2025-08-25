using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BannerlordModEditor.UI.Views.Editors;

public partial class ItemEditorView : BaseEditorView
{
    public ItemEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}