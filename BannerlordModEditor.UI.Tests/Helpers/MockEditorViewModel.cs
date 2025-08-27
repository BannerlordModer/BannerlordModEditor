using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Services;
using System.Collections.ObjectModel;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试用的模拟编辑器ViewModel
    /// 
    /// 这个类为测试提供一个通用的编辑器ViewModel实现，
    /// 当实际的编辑器ViewModel无法创建时作为回退方案。
    /// </summary>
    public partial class MockEditorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string filePath = string.Empty;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private bool hasUnsavedChanges = false;

        [ObservableProperty]
        private ObservableCollection<object> items = new ObservableCollection<object>();

        [ObservableProperty]
        private object? selectedItem;

        [ObservableProperty]
        private string searchFilter = string.Empty;

        [ObservableProperty]
        private int totalCount = 0;

        [ObservableProperty]
        private int filteredCount = 0;

        public MockEditorViewModel(
            IErrorHandlerService? errorHandler = null,
            ILogService? logService = null)
            : base(errorHandler, logService)
        {
            // 初始化一些测试数据
            Items.Add(new { Id = 1, Name = "测试项1", DefaultValue = "默认值1" });
            Items.Add(new { Id = 2, Name = "测试项2", DefaultValue = "默认值2" });
            Items.Add(new { Id = 3, Name = "测试项3", DefaultValue = "默认值3" });
            
            TotalCount = Items.Count;
            FilteredCount = Items.Count;
            
            StatusMessage = "模拟编辑器已就绪";
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "正在加载测试数据...";
                
                // 模拟异步加载
                await Task.Delay(100);
                
                StatusMessage = "测试数据加载完成";
            }
            catch (Exception ex)
            {
                StatusMessage = $"加载失败: {ex.Message}";
                ErrorHandler?.HandleError(ex, "MockEditorViewModel.LoadDataAsync");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SaveDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "正在保存测试数据...";
                
                // 模拟异步保存
                await Task.Delay(100);
                
                HasUnsavedChanges = false;
                StatusMessage = "测试数据保存完成";
            }
            catch (Exception ex)
            {
                StatusMessage = $"保存失败: {ex.Message}";
                ErrorHandler?.HandleError(ex, "MockEditorViewModel.SaveDataAsync");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Refresh()
        {
            StatusMessage = "刷新测试数据...";
            Items.Clear();
            Items.Add(new { Id = 1, Name = "刷新项1", DefaultValue = "刷新默认值1" });
            Items.Add(new { Id = 2, Name = "刷新项2", DefaultValue = "刷新默认值2" });
            TotalCount = Items.Count;
            FilteredCount = Items.Count;
            StatusMessage = "刷新完成";
        }

        [RelayCommand]
        private void AddItem()
        {
            var newItem = new 
            { 
                Id = Items.Count + 1, 
                Name = $"新项目{Items.Count + 1}", 
                DefaultValue = "新默认值" 
            };
            Items.Add(newItem);
            TotalCount = Items.Count;
            FilteredCount = Items.Count;
            HasUnsavedChanges = true;
            StatusMessage = "已添加新项目";
        }

        [RelayCommand]
        private void RemoveItem(object item)
        {
            if (item != null && Items.Contains(item))
            {
                Items.Remove(item);
                TotalCount = Items.Count;
                FilteredCount = Items.Count;
                HasUnsavedChanges = true;
                StatusMessage = "已删除项目";
            }
        }

        [RelayCommand]
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchFilter))
            {
                FilteredCount = TotalCount;
                StatusMessage = "显示所有项目";
            }
            else
            {
                // 这里应该实现实际的过滤逻辑
                FilteredCount = TotalCount; // 简化处理
                StatusMessage = $"应用过滤器: {SearchFilter}";
            }
        }

        public bool ValidateData()
        {
            // 模拟数据验证
            return Items.Count > 0;
        }

        public async Task<bool> ValidateDataAsync()
        {
            // 模拟异步数据验证
            await Task.Delay(50);
            return Items.Count > 0;
        }

        public override string ToString()
        {
            return $"MockEditorViewModel: {FilePath ?? "未命名"}";
        }
    }
}