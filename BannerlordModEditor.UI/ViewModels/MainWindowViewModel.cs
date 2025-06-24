using System.Collections.ObjectModel;
// using BannerlordModEditor.Common.Services;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.UI.ViewModels.Editors;
using System;
using System.Windows.Input;
using Avalonia.Controls;
using BannerlordModEditor.Common.Models.Configuration;
using CommunityToolkit.Mvvm.Input;
using Velopack;
using Velopack.Sources;

namespace BannerlordModEditor.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private EditorManagerViewModel editorManager;

    // 保留兼容性，但不再在界面中直接使用
    [ObservableProperty]
    private AttributeEditorViewModel attributeEditor;

    [ObservableProperty]
    private BoneBodyTypeEditorViewModel boneBodyTypeEditor;

    [ObservableProperty]
    private SkillEditorViewModel skillEditor;

    // 保留原有通用编辑器相关属性（暂时）
    public ObservableCollection<FileEntryViewModel> FileEntries { get; } = new();

    [ObservableProperty]
    private FileEntryViewModel? selectedEntry;

    [ObservableProperty]
    private bool showDefaultContent = true;

    [ObservableProperty]
    private bool showAttributeEditor = false;

    [ObservableProperty]
    private bool showBoneBodyTypeEditor = false;

    [ObservableProperty]
    private bool showSkillEditor = false;

    public ICommand OpenSettingsCommand { get; }
    public ICommand CheckForUpdatesCommand { get; }

    public MainWindowViewModel()
    {
        EditorManager = new EditorManagerViewModel();
        AttributeEditor = new AttributeEditorViewModel();
        BoneBodyTypeEditor = new BoneBodyTypeEditorViewModel();
        SkillEditor = new SkillEditorViewModel();
        
        // 订阅编辑器选择事件
        EditorManager.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(EditorManager.SelectedEditor))
            {
                LoadSelectedEditor();
            }
        };
        
        // 初始化示例数据（可删除）
        InitializeSampleData();

        OpenSettingsCommand = new RelayCommand(OpenSettings);
        CheckForUpdatesCommand = new AsyncRelayCommand(CheckForUpdatesAsync);
    }

    private void LoadSelectedEditor()
    {
        var selectedEditor = EditorManager.SelectedEditor;
        
        // 隐藏所有编辑器
        ShowDefaultContent = false;
        ShowAttributeEditor = false;
        ShowBoneBodyTypeEditor = false;
        ShowSkillEditor = false;
        
        if (selectedEditor == null) 
        {
            ShowDefaultContent = true;
            return;
        }

        switch (selectedEditor.EditorType)
        {
            case "AttributeEditor":
                AttributeEditor.LoadXmlFile(selectedEditor.XmlFileName);
                ShowAttributeEditor = true;
                break;
            case "BoneBodyTypeEditor":
                BoneBodyTypeEditor.LoadXmlFile(selectedEditor.XmlFileName);
                ShowBoneBodyTypeEditor = true;
                break;
            case "SkillEditor":
                SkillEditor.LoadXmlFile(selectedEditor.XmlFileName);
                ShowSkillEditor = true;
                break;
            default:
                ShowDefaultContent = true;
                break;
        }
    }

    private void InitializeSampleData()
    {
        // 这些数据现在主要用于向后兼容，实际界面使用EditorManager
        FileEntries.Add(new FileEntryViewModel { DisplayName = "Sample Entry 1" });
        FileEntries.Add(new FileEntryViewModel { DisplayName = "Sample Entry 2" });
    }

    private void OpenSettings()
    {
        // TODO: Implement settings logic
    }

    private async Task CheckForUpdatesAsync()
    {
        try
        {
            var mgr = new UpdateManager(new GithubSource("https://github.com/BannerlordModer/BannerlordModEditor", null, false));

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                // TODO: Show a message to the user that they are on the latest version.
                return; // no update available
            }

            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
        catch (Exception e)
        {
            // TODO: Log the exception or show a message to the user.
        }
    }
}

// 保留用于向后兼容
public class FileEntryViewModel : ViewModelBase
{
    public string DisplayName { get; set; } = string.Empty;
    public ObservableCollection<AttributeViewModel> Attributes { get; } = new();
    public ObservableCollection<ChildElementViewModel> Children { get; } = new();
}

public class AttributeViewModel : ViewModelBase
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class ChildElementViewModel : ViewModelBase
{
    public string ElementName { get; set; } = string.Empty;
    public ObservableCollection<AttributeViewModel> Attributes { get; } = new();
}
