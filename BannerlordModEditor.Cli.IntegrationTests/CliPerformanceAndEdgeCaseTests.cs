using FluentAssertions;
using Xunit;

namespace BannerlordModEditor.Cli.IntegrationTests
{
    /// <summary>
    /// CLI性能和边界情况测试
    /// </summary>
    public class CliPerformanceAndEdgeCaseTests : CliIntegrationTestBase
    {
        [Fact]
        public async Task PerformanceTest_LargeXmlFile_ShouldHandleEfficiently()
        {
            // Arrange
            var inputFile = GetTestDataPath("flora_kinds.xml"); // 这是一个较大的XML文件
            var outputFile = GetTempFilePath(".xlsx");

            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                stopwatch.Stop();

                // Assert
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
                
                // 验证性能（应该在30秒内完成）
                stopwatch.ElapsedMilliseconds.Should().BeLessThan(30000, 
                    $"大型XML文件转换应该在30秒内完成，实际耗时: {stopwatch.ElapsedMilliseconds}ms");
                
                VerifyExcelFormat(outputFile);
            }
            finally
            {
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task EdgeCase_EmptyXmlFile_ShouldHandleGracefully()
        {
            // Arrange
            var inputFile = GetTempFilePath(".xml");
            var outputFile = GetTempFilePath(".xlsx");
            await File.WriteAllTextAsync(inputFile, "<?xml version=\"1.0\" encoding=\"utf-8\"?><root></root>");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert - CLI工具显示错误信息但返回成功码
                result.ShouldSucceed();
                result.ShouldContain("错误");
                result.ShouldContain("无法识别 XML 格式");
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task EdgeCase_XmlWithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var inputFile = GetTempFilePath(".xml");
            var outputFile = GetTempFilePath(".xlsx");
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
    <action name=""test&amp;action"" type=""swing&gt;thrust"" usage_direction=""one_handed"" action_stage=""attack&lt;special""/>
</action_types>";
            await File.WriteAllTextAsync(inputFile, xmlContent);

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
                
                VerifyExcelFormat(outputFile);
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task EdgeCase_VeryLongFilePath_ShouldHandleCorrectly()
        {
            // Arrange
            var longFileName = new string('a', 200) + ".xml";
            var inputFile = Path.Combine(_tempPath, longFileName);
            var outputFile = GetTempFilePath(".xlsx");
            
            // 创建一个简单的XML文件
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
    <action name=""test"" type=""swing"" usage_direction=""one_handed"" action_stage=""attack""/>
</action_types>";
            await File.WriteAllTextAsync(inputFile, xmlContent);

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
                
                VerifyExcelFormat(outputFile);
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task EdgeCase_OutputFileAlreadyExists_ShouldOverwrite()
        {
            // Arrange
            var inputFile = GetTestDataPath("action_types.xml");
            var outputFile = GetTempFilePath(".xlsx");
            
            // 预先创建输出文件
            await File.WriteAllTextAsync(outputFile, "existing content");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
                
                VerifyExcelFormat(outputFile);
                
                // 验证文件确实被覆盖了
                var content = await File.ReadAllTextAsync(outputFile);
                content.Should().NotContain("existing content", "输出文件应该被覆盖");
            }
            finally
            {
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task EdgeCase_InvalidCommand_ShouldShowHelp()
        {
            // Act
            var result = await ExecuteCliCommandAsync("invalid-command");

            // Assert - CliFx对无效命令显示帮助信息并返回成功码
            result.ShouldSucceed();
            result.ShouldContain("USAGE");
            result.ShouldContain("Unexpected parameter");
        }

        [Fact]
        public async Task EdgeCase_UnrecognizedXml_ShouldShowError()
        {
            // Arrange
            var inputFile = GetTempFilePath(".xml");
            var outputFile = GetTempFilePath(".xlsx");
            await File.WriteAllTextAsync(inputFile, "<?xml version=\"1.0\"?><unknown_type><item>test</item></unknown_type>");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert - CLI工具显示错误信息但返回成功码
                result.ShouldSucceed();
                result.ShouldContain("错误");
                result.ShouldContain("无法识别 XML 格式");
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task StressTest_MultipleConsecutiveCommands_ShouldWork()
        {
            // Arrange
            var inputFile = GetTestDataPath("action_types.xml");
            var outputFile1 = GetTempFilePath(".xlsx");
            var outputFile2 = GetTempFilePath(".xlsx");
            var outputFile3 = GetTempFilePath(".xlsx");

            try
            {
                // Act - 连续执行多个命令
                var result1 = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile1}\"");
                var result2 = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile2}\"");
                var result3 = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile3}\"");

                // Assert
                result1.ShouldSucceed();
                result2.ShouldSucceed();
                result3.ShouldSucceed();
                
                VerifyExcelFormat(outputFile1);
                VerifyExcelFormat(outputFile2);
                VerifyExcelFormat(outputFile3);
            }
            finally
            {
                CleanupTempFile(outputFile1);
                CleanupTempFile(outputFile2);
                CleanupTempFile(outputFile3);
            }
        }

        [Fact]
        public async Task MemoryTest_LargeDataSet_ShouldNotCrash()
        {
            // Arrange
            var inputFile = GetTempFilePath(".xml");
            var outputFile = GetTempFilePath(".xlsx");
            
            // 创建一个包含大量数据的XML文件
            var largeXmlContent = GenerateLargeXmlContent(1000); // 1000个条目
            await File.WriteAllTextAsync(inputFile, largeXmlContent);

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"", TimeSpan.FromMinutes(2));

                // Assert
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
                
                VerifyExcelFormat(outputFile);
                
                // 验证输出文件大小合理
                var fileInfo = new FileInfo(outputFile);
                fileInfo.Length.Should().BeGreaterThan(1024 * 20, "大型数据集的Excel文件应该大于20KB");
            }
            finally
            {
                CleanupTempFile(inputFile);
                CleanupTempFile(outputFile);
            }
        }

        [Fact]
        public async Task EdgeCase_NonexistentOutputDirectory_ShouldCreateDirectory()
        {
            // Arrange
            var inputFile = GetTestDataPath("action_types.xml");
            var outputDir = Path.Combine(_tempPath, $"test_output_{Guid.NewGuid()}");
            var outputFile = Path.Combine(outputDir, "output.xlsx");

            try
            {
                // Act
                var result = await ExecuteCliCommandAsync($"convert -i \"{inputFile}\" -o \"{outputFile}\"");

                // Assert
                result.ShouldSucceed();
                result.ShouldContain("✓ XML 转 Excel 转换成功");
                
                VerifyExcelFormat(outputFile);
                
                // 验证目录被创建
                Directory.Exists(outputDir).Should().BeTrue("输出目录应该被创建");
            }
            finally
            {
                CleanupTempFile(outputFile);
                if (Directory.Exists(outputDir))
                {
                    Directory.Delete(outputDir, true);
                }
            }
        }

        /// <summary>
        /// 生成大型XML内容用于测试
        /// </summary>
        private string GenerateLargeXmlContent(int itemCount)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<action_types>");
            
            for (int i = 0; i < itemCount; i++)
            {
                sb.AppendLine($"  <action name=\"action_{i}\" type=\"type_{i % 10}\" usage_direction=\"direction_{i % 5}\" action_stage=\"stage_{i % 3}\"/>");
            }
            
            sb.AppendLine("</action_types>");
            return sb.ToString();
        }
    }
}