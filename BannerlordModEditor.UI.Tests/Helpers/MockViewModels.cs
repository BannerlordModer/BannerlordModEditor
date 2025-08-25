using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BannerlordModEditor.UI.ViewModels;

namespace BannerlordModEditor.UI.Tests.Helpers;

/// <summary>
/// 模拟的MainWindowViewModel用于测试
/// </summary>
public partial class MockMainWindowViewModel : ObservableObject
{
    private readonly MockEditorManager _editorManager;

    public MockEditorManager EditorManager => _editorManager;

    public MockMainWindowViewModel()
    {
        _editorManager = new MockEditorManager();
    }

    [RelayCommand]
    public void OpenSettings()
    {
        // 模拟打开设置
    }

    [RelayCommand]
    public void CheckForUpdates()
    {
        // 模拟检查更新
    }
}

/// <summary>
/// 模拟的EditorManager用于测试
/// </summary>
public partial class MockEditorManager : ObservableObject
{
    private string _searchText = string.Empty;
    private string _currentBreadcrumb = "首页";
    private ObservableObject? _currentEditorViewModel;

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            FilterCategories();
        }
    }

    public string CurrentBreadcrumb
    {
        get => _currentBreadcrumb;
        set
        {
            _currentBreadcrumb = value;
            OnPropertyChanged();
        }
    }

    public ObservableObject? CurrentEditorViewModel
    {
        get => _currentEditorViewModel;
        set
        {
            _currentEditorViewModel = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<MockEditorCategory> Categories { get; } = new();

    public MockEditorManager()
    {
        InitializeCategories();
    }

    private void InitializeCategories()
    {
        var characterCategory = new MockEditorCategory
        {
            Name = "角色",
            Icon = "👤",
            Description = "角色相关配置",
            IsExpanded = true
        };
        
        foreach (var editor in new[]
        {
            new MockEditorInfo
            {
                Name = "属性编辑器",
                Icon = "⚙️",
                XmlFileName = "attributes.xml",
                IsAvailable = true
            },
            new MockEditorInfo
            {
                Name = "技能编辑器",
                Icon = "🎯",
                XmlFileName = "skills.xml",
                IsAvailable = true
            }
        })
        {
            characterCategory.Editors.Add(editor);
        }
        Categories.Add(characterCategory);

        var itemCategory = new MockEditorCategory
        {
            Name = "物品",
            Icon = "🗡️",
            Description = "物品相关配置",
            IsExpanded = true
        };
        
        foreach (var editor in new[]
        {
            new MockEditorInfo
            {
                Name = "物品编辑器",
                Icon = "📦",
                XmlFileName = "items.xml",
                IsAvailable = true
            }
        })
        {
            itemCategory.Editors.Add(editor);
        }
        Categories.Add(itemCategory);
    }

    [RelayCommand]
    public void SelectEditor(MockEditorInfo editorInfo)
    {
        CurrentEditorViewModel = new MockBaseEditorViewModel();
        CurrentBreadcrumb = $"首页 > {editorInfo.Name}";
    }

    private void FilterCategories()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // 重置所有编辑器的可见性
            foreach (var category in Categories)
            {
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = true;
                }
            }
        }
        else
        {
            // 根据搜索文本过滤
            var searchLower = SearchText.ToLower();
            foreach (var category in Categories)
            {
                foreach (var editor in category.Editors)
                {
                    editor.IsAvailable = editor.Name.ToLower().Contains(searchLower) ||
                                       editor.XmlFileName.ToLower().Contains(searchLower);
                }
            }
        }
    }
}

/// <summary>
/// 模拟的编辑器分类
/// </summary>
public class MockEditorCategory : ObservableObject
{
    private string _name = string.Empty;
    private string _icon = string.Empty;
    private string _description = string.Empty;
    private bool _isExpanded;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }

    public ObservableCollection<MockEditorInfo> Editors { get; } = new();
}

/// <summary>
/// 模拟的编辑器信息
/// </summary>
public class MockEditorInfo : ObservableObject
{
    private string _name = string.Empty;
    private string _icon = string.Empty;
    private string _xmlFileName = string.Empty;
    private bool _isAvailable;
    private string _editorType = string.Empty;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    public string XmlFileName
    {
        get => _xmlFileName;
        set => SetProperty(ref _xmlFileName, value);
    }

    public bool IsAvailable
    {
        get => _isAvailable;
        set => SetProperty(ref _isAvailable, value);
    }

    public string EditorType
    {
        get => _editorType;
        set => SetProperty(ref _editorType, value);
    }
}

/// <summary>
/// 模拟的基础编辑器视图模型
/// </summary>
public partial class MockBaseEditorViewModel : ViewModelBase
{
    private string _filePath = string.Empty;
    private string _searchFilter = string.Empty;
    private string _statusMessage = "就绪";
    private bool _isLoading;
    private bool _hasUnsavedChanges;

    public string FilePath
    {
        get => _filePath;
        set => SetProperty(ref _filePath, value);
    }

    public string SearchFilter
    {
        get => _searchFilter;
        set => SetProperty(ref _searchFilter, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        set => SetProperty(ref _hasUnsavedChanges, value);
    }

    [RelayCommand]
    public async Task LoadFileAsync()
    {
        IsLoading = true;
        StatusMessage = "正在加载文件...";
        await Task.Delay(100); // 模拟异步操作
        FilePath = "test_attributes.xml";
        StatusMessage = "文件加载完成";
        IsLoading = false;
    }

    [RelayCommand]
    public async Task SaveFileAsync()
    {
        IsLoading = true;
        StatusMessage = "正在保存文件...";
        await Task.Delay(100); // 模拟异步操作
        HasUnsavedChanges = false;
        StatusMessage = "文件保存完成";
        IsLoading = false;
    }

    [RelayCommand]
    public void AddItem()
    {
        HasUnsavedChanges = true;
        StatusMessage = "已添加新项目";
    }
}