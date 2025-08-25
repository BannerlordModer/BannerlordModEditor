using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BannerlordModEditor.UI.Controls;

public partial class ValidationErrorDisplay : UserControl
{
    public ValidationErrorDisplay()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}