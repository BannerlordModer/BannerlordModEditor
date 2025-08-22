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
using Microsoft.Extensions.DependencyInjection;
using BannerlordModEditor.UI.Factories;

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

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        var editorFactory = serviceProvider.GetRequiredService<IEditorFactory>();
        EditorManager = new EditorManagerViewModel(editorFactory);
        AttributeEditor = serviceProvider.GetRequiredService<AttributeEditorViewModel>();
        BoneBodyTypeEditor = serviceProvider.GetRequiredService<BoneBodyTypeEditorViewModel>();
        SkillEditor = serviceProvider.GetRequiredService<SkillEditorViewModel>();
        
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

    private async void LoadSelectedEditor()
    {
        try
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
                    if (AttributeEditor != null)
                    {
                        await AttributeEditor.LoadXmlFileAsync(selectedEditor.XmlFileName);
                        ShowAttributeEditor = true;
                    }
                    else
                    {
                        ShowDefaultContent = true;
                    }
                    break;
                case "BoneBodyTypeEditor":
                    if (BoneBodyTypeEditor != null)
                    {
                        await BoneBodyTypeEditor.LoadXmlFileAsync(selectedEditor.XmlFileName);
                        ShowBoneBodyTypeEditor = true;
                    }
                    else
                    {
                        ShowDefaultContent = true;
                    }
                    break;
                case "SkillEditor":
                    if (SkillEditor != null)
                    {
                        await SkillEditor.LoadXmlFileAsync(selectedEditor.XmlFileName);
                        ShowSkillEditor = true;
                    }
                    else
                    {
                        ShowDefaultContent = true;
                    }
                    break;
                default:
                    ShowDefaultContent = true;
                    break;
            }
        }
        catch (Exception ex)
        {
            // 发生异常时显示默认内容
            ShowDefaultContent = true;
            ShowAttributeEditor = false;
            ShowBoneBodyTypeEditor = false;
            ShowSkillEditor = false;
            
            // 记录错误（如果有日志服务）
            System.Diagnostics.Debug.WriteLine($"Error in LoadSelectedEditor: {ex.Message}");
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
