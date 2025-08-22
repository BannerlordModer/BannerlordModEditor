using BannerlordModEditor.Common.Services;
using BannerlordModEditor.XmlAdaptationChecker;
using BannerlordModEditor.XmlAdaptationChecker.Core;
using BannerlordModEditor.XmlAdaptationChecker.Interfaces;
using BannerlordModEditor.XmlAdaptationChecker.Services;
using BannerlordModEditor.XmlAdaptationChecker.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using FluentAssertions;
using XmlChecker = BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker;

namespace BannerlordModEditor.XmlAdaptationChecker.Tests.Integration
{
    /// <summary>
    /// 输出格式服务集成测试
    /// </summary>
    public class OutputFormatServiceIntegrationTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _outputDirectory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOutputFormatService _outputService;
        private readonly XmlChecker.AdaptationCheckResult _testResult;

        public OutputFormatServiceIntegrationTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), $"OutputFormatService_Test_{Guid.NewGuid()}");
            _outputDirectory = Path.Combine(_testDirectory, "output");

            Directory.CreateDirectory(_testDirectory);
            Directory.CreateDirectory(_outputDirectory);

            // 创建服务提供者
            _serviceProvider = CreateServiceProvider();
            _outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();
            _testResult = CreateTestResult();
        }

        [Fact]
        public async Task OutputFormatService_WithAllFormats_ShouldGenerateValidOutputs()
        {
            // Arrange
            var formats = new[] { OutputFormat.Console, OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json };
            var outputs = new Dictionary<OutputFormat, string>();

            // Act
            foreach (var format in formats)
            {
                var output = await _outputService.OutputResultAsync(_testResult, format);
                outputs[format] = output;
            }

            // Assert
            outputs.Should().HaveCount(4);
            outputs.Values.Should().AllSatisfy(output => output.Should().NotBeNullOrEmpty());

            // 验证每种格式的特定内容
            outputs[OutputFormat.Console].Should().Contain("XML适配状态检查结果");
            outputs[OutputFormat.Markdown].Should().Contain("# XML适配状态报告");
            outputs[OutputFormat.Csv].Should().Contain("XML适配状态报告");
            outputs[OutputFormat.Json].Should().Contain("\"totalFiles\"");
        }

        [Fact]
        public async Task OutputFormatService_WithFileOutput_ShouldCreateFiles()
        {
            // Arrange
            var formats = new[] { OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json };
            var filePaths = new Dictionary<OutputFormat, string>();

            // Act
            foreach (var format in formats)
            {
                var fileName = $"test_output_{format.ToString().ToLower()}.txt";
                var filePath = Path.Combine(_outputDirectory, fileName);
                filePaths[format] = filePath;

                await _outputService.OutputResultAsync(_testResult, format, filePath);
            }

            // Assert
            foreach (var (format, filePath) in filePaths)
            {
                File.Exists(filePath).Should().BeTrue();
                var content = await File.ReadAllTextAsync(filePath);
                content.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public async Task OutputFormatService_WithLargeDataSet_ShouldHandleEfficiently()
        {
            // Arrange
            var largeResult = CreateLargeTestResult();

            // Act
            var outputs = new List<string>();
            var formats = new[] { OutputFormat.Console, OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json };

            foreach (var format in formats)
            {
                var output = await _outputService.OutputResultAsync(largeResult, format);
                outputs.Add(output);
            }

            // Assert
            outputs.Should().HaveCount(4);
            outputs.Should().AllSatisfy(output => output.Should().NotBeNullOrEmpty());

            // 验证输出大小合理（不是过大）
            outputs.Should().AllSatisfy(output => output.Length.Should().BeLessThan(1024 * 1024)); // 小于1MB
        }

        [Fact]
        public async Task OutputFormatService_WithEmptyResult_ShouldHandleGracefully()
        {
            // Arrange
            var emptyResult = new XmlChecker.AdaptationCheckResult();

            // Act
            var consoleOutput = await _outputService.OutputResultAsync(emptyResult, OutputFormat.Console);
            var markdownOutput = await _outputService.OutputResultAsync(emptyResult, OutputFormat.Markdown);
            var csvOutput = await _outputService.OutputResultAsync(emptyResult, OutputFormat.Csv);
            var jsonOutput = await _outputService.OutputResultAsync(emptyResult, OutputFormat.Json);

            // Assert
            consoleOutput.Should().NotBeNullOrEmpty();
            markdownOutput.Should().NotBeNullOrEmpty();
            csvOutput.Should().NotBeNullOrEmpty();
            jsonOutput.Should().NotBeNullOrEmpty();

            // 验证空结果的正确显示
            consoleOutput.Should().Contain("总文件数: 0");
            markdownOutput.Should().Contain("| 总文件数 | 0 |");
            csvOutput.Should().Contain("总文件数,0");
            jsonOutput.Should().Contain("\"totalFiles\":0");
        }

        [Fact]
        public async Task OutputFormatService_WithErrorResult_ShouldIncludeErrors()
        {
            // Arrange
            var errorResult = new XmlChecker.AdaptationCheckResult
            {
                Errors = new List<string> { "错误1: 测试错误", "错误2: 另一个错误" }
            };

            // Act
            var consoleOutput = await _outputService.OutputResultAsync(errorResult, OutputFormat.Console);
            var markdownOutput = await _outputService.OutputResultAsync(errorResult, OutputFormat.Markdown);
            var csvOutput = await _outputService.OutputResultAsync(errorResult, OutputFormat.Csv);
            var jsonOutput = await _outputService.OutputResultAsync(errorResult, OutputFormat.Json);

            // Assert
            consoleOutput.Should().Contain("错误信息:");
            consoleOutput.Should().Contain("错误1: 测试错误");
            consoleOutput.Should().Contain("错误2: 另一个错误");

            markdownOutput.Should().Contain("## 错误信息");
            markdownOutput.Should().Contain("- 错误1: 测试错误");
            markdownOutput.Should().Contain("- 错误2: 另一个错误");

            csvOutput.Should().Contain("错误信息");
            csvOutput.Should().Contain("错误1: 测试错误");
            csvOutput.Should().Contain("错误2: 另一个错误");

            jsonOutput.Should().Contain("\"errors\"");
            jsonOutput.Should().Contain("错误1: 测试错误");
            jsonOutput.Should().Contain("错误2: 另一个错误");
        }

        [Fact]
        public async Task OutputFormatService_WithMixedComplexity_ShouldGroupCorrectly()
        {
            // Arrange
            var mixedResult = CreateMixedComplexityResult();

            // Act
            var markdownOutput = await _outputService.OutputResultAsync(mixedResult, OutputFormat.Markdown);
            var csvOutput = await _outputService.OutputResultAsync(mixedResult, OutputFormat.Csv);
            var jsonOutput = await _outputService.OutputResultAsync(mixedResult, OutputFormat.Json);

            // Assert
            // Markdown 应该按复杂度分组
            markdownOutput.Should().Contain("#### Large 复杂度");
            markdownOutput.Should().Contain("#### Complex 复杂度");
            markdownOutput.Should().Contain("#### Medium 复杂度");
            markdownOutput.Should().Contain("#### Simple 复杂度");

            // CSV 应该包含复杂度信息
            csvOutput.Should().Contain("复杂度");

            // JSON 应该包含复杂度信息
            jsonOutput.Should().Contain("\"complexity\"");
        }

        [Fact]
        public async Task OutputFormatService_WithDifferentFileSizes_ShouldFormatCorrectly()
        {
            // Arrange
            var differentSizesResult = CreateDifferentFileSizeResult();

            // Act
            var consoleOutput = await _outputService.OutputResultAsync(differentSizesResult, OutputFormat.Console);
            var markdownOutput = await _outputService.OutputResultAsync(differentSizesResult, OutputFormat.Markdown);
            var csvOutput = await _outputService.OutputResultAsync(differentSizesResult, OutputFormat.Csv);
            var jsonOutput = await _outputService.OutputResultAsync(differentSizesResult, OutputFormat.Json);

            // Assert
            // 验证文件大小格式化
            consoleOutput.Should().Contain("KB");
            consoleOutput.Should().Contain("MB");

            markdownOutput.Should().Contain("KB");
            markdownOutput.Should().Contain("MB");

            csvOutput.Should().Contain("1024");
            csvOutput.Should().Contain("1048576");

            jsonOutput.Should().Contain("fileSizeFormatted");
            jsonOutput.Should().Contain("KB");
            jsonOutput.Should().Contain("MB");
        }

        [Fact]
        public async Task OutputFormatService_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var specialCharResult = new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = 2,
                AdaptedFiles = 1,
                UnadaptedFiles = 1,
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>
                {
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "special_文件.xml",
                        FileSize = 1024,
                        ExpectedModelName = "SpecialFile",
                        FullPath = "/test/special_文件.xml",
                        Complexity = AdaptationComplexity.Simple
                    }
                },
                Errors = new List<string> { "特殊字符错误: 测试 & 错误" }
            };

            // Act
            var outputs = new List<string>();
            var formats = new[] { OutputFormat.Console, OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json };

            foreach (var format in formats)
            {
                var output = await _outputService.OutputResultAsync(specialCharResult, format);
                outputs.Add(output);
            }

            // Assert
            outputs.Should().AllSatisfy(output => output.Should().NotBeNullOrEmpty());
            outputs.Should().AllSatisfy(output => output.Should().Contain("special_文件.xml"));
            outputs.Should().AllSatisfy(output => output.Should().Contain("特殊字符错误"));
        }

        [Fact]
        public async Task OutputFormatService_WithConcurrentAccess_ShouldHandleThreadSafety()
        {
            // Arrange
            var tasks = new List<Task<string>>();
            var formats = new[] { OutputFormat.Console, OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json };

            // Act - 并发生成输出
            for (int i = 0; i < 10; i++)
            {
                foreach (var format in formats)
                {
                    tasks.Add(_outputService.OutputResultAsync(_testResult, format));
                }
            }

            var results = await Task.WhenAll(tasks);

            // Assert
            results.Should().HaveCount(40);
            results.Should().AllSatisfy(result => result.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task OutputFormatService_WithFilePermissionIssues_ShouldHandleGracefully()
        {
            // Arrange
            var readOnlyPath = Path.Combine(_outputDirectory, "readonly.md");
            var readOnlyContent = "# Read-only content";
            await File.WriteAllTextAsync(readOnlyPath, readOnlyContent);
            
            // 设置文件为只读
            var fileInfo = new FileInfo(readOnlyPath);
            fileInfo.IsReadOnly = true;

            try
            {
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                    _outputService.OutputResultAsync(_testResult, OutputFormat.Markdown, readOnlyPath));
            }
            finally
            {
                // 清理
                if (File.Exists(readOnlyPath))
                {
                    fileInfo.IsReadOnly = false;
                    File.Delete(readOnlyPath);
                }
            }
        }

        private XmlChecker.AdaptationCheckResult CreateTestResult()
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

        private XmlChecker.AdaptationCheckResult CreateLargeTestResult()
        {
            var result = new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = 1000,
                AdaptedFiles = 700,
                UnadaptedFiles = 300,
                AdaptedFileInfos = new List<XmlChecker.AdaptedFileInfo>(),
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>(),
                Errors = new List<string>()
            };

            // 创建大量适配文件
            for (int i = 0; i < 700; i++)
            {
                result.AdaptedFileInfos.Add(new XmlChecker.AdaptedFileInfo
                {
                    FileName = $"adapted_{i}.xml",
                    FileSize = 1024,
                    ModelName = $"Adapted{i}",
                    FullPath = $"/test/adapted_{i}.xml"
                });
            }

            // 创建大量未适配文件
            for (int i = 0; i < 300; i++)
            {
                result.UnadaptedFileInfos.Add(new XmlChecker.UnadaptedFileInfo
                {
                    FileName = $"unadapted_{i}.xml",
                    FileSize = 1024,
                    ExpectedModelName = $"Unadapted{i}",
                    FullPath = $"/test/unadapted_{i}.xml",
                    Complexity = AdaptationComplexity.Simple
                });
            }

            return result;
        }

        private XmlChecker.AdaptationCheckResult CreateMixedComplexityResult()
        {
            var result = new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = 10,
                AdaptedFiles = 0,
                UnadaptedFiles = 10,
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>()
            };

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

        private XmlChecker.AdaptationCheckResult CreateDifferentFileSizeResult()
        {
            return new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = 4,
                AdaptedFiles = 0,
                UnadaptedFiles = 4,
                UnadaptedFileInfos = new List<XmlChecker.UnadaptedFileInfo>
                {
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "small.xml",
                        FileSize = 512,
                        ExpectedModelName = "Small",
                        FullPath = "/test/small.xml",
                        Complexity = AdaptationComplexity.Simple
                    },
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "medium.xml",
                        FileSize = 1024,
                        ExpectedModelName = "Medium",
                        FullPath = "/test/medium.xml",
                        Complexity = AdaptationComplexity.Medium
                    },
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "large.xml",
                        FileSize = 1024 * 1024,
                        ExpectedModelName = "Large",
                        FullPath = "/test/large.xml",
                        Complexity = AdaptationComplexity.Large
                    },
                    new XmlChecker.UnadaptedFileInfo
                    {
                        FileName = "huge.xml",
                        FileSize = 1024 * 1024 * 1024,
                        ExpectedModelName = "Huge",
                        FullPath = "/test/huge.xml",
                        Complexity = AdaptationComplexity.Large
                    }
                },
                Errors = new List<string>()
            };
        }

        private IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            // 添加日志服务
            services.AddLogging(builder => builder.AddConsole());

            // 添加核心服务
            services.AddSingleton<FileDiscoveryService>();
            services.AddSingleton<IFileDiscoveryService>(sp => sp.GetRequiredService<FileDiscoveryService>());
            services.AddSingleton<IConfigurationValidator, ConfigurationValidator>();
            services.AddSingleton<IOutputFormatService, OutputFormatService>();

            // 添加配置
            services.AddSingleton(new AdaptationCheckerConfiguration());

            return services.BuildServiceProvider();
        }

        public void Dispose()
        {
            // 清理测试目录
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }
    }
}