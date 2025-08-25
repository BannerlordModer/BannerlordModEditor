using BannerlordModEditor.XmlAdaptationChecker.Validators;
using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.XmlAdaptationChecker.Tests.Validators
{
    /// <summary>
    /// ConfigurationValidator 单元测试
    /// </summary>
    public class ConfigurationValidatorTests
    {
        private readonly ConfigurationValidator _validator;

        public ConfigurationValidatorTests()
        {
            _validator = new ConfigurationValidator();
        }

        [Fact]
        public void Validate_WithValidConfiguration_ShouldReturnValidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/tmp/test",
                ModelDirectories = new List<string> { "/tmp/models" },
                OutputFormats = new List<OutputFormat> { OutputFormat.Console },
                MaxParallelism = 4,
                FileSizeThreshold = 1024 * 1024
            };

            // Mock directory existence
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.CreateDirectory(config.XmlDirectory);
            mockFileSystem.Directory.CreateDirectory(config.ModelDirectories.First());

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Validate_WithEmptyXmlDirectory_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "",
                ModelDirectories = new List<string> { "/tmp/models" },
                OutputFormats = new List<OutputFormat> { OutputFormat.Console }
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("XML目录路径不能为空");
        }

        [Fact]
        public void Validate_WithNonExistentXmlDirectory_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/non/existent/path",
                ModelDirectories = new List<string> { "/tmp/models" },
                OutputFormats = new List<OutputFormat> { OutputFormat.Console }
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("XML目录不存在: /non/existent/path");
        }

        [Fact]
        public void Validate_WithEmptyModelDirectories_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/tmp/test",
                ModelDirectories = new List<string>(),
                OutputFormats = new List<OutputFormat> { OutputFormat.Console }
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("至少需要指定一个模型目录");
        }

        [Fact]
        public void Validate_WithNonExistentModelDirectory_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/tmp/test",
                ModelDirectories = new List<string> { "/non/existent/models" },
                OutputFormats = new List<OutputFormat> { OutputFormat.Console }
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("模型目录不存在: /non/existent/models");
        }

        [Fact]
        public void Validate_WithEmptyOutputFormats_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/tmp/test",
                ModelDirectories = new List<string> { "/tmp/models" },
                OutputFormats = new List<OutputFormat>()
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("至少需要指定一种输出格式");
        }

        [Fact]
        public void Validate_WithInvalidMaxParallelism_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/tmp/test",
                ModelDirectories = new List<string> { "/tmp/models" },
                OutputFormats = new List<OutputFormat> { OutputFormat.Console },
                MaxParallelism = 0
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("最大并行度必须大于0");
        }

        [Fact]
        public void Validate_WithNegativeFileSizeThreshold_ShouldReturnInvalidResult()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/tmp/test",
                ModelDirectories = new List<string> { "/tmp/models" },
                OutputFormats = new List<OutputFormat> { OutputFormat.Console },
                FileSizeThreshold = -1
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain("文件大小阈值不能为负数");
        }

        [Fact]
        public void Validate_WithMultipleErrors_ShouldReturnAllErrors()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "",
                ModelDirectories = new List<string>(),
                OutputFormats = new List<OutputFormat>(),
                MaxParallelism = -1,
                FileSizeThreshold = -1
            };

            // Act
            var result = _validator.Validate(config);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(3);
            result.Errors.Should().Contain("XML目录路径不能为空");
            result.Errors.Should().Contain("至少需要指定一个模型目录");
            result.Errors.Should().Contain("至少需要指定一种输出格式");
            result.Errors.Should().Contain("最大并行度必须大于0");
            result.Errors.Should().Contain("文件大小阈值不能为负数");
        }
    }

    /// <summary>
    /// Mock file system for testing
    /// </summary>
    internal class MockFileSystem
    {
        public MockDirectory Directory { get; } = new MockDirectory();
    }

    internal class MockDirectory
    {
        private readonly HashSet<string> _directories = new();

        public void CreateDirectory(string path)
        {
            _directories.Add(path);
        }

        public bool Exists(string path)
        {
            return _directories.Contains(path);
        }
    }
}
