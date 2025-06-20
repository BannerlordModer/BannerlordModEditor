using System.Collections.ObjectModel;
// using BannerlordModEditor.Common.Services;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public ObservableCollection<object> Items { get; } = new();

    [ObservableProperty]
    private ObservableCollection<EntryViewModel> fileEntries = new();

    [ObservableProperty]
    private EntryViewModel? selectedEntry;

    public MainWindowViewModel()
    {
        // Initialize with some sample data for now
        FileEntries.Add(new EntryViewModel("Sample Entry 1"));
        FileEntries.Add(new EntryViewModel("Sample Entry 2"));
        
        // var service = new XmlFileService();
        // var doc = service.LoadXml("C:/Workspace/CSharp/BannerlordModEditor/example/ModuleData/mpitems.xml");
        // if (doc?.Root != null)
        // {
        //     foreach (var item in doc.Root.Elements("Item"))
        //     {
        //         Items.Add(new EntryViewModel(item));
        //     }
        // }
    }
}

// Basic EntryViewModel class to support the UI
public partial class EntryViewModel : ViewModelBase
{
    [ObservableProperty]
    private string displayName = string.Empty;

    [ObservableProperty]
    private ObservableCollection<AttributeViewModel> attributes = new();

    [ObservableProperty]
    private ObservableCollection<EntryViewModel> children = new();

    [ObservableProperty]
    private string elementName = string.Empty;

    public EntryViewModel(string displayName)
    {
        DisplayName = displayName;
        ElementName = displayName;
        
        // Add some sample attributes for demonstration
        Attributes.Add(new AttributeViewModel("Name", displayName));
        Attributes.Add(new AttributeViewModel("Type", "Sample"));
    }

    public EntryViewModel(XElement element)
    {
        DisplayName = element.Attribute("id")?.Value ?? element.Name.LocalName;
        ElementName = element.Name.LocalName;
        
        // Load attributes from XML
        foreach (var attr in element.Attributes())
        {
            Attributes.Add(new AttributeViewModel(attr.Name.LocalName, attr.Value));
        }
        
        // Load child elements
        foreach (var child in element.Elements())
        {
            Children.Add(new EntryViewModel(child));
        }
    }
}

// AttributeViewModel to support attribute editing
public partial class AttributeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string value = string.Empty;

    public AttributeViewModel(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
