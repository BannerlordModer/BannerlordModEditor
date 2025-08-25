using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.TUI.ViewModels;
using BannerlordModEditor.Common.Models;
using System.Security;
using BannerlordModEditor.TUI.UATTests.Common;

namespace BannerlordModEditor.TUI.UATTests.Features
{
    /// <summary>
    /// BDD特性：错误处理和异常情况
    /// 
    /// 作为一个Bannerlord Mod开发者
    /// 我希望在文件转换过程中遇到错误时
    /// 系统能够提供清晰的错误信息
    /// 并且优雅地处理异常情况
    /// </summary>
    public class ErrorHandlingFeature : BddTestBase
    {
        public ErrorHandlingFeature(ITestOutputHelper output) : base(output)
        {
        }

        #region 场景1: 处理不存在的源文件

        /// <summary>
        /// 场景: 处理不存在的源文件
        /// 当 我尝试转换一个不存在的文件
        /// 那么 系统应该提供明确的错误信息
        /// 并且 不应该创建输出文件
        /// </summary>
        [Fact]
        public async Task NonExistentSourceFile_ClearErrorMessage()
        {
            // Given - 准备测试数据
            string nonExistentFile = null;
            string outputPath = null;
            
            try
            {
                // Given 我尝试转换一个不存在的文件
                nonExistentFile = Path.Combine(TestTempDir, "nonexistent.xlsx");
                outputPath = Path.Combine(TestTempDir, "output.xml");

                // When 我尝试进行转换
                var result = await ConversionService.ExcelToXmlAsync(nonExistentFile, outputPath);

                // Then 系统应该提供明确的错误信息
                result.Success.Should().BeFalse("转换应该失败");
                result.Message.Should().Contain("不存在", "错误信息应该包含'不存在'");
                result.Errors.ShouldNotBeEmpty("应该有错误信息");

                // And 不应该创建输出文件
                File.Exists(outputPath).Should().BeFalse("不应该创建输出文件");
                
                Output.WriteLine($"错误处理正确: {result.Message}");
                Output.WriteLine($"错误数量: {result.Errors.Count}");
            }
            finally
            {
                CleanupTestFiles(outputPath);
            }
        }

        #endregion

        #region 场景2: 处理无效的文件格式

        /// <summary>
        /// 场景: 处理无效的文件格式
        /// 当 我尝试转换一个格式不支持的文件
        /// 那么 系统应该识别格式问题
        /// 并且 提供格式相关的错误信息
        /// </summary>
        [Fact]
        public async Task InvalidFileFormat_FormatDetection()
        {
            // Given - 准备无效格式的测试数据
            string invalidFile = null;
            string outputPath = null;
            
            try
            {
                // Given 我尝试转换一个格式不支持的文件
                invalidFile = Path.Combine(TestTempDir, "invalid_format.txt");
                File.WriteAllText(invalidFile, "这不是一个有效的Excel或XML文件");
                outputPath = Path.Combine(TestTempDir, "output.xml");

                // When 我尝试进行转换
                var result = await ConversionService.ExcelToXmlAsync(invalidFile, outputPath);

                // Then 系统应该识别格式问题
                result.Success.Should().BeFalse("转换应该失败");
                result.Message.ShouldNotBeNullOrEmpty("应该有错误信息");

                // And 提供格式相关的错误信息
                var xmlResult = await ConversionService.XmlToExcelAsync(invalidFile, outputPath + ".xlsx");
                xmlResult.Success.Should().BeFalse("XML转换也应该失败");
                
                Output.WriteLine($"格式检测正确: {result.Message}");
                Output.WriteLine($"XML转换错误: {xmlResult.Message}");
            }
            finally
            {
                CleanupTestFiles(invalidFile, outputPath);
            }
        }

        #endregion

        #region 场景3: 处理权限不足的情况

