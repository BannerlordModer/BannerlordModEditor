using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.UI.ViewModels;

/// <summary>
/// XML文件发现和管理视图模型
/// </summary>
public partial class XmlFileDiscoveryViewModel : ViewModelBase
{
    private readonly IFileDiscoveryService _fileDiscoveryService;

    [ObservableProperty]
    private ObservableCollection<XmlFileEntry> availableFiles = new();

    [ObservableProperty]
    private ObservableCollection<XmlFileEntry> adaptedFiles = new();

    [ObservableProperty]
    private ObservableCollection<XmlFileEntry> unadaptedFiles = new();

    [ObservableProperty]
    private string searchFilter = string.Empty;

    [ObservableProperty]
    private string statusMessage = "正在扫描XML文件...";

    [ObservableProperty]
    private bool isScanning;

    [ObservableProperty]
    private int totalFilesCount;

    [ObservableProperty]
    private int adaptedFilesCount;

    [ObservableProperty]
    private int unadaptedFilesCount;

    public XmlFileDiscoveryViewModel(IFileDiscoveryService fileDiscoveryService)
    {
        _fileDiscoveryService = fileDiscoveryService;
        
        // 初始化集合
        AvailableFiles = new ObservableCollection<XmlFileEntry>();
        AdaptedFiles = new ObservableCollection<XmlFileEntry>();
        UnadaptedFiles = new ObservableCollection<XmlFileEntry>();
        
        // 监听集合变化
        AvailableFiles.CollectionChanged += (s, e) => UpdateCounts();
        AdaptedFiles.CollectionChanged += (s, e) => UpdateCounts();
        UnadaptedFiles.CollectionChanged += (s, e) => UpdateCounts();
    }

    /// <summary>
    /// 扫描XML文件
    /// </summary>
    [RelayCommand]
    private async Task ScanFilesAsync()
    {
        try
        {
            IsScanning = true;
            StatusMessage = "正在扫描XML文件...";

            await Task.Run(() =>
            {
                var scanPaths = new[]
                {
                    "TestData",
                    "BannerlordModEditor.Common.Tests/TestData",
                    "example/ModuleData"
                };

                AvailableFiles.Clear();
                AdaptedFiles.Clear();
                UnadaptedFiles.Clear();

                foreach (var path in scanPaths)
                {
                    if (Directory.Exists(path))
                    {
                        var xmlFiles = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
                        foreach (var file in xmlFiles)
                        {
                            var entry = new XmlFileEntry
                            {
                                FileName = Path.GetFileName(file),
                                FullPath = file,
                                RelativePath = GetRelativePath(file),
                                FileSize = new FileInfo(file).Length,
                                LastModified = File.GetLastWriteTime(file)
                            };

                            // 检查是否已适配
                            entry.IsAdapted = _fileDiscoveryService.IsFileAdapted(entry.FileName);
                            entry.Status = entry.IsAdapted ? "已适配" : "未适配";

                            AvailableFiles.Add(entry);

                            if (entry.IsAdapted)
                            {
                                AdaptedFiles.Add(entry);
                            }
                            else
                            {
                                UnadaptedFiles.Add(entry);
                            }
                        }
                    }
                }
            });

            StatusMessage = $"扫描完成，共发现 {AvailableFiles.Count} 个XML文件";
        }
        catch (Exception ex)
        {
            StatusMessage = $"扫描失败: {ex.Message}";
        }
        finally
        {
            IsScanning = false;
        }
    }

    /// <summary>
    /// 打开文件
    /// </summary>
    [RelayCommand]
    private void OpenFile(XmlFileEntry entry)
    {
        if (entry != null)
        {
            // TODO: 实现文件打开逻辑
            StatusMessage = $"打开文件: {entry.FileName}";
        }
    }

    /// <summary>
    /// 适配文件
    /// </summary>
    [RelayCommand]
    private async Task AdaptFileAsync(XmlFileEntry entry)
    {
        if (entry != null && !entry.IsAdapted)
        {
            try
            {
                StatusMessage = $"正在适配 {entry.FileName}...";
                
                // TODO: 实现文件适配逻辑
                await Task.Delay(1000); // 模拟适配过程
                
                entry.IsAdapted = true;
                entry.Status = "已适配";
                
                // 移动到已适配列表
                UnadaptedFiles.Remove(entry);
                AdaptedFiles.Add(entry);
                
                StatusMessage = $"适配完成: {entry.FileName}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"适配失败: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// 刷新文件列表
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await ScanFilesAsync();
    }

    /// <summary>
    /// 搜索过滤
    /// </summary>
    partial void OnSearchFilterChanged(string value)
    {
        UpdateFilteredFiles();
    }

    /// <summary>
    /// 更新过滤后的文件
    /// </summary>
    private void UpdateFilteredFiles()
    {
        if (string.IsNullOrWhiteSpace(SearchFilter))
        {
            // 显示所有文件
            foreach (var file in AvailableFiles)
            {
                file.IsVisible = true;
            }
        }
        else
        {
            var searchLower = SearchFilter.ToLower();
            foreach (var file in AvailableFiles)
            {
                file.IsVisible = file.FileName.ToLower().Contains(searchLower) ||
                                file.RelativePath.ToLower().Contains(searchLower);
            }
        }
    }

    /// <summary>
    /// 更新计数
    /// </summary>
    private void UpdateCounts()
    {
        TotalFilesCount = AvailableFiles.Count;
        AdaptedFilesCount = AdaptedFiles.Count;
        UnadaptedFilesCount = UnadaptedFiles.Count;
    }

    /// <summary>
    /// 获取相对路径
    /// </summary>
    private string GetRelativePath(string fullPath)
    {
        var basePath = Directory.GetCurrentDirectory();
        return fullPath.Replace(basePath + Path.DirectorySeparatorChar, "");
    }
}

/// <summary>
/// XML文件条目
/// </summary>
public partial class XmlFileEntry : ObservableObject
{
    [ObservableProperty]
    private string fileName = string.Empty;

    [ObservableProperty]
    private string fullPath = string.Empty;

    [ObservableProperty]
    private string relativePath = string.Empty;

    [ObservableProperty]
    private long fileSize;

    [ObservableProperty]
    private DateTime lastModified;

    [ObservableProperty]
    private bool isAdapted;

    [ObservableProperty]
    private string status = "未知";

    [ObservableProperty]
    private bool isVisible = true;
}