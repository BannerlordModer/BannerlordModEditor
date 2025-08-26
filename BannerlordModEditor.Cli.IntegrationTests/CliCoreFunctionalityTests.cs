using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.IntegrationTests
{
    /// <summary>
    /// CLI核心功能的集成测试
    /// </summary>
    public class CliCoreFunctionalityTests : CliIntegrationTestBase
    {
        [Fact]
        public async Task ListModelsCommand_ShouldReturnAllSupportedModels()
        {
            // Act
            var result = await ExecuteCliCommandAsync("list-models");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("支持的模型类型:");
            result.ShouldContain("ActionTypesDO");
            result.ShouldContain("CombatParametersDO");
            result.ShouldContain("MapIconsDO");
            
            var modelTypes = result.GetModelTypes();
            modelTypes.Should().HaveCountGreaterThan(30, "应该支持至少30种模型类型");
            modelTypes.Should().Contain("ActionTypesDO", "应该支持ActionTypesDO模型");
        }

        [Fact]
        public async Task HelpCommand_ShouldDisplayUsageInformation()
        {
            // Act
            var result = await ExecuteCliCommandAsync("--help");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("BannerlordModEditor.Cli");
            result.ShouldContain("USAGE");
            result.ShouldContain("OPTIONS");
            result.ShouldContain("COMMANDS");
            result.ShouldContain("convert");
            result.ShouldContain("list-models");
            result.ShouldContain("recognize");
        }

        [Fact]
        public async Task VersionCommand_ShouldDisplayVersion()
        {
            // Act
            var result = await ExecuteCliCommandAsync("--version");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("v1.0.0");
        }

        [Fact]
        public async Task RecognizeCommand_WithValidXml_ShouldIdentifyModelType()
        {
            // Arrange
            var xmlFile = GetTestDataPath("action_types.xml");

            // Act
            var result = await ExecuteCliCommandAsync($"recognize -i \"{xmlFile}\"");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("✓ 识别成功");
            var modelType = result.GetRecognizedModelType();
            modelType.Should().Be("action_types", "应该正确识别action_types.xml");
        }

        [Fact]
        public async Task RecognizeCommand_WithCombatParametersXml_ShouldIdentifyModelType()
        {
            // Arrange
            var xmlFile = GetTestDataPath("combat_parameters.xml");

            // Act
            var result = await ExecuteCliCommandAsync($"recognize -i \"{xmlFile}\"");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("✓ 识别成功");
            var modelType = result.GetRecognizedModelType();
            modelType.Should().Be("CombatParametersDO", "应该正确识别combat_parameters.xml");
        }

        [Fact]
        public async Task RecognizeCommand_WithInvalidXml_ShouldShowError()
        {
            // Arrange
            var invalidXmlFile = GetTempFilePath(".xml");
            await File.WriteAllTextAsync(invalidXmlFile, "invalid xml content");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"recognize -i \"{invalidXmlFile}\"");

                // Assert
                result.ShouldFailWithError("错误");
                result.ShouldContain("XML 格式识别失败");
            }
            finally
            {
                CleanupTempFile(invalidXmlFile);
            }
        }

        [Fact]
        public async Task RecognizeCommand_WithNonexistentFile_ShouldShowError()
        {
            // Arrange
            var nonexistentFile = "/path/to/nonexistent/file.xml";

            // Act
            var result = await ExecuteCliCommandAsync($"recognize -i \"{nonexistentFile}\"");

            // Assert
            result.ShouldFailWithError("错误");
            result.ShouldContain("文件不存在");
        }

        [Fact]
        public async Task RecognizeCommand_WithVerboseFlag_ShouldShowDetailedInfo()
        {
            // Arrange
            var xmlFile = GetTestDataPath("action_types.xml");

            // Act
            var result = await ExecuteCliCommandAsync($"recognize -i \"{xmlFile}\" --verbose");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("✓ 识别成功");
            result.ShouldContain("详细信息");
        }

        [Fact]
        public async Task ConvertCommand_Help_ShouldShowUsage()
        {
            // Act
            var result = await ExecuteCliCommandAsync("convert --help");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("Excel 和 XML 文件之间的相互转换");
            result.ShouldContain("--input");
            result.ShouldContain("--output");
            result.ShouldContain("--model");
            result.ShouldContain("--validate");
        }

        [Fact]
        public async Task ConvertCommand_WithMissingRequiredParameters_ShouldShowHelp()
        {
            // Act
            var result = await ExecuteCliCommandAsync("convert");

            // Assert
            result.ShouldSucceed();
            result.ShouldContain("USAGE");
            result.ShouldContain("Missing required option(s)");
            result.ShouldContain("--input");
            result.ShouldContain("--output");
        }

        [Fact]
        public async Task ConvertCommand_WithInvalidFileExtension_ShouldShowError()
        {
            // Arrange
            var inputFile = GetTempFilePath(".txt");
            var outputFile = GetTempFilePath(".xml");
            await File.WriteAllTextAsync(inputFile, "test content");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert
                result.ShouldFailWithError("错误");
                result.ShouldContain("不支持的输入文件格式");
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task ConvertCommand_WithSameInputOutputFormat_ShouldShowError()
        {
            // Arrange
            var inputFile = GetTempFilePath(".xml");
            var outputFile = GetTempFilePath(".xml");
            await File.WriteAllTextAsync(inputFile, "<test>content</test>");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert
                result.ShouldFailWithError("错误");
                result.ShouldContain("输入和输出文件格式相同");
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }
    }
}