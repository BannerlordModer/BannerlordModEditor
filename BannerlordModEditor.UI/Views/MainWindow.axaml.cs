using Avalonia.Controls;
using Avalonia.Platform;
using System;

namespace BannerlordModEditor.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var screen = Screens.Primary;
        if (screen is not null)
        {
            var scaling = screen.Scaling;
            var workingArea = screen.WorkingArea;
            
            // Set window size to a fraction of the working area, considering scaling
            this.Width = workingArea.Width / scaling * 0.8;
            this.Height = workingArea.Height / scaling * 0.9;
        }
    }
}