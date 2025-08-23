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

namespace BannerlordModEditor.XmlAdaptationChecker.Tests.Integration
{
    /// <summary>
    /// 完整检查流程的集成测试
    /// </summary>
    public class CompleteCheckFlowIntegrationTests : IDisposable
    {
        private readonly string _testDirectory;
        private readonly string _xmlDirectory;
        private readonly string _modelDirectory;
        private readonly string _outputDirectory;
        private readonly IServiceProvider _serviceProvider;

        public CompleteCheckFlowIntegrationTests()
        {
            // 创建测试目录
            _testDirectory = Path.Combine(Path.GetTempPath(), $"XmlAdaptationChecker_Test_{Guid.NewGuid()}");
            _xmlDirectory = Path.Combine(_testDirectory, "xml");
            _modelDirectory = Path.Combine(_testDirectory, "models");
            _outputDirectory = Path.Combine(_testDirectory, "output");

            Directory.CreateDirectory(_testDirectory);
            Directory.CreateDirectory(_xmlDirectory);
            Directory.CreateDirectory(_modelDirectory);
            Directory.CreateDirectory(_outputDirectory);

            // 创建测试文件
            CreateTestFiles();

            // 创建服务提供者
            _serviceProvider = CreateServiceProvider();
        }

        [Fact]
        public async Task CompleteCheckFlow_WithValidConfiguration_ShouldExecuteSuccessfully()
        {
            // Arrange
            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().BeGreaterThan(0);
            result.AdaptationRate.Should().BeGreaterThanOrEqualTo(0);
            result.AdaptationRate.Should().BeLessThanOrEqualTo(100);
        }

        [Fact]
        public async Task CompleteCheckFlow_WithOutputFormats_ShouldGenerateOutputFiles()
        {
            // Arrange
            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // 生成各种格式的输出
            var consoleOutput = await outputService.OutputResultAsync(result, OutputFormat.Console);
            var markdownOutput = await outputService.OutputResultAsync(result, OutputFormat.Markdown);
            var csvOutput = await outputService.OutputResultAsync(result, OutputFormat.Csv);
            var jsonOutput = await outputService.OutputResultAsync(result, OutputFormat.Json);

            // Assert
            consoleOutput.Should().NotBeNullOrEmpty();
            markdownOutput.Should().NotBeNullOrEmpty();
            csvOutput.Should().NotBeNullOrEmpty();
            jsonOutput.Should().NotBeNullOrEmpty();

            // 验证输出内容
            consoleOutput.Should().Contain("XML适配状态检查结果");
            markdownOutput.Should().Contain("# XML适配状态报告");
            csvOutput.Should().Contain("XML适配状态报告");
            jsonOutput.Should().Contain("\"totalFiles\"");
        }

