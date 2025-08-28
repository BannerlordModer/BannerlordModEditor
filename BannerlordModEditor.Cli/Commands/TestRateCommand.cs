using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Cli.Helpers;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Repositories.Testing;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Common.Services.Testing;
using BannerlordModEditor.Common.Utils.Testing;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace BannerlordModEditor.Cli.Commands
{
    /// <summary>
    /// 测试通过率检测命令
    /// </summary>
    [Command("test-rate", Description = "检测和监控测试通过率")]
    public class TestRateCommand : ICommand
    {
        private readonly TestExecutionMonitorService _monitorService;
        private readonly TestResultAnalysisService _analysisService;
        private readonly QualityGateService _qualityGateService;
        private readonly TestResultRepository _repository;
        private readonly TestExecutionUtils _executionUtils;
        private readonly TestReportGenerator _reportGenerator;
        private readonly TestNotificationService _notificationService;
        private readonly QualityMonitoringService _qualityMonitoringService;
        private readonly CliOutputHelper _outputHelper;

        /// <summary>
        /// 项目路径
        /// </summary>
        [CommandOption("project", 'p', Description = "测试项目路径")]
        public string? ProjectPath { get; set; }

        /// <summary>
        /// 解决方案路径
        /// </summary>
        [CommandOption("solution", 's', Description = "解决方案路径")]
        public string? SolutionPath { get; set; }

        /// <summary>
        /// 构建配置
        /// </summary>
        [CommandOption("configuration", 'c', Description = "构建配置 (Debug/Release)")]
        public string Configuration { get; set; } = "Debug";

        /// <summary>
        /// 目标框架
        /// </summary>
        [CommandOption("framework", 'f', Description = "目标框架")]
        public string? Framework { get; set; }

        /// <summary>
        /// 输出格式
        /// </summary>
        [CommandOption("output-format", 'o', Description = "输出格式 (Html/Json/Xml/Markdown/Csv)")]
        public string OutputFormat { get; set; } = "Html";

        /// <summary>
        /// 输出文件
        /// </summary>
        [CommandOption("output-file", Description = "输出文件路径")]
        public string? OutputFile { get; set; }

        /// <summary>
        /// 会话名称
        /// </summary>
        [CommandOption("session-name", Description = "测试会话名称")]
        public string? SessionName { get; set; }

        /// <summary>
        /// 是否启用质量门禁
        /// </summary>
        [CommandOption("enable-gates", Description = "启用质量门禁检查")]
        public bool EnableQualityGates { get; set; }

        /// <summary>
        /// 是否收集覆盖率
        /// </summary>
        [CommandOption("collect-coverage", Description = "收集代码覆盖率")]
        public bool CollectCoverage { get; set; }

        /// <summary>
        /// 是否发送通知
        /// </summary>
        [CommandOption("send-notifications", Description = "发送测试结果通知")]
        public bool SendNotifications { get; set; }

        /// <summary>
        /// 通知渠道
        /// </summary>
        [CommandOption("notification-channels", Description = "通知渠道 (逗号分隔)")]
        public string? NotificationChannels { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        [CommandOption("storage-path", Description = "测试结果存储路径")]
        public string? StoragePath { get; set; }

        /// <summary>
        /// 附加参数
        /// </summary>
        [CommandOption("additional-args", Description = "附加测试参数 (JSON格式)")]
        public string? AdditionalArgs { get; set; }

        /// <summary>
        /// 是否详细输出
        /// </summary>
        [CommandOption("verbose", 'v', Description = "详细输出")]
        public bool Verbose { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TestRateCommand(
            TestExecutionMonitorService monitorService,
            TestResultAnalysisService analysisService,
            QualityGateService qualityGateService,
            TestResultRepository repository,
            TestExecutionUtils executionUtils,
            TestReportGenerator reportGenerator,
            TestNotificationService notificationService,
            QualityMonitoringService qualityMonitoringService,
            CliOutputHelper outputHelper)
        {
            _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
            _analysisService = analysisService ?? throw new ArgumentNullException(nameof(analysisService));
            _qualityGateService = qualityGateService ?? throw new ArgumentNullException(nameof(qualityGateService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _executionUtils = executionUtils ?? throw new ArgumentNullException(nameof(executionUtils));
            _reportGenerator = reportGenerator ?? throw new ArgumentNullException(nameof(reportGenerator));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _qualityMonitoringService = qualityMonitoringService ?? throw new ArgumentNullException(nameof(qualityMonitoringService));
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
                
                // 验证参数
                ValidateParameters();
                
                // 设置存储路径
                var storagePath = StoragePath ?? Path.Combine(Directory.GetCurrentDirectory(), "test-results");
                var repository = new TestResultRepository(storagePath);
                
                // 解析附加参数
                var additionalArgs = ParseAdditionalArgs();
                
                _outputHelper.WriteLine("🚀 开始测试通过率检测...", ConsoleColor.Cyan);
                
                // 创建测试会话
                var sessionName = SessionName ?? $"Test Rate Check {DateTime.Now:yyyyMMdd_HHmmss}";
                var projectToTest = ProjectPath ?? SolutionPath ?? throw new InvalidOperationException("必须指定项目路径或解决方案路径");
                
                var session = _monitorService.CreateTestSession(sessionName, projectToTest, SolutionPath ?? string.Empty);
                _outputHelper.WriteLine($"📝 创建测试会话: {session.SessionId}", ConsoleColor.Green);
                
                // 开始执行测试
                await _monitorService.StartTestSessionAsync(session.SessionId);
                
                TestSessionDO testSession;
                
                // 执行测试
                if (!string.IsNullOrEmpty(SolutionPath))
                {
                    _outputHelper.WriteLine($"🧪 执行解决方案测试: {SolutionPath}", ConsoleColor.Yellow);
                    testSession = await _executionUtils.ExecuteSolutionTestsAsync(SolutionPath, Configuration, Framework, additionalArgs);
                }
                else
                {
                    _outputHelper.WriteLine($"🧪 执行项目测试: {ProjectPath}", ConsoleColor.Yellow);
                    testSession = await _executionUtils.ExecuteTestProjectAsync(ProjectPath!, SolutionPath, Configuration, Framework, additionalArgs);
                }
                
                // 收集覆盖率
                if (CollectCoverage)
                {
                    _outputHelper.WriteLine("📊 收集代码覆盖率...", ConsoleColor.Yellow);
                    var coverage = await _executionUtils.CollectCoverageAsync(ProjectPath ?? SolutionPath!, Configuration);
                    testSession.CoverageMetrics = coverage;
                }
                
                // 检查质量门禁
                if (EnableQualityGates)
                {
                    _outputHelper.WriteLine("🚪 检查质量门禁...", ConsoleColor.Yellow);
                    var qualityResult = await _qualityGateService.CheckQualityGatesAsync(testSession);
                    testSession.QualityGates = qualityResult.GateStatuses;
                    
                    if (qualityResult.OverallStatus == QualityGateStatus.Failed)
                    {
                        _outputHelper.WriteLine("❌ 质量门禁检查失败!", ConsoleColor.Red);
                        
                        if (SendNotifications)
                        {
                            var failedGates = qualityResult.GateStatuses
                                .Where(g => g.Status == QualityGateStatus.Failed)
                                .Select(g => g.GateName)
                                .ToList();
                            
                            await _notificationService.SendQualityGateFailedNotificationAsync(testSession, failedGates, ParseNotificationChannels());
                        }
                    }
                    else
                    {
                        _outputHelper.WriteLine("✅ 质量门禁检查通过!", ConsoleColor.Green);
                    }
                }
                
                // 完成测试会话
                await _monitorService.CompleteTestSessionAsync(session.SessionId);
                
                // 保存测试结果
                _outputHelper.WriteLine("💾 保存测试结果...", ConsoleColor.Yellow);
                await repository.SaveTestSessionAsync(testSession);
                
                // 分析测试结果
                _outputHelper.WriteLine("🔍 分析测试结果...", ConsoleColor.Yellow);
                var analysis = _analysisService.AnalyzeTestSession(testSession);
                
                // 生成报告
                _outputHelper.WriteLine("📄 生成测试报告...", ConsoleColor.Yellow);
                await GenerateReport(testSession, analysis);
                
                // 发送通知
                if (SendNotifications)
                {
                    _outputHelper.WriteLine("📧 发送通知...", ConsoleColor.Yellow);
                    await SendTestNotifications(testSession, analysis);
                }
                
                // 显示结果
                await DisplayResults(testSession, analysis);
                
                _outputHelper.WriteLine("✨ 测试通过率检测完成!", ConsoleColor.Green);
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
        /// 验证参数
        /// </summary>
        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(ProjectPath) && string.IsNullOrEmpty(SolutionPath))
            {
                throw new InvalidOperationException("必须指定项目路径或解决方案路径");
            }
            
            if (!string.IsNullOrEmpty(ProjectPath) && !File.Exists(ProjectPath))
            {
                throw new FileNotFoundException($"测试项目文件不存在: {ProjectPath}");
            }
            
            if (!string.IsNullOrEmpty(SolutionPath) && !File.Exists(SolutionPath))
            {
                throw new FileNotFoundException($"解决方案文件不存在: {SolutionPath}");
            }
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                var directory = Path.GetDirectoryName(OutputFile);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }

        /// <summary>
        /// 解析附加参数
        /// </summary>
        private Dictionary<string, string>? ParseAdditionalArgs()
        {
            if (string.IsNullOrEmpty(AdditionalArgs))
            {
                return null;
            }
            
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(AdditionalArgs);
            }
            catch
            {
                _outputHelper.WriteLine("⚠️ 附加参数格式错误，应为JSON格式", ConsoleColor.Yellow);
                return null;
            }
        }

        /// <summary>
        /// 解析通知渠道
        /// </summary>
        private List<string>? ParseNotificationChannels()
        {
            if (string.IsNullOrEmpty(NotificationChannels))
            {
                return null;
            }
            
            return NotificationChannels.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToList();
        }

        /// <summary>
        /// 生成报告
        /// </summary>
        private async Task GenerateReport(TestSessionDO session, TestSessionAnalysis analysis)
        {
            string reportContent;
            var format = Enum.Parse<ReportFormat>(OutputFormat, true);
            
            switch (format)
            {
                case ReportFormat.Html:
                    reportContent = await _reportGenerator.GenerateHtmlReportAsync(session);
                    break;
                case ReportFormat.Xml:
                    reportContent = await _reportGenerator.GenerateXmlReportAsync(session);
                    break;
                case ReportFormat.Json:
                    reportContent = await _reportGenerator.GenerateJsonReportAsync(session);
                    break;
                case ReportFormat.Markdown:
                    reportContent = await _reportGenerator.GenerateMarkdownReportAsync(session);
                    break;
                case ReportFormat.Csv:
                    reportContent = await _reportGenerator.GenerateCsvReportAsync(session);
                    break;
                default:
                    reportContent = await _reportGenerator.GenerateHtmlReportAsync(session);
                    break;
            }
            
            if (!string.IsNullOrEmpty(OutputFile))
            {
                await _reportGenerator.SaveReportAsync(reportContent, OutputFile, format);
                _outputHelper.WriteLine($"📄 报告已保存到: {OutputFile}", ConsoleColor.Green);
            }
            else
            {
                // 对于所有报告格式，都只显示摘要信息，避免格式化问题
                _outputHelper.WriteLine("📄 报告已生成到控制台输出", ConsoleColor.Cyan);
                _outputHelper.WriteLine($"报告格式: {format}", ConsoleColor.White);
                _outputHelper.WriteLine($"报告长度: {reportContent.Length} 字符", ConsoleColor.White);
                
                if (Verbose)
                {
                    _outputHelper.WriteLine("报告预览 (前200字符):", ConsoleColor.Gray);
                    var preview = reportContent.Length > 200 ? reportContent.Substring(0, 200) + "..." : reportContent;
                    // 简单输出预览，避免格式化问题
                    _outputHelper.WriteLine("预览内容已生成，但为避免格式化问题不在此显示", ConsoleColor.Gray);
                    _outputHelper.WriteLine($"预览长度: {preview.Length} 字符", ConsoleColor.Gray);
                }
            }
        }

        /// <summary>
        /// 发送测试通知
        /// </summary>
        private async Task SendTestNotifications(TestSessionDO session, TestSessionAnalysis analysis)
        {
            var channels = ParseNotificationChannels();
            
            // 发送测试会话完成通知
            await _notificationService.SendTestSessionCompletedNotificationAsync(session, channels);
            
            // 发送失败测试通知
            var failedTests = session.TestResults.Where(r => r.Status == TestStatus.Failed).ToList();
            if (failedTests.Count > 0)
            {
                await _notificationService.SendTestFailedNotificationAsync(session, failedTests, channels);
            }
            
            // 发送性能警告通知
            if (analysis.PerformanceInsights.PerformanceIssues.Count > 0)
            {
                await _notificationService.SendPerformanceWarningNotificationAsync(session, analysis.PerformanceInsights.PerformanceIssues, channels);
            }
        }

        /// <summary>
        /// 显示结果
        /// </summary>
        private async Task DisplayResults(TestSessionDO session, TestSessionAnalysis analysis)
        {
            _outputHelper.WriteLine("\n📊 测试结果摘要:", ConsoleColor.Cyan);
            _outputHelper.WriteLine($"  会话名称: {session.SessionName}", ConsoleColor.White);
            _outputHelper.WriteLine($"  总测试数: {session.TotalTests}", ConsoleColor.White);
            _outputHelper.WriteLine($"  通过测试: {session.PassedTests}", ConsoleColor.Green);
            _outputHelper.WriteLine($"  失败测试: {session.FailedTests}", ConsoleColor.Red);
            _outputHelper.WriteLine($"  跳过测试: {session.SkippedTests}", ConsoleColor.Yellow);
            _outputHelper.WriteLine($"  通过率: {session.PassRate:F2}%", ConsoleColor.White);
            _outputHelper.WriteLine($"  执行时间: {session.TotalDurationMs} ms", ConsoleColor.White);
            
            if (session.CoverageMetrics != null)
            {
                _outputHelper.WriteLine("\n📈 覆盖率指标:", ConsoleColor.Cyan);
                _outputHelper.WriteLine($"  行覆盖率: {session.CoverageMetrics.LineCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"  分支覆盖率: {session.CoverageMetrics.BranchCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"  方法覆盖率: {session.CoverageMetrics.MethodCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"  类覆盖率: {session.CoverageMetrics.ClassCoverage:F2}%", ConsoleColor.White);
                _outputHelper.WriteLine($"  覆盖率等级: {session.CoverageMetrics.CoverageGrade}", ConsoleColor.White);
            }
            
            _outputHelper.WriteLine($"\n🏥 整体健康状况: {analysis.OverallHealth}", ConsoleColor.Cyan);
            
            if (analysis.RiskAssessment.RiskItems.Count > 0)
            {
                _outputHelper.WriteLine($"\n⚠️  风险评估 ({analysis.RiskAssessment.OverallRiskLevel}):", ConsoleColor.Yellow);
                foreach (var risk in analysis.RiskAssessment.RiskItems)
                {
                    _outputHelper.WriteLine($"  - {risk.Category}: {risk.Description}", ConsoleColor.White);
                }
            }
            
            if (analysis.Recommendations.Count > 0)
            {
                _outputHelper.WriteLine("\n💡 改进建议:", ConsoleColor.Cyan);
                foreach (var recommendation in analysis.Recommendations)
                {
                    _outputHelper.WriteLine($"  - {recommendation}", ConsoleColor.White);
                }
            }
            
            if (Verbose)
            {
                _outputHelper.WriteLine("\n🔍 详细分析:", ConsoleColor.Cyan);
                var analysisReport = _analysisService.GenerateAnalysisReport(analysis);
                _outputHelper.WriteLine(analysisReport, ConsoleColor.Gray);
            }
        }
    }
}