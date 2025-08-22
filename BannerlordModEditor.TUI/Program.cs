using System;
using Terminal.Gui;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.TUI.Views;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.TUI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Application.Init();
            
            // 注意：Terminal.Gui 1.14.0 中 SetColors 方法可能不存在或签名不同

            // 初始化服务
            var fileDiscoveryService = new FileDiscoveryService();
            var conversionService = new FormatConversionService(fileDiscoveryService);

            // 创建主窗口
            var mainViewModel = new MainViewModel(conversionService);
            var mainWindow = new MainWindow(mainViewModel);
            
            Application.Top.Add(mainWindow);
            Application.Run();
            Application.Shutdown();
        }
    }
}