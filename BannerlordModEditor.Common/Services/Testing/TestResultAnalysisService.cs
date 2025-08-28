using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Services.Testing
{
    /// <summary>
    /// 测试结果分析服务
    /// 负责分析测试结果并提供洞察
    /// </summary>
    public class TestResultAnalysisService
    {
        private readonly QualityMonitoringService _qualityMonitoringService;

        /// <summary>
        /// 初始化测试结果分析服务
        /// </summary>
        public TestResultAnalysisService(QualityMonitoringService qualityMonitoringService)
        {
            _qualityMonitoringService = qualityMonitoringService ?? throw new ArgumentNullException(nameof(qualityMonitoringService));
        }

        /// <summary>
        /// 分析测试会话结果
        /// </summary>
        public TestSessionAnalysis AnalyzeTestSession(TestSessionDO session)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var analysis = new TestSessionAnalysis
                {
                    SessionId = session.SessionId,
                    SessionName = session.SessionName,
                    AnalysisTime = DateTime.Now,
                    OverallHealth = CalculateOverallHealth(session),
                    KeyMetrics = CalculateKeyMetrics(session),
                    TrendAnalysis = AnalyzeTrends(session),
                    Recommendations = GenerateRecommendations(session),
                    RiskAssessment = AssessRisks(session),
                    PerformanceInsights = AnalyzePerformance(session)
                };

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("AnalyzeTestSession", stopwatch.ElapsedMilliseconds, true);
                
                return analysis;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("AnalyzeTestSession", stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 计算整体健康状况
        /// </summary>
        private HealthStatus CalculateOverallHealth(TestSessionDO session)
        {
            var passRate = session.PassRate;
            var coverageAvg = (session.CoverageMetrics.LineCoverage + 
                              session.CoverageMetrics.BranchCoverage + 
                              session.CoverageMetrics.MethodCoverage + 
                              session.CoverageMetrics.ClassCoverage) / 4.0;

            // 通过率权重 60%，覆盖率权重 40%
            var healthScore = (passRate * 0.6) + (coverageAvg * 0.4);

            if (healthScore >= 90.0)
                return HealthStatus.Excellent;
            else if (healthScore >= 80.0)
                return HealthStatus.Good;
            else if (healthScore >= 70.0)
                return HealthStatus.Fair;
            else if (healthScore >= 60.0)
                return HealthStatus.Poor;
            else
                return HealthStatus.Critical;
        }

        /// <summary>
        /// 计算关键指标
        /// </summary>
        private KeyMetrics CalculateKeyMetrics(TestSessionDO session)
        {
            var avgTestDuration = session.TotalTests > 0 ? 
                session.TotalDurationMs / (double)session.TotalTests : 0.0;

            var failedTests = session.TestResults.Where(r => r.Status == TestStatus.Failed).ToList();
            var errorCategories = failedTests
                .GroupBy(t => t.Category)
                .ToDictionary(g => g.Key, g => g.Count());

            var slowTests = session.TestResults
                .Where(r => r.DurationMs > 1000)
                .OrderByDescending(r => r.DurationMs)
                .Take(10)
                .ToList();

            return new KeyMetrics
            {
                PassRate = session.PassRate,
                TotalTests = session.TotalTests,
                FailedTests = session.FailedTests,
                SkippedTests = session.SkippedTests,
                AverageTestDuration = avgTestDuration,
                TotalExecutionTime = session.TotalDurationMs,
                Coverage = session.CoverageMetrics,
                ErrorCategories = errorCategories,
                SlowestTests = slowTests,
                QualityGatesPassed = session.QualityGates.Count(g => g.Status == QualityGateStatus.Passed),
                QualityGatesFailed = session.QualityGates.Count(g => g.Status == QualityGateStatus.Failed)
            };
        }

        /// <summary>
        /// 分析趋势
        /// </summary>
        private TrendAnalysis AnalyzeTrends(TestSessionDO session)
        {
            // 这里简化实现，实际应用中需要历史数据
            return new TrendAnalysis
            {
                PassRateTrend = TrendDirection.Stable,
                CoverageTrend = TrendDirection.Stable,
                PerformanceTrend = TrendDirection.Stable,
                ReliabilityTrend = TrendDirection.Stable,
                Notes = new List<string>
                {
                    "趋势分析需要历史数据进行对比",
                    "建议建立测试结果历史记录"
                }
            };
        }

        /// <summary>
        /// 生成建议
        /// </summary>
        private List<string> GenerateRecommendations(TestSessionDO session)
        {
            var recommendations = new List<string>();

            // 通过率相关建议
            if (session.PassRate < 80.0)
            {
                recommendations.Add($"测试通过率较低 ({session.PassRate:F2}%)，建议检查失败的测试用例");
            }

            // 覆盖率相关建议
            var coverageAvg = (session.CoverageMetrics.LineCoverage + 
                              session.CoverageMetrics.BranchCoverage + 
                              session.CoverageMetrics.MethodCoverage + 
                              session.CoverageMetrics.ClassCoverage) / 4.0;

            if (coverageAvg < 70.0)
            {
                recommendations.Add($"代码覆盖率较低 ({coverageAvg:F2}%)，建议增加测试用例覆盖关键路径");
            }

            // 性能相关建议
            var avgTestDuration = session.TotalTests > 0 ? 
                session.TotalDurationMs / (double)session.TotalTests : 0.0;

            if (avgTestDuration > 500)
            {
                recommendations.Add($"平均测试执行时间较长 ({avgTestDuration:F2} ms)，建议优化测试性能");
            }

            // 错误分析建议
            var failedTests = session.TestResults.Where(r => r.Status == TestStatus.Failed).ToList();
            if (failedTests.Count > 0)
            {
                var errorGroups = failedTests.GroupBy(t => t.Category).ToList();
                var mostProblematicCategory = errorGroups.OrderByDescending(g => g.Count()).First();
                
                recommendations.Add($"{mostProblematicCategory.Key} 类别的测试失败率最高，建议重点关注");
            }

            // 质量门禁建议
            var failedGates = session.QualityGates.Where(g => g.Status == QualityGateStatus.Failed).ToList();
            if (failedGates.Count > 0)
            {
                foreach (var gate in failedGates)
                {
                    recommendations.Add($"质量门禁 '{gate.GateName}' 未通过: {gate.Message}");
                }
            }

            return recommendations;
        }

        /// <summary>
        /// 评估风险
        /// </summary>
        private RiskAssessment AssessRisks(TestSessionDO session)
        {
            var risks = new List<RiskItem>();
            var overallRiskLevel = RiskLevel.Low;

            // 通过率风险
            if (session.PassRate < 60.0)
            {
                risks.Add(new RiskItem
                {
                    Level = RiskLevel.Critical,
                    Category = "测试通过率",
                    Description = $"测试通过率过低 ({session.PassRate:F2}%)",
                    Impact = "可能导致严重质量问题",
                    Recommendation = "立即停止部署并修复所有失败的测试"
                });
                overallRiskLevel = RiskLevel.Critical;
            }
            else if (session.PassRate < 80.0)
            {
                risks.Add(new RiskItem
                {
                    Level = RiskLevel.High,
                    Category = "测试通过率",
                    Description = $"测试通过率低于目标 ({session.PassRate:F2}%)",
                    Impact = "可能存在隐藏的质量问题",
                    Recommendation = "在部署前修复关键测试失败"
                });
                if (overallRiskLevel < RiskLevel.High) overallRiskLevel = RiskLevel.High;
            }

            // 覆盖率风险
            var coverageAvg = (session.CoverageMetrics.LineCoverage + 
                              session.CoverageMetrics.BranchCoverage + 
                              session.CoverageMetrics.MethodCoverage + 
                              session.CoverageMetrics.ClassCoverage) / 4.0;

            if (coverageAvg < 50.0)
            {
                risks.Add(new RiskItem
                {
                    Level = RiskLevel.High,
                    Category = "代码覆盖率",
                    Description = $"代码覆盖率过低 ({coverageAvg:F2}%)",
                    Impact = "大量代码未经测试，存在未知缺陷风险",
                    Recommendation = "增加测试用例以提高覆盖率"
                });
                if (overallRiskLevel < RiskLevel.High) overallRiskLevel = RiskLevel.High;
            }

            // 性能风险
            var avgTestDuration = session.TotalTests > 0 ? 
                session.TotalDurationMs / (double)session.TotalTests : 0.0;

            if (avgTestDuration > 1000)
            {
                risks.Add(new RiskItem
                {
                    Level = RiskLevel.Medium,
                    Category = "测试性能",
                    Description = $"测试执行时间过长 (平均 {avgTestDuration:F2} ms)",
                    Impact = "影响开发效率和CI/CD流程",
                    Recommendation = "优化测试性能，考虑并行执行"
                });
                if (overallRiskLevel < RiskLevel.Medium) overallRiskLevel = RiskLevel.Medium;
            }

            return new RiskAssessment
            {
                OverallRiskLevel = overallRiskLevel,
                RiskItems = risks,
                TotalRiskCount = risks.Count,
                CriticalRiskCount = risks.Count(r => r.Level == RiskLevel.Critical),
                HighRiskCount = risks.Count(r => r.Level == RiskLevel.High),
                MediumRiskCount = risks.Count(r => r.Level == RiskLevel.Medium),
                LowRiskCount = risks.Count(r => r.Level == RiskLevel.Low)
            };
        }

        /// <summary>
        /// 分析性能
        /// </summary>
        private PerformanceInsights AnalyzePerformance(TestSessionDO session)
        {
            var testResults = session.TestResults;
            
            var performanceInsights = new PerformanceInsights
            {
                TotalTests = testResults.Count,
                TotalExecutionTime = session.TotalDurationMs,
                AverageTestDuration = testResults.Count > 0 ? 
                    session.TotalDurationMs / (double)testResults.Count : 0.0,
                FastestTest = testResults.OrderBy(r => r.DurationMs).FirstOrDefault(),
                SlowestTest = testResults.OrderByDescending(r => r.DurationMs).FirstOrDefault(),
                PerformanceIssues = new List<string>()
            };

            // 识别性能问题
            if (performanceInsights.AverageTestDuration > 500)
            {
                performanceInsights.PerformanceIssues.Add($"平均测试执行时间过长 ({performanceInsights.AverageTestDuration:F2} ms)");
            }

            var slowTests = testResults.Where(r => r.DurationMs > 1000).ToList();
            if (slowTests.Count > 0)
            {
                performanceInsights.PerformanceIssues.Add($"发现 {slowTests.Count} 个执行时间超过1秒的测试");
            }

            var verySlowTests = testResults.Where(r => r.DurationMs > 5000).ToList();
            if (verySlowTests.Count > 0)
            {
                performanceInsights.PerformanceIssues.Add($"发现 {verySlowTests.Count} 个执行时间超过5秒的测试，需要优化");
            }

            return performanceInsights;
        }

        /// <summary>
        /// 生成详细分析报告
        /// </summary>
        public string GenerateAnalysisReport(TestSessionAnalysis analysis)
        {
            var report = new System.Text.StringBuilder();
            
            report.AppendLine("=== 测试会话分析报告 ===");
            report.AppendLine($"会话ID: {analysis.SessionId}");
            report.AppendLine($"会话名称: {analysis.SessionName}");
            report.AppendLine($"分析时间: {analysis.AnalysisTime:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine();
            
            report.AppendLine("=== 整体健康状况 ===");
            report.AppendLine($"健康状况: {analysis.OverallHealth}");
            report.AppendLine();
            
            report.AppendLine("=== 关键指标 ===");
            report.AppendLine($"测试通过率: {analysis.KeyMetrics.PassRate:F2}%");
            report.AppendLine($"总测试数: {analysis.KeyMetrics.TotalTests}");
            report.AppendLine($"失败测试数: {analysis.KeyMetrics.FailedTests}");
            report.AppendLine($"跳过测试数: {analysis.KeyMetrics.SkippedTests}");
            report.AppendLine($"平均测试执行时间: {analysis.KeyMetrics.AverageTestDuration:F2} ms");
            report.AppendLine($"总执行时间: {analysis.KeyMetrics.TotalExecutionTime} ms");
            report.AppendLine();
            
            if (analysis.KeyMetrics.Coverage != null)
            {
                report.AppendLine("=== 覆盖率指标 ===");
                report.AppendLine($"行覆盖率: {analysis.KeyMetrics.Coverage.LineCoverage:F2}%");
                report.AppendLine($"分支覆盖率: {analysis.KeyMetrics.Coverage.BranchCoverage:F2}%");
                report.AppendLine($"方法覆盖率: {analysis.KeyMetrics.Coverage.MethodCoverage:F2}%");
                report.AppendLine($"类覆盖率: {analysis.KeyMetrics.Coverage.ClassCoverage:F2}%");
                report.AppendLine();
            }
            
            report.AppendLine("=== 风险评估 ===");
            report.AppendLine($"整体风险等级: {analysis.RiskAssessment.OverallRiskLevel}");
            report.AppendLine($"总风险项: {analysis.RiskAssessment.TotalRiskCount}");
            report.AppendLine($"严重风险: {analysis.RiskAssessment.CriticalRiskCount}");
            report.AppendLine($"高风险: {analysis.RiskAssessment.HighRiskCount}");
            report.AppendLine($"中等风险: {analysis.RiskAssessment.MediumRiskCount}");
            report.AppendLine($"低风险: {analysis.RiskAssessment.LowRiskCount}");
            report.AppendLine();
            
            if (analysis.RiskAssessment.RiskItems.Count > 0)
            {
                report.AppendLine("风险详情:");
                foreach (var risk in analysis.RiskAssessment.RiskItems)
                {
                    report.AppendLine($"  - [{risk.Level}] {risk.Category}: {risk.Description}");
                    report.AppendLine($"    影响: {risk.Impact}");
                    report.AppendLine($"    建议: {risk.Recommendation}");
                }
                report.AppendLine();
            }
            
            report.AppendLine("=== 改进建议 ===");
            foreach (var recommendation in analysis.Recommendations)
            {
                report.AppendLine($"- {recommendation}");
            }
            report.AppendLine();
            
            if (analysis.PerformanceInsights.PerformanceIssues.Count > 0)
            {
                report.AppendLine("=== 性能问题 ===");
                foreach (var issue in analysis.PerformanceInsights.PerformanceIssues)
                {
                    report.AppendLine($"- {issue}");
                }
                report.AppendLine();
            }
            
            return report.ToString();
        }
    }

    /// <summary>
    /// 测试会话分析结果
    /// </summary>
    public class TestSessionAnalysis
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// 会话名称
        /// </summary>
        public string SessionName { get; set; } = string.Empty;

        /// <summary>
        /// 分析时间
        /// </summary>
        public DateTime AnalysisTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 整体健康状况
        /// </summary>
        public HealthStatus OverallHealth { get; set; } = HealthStatus.Unknown;

        /// <summary>
        /// 关键指标
        /// </summary>
        public KeyMetrics KeyMetrics { get; set; } = new KeyMetrics();

        /// <summary>
        /// 趋势分析
        /// </summary>
        public TrendAnalysis TrendAnalysis { get; set; } = new TrendAnalysis();

        /// <summary>
        /// 改进建议
        /// </summary>
        public List<string> Recommendations { get; set; } = new List<string>();

        /// <summary>
        /// 风险评估
        /// </summary>
        public RiskAssessment RiskAssessment { get; set; } = new RiskAssessment();

        /// <summary>
        /// 性能洞察
        /// </summary>
        public PerformanceInsights PerformanceInsights { get; set; } = new PerformanceInsights();
    }

    /// <summary>
    /// 健康状况枚举
    /// </summary>
    public enum HealthStatus
    {
        Unknown = 0,
        Critical = 1,
        Poor = 2,
        Fair = 3,
        Good = 4,
        Excellent = 5
    }

    /// <summary>
    /// 关键指标
    /// </summary>
    public class KeyMetrics
    {
        /// <summary>
        /// 通过率
        /// </summary>
        public double PassRate { get; set; }

        /// <summary>
        /// 总测试数
        /// </summary>
        public int TotalTests { get; set; }

        /// <summary>
        /// 失败测试数
        /// </summary>
        public int FailedTests { get; set; }

        /// <summary>
        /// 跳过测试数
        /// </summary>
        public int SkippedTests { get; set; }

        /// <summary>
        /// 平均测试执行时间
        /// </summary>
        public double AverageTestDuration { get; set; }

        /// <summary>
        /// 总执行时间
        /// </summary>
        public long TotalExecutionTime { get; set; }

        /// <summary>
        /// 覆盖率指标
        /// </summary>
        public CoverageMetricsDO Coverage { get; set; } = new CoverageMetricsDO();

        /// <summary>
        /// 错误类别分布
        /// </summary>
        public Dictionary<string, int> ErrorCategories { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 最慢的测试
        /// </summary>
        public List<TestResultDO> SlowestTests { get; set; } = new List<TestResultDO>();

        /// <summary>
        /// 通过的质量门禁数
        /// </summary>
        public int QualityGatesPassed { get; set; }

        /// <summary>
        /// 失败的质量门禁数
        /// </summary>
        public int QualityGatesFailed { get; set; }
    }

    /// <summary>
    /// 趋势分析
    /// </summary>
    public class TrendAnalysis
    {
        /// <summary>
        /// 通过率趋势
        /// </summary>
        public TrendDirection PassRateTrend { get; set; } = TrendDirection.Stable;

        /// <summary>
        /// 覆盖率趋势
        /// </summary>
        public TrendDirection CoverageTrend { get; set; } = TrendDirection.Stable;

        /// <summary>
        /// 性能趋势
        /// </summary>
        public TrendDirection PerformanceTrend { get; set; } = TrendDirection.Stable;

        /// <summary>
        /// 可靠性趋势
        /// </summary>
        public TrendDirection ReliabilityTrend { get; set; } = TrendDirection.Stable;

        /// <summary>
        /// 说明备注
        /// </summary>
        public List<string> Notes { get; set; } = new List<string>();
    }

    /// <summary>
    /// 趋势方向枚举
    /// </summary>
    public enum TrendDirection
    {
        Stable = 0,
        Improving = 1,
        Declining = 2,
        Fluctuating = 3
    }

    /// <summary>
    /// 风险评估
    /// </summary>
    public class RiskAssessment
    {
        /// <summary>
        /// 整体风险等级
        /// </summary>
        public RiskLevel OverallRiskLevel { get; set; } = RiskLevel.Low;

        /// <summary>
        /// 风险项列表
        /// </summary>
        public List<RiskItem> RiskItems { get; set; } = new List<RiskItem>();

        /// <summary>
        /// 总风险项数
        /// </summary>
        public int TotalRiskCount { get; set; }

        /// <summary>
        /// 严重风险数
        /// </summary>
        public int CriticalRiskCount { get; set; }

        /// <summary>
        /// 高风险数
        /// </summary>
        public int HighRiskCount { get; set; }

        /// <summary>
        /// 中等风险数
        /// </summary>
        public int MediumRiskCount { get; set; }

        /// <summary>
        /// 低风险数
        /// </summary>
        public int LowRiskCount { get; set; }
    }

    /// <summary>
    /// 风险等级枚举
    /// </summary>
    public enum RiskLevel
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }

    /// <summary>
    /// 风险项
    /// </summary>
    public class RiskItem
    {
        /// <summary>
        /// 风险等级
        /// </summary>
        public RiskLevel Level { get; set; }

        /// <summary>
        /// 风险类别
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 风险描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 影响
        /// </summary>
        public string Impact { get; set; } = string.Empty;

        /// <summary>
        /// 建议
        /// </summary>
        public string Recommendation { get; set; } = string.Empty;
    }

    /// <summary>
    /// 性能洞察
    /// </summary>
    public class PerformanceInsights
    {
        /// <summary>
        /// 总测试数
        /// </summary>
        public int TotalTests { get; set; }

        /// <summary>
        /// 总执行时间
        /// </summary>
        public long TotalExecutionTime { get; set; }

        /// <summary>
        /// 平均测试执行时间
        /// </summary>
        public double AverageTestDuration { get; set; }

        /// <summary>
        /// 最快的测试
        /// </summary>
        public TestResultDO? FastestTest { get; set; }

        /// <summary>
        /// 最慢的测试
        /// </summary>
        public TestResultDO? SlowestTest { get; set; }

        /// <summary>
        /// 性能问题
        /// </summary>
        public List<string> PerformanceIssues { get; set; } = new List<string>();
    }
}