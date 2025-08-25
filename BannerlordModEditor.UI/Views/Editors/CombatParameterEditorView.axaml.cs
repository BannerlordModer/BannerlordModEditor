using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BannerlordModEditor.UI.Views.Editors;

public partial class CombatParameterEditorView : BaseEditorView
{
    public CombatParameterEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}