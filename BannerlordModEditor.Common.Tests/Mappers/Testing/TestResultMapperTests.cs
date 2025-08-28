using System;
using System.Collections.Generic;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Models.DTO.Testing;
using BannerlordModEditor.Common.Mappers.Testing;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Mappers.Testing
{
    /// <summary>
    /// 测试结果映射器测试
    /// </summary>
    public class TestResultMapperTests
    {
        [Fact]
        public void ToDTO_WithValidDO_ShouldMapCorrectly()
        {
            // Arrange
            var testResultDO = new TestResultDO
            {
                Id = "test-id-123",
                Name = "TestMethod1",
                Type = "Unit",
                Category = "Common",
                Status = TestStatus.Passed,
                StartTime = new DateTime(2023, 1, 1, 10, 0, 0),
                EndTime = new DateTime(2023, 1, 1, 10, 0, 1),
                DurationMs = 1000,
                ErrorMessage = null,
                ErrorStackTrace = null,
                Output = "Test output",
                ProjectPath = "/path/to/project.csproj",
                MethodFullName = "Namespace.Class.TestMethod1",
                FilePath = "/path/to/test/file.cs",
                LineNumber = 25
            };

            // Act
            var resultDTO = TestResultMapper.ToDTO(testResultDO);

            // Assert
            Assert.NotNull(resultDTO);
            Assert.Equal(testResultDO.Id, resultDTO.Id);
            Assert.Equal(testResultDO.Name, resultDTO.Name);
            Assert.Equal(testResultDO.Type, resultDTO.Type);
            Assert.Equal(testResultDO.Category, resultDTO.Category);
            Assert.Equal(testResultDO.Status, resultDTO.Status);
            Assert.Equal(testResultDO.StartTime, resultDTO.StartTime);
            Assert.Equal(testResultDO.EndTime, resultDTO.EndTime);
            Assert.Equal(testResultDO.DurationMs, resultDTO.DurationMs);
            Assert.Equal(testResultDO.ErrorMessage, resultDTO.ErrorMessage);
            Assert.Equal(testResultDO.ErrorStackTrace, resultDTO.ErrorStackTrace);
            Assert.Equal(testResultDO.Output, resultDTO.Output);
            Assert.Equal(testResultDO.ProjectPath, resultDTO.ProjectPath);
            Assert.Equal(testResultDO.MethodFullName, resultDTO.MethodFullName);
            Assert.Equal(testResultDO.FilePath, resultDTO.FilePath);
            Assert.Equal(testResultDO.LineNumber, resultDTO.LineNumber);
        }

        [Fact]
        public void ToDTO_WithNullDO_ShouldReturnNull()
        {
            // Act
            var result = TestResultMapper.ToDTO((TestResultDO)null!);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void TODO_WithValidDTO_ShouldMapCorrectly()
        {
            // Arrange
            var testResultDTO = new TestResultDTO
            {
                Id = "test-id-456",
                Name = "TestMethod2",
                Type = "Integration",
                Category = "Database",
                Status = TestStatus.Failed,
                StartTime = new DateTime(2023, 1, 1, 11, 0, 0),
                EndTime = new DateTime(2023, 1, 1, 11, 0, 2),
                DurationMs = 2000,
                ErrorMessage = "Assertion failed",
                ErrorStackTrace = "at Namespace.Class.TestMethod2() in file.cs:line 30",
                Output = "Test failed output",
                ProjectPath = "/path/to/project.csproj",
                MethodFullName = "Namespace.Class.TestMethod2",
                FilePath = "/path/to/test/file.cs",
                LineNumber = 30
            };

            // Act
            var resultDO = TestResultMapper.ToDO(testResultDTO);

            // Assert
            Assert.NotNull(resultDO);
            Assert.Equal(testResultDTO.Id, resultDO.Id);
            Assert.Equal(testResultDTO.Name, resultDO.Name);
            Assert.Equal(testResultDTO.Type, resultDO.Type);
            Assert.Equal(testResultDTO.Category, resultDO.Category);
            Assert.Equal(testResultDTO.Status, resultDO.Status);
            Assert.Equal(testResultDTO.StartTime, resultDO.StartTime);
            Assert.Equal(testResultDTO.EndTime, resultDO.EndTime);
            Assert.Equal(testResultDTO.DurationMs, resultDO.DurationMs);
            Assert.Equal(testResultDTO.ErrorMessage, resultDO.ErrorMessage);
            Assert.Equal(testResultDTO.ErrorStackTrace, resultDO.ErrorStackTrace);
            Assert.Equal(testResultDTO.Output, resultDO.Output);
            Assert.Equal(testResultDTO.ProjectPath, resultDO.ProjectPath);
            Assert.Equal(testResultDTO.MethodFullName, resultDO.MethodFullName);
            Assert.Equal(testResultDTO.FilePath, resultDO.FilePath);
            Assert.Equal(testResultDTO.LineNumber, resultDO.LineNumber);
        }

        [Fact]
        public void TODO_WithNullDTO_ShouldReturnNull()
        {
            // Act
            var result = TestResultMapper.ToDO((TestResultDTO)null!);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToDTOList_WithValidList_ShouldMapAllItems()
        {
            // Arrange
            var testResultsDO = new List<TestResultDO>
            {
                new TestResultDO
                {
                    Id = "test-1",
                    Name = "Test1",
                    Type = "Unit",
                    Category = "Common",
                    Status = TestStatus.Passed,
                    DurationMs = 100
                },
                new TestResultDO
                {
                    Id = "test-2",
                    Name = "Test2",
                    Type = "Integration",
                    Category = "Database",
                    Status = TestStatus.Failed,
                    DurationMs = 200
                },
                new TestResultDO
                {
                    Id = "test-3",
                    Name = "Test3",
                    Type = "Unit",
                    Category = "Common",
                    Status = TestStatus.Skipped,
                    DurationMs = 0
                }
            };

            // Act
            var resultsDTO = TestResultMapper.ToDTOList(testResultsDO);

            // Assert
            Assert.NotNull(resultsDTO);
            Assert.Equal(3, resultsDTO.Count);
            
            Assert.Equal("test-1", resultsDTO[0].Id);
            Assert.Equal("Test1", resultsDTO[0].Name);
            Assert.Equal("test-2", resultsDTO[1].Id);
            Assert.Equal("Test2", resultsDTO[1].Name);
            Assert.Equal("test-3", resultsDTO[2].Id);
            Assert.Equal("Test3", resultsDTO[2].Name);
        }

        [Fact]
        public void ToDTOList_WithNullList_ShouldReturnEmptyList()
        {
            // Act
            var result = TestResultMapper.ToDTOList((List<TestResultDO>)null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ToDOList_WithValidList_ShouldMapAllItems()
        {
            // Arrange
            var testResultsDTO = new List<TestResultDTO>
            {
                new TestResultDTO
                {
                    Id = "dto-1",
                    Name = "DtoTest1",
                    Type = "Unit",
                    Category = "Common",
                    Status = TestStatus.Passed,
                    DurationMs = 150
                },
                new TestResultDTO
                {
                    Id = "dto-2",
                    Name = "DtoTest2",
                    Type = "Integration",
                    Category = "API",
                    Status = TestStatus.Failed,
                    DurationMs = 250
                }
            };

            // Act
            var resultsDO = TestResultMapper.ToDOList(testResultsDTO);

            // Assert
            Assert.NotNull(resultsDO);
            Assert.Equal(2, resultsDO.Count);
            
            Assert.Equal("dto-1", resultsDO[0].Id);
            Assert.Equal("DtoTest1", resultsDO[0].Name);
            Assert.Equal("dto-2", resultsDO[1].Id);
            Assert.Equal("DtoTest2", resultsDO[1].Name);
        }

        [Fact]
        public void ToDOList_WithNullList_ShouldReturnEmptyList()
        {
            // Act
            var result = TestResultMapper.ToDOList((List<TestResultDTO>)null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ToDTOList_WithListContainingNullItems_ShouldFilterOutNulls()
        {
            // Arrange
            var testResultsDO = new List<TestResultDO?>
            {
                new TestResultDO
                {
                    Id = "test-1",
                    Name = "Test1",
                    Type = "Unit",
                    Category = "Common",
                    Status = TestStatus.Passed,
                    DurationMs = 100
                },
                null,
                new TestResultDO
                {
                    Id = "test-2",
                    Name = "Test2",
                    Type = "Integration",
                    Category = "Database",
                    Status = TestStatus.Failed,
                    DurationMs = 200
                }
            };

            // Act
            var resultsDTO = TestResultMapper.ToDTOList(testResultsDO!);

            // Assert
            Assert.NotNull(resultsDTO);
            Assert.Equal(2, resultsDTO.Count);
            Assert.Equal("test-1", resultsDTO[0].Id);
            Assert.Equal("test-2", resultsDTO[1].Id);
        }

        [Fact]
        public void CoverageMetricsMapper_ToDTO_ShouldMapCorrectly()
        {
            // Arrange
            var coverageDO = new CoverageMetricsDO
            {
                ProjectName = "TestProject",
                LineCoverage = 85.5,
                BranchCoverage = 78.2,
                MethodCoverage = 92.1,
                ClassCoverage = 88.7,
                CoveredLines = 855,
                TotalLines = 1000,
                CoveredBranches = 391,
                TotalBranches = 500,
                CoveredMethods = 184,
                TotalMethods = 200,
                CoveredClasses = 44,
                TotalClasses = 50,
                GeneratedAt = new DateTime(2023, 1, 1, 12, 0, 0),
                ReportPath = "/path/to/coverage.xml",
                CoverageGrade = CoverageGrade.Good
            };

            // Act
            var coverageDTO = CoverageMetricsMapper.ToDTO(coverageDO);

            // Assert
            Assert.NotNull(coverageDTO);
            Assert.Equal(coverageDO.ProjectName, coverageDTO.ProjectName);
            Assert.Equal(coverageDO.LineCoverage, coverageDTO.LineCoverage);
            Assert.Equal(coverageDO.BranchCoverage, coverageDTO.BranchCoverage);
            Assert.Equal(coverageDO.MethodCoverage, coverageDTO.MethodCoverage);
            Assert.Equal(coverageDO.ClassCoverage, coverageDTO.ClassCoverage);
            Assert.Equal(coverageDO.CoveredLines, coverageDTO.CoveredLines);
            Assert.Equal(coverageDO.TotalLines, coverageDTO.TotalLines);
            Assert.Equal(coverageDO.CoveredBranches, coverageDTO.CoveredBranches);
            Assert.Equal(coverageDO.TotalBranches, coverageDTO.TotalBranches);
            Assert.Equal(coverageDO.CoveredMethods, coverageDTO.CoveredMethods);
            Assert.Equal(coverageDO.TotalMethods, coverageDTO.TotalMethods);
            Assert.Equal(coverageDO.CoveredClasses, coverageDTO.CoveredClasses);
            Assert.Equal(coverageDO.TotalClasses, coverageDTO.TotalClasses);
            Assert.Equal(coverageDO.GeneratedAt, coverageDTO.GeneratedAt);
            Assert.Equal(coverageDO.ReportPath, coverageDTO.ReportPath);
            Assert.Equal(coverageDO.CoverageGrade, coverageDTO.CoverageGrade);
        }

        [Fact]
        public void QualityGateStatusMapper_ToDTOList_ShouldMapCorrectly()
        {
            // Arrange
            var gateStatusesDO = new List<QualityGateStatusDO>
            {
                new QualityGateStatusDO
                {
                    GateId = "gate-1",
                    GateName = "Test Pass Rate",
                    GateType = QualityGateType.TestPassRate,
                    Status = QualityGateStatus.Passed,
                    Threshold = 80.0,
                    CurrentValue = 85.0,
                    CheckedAt = new DateTime(2023, 1, 1, 13, 0, 0),
                    Message = "Test pass rate is acceptable"
                },
                new QualityGateStatusDO
                {
                    GateId = "gate-2",
                    GateName = "Code Coverage",
                    GateType = QualityGateType.CodeCoverage,
                    Status = QualityGateStatus.Failed,
                    Threshold = 70.0,
                    CurrentValue = 65.0,
                    CheckedAt = new DateTime(2023, 1, 1, 13, 0, 0),
                    Message = "Code coverage is below threshold"
                }
            };

            // Act
            var gateStatusesDTO = QualityGateStatusMapper.ToDTOList(gateStatusesDO);

            // Assert
            Assert.NotNull(gateStatusesDTO);
            Assert.Equal(2, gateStatusesDTO.Count);
            
            Assert.Equal("gate-1", gateStatusesDTO[0].GateId);
            Assert.Equal("Test Pass Rate", gateStatusesDTO[0].GateName);
            Assert.Equal(QualityGateStatus.Passed, gateStatusesDTO[0].Status);
            
            Assert.Equal("gate-2", gateStatusesDTO[1].GateId);
            Assert.Equal("Code Coverage", gateStatusesDTO[1].GateName);
            Assert.Equal(QualityGateStatus.Failed, gateStatusesDTO[1].Status);
        }

        [Fact]
        public void TestSessionMapper_ToDTO_ShouldMapComplexSessionCorrectly()
        {
            // Arrange
            var sessionDO = new TestSessionDO
            {
                SessionId = "session-123",
                SessionName = "Complex Test Session",
                StartTime = new DateTime(2023, 1, 1, 14, 0, 0),
                EndTime = new DateTime(2023, 1, 1, 14, 5, 0),
                ProjectPath = "/path/to/project.csproj",
                SolutionPath = "/path/to/solution.sln",
                BuildConfiguration = "Release",
                TargetFramework = "net9.0",
                TotalTests = 10,
                PassedTests = 8,
                FailedTests = 1,
                SkippedTests = 1,
                PassRate = 80.0,
                TotalDurationMs = 300000,
                SessionStatus = TestSessionStatus.Completed,
                CoverageMetrics = new CoverageMetricsDO
                {
                    ProjectName = "TestProject",
                    LineCoverage = 75.0,
                    BranchCoverage = 70.0,
                    MethodCoverage = 80.0,
                    ClassCoverage = 75.0
                },
                QualityGates = new List<QualityGateStatusDO>
                {
                    new QualityGateStatusDO
                    {
                        GateId = "gate-1",
                        GateName = "Test Pass Rate",
                        GateType = QualityGateType.TestPassRate,
                        Status = QualityGateStatus.Passed,
                        Threshold = 80.0,
                        CurrentValue = 80.0
                    }
                }
            };

            // Add test results
            sessionDO.TestResults.AddRange(new List<TestResultDO>
            {
                new TestResultDO { Id = "test-1", Name = "Test1", Status = TestStatus.Passed },
                new TestResultDO { Id = "test-2", Name = "Test2", Status = TestStatus.Failed }
            });

            // Act
            var sessionDTO = TestSessionMapper.ToDTO(sessionDO);

            // Assert
            Assert.NotNull(sessionDTO);
            Assert.Equal(sessionDO.SessionId, sessionDTO.SessionId);
            Assert.Equal(sessionDO.SessionName, sessionDTO.SessionName);
            Assert.Equal(sessionDO.TotalTests, sessionDTO.TotalTests);
            Assert.Equal(sessionDO.PassedTests, sessionDTO.PassedTests);
            Assert.Equal(sessionDO.FailedTests, sessionDTO.FailedTests);
            Assert.Equal(sessionDO.SkippedTests, sessionDTO.SkippedTests);
            Assert.Equal(sessionDO.PassRate, sessionDTO.PassRate);
            Assert.Equal(sessionDO.TotalDurationMs, sessionDTO.TotalDurationMs);
            Assert.Equal(sessionDO.SessionStatus, sessionDTO.SessionStatus);
            
            Assert.NotNull(sessionDTO.CoverageMetrics);
            Assert.Equal(sessionDO.CoverageMetrics.LineCoverage, sessionDTO.CoverageMetrics.LineCoverage);
            
            Assert.Single(sessionDTO.QualityGates);
            Assert.Equal("gate-1", sessionDTO.QualityGates[0].GateId);
            
            Assert.Equal(2, sessionDTO.TestResults.Count);
            Assert.Equal("test-1", sessionDTO.TestResults[0].Id);
            Assert.Equal("test-2", sessionDTO.TestResults[1].Id);
        }

        [Fact]
        public void RoundTripMapping_ShouldPreserveAllData()
        {
            // Arrange
            var originalDO = new TestResultDO
            {
                Id = "round-trip-test",
                Name = "RoundTripTest",
                Type = "Unit",
                Category = "Mapping",
                Status = TestStatus.Passed,
                StartTime = new DateTime(2023, 1, 1, 15, 0, 0),
                EndTime = new DateTime(2023, 1, 1, 15, 0, 1),
                DurationMs = 1000,
                ErrorMessage = null,
                ErrorStackTrace = null,
                Output = "Round trip test output",
                ProjectPath = "/path/to/project.csproj",
                MethodFullName = "Namespace.Class.RoundTripTest",
                FilePath = "/path/to/test/file.cs",
                LineNumber = 42
            };

            // Act - DO -> DTO -> DO
            var dto = TestResultMapper.ToDTO(originalDO);
            var resultDO = TestResultMapper.ToDO(dto);

            // Assert
            Assert.NotNull(resultDO);
            Assert.Equal(originalDO.Id, resultDO.Id);
            Assert.Equal(originalDO.Name, resultDO.Name);
            Assert.Equal(originalDO.Type, resultDO.Type);
            Assert.Equal(originalDO.Category, resultDO.Category);
            Assert.Equal(originalDO.Status, resultDO.Status);
            Assert.Equal(originalDO.StartTime, resultDO.StartTime);
            Assert.Equal(originalDO.EndTime, resultDO.EndTime);
            Assert.Equal(originalDO.DurationMs, resultDO.DurationMs);
            Assert.Equal(originalDO.ErrorMessage, resultDO.ErrorMessage);
            Assert.Equal(originalDO.ErrorStackTrace, resultDO.ErrorStackTrace);
            Assert.Equal(originalDO.Output, resultDO.Output);
            Assert.Equal(originalDO.ProjectPath, resultDO.ProjectPath);
            Assert.Equal(originalDO.MethodFullName, resultDO.MethodFullName);
            Assert.Equal(originalDO.FilePath, resultDO.FilePath);
            Assert.Equal(originalDO.LineNumber, resultDO.LineNumber);
        }
    }
}