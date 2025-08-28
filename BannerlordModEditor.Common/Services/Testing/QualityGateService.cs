using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Services.Testing
{
    /// <summary>
    /// 质量门禁服务
    /// 负责定义和检查质量门禁
    /// </summary>
    public class QualityGateService
    {
        private readonly object _lock = new();
        private readonly Dictionary<string, QualityGateDefinition> _gateDefinitions = new();
        private readonly QualityMonitoringService _qualityMonitoringService;

        /// <summary>
        /// 初始化质量门禁服务
        /// </summary>
        public QualityGateService(QualityMonitoringService qualityMonitoringService)
        {
            _qualityMonitoringService = qualityMonitoringService ?? throw new ArgumentNullException(nameof(qualityMonitoringService));
            InitializeDefaultGates();
        }

        /// <summary>
        /// 初始化默认质量门禁
        /// </summary>
        private void InitializeDefaultGates()
        {
            lock (_lock)
            {
                // 测试通过率门禁
                _gateDefinitions["test_pass_rate"] = new QualityGateDefinition
                {
                    Id = "test_pass_rate",
                    Name = "测试通过率",
                    Description = "确保测试通过率达到目标",
                    Type = QualityGateType.TestPassRate,
                    Threshold = 80.0,
                    Operator = ComparisonOperator.GreaterThanOrEqual,
                    Severity = RiskLevel.High,
                    Enabled = true,
                    MessageTemplate = "测试通过率 {current_value:F2}% 低于目标 {threshold:F2}%",
                    SuccessMessage = "测试通过率达到目标 {current_value:F2}%"
                };

                // 代码覆盖率门禁
                _gateDefinitions["code_coverage"] = new QualityGateDefinition
                {
                    Id = "code_coverage",
                    Name = "代码覆盖率",
                    Description = "确保代码覆盖率达到目标",
                    Type = QualityGateType.CodeCoverage,
                    Threshold = 70.0,
                    Operator = ComparisonOperator.GreaterThanOrEqual,
                    Severity = RiskLevel.Medium,
                    Enabled = true,
                    MessageTemplate = "代码覆盖率 {current_value:F2}% 低于目标 {threshold:F2}%",
                    SuccessMessage = "代码覆盖率达到目标 {current_value:F2}%"
                };

                // 行覆盖率门禁
                _gateDefinitions["line_coverage"] = new QualityGateDefinition
                {
                    Id = "line_coverage",
                    Name = "行覆盖率",
                    Description = "确保行覆盖率达到目标",
                    Type = QualityGateType.CodeCoverage,
                    Threshold = 70.0,
                    Operator = ComparisonOperator.GreaterThanOrEqual,
                    Severity = RiskLevel.Medium,
                    Enabled = true,
                    MessageTemplate = "行覆盖率 {current_value:F2}% 低于目标 {threshold:F2}%",
                    SuccessMessage = "行覆盖率达到目标 {current_value:F2}%"
                };

                // 分支覆盖率门禁
                _gateDefinitions["branch_coverage"] = new QualityGateDefinition
                {
                    Id = "branch_coverage",
                    Name = "分支覆盖率",
                    Description = "确保分支覆盖率达到目标",
                    Type = QualityGateType.CodeCoverage,
                    Threshold = 60.0,
                    Operator = ComparisonOperator.GreaterThanOrEqual,
                    Severity = RiskLevel.Medium,
                    Enabled = true,
                    MessageTemplate = "分支覆盖率 {current_value:F2}% 低于目标 {threshold:F2}%",
                    SuccessMessage = "分支覆盖率达到目标 {current_value:F2}%"
                };

                // 执行时间门禁
                _gateDefinitions["execution_time"] = new QualityGateDefinition
                {
                    Id = "execution_time",
                    Name = "执行时间",
                    Description = "确保测试执行时间在可接受范围内",
                    Type = QualityGateType.ExecutionTime,
                    Threshold = 300000, // 5分钟
                    Operator = ComparisonOperator.LessThanOrEqual,
                    Severity = RiskLevel.Low,
                    Enabled = true,
                    MessageTemplate = "测试执行时间 {current_value:F0} ms 超过目标 {threshold:F0} ms",
                    SuccessMessage = "测试执行时间在目标范围内 {current_value:F0} ms"
                };

                // 错误率门禁
                _gateDefinitions["error_rate"] = new QualityGateDefinition
                {
                    Id = "error_rate",
                    Name = "错误率",
                    Description = "确保测试错误率低于目标",
                    Type = QualityGateType.ErrorRate,
                    Threshold = 5.0,
                    Operator = ComparisonOperator.LessThanOrEqual,
                    Severity = RiskLevel.High,
                    Enabled = true,
                    MessageTemplate = "测试错误率 {current_value:F2}% 超过目标 {threshold:F2}%",
                    SuccessMessage = "测试错误率在目标范围内 {current_value:F2}%"
                };
            }
        }

        /// <summary>
        /// 检查测试会话的质量门禁
        /// </summary>
        public async Task<QualityGateCheckResult> CheckQualityGatesAsync(TestSessionDO session)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var result = new QualityGateCheckResult
                {
                    SessionId = session.SessionId,
                    CheckTime = DateTime.Now,
                    GateStatuses = new List<QualityGateStatusDO>(),
                    OverallStatus = QualityGateStatus.Passed
                };

                lock (_lock)
                {
                    var enabledGates = _gateDefinitions.Values.Where(g => g.Enabled).ToList();

                    foreach (var gateDefinition in enabledGates)
                    {
                        var gateStatus = CheckSingleGate(gateDefinition, session);
                        result.GateStatuses.Add(gateStatus);

                        if (gateStatus.Status == QualityGateStatus.Failed)
                        {
                            result.OverallStatus = QualityGateStatus.Failed;
                        }
                    }
                }

                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("CheckQualityGates", stopwatch.ElapsedMilliseconds, true);
                
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _qualityMonitoringService.RecordOperation("CheckQualityGates", stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 检查单个质量门禁
        /// </summary>
        private QualityGateStatusDO CheckSingleGate(QualityGateDefinition gate, TestSessionDO session)
        {
            var gateStatus = new QualityGateStatusDO
            {
                GateId = gate.Id,
                GateName = gate.Name,
                GateType = gate.Type,
                Threshold = gate.Threshold,
                CheckedAt = DateTime.Now,
                Status = QualityGateStatus.Checking
            };

            try
            {
                double currentValue = GetGateValue(gate, session);
                gateStatus.CurrentValue = currentValue;

                bool passed = EvaluateGateCondition(gate, currentValue);
                gateStatus.Status = passed ? QualityGateStatus.Passed : QualityGateStatus.Failed;

                // 生成消息
                if (passed)
                {
                    gateStatus.Message = FormatMessage(gate.SuccessMessage, currentValue, gate.Threshold);
                }
                else
                {
                    gateStatus.Message = FormatMessage(gate.MessageTemplate, currentValue, gate.Threshold);
                }

                // 生成详细信息
                gateStatus.Details = GenerateGateDetails(gate, session, currentValue);
            }
            catch (Exception ex)
            {
                gateStatus.Status = QualityGateStatus.Error;
                gateStatus.Message = $"检查质量门禁时发生错误: {ex.Message}";
                gateStatus.Details = $"错误类型: {ex.GetType().Name}\n错误详情: {ex.StackTrace}";
            }

            return gateStatus;
        }

        /// <summary>
        /// 获取门禁值
        /// </summary>
        private double GetGateValue(QualityGateDefinition gate, TestSessionDO session)
        {
            switch (gate.Type)
            {
                case QualityGateType.TestPassRate:
                    return session.PassRate;

                case QualityGateType.CodeCoverage:
                    // 计算平均覆盖率
                    return (session.CoverageMetrics.LineCoverage +
                           session.CoverageMetrics.BranchCoverage +
                           session.CoverageMetrics.MethodCoverage +
                           session.CoverageMetrics.ClassCoverage) / 4.0;

                case QualityGateType.ExecutionTime:
                    return session.TotalDurationMs;

                case QualityGateType.ErrorRate:
                    return session.TotalTests > 0 ? (session.FailedTests * 100.0 / session.TotalTests) : 0.0;

                case QualityGateType.Performance:
                    // 计算平均测试执行时间
                    return session.TotalTests > 0 ? (session.TotalDurationMs / (double)session.TotalTests) : 0.0;

                default:
                    throw new NotSupportedException($"不支持的质量门禁类型: {gate.Type}");
            }
        }

        /// <summary>
        /// 评估门禁条件
        /// </summary>
        private bool EvaluateGateCondition(QualityGateDefinition gate, double currentValue)
        {
            switch (gate.Operator)
            {
                case ComparisonOperator.Equal:
                    return Math.Abs(currentValue - gate.Threshold) < 0.001;

                case ComparisonOperator.NotEqual:
                    return Math.Abs(currentValue - gate.Threshold) >= 0.001;

                case ComparisonOperator.GreaterThan:
                    return currentValue > gate.Threshold;

                case ComparisonOperator.GreaterThanOrEqual:
                    return currentValue >= gate.Threshold;

                case ComparisonOperator.LessThan:
                    return currentValue < gate.Threshold;

                case ComparisonOperator.LessThanOrEqual:
                    return currentValue <= gate.Threshold;

                default:
                    throw new NotSupportedException($"不支持的比较操作符: {gate.Operator}");
            }
        }

        /// <summary>
        /// 格式化消息
        /// </summary>
        private string FormatMessage(string template, double currentValue, double threshold)
        {
            return template
                .Replace("{current_value}", currentValue.ToString("F2"))
                .Replace("{threshold}", threshold.ToString("F2"));
        }

        /// <summary>
        /// 生成门禁详细信息
        /// </summary>
        private string GenerateGateDetails(QualityGateDefinition gate, TestSessionDO session, double currentValue)
        {
            var details = new System.Text.StringBuilder();
            
            details.AppendLine($"门禁定义: {gate.Name}");
            details.AppendLine($"门禁类型: {gate.Type}");
            details.AppendLine($"操作符: {gate.Operator}");
            details.AppendLine($"当前值: {currentValue:F2}");
            details.AppendLine($"阈值: {gate.Threshold:F2}");
            details.AppendLine($"严重程度: {gate.Severity}");
            details.AppendLine();

            switch (gate.Type)
            {
                case QualityGateType.TestPassRate:
                    details.AppendLine($"测试统计:");
                    details.AppendLine($"  总测试数: {session.TotalTests}");
                    details.AppendLine($"  通过测试数: {session.PassedTests}");
                    details.AppendLine($"  失败测试数: {session.FailedTests}");
                    details.AppendLine($"  跳过测试数: {session.SkippedTests}");
                    break;

                case QualityGateType.CodeCoverage:
                    details.AppendLine($"覆盖率详情:");
                    details.AppendLine($"  行覆盖率: {session.CoverageMetrics.LineCoverage:F2}%");
                    details.AppendLine($"  分支覆盖率: {session.CoverageMetrics.BranchCoverage:F2}%");
                    details.AppendLine($"  方法覆盖率: {session.CoverageMetrics.MethodCoverage:F2}%");
                    details.AppendLine($"  类覆盖率: {session.CoverageMetrics.ClassCoverage:F2}%");
                    break;

                case QualityGateType.ExecutionTime:
                    details.AppendLine($"执行时间详情:");
                    details.AppendLine($"  总执行时间: {session.TotalDurationMs} ms");
                    details.AppendLine($"  平均测试时间: {(session.TotalTests > 0 ? (session.TotalDurationMs / (double)session.TotalTests) : 0):F2} ms");
                    break;

                case QualityGateType.ErrorRate:
                    details.AppendLine($"错误统计:");
                    details.AppendLine($"  错误率: {currentValue:F2}%");
                    details.AppendLine($"  失败测试数: {session.FailedTests}");
                    details.AppendLine($"  总测试数: {session.TotalTests}");
                    break;
            }

            return details.ToString();
        }

        /// <summary>
        /// 添加自定义质量门禁
        /// </summary>
        public void AddQualityGate(QualityGateDefinition gate)
        {
            lock (_lock)
            {
                _gateDefinitions[gate.Id] = gate;
                _qualityMonitoringService.RecordOperation("AddQualityGate", 0, true);
            }
        }

        /// <summary>
        /// 更新质量门禁
        /// </summary>
        public void UpdateQualityGate(string gateId, Action<QualityGateDefinition> updateAction)
        {
            lock (_lock)
            {
                if (_gateDefinitions.TryGetValue(gateId, out var gate))
                {
                    updateAction(gate);
                    _qualityMonitoringService.RecordOperation("UpdateQualityGate", 0, true);
                }
                else
                {
                    throw new KeyNotFoundException($"质量门禁 '{gateId}' 不存在");
                }
            }
        }

        /// <summary>
        /// 删除质量门禁
        /// </summary>
        public void RemoveQualityGate(string gateId)
        {
            lock (_lock)
            {
                if (_gateDefinitions.Remove(gateId))
                {
                    _qualityMonitoringService.RecordOperation("RemoveQualityGate", 0, true);
                }
                else
                {
                    throw new KeyNotFoundException($"质量门禁 '{gateId}' 不存在");
                }
            }
        }

        /// <summary>
        /// 启用/禁用质量门禁
        /// </summary>
        public void SetQualityGateEnabled(string gateId, bool enabled)
        {
            UpdateQualityGate(gateId, gate => gate.Enabled = enabled);
        }

        /// <summary>
        /// 获取质量门禁定义
        /// </summary>
        public QualityGateDefinition? GetQualityGateDefinition(string gateId)
        {
            lock (_lock)
            {
                _gateDefinitions.TryGetValue(gateId, out var gate);
                return gate;
            }
        }

        /// <summary>
        /// 获取所有质量门禁定义
        /// </summary>
        public List<QualityGateDefinition> GetAllQualityGateDefinitions()
        {
            lock (_lock)
            {
                return _gateDefinitions.Values.ToList();
            }
        }

        /// <summary>
        /// 获取启用的质量门禁定义
        /// </summary>
        public List<QualityGateDefinition> GetEnabledQualityGateDefinitions()
        {
            lock (_lock)
            {
                return _gateDefinitions.Values.Where(g => g.Enabled).ToList();
            }
        }

        /// <summary>
        /// 验证质量门禁配置
        /// </summary>
        public List<string> ValidateQualityGateConfiguration()
        {
            var validationErrors = new List<string>();

            lock (_lock)
            {
                foreach (var gate in _gateDefinitions.Values)
                {
                    if (string.IsNullOrWhiteSpace(gate.Id))
                    {
                        validationErrors.Add($"质量门禁 '{gate.Name}' 的ID不能为空");
                    }

                    if (string.IsNullOrWhiteSpace(gate.Name))
                    {
                        validationErrors.Add($"质量门禁ID '{gate.Id}' 的名称不能为空");
                    }

                    if (gate.Threshold < 0)
                    {
                        validationErrors.Add($"质量门禁 '{gate.Name}' 的阈值不能为负数");
                    }

                    if (gate.Type == QualityGateType.TestPassRate && gate.Threshold > 100)
                    {
                        validationErrors.Add($"质量门禁 '{gate.Name}' 的通过率阈值不能超过100%");
                    }

                    if (gate.Type == QualityGateType.CodeCoverage && gate.Threshold > 100)
                    {
                        validationErrors.Add($"质量门禁 '{gate.Name}' 的覆盖率阈值不能超过100%");
                    }
                }
            }

            return validationErrors;
        }

        /// <summary>
        /// 导出质量门禁配置
        /// </summary>
        public string ExportQualityGateConfiguration()
        {
            var config = new System.Text.StringBuilder();
            
            config.AppendLine("=== 质量门禁配置 ===");
            config.AppendLine($"导出时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            config.AppendLine();

            lock (_lock)
            {
                foreach (var gate in _gateDefinitions.Values.OrderBy(g => g.Name))
                {
                    config.AppendLine($"门禁: {gate.Name}");
                    config.AppendLine($"  ID: {gate.Id}");
                    config.AppendLine($"  类型: {gate.Type}");
                    config.AppendLine($"  描述: {gate.Description}");
                    config.AppendLine($"  阈值: {gate.Threshold}");
                    config.AppendLine($"  操作符: {gate.Operator}");
                    config.AppendLine($"  严重程度: {gate.Severity}");
                    config.AppendLine($"  启用状态: {gate.Enabled}");
                    config.AppendLine($"  失败消息: {gate.MessageTemplate}");
                    config.AppendLine($"  成功消息: {gate.SuccessMessage}");
                    config.AppendLine();
                }
            }

            return config.ToString();
        }
    }

    /// <summary>
    /// 质量门禁定义
    /// </summary>
    public class QualityGateDefinition
    {
        /// <summary>
        /// 门禁ID
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 门禁名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 门禁描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 门禁类型
        /// </summary>
        public QualityGateType Type { get; set; } = QualityGateType.TestPassRate;

        /// <summary>
        /// 阈值
        /// </summary>
        public double Threshold { get; set; }

        /// <summary>
        /// 比较操作符
        /// </summary>
        public ComparisonOperator Operator { get; set; } = ComparisonOperator.GreaterThanOrEqual;

        /// <summary>
        /// 严重程度
        /// </summary>
        public RiskLevel Severity { get; set; } = RiskLevel.Medium;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 失败消息模板
        /// </summary>
        public string MessageTemplate { get; set; } = string.Empty;

        /// <summary>
        /// 成功消息模板
        /// </summary>
        public string SuccessMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// 比较操作符枚举
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 0,

        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 1,

        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 2,

        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqual = 3,

        /// <summary>
        /// 小于
        /// </summary>
        LessThan = 4,

        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqual = 5
    }

    /// <summary>
    /// 质量门禁检查结果
    /// </summary>
    public class QualityGateCheckResult
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime CheckTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 门禁状态列表
        /// </summary>
        public List<QualityGateStatusDO> GateStatuses { get; set; } = new List<QualityGateStatusDO>();

        /// <summary>
        /// 整体状态
        /// </summary>
        public QualityGateStatus OverallStatus { get; set; } = QualityGateStatus.Passed;

        /// <summary>
        /// 是否通过所有门禁
        /// </summary>
        public bool AllPassed => OverallStatus == QualityGateStatus.Passed;

        /// <summary>
        /// 通过的门禁数
        /// </summary>
        public int PassedGates => GateStatuses.Count(g => g.Status == QualityGateStatus.Passed);

        /// <summary>
        /// 失败的门禁数
        /// </summary>
        public int FailedGates => GateStatuses.Count(g => g.Status == QualityGateStatus.Failed);

        /// <summary>
        /// 错误的门禁数
        /// </summary>
        public int ErrorGates => GateStatuses.Count(g => g.Status == QualityGateStatus.Error);
    }
}