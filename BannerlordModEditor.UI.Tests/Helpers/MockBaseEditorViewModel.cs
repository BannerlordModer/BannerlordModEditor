using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using BannerlordModEditor.UI.ViewModels;
using BannerlordModEditor.UI.Services;

namespace BannerlordModEditor.UI.Tests.Helpers
{
    /// <summary>
    /// 测试用的模拟基础编辑器ViewModel
    /// </summary>
    public partial class MockBaseEditorViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool hasChanges;

        [ObservableProperty]
        private string? lastError;

        
        public MockBaseEditorViewModel(
            IErrorHandlerService? errorHandler = null,
            ILogService? logService = null)
            : base(errorHandler, logService)
        {
            FilePath = string.Empty;
            StatusMessage = "模拟编辑器";
            IsLoading = false;
            HasChanges = false;
            LastError = null;
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "正在加载数据...";
                LastError = null;

                // 模拟异步加载操作
                await Task.Delay(100);

                StatusMessage = "数据加载完成";
            }
            catch (System.Exception ex)
            {
                LastError = ex.Message;
                StatusMessage = "数据加载失败";
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
                StatusMessage = "正在保存数据...";
                LastError = null;

                // 模拟异步保存操作
                await Task.Delay(100);

                HasChanges = false;
                StatusMessage = "数据保存完成";
            }
            catch (System.Exception ex)
            {
                LastError = ex.Message;
                StatusMessage = "数据保存失败";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void RefreshData()
        {
            StatusMessage = "数据已刷新";
            LastError = null;
        }

        public override string ToString()
        {
            return $"MockBaseEditorViewModel: {FilePath ?? "未命名"}";
        }
    }
}