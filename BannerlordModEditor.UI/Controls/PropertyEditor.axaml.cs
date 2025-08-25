using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BannerlordModEditor.UI.Controls;

public partial class PropertyEditor : UserControl
{
    public PropertyEditor()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}