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

    public MainWindowViewModel()
    {
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
