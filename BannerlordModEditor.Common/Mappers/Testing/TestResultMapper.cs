using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Models.DTO.Testing;

namespace BannerlordModEditor.Common.Mappers.Testing
{
    /// <summary>
    /// 测试结果映射器
    /// </summary>
    public static class TestResultMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static TestResultDTO ToDTO(TestResultDO source)
        {
            if (source == null) return null;

            return new TestResultDTO
            {
                Id = source.Id,
                Name = source.Name,
                Type = source.Type,
                Category = source.Category,
                Status = source.Status,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                DurationMs = source.DurationMs,
                ErrorMessage = source.ErrorMessage,
                ErrorStackTrace = source.ErrorStackTrace,
                Output = source.Output,
                ProjectPath = source.ProjectPath,
                MethodFullName = source.MethodFullName,
                FilePath = source.FilePath,
                LineNumber = source.LineNumber
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static TestResultDO ToDO(TestResultDTO source)
        {
            if (source == null) return null;

            return new TestResultDO
            {
                Id = source.Id,
                Name = source.Name,
                Type = source.Type,
                Category = source.Category,
                Status = source.Status,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                DurationMs = source.DurationMs,
                ErrorMessage = source.ErrorMessage,
                ErrorStackTrace = source.ErrorStackTrace,
                Output = source.Output,
                ProjectPath = source.ProjectPath,
                MethodFullName = source.MethodFullName,
                FilePath = source.FilePath,
                LineNumber = source.LineNumber
            };
        }

        /// <summary>
        /// 将领域对象列表转换为数据传输对象列表
        /// </summary>
        public static List<TestResultDTO> ToDTOList(List<TestResultDO> source)
        {
            return source?.Select(ToDTO).Where(x => x != null).ToList() ?? new List<TestResultDTO>();
        }

        /// <summary>
        /// 将数据传输对象列表转换为领域对象列表
        /// </summary>
        public static List<TestResultDO> ToDOList(List<TestResultDTO> source)
        {
            return source?.Select(ToDO).Where(x => x != null).ToList() ?? new List<TestResultDO>();
        }
    }

    /// <summary>
    /// 覆盖率指标映射器
    /// </summary>
    public static class CoverageMetricsMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static CoverageMetricsDTO ToDTO(CoverageMetricsDO source)
        {
            if (source == null) return null;

            return new CoverageMetricsDTO
            {
                ProjectName = source.ProjectName,
                LineCoverage = source.LineCoverage,
                BranchCoverage = source.BranchCoverage,
                MethodCoverage = source.MethodCoverage,
                ClassCoverage = source.ClassCoverage,
                CoveredLines = source.CoveredLines,
                TotalLines = source.TotalLines,
                CoveredBranches = source.CoveredBranches,
                TotalBranches = source.TotalBranches,
                CoveredMethods = source.CoveredMethods,
                TotalMethods = source.TotalMethods,
                CoveredClasses = source.CoveredClasses,
                TotalClasses = source.TotalClasses,
                GeneratedAt = source.GeneratedAt,
                ReportPath = source.ReportPath,
                CoverageGrade = source.CoverageGrade
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static CoverageMetricsDO ToDO(CoverageMetricsDTO source)
        {
            if (source == null) return null;

            return new CoverageMetricsDO
            {
                ProjectName = source.ProjectName,
                LineCoverage = source.LineCoverage,
                BranchCoverage = source.BranchCoverage,
                MethodCoverage = source.MethodCoverage,
                ClassCoverage = source.ClassCoverage,
                CoveredLines = source.CoveredLines,
                TotalLines = source.TotalLines,
                CoveredBranches = source.CoveredBranches,
                TotalBranches = source.TotalBranches,
                CoveredMethods = source.CoveredMethods,
                TotalMethods = source.TotalMethods,
                CoveredClasses = source.CoveredClasses,
                TotalClasses = source.TotalClasses,
                GeneratedAt = source.GeneratedAt,
                ReportPath = source.ReportPath,
                CoverageGrade = source.CoverageGrade
            };
        }
    }

