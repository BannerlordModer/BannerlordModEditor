using System;
using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Cli.Services;
using BannerlordModEditor.Common.Services;
using Xunit;
using ClosedXML.Excel;

namespace BannerlordModEditor.Cli.Tests
{
    /// <summary>
    /// 增强的Excel-XML转换功能测试
    /// </summary>
    public class EnhancedExcelXmlConverterTests
    {
        private readonly IFileDiscoveryService _fileDiscoveryService;
        private readonly EnhancedExcelXmlConverterService _converterService;
        private readonly string _testDataPath;

        public EnhancedExcelXmlConverterTests()
        {
            _fileDiscoveryService = new FileDiscoveryService();
            _converterService = new EnhancedExcelXmlConverterService(_fileDiscoveryService);
            _testDataPath = Path.Combine("TestData");
        }

        [Fact]
        public async Task RecognizeXmlFormat_ShouldRecognizeActionTypes()
        {
            // Arrange
            var xmlFile = Path.Combine(_testDataPath, "action_types.xml");
            if (!File.Exists(xmlFile))
            {
                // 创建测试XML文件
                await CreateTestActionTypesXml(xmlFile);
            }

            // Act
            var result = await _converterService.RecognizeXmlFormatAsync(xmlFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ActionTypesDO", result);
        }

        [Fact]
        public async Task ConvertXmlToExcel_ShouldConvertActionTypes()
        {
            // Arrange
            var xmlFile = Path.Combine(_testDataPath, "action_types.xml");
            var excelFile = Path.Combine(Path.GetTempPath(), "test_action_types.xlsx");
            
            if (!File.Exists(xmlFile))
            {
                await CreateTestActionTypesXml(xmlFile);
            }

            // Act
            var result = await _converterService.ConvertXmlToExcelAsync(xmlFile, excelFile);

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(excelFile));

            // 验证Excel文件内容
            using var workbook = new XLWorkbook(excelFile);
            var worksheet = workbook.Worksheets.First();
            Assert.True(worksheet.RowsUsed().Count() > 1); // 至少有表头和一行数据

            // 清理
            File.Delete(excelFile);
        }

        [Fact]
        public async Task ConvertExcelToXml_ShouldConvertActionTypes()
        {
            // Arrange
            var excelFile = Path.Combine(Path.GetTempPath(), "test_action_types.xlsx");
            var xmlFile = Path.Combine(Path.GetTempPath(), "test_action_types_output.xml");
            
            // 创建测试Excel文件
            await CreateTestActionTypesExcel(excelFile);

            // Act
            var result = await _converterService.ConvertExcelToXmlAsync(excelFile, xmlFile, "action_types");

            // Assert
            Assert.True(result);
            Assert.True(File.Exists(xmlFile));

            // 验证XML文件内容
            var xmlContent = await File.ReadAllTextAsync(xmlFile);
            Assert.Contains("action_types", xmlContent);
            Assert.Contains("action", xmlContent);

            // 清理
            File.Delete(excelFile);
            File.Delete(xmlFile);
        }

        [Fact]
        public async Task ValidateExcelFormat_ShouldValidateActionTypes()
        {
            // Arrange
            var excelFile = Path.Combine(Path.GetTempPath(), "test_action_types.xlsx");
            await CreateTestActionTypesExcel(excelFile);

            // Act
            var result = await _converterService.ValidateExcelFormatAsync(excelFile, "action_types");

            // Assert
            Assert.True(result);

            // 清理
            File.Delete(excelFile);
        }

        private async Task CreateTestActionTypesXml(string filePath)
        {
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<action_types>
    <action name=""act_test1"" type=""test"" usage_direction=""up"" action_stage=""start"" />
    <action name=""act_test2"" type=""sample"" usage_direction=""down"" action_stage=""end"" />
</action_types>";
            
            await File.WriteAllTextAsync(filePath, xmlContent);
        }

        private async Task CreateTestActionTypesExcel(string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ActionTypes");

            // 写入表头
            worksheet.Cell(1, 1).Value = "name";
            worksheet.Cell(1, 2).Value = "type";
            worksheet.Cell(1, 3).Value = "usage_direction";
            worksheet.Cell(1, 4).Value = "action_stage";

            // 写入测试数据
            worksheet.Cell(2, 1).Value = "act_test1";
            worksheet.Cell(2, 2).Value = "test";
            worksheet.Cell(2, 3).Value = "up";
            worksheet.Cell(2, 4).Value = "start";

            worksheet.Cell(3, 1).Value = "act_test2";
            worksheet.Cell(3, 2).Value = "sample";
            worksheet.Cell(3, 3).Value = "down";
            worksheet.Cell(3, 4).Value = "end";

            workbook.SaveAs(filePath);
        }

        [Fact]
        public async Task RoundTripConversion_ShouldPreserveData()
        {
            // Arrange
            var originalXml = Path.Combine(Path.GetTempPath(), "original_action_types.xml");
            var intermediateExcel = Path.Combine(Path.GetTempPath(), "intermediate_action_types.xlsx");
            var finalXml = Path.Combine(Path.GetTempPath(), "final_action_types.xml");

            // 创建原始XML文件
            await CreateTestActionTypesXml(originalXml);

            // Act
            // XML -> Excel
            var xmlToExcelResult = await _converterService.ConvertXmlToExcelAsync(originalXml, intermediateExcel);
            Assert.True(xmlToExcelResult);

            // Excel -> XML
            var excelToXmlResult = await _converterService.ConvertExcelToXmlAsync(intermediateExcel, finalXml, "action_types");
            Assert.True(excelToXmlResult);

            // Assert
            var originalContent = await File.ReadAllTextAsync(originalXml);
            var finalContent = await File.ReadAllTextAsync(finalXml);

            // 验证核心数据是否保留（忽略格式差异）
            Assert.Contains("act_test1", finalContent);
            Assert.Contains("act_test2", finalContent);
            Assert.Contains("test", finalContent);
            Assert.Contains("sample", finalContent);

            // 清理
            File.Delete(originalXml);
            File.Delete(intermediateExcel);
            File.Delete(finalXml);
        }
    }
}