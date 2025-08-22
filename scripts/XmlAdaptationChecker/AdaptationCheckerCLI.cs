using System.IO;
using Microsoft.Extensions.Configuration;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.XmlAdaptationChecker.Configuration;
using BannerlordModEditor.XmlAdaptationChecker.Core;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;
using BannerlordModEditor.XmlAdaptationChecker.Services;
using BannerlordModEditor.XmlAdaptationChecker.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace BannerlordModEditor.XmlAdaptationChecker
{
    /// <summary>
    /// XML适配检查器命令行界面
    /// </summary>
    public class AdaptationCheckerCLI
    {
        private readonly IServiceProvider _serviceProvider;

        public AdaptationCheckerCLI(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// 运行命令行界面
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns>退出代码</returns>
        public static async Task<int> RunAsync(string[] args)
        {
            try
            {
                var serviceProvider = BuildServiceProvider();
                var cli = new AdaptationCheckerCLI(serviceProvider);

                return await cli.RunInternalAsync(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]启动失败: {ex.Message}[/]");
                return 1;
            }
        }

        /// <summary>
        /// 构建服务提供者
        /// </summary>
        /// <returns>服务提供者</returns>
        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            // 添加日志服务
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            // 添加配置服务
            services.AddSingleton<IConfiguration>(sp =>
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                configBuilder.AddJsonFile("appsettings.json", optional: true);
                configBuilder.AddEnvironmentVariables();
                return configBuilder.Build();
            });

            // 添加核心服务
            services.AddSingleton<AdaptationConfigurationManager>();
            services.AddSingleton<FileDiscoveryService>();
            services.AddSingleton<IFileDiscoveryService>(sp => sp.GetRequiredService<FileDiscoveryService>());
            services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();
            services.AddSingleton<IOutputFormatService, OutputFormatService>();

            // 添加配置
            services.AddSingleton(sp =>
            {
                var configManager = sp.GetRequiredService<AdaptationConfigurationManager>();
                return configManager.LoadConfiguration();
            });

            services.AddSingleton<Core.XmlAdaptationChecker>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// 内部运行方法
        /// </summary>
        /// <param name="args">命令行参数</param>
        /// <returns>退出代码</returns>
        private async Task<int> RunInternalAsync(string[] args)
        {
            if (args.Length == 0)
            {
                return await ExecuteCheckCommandAsync();
            }

            return args[0].ToLower() switch
            {
                "check" => await ExecuteCheckCommandAsync(),
                "summary" => await ExecuteSummaryCommandAsync(),
                "export" when args.Length > 1 => await ExecuteExportCommandAsync(args[1]),
                "config" when args.Length > 1 && args[1] == "init" => ExecuteConfigInitCommandAsync(),
                _ => await ExecuteCheckCommandAsync()
            };
        }

        /// <summary>
        /// 执行检查命令
        /// </summary>
        private async Task<int> ExecuteCheckCommandAsync()
        {
            try
            {
                var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
                var outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();
                var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();

                AnsiConsole.MarkupLine("[green]开始XML适配状态检查...[/]");

                var result = await checker.CheckAdaptationStatusAsync();

                if (result.Errors.Any())
                {
                    AnsiConsole.MarkupLine("[red]检查完成，但有错误:[/]");
                    foreach (var error in result.Errors)
                    {
                        AnsiConsole.MarkupLine($"[red]• {error}[/]");
                    }
                    return 1;
                }

                // 显示结果表格
                var table = new Table();
                table.AddColumn("指标");
                table.AddColumn("数值");

                table.AddRow("总文件数", result.TotalFiles.ToString());
                table.AddRow("已适配", $"[green]{result.AdaptedFiles}[/]");
                table.AddRow("未适配", $"[red]{result.UnadaptedFiles}[/]");
                table.AddRow("适配率", $"{result.AdaptationRate:F1}%");

                AnsiConsole.Write(table);

                // 显示未适配文件
                if (result.UnadaptedFiles > 0)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[yellow]未适配文件:[/]");

                    var unadaptedTable = new Table();
                    unadaptedTable.AddColumn("文件名");
                    unadaptedTable.AddColumn("大小");
                    unadaptedTable.AddColumn("复杂度");

                    foreach (var file in result.UnadaptedFileInfos.Take(10))
                    {
                        unadaptedTable.AddRow(
                            file.FileName,
                            FormatFileSize(file.FileSize),
                            file.Complexity.ToString()
                        );
                    }

                    AnsiConsole.Write(unadaptedTable);

                    if (result.UnadaptedFiles > 10)
                    {
                        AnsiConsole.MarkupLine($"[grey]... 还有 {result.UnadaptedFiles - 10} 个文件[/]");
                    }
                }

                // 输出各种格式文件
                if (config.OutputFormats.Any())
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[blue]正在生成输出文件...[/]");

                    foreach (var format in config.OutputFormats.Where(f => f != OutputFormat.Console))
                    {
                        try
                        {
                            var outputPath = Path.Combine(config.OutputDirectory, $"adaptation_report_{DateTime.Now:yyyyMMdd_HHmmss}.{format.ToString().ToLower()}");
                            await outputService.OutputResultAsync(result, format, outputPath);
                            AnsiConsole.MarkupLine($"[green]✓[/] {format} 格式报告已生成: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.MarkupLine($"[red]✗[/] 生成 {format} 格式报告失败: {ex.Message}");
                        }
                    }
                }

                AnsiConsole.MarkupLine("[green]检查完成！[/]");
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]检查失败: {ex.Message}[/]");
                return 1;
            }
        }

        /// <summary>
        /// 执行摘要命令
        /// </summary>
        private async Task<int> ExecuteSummaryCommandAsync()
        {
            try
            {
                var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

                var summary = await checker.GetUnadaptedFilesSummaryAsync();
                AnsiConsole.WriteLine(summary);

                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]获取摘要失败: {ex.Message}[/]");
                return 1;
            }
        }

        /// <summary>
        /// 执行导出命令
        /// </summary>
        private async Task<int> ExecuteExportCommandAsync(string format)
        {
            try
            {
                var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
                var outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();
                var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();

                if (!Enum.TryParse<OutputFormat>(format, true, out var outputFormat))
                {
                    AnsiConsole.MarkupLine($"[red]不支持的输出格式: {format}[/]");
                    return 1;
                }

                AnsiConsole.MarkupLine($"[green]正在生成 {outputFormat} 格式报告...[/]");

                var result = await checker.CheckAdaptationStatusAsync();

                if (result.Errors.Any())
                {
                    AnsiConsole.MarkupLine("[red]检查完成，但有错误:[/]");
                    foreach (var error in result.Errors)
                    {
                        AnsiConsole.MarkupLine($"[red]• {error}[/]");
                    }
                    return 1;
                }

                var outputPath = Path.Combine(config.OutputDirectory, $"adaptation_report_{DateTime.Now:yyyyMMdd_HHmmss}.{outputFormat.ToString().ToLower()}");
                await outputService.OutputResultAsync(result, outputFormat, outputPath);

                AnsiConsole.MarkupLine($"[green]报告已导出到: {outputPath}[/]");
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]导出失败: {ex.Message}[/]");
                return 1;
            }
        }

        /// <summary>
        /// 执行配置初始化命令
        /// </summary>
        private static int ExecuteConfigInitCommandAsync()
        {
            try
            {
                var configPath = "appsettings.json";
                AdaptationConfigurationManager.CreateDefaultConfig(configPath);

                AnsiConsole.MarkupLine($"[green]已创建默认配置文件: {configPath}[/]");
                return 0;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]创建配置文件失败: {ex.Message}[/]");
                return 1;
            }
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:F1} {sizes[order]}";
        }
    }
}