    /// <summary>
    /// 质量门禁状态映射器
    /// </summary>
    public static class QualityGateStatusMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static QualityGateStatusDTO ToDTO(QualityGateStatusDO source)
        {
            if (source == null) return null;

            return new QualityGateStatusDTO
            {
                GateId = source.GateId,
                GateName = source.GateName,
                GateType = source.GateType,
                Status = source.Status,
                Threshold = source.Threshold,
                CurrentValue = source.CurrentValue,
                CheckedAt = source.CheckedAt,
                Message = source.Message,
                Details = source.Details
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static QualityGateStatusDO ToDO(QualityGateStatusDTO source)
        {
            if (source == null) return null;

            return new QualityGateStatusDO
            {
                GateId = source.GateId,
                GateName = source.GateName,
                GateType = source.GateType,
                Status = source.Status,
                Threshold = source.Threshold,
                CurrentValue = source.CurrentValue,
                CheckedAt = source.CheckedAt,
                Message = source.Message,
                Details = source.Details
            };
        }

        /// <summary>
        /// 将领域对象列表转换为数据传输对象列表
        /// </summary>
        public static List<QualityGateStatusDTO> ToDTOList(List<QualityGateStatusDO> source)
        {
            return source?.Select(ToDTO).Where(x => x != null).ToList() ?? new List<QualityGateStatusDTO>();
        }

        /// <summary>
        /// 将数据传输对象列表转换为领域对象列表
        /// </summary>
        public static List<QualityGateStatusDO> ToDOList(List<QualityGateStatusDTO> source)
        {
            return source?.Select(ToDO).Where(x => x != null).ToList() ?? new List<QualityGateStatusDO>();
        }
    }

    /// <summary>
    /// 测试会话映射器
    /// </summary>
    public static class TestSessionMapper
    {
        /// <summary>
        /// 将领域对象转换为数据传输对象
        /// </summary>
        public static TestSessionDTO ToDTO(TestSessionDO source)
        {
            if (source == null) return null;

            return new TestSessionDTO
            {
                SessionId = source.SessionId,
                SessionName = source.SessionName,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                TestResults = TestResultMapper.ToDTOList(source.TestResults),
                CoverageMetrics = CoverageMetricsMapper.ToDTO(source.CoverageMetrics),
                QualityGates = QualityGateStatusMapper.ToDTOList(source.QualityGates),
                ProjectPath = source.ProjectPath,
                SolutionPath = source.SolutionPath,
                BuildConfiguration = source.BuildConfiguration,
                TargetFramework = source.TargetFramework,
                TotalTests = source.TotalTests,
                PassedTests = source.PassedTests,
                FailedTests = source.FailedTests,
                SkippedTests = source.SkippedTests,
                PassRate = source.PassRate,
                TotalDurationMs = source.TotalDurationMs,
                SessionStatus = source.SessionStatus
            };
        }

        /// <summary>
        /// 将数据传输对象转换为领域对象
        /// </summary>
        public static TestSessionDO ToDO(TestSessionDTO source)
        {
            if (source == null) return null;

            return new TestSessionDO
            {
                SessionId = source.SessionId,
                SessionName = source.SessionName,
                StartTime = source.StartTime,
                EndTime = source.EndTime,
                TestResults = TestResultMapper.ToDOList(source.TestResults),
                CoverageMetrics = CoverageMetricsMapper.ToDO(source.CoverageMetrics),
                QualityGates = QualityGateStatusMapper.ToDOList(source.QualityGates),
                ProjectPath = source.ProjectPath,
                SolutionPath = source.SolutionPath,
                BuildConfiguration = source.BuildConfiguration,
                TargetFramework = source.TargetFramework,
                TotalTests = source.TotalTests,
                PassedTests = source.PassedTests,
                FailedTests = source.FailedTests,
                SkippedTests = source.SkippedTests,
                PassRate = source.PassRate,
                TotalDurationMs = source.TotalDurationMs,
                SessionStatus = source.SessionStatus
            };
        }
    }
}