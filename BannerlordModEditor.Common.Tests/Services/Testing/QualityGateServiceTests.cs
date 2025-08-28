using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Services.Testing;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services.Testing
{
    /// <summary>
    /// 质量门禁服务测试
    /// </summary>
    public class QualityGateServiceTests
    {
        private readonly QualityMonitoringService _qualityMonitoringService;
        private readonly QualityGateService _qualityGateService;

        public QualityGateServiceTests()
        {
            _qualityMonitoringService = new QualityMonitoringService();
            _qualityGateService = new QualityGateService(_qualityMonitoringService);
        }

        [Fact]
        public void GetAllQualityGateDefinitions_ShouldReturnDefaultGates()
        {
            // Act
            var gates = _qualityGateService.GetAllQualityGateDefinitions();

            // Assert
            Assert.NotEmpty(gates);
            Assert.True(gates.Count >= 5); // 至少有5个默认门禁
            
            // 检查必要的默认门禁
            Assert.Contains(gates, g => g.Id == "test_pass_rate");
            Assert.Contains(gates, g => g.Id == "code_coverage");
            Assert.Contains(gates, g => g.Id == "execution_time");
            Assert.Contains(gates, g => g.Id == "error_rate");
        }

        [Fact]
        public void GetEnabledQualityGateDefinitions_ShouldReturnOnlyEnabledGates()
        {
            // Arrange
            _qualityGateService.SetQualityGateEnabled("test_pass_rate", false);

            // Act
            var enabledGates = _qualityGateService.GetEnabledQualityGateDefinitions();

            // Assert
            Assert.DoesNotContain(enabledGates, g => g.Id == "test_pass_rate");
            Assert.Contains(enabledGates, g => g.Id == "code_coverage");
        }

        [Fact]
        public void AddQualityGate_ShouldAddNewGate()
        {
            // Arrange
            var newGate = new QualityGateDefinition
            {
                Id = "custom_gate",
                Name = "Custom Gate",
                Description = "Custom quality gate",
                Type = QualityGateType.Custom,
                Threshold = 95.0,
                Operator = ComparisonOperator.GreaterThanOrEqual,
                Severity = RiskLevel.Medium,
                Enabled = true,
                MessageTemplate = "Custom gate failed: {current_value:F2} < {threshold:F2}",
                SuccessMessage = "Custom gate passed: {current_value:F2} >= {threshold:F2}"
            };

            // Act
            _qualityGateService.AddQualityGate(newGate);

            // Assert
            var retrievedGate = _qualityGateService.GetQualityGateDefinition("custom_gate");
            Assert.NotNull(retrievedGate);
            Assert.Equal("Custom Gate", retrievedGate.Name);
            Assert.Equal(QualityGateType.Custom, retrievedGate.Type);
            Assert.Equal(95.0, retrievedGate.Threshold);
        }

        [Fact]
        public void UpdateQualityGate_ShouldUpdateExistingGate()
        {
            // Arrange
            var gateId = "test_pass_rate";
            var originalGate = _qualityGateService.GetQualityGateDefinition(gateId);
            var originalThreshold = originalGate?.Threshold ?? 0;

            // Act
            _qualityGateService.UpdateQualityGate(gateId, gate =>
            {
                gate.Threshold = 90.0;
                gate.Severity = RiskLevel.High;
            });

            // Assert
            var updatedGate = _qualityGateService.GetQualityGateDefinition(gateId);
            Assert.NotNull(updatedGate);
            Assert.Equal(90.0, updatedGate.Threshold);
            Assert.Equal(RiskLevel.High, updatedGate.Severity);
            Assert.NotEqual(originalThreshold, updatedGate.Threshold);
        }

        [Fact]
        public void RemoveQualityGate_ShouldRemoveGate()
        {
            // Arrange
            var newGate = new QualityGateDefinition
            {
                Id = "temp_gate",
                Name = "Temporary Gate",
                Type = QualityGateType.Custom,
                Threshold = 80.0,
                Operator = ComparisonOperator.GreaterThanOrEqual,
                Severity = RiskLevel.Medium,
                Enabled = true
            };

            _qualityGateService.AddQualityGate(newGate);

            // Act
            _qualityGateService.RemoveQualityGate("temp_gate");

            // Assert
            var removedGate = _qualityGateService.GetQualityGateDefinition("temp_gate");
            Assert.Null(removedGate);
        }

        [Fact]
        public void SetQualityGateEnabled_ShouldEnableDisableGate()
        {
            // Arrange
            var gateId = "test_pass_rate";

            // Act - Disable
            _qualityGateService.SetQualityGateEnabled(gateId, false);

            // Assert
            var gate = _qualityGateService.GetQualityGateDefinition(gateId);
            Assert.NotNull(gate);
            Assert.False(gate.Enabled);

            // Act - Enable
            _qualityGateService.SetQualityGateEnabled(gateId, true);

            // Assert
            gate = _qualityGateService.GetQualityGateDefinition(gateId);
            Assert.NotNull(gate);
            Assert.True(gate.Enabled);
        }

        [Fact]
        public async Task CheckQualityGatesAsync_WithPassingSession_ShouldPassAllGates()
        {
            // Arrange
            var session = new TestSessionDO
            {
                SessionId = Guid.NewGuid().ToString(),
                SessionName = "Test Session",
                PassRate = 95.0,
                TotalTests = 100,
                PassedTests = 95,
                FailedTests = 5,
                SkippedTests = 0,
                TotalDurationMs = 5000,
                CoverageMetrics = new CoverageMetricsDO
                {
                    LineCoverage = 85.0,
                    BranchCoverage = 80.0,
                    MethodCoverage = 90.0,
                    ClassCoverage = 85.0
                }
            };

            // Act
            var result = await _qualityGateService.CheckQualityGatesAsync(session);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(QualityGateStatus.Passed, result.OverallStatus);
            Assert.True(result.AllPassed);
            Assert.Equal(result.GateStatuses.Count, result.PassedGates);
            Assert.Equal(0, result.FailedGates);
        }

        [Fact]
        public async Task CheckQualityGatesAsync_WithFailingSession_ShouldFailGates()
        {
            // Arrange
            var session = new TestSessionDO
            {
                SessionId = Guid.NewGuid().ToString(),
                SessionName = "Test Session",
                PassRate = 60.0,
                TotalTests = 100,
                PassedTests = 60,
                FailedTests = 40,
                SkippedTests = 0,
                TotalDurationMs = 600000, // 10 minutes
                CoverageMetrics = new CoverageMetricsDO
                {
                    LineCoverage = 50.0,
                    BranchCoverage = 45.0,
                    MethodCoverage = 55.0,
                    ClassCoverage = 50.0
                }
            };

            // Act
            var result = await _qualityGateService.CheckQualityGatesAsync(session);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(QualityGateStatus.Failed, result.OverallStatus);
            Assert.False(result.AllPassed);
            Assert.True(result.FailedGates > 0);
        }

        [Fact]
        public void ValidateQualityGateConfiguration_WithValidConfiguration_ShouldReturnNoErrors()
        {
            // Act
            var errors = _qualityGateService.ValidateQualityGateConfiguration();

            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidateQualityGateConfiguration_WithInvalidGate_ShouldReturnErrors()
        {
            // Arrange
            var invalidGate = new QualityGateDefinition
            {
                Id = "",
                Name = "Invalid Gate",
                Type = QualityGateType.TestPassRate,
                Threshold = -10.0,
                Operator = ComparisonOperator.GreaterThanOrEqual,
                Severity = RiskLevel.Medium,
                Enabled = true
            };

            _qualityGateService.AddQualityGate(invalidGate);

            // Act
            var errors = _qualityGateService.ValidateQualityGateConfiguration();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("ID不能为空"));
            Assert.Contains(errors, e => e.Contains("阈值不能为负数"));
        }

        [Fact]
        public void ValidateQualityGateConfiguration_WithInvalidPercentage_ShouldReturnErrors()
        {
            // Arrange
            var invalidGate = new QualityGateDefinition
            {
                Id = "invalid_percentage",
                Name = "Invalid Percentage Gate",
                Type = QualityGateType.TestPassRate,
                Threshold = 150.0, // 超过100%
                Operator = ComparisonOperator.GreaterThanOrEqual,
                Severity = RiskLevel.Medium,
                Enabled = true
            };

            _qualityGateService.AddQualityGate(invalidGate);

            // Act
            var errors = _qualityGateService.ValidateQualityGateConfiguration();

            // Assert
            Assert.NotEmpty(errors);
            Assert.Contains(errors, e => e.Contains("通过率阈值不能超过100%"));
        }

        [Fact]
        public void GetQualityGateDefinition_WithNonexistentGate_ShouldReturnNull()
        {
            // Act
            var gate = _qualityGateService.GetQualityGateDefinition("nonexistent_gate");

            // Assert
            Assert.Null(gate);
        }

        [Fact]
        public void UpdateQualityGate_WithNonexistentGate_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _qualityGateService.UpdateQualityGate("nonexistent_gate", gate => { }));
        }

        [Fact]
        public void RemoveQualityGate_WithNonexistentGate_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _qualityGateService.RemoveQualityGate("nonexistent_gate"));
        }

        [Fact]
        public void SetQualityGateEnabled_WithNonexistentGate_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
                _qualityGateService.SetQualityGateEnabled("nonexistent_gate", true));
        }

        [Fact]
        public async Task CheckQualityGatesAsync_WithEmptySession_ShouldHandleGracefully()
        {
            // Arrange
            var session = new TestSessionDO
            {
                SessionId = Guid.NewGuid().ToString(),
                SessionName = "Empty Session",
                PassRate = 0.0,
                TotalTests = 0,
                PassedTests = 0,
                FailedTests = 0,
                SkippedTests = 0,
                TotalDurationMs = 0,
                CoverageMetrics = new CoverageMetricsDO()
            };

            // Act
            var result = await _qualityGateService.CheckQualityGatesAsync(session);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(QualityGateStatus.Failed, result.OverallStatus);
            Assert.False(result.AllPassed);
        }

        [Fact]
        public void ExportQualityGateConfiguration_ShouldReturnValidConfiguration()
        {
            // Act
            var config = _qualityGateService.ExportQualityGateConfiguration();

            // Assert
            Assert.NotEmpty(config);
            Assert.Contains("质量门禁配置", config);
            Assert.Contains("test_pass_rate", config);
            Assert.Contains("code_coverage", config);
            
          }
    }
}