using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Cli.Helpers;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Repositories.Testing;
using BannerlordModEditor.Common.Services.Testing;
using BannerlordModEditor.Common.Utils.Testing;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace BannerlordModEditor.Cli.Commands
{
    /// <summary>
    /// 测试结果查询命令
    /// </summary>
    [Command("test-results", Description = "查询和管理测试结果")]
    public class TestResultsCommand : ICommand
    {
        private readonly TestResultRepository _repository;
        private readonly TestReportGenerator _reportGenerator;
        private readonly CliOutputHelper _outputHelper;

        /// <summary>
        /// 操作类型
        /// </summary>
        [CommandOption("action", 'a', Description = "操作类型 (list/show/stats/export/cleanup)")]
        public string Action { get; set; } = "list";

        /// <summary>
        /// 会话ID
        /// </summary>
        [CommandOption("session-id", Description = "测试会话ID")]
        public string? SessionId { get; set; }

        /// <summary>
        /// 项目路径
        /// </summary>
        [CommandOption("project-path", Description = "项目路径过滤")]
        public string? ProjectPath { get; set; }

        /// <summary>
        /// 状态过滤
        /// </summary>
        [CommandOption("status", Description = "会话状态过滤 (Created/Running/Completed/Failed/Cancelled)")]
        public string? Status { get; set; }

        /// <summary>
        /// 限制数量
        /// </summary>
        [CommandOption("limit", 'l', Description = "限制返回结果数量")]
        public int? Limit { get; set; }

        /// <summary>
        /// 输出格式
        /// </summary>
        [CommandOption("output-format", 'o', Description = "输出格式 (Table/Json/Xml/Html/Markdown)")]
        public string OutputFormat { get; set; } = "Table";

        /// <summary>
        /// 输出格式枚举
        /// </summary>
        public enum OutputFormatEnum
        {
            Table,
            Json,
            Xml,
            Html,
            Markdown
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        [CommandOption("output-file", Description = "输出文件路径")]
        public string? OutputFile { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        [CommandOption("storage-path", Description = "测试结果存储路径")]
        public string? StoragePath { get; set; }

        /// <summary>
        /// 清理天数
        /// </summary>
        [CommandOption("cleanup-days", Description = "清理指定天数前的数据")]
        public int? CleanupDays { get; set; }

        /// <summary>
        /// 是否详细输出
        /// </summary>
        [CommandOption("verbose", 'v', Description = "详细输出")]
        public bool Verbose { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestResultsCommand(
            TestResultRepository repository,
            TestReportGenerator reportGenerator,
            CliOutputHelper outputHelper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _reportGenerator = reportGenerator ?? throw new ArgumentNullException(nameof(reportGenerator));
            _outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                _outputHelper.SetConsole(console);
                
                // 设置存储路径
                var storagePath = StoragePath ?? Path.Combine(Directory.GetCurrentDirectory(), "test-results");
                var repository = new TestResultRepository(storagePath);
                
                switch (Action.ToLower())
                {
                    case "list":
                        await ListTestResults(repository);
                        break;
                    case "show":
                        await ShowTestSession(repository);
                        break;
                    case "stats":
                        await ShowStatistics(repository);
                        break;
                    case "export":
                        await ExportTestResults(repository);
                        break;
                    case "cleanup":
                        await CleanupTestResults(repository);
                        break;
                    default:
                        throw new InvalidOperationException($"不支持的操作类型: {Action}");
                }
            }
            catch (Exception ex)
            {
                _outputHelper.WriteLine($"❌ 执行失败: {ex.Message}", ConsoleColor.Red);
                if (Verbose)
                {
                    _outputHelper.WriteLine($"堆栈跟踪: {ex.StackTrace}", ConsoleColor.Gray);
                }
                throw;
            }
        }

        /// <summary>
        /// 列出测试结果
        /// </summary>
        private async Task ListTestResults(TestResultRepository repository)
        {
            _outputHelper.WriteLine("📋 测试会话列表:", ConsoleColor.Cyan);
            
            List<TestSessionDO> sessions;
            
            if (!string.IsNullOrEmpty(ProjectPath))
            {
                sessions = await repository.GetTestSessionsByProjectAsync(ProjectPath);
            }
            else if (!string.IsNullOrEmpty(Status) && Enum.TryParse<TestSessionStatus>(Status, true, out var sessionStatus))
            {
                sessions = await repository.GetTestSessionsByStatusAsync(sessionStatus);
            }
            else
            {
                sessions = await repository.GetRecentTestSessionsAsync(Limit ?? 10);
            }
            
            if (sessions.Count == 0)
            {
                _outputHelper.WriteLine("  没有找到测试会话", ConsoleColor.Yellow);
                return;
            }
            
            var format = Enum.Parse<OutputFormatEnum>(OutputFormat, true);
            
            switch (format)
            {
                case OutputFormatEnum.Table:
                    DisplaySessionsTable(sessions);
                    break;
                case OutputFormatEnum.Json:
                    await ExportAsJson(sessions);
                    break;
                case OutputFormatEnum.Xml:
                    await ExportAsXml(sessions);
                    break;
                case OutputFormatEnum.Html:
                    await ExportAsHtml(sessions);
                    break;
                case OutputFormatEnum.Markdown:
                    await ExportAsMarkdown(sessions);
                    break;
                default:
                    DisplaySessionsTable(sessions);
                    break;
            }
        }

        /// <summary>
        /// 显示测试会话详情
        /// </summary>
        private async Task ShowTestSession(TestResultRepository repository)
        {
            if (string.IsNullOrEmpty(SessionId))
            {
                throw new InvalidOperationException("显示会话详情需要指定会话ID");
            }
            
            _outputHelper.WriteLine($"📄 测试会话详情: {SessionId}", ConsoleColor.Cyan);
            
            var session = await repository.LoadTestSessionAsync(SessionId);
            if (session == null)
            {
                _outputHelper.WriteLine($"  会话不存在: {SessionId}", ConsoleColor.Red);
                return;
            }
            
            var format = Enum.Parse<OutputFormatEnum>(OutputFormat, true);
            string reportContent;
            
            switch (format)
            {
                case OutputFormatEnum.Json:
                    reportContent = await _reportGenerator.GenerateJsonReportAsync(session);
                    break;
                case OutputFormatEnum.Xml:
                    reportContent = await _reportGenerator.GenerateXmlReportAsync(session);
                    break;
                case OutputFormatEnum.Html:
                    reportContent = await _reportGenerator.GenerateHtmlReportAsync(session);
                    break;
                case OutputFormatEnum.Markdown:
                    reportContent = await _reportGenerator.GenerateMarkdownReportAsync(session);
                    break;
                default:
                    DisplaySessionDetails(session);
                    return;
            }
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await _reportGenerator.SaveReportAsync(reportContent, OutputFile, 
                    Enum.Parse<ReportFormat>(OutputFormat, true));
                _outputHelper.WriteLine($"📄 报告已保存到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine(reportContent, ConsoleColor.White);
            }
        }

        /// <summary>
        /// 显示统计信息
        /// </summary>
        private async Task ShowStatistics(TestResultRepository repository)
        {
            _outputHelper.WriteLine("📊 测试统计信息:", ConsoleColor.Cyan);
            
            var stats = await repository.GetSessionStatisticsAsync();
            
            _outputHelper.WriteLine($"  总会话数: {stats.TotalSessions}", ConsoleColor.White);
            _outputHelper.WriteLine($"  已完成会话: {stats.CompletedSessions}", ConsoleColor.Green);
            _outputHelper.WriteLine($"  失败会话: {stats.FailedSessions}", ConsoleColor.Red);
            _outputHelper.WriteLine($"  取消会话: {stats.CancelledSessions}", ConsoleColor.Yellow);
            _outputHelper.WriteLine($"  总测试数: {stats.TotalTests}", ConsoleColor.White);
            _outputHelper.WriteLine($"  总通过测试: {stats.TotalPassedTests}", ConsoleColor.Green);
            _outputHelper.WriteLine($"  总失败测试: {stats.TotalFailedTests}", ConsoleColor.Red);
            _outputHelper.WriteLine($"  总跳过测试: {stats.TotalSkippedTests}", ConsoleColor.Yellow);
            _outputHelper.WriteLine($"  平均通过率: {stats.AveragePassRate:F2}%", ConsoleColor.White);
            _outputHelper.WriteLine($"  总执行时间: {TimeSpan.FromMilliseconds(stats.TotalExecutionTime):hh\\:mm\\:ss}", ConsoleColor.White);
            _outputHelper.WriteLine($"  最后会话时间: {stats.LastSessionTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.White);
            _outputHelper.WriteLine($"  最旧会话时间: {stats.OldestSessionTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.White);
            
            if (Verbose)
            {
                var storageInfo = await repository.GetStorageUsageInfoAsync();
                _outputHelper.WriteLine("\n💾 存储使用情况:", ConsoleColor.Cyan);
                _outputHelper.WriteLine($"  总文件数: {storageInfo.TotalFiles}", ConsoleColor.White);
                _outputHelper.WriteLine($"  总大小: {FormatBytes((long)storageInfo.TotalSizeBytes)}", ConsoleColor.White);
                _outputHelper.WriteLine($"  平均文件大小: {FormatBytes((long)storageInfo.AverageFileSizeBytes)}", ConsoleColor.White);
                _outputHelper.WriteLine($"  最旧文件时间: {storageInfo.OldestFileTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.White);
                _outputHelper.WriteLine($"  最新文件时间: {storageInfo.NewestFileTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.White);
                
                var validationResult = await repository.ValidateStorageIntegrityAsync();
                _outputHelper.WriteLine("\n🔍 存储完整性验证:", ConsoleColor.Cyan);
                _outputHelper.WriteLine($"  验证状态: {(validationResult.IsValid ? "✅ 通过" : "❌ 失败")}", 
                    validationResult.IsValid ? ConsoleColor.Green : ConsoleColor.Red);
                
                if (validationResult.Errors.Count > 0)
                {
                    _outputHelper.WriteLine("  错误:", ConsoleColor.Red);
                    foreach (var error in validationResult.Errors)
                    {
                        _outputHelper.WriteLine($"    - {error}", ConsoleColor.Red);
                    }
                }
                
                if (validationResult.Warnings.Count > 0)
                {
                    _outputHelper.WriteLine("  警告:", ConsoleColor.Yellow);
                    foreach (var warning in validationResult.Warnings)
                    {
                        _outputHelper.WriteLine($"    - {warning}", ConsoleColor.Yellow);
                    }
                }
            }
        }

        /// <summary>
        /// 导出测试结果
        /// </summary>
        private async Task ExportTestResults(TestResultRepository repository)
        {
            List<TestSessionDO> sessions;
            
            if (!string.IsNullOrEmpty(SessionId))
            {
                var session = await repository.LoadTestSessionAsync(SessionId);
                sessions = session != null ? new List<TestSessionDO> { session } : new List<TestSessionDO>();
            }
            else
            {
                sessions = await repository.GetAllTestSessionsAsync();
            }
            
            if (sessions.Count == 0)
            {
                _outputHelper.WriteLine("  没有找到测试会话", ConsoleColor.Yellow);
                return;
            }
            
            var format = Enum.Parse<OutputFormatEnum>(OutputFormat, true);
            string reportContent;
            string defaultFileName;
            
            switch (format)
            {
                case OutputFormatEnum.Json:
                    reportContent = await _reportGenerator.GenerateJsonReportAsync(sessions.First());
                    defaultFileName = $"test_report_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    break;
                case OutputFormatEnum.Xml:
                    reportContent = await _reportGenerator.GenerateXmlReportAsync(sessions.First());
                    defaultFileName = $"test_report_{DateTime.Now:yyyyMMdd_HHmmss}.xml";
                    break;
                case OutputFormatEnum.Html:
                    reportContent = await _reportGenerator.GenerateHtmlReportAsync(sessions.First());
                    defaultFileName = $"test_report_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                    break;
                case OutputFormatEnum.Markdown:
                    reportContent = await _reportGenerator.GenerateMarkdownReportAsync(sessions.First());
                    defaultFileName = $"test_report_{DateTime.Now:yyyyMMdd_HHmmss}.md";
                    break;
                default:
                    reportContent = await _reportGenerator.GenerateJsonReportAsync(sessions.First());
                    defaultFileName = $"test_report_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                    break;
            }
            
            var outputFile = OutputFile ?? Path.Combine(Directory.GetCurrentDirectory(), defaultFileName);
            await _reportGenerator.SaveReportAsync(reportContent, outputFile, 
                Enum.Parse<ReportFormat>(OutputFormat, true));
            
            _outputHelper.WriteLine($"📄 测试结果已导出到: {outputFile}", ConsoleColor.Green);
        }

        /// <summary>
        /// 清理测试结果
        /// </summary>
        private async Task CleanupTestResults(TestResultRepository repository)
        {
            if (!CleanupDays.HasValue || CleanupDays.Value <= 0)
            {
                throw new InvalidOperationException("清理天数必须大于0");
            }
            
            _outputHelper.WriteLine($"🧹 清理 {CleanupDays} 天前的测试结果...", ConsoleColor.Cyan);
            
            var cutoffDate = DateTime.Now.AddDays(-CleanupDays.Value);
            await repository.CleanupOldSessionsAsync(cutoffDate);
            
            _outputHelper.WriteLine($"✅ 清理完成", ConsoleColor.Green);
        }

        /// <summary>
        /// 显示会话表格
        /// </summary>
        private void DisplaySessionsTable(List<TestSessionDO> sessions)
        {
            _outputHelper.WriteLine("  会话ID                          会话名称                      状态      测试数  通过率   执行时间", ConsoleColor.White);
            _outputHelper.WriteLine("  " + new string('-', 90), ConsoleColor.White);
            
            foreach (var session in sessions)
            {
                var statusColor = GetStatusColor(session.SessionStatus);
                var passRateColor = GetPassRateColor(session.PassRate);
                
                _outputHelper.WriteLine($"  {session.SessionId,-32} {session.SessionName,-28} " +
                    $"{session.SessionStatus,-8} {session.TotalTests,6} " +
                    $"{session.PassRate,6:F2}% {TimeSpan.FromMilliseconds(session.TotalDurationMs):hh\\:mm\\:ss}", 
                    ConsoleColor.White);
            }
        }

        /// <summary>
        /// 显示会话详情
        /// </summary>
        private void DisplaySessionDetails(TestSessionDO session)
        {
            _outputHelper.WriteLine($"  会话ID: {session.SessionId}", ConsoleColor.White);
            _outputHelper.WriteLine($"  会话名称: {session.SessionName}", ConsoleColor.White);
            _outputHelper.WriteLine($"  项目路径: {session.ProjectPath}", ConsoleColor.White);
            _outputHelper.WriteLine($"  解决方案路径: {session.SolutionPath}", ConsoleColor.White);
            _outputHelper.WriteLine($"  开始时间: {session.StartTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.White);
            _outputHelper.WriteLine($"  结束时间: {session.EndTime:yyyy-MM-dd HH:mm:ss}", ConsoleColor.White);
            _outputHelper.WriteLine($"  状态: {session.SessionStatus}", GetStatusColor(session.SessionStatus));
            _outputHelper.WriteLine($"  构建配置: {session.BuildConfiguration}", ConsoleColor.White);
            _outputHelper.WriteLine($"  目标框架: {session.TargetFramework}", ConsoleColor.White);
            _outputHelper.WriteLine(string.Empty);
            
            _outputHelper.WriteLine($"  总测试数: {session.TotalTests}", ConsoleColor.White);
            _outputHelper.WriteLine($"  通过测试: {session.PassedTests}", ConsoleColor.Green);
            _outputHelper.WriteLine($"  失败测试: {session.FailedTests}", ConsoleColor.Red);
            _outputHelper.WriteLine($"  跳过测试: {session.SkippedTests}", ConsoleColor.Yellow);
            _outputHelper.WriteLine($"  通过率: {session.PassRate:F2}%", GetPassRateColor(session.PassRate));
            _outputHelper.WriteLine($"  执行时间: {TimeSpan.FromMilliseconds(session.TotalDurationMs):hh\\:mm\\:ss}", ConsoleColor.White);
            
            if (session.CoverageMetrics != null)
            {
                _outputHelper.WriteLine(string.Empty);
                _outputHelper.WriteLine("  覆盖率指标:", ConsoleColor.White);
                _outputHelper.WriteLine($"    行覆盖率: {session.CoverageMetrics.LineCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"    分支覆盖率: {session.CoverageMetrics.BranchCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"    方法覆盖率: {session.CoverageMetrics.MethodCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"    类覆盖率: {session.CoverageMetrics.ClassCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"    覆盖率等级: {session.CoverageMetrics.CoverageGrade}", ConsoleColor.White);
            }
            
            if (session.QualityGates.Count > 0)
            {
                _outputHelper.WriteLine(string.Empty);
                _outputHelper.WriteLine("  质量门禁:", ConsoleColor.White);
                foreach (var gate in session.QualityGates)
                {
                    var gateColor = gate.Status == QualityGateStatus.Passed ? ConsoleColor.Green : ConsoleColor.Red;
                    _outputHelper.WriteLine($"    {gate.GateName}: {gate.Status} ({gate.CurrentValue:F2} / {gate.Threshold:F2})", 
                        gateColor);
                }
            }
            
            if (Verbose)
            {
                _outputHelper.WriteLine(string.Empty);
                _outputHelper.WriteLine("  测试结果详情:", ConsoleColor.White);
                
                var groupedResults = session.TestResults.GroupBy(r => r.Status);
                foreach (var group in groupedResults)
                {
                    _outputHelper.WriteLine($"    {group.Key} ({group.Count()}):", GetStatusColor(group.Key));
                    foreach (var result in group.Take(5)) // 只显示前5个
                    {
                        _outputHelper.WriteLine($"      - {result.Name} ({result.DurationMs} ms)", ConsoleColor.White);
                    }
                    if (group.Count() > 5)
                    {
                        _outputHelper.WriteLine($"      ... 还有 {group.Count() - 5} 个测试", ConsoleColor.Gray);
                    }
                }
            }
        }

        /// <summary>
        /// 导出为JSON
        /// </summary>
        private async Task ExportAsJson(List<TestSessionDO> sessions)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(sessions, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await File.WriteAllTextAsync(OutputFile, json);
                _outputHelper.WriteLine($"📄 JSON数据已保存到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine(json, ConsoleColor.White);
            }
        }

        /// <summary>
        /// 导出为XML
        /// </summary>
        private async Task ExportAsXml(List<TestSessionDO> sessions)
        {
            var xml = await _reportGenerator.GenerateXmlReportAsync(sessions.First());
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await File.WriteAllTextAsync(OutputFile, xml);
                _outputHelper.WriteLine($"📄 XML数据已保存到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine(xml, ConsoleColor.White);
            }
        }

        /// <summary>
        /// 导出为HTML
        /// </summary>
        private async Task ExportAsHtml(List<TestSessionDO> sessions)
        {
            var html = await _reportGenerator.GenerateHtmlReportAsync(sessions.First());
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await File.WriteAllTextAsync(OutputFile, html);
                _outputHelper.WriteLine($"📄 HTML报告已保存到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine(html, ConsoleColor.White);
            }
        }

        /// <summary>
        /// 导出为Markdown
        /// </summary>
        private async Task ExportAsMarkdown(List<TestSessionDO> sessions)
        {
            var markdown = await _reportGenerator.GenerateMarkdownReportAsync(sessions.First());
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await File.WriteAllTextAsync(OutputFile, markdown);
                _outputHelper.WriteLine($"📄 Markdown报告已保存到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                _outputHelper.WriteLine(markdown, ConsoleColor.White);
            }
        }

        /// <summary>
        /// 获取状态颜色
        /// </summary>
        private ConsoleColor GetStatusColor(TestSessionStatus status)
        {
            return status switch
            {
                TestSessionStatus.Completed => ConsoleColor.Green,
                TestSessionStatus.Failed => ConsoleColor.Red,
                TestSessionStatus.Cancelled => ConsoleColor.Yellow,
                TestSessionStatus.Running => ConsoleColor.Cyan,
                _ => ConsoleColor.White
            };
        }

        /// <summary>
        /// 获取测试状态颜色
        /// </summary>
        private ConsoleColor GetStatusColor(TestStatus status)
        {
            return status switch
            {
                TestStatus.Passed => ConsoleColor.Green,
                TestStatus.Failed => ConsoleColor.Red,
                TestStatus.Skipped => ConsoleColor.Yellow,
                TestStatus.Running => ConsoleColor.Cyan,
                _ => ConsoleColor.White
            };
        }

        /// <summary>
        /// 获取通过率颜色
        /// </summary>
        private ConsoleColor GetPassRateColor(double passRate)
        {
            return passRate switch
            {
                >= 90.0 => ConsoleColor.Green,
                >= 80.0 => ConsoleColor.Cyan,
                >= 70.0 => ConsoleColor.Yellow,
                >= 60.0 => ConsoleColor.Magenta,
                _ => ConsoleColor.Red
            };
        }

        /// <summary>
        /// 格式化字节数
        /// </summary>
        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            
            return $"{len:0.##} {sizes[order]}";
        }
    }

  }