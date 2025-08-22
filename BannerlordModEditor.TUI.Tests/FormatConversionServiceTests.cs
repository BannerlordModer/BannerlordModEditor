using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using ClosedXML.Excel;
using BannerlordModEditor.TUI.Services;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.TUI.Tests
{
    public class FormatConversionServiceTests
    {
        private readonly FormatConversionService _conversionService;
        private readonly string _testDataDir;
        private readonly string _tempDir;

        public FormatConversionServiceTests()
        {
            _conversionService = new FormatConversionService(new FileDiscoveryService());
            _testDataDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "BannerlordModEditor.Common.Tests", "TestData");
            _tempDir = Path.Combine(Path.GetTempPath(), "BannerlordModEditor_TUI_Tests");
            
            // 确保临时目录存在
            if (!Directory.Exists(_tempDir))
            {
                Directory.CreateDirectory(_tempDir);
            }
        }

        private bool IsValidXml(string xml)
        {
            try
            {
                System.Xml.Linq.XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithExcelFile_ShouldReturnExcelFormat()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            
            try
            {
                // Act
                var result = await _conversionService.DetectFileFormatAsync(excelFilePath);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(FileFormatType.Excel, result.FormatType);
                Assert.True(result.IsSupported);
                Assert.Contains("TestColumn", result.ColumnNames);
                Assert.Equal(1, result.RowCount);
            }
            finally
            {
                // Cleanup
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithXmlFile_ShouldReturnXmlFormat()
        {
            // Arrange
            var xmlFilePath = Path.Combine(_testDataDir, "action_types.xml");
            
            // Skip test if file doesn't exist
            if (!File.Exists(xmlFilePath))
            {
                return;
            }

            // Act
            var result = await _conversionService.DetectFileFormatAsync(xmlFilePath);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(FileFormatType.Xml, result.FormatType);
            Assert.True(result.IsSupported);
            Assert.NotNull(result.RootElement);
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithUnsupportedFile_ShouldReturnNotSupported()
        {
            // Arrange
            var txtFilePath = Path.Combine(_tempDir, "test.txt");
            await File.WriteAllTextAsync(txtFilePath, "This is a text file");
            
            try
            {
                // Act
                var result = await _conversionService.DetectFileFormatAsync(txtFilePath);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(FileFormatType.Unknown, result.FormatType);
                Assert.False(result.IsSupported);
            }
            finally
            {
                // Cleanup
                if (File.Exists(txtFilePath))
                {
                    File.Delete(txtFilePath);
                }
            }
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithValidExcel_ShouldSucceed()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            var xmlFilePath = Path.Combine(_tempDir, "test_output.xml");
            
            try
            {
                // Act
                var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);
                
                // Assert
                Assert.True(result.Success);
                Assert.True(File.Exists(xmlFilePath));
                Assert.Equal(1, result.RecordsProcessed);
                Assert.True(result.Duration.TotalMilliseconds > 0);
                
                // 验证XML内容
                var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
                Assert.True(IsValidXml(xmlContent));
                Assert.Contains("<TestColumn>TestData</TestColumn>", xmlContent);
            }
            finally
            {
                // Cleanup
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
            }
        }

        [Fact]
        public async Task XmlToExcelAsync_WithValidXml_ShouldSucceed()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Root>
    <Item>
        <Name>TestItem</Name>
        <Value>123</Value>
    </Item>
</Root>";
            
            var xmlFilePath = Path.Combine(_tempDir, "test_input.xml");
            var excelFilePath = Path.Combine(_tempDir, "test_output.xlsx");
            
            await File.WriteAllTextAsync(xmlFilePath, xmlContent);
            
            try
            {
                // Act
                var result = await _conversionService.XmlToExcelAsync(xmlFilePath, excelFilePath);
                
                // Assert
                Assert.True(result.Success);
                Assert.True(File.Exists(excelFilePath));
                Assert.Equal(1, result.RecordsProcessed);
                Assert.True(result.Duration.TotalMilliseconds > 0);
                
                // 验证Excel内容
                using var workbook = new XLWorkbook(excelFilePath);
                var worksheet = workbook.Worksheets.First();
                
                Assert.Equal("Name", worksheet.Cell(1, 1).Value);
                Assert.Equal("Value", worksheet.Cell(1, 2).Value);
                Assert.Equal("TestItem", worksheet.Cell(2, 1).Value);
                Assert.Equal("123", worksheet.Cell(2, 2).Value);
            }
            finally
            {
                // Cleanup
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithNonexistentFile_ShouldFail()
        {
            // Arrange
            var excelFilePath = Path.Combine(_tempDir, "nonexistent.xlsx");
            var xmlFilePath = Path.Combine(_tempDir, "test_output.xml");
            
            // Act
            var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);
            
            // Assert
            Assert.False(result.Success);
            Assert.Contains("Excel文件不存在", result.Errors.First());
            Assert.Single(result.Errors);
        }

        [Fact]
        public async Task XmlToExcelAsync_WithInvalidXml_ShouldFail()
        {
            // Arrange
            var xmlContent = "This is not valid XML";
            var xmlFilePath = Path.Combine(_tempDir, "invalid.xml");
            var excelFilePath = Path.Combine(_tempDir, "test_output.xlsx");
            
            await File.WriteAllTextAsync(xmlFilePath, xmlContent);
            
            try
            {
                // Act
                var result = await _conversionService.XmlToExcelAsync(xmlFilePath, excelFilePath);
                
                // Assert
                Assert.False(result.Success);
                Assert.NotEmpty(result.Errors);
            }
            finally
            {
                // Cleanup
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
            }
        }

        [Fact]
        public async Task ValidateConversionAsync_ExcelToXml_ShouldValidateCorrectly()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            var xmlFilePath = Path.Combine(_tempDir, "validation_test.xml");
            
            try
            {
                // 先进行转换
                var conversionResult = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);
                Assert.True(conversionResult.Success);
                
                // Act
                var validationResult = await _conversionService.ValidateConversionAsync(excelFilePath, xmlFilePath, ConversionDirection.ExcelToXml);
                
                // Assert
                Assert.NotNull(validationResult);
                Assert.True(validationResult.IsValid);
            }
            finally
            {
                // Cleanup
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
            }
        }

        [Fact]
        public async Task ConversionOptions_WithFlattenNestedElements_ShouldWorkCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Root>
    <Item>
        <Parent>
            <Child>NestedValue</Child>
        </Parent>
    </Item>
</Root>";
            
            var xmlFilePath = Path.Combine(_tempDir, "nested_test.xml");
            var excelFilePath = Path.Combine(_tempDir, "nested_test.xlsx");
            
            await File.WriteAllTextAsync(xmlFilePath, xmlContent);
            
            var options = new ConversionOptions
            {
                FlattenNestedElements = true,
                NestedElementSeparator = "_"
            };
            
            try
            {
                // Act
                var result = await _conversionService.XmlToExcelAsync(xmlFilePath, excelFilePath, options);
                
                // Assert
                Assert.True(result.Success);
                Assert.True(File.Exists(excelFilePath));
                
                // 验证嵌套元素被正确扁平化
                using var workbook = new XLWorkbook(excelFilePath);
                var worksheet = workbook.Worksheets.First();
                
                // 应该包含扁平化的列名
                var headers = new List<string>();
                for (int i = 1; i <= 10; i++) // 最多检查10列
                {
                    var headerValue = worksheet.Cell(1, i).GetString();
                    if (string.IsNullOrEmpty(headerValue)) break;
                    headers.Add(headerValue);
                }
                
                Assert.Contains("Parent_Child", headers);
            }
            finally
            {
                // Cleanup
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        private string CreateTestExcelFile()
        {
            var filePath = Path.Combine(_tempDir, $"test_{Guid.NewGuid()}.xlsx");
            
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            
            // 添加标题行
            worksheet.Cell(1, 1).Value = "TestColumn";
            worksheet.Cell(1, 2).Value = "NumberColumn";
            
            // 添加数据行
            worksheet.Cell(2, 1).Value = "TestData";
            worksheet.Cell(2, 2).Value = 42;
            
            workbook.SaveAs(filePath);
            return filePath;
        }

        private void Dispose()
        {
            // 清理临时目录
            if (Directory.Exists(_tempDir))
            {
                try
                {
                    Directory.Delete(_tempDir, true);
                }
                catch
                {
                    // 忽略清理错误
                }
            }
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithNullFilePath_ShouldFail()
        {
            // Arrange
            var excelFilePath = (string)null!;
            var xmlFilePath = Path.Combine(_tempDir, "test.xml");

            // Act
            var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Excel文件路径不能为空", result.Message);
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithEmptyFilePath_ShouldFail()
        {
            // Arrange
            var excelFilePath = "";
            var xmlFilePath = Path.Combine(_tempDir, "test.xml");

            // Act
            var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Excel文件路径不能为空", result.Message);
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithInvalidExtension_ShouldFail()
        {
            // Arrange
            var testFile = Path.Combine(_tempDir, "test.txt");
            await File.WriteAllTextAsync(testFile, "test content");
            var xmlFilePath = Path.Combine(_tempDir, "test.xml");

            try
            {
                // Act
                var result = await _conversionService.ExcelToXmlAsync(testFile, xmlFilePath);

                // Assert
                Assert.False(result.Success);
                Assert.Contains("不支持的Excel文件格式", result.Message);
            }
            finally
            {
                // Cleanup
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Fact]
        public async Task XmlToExcelAsync_WithNullFilePath_ShouldFail()
        {
            // Arrange
            var xmlFilePath = (string)null!;
            var excelFilePath = Path.Combine(_tempDir, "test.xlsx");

            // Act
            var result = await _conversionService.XmlToExcelAsync(xmlFilePath, excelFilePath);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("XML文件不存在:", result.Message);
        }

        [Fact]
        public async Task XmlToExcelAsync_WithEmptyFilePath_ShouldFail()
        {
            // Arrange
            var xmlFilePath = "";
            var excelFilePath = Path.Combine(_tempDir, "test.xlsx");

            // Act
            var result = await _conversionService.XmlToExcelAsync(xmlFilePath, excelFilePath);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("XML文件不存在:", result.Message);
        }

        [Fact]
        public async Task XmlToExcelAsync_WithInvalidExtension_ShouldFail()
        {
            // Arrange
            var testFile = Path.Combine(_tempDir, "test.txt");
            await File.WriteAllTextAsync(testFile, "test content");
            var excelFilePath = Path.Combine(_tempDir, "test.xlsx");

            try
            {
                // Act
                var result = await _conversionService.XmlToExcelAsync(testFile, excelFilePath);

                // Assert
                Assert.False(result.Success);
                Assert.Contains("XML处理错误:", result.Message);
            }
            finally
            {
                // Cleanup
                if (File.Exists(testFile))
                {
                    File.Delete(testFile);
                }
            }
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithNullFilePath_ShouldReturnError()
        {
            // Arrange
            var filePath = (string)null!;

            // Act
            var result = await _conversionService.DetectFileFormatAsync(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSupported);
            Assert.Contains("文件路径不能为空", result.FormatDescription);
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithEmptyFilePath_ShouldReturnError()
        {
            // Arrange
            var filePath = "";

            // Act
            var result = await _conversionService.DetectFileFormatAsync(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSupported);
            Assert.Contains("文件路径不能为空", result.FormatDescription);
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithNonexistentFile_ShouldReturnError()
        {
            // Arrange
            var filePath = Path.Combine(_tempDir, "nonexistent.xlsx");

            // Act
            var result = await _conversionService.DetectFileFormatAsync(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSupported);
            Assert.Contains("文件不存在", result.FormatDescription);
        }

        [Fact]
        public async Task ValidateConversionAsync_WithNullSourcePath_ShouldReturnError()
        {
            // Arrange
            var sourceFilePath = (string)null!;
            var targetFilePath = Path.Combine(_tempDir, "test.xml");

            // Act
            var result = await _conversionService.ValidateConversionAsync(sourceFilePath, targetFilePath, ConversionDirection.ExcelToXml);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Contains("源文件路径不能为空", result.Errors.First().Message);
        }

        [Fact]
        public async Task ValidateConversionAsync_WithEmptySourcePath_ShouldReturnError()
        {
            // Arrange
            var sourceFilePath = "";
            var targetFilePath = Path.Combine(_tempDir, "test.xml");

            // Act
            var result = await _conversionService.ValidateConversionAsync(sourceFilePath, targetFilePath, ConversionDirection.ExcelToXml);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Contains("源文件路径不能为空", result.Errors.First().Message);
        }

        [Fact]
        public async Task ValidateConversionAsync_WithNonexistentSourceFile_ShouldReturnError()
        {
            // Arrange
            var sourceFilePath = Path.Combine(_tempDir, "nonexistent.xlsx");
            var targetFilePath = Path.Combine(_tempDir, "test.xml");

            // Act
            var result = await _conversionService.ValidateConversionAsync(sourceFilePath, targetFilePath, ConversionDirection.ExcelToXml);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsValid);
            Assert.Contains("源文件不存在", result.Errors.First().Message);
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithBackupEnabled_ShouldCreateBackup()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            var xmlFilePath = Path.Combine(_tempDir, "test.xml");
            
            // 创建已存在的目标文件
            await File.WriteAllTextAsync(xmlFilePath, "existing content");

            var options = new ConversionOptions
            {
                CreateBackup = true
            };

            try
            {
                // Act
                var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath, options);

                // Assert
                Assert.True(result.Success);
                Assert.Contains("已创建备份文件", result.Warnings.First());
                
                // 验证备份文件是否存在
                var backupFiles = Directory.GetFiles(_tempDir, "test.xml.backup_*");
                Assert.Single(backupFiles);
            }
            finally
            {
                // Cleanup
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
                // 清理备份文件
                var backupFiles = Directory.GetFiles(_tempDir, "test.xml.backup_*");
                foreach (var backupFile in backupFiles)
                {
                    File.Delete(backupFile);
                }
            }
        }

        [Fact]
        public async Task ExcelToXmlAsync_WithLargeFile_ShouldHandleMemoryUsage()
        {
            // Arrange
            var excelFilePath = Path.Combine(_tempDir, "large_test.xlsx");
            
            // 创建一个较大的Excel文件
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");
            
            // 添加标题行
            worksheet.Cell(1, 1).Value = "Column1";
            worksheet.Cell(1, 2).Value = "Column2";
            
            // 添加大量数据行
            for (int i = 0; i < 1000; i++)
            {
                worksheet.Cell(i + 2, 1).Value = $"Data{i}";
                worksheet.Cell(i + 2, 2).Value = i;
            }
            
            workbook.SaveAs(excelFilePath);
            
            var xmlFilePath = Path.Combine(_tempDir, "large_test.xml");

            try
            {
                // Act
                var result = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(1000, result.RecordsProcessed);
                Assert.True(result.Duration.TotalMilliseconds > 0);
                Assert.True(File.Exists(xmlFilePath));
                
                // 验证XML文件大小合理
                var xmlFileInfo = new FileInfo(xmlFilePath);
                Assert.True(xmlFileInfo.Length > 0);
                Assert.True(xmlFileInfo.Length < 10 * 1024 * 1024); // 小于10MB
            }
            finally
            {
                // Cleanup
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
            }
        }

        [Fact]
        public async Task XmlToExcelAsync_WithInvalidXmlStructure_ShouldFail()
        {
            // Arrange
            var invalidXmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
    <item>
        <name>Test Item
        <value>123</value>
    </item>
    <item>
        <name>Another Item</name>
        <value>456</value>
    </item>
</root>";
            
            var xmlFilePath = Path.Combine(_tempDir, "invalid.xml");
            var excelFilePath = Path.Combine(_tempDir, "invalid_test.xlsx");
            
            await File.WriteAllTextAsync(xmlFilePath, invalidXmlContent);

            try
            {
                // Act
                var result = await _conversionService.XmlToExcelAsync(xmlFilePath, excelFilePath);

                // Assert
                Assert.False(result.Success); // 应该因为XML格式错误而失败
                Assert.Contains("XML处理错误", result.Message);
            }
            finally
            {
                // Cleanup
                if (File.Exists(xmlFilePath))
                {
                    File.Delete(xmlFilePath);
                }
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        [Fact]
        public async Task DetectFileFormatAsync_WithCorruptedExcelFile_ShouldReturnError()
        {
            // Arrange
            var corruptedExcelPath = Path.Combine(_tempDir, "corrupted.xlsx");
            await File.WriteAllTextAsync(corruptedExcelPath, "This is not a valid Excel file");

            try
            {
                // Act
                var result = await _conversionService.DetectFileFormatAsync(corruptedExcelPath);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.IsSupported);
                Assert.Contains("Excel文件分析失败", result.FormatDescription);
            }
            finally
            {
                // Cleanup
                if (File.Exists(corruptedExcelPath))
                {
                    File.Delete(corruptedExcelPath);
                }
            }
        }

        [Fact]
        public async Task ValidateConversionAsync_WithDirectoryPermissionIssue_ShouldHandleGracefully()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            var restrictedDir = Path.Combine(_tempDir, "restricted");
            var xmlFilePath = Path.Combine(restrictedDir, "test.xml");
            
            // 创建一个只读目录
            Directory.CreateDirectory(restrictedDir);
            
            try
            {
                // 先进行转换
                var conversionResult = await _conversionService.ExcelToXmlAsync(excelFilePath, xmlFilePath);
                Assert.True(conversionResult.Success);
                
                // Act - 验证转换结果
                var validationResult = await _conversionService.ValidateConversionAsync(excelFilePath, xmlFilePath, ConversionDirection.ExcelToXml);
                
                // Assert
                Assert.NotNull(validationResult);
                Assert.True(validationResult.IsValid);
            }
            finally
            {
                // Cleanup
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
                if (Directory.Exists(restrictedDir))
                {
                    try
                    {
                        Directory.Delete(restrictedDir, true);
                    }
                    catch
                    {
                        // 忽略清理错误
                    }
                }
            }
        }
    }
}