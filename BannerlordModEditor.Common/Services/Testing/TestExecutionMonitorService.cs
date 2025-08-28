using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Services.Testing
{
    /// <summary>
    /// 测试执行监控服务
    /// 负责监控和记录测试执行过程
    /// </summary>
    public class TestExecutionMonitorService
    {
        private readonly object _lock = new();
        private readonly Dictionary<string, TestSessionDO> _activeSessions = new();
        private readonly List<TestSessionDO> _completedSessions = new();
        private readonly QualityMonitoringService _qualityMonitoringService;

        /// <summary>
        /// 初始化测试执行监控服务
        /// </summary>
        public TestExecutionMonitorService(QualityMonitoringService qualityMonitoringService)
        {
            _qualityMonitoringService = qualityMonitoringService ?? throw new ArgumentNullException(nameof(qualityMonitoringService));
        }

        /// <summary>
        /// 创建新的测试会话
        /// </summary>
        public TestSessionDO CreateTestSession(string sessionName, string projectPath, string solutionPath)
        {
            lock (_lock)
            {
                var session = new TestSessionDO
                {
                    SessionId = Guid.NewGuid().ToString(),
                    SessionName = sessionName,
                    ProjectPath = projectPath,
                    SolutionPath = solutionPath,
                    StartTime = DateTime.Now,
                    SessionStatus = TestSessionStatus.Created
                };

                _activeSessions[session.SessionId] = session;
                
                _qualityMonitoringService.RecordOperation("CreateTestSession", 0, true);
                
                return session;
            }
        }

        /// <summary>
        /// 开始执行测试会话
        /// </summary>
        public async Task StartTestSessionAsync(string sessionId)
        {
            lock (_lock)
            {
                if (!_activeSessions.TryGetValue(sessionId, out var session))
                {
                    throw new KeyNotFoundException($"Test session with ID {sessionId} not found");
                }

                session.SessionStatus = TestSessionStatus.Running;
                session.StartTime = DateTime.Now;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 记录测试结果
        /// </summary>
        public void RecordTestResult(string sessionId, TestResultDO testResult)
        {
            lock (_lock)
            {
                if (!_activeSessions.TryGetValue(sessionId, out var session))
                {
                    throw new KeyNotFoundException($"Test session with ID {sessionId} not found");
                }

                testResult.Id = Guid.NewGuid().ToString();
                session.TestResults.Add(testResult);
                
                // 更新质量监控
                var operationName = $"Test_{testResult.Type}_{testResult.Category}";
                var success = testResult.Status == TestStatus.Passed;
                var errorMessage = testResult.ErrorMessage;
                
                _qualityMonitoringService.RecordOperation(operationName, testResult.DurationMs, success, errorMessage);
            }
        }

        /// <summary>
        /// 更新测试覆盖率指标
        /// </summary>
        public void UpdateCoverageMetrics(string sessionId, CoverageMetricsDO coverageMetrics)
        {
            lock (_lock)
            {
                if (!_activeSessions.TryGetValue(sessionId, out var session))
                {
                    throw new KeyNotFoundException($"Test session with ID {sessionId} not found");
                }

                session.CoverageMetrics = coverageMetrics;
                coverageMetrics.CalculateCoverageGrade();
                
                // 记录覆盖率操作
                _qualityMonitoringService.RecordOperation("UpdateCoverageMetrics", 0, true);
            }
        }

        /// <summary>
        /// 完成测试会话
        /// </summary>
        public async Task CompleteTestSessionAsync(string sessionId)
        {
            lock (_lock)
            {
                if (!_activeSessions.TryGetValue(sessionId, out var session))
                {
                    throw new KeyNotFoundException($"Test session with ID {sessionId} not found");
                }

                session.EndTime = DateTime.Now;
                session.SessionStatus = TestSessionStatus.Completed;
                session.CalculateStatistics();

                // 移动到已完成会话列表
                _activeSessions.Remove(sessionId);
                _completedSessions.Add(session);
                
                // 记录会话完成操作
                _qualityMonitoringService.RecordOperation("CompleteTestSession", 
                    (session.EndTime - session.StartTime).TotalMilliseconds, true);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 取消测试会话
        /// </summary>
        public async Task CancelTestSessionAsync(string sessionId)
        {
            lock (_lock)
            {
                if (!_activeSessions.TryGetValue(sessionId, out var session))
                {
                    throw new KeyNotFoundException($"Test session with ID {sessionId} not found");
                }

                session.EndTime = DateTime.Now;
                session.SessionStatus = TestSessionStatus.Cancelled;
                session.CalculateStatistics();

                // 移动到已完成会话列表
                _activeSessions.Remove(sessionId);
                _completedSessions.Add(session);
                
                // 记录会话取消操作
                _qualityMonitoringService.RecordOperation("CancelTestSession", 
                    (session.EndTime - session.StartTime).TotalMilliseconds, false, "Test session cancelled");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取活动测试会话
        /// </summary>
        public TestSessionDO? GetActiveSession(string sessionId)
        {
            lock (_lock)
            {
                _activeSessions.TryGetValue(sessionId, out var session);
                return session;
            }
        }

        /// <summary>
        /// 获取所有活动测试会话
        /// </summary>
        public List<TestSessionDO> GetActiveSessions()
        {
            lock (_lock)
            {
                return _activeSessions.Values.ToList();
            }
        }

        /// <summary>
        /// 获取已完成测试会话
        /// </summary>
        public List<TestSessionDO> GetCompletedSessions()
        {
            lock (_lock)
            {
                return _completedSessions.ToList();
            }
        }

        /// <summary>
        /// 获取测试会话状态
        /// </summary>
        public TestSessionStatus GetSessionStatus(string sessionId)
        {
            lock (_lock)
            {
                if (_activeSessions.TryGetValue(sessionId, out var activeSession))
                {
                    return activeSession.SessionStatus;
                }

                var completedSession = _completedSessions.FirstOrDefault(s => s.SessionId == sessionId);
                return completedSession?.SessionStatus ?? TestSessionStatus.Created;
            }
        }

        /// <summary>
        /// 获取测试执行统计信息
        /// </summary>
        public TestExecutionStatistics GetExecutionStatistics()
        {
            lock (_lock)
            {
                var allSessions = _activeSessions.Values.Concat(_completedSessions).ToList();
                
                return new TestExecutionStatistics
                {
                    TotalSessions = allSessions.Count,
                    ActiveSessions = _activeSessions.Count,
                    CompletedSessions = _completedSessions.Count,
                    TotalTests = allSessions.Sum(s => s.TotalTests),
                    PassedTests = allSessions.Sum(s => s.PassedTests),
                    FailedTests = allSessions.Sum(s => s.FailedTests),
                    SkippedTests = allSessions.Sum(s => s.SkippedTests),
                    AveragePassRate = allSessions.Count > 0 ? 
                        allSessions.Average(s => s.PassRate) : 0.0,
                    TotalExecutionTime = allSessions.Sum(s => s.TotalDurationMs),
                    LastActivity = allSessions.OrderByDescending(s => s.EndTime != DateTime.MinValue ? s.EndTime : s.StartTime)
                        .FirstOrDefault()?.EndTime ?? DateTime.MinValue
                };
            }
        }

        /// <summary>
        /// 清理旧的测试会话
        /// </summary>
        public void CleanupOldSessions(int maxSessionsToKeep = 100)
        {
            lock (_lock)
            {
                if (_completedSessions.Count > maxSessionsToKeep)
                {
                    var sessionsToRemove = _completedSessions
                        .OrderBy(s => s.EndTime)
                        .Take(_completedSessions.Count - maxSessionsToKeep)
                        .ToList();

                    foreach (var session in sessionsToRemove)
                    {
                        _completedSessions.Remove(session);
                    }
                }
            }
        }

        /// <summary>
        /// 导出测试会话数据
        /// </summary>
        public string ExportSessionData(string sessionId)
        {
            lock (_lock)
            {
                TestSessionDO? session = null;
                
                if (_activeSessions.TryGetValue(sessionId, out var activeSession))
                {
                    session = activeSession;
                }
                else
                {
                    session = _completedSessions.FirstOrDefault(s => s.SessionId == sessionId);
                }

                if (session == null)
                {
                    throw new KeyNotFoundException($"Test session with ID {sessionId} not found");
                }

                return GenerateSessionReport(session);
            }
        }

        /// <summary>
        /// 生成测试会话报告
        /// </summary>
        private string GenerateSessionReport(TestSessionDO session)
        {
            var report = new System.Text.StringBuilder();
            
            report.AppendLine("=== 测试会话报告 ===");
            report.AppendLine($"会话ID: {session.SessionId}");
            report.AppendLine($"会话名称: {session.SessionName}");
            report.AppendLine($"项目路径: {session.ProjectPath}");
            report.AppendLine($"解决方案路径: {session.SolutionPath}");
            report.AppendLine($"开始时间: {session.StartTime:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine($"结束时间: {session.EndTime:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine($"会话状态: {session.SessionStatus}");
            report.AppendLine($"构建配置: {session.BuildConfiguration}");
            report.AppendLine($"目标框架: {session.TargetFramework}");
            report.AppendLine();
            
            report.AppendLine("=== 测试统计 ===");
            report.AppendLine($"总测试数: {session.TotalTests}");
            report.AppendLine($"通过测试数: {session.PassedTests}");
            report.AppendLine($"失败测试数: {session.FailedTests}");
            report.AppendLine($"跳过测试数: {session.SkippedTests}");
            report.AppendLine($"测试通过率: {session.PassRate:F2}%");
            report.AppendLine($"总执行时间: {session.TotalDurationMs} ms");
            report.AppendLine();
            
            if (session.CoverageMetrics != null)
            {
                report.AppendLine("=== 覆盖率指标 ===");
                report.AppendLine($"项目名称: {session.CoverageMetrics.ProjectName}");
                report.AppendLine($"行覆盖率: {session.CoverageMetrics.LineCoverage:F2}%");
                report.AppendLine($"分支覆盖率: {session.CoverageMetrics.BranchCoverage:F2}%");
                report.AppendLine($"方法覆盖率: {session.CoverageMetrics.MethodCoverage:F2}%");
                report.AppendLine($"类覆盖率: {session.CoverageMetrics.ClassCoverage:F2}%");
                report.AppendLine($"覆盖率等级: {session.CoverageMetrics.CoverageGrade}");
                report.AppendLine($"报告路径: {session.CoverageMetrics.ReportPath}");
                report.AppendLine();
            }
            
            if (session.QualityGates.Count > 0)
            {
                report.AppendLine("=== 质量门禁 ===");
                foreach (var gate in session.QualityGates)
                {
                    report.AppendLine($"门禁名称: {gate.GateName}");
                    report.AppendLine($"门禁类型: {gate.GateType}");
                    report.AppendLine($"状态: {gate.Status}");
                    report.AppendLine($"阈值: {gate.Threshold}");
                    report.AppendLine($"当前值: {gate.CurrentValue}");
                    report.AppendLine($"消息: {gate.Message}");
                    report.AppendLine();
                }
            }
            
            report.AppendLine("=== 测试结果详情 ===");
            var statusGroups = session.TestResults.GroupBy(r => r.Status);
            foreach (var group in statusGroups)
            {
                report.AppendLine($"{group.Key} 测试 ({group.Count()} 个):");
                foreach (var result in group)
                {
                    report.AppendLine($"  - {result.Name} ({result.DurationMs} ms)");
                    if (result.Status == TestStatus.Failed && !string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        report.AppendLine($"    错误: {result.ErrorMessage}");
                    }
                }
                report.AppendLine();
            }
            
            return report.ToString();
        }
    }

    /// <summary>
    /// 测试执行统计信息
    /// </summary>
    public class TestExecutionStatistics
    {
        /// <summary>
        /// 总会话数
        /// </summary>
        public int TotalSessions { get; set; }

        /// <summary>
        /// 活动会话数
        /// </summary>
        public int ActiveSessions { get; set; }

        /// <summary>
        /// 已完成会话数
        /// </summary>
        public int CompletedSessions { get; set; }

        /// <summary>
        /// 总测试数
        /// </summary>
        public int TotalTests { get; set; }

        /// <summary>
        /// 通过测试数
        /// </summary>
        public int PassedTests { get; set; }

        /// <summary>
        /// 失败测试数
        /// </summary>
        public int FailedTests { get; set; }

        /// <summary>
        /// 跳过测试数
        /// </summary>
        public int SkippedTests { get; set; }

        /// <summary>
        /// 平均通过率
        /// </summary>
        public double AveragePassRate { get; set; }

        /// <summary>
        /// 总执行时间（毫秒）
        /// </summary>
        public long TotalExecutionTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime LastActivity { get; set; }
    }
}