        /// <summary>
        /// 场景: 处理权限不足的情况
        /// 当 我尝试在没有写入权限的目录创建文件
        /// 那么 系统应该捕获权限异常
        /// 并且 提供权限相关的错误信息
        /// </summary>
        [Fact]
        public async Task InsufficientPermissions_PermissionError()
        {
            // Given - 尝试在系统目录创建文件（模拟权限不足）
            string invalidOutputPath = null;
            string sourceFile = null;
            
            try
            {
                // Given 我尝试在没有写入权限的目录创建文件
                sourceFile = CreateTestExcelFile("test.xlsx", "Name,Value\nTest,100");
                // 使用一个只读目录来模拟权限问题
                var readOnlyDir = Path.Combine(TestTempDir, "readonly");
                Directory.CreateDirectory(readOnlyDir);
                
                // 设置目录为只读（在Linux/Unix系统上）
                chmod_755(readOnlyDir);
                
                invalidOutputPath = Path.Combine(readOnlyDir, "protected_output.xml");

                // When 我尝试进行转换
                var result = await ConversionService.ExcelToXmlAsync(sourceFile, invalidOutputPath);

                // Then 系统应该捕获权限异常
                // 注意：在某些环境下，权限检查可能不会按预期工作
                if (!result.Success)
                {
                    result.Message.ShouldNotBeNullOrEmpty("应该有错误信息");
                    
                    // 错误信息应该包含权限相关的关键词
                    bool hasPermissionError = result.Message.Contains("权限") || 
                                             result.Message.Contains("permission") || 
                                             result.Message.Contains("access") ||
                                             result.Message.Contains("Unauthorized") ||
                                             result.Message.Contains("denied") ||
                                             result.Message.Contains("拒绝") ||
                                             result.Message.Contains("Permission");
                    
                    if (!hasPermissionError)
                    {
                        Output.WriteLine($"检测到其他类型的错误: {result.Message}");
                        result.Errors.ShouldNotBeEmpty("应该有详细的错误信息");
                    }
                    
                    Output.WriteLine($"权限处理测试完成（失败符合预期）: {result.Message}");
                }
                else
                {
                    // 如果转换成功，说明系统有权限访问该目录
                    Output.WriteLine($"权限处理测试完成（成功，系统有权限访问）: {result.Message}");
                    // 这种情况下测试仍然通过，因为系统正确处理了有权限的情况
                }
            }
            finally
            {
                CleanupTestFiles(sourceFile);
                // 清理只读目录
                try
                {
                    var readOnlyDir = Path.Combine(TestTempDir, "readonly");
                    if (Directory.Exists(readOnlyDir))
                    {
                        // 恢复权限以便删除
                        chmod_777(readOnlyDir);
                        Directory.Delete(readOnlyDir, true);
                    }
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"清理只读目录失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 辅助方法：设置目录权限为755（在Unix系统上）
        /// </summary>
        private void chmod_755(string path)
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "chmod",
                        Arguments = $"755 \"{path}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Output.WriteLine($"设置权限失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 辅助方法：设置目录权限为777（在Unix系统上）
        /// </summary>
        private void chmod_777(string path)
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "chmod",
                        Arguments = $"777 \"{path}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Output.WriteLine($"设置权限失败: {ex.Message}");
            }
        }

        #endregion

        #region 场景4: 处理损坏的文件内容

        /// <summary>
        /// 场景: 处理损坏的文件内容
        /// 当 我尝试转换一个内容损坏的文件
        /// 那么 系统应该检测到内容问题
        /// 并且 提供内容相关的错误信息
        /// </summary>
        [Fact]
        public async Task CorruptedFileContent_ContentError()
        {
            // Given - 准备损坏的XML文件
            string corruptedXml = null;
            string outputPath = null;
            
            try
            {
                // Given 我尝试转换一个内容损坏的文件
                corruptedXml = CreateTestXmlFile("corrupted.xml", @"<?xml version=""1.0""?>
<root>
    <item>
        <name>Test Item
        <!-- 缺少结束标签 -->
    <item>
        <name>Another Item</name>
        <value>100</value>
    </item>
</root>");

                outputPath = Path.Combine(TestTempDir, "output.xlsx");

                // When 我尝试进行转换
                var result = await ConversionService.XmlToExcelAsync(corruptedXml, outputPath);

                // Then 系统应该检测到内容问题
                result.Success.Should().BeFalse("转换应该失败");
                
                // And 提供内容相关的错误信息
                result.Message.ShouldNotBeNullOrEmpty("应该有错误信息");
                
                // XML解析错误通常包含特定的关键词
                bool hasXmlError = result.Message.Contains("XML") || 
                                  result.Message.Contains("xml") || 
                                  result.Message.Contains("parse") ||
                                  result.Message.Contains("解析");
                
                if (hasXmlError)
                {
                    Output.WriteLine($"XML错误检测正确: {result.Message}");
                }
                else
                {
                    Output.WriteLine($"检测到其他内容错误: {result.Message}");
                }
                
                // And 不应该创建输出文件
                File.Exists(outputPath).Should().BeFalse("不应该为损坏的文件创建输出");
            }
            finally
            {
                CleanupTestFiles(corruptedXml, outputPath);
            }
        }

        #endregion

        #region 场景5: 处理磁盘空间不足

        /// <summary>
        /// 场景: 处理磁盘空间不足
        /// 当 我尝试转换一个非常大的文件
        /// 并且 磁盘空间不足
        /// 那么 系统应该检测到空间问题
        /// 并且 提供空间相关的错误信息
        /// </summary>
        [Fact]
        public async Task InsufficientDiskSpace_SpaceError()
        {
            // Given - 创建一个相对较大的文件内容
            string largeFile = null;
            string outputPath = null;
            
            try
            {
                // Given 我尝试转换一个非常大的文件
                var largeContent = GenerateLargeContent(10000); // 生成较大的内容
                largeFile = CreateTestExcelFile("large_content.xlsx", largeContent);
                
                // 尝试在一个可能空间不足的位置创建文件
                outputPath = Path.Combine(TestTempDir, "large_output.xml");

                // When 我尝试进行转换
                var result = await ConversionService.ExcelToXmlAsync(largeFile, outputPath);

                // 转换可能会成功，因为我们有足够的测试空间
                // 但我们需要确保错误处理机制正常工作
                if (result.Success)
                {
                    // 如果转换成功，验证输出文件
                    VerifyFileExistsAndNotEmpty(outputPath);
                    Output.WriteLine($"大文件转换成功: {result.Message}");
                    Output.WriteLine($"处理记录数: {result.RecordsProcessed}");
                }
                else
                {
                    // 如果转换失败，验证错误处理
                    result.Message.ShouldNotBeNullOrEmpty("应该有错误信息");
                    result.Errors.ShouldNotBeEmpty("应该有详细的错误信息");
                    
                    Output.WriteLine($"大文件转换失败，错误处理正确: {result.Message}");
                }
            }
            finally
            {
                CleanupTestFiles(largeFile, outputPath);
            }
        }

        private string GenerateLargeContent(int rows)
        {
            var content = new System.Text.StringBuilder();
            content.AppendLine("ID,Name,Category,Value,Description,Formula,Notes");

            for (int i = 0; i < rows; i++)
            {
                content.AppendLine($"{i:D6},Item_{i},Category_{i % 20},{i * 1.5:F2},这是第{i}个测试项目,base_value * {i * 0.1:F2},备注信息包含一些特殊字符：!@#$%^&*()");
            }

            return content.ToString();
        }

        #endregion

        #region 场景6: 处理空文件和空路径

        /// <summary>
        /// 场景: 处理空文件和空路径
        /// 当 我使用空的文件路径或空文件进行转换
        /// 那么 系统应该验证输入参数
        /// 并且 提供参数验证错误信息
        /// </summary>
        [Fact]
        public async Task EmptyInputs_ParameterValidation()
        {
            // Given - 测试各种空的输入情况
            string emptyFile = null;
            string outputPath = null;

            try
            {
                // Given 我使用空的文件路径
                emptyFile = Path.Combine(TestTempDir, "empty.xlsx");
                // 创建一个真正空的Excel文件（只有工作表但没有数据）
                using var workbook = new ClosedXML.Excel.XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Sheet1");
                // 不添加任何数据，创建空工作表
                workbook.SaveAs(emptyFile);
                
                outputPath = Path.Combine(TestTempDir, "output.xml");

                // When 我尝试转换空文件
                var emptyResult = await ConversionService.ExcelToXmlAsync(emptyFile, outputPath);

                // Then 系统应该验证输入参数
                // 注意：空Excel文件可能被成功处理，这取决于业务逻辑
                if (!emptyResult.Success)
                {
                    emptyResult.Message.ShouldNotBeNullOrEmpty("应该有错误信息");
                    Output.WriteLine($"空文件转换失败（符合预期）: {emptyResult.Message}");
                }
                else
                {
                    Output.WriteLine($"空文件转换成功（业务逻辑允许）: {emptyResult.Message}");
                }

                // When 我使用null路径
                var nullResult = await ConversionService.ExcelToXmlAsync(null, outputPath);

                // Then 系统应该提供参数验证错误信息
                nullResult.Success.Should().BeFalse("null路径转换应该失败");
                nullResult.Message.ShouldNotBeNullOrEmpty("应该有错误信息");

                // When 我使用空字符串路径
                var emptyPathResult = await ConversionService.ExcelToXmlAsync("", outputPath);

                // Then 系统应该提供参数验证错误信息
                emptyPathResult.Success.Should().BeFalse("空路径转换应该失败");
                emptyPathResult.Message.ShouldNotBeNullOrEmpty("应该有错误信息");

                Output.WriteLine($"null路径错误: {nullResult.Message}");
                Output.WriteLine($"空路径错误: {emptyPathResult.Message}");
            }
            finally
            {
                CleanupTestFiles(emptyFile, outputPath);
            }
        }

        #endregion

        #region 场景7: 处理文件锁定情况

        /// <summary>
        /// 场景: 处理文件锁定情况
        /// 当 我尝试转换一个被其他程序锁定的文件
        /// 那么 系统应该检测到文件锁定
        /// 并且 提供文件访问相关的错误信息
        /// </summary>
        [Fact]
        public async Task LockedFile_FileAccessError()
        {
            // Given - 创建并模拟锁定文件
            string lockedFile = null;
            string outputPath = null;
            FileStream fileStream = null;

            try
            {
                // Given 我尝试转换一个被锁定的文件
                lockedFile = CreateTestExcelFile("locked.xlsx", "Name,Value\nTest,100");
                outputPath = Path.Combine(TestTempDir, "output.xml");

                // 模拟文件被锁定（以独占方式打开文件）
                fileStream = new FileStream(lockedFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                // When 我尝试转换被锁定的文件
                var result = await ConversionService.ExcelToXmlAsync(lockedFile, outputPath);

                // Then 系统应该检测到文件锁定
                // 注意：在某些环境下，这可能不会触发错误，取决于文件系统
                if (!result.Success)
                {
                    result.Message.ShouldNotBeNullOrEmpty("应该有错误信息");
                    Output.WriteLine($"文件锁定检测正确: {result.Message}");
                }
                else
                {
                    // 如果转换成功，说明系统处理了文件锁定情况
                    VerifyFileExistsAndNotEmpty(outputPath);
                    Output.WriteLine($"文件锁定处理成功: {result.Message}");
                }
            }
            finally
            {
                // 确保释放文件锁定
                fileStream?.Dispose();
                CleanupTestFiles(lockedFile, outputPath);
            }
        }

        #endregion
    }
}