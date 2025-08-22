using CommunityToolkit.Mvvm.ComponentModel;

namespace BannerlordModEditor.UI.ViewModels.Editors;

/// <summary>
/// 通用编辑器接口
/// </summary>
public interface IBaseEditorViewModel
{
    /// <summary>
    /// XML文件名
    /// </summary>
    string XmlFileName { get; }
    
    /// <summary>
    /// 编辑器名称
    /// </summary>
    string EditorName { get; }
    
    /// <summary>
    /// 是否有未保存的更改
    /// </summary>
    bool HasUnsavedChanges { get; set; }
    
    /// <summary>
    /// 文件路径
    /// </summary>
    string FilePath { get; set; }
    
    /// <summary>
    /// 设置XML文件名
    /// </summary>
    void SetXmlFileName(string xmlFileName);
    
    /// <summary>
    /// 加载XML文件
    /// </summary>
    void LoadXmlFile(string fileName);
    
    /// <summary>
    /// 保存XML文件
    /// </summary>
    void SaveXmlFile();
    
    /// <summary>
    /// 刷新编辑器状态
    /// </summary>
    void Refresh();
}

/// <summary>
/// 基础编辑器视图模型基类
/// </summary>
public abstract partial class BaseEditorViewModel : ViewModelBase, IBaseEditorViewModel
{
    [ObservableProperty]
    private string xmlFileName = string.Empty;
    
    [ObservableProperty]
    private string editorName = string.Empty;
    
    [ObservableProperty]
    private bool hasUnsavedChanges;
    
    [ObservableProperty]
    private string filePath = string.Empty;
    
    [ObservableProperty]
    private string statusMessage = "就绪";

    protected BaseEditorViewModel(string xmlFileName, string editorName)
    {
        XmlFileName = xmlFileName;
        EditorName = editorName;
    }

    /// <summary>
    /// 设置XML文件名
    /// </summary>
    public void SetXmlFileName(string xmlFileName)
    {
        XmlFileName = xmlFileName;
    }

    /// <summary>
    /// 加载XML文件
    /// </summary>
    public abstract void LoadXmlFile(string fileName);
    
    /// <summary>
    /// 保存XML文件
    /// </summary>
    public abstract void SaveXmlFile();
    
    /// <summary>
    /// 刷新编辑器状态
    /// </summary>
    public virtual void Refresh()
    {
        // 默认实现为空，子类可以重写
    }
}