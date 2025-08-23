using BannerlordModEditor.XmlAdaptationChecker;
using Xunit;
using FluentAssertions;

namespace BannerlordModEditor.XmlAdaptationChecker.Tests
{
    /// <summary>
    /// AdaptationCheckerConfiguration 单元测试
    /// </summary>
    public class AdaptationCheckerConfigurationTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var config = new AdaptationCheckerConfiguration();

            // Assert
            config.XmlDirectory.Should().Be(string.Empty);
            config.ModelDirectories.Should().BeEmpty();
            config.OutputDirectory.Should().Be(string.Empty);
            config.OutputFormats.Should().BeEmpty();
            config.VerboseLogging.Should().BeFalse();
            config.EnableParallelProcessing.Should().BeTrue();
            config.MaxParallelism.Should().Be(Environment.ProcessorCount);
            config.FileSizeThreshold.Should().Be(1024 * 1024); // 1MB
            config.AnalyzeComplexity.Should().BeTrue();
            config.GenerateStatistics.Should().BeTrue();
            config.ExcludePatterns.Should().BeEmpty();
        }

        [Fact]
        public void XmlDirectory_ShouldAllowSettingValidPath()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var validPath = "/valid/path";

            // Act
            config.XmlDirectory = validPath;

            // Assert
            config.XmlDirectory.Should().Be(validPath);
        }

        [Fact]
        public void XmlDirectory_ShouldAllowEmptyString()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.XmlDirectory = "";

            // Assert
            config.XmlDirectory.Should().Be("");
        }

        [Fact]
        public void XmlDirectory_ShouldAllowNull()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.XmlDirectory = null!;

            // Assert
            config.XmlDirectory.Should().BeNull();
        }

        [Fact]
        public void ModelDirectories_ShouldAllowSettingValidDirectories()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var directories = new List<string> { "/path1", "/path2" };

            // Act
            config.ModelDirectories = directories;

            // Assert
            config.ModelDirectories.Should().BeEquivalentTo(directories);
        }

        [Fact]
        public void ModelDirectories_ShouldAllowEmptyList()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.ModelDirectories = new List<string>();

            // Assert
            config.ModelDirectories.Should().BeEmpty();
        }

        [Fact]
        public void ModelDirectories_ShouldAllowNull()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.ModelDirectories = null!;

            // Assert
            config.ModelDirectories.Should().BeNull();
        }

        [Fact]
        public void OutputDirectory_ShouldAllowSettingValidPath()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var validPath = "/valid/output";

            // Act
            config.OutputDirectory = validPath;

            // Assert
            config.OutputDirectory.Should().Be(validPath);
        }

        [Fact]
        public void OutputFormats_ShouldAllowSettingValidFormats()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var formats = new List<OutputFormat> { OutputFormat.Console, OutputFormat.Json };

            // Act
            config.OutputFormats = formats;

            // Assert
            config.OutputFormats.Should().BeEquivalentTo(formats);
        }

        [Fact]
        public void OutputFormats_ShouldAllowEmptyList()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.OutputFormats = new List<OutputFormat>();

            // Assert
            config.OutputFormats.Should().BeEmpty();
        }

        [Fact]
        public void VerboseLogging_ShouldAllowSettingTrue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.VerboseLogging = true;

            // Assert
            config.VerboseLogging.Should().BeTrue();
        }

        [Fact]
        public void VerboseLogging_ShouldAllowSettingFalse()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.VerboseLogging = false;

            // Assert
            config.VerboseLogging.Should().BeFalse();
        }

        [Fact]
        public void EnableParallelProcessing_ShouldAllowSettingTrue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.EnableParallelProcessing = true;

            // Assert
            config.EnableParallelProcessing.Should().BeTrue();
        }

        [Fact]
        public void EnableParallelProcessing_ShouldAllowSettingFalse()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.EnableParallelProcessing = false;

            // Assert
            config.EnableParallelProcessing.Should().BeFalse();
        }

        [Fact]
        public void MaxParallelism_ShouldAllowSettingValidValue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var validValue = 8;

            // Act
            config.MaxParallelism = validValue;

            // Assert
            config.MaxParallelism.Should().Be(validValue);
        }

        [Fact]
        public void MaxParallelism_ShouldAllowSettingZero()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.MaxParallelism = 0;

            // Assert
            config.MaxParallelism.Should().Be(0);
        }

        [Fact]
        public void MaxParallelism_ShouldAllowSettingNegativeValue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.MaxParallelism = -1;

            // Assert
            config.MaxParallelism.Should().Be(-1);
        }

        [Fact]
        public void FileSizeThreshold_ShouldAllowSettingValidValue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var validValue = 2048;

            // Act
            config.FileSizeThreshold = validValue;

            // Assert
            config.FileSizeThreshold.Should().Be(validValue);
        }

        [Fact]
        public void FileSizeThreshold_ShouldAllowSettingZero()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.FileSizeThreshold = 0;

            // Assert
            config.FileSizeThreshold.Should().Be(0);
        }

        [Fact]
        public void FileSizeThreshold_ShouldAllowSettingNegativeValue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.FileSizeThreshold = -1;

            // Assert
            config.FileSizeThreshold.Should().Be(-1);
        }

        [Fact]
        public void AnalyzeComplexity_ShouldAllowSettingTrue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.AnalyzeComplexity = true;

            // Assert
            config.AnalyzeComplexity.Should().BeTrue();
        }

        [Fact]
        public void AnalyzeComplexity_ShouldAllowSettingFalse()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.AnalyzeComplexity = false;

            // Assert
            config.AnalyzeComplexity.Should().BeFalse();
        }

        [Fact]
        public void GenerateStatistics_ShouldAllowSettingTrue()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.GenerateStatistics = true;

            // Assert
            config.GenerateStatistics.Should().BeTrue();
        }

        [Fact]
        public void GenerateStatistics_ShouldAllowSettingFalse()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.GenerateStatistics = false;

            // Assert
            config.GenerateStatistics.Should().BeFalse();
        }

        [Fact]
        public void ExcludePatterns_ShouldAllowSettingValidPatterns()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();
            var patterns = new List<string> { "test_", "temp_", "*.bak" };

            // Act
            config.ExcludePatterns = patterns;

            // Assert
            config.ExcludePatterns.Should().BeEquivalentTo(patterns);
        }

        [Fact]
        public void ExcludePatterns_ShouldAllowEmptyList()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.ExcludePatterns = new List<string>();

            // Assert
            config.ExcludePatterns.Should().BeEmpty();
        }

        [Fact]
        public void ExcludePatterns_ShouldAllowNull()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.ExcludePatterns = null!;

            // Assert
            config.ExcludePatterns.Should().BeNull();
        }

        [Fact]
        public void Configuration_ShouldAllowCompleteCustomization()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration();

            // Act
            config.XmlDirectory = "/custom/xml/path";
            config.ModelDirectories = new List<string> { "/custom/models1", "/custom/models2" };
            config.OutputDirectory = "/custom/output";
            config.OutputFormats = new List<OutputFormat> { OutputFormat.Json, OutputFormat.Csv };
            config.VerboseLogging = true;
            config.EnableParallelProcessing = false;
            config.MaxParallelism = 2;
            config.FileSizeThreshold = 512;
            config.AnalyzeComplexity = false;
            config.GenerateStatistics = false;
            config.ExcludePatterns = new List<string> { "exclude_" };

            // Assert
            config.XmlDirectory.Should().Be("/custom/xml/path");
            config.ModelDirectories.Should().BeEquivalentTo(new List<string> { "/custom/models1", "/custom/models2" });
            config.OutputDirectory.Should().Be("/custom/output");
            config.OutputFormats.Should().BeEquivalentTo(new List<OutputFormat> { OutputFormat.Json, OutputFormat.Csv });
            config.VerboseLogging.Should().BeTrue();
            config.EnableParallelProcessing.Should().BeFalse();
            config.MaxParallelism.Should().Be(2);
            config.FileSizeThreshold.Should().Be(512);
            config.AnalyzeComplexity.Should().BeFalse();
            config.GenerateStatistics.Should().BeFalse();
            config.ExcludePatterns.Should().BeEquivalentTo(new List<string> { "exclude_" });
        }

        [Fact]
        public void Configuration_ShouldMaintainDefaultValuesForUnsetProperties()
        {
            // Arrange & Act
            var config = new AdaptationCheckerConfiguration();

            // 只设置一个属性
            config.XmlDirectory = "/some/path";

            // Assert
            config.XmlDirectory.Should().Be("/some/path");
            config.ModelDirectories.Should().BeEmpty(); // 保持默认值
            config.OutputDirectory.Should().Be(string.Empty); // 保持默认值
            config.OutputFormats.Should().BeEmpty(); // 保持默认值
            config.VerboseLogging.Should().BeFalse(); // 保持默认值
            config.EnableParallelProcessing.Should().BeTrue(); // 保持默认值
            config.MaxParallelism.Should().Be(Environment.ProcessorCount); // 保持默认值
            config.FileSizeThreshold.Should().Be(1024 * 1024); // 保持默认值
            config.AnalyzeComplexity.Should().BeTrue(); // 保持默认值
            config.GenerateStatistics.Should().BeTrue(); // 保持默认值
            config.ExcludePatterns.Should().BeEmpty(); // 保持默认值
        }

        [Fact]
        public void Configuration_ShouldBeSerializable()
        {
            // Arrange
            var config = new AdaptationCheckerConfiguration
            {
                XmlDirectory = "/test/xml",
                ModelDirectories = new List<string> { "/test/models" },
                OutputDirectory = "/test/output",
                OutputFormats = new List<OutputFormat> { OutputFormat.Json },
                VerboseLogging = true,
                EnableParallelProcessing = false,
                MaxParallelism = 4,
                FileSizeThreshold = 2048,
                AnalyzeComplexity = false,
                GenerateStatistics = true,
                ExcludePatterns = new List<string> { "test_" }
            };

            // Act & Assert - 简单的序列化测试
            // 这个测试确保配置对象可以被序列化（没有循环引用等）
            var json = System.Text.Json.JsonSerializer.Serialize(config);
            json.Should().NotBeNullOrEmpty();
            json.Should().Contain("/test/xml");
            json.Should().Contain("/test/models");
            json.Should().Contain("Json");
        }
    }
}