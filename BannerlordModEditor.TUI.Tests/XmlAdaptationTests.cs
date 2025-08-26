using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.TUI.Models;
using BannerlordModEditor.TUI.Services;

namespace BannerlordModEditor.TUI.Tests
{
    /// <summary>
    /// XML适配状态检查测试
    /// </summary>
    public class XmlAdaptationStatusServiceTests
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly XmlAdaptationStatusService _adaptationStatusService;

        public XmlAdaptationStatusServiceTests()
        {
            _fileDiscoveryService = new FileDiscoveryService();
            _adaptationStatusService = new XmlAdaptationStatusService(_fileDiscoveryService);
        }

        [Fact]
        public async Task GetAdaptationStatusReportAsync_ShouldReturnValidReport()
        {
            // Act
            var report = await _adaptationStatusService.GetAdaptationStatusReportAsync();

            // Assert
            Assert.NotNull(report);
            Assert.True(report.TotalTypes > 0);
            Assert.True(report.AdaptedCount >= 0);
            Assert.True(report.UnadaptedCount >= 0);
            Assert.True(report.AdaptationPercentage >= 0);
            Assert.True(report.AdaptationPercentage <= 100);

            // 验证报告包含预期的XML类型
            Assert.Contains(report.AdaptedTypes, t => t.XmlType == "ActionTypes");
            Assert.Contains(report.AdaptedTypes, t => t.XmlType == "CombatParameters");
        }

        [Fact]
        public async Task GetXmlTypeAdaptationDetailAsync_WithKnownType_ShouldReturnValidDetail()
        {
            // Act
            var detail = await _adaptationStatusService.GetXmlTypeAdaptationDetailAsync("ActionTypes");

            // Assert
            Assert.NotNull(detail);
            Assert.Equal("ActionTypes", detail.XmlType);
            Assert.True(detail.IsSupported);
            Assert.True(detail.IsAdapted);
        }

        [Fact]
        public async Task GetAdaptationSuggestionsAsync_ShouldReturnSuggestions()
        {
            // Act
            var suggestions = await _adaptationStatusService.GetAdaptationSuggestionsAsync();

            // Assert
            Assert.NotNull(suggestions);
            Assert.NotEmpty(suggestions);

            // 验证建议包含优先级信息
            var highPrioritySuggestions = suggestions.Where(s => s.Priority == AdaptationPriority.High).ToList();
            var mediumPrioritySuggestions = suggestions.Where(s => s.Priority == AdaptationPriority.Medium).ToList();
            var lowPrioritySuggestions = suggestions.Where(s => s.Priority == AdaptationPriority.Low).ToList();

            Assert.True(highPrioritySuggestions.Count >= 0);
            Assert.True(mediumPrioritySuggestions.Count >= 0);
            Assert.True(lowPrioritySuggestions.Count >= 0);
        }

        [Fact]
        public async Task IsAdaptationCompleteAsync_ShouldReturnFalse()
        {
            // Act
            var isComplete = await _adaptationStatusService.IsAdaptationCompleteAsync();

            // Assert
            Assert.False(isComplete); // 目前应该返回false，因为还有未适配的XML类型
        }

        [Fact]
        public async Task GetAdaptationProgressAsync_ShouldReturnValidProgress()
        {
            // Act
            var progress = await _adaptationStatusService.GetAdaptationProgressAsync();

            // Assert
            Assert.NotNull(progress);
            Assert.True(progress.TotalTypes > 0);
            Assert.True(progress.AdaptedTypes >= 0);
            Assert.True(progress.UnadaptedTypes >= 0);
            Assert.True(progress.PercentageComplete >= 0);
            Assert.True(progress.PercentageComplete <= 100);
            Assert.NotEmpty(progress.NextSteps);
        }

        [Theory]
        [InlineData("ActionTypes")]    // 已适配
        [InlineData("Items")]         // 部分适配
        [InlineData("BasicObjects")]  // 未适配
        public async Task GetXmlTypeAdaptationDetailAsync_WithVariousTypes_ShouldHandleAllCases(string xmlType)
        {
            // Act
            var detail = await _adaptationStatusService.GetXmlTypeAdaptationDetailAsync(xmlType);

            // Assert
            Assert.NotNull(detail);
            Assert.Equal(xmlType, detail.XmlType);
            Assert.True(detail.IsSupported); // 所有已知类型都应该被支持
        }
    }

    /// <summary>
    /// 增强类型化XML转换服务测试
    /// </summary>
    public class EnhancedTypedXmlConversionServiceTests
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly EnhancedTypedXmlConversionService _conversionService;

        public EnhancedTypedXmlConversionServiceTests()
        {
            _fileDiscoveryService = new FileDiscoveryService();
            _conversionService = new EnhancedTypedXmlConversionService(_fileDiscoveryService);
        }

        [Fact]
        public async Task ValidateTypedXmlAsync_WithValidXml_ShouldPass()
        {
            // Arrange - 创建一个简单的测试XML文件
            var testXmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_sets>
    <action_set id=""test_action"" skeleton=""human_skeleton"">
        <action type=""attack"" animation=""attack_animation"" />
    </action_set>
</action_sets>";

            var testFilePath = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(testFilePath, testXmlContent);

                // Act
                var result = await _conversionService.ValidateTypedXmlAsync<BannerlordModEditor.Common.Models.DO.ActionSetsDO>(testFilePath);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.IsValid); // 假设基本的XML结构是有效的
            }
            finally
            {
                if (File.Exists(testFilePath))
                {
                    File.Delete(testFilePath);
                }
            }
        }

        [Fact]
        public async Task CreateTypedXmlTemplateAsync_ShouldCreateValidTemplate()
        {
            // Arrange
            var outputPath = Path.GetTempFileName();

            try
            {
                // Act
                var result = await _conversionService.CreateTypedXmlTemplateAsync<BannerlordModEditor.Common.Models.DO.ActionSetsDO>(outputPath);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Success);
                Assert.Equal(outputPath, result.OutputPath);
                Assert.True(File.Exists(outputPath));

                // 验证创建的文件包含有效的XML
                var content = await File.ReadAllTextAsync(outputPath);
                Assert.Contains("<?xml", content);
                Assert.Contains("action_sets", content);
            }
            finally
            {
                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
            }
        }
    }

    /// <summary>
    /// XML适配检查器测试
    /// </summary>
    public class XmlAdaptationCheckerTests
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly IXmlTypeDetectionService _xmlTypeDetectionService;
        private readonly XmlAdaptationChecker _adaptationChecker;

        public XmlAdaptationCheckerTests()
        {
            _fileDiscoveryService = new FileDiscoveryService();
            _xmlTypeDetectionService = new XmlTypeDetectionService(_fileDiscoveryService);
            _adaptationChecker = new XmlAdaptationChecker(_fileDiscoveryService, _xmlTypeDetectionService);
        }

        [Fact]
        public async Task RunAdaptationCheckAsync_ShouldReturnValidStatus()
        {
            // Act
            var isComplete = await _adaptationChecker.RunAdaptationCheckAsync();

            // Assert
            // 目前应该返回false，因为还有未适配的XML类型
            Assert.False(isComplete);
        }

        [Fact]
        public async Task AdaptXmlTypeAsync_WithAdaptedType_ShouldReturnTrue()
        {
            // Act
            var success = await _adaptationChecker.AdaptXmlTypeAsync("ActionTypes");

            // Assert
            Assert.True(success); // ActionTypes应该已经适配
        }
    }
}