using BannerlordModEditor.XmlAdaptationChecker.Core;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;
using BannerlordModEditor.XmlAdaptationChecker.Services;
using Xunit;
using FluentAssertions;
using System.Text.Json;
using System.Linq;
using XmlChecker = BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker;

namespace BannerlordModEditor.XmlAdaptationChecker.Tests.Services
{
    /// <summary>
    /// OutputFormatService 单元测试
    /// </summary>
    public class OutputFormatServiceTests
    {
        private readonly OutputFormatService _service;
        private readonly XmlChecker.AdaptationCheckResult _sampleResult;

        public OutputFormatServiceTests()
        {
            _service = new OutputFormatService();
            _sampleResult = CreateSampleResult();
        }

        [Fact]
        public async Task OutputResultAsync_WithConsoleFormat_ShouldReturnConsoleOutput()
        {
            // Arrange
            var format = OutputFormat.Console;

            // Act
            var result = await _service.OutputResultAsync(_sampleResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("XML适配状态检查结果");
            result.Should().Contain("总文件数: 10");
            result.Should().Contain("已适配: 7");
            result.Should().Contain("未适配: 3");
            result.Should().Contain("适配率: 70.0%");
        }

        [Fact]
        public async Task OutputResultAsync_WithMarkdownFormat_ShouldReturnMarkdownOutput()
        {
            // Arrange
            var format = OutputFormat.Markdown;

            // Act
            var result = await _service.OutputResultAsync(_sampleResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("# XML适配状态报告");
            result.Should().Contain("## 统计信息");
            result.Should().Contain("| 总文件数 | 10 |");
            result.Should().Contain("| 已适配 | 7 |");
            result.Should().Contain("| 未适配 | 3 |");
            result.Should().Contain("| 适配率 | 70.0% |");
        }

        [Fact]
        public async Task OutputResultAsync_WithCsvFormat_ShouldReturnCsvOutput()
        {
            // Arrange
            var format = OutputFormat.Csv;

            // Act
            var result = await _service.OutputResultAsync(_sampleResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("XML适配状态报告");
            result.Should().Contain("总文件数,10");
            result.Should().Contain("已适配,7");
            result.Should().Contain("未适配,3");
            result.Should().Contain("适配率,70.0%");
        }

        [Fact]
        public async Task OutputResultAsync_WithJsonFormat_ShouldReturnJsonOutput()
        {
            // Arrange
            var format = OutputFormat.Json;

            // Act
            var result = await _service.OutputResultAsync(_sampleResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            
            // 验证JSON格式
            var jsonDoc = JsonDocument.Parse(result);
            jsonDoc.RootElement.GetProperty("totalFiles").GetInt32().Should().Be(10);
            jsonDoc.RootElement.GetProperty("adaptedFiles").GetInt32().Should().Be(7);
            jsonDoc.RootElement.GetProperty("unadaptedFiles").GetInt32().Should().Be(3);
            jsonDoc.RootElement.GetProperty("adaptationRate").GetDouble().Should().Be(70.0);
        }

        [Fact]
        public async Task OutputResultAsync_WithOutputPath_ShouldWriteToFile()
        {
            // Arrange
            var format = OutputFormat.Console;
            var outputPath = Path.GetTempFileName();
            
            try
            {
                // Act
                var result = await _service.OutputResultAsync(_sampleResult, format, outputPath);

                // Assert
                result.Should().NotBeNullOrEmpty();
                File.Exists(outputPath).Should().BeTrue();
                var fileContent = await File.ReadAllTextAsync(outputPath);
                fileContent.Should().Be(result);
            }
            finally
            {
                // 清理
                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
            }
        }

        [Fact]
        public async Task OutputResultAsync_WithUnsupportedFormat_ShouldThrowException()
        {
            // Arrange
            var format = (OutputFormat)99; // 无效格式

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => 
                _service.OutputResultAsync(_sampleResult, format));
        }

        [Fact]
        public async Task OutputResultAsync_WithEmptyResult_ShouldHandleGracefully()
        {
            // Arrange
            var format = OutputFormat.Console;
            var emptyResult = new XmlAdaptationChecker.AdaptationCheckResult();

            // Act
            var result = await _service.OutputResultAsync(emptyResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("XML适配状态检查结果");
            result.Should().Contain("总文件数: 0");
            result.Should().Contain("已适配: 0");
            result.Should().Contain("未适配: 0");
            result.Should().Contain("适配率: 0.0%");
        }

        [Fact]
        public async Task OutputResultAsync_WithErrorResult_ShouldIncludeErrors()
        {
            // Arrange
            var format = OutputFormat.Console;
            var errorResult = new XmlAdaptationChecker.AdaptationCheckResult
            {
                Errors = new List<string> { "错误1", "错误2" }
            };

            // Act
            var result = await _service.OutputResultAsync(errorResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("错误信息:");
            result.Should().Contain("错误1");
            result.Should().Contain("错误2");
        }

        [Fact]
        public async Task OutputResultAsync_WithNoUnadaptedFiles_ShouldNotShowUnadaptedSection()
        {
            // Arrange
            var format = OutputFormat.Console;
            var noUnadaptedResult = new XmlAdaptationChecker.AdaptationCheckResult
            {
                TotalFiles = 10,
                AdaptedFiles = 10,
                UnadaptedFiles = 0
            };

            // Act
            var result = await _service.OutputResultAsync(noUnadaptedResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("适配率: 100.0%");
            result.Should().NotContain("未适配文件:");
        }

        [Fact]
        public async Task OutputResultAsync_WithManyUnadaptedFiles_ShouldShowLimitedList()
        {
            // Arrange
            var format = OutputFormat.Console;
            var manyUnadaptedResult = CreateManyUnadaptedFilesResult();

            // Act
            var result = await _service.OutputResultAsync(manyUnadaptedResult, format);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("未适配文件:");
            result.Should().Contain("还有 5 个文件");
        }

        [Fact]
        public void FormatFileSize_ShouldFormatCorrectly()
        {
            // Arrange & Act & Assert
            // 测试字节格式化
            var methodInfo = typeof(OutputFormatService).GetMethod("FormatFileSize", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            
            methodInfo.Should().NotBeNull();
            
            var formatMethod = (Func<long, string>)methodInfo!.CreateDelegate(typeof(Func<long, string>));
            
            formatMethod(0).Should().Be("0.0 B");
            formatMethod(1024).Should().Be("1.0 KB");
            formatMethod(1024 * 1024).Should().Be("1.0 MB");
            formatMethod(1024 * 1024 * 1024).Should().Be("1.0 GB");
        }

        [Fact]
        public async Task GenerateMarkdownOutput_WithComplexityGrouping_ShouldGroupCorrectly()
        {
            // Arrange
            var format = OutputFormat.Markdown;
            var result = CreateComplexityGroupedResult();

            // Act
            var output = await _service.OutputResultAsync(result, format);

            // Assert
            output.Should().Contain("#### Large 复杂度 (1 个文件)");
            output.Should().Contain("#### Complex 复杂度 (2 个文件)");
            output.Should().Contain("#### Medium 复杂度 (3 个文件)");
            output.Should().Contain("#### Simple 复杂度 (4 个文件)");
        }

        [Fact]
        public async Task GenerateCsvOutput_WithUnadaptedFiles_ShouldIncludeAllDetails()
        {
            // Arrange
            var format = OutputFormat.Csv;
            var result = CreateComplexityGroupedResult();

            // Act
            var output = await _service.OutputResultAsync(result, format);

            // Assert
            output.Should().Contain("文件名,文件大小,复杂度,预期模型名,完整路径");
            var lines = output.Split('\n');
            lines.Should().HaveCountGreaterThan(10); // 至少有标题行和多个数据行
        }

        [Fact]
        public async Task GenerateJsonOutput_WithUnadaptedFiles_ShouldIncludeFormattedFileSize()
        {
            // Arrange
            var format = OutputFormat.Json;
            var result = CreateComplexityGroupedResult();

            // Act
            var output = await _service.OutputResultAsync(result, format);

            // Assert
            var jsonDoc = JsonDocument.Parse(output);
            var unadaptedFiles = jsonDoc.RootElement.GetProperty("unadaptedFileDetails");
            unadaptedFiles.GetArrayLength().Should().Be(10);
            
            var firstFile = unadaptedFiles[0];
            firstFile.GetProperty("fileName").GetString().Should().NotBeNullOrEmpty();
            firstFile.GetProperty("fileSizeFormatted").GetString().Should().NotBeNullOrEmpty();
        }

        private XmlChecker.AdaptationCheckResult CreateSampleResult()
        {
            return new XmlChecker.AdaptationCheckResult
            {
                CheckTime = DateTime.Now,
                TotalFiles = 10,
                AdaptedFiles = 7,
                UnadaptedFiles = 3,
                AdaptedFileInfos = new List<XmlChecker.AdaptedFileInfo>
                {
                    new XmlChecker.AdaptedFileInfo
                    {
                        FileName = "adapted1.xml",
                        FileSize = 1024,
                        ModelName = "Adapted1",
                        FullPath = "/test/adapted1.xml"
                    },
                    new XmlChecker.AdaptedFileInfo
                    {
                        FileName = "adapted2.xml",
                        FileSize = 2048,
                        ModelName = "Adapted2",
                        FullPath = "/test/adapted2.xml"
                    }
                },
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>
                {
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "unadapted1.xml",
                        FileSize = 512,
                        ExpectedModelName = "Unadapted1",
                        FullPath = "/test/unadapted1.xml",
                        Complexity = AdaptationComplexity.Simple
                    },
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "unadapted2.xml",
                        FileSize = 1024,
                        ExpectedModelName = "Unadapted2",
                        FullPath = "/test/unadapted2.xml",
                        Complexity = AdaptationComplexity.Medium
                    },
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "unadapted3.xml",
                        FileSize = 2048,
                        ExpectedModelName = "Unadapted3",
                        FullPath = "/test/unadapted3.xml",
                        Complexity = AdaptationComplexity.Complex
                    }
                },
                Errors = new List<string>()
            };
        }

        private XmlChecker.AdaptationCheckResult CreateManyUnadaptedFilesResult()
        {
            var result = new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = 15,
                AdaptedFiles = 5,
                UnadaptedFiles = 10,
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>()
            };

            for (int i = 0; i < 10; i++)
            {
                result.UnadaptedFileInfos.Add(new XmlChecker.UnadaptedFileInfo
                {
                    FileName = $"unadapted{i}.xml",
                    FileSize = 1024,
                    ExpectedModelName = $"Unadapted{i}",
                    FullPath = $"/test/unadapted{i}.xml",
                    Complexity = AdaptationComplexity.Simple
                });
            }

            return result;
        }

        private XmlChecker.AdaptationCheckResult CreateComplexityGroupedResult()
        {
            var result = new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = 10,
                AdaptedFiles = 0,
                UnadaptedFiles = 10,
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>()
            };

            // 创建不同复杂度的文件
            var complexities = new[]
            {
                AdaptationComplexity.Large,
                AdaptationComplexity.Complex,
                AdaptationComplexity.Complex,
                AdaptationComplexity.Medium,
                AdaptationComplexity.Medium,
                AdaptationComplexity.Medium,
                AdaptationComplexity.Simple,
                AdaptationComplexity.Simple,
                AdaptationComplexity.Simple,
                AdaptationComplexity.Simple
            };

            for (int i = 0; i < 10; i++)
            {
                result.UnadaptedFileInfos.Add(new XmlChecker.UnadaptedFileInfo
                {
                    FileName = $"file_{complexities[i]}_{i}.xml",
                    FileSize = 1024,
                    ExpectedModelName = $"File{complexities[i]}{i}",
                    FullPath = $"/test/file_{complexities[i]}_{i}.xml",
                    Complexity = complexities[i]
                });
            }

            return result;
        }
    }
}