using System;
using Terminal.Gui;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.TUI.Views;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.TUI.Models;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.TUI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // 检查是否在测试模式或命令行模式下运行
            if (args.Length > 0)
            {
                HandleCommandLineArgs(args).Wait();
                return;
            }

            try
            {
                Application.Init();
                
                // 注意：Terminal.Gui 1.14.0 中 SetColors 方法可能不存在或签名不同

                // 初始化服务
                var fileDiscoveryService = new FileDiscoveryService();
                var xmlTypeDetectionService = new XmlTypeDetectionService(fileDiscoveryService);
                
                // 创建转换服务
                var conversionService = new FormatConversionService(fileDiscoveryService, xmlTypeDetectionService);
                
                // 创建类型化转换服务（可选功能）
                var typedXmlConversionService = new TypedXmlConversionService(fileDiscoveryService, conversionService);

                // 创建主窗口
                var mainViewModel = new MainViewModel(conversionService);
                var mainWindow = new MainWindow(mainViewModel);
                
                Application.Top.Add(mainWindow);
                Application.Run();
                Application.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TUI应用程序启动失败: {ex.Message}");
                Console.WriteLine("这可能是因为在非交互式终端环境中运行。");
                Console.WriteLine("请确保在支持终端交互的环境中运行此应用程序。");
                Environment.Exit(1);
            }
        }

        private static async Task HandleCommandLineArgs(string[] args)
        {
            var command = args[0].ToLower();
            
            switch (command)
            {
                case "--version":
                case "-v":
                    Console.WriteLine("Bannerlord Mod Editor TUI v1.0.0");
                    break;
                    
                case "--help":
                case "-h":
                    ShowHelp();
                    break;
                    
                case "--test":
                    Console.WriteLine("TUI应用程序测试模式 - 应用程序可以正常启动");
                    break;
                    
                case "--convert":
                    if (args.Length < 3)
                    {
                        Console.WriteLine("错误: 需要指定输入文件和输出文件路径");
                        Console.WriteLine("用法: --convert <输入文件> <输出文件>");
                        Environment.Exit(1);
                    }
                    
                    var inputFile = args[1];
                    var outputFile = args[2];
                    
                    try
                    {
                        // 初始化服务
                        var fileDiscoveryService = new FileDiscoveryService();
                        var xmlTypeDetectionService = new XmlTypeDetectionService(fileDiscoveryService);
                        var conversionService = new FormatConversionService(fileDiscoveryService, xmlTypeDetectionService);
                        
                        // 执行转换
                        var result = await conversionService.ExcelToXmlAsync(inputFile, outputFile);
                        
                        if (result.Success)
                        {
                            Console.WriteLine($"成功转换文件: {inputFile} -> {outputFile}");
                            Console.WriteLine($"处理记录数: {result.RecordsProcessed}");
                            Console.WriteLine($"耗时: {result.Duration.TotalMilliseconds:F2}ms");
                            
                            if (result.Warnings.Count > 0)
                            {
                                Console.WriteLine("警告:");
                                foreach (var warning in result.Warnings)
                                {
                                    Console.WriteLine($"  - {warning}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"转换失败: {inputFile}");
                            Console.WriteLine($"错误: {result.Message}");
                            
                            if (result.Errors.Count > 0)
                            {
                                Console.WriteLine("详细错误:");
                                foreach (var error in result.Errors)
                                {
                                    Console.WriteLine($"  - {error}");
                                }
                            }
                            Environment.Exit(1);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"转换过程中发生错误: {ex.Message}");
                        Environment.Exit(1);
                    }
                    break;
                    
                default:
                    Console.WriteLine($"未知命令: {command}");
                    ShowHelp();
                    Environment.Exit(1);
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Bannerlord Mod Editor TUI - 命令行界面");
            Console.WriteLine();
            Console.WriteLine("用法:");
            Console.WriteLine("  BannerlordModEditor.TUI                    # 启动交互式TUI界面");
            Console.WriteLine("  BannerlordModEditor.TUI --help           # 显示帮助信息");
            Console.WriteLine("  BannerlordModEditor.TUI --version        # 显示版本信息");
            Console.WriteLine("  BannerlordModEditor.TUI --test           # 测试模式");
            Console.WriteLine("  BannerlordModEditor.TUI --convert <输入> <输出> # 命令行转换");
            Console.WriteLine();
            Console.WriteLine("说明:");
            Console.WriteLine("  此应用程序需要支持终端交互的环境才能正常运行。");
            Console.WriteLine("  在非交互式环境中，请使用命令行参数进行操作。");
        }
    }
}