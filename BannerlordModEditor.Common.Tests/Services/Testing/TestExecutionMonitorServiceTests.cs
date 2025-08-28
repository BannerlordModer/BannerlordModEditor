using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Mappers.Testing;
using BannerlordModEditor.Common.Services.Testing;
using BannerlordModEditor.Common.Utils.Testing;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services.Testing
{
    /// <summary>
    /// 测试执行监控服务测试
    /// </summary>
    public class TestExecutionMonitorServiceTests
    {
        private readonly QualityMonitoringService _qualityMonitoringService;
        private readonly TestExecutionMonitorService _monitorService;

        public TestExecutionMonitorServiceTests()
        {
            _qualityMonitoringService = new QualityMonitoringService();
            _monitorService = new TestExecutionMonitorService(_qualityMonitoringService);
        }

        [Fact]
        public void CreateTestSession_ShouldCreateValidSession()
        {
            // Arrange
            var sessionName = "Test Session 1";
            var projectPath = "/path/to/project.csproj";
            var solutionPath = "/path/to/solution.sln";

            // Act
            var session = _monitorService.CreateTestSession(sessionName, projectPath, solutionPath);

            // Assert
            Assert.NotNull(session);
            Assert.NotEmpty(session.SessionId);
            Assert.Equal(sessionName, session.SessionName);
            Assert.Equal(projectPath, session.ProjectPath);
            Assert.Equal(solutionPath, session.SolutionPath);
            Assert.Equal(TestSessionStatus.Created, session.SessionStatus);
            Assert.True(session.StartTime <= DateTime.Now);
        }

        [Fact]
        public async Task StartTestSessionAsync_ShouldUpdateSessionStatus()
        {
            // Arrange
            var session = _monitorService.CreateTestSession("Test Session", "/path/to/project.csproj", "/path/to/solution.sln");

            // Act
            await _monitorService.StartTestSessionAsync(session.SessionId);

            // Assert
            var activeSession = _monitorService.GetActiveSession(session.SessionId);
            Assert.NotNull(activeSession);
            Assert.Equal(TestSessionStatus.Running, activeSession.SessionStatus);
        }

        [Fact]
        public void RecordTestResult_ShouldAddResultToSession()
        {
            // Arrange
            var session = _monitorService.CreateTestSession("Test Session", "/path/to/project.csproj", "/path/to/solution.sln");
            var testResult = new TestResultDO
            {
                Name = "Test Method 1",
                Type = "Unit",
                Category = "Common",
                Status = TestStatus.Passed,
                DurationMs = 100,
                ProjectPath = "/path/to/project.csproj"
            };

            // Act
            _monitorService.RecordTestResult(session.SessionId, testResult);

            // Assert
            var activeSession = _monitorService.GetActiveSession(session.SessionId);
            Assert.NotNull(activeSession);
            Assert.Single(activeSession.TestResults);
            Assert.Equal("Test Method 1", activeSession.TestResults[0].Name);
        }

        [Fact]
        public void GetActiveSession_ShouldReturnCorrectSession()
        {
            // Arrange
            var session = _monitorService.CreateTestSession("Test Session", "/path/to/project.csproj", "/path/to/solution.sln");

            // Act
            var retrievedSession = _monitorService.GetActiveSession(session.SessionId);

            // Assert
            Assert.NotNull(retrievedSession);
            Assert.Equal(session.SessionId, retrievedSession.SessionId);
        }

        [Fact]
        public void GetActiveSessions_ShouldReturnAllActiveSessions()
        {
            // Arrange
            var session1 = _monitorService.CreateTestSession("Session 1", "/path/to/project1.csproj", "/path/to/solution.sln");
            var session2 = _monitorService.CreateTestSession("Session 2", "/path/to/project2.csproj", "/path/to/solution.sln");

            // Act
            var activeSessions = _monitorService.GetActiveSessions();

            // Assert
            Assert.Equal(2, activeSessions.Count);
            Assert.Contains(activeSessions, s => s.SessionId == session1.SessionId);
            Assert.Contains(activeSessions, s => s.SessionId == session2.SessionId);
        }

        [Fact]
        public async Task CompleteTestSessionAsync_ShouldMoveToCompletedSessions()
        {
            // Arrange
            var session = _monitorService.CreateTestSession("Test Session", "/path/to/project.csproj", "/path/to/solution.sln");
            await _monitorService.StartTestSessionAsync(session.SessionId);

            // Add some test results
            var testResult = new TestResultDO
            {
                Name = "Test Method 1",
                Type = "Unit",
                Category = "Common",
                Status = TestStatus.Passed,
                DurationMs = 100,
                ProjectPath = "/path/to/project.csproj"
            };
            _monitorService.RecordTestResult(session.SessionId, testResult);

            // Act
            await _monitorService.CompleteTestSessionAsync(session.SessionId);

            // Assert
            var activeSession = _monitorService.GetActiveSession(session.SessionId);
            Assert.Null(activeSession);

            var completedSessions = _monitorService.GetCompletedSessions();
            Assert.Single(completedSessions);
            Assert.Equal(session.SessionId, completedSessions[0].SessionId);
            Assert.Equal(TestSessionStatus.Completed, completedSessions[0].SessionStatus);
            Assert.Equal(1, completedSessions[0].TotalTests);
            Assert.Equal(1, completedSessions[0].PassedTests);
            Assert.Equal(100.0, completedSessions[0].PassRate);
        }

        [Fact]
        public async Task GetExecutionStatistics_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var session1 = _monitorService.CreateTestSession("Session 1", "/path/to/project1.csproj", "/path/to/solution.sln");
            var session2 = _monitorService.CreateTestSession("Session 2", "/path/to/project2.csproj", "/path/to/solution.sln");
            
            // Start the sessions to make them active
            await _monitorService.StartTestSessionAsync(session1.SessionId);
            await _monitorService.StartTestSessionAsync(session2.SessionId);

            // Add test results to session1
            var testResult1 = new TestResultDO
            {
                Name = "Test Method 1",
                Type = "Unit",
                Category = "Common",
                Status = TestStatus.Passed,
                DurationMs = 100,
                ProjectPath = "/path/to/project1.csproj"
            };
            _monitorService.RecordTestResult(session1.SessionId, testResult1);

            var testResult2 = new TestResultDO
            {
                Name = "Test Method 2",
                Type = "Unit",
                Category = "Common",
                Status = TestStatus.Failed,
                DurationMs = 200,
                ProjectPath = "/path/to/project1.csproj"
            };
            _monitorService.RecordTestResult(session1.SessionId, testResult2);

            // Complete session1 to move it to completed sessions
            await _monitorService.CompleteTestSessionAsync(session1.SessionId);

            // Act
            var stats = _monitorService.GetExecutionStatistics();

            // Assert
            Assert.Equal(2, stats.TotalSessions);
            Assert.Equal(1, stats.ActiveSessions); // Only session2 is still active
            Assert.Equal(1, stats.CompletedSessions); // session1 is completed
            Assert.Equal(2, stats.TotalTests);
            Assert.Equal(1, stats.PassedTests);
            Assert.Equal(1, stats.FailedTests);
            Assert.Equal(0, stats.SkippedTests);
            // Average pass rate is calculated as the average of session pass rates
            // session1 has 1 passed, 1 failed = 50% pass rate
            // session2 has 0 passed, 0 failed = 0% pass rate
            // Average = (50 + 0) / 2 = 25%
            Assert.Equal(25.0, stats.AveragePassRate);
            Assert.Equal(300, stats.TotalExecutionTime);
        }

        [Fact]
        public void CleanupOldSessions_ShouldRemoveExcessSessions()
        {
            // Arrange
            var sessions = new List<TestSessionDO>();
            for (int i = 0; i < 105; i++)
            {
                var session = _monitorService.CreateTestSession($"Session {i}", $"/path/to/project{i}.csproj", "/path/to/solution.sln");
                sessions.Add(session);
            }

            // Move some sessions to completed
            foreach (var session in sessions.Take(50))
            {
                _monitorService.CancelTestSessionAsync(session.SessionId).Wait();
            }

            // Act
            _monitorService.CleanupOldSessions(100);

            // Assert
            var completedSessions = _monitorService.GetCompletedSessions();
            Assert.True(completedSessions.Count <= 100);
        }

        [Fact]
        public void RecordTestResult_WithNonexistentSession_ShouldThrowException()
        {
            // Arrange
            var testResult = new TestResultDO
            {
                Name = "Test Method 1",
                Type = "Unit",
                Category = "Common",
                Status = TestStatus.Passed,
                DurationMs = 100,
                ProjectPath = "/path/to/project.csproj"
            };

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _monitorService.RecordTestResult("nonexistent-session", testResult));
        }

        [Fact]
        public async Task StartTestSessionAsync_WithNonexistentSession_ShouldThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _monitorService.StartTestSessionAsync("nonexistent-session"));
        }

        [Fact]
        public async Task CompleteTestSessionAsync_WithNonexistentSession_ShouldThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _monitorService.CompleteTestSessionAsync("nonexistent-session"));
        }
    }
}