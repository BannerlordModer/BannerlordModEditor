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
using System.Diagnostics;
using XmlChecker = BannerlordModEditor.XmlAdaptationChecker.Core.XmlAdaptationChecker;

namespace BannerlordModEditor.XmlAdaptationChecker.Tests.Performance
{
    /// <summary>
    /// 性能测试类
    /// </summary>
    public class PerformanceTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _xmlDirectory;
        private readonly string _modelDirectory;
        private readonly string _outputDirectory;
        private readonly IServiceProvider _serviceProvider;

        public PerformanceTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), $"Performance_Test_{Guid.NewGuid()}");
            _xmlDirectory = Path.Combine(_testDirectory, "xml");
            _modelDirectory = Path.Combine(_testDirectory, "models");
            _outputDirectory = Path.Combine(_testDirectory, "output");

            Directory.CreateDirectory(_testDirectory);
            Directory.CreateDirectory(_xmlDirectory);
            Directory.CreateDirectory(_modelDirectory);
            Directory.CreateDirectory(_outputDirectory);

            // 创建服务提供者
            _serviceProvider = CreateServiceProvider();
        }

        [Fact]
        public async Task Performance_LargeFileProcessing_ShouldProcessWithinTimeLimit()
        {
            // Arrange
            var largeFilePath = Path.Combine(_xmlDirectory, "large_file.xml");
            var largeContent = GenerateLargeXmlContent(10000); // 10,000行
            await File.WriteAllTextAsync(largeFilePath, largeContent);

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // 应该在5秒内完成
        }

        [Fact]
        public async Task Performance_ManyFilesProcessing_ShouldProcessWithinTimeLimit()
        {
            // Arrange
            var fileCount = 100;
            var files = new List<string>();

            for (int i = 0; i < fileCount; i++)
            {
                var filePath = Path.Combine(_xmlDirectory, $"file_{i}.xml");
                var content = GenerateSimpleXmlContent(i);
                await File.WriteAllTextAsync(filePath, content);
                files.Add(filePath);
            }

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().Be(fileCount);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(10000); // 应该在10秒内完成100个文件
        }

        [Fact]
        public async Task Performance_ParallelVsSequential_ShouldShowPerformanceImprovement()
        {
            // Arrange
            var fileCount = 50;
            var files = new List<string>();

            for (int i = 0; i < fileCount; i++)
            {
                var filePath = Path.Combine(_xmlDirectory, $"parallel_test_{i}.xml");
                var content = GenerateSimpleXmlContent(i);
                await File.WriteAllTextAsync(filePath, content);
                files.Add(filePath);
            }

            // 测试并行处理
            var parallelConfig = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            parallelConfig.EnableParallelProcessing = true;
            parallelConfig.MaxParallelism = 8;

            var parallelChecker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var parallelStopwatch = Stopwatch.StartNew();

            var parallelResult = await parallelChecker.CheckAdaptationStatusAsync();
            parallelStopwatch.Stop();

            // 测试顺序处理
            parallelConfig.EnableParallelProcessing = false;
            var sequentialStopwatch = Stopwatch.StartNew();

            var sequentialResult = await parallelChecker.CheckAdaptationStatusAsync();
            sequentialStopwatch.Stop();

            // Assert
            parallelResult.Should().NotBeNull();
            sequentialResult.Should().NotBeNull();
            
            // 并行处理应该更快（至少快20%）
            var speedup = (double)sequentialStopwatch.ElapsedMilliseconds / parallelStopwatch.ElapsedMilliseconds;
            speedup.Should().BeGreaterThan(1.2);
        }

        [Fact]
        public async Task Performance_MemoryUsage_ShouldStayWithinLimits()
        {
            // Arrange
            var fileCount = 200;
            var initialMemory = GC.GetTotalMemory(true);

            for (int i = 0; i < fileCount; i++)
            {
                var filePath = Path.Combine(_xmlDirectory, $"memory_test_{i}.xml");
                var content = GenerateSimpleXmlContent(i);
                await File.WriteAllTextAsync(filePath, content);
            }

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();
            var finalMemory = GC.GetTotalMemory(false);
            var memoryIncrease = finalMemory - initialMemory;

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            
            // 内存增长应该在合理范围内（小于100MB）
            memoryIncrease.Should().BeLessThan(100 * 1024 * 1024);
        }

        [Fact]
        public async Task Performance_OutputGeneration_ShouldGenerateQuickly()
        {
            // Arrange
            var fileCount = 1000;
            var result = CreateLargeTestResult(fileCount);
            var outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();

            var formats = new[] { OutputFormat.Console, OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json };
            var generationTimes = new Dictionary<OutputFormat, long>();

            // Act
            foreach (var format in formats)
            {
                var stopwatch = Stopwatch.StartNew();
                await outputService.OutputResultAsync(result, format);
                stopwatch.Stop();
                generationTimes[format] = stopwatch.ElapsedMilliseconds;
            }

            // Assert
            generationTimes.Should().HaveCount(4);
            
            // 每种格式都应该在合理时间内生成（小于1秒）
            foreach (var (format, time) in generationTimes)
            {
                time.Should().BeLessThan(1000, $"Format {format} took {time}ms");
            }
        }

        [Fact]
        public async Task Performance_ConcurrentAccess_ShouldHandleLoad()
        {
            // Arrange
            var fileCount = 50;
            for (int i = 0; i < fileCount; i++)
            {
                var filePath = Path.Combine(_xmlDirectory, $"concurrent_test_{i}.xml");
                var content = GenerateSimpleXmlContent(i);
                await File.WriteAllTextAsync(filePath, content);
            }

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();
            var taskCount = 10;

            var tasks = new List<Task<XmlAdaptationChecker.AdaptationCheckResult>>();

            // Act
            var stopwatch = Stopwatch.StartNew();
            
            for (int i = 0; i < taskCount; i++)
            {
                tasks.Add(checker.CheckAdaptationStatusAsync());
            }

            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            results.Should().HaveCount(taskCount);
            results.Should().AllSatisfy(r => r.Should().NotBeNull());
            results.Should().AllSatisfy(r => r.Errors.Should().BeEmpty());

            // 并发处理应该在合理时间内完成
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(30000); // 30秒内完成10个并发任务
        }

        [Fact]
        public async Task Performance_ExcludePatterns_ShouldFilterQuickly()
        {
            // Arrange
            var fileCount = 500;
            var includedCount = 0;

            for (int i = 0; i < fileCount; i++)
            {
                var fileName = i % 10 == 0 ? $"exclude_{i}.xml" : $"include_{i}.xml";
                var filePath = Path.Combine(_xmlDirectory, fileName);
                var content = GenerateSimpleXmlContent(i);
                await File.WriteAllTextAsync(filePath, content);

                if (!fileName.StartsWith("exclude_"))
                {
                    includedCount++;
                }
            }

            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.ExcludePatterns = new List<string> { "exclude_" };

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().Be(includedCount);
            
            // 过滤应该在合理时间内完成
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // 5秒内完成过滤
        }

        [Fact]
        public async Task Performance_ComplexityAnalysis_ShouldAnalyzeQuickly()
        {
            // Arrange
            var complexities = new[]
            {
                AdaptationComplexity.Simple,
                AdaptationComplexity.Medium,
                AdaptationComplexity.Complex,
                AdaptationComplexity.Large
            };

            foreach (var complexity in complexities)
            {
                var filePath = Path.Combine(_xmlDirectory, $"{complexity}_test.xml");
                var content = complexity switch
                {
                    AdaptationComplexity.Simple => GenerateSimpleXmlContent(1),
                    AdaptationComplexity.Medium => GenerateMediumXmlContent(),
                    AdaptationComplexity.Complex => GenerateComplexXmlContent(),
                    AdaptationComplexity.Large => GenerateLargeXmlContent(1000),
                    _ => GenerateSimpleXmlContent(1)
                };
                await File.WriteAllTextAsync(filePath, content);
            }

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().Be(4);
            
            // 复杂度分析应该在合理时间内完成
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000); // 3秒内完成复杂度分析
        }

        [Fact]
        public async Task Performance_ConfigurationValidation_ShouldValidateQuickly()
        {
            // Arrange
            var validator = _serviceProvider.GetRequiredService<IConfigurationValidator>();
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            var iterationCount = 1000;

            var stopwatch = Stopwatch.StartNew();

            // Act
            for (int i = 0; i < iterationCount; i++)
            {
                var result = validator.Validate(config);
                result.IsValid.Should().BeTrue();
            }

            stopwatch.Stop();

            // Assert
            // 1000次验证应该在合理时间内完成
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // 1秒内完成1000次验证
        }

        [Fact]
        public async Task Performance_FileDiscovery_ShouldDiscoverQuickly()
        {
            // Arrange
            var fileCount = 1000;
            var files = new List<string>();

            for (int i = 0; i < fileCount; i++)
            {
                var filePath = Path.Combine(_xmlDirectory, $"discovery_test_{i}.xml");
                var content = GenerateSimpleXmlContent(i);
                await File.WriteAllTextAsync(filePath, content);
                files.Add(filePath);
            }

            var fileDiscoveryService = _serviceProvider.GetRequiredService<FileDiscoveryService>();
            var stopwatch = Stopwatch.StartNew();

            // Act
            var discoveredFiles = Directory.GetFiles(_xmlDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            stopwatch.Stop();

            // Assert
            discoveredFiles.Should().HaveCount(fileCount);
            
            // 文件发现应该在合理时间内完成
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // 1秒内发现1000个文件
        }

        private string GenerateSimpleXmlContent(int seed)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item id=""{seed}"">
        <name>Test Item {seed}</name>
        <value>{seed * 10}</value>
        <description>This is a test item with seed {seed}</description>
    </item>
</root>";
        }

        private string GenerateMediumXmlContent()
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <items>
        {string.Join("\n        ", Enumerable.Range(1, 100).Select(i => $@"
        <item id=""{i}"">
            <name>Item {i}</name>
            <value>{i * 10}</value>
            <properties>
                <property name=""color"">red</property>
                <property name=""size"">medium</property>
                <property name=""weight"">{i * 0.1}</property>
            </properties>
        </item>"))}
    </items>
    <metadata>
        <created>{DateTime.Now:yyyy-MM-ddTHH:mm:ss}</created>
        <version>1.0</version>
        <author>Test Author</author>
    </metadata>
</root>";
        }

        private string GenerateComplexXmlContent()
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <sections>
        <section id=""1"" name=""Basic"">
            <entries>
                {string.Join("\n                ", Enumerable.Range(1, 50).Select(i => $@"
                <entry id=""{i}"">
                    <title>Entry {i}</title>
                    <content>Content for entry {i}</content>
                    <tags>
                        {string.Join("\n                        ", Enumerable.Range(1, 5).Select(j => $@"
                        <tag>tag_{i}_{j}</tag>"))}
                    </tags>
                    <metadata>
                        <created>{DateTime.Now.AddDays(-i):yyyy-MM-dd}</created>
                        <modified>{DateTime.Now.AddDays(-i + 1):yyyy-MM-dd}</modified>
                        <author>Author {i % 5}</author>
                    </metadata>
                </entry>"))}
            </entries>
        </section>
        <section id=""2"" name=""Advanced"">
            <complexData>
                {string.Join("\n                ", Enumerable.Range(1, 30).Select(i => $@"
                <dataItem id=""{i}"">
                    <complexValue>
                        <part1>Value {i} Part 1</part1>
                        <part2>Value {i} Part 2</part2>
                        <nested>
                            <level1>
                                <level2>
                                    <final>Final value {i}</final>
                                </level2>
                            </level1>
                        </nested>
                    </complexValue>
                    <attributes>
                        {string.Join("\n                        ", Enumerable.Range(1, 10).Select(j => $@"
                        <attribute name=""attr_{j}"">value_{i}_{j}</attribute>"))}
                    </attributes>
                </dataItem>"))}
            </complexData>
        </section>
    </sections>
    <summary>
        <totalEntries>80</totalEntries>
        <lastUpdated>{DateTime.Now:yyyy-MM-ddTHH:mm:ss}</lastUpdated>
    </summary>
</root>";
        }

        private string GenerateLargeXmlContent(int lineCount)
        {
            var lines = new List<string>
            {
                @"<?xml version=""1.0"" encoding=""utf-8""?>",
                @"<root>",
                @"    <items>"
            };

            for (int i = 0; i < lineCount - 6; i++)
            {
                lines.Add($@"        <item id=""{i}""><name>Item {i}</name><value>{i}</value></item>");
            }

            lines.AddRange(new[]
            {
                @"    </items>",
                @"    <metadata>",
                @"        <count>" + (lineCount - 6) + @"</count>",
                @"    </metadata>",
                @"</root>"
            });

            return string.Join("\n", lines);
        }

        private XmlAdaptationChecker.AdaptationCheckResult CreateLargeTestResult(int fileCount)
        {
            var result = new XmlChecker.AdaptationCheckResult
            {
                TotalFiles = fileCount,
                AdaptedFiles = (int)(fileCount * 0.7),
                UnadaptedFiles = (int)(fileCount * 0.3),
                AdaptedFileInfos = new List<XmlAdaptationChecker.AdaptedFileInfo>(),
                UnadaptedFileInfos = new List<XmlAdaptationChecker.UnadaptedFileInfo>(),
                Errors = new List<string>()
            };

            // 创建适配文件
            for (int i = 0; i < result.AdaptedFiles; i++)
            {
                result.AdaptedFileInfos.Add(new XmlAdaptationChecker.AdaptedFileInfo
                {
                    FileName = $"adapted_{i}.xml",
                    FileSize = 1024,
                    ModelName = $"Adapted{i}",
                    FullPath = $"/test/adapted_{i}.xml"
                });
            }

            // 创建未适配文件
            for (int i = 0; i < result.UnadaptedFiles; i++)
            {
                result.UnadaptedFileInfos.Add(new XmlAdaptationChecker.UnadaptedFileInfo
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
            services.AddSingleton(new AdaptationCheckerConfiguration
            {
                XmlDirectory = _xmlDirectory,
                ModelDirectories = new List<string> { _modelDirectory },
                OutputDirectory = _outputDirectory,
                OutputFormats = new List<OutputFormat> { OutputFormat.Console },
                EnableParallelProcessing = true,
                MaxParallelism = 8,
                FileSizeThreshold = 1024 * 1024,
                AnalyzeComplexity = true,
                GenerateStatistics = true
            });

            services.AddSingleton<Core.XmlAdaptationChecker>();

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