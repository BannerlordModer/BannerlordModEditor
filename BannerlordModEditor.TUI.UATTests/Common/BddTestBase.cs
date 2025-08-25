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
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace BannerlordModEditor.TUI.UATTests.Common
{
    /// <summary>
    /// BDD测试基类，提供通用的测试设置和工具方法
    /// </summary>
    public abstract class BddTestBase : IDisposable
    {
        protected readonly ITestOutputHelper Output;
        protected readonly MockFileSystem FileSystem;
        protected readonly IFormatConversionService ConversionService;
        protected readonly string TestTempDir;
        protected readonly string TestDataDir;

        protected BddTestBase(ITestOutputHelper output)
        {
            Output = output;
            FileSystem = new MockFileSystem();
            
            // 设置测试目录
            TestTempDir = Path.Combine(Path.GetTempPath(), $"BddTest_{Guid.NewGuid():N}");
            TestDataDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "BannerlordModEditor.TUI.UATTests", "TestData");
            
            // 创建实际的转换服务（使用真实文件系统进行UAT）
            var fileDiscoveryService = new BannerlordModEditor.Common.Services.FileDiscoveryService();
            var xmlTypeDetectionService = new BannerlordModEditor.TUI.Models.XmlTypeDetectionService(fileDiscoveryService);
            ConversionService = new FormatConversionService(fileDiscoveryService, xmlTypeDetectionService);
            
            // 确保测试目录存在
            Directory.CreateDirectory(TestTempDir);
            
            Output.WriteLine($"测试临时目录: {TestTempDir}");
            Output.WriteLine($"测试数据目录: {TestDataDir}");
        }

        /// <summary>
        /// 创建测试用的Excel文件
        /// </summary>
        protected string CreateTestExcelFile(string fileName, string content = "")
        {
            var filePath = Path.Combine(TestTempDir, fileName);
            
            // 创建真正的Excel文件
            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Data");
            
            if (string.IsNullOrEmpty(content))
            {
                // 默认测试数据
                var data = new List<object[]>
                {
                    new object[] { "Name", "Value", "Description" },
                    new object[] { "Test1", 100, "测试数据1" },
                    new object[] { "Test2", 200, "测试数据2" },
                    new object[] { "Test3", 300, "测试数据3" }
                };
                
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        var value = data[i][j];
                        if (value is string strValue)
                        {
                            worksheet.Cell(i + 1, j + 1).Value = strValue;
                        }
                        else if (value is int intValue)
                        {
                            worksheet.Cell(i + 1, j + 1).Value = intValue;
                        }
                        else if (value is decimal decimalValue)
                        {
                            worksheet.Cell(i + 1, j + 1).Value = decimalValue;
                        }
                        else
                        {
                            worksheet.Cell(i + 1, j + 1).Value = value?.ToString() ?? "";
                        }
                    }
                }
            }
            else
            {
                // 解析CSV内容并创建Excel文件
                var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',');
                    for (int j = 0; j < columns.Length; j++)
                    {
                        var cellValue = columns[j].Trim();
                        // 尝试解析为数字
                        if (decimal.TryParse(cellValue, out var numericValue))
                        {
                            worksheet.Cell(i + 1, j + 1).Value = (ClosedXML.Excel.XLCellValue)numericValue;
                        }
                        else
                        {
                            worksheet.Cell(i + 1, j + 1).Value = (ClosedXML.Excel.XLCellValue)cellValue;
                        }
                    }
                }
            }
            
            workbook.SaveAs(filePath);
            Output.WriteLine($"创建测试Excel文件: {filePath}");
            return filePath;
        }

        /// <summary>
        /// 创建测试用的XML文件
        /// </summary>
        protected string CreateTestXmlFile(string fileName, string content = "")
        {
            var filePath = Path.Combine(TestTempDir, fileName);
            
            // 创建简单的XML内容
            if (string.IsNullOrEmpty(content))
            {
                content = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item>
        <name>Test1</name>
        <value>100</value>
        <description>测试数据1</description>
    </item>
    <item>
        <name>Test2</name>
        <value>200</value>
        <description>测试数据2</description>
    </item>
</root>";
            }
            
            File.WriteAllText(filePath, content);
            Output.WriteLine($"创建测试XML文件: {filePath}");
            return filePath;
        }

        /// <summary>
        /// 获取测试数据文件路径
        /// </summary>
        protected string GetTestDataPath(string relativePath)
        {
            var fullPath = Path.Combine(TestDataDir, relativePath);
            if (!File.Exists(fullPath))
            {
                Output.WriteLine($"警告: 测试数据文件不存在: {fullPath}");
            }
            return fullPath;
        }

        /// <summary>
        /// 验证文件是否存在且内容不为空
        /// </summary>
        protected void VerifyFileExistsAndNotEmpty(string filePath)
        {
            File.Exists(filePath).Should().BeTrue($"文件应该存在: {filePath}");
            new FileInfo(filePath).Length.Should().BeGreaterThan(0, $"文件内容不应为空: {filePath}");
        }

        /// <summary>
        /// 验证文件格式
        /// </summary>
        protected void VerifyFileFormat(string filePath, string expectedExtension)
        {
            Path.GetExtension(filePath).ShouldBe(expectedExtension, $"文件扩展名应该为 {expectedExtension}");
        }

        /// <summary>
        /// 清理测试文件
        /// </summary>
        protected void CleanupTestFiles(params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Output.WriteLine($"删除测试文件: {filePath}");
                    }
                }
                catch (Exception ex)
                {
                    Output.WriteLine($"删除文件失败 {filePath}: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(TestTempDir))
                {
                    Directory.Delete(TestTempDir, true);
                    Output.WriteLine($"清理测试目录: {TestTempDir}");
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine($"清理测试目录失败 {TestTempDir}: {ex.Message}");
            }
            
            GC.SuppressFinalize(this);
        }
    }
}