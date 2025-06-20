using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BannerlordModEditor.UI.ViewModels;

public partial class EditorCategoryViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string icon = string.Empty;

    [ObservableProperty]
    private bool isExpanded = true;

    public ObservableCollection<EditorItemViewModel> Editors { get; } = new();

    public EditorCategoryViewModel(string name, string description, string icon = "📁")
    {
        Name = name;
        Description = description;
        Icon = icon;
    }
}

public partial class EditorItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string xmlFileName = string.Empty;

    [ObservableProperty]
    private string editorType = string.Empty;

    [ObservableProperty]
    private string icon = string.Empty;

    [ObservableProperty]
    private bool isAvailable = true;

    public EditorItemViewModel(string name, string description, string xmlFileName, string editorType, string icon = "📄")
    {
        Name = name;
        Description = description;
        XmlFileName = xmlFileName;
        EditorType = editorType;
        Icon = icon;
    }
} 