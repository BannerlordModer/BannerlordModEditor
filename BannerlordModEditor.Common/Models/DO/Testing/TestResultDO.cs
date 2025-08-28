using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Models.DO.Testing
{
    /// <summary>
    /// 测试执行结果领域对象
    /// </summary>
    [XmlRoot("test_result")]
    public class TestResultDO
    {
        /// <summary>
        /// 测试ID
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 测试名称
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 测试类型
        /// </summary>
        [XmlElement("type")]
        public string Type { get; set; } = "Unit";

        /// <summary>
        /// 测试类别
        /// </summary>
        [XmlElement("category")]
        public string Category { get; set; } = "Common";

        /// <summary>
        /// 执行状态
        /// </summary>
        [XmlElement("status")]
        public TestStatus Status { get; set; } = TestStatus.Pending;

        /// <summary>
        /// 开始时间
        /// </summary>
        [XmlElement("start_time")]
        public DateTime StartTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 结束时间
        /// </summary>
        [XmlElement("end_time")]
        public DateTime EndTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 执行持续时间（毫秒）
        /// </summary>
        [XmlElement("duration_ms")]
        public long DurationMs { get; set; } = 0;

        /// <summary>
        /// 错误消息
        /// </summary>
        [XmlElement("error_message")]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 错误堆栈
        /// </summary>
        [XmlElement("error_stack_trace")]
        public string? ErrorStackTrace { get; set; }

        /// <summary>
        /// 测试输出
        /// </summary>
        [XmlElement("output")]
        public string? Output { get; set; }

        /// <summary>
        /// 测试项目路径
        /// </summary>
        [XmlElement("project_path")]
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 测试方法全名
        /// </summary>
        [XmlElement("method_full_name")]
        public string MethodFullName { get; set; } = string.Empty;

        /// <summary>
        /// 测试文件路径
        /// </summary>
        [XmlElement("file_path")]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 行号
        /// </summary>
        [XmlElement("line_number")]
        public int LineNumber { get; set; } = 0;

        /// <summary>
        /// 是否应该序列化错误消息
        /// </summary>
        public bool ShouldSerializeErrorMessage() => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// 是否应该序列化错误堆栈
        /// </summary>
        public bool ShouldSerializeErrorStackTrace() => !string.IsNullOrEmpty(ErrorStackTrace);

        /// <summary>
        /// 是否应该序列化测试输出
        /// </summary>
        public bool ShouldSerializeOutput() => !string.IsNullOrEmpty(Output);

        /// <summary>
        /// 是否应该序列化开始时间
        /// </summary>
        public bool ShouldSerializeStartTime() => StartTime != DateTime.MinValue;

        /// <summary>
        /// 是否应该序列化结束时间
        /// </summary>
        public bool ShouldSerializeEndTime() => EndTime != DateTime.MinValue;
    }

    /// <summary>
    /// 测试状态枚举
    /// </summary>
    public enum TestStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        Running = 1,

        /// <summary>
        /// 通过
        /// </summary>
        Passed = 2,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 3,

        /// <summary>
        /// 跳过
        /// </summary>
        Skipped = 4,

        /// <summary>
        /// 取消
        /// </summary>
        Cancelled = 5
    }

    /// <summary>
    /// 测试覆盖率指标领域对象
    /// </summary>
    [XmlRoot("coverage_metrics")]
    public class CoverageMetricsDO
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [XmlElement("project_name")]
        public string ProjectName { get; set; } = string.Empty;

        /// <summary>
        /// 行覆盖率
        /// </summary>
        [XmlElement("line_coverage")]
        public double LineCoverage { get; set; } = 0.0;

        /// <summary>
        /// 分支覆盖率
        /// </summary>
        [XmlElement("branch_coverage")]
        public double BranchCoverage { get; set; } = 0.0;

        /// <summary>
        /// 方法覆盖率
        /// </summary>
        [XmlElement("method_coverage")]
        public double MethodCoverage { get; set; } = 0.0;

        /// <summary>
        /// 类覆盖率
        /// </summary>
        [XmlElement("class_coverage")]
        public double ClassCoverage { get; set; } = 0.0;

        /// <summary>
        /// 覆盖行数
        /// </summary>
        [XmlElement("covered_lines")]
        public int CoveredLines { get; set; } = 0;

        /// <summary>
        /// 总行数
        /// </summary>
        [XmlElement("total_lines")]
        public int TotalLines { get; set; } = 0;

        /// <summary>
        /// 覆盖分支数
        /// </summary>
        [XmlElement("covered_branches")]
        public int CoveredBranches { get; set; } = 0;

        /// <summary>
        /// 总分支数
        /// </summary>
        [XmlElement("total_branches")]
        public int TotalBranches { get; set; } = 0;

        /// <summary>
        /// 覆盖方法数
        /// </summary>
        [XmlElement("covered_methods")]
        public int CoveredMethods { get; set; } = 0;

        /// <summary>
        /// 总方法数
        /// </summary>
        [XmlElement("total_methods")]
        public int TotalMethods { get; set; } = 0;

        /// <summary>
        /// 覆盖类数
        /// </summary>
        [XmlElement("covered_classes")]
        public int CoveredClasses { get; set; } = 0;

        /// <summary>
        /// 总类数
        /// </summary>
        [XmlElement("total_classes")]
        public int TotalClasses { get; set; } = 0;

        /// <summary>
        /// 生成时间
        /// </summary>
        [XmlElement("generated_at")]
        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 覆盖率报告路径
        /// </summary>
        [XmlElement("report_path")]
        public string ReportPath { get; set; } = string.Empty;

        /// <summary>
        /// 覆盖率等级
        /// </summary>
        [XmlElement("coverage_grade")]
        public CoverageGrade CoverageGrade { get; set; } = CoverageGrade.Unknown;

        /// <summary>
        /// 计算覆盖率等级
        /// </summary>
        public void CalculateCoverageGrade()
        {
            var avgCoverage = (LineCoverage + BranchCoverage + MethodCoverage + ClassCoverage) / 4.0;
            
            if (avgCoverage >= 90.0)
                CoverageGrade = CoverageGrade.Excellent;
            else if (avgCoverage >= 80.0)
                CoverageGrade = CoverageGrade.Good;
            else if (avgCoverage >= 70.0)
                CoverageGrade = CoverageGrade.Average;
            else if (avgCoverage >= 60.0)
                CoverageGrade = CoverageGrade.Poor;
            else
                CoverageGrade = CoverageGrade.Critical;
        }

        /// <summary>
        /// 是否应该序列化覆盖率报告路径
        /// </summary>
        public bool ShouldSerializeReportPath() => !string.IsNullOrEmpty(ReportPath);
    }

    /// <summary>
    /// 覆盖率等级枚举
    /// </summary>
    public enum CoverageGrade
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 优秀
        /// </summary>
        Excellent = 1,

        /// <summary>
        /// 良好
        /// </summary>
        Good = 2,

        /// <summary>
        /// 一般
        /// </summary>
        Average = 3,

        /// <summary>
        /// 较差
        /// </summary>
        Poor = 4,

        /// <summary>
        /// 危险
        /// </summary>
        Critical = 5
    }

    /// <summary>
    /// 质量门禁状态领域对象
    /// </summary>
    [XmlRoot("quality_gate_status")]
    public class QualityGateStatusDO
    {
        /// <summary>
        /// 门禁ID
        /// </summary>
        [XmlElement("gate_id")]
        public string GateId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 门禁名称
        /// </summary>
        [XmlElement("gate_name")]
        public string GateName { get; set; } = string.Empty;

        /// <summary>
        /// 门禁类型
        /// </summary>
        [XmlElement("gate_type")]
        public QualityGateType GateType { get; set; } = QualityGateType.TestPassRate;

        /// <summary>
        /// 状态
        /// </summary>
        [XmlElement("status")]
        public QualityGateStatus Status { get; set; } = QualityGateStatus.Pending;

        /// <summary>
        /// 阈值
        /// </summary>
        [XmlElement("threshold")]
        public double Threshold { get; set; } = 0.0;

        /// <summary>
        /// 当前值
        /// </summary>
        [XmlElement("current_value")]
        public double CurrentValue { get; set; } = 0.0;

        /// <summary>
        /// 检查时间
        /// </summary>
        [XmlElement("checked_at")]
        public DateTime CheckedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 消息
        /// </summary>
        [XmlElement("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 详细信息
        /// </summary>
        [XmlElement("details")]
        public string Details { get; set; } = string.Empty;

        /// <summary>
        /// 是否通过
        /// </summary>
        [XmlIgnore]
        public bool IsPassed => Status == QualityGateStatus.Passed;

        /// <summary>
        /// 是否应该序列化详细信息
        /// </summary>
        public bool ShouldSerializeDetails() => !string.IsNullOrEmpty(Details);
    }

    /// <summary>
    /// 质量门禁类型枚举
    /// </summary>
    public enum QualityGateType
    {
        /// <summary>
        /// 测试通过率
        /// </summary>
        TestPassRate = 0,

        /// <summary>
        /// 代码覆盖率
        /// </summary>
        CodeCoverage = 1,

        /// <summary>
        /// 执行时间
        /// </summary>
        ExecutionTime = 2,

        /// <summary>
        /// 错误率
        /// </summary>
        ErrorRate = 3,

        /// <summary>
        /// 性能指标
        /// </summary>
        Performance = 4,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 5
    }

    /// <summary>
    /// 质量门禁状态枚举
    /// </summary>
    public enum QualityGateStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 检查中
        /// </summary>
        Checking = 1,

        /// <summary>
        /// 通过
        /// </summary>
        Passed = 2,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 3,

        /// <summary>
        /// 跳过
        /// </summary>
        Skipped = 4,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 5
    }

    /// <summary>
    /// 测试会话领域对象
    /// </summary>
    [XmlRoot("test_session")]
    public class TestSessionDO
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        [XmlElement("session_id")]
        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 会话名称
        /// </summary>
        [XmlElement("session_name")]
        public string SessionName { get; set; } = string.Empty;

        /// <summary>
        /// 开始时间
        /// </summary>
        [XmlElement("start_time")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        [XmlElement("end_time")]
        public DateTime EndTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 测试结果列表
        /// </summary>
        [XmlArray("test_results")]
        [XmlArrayItem("test_result")]
        public List<TestResultDO> TestResults { get; set; } = new List<TestResultDO>();

        /// <summary>
        /// 覆盖率指标
        /// </summary>
        [XmlElement("coverage_metrics")]
        public CoverageMetricsDO CoverageMetrics { get; set; } = new CoverageMetricsDO();

        /// <summary>
        /// 质量门禁状态列表
        /// </summary>
        [XmlArray("quality_gates")]
        [XmlArrayItem("quality_gate")]
        public List<QualityGateStatusDO> QualityGates { get; set; } = new List<QualityGateStatusDO>();

        /// <summary>
        /// 项目路径
        /// </summary>
        [XmlElement("project_path")]
        public string ProjectPath { get; set; } = string.Empty;

        /// <summary>
        /// 解决方案路径
        /// </summary>
        [XmlElement("solution_path")]
        public string SolutionPath { get; set; } = string.Empty;

        /// <summary>
        /// 构建配置
        /// </summary>
        [XmlElement("build_configuration")]
        public string BuildConfiguration { get; set; } = "Debug";

        /// <summary>
        /// 目标框架
        /// </summary>
        [XmlElement("target_framework")]
        public string TargetFramework { get; set; } = "net9.0";

        /// <summary>
        /// 总测试数
        /// </summary>
        [XmlElement("total_tests")]
        public int TotalTests { get; set; } = 0;

        /// <summary>
        /// 通过测试数
        /// </summary>
        [XmlElement("passed_tests")]
        public int PassedTests { get; set; } = 0;

        /// <summary>
        /// 失败测试数
        /// </summary>
        [XmlElement("failed_tests")]
        public int FailedTests { get; set; } = 0;

        /// <summary>
        /// 跳过测试数
        /// </summary>
        [XmlElement("skipped_tests")]
        public int SkippedTests { get; set; } = 0;

        /// <summary>
        /// 测试通过率
        /// </summary>
        [XmlElement("pass_rate")]
        public double PassRate { get; set; } = 0.0;

        /// <summary>
        /// 总执行时间（毫秒）
        /// </summary>
        [XmlElement("total_duration_ms")]
        public long TotalDurationMs { get; set; } = 0;

        /// <summary>
        /// 会话状态
        /// </summary>
        [XmlElement("session_status")]
        public TestSessionStatus SessionStatus { get; set; } = TestSessionStatus.Created;

        /// <summary>
        /// 是否应该序列化结束时间
        /// </summary>
        public bool ShouldSerializeEndTime() => EndTime != DateTime.MinValue;

        /// <summary>
        /// 计算测试统计信息
        /// </summary>
        public void CalculateStatistics()
        {
            TotalTests = TestResults.Count;
            PassedTests = TestResults.Count(r => r.Status == TestStatus.Passed);
            FailedTests = TestResults.Count(r => r.Status == TestStatus.Failed);
            SkippedTests = TestResults.Count(r => r.Status == TestStatus.Skipped);
            
            PassRate = TotalTests > 0 ? (PassedTests * 100.0 / TotalTests) : 0.0;
            TotalDurationMs = TestResults.Sum(r => r.DurationMs);
        }

        /// <summary>
        /// 获取测试结果状态分布
        /// </summary>
        public Dictionary<TestStatus, int> GetStatusDistribution()
        {
            return TestResults
                .GroupBy(r => r.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// 获取按类别分组的测试结果
        /// </summary>
        public Dictionary<string, List<TestResultDO>> GetResultsByCategory()
        {
            return TestResults
                .GroupBy(r => r.Category)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }

    /// <summary>
    /// 测试会话状态枚举
    /// </summary>
    public enum TestSessionStatus
    {
        /// <summary>
        /// 已创建
        /// </summary>
        Created = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        Running = 1,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 2,

        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 4
    }
}