        [Fact]
        public async Task CompleteCheckFlow_WithOutputToFile_ShouldCreateFiles()
        {
            // Arrange
            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();
            var outputService = _serviceProvider.GetRequiredService<IOutputFormatService>();
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();

            var markdownPath = Path.Combine(_outputDirectory, "test_report.md");
            var jsonPath = Path.Combine(_outputDirectory, "test_report.json");

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            await outputService.OutputResultAsync(result, OutputFormat.Markdown, markdownPath);
            await outputService.OutputResultAsync(result, OutputFormat.Json, jsonPath);

            // Assert
            File.Exists(markdownPath).Should().BeTrue();
            File.Exists(jsonPath).Should().BeTrue();

            var markdownContent = await File.ReadAllTextAsync(markdownPath);
            var jsonContent = await File.ReadAllTextAsync(jsonPath);

            markdownContent.Should().NotBeNullOrEmpty();
            jsonContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CompleteCheckFlow_WithParallelProcessing_ShouldExecuteSuccessfully()
        {
            // Arrange
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.EnableParallelProcessing = true;
            config.MaxParallelism = 4;

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CompleteCheckFlow_WithSequentialProcessing_ShouldExecuteSuccessfully()
        {
            // Arrange
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.EnableParallelProcessing = false;

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CompleteCheckFlow_WithExcludePatterns_ShouldExcludeFiles()
        {
            // Arrange
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.ExcludePatterns = new List<string> { "exclude_" };

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            
            // 验证被排除的文件不在结果中
            var excludedFiles = result.UnadaptedFileInfos
                .Where(f => f.FileName.Contains("exclude_"))
                .ToList();
            excludedFiles.Should().BeEmpty();
        }

        [Fact]
        public async Task CompleteCheckFlow_WithDifferentComplexityLevels_ShouldAnalyzeCorrectly()
        {
            // Arrange
            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            // 验证复杂度分析
            var complexityLevels = result.UnadaptedFileInfos
                .Select(f => f.Complexity)
                .Distinct()
                .ToList();
            
            complexityLevels.Should().NotBeEmpty();
            complexityLevels.Should().Contain(AdaptationComplexity.Simple);
        }

        [Fact]
        public async Task CompleteCheckFlow_WithConfigurationValidation_ShouldValidateCorrectly()
        {
            // Arrange
            var validator = _serviceProvider.GetRequiredService<IConfigurationValidator>();
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();

            // Act
            var validationResult = validator.Validate(config);

            // Assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task CompleteCheckFlow_WithInvalidConfiguration_ShouldReturnErrors()
        {
            // Arrange
            var validator = _serviceProvider.GetRequiredService<IConfigurationValidator>();
            var invalidConfig = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/non/existent/path",
                ModelDirectories = new List<string>(),
                OutputFormats = new List<OutputFormat>()
            };

            // Act
            var validationResult = validator.Validate(invalidConfig);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().NotBeEmpty();
            validationResult.Errors.Should().Contain("XML目录不存在");
            validationResult.Errors.Should().Contain("至少需要指定一个模型目录");
            validationResult.Errors.Should().Contain("至少需要指定一种输出格式");
        }

        [Fact]
        public async Task CompleteCheckFlow_WithSummaryGeneration_ShouldGenerateSummary()
        {
            // Arrange
            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var summary = await checker.GetUnadaptedFilesSummaryAsync();

            // Assert
            summary.Should().NotBeNullOrEmpty();
            summary.Should().Contain("XML适配状态摘要");
            summary.Should().Contain("总文件数");
            summary.Should().Contain("已适配");
            summary.Should().Contain("未适配");
            summary.Should().Contain("适配率");
        }

        [Fact]
        public async Task CompleteCheckFlow_WithErrorHandling_ShouldHandleExceptions()
        {
            // Arrange
            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.XmlDirectory = "/non/existent/path";

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().Contain(e => e.Contains("XML目录不存在"));
        }

        [Fact]
        public async Task CompleteCheckFlow_WithLargeFileHandling_ShouldHandleLargeFiles()
        {
            // Arrange
            var largeFilePath = Path.Combine(_xmlDirectory, "large_file.xml");
            var largeContent = new string('x', 1024 * 1024); // 1MB
            await File.WriteAllTextAsync(largeFilePath, largeContent);

            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.FileSizeThreshold = 512 * 1024; // 512KB

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();

            // 验证大文件被正确标记为Large复杂度
            var largeFiles = result.UnadaptedFileInfos
                .Where(f => f.Complexity == AdaptationComplexity.Large)
                .ToList();
            largeFiles.Should().Contain(f => f.FileName == "large_file.xml");
        }

        [Fact]
        public async Task CompleteCheckFlow_WithEmptyDirectory_ShouldHandleEmptyCase()
        {
            // Arrange
            var emptyXmlDir = Path.Combine(_testDirectory, "empty_xml");
            Directory.CreateDirectory(emptyXmlDir);

            var config = _serviceProvider.GetRequiredService<AdaptationCheckerConfiguration>();
            config.XmlDirectory = emptyXmlDir;

            var checker = _serviceProvider.GetRequiredService<Core.XmlAdaptationChecker>();

            // Act
            var result = await checker.CheckAdaptationStatusAsync();

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
            result.TotalFiles.Should().Be(0);
            result.AdaptedFiles.Should().Be(0);
            result.UnadaptedFiles.Should().Be(0);
            result.AdaptationRate.Should().Be(0);
        }

        private void CreateTestFiles()
        {
            // 创建一些测试XML文件
            var testFiles = new[]
            {
                ("adapted_file1.xml", "<root><item>test</item></root>"),
                ("adapted_file2.xml", "<root><data>value</data></root>"),
                ("unadapted_file1.xml", "<root><test>data</test></root>"),
                ("unadapted_file2.xml", "<root><example>content</example></root>"),
                ("exclude_file.xml", "<root><exclude>true</exclude></root>")
            };

            foreach (var (fileName, content) in testFiles)
            {
                var filePath = Path.Combine(_xmlDirectory, fileName);
                File.WriteAllText(filePath, content);
            }
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
                OutputFormats = new List<OutputFormat> { OutputFormat.Console, OutputFormat.Markdown, OutputFormat.Csv, OutputFormat.Json },
                EnableParallelProcessing = true,
                MaxParallelism = 4,
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