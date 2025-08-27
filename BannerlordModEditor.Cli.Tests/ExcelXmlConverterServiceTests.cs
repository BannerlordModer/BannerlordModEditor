using BannerlordModEditor.Cli.Services;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Services;
using ClosedXML.Excel;
using System.Xml.Serialization;
using System.Xml;

namespace BannerlordModEditor.Cli.Tests
{
    /// <summary>
    /// ExcelXmlConverterService 的单元测试
    /// </summary>
    public class ExcelXmlConverterServiceTests
    {
        private readonly IExcelXmlConverterService _converterService;
        private readonly string _testDataDir;
        private readonly string _tempDir;

        public ExcelXmlConverterServiceTests()
        {
            var fileDiscoveryService = new FileDiscoveryService();
            _converterService = new ExcelXmlConverterService(fileDiscoveryService);
            
            _testDataDir = Path.Combine("TestData");
            _tempDir = Path.GetTempPath();
        }

        [Fact]
        public async Task RecognizeXmlFormatAsync_WithValidActionTypesXml_ReturnsCorrectModelType()
        {
            // Arrange
            var xmlFilePath = Path.Combine(_testDataDir, "action_types.xml");
            
            // Act
            var result = await _converterService.RecognizeXmlFormatAsync(xmlFilePath);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("action_types", result);
        }

        [Fact]
        public async Task RecognizeXmlFormatAsync_WithValidCombatParametersXml_ReturnsCorrectModelType()
        {
            // Arrange
            var xmlFilePath = Path.Combine(_testDataDir, "combat_parameters.xml");
            
            // Act
            var result = await _converterService.RecognizeXmlFormatAsync(xmlFilePath);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("base", result);
        }

        [Fact]
        public async Task RecognizeXmlFormatAsync_WithInvalidXml_ThrowsException()
        {
            // Arrange
            var invalidXmlPath = Path.Combine(_tempDir, "invalid.xml");
            await File.WriteAllTextAsync(invalidXmlPath, "invalid xml content");
            
            // Act & Assert
            await Assert.ThrowsAsync<XmlFormatException>(() => 
                _converterService.RecognizeXmlFormatAsync(invalidXmlPath));
        }

        [Fact]
        public async Task RecognizeXmlFormatAsync_WithNonexistentFile_ThrowsException()
        {
            // Arrange
            var nonexistentPath = Path.Combine(_tempDir, "nonexistent.xml");
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<XmlFormatException>(() => 
                _converterService.RecognizeXmlFormatAsync(nonexistentPath));
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        [Fact]
        public async Task ConvertXmlToExcelAsync_WithActionTypesXml_CreatesValidExcel()
        {
            // Arrange
            var xmlFilePath = Path.Combine(_testDataDir, "action_types.xml");
            var excelFilePath = Path.Combine(_tempDir, $"test_actions_{Guid.NewGuid()}.xlsx");
            
            try
            {
                // Act
                var result = await _converterService.ConvertXmlToExcelAsync(xmlFilePath, excelFilePath);
                
                // Assert
                Assert.True(result);
                Assert.True(File.Exists(excelFilePath));
                
                // 验证 Excel 文件内容
                using var workbook = new XLWorkbook(excelFilePath);
                var worksheet = workbook.Worksheets.First();
                
                Assert.NotNull(worksheet);
                Assert.True(worksheet.RowsUsed().Count() > 1); // 至少有表头和数据行
            }
            finally
            {
                // 清理
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        [Fact]
        public async Task ConvertXmlToExcelAsync_WithInvalidXml_ThrowsException()
        {
            // Arrange
            var invalidXmlPath = Path.Combine(_tempDir, "invalid.xml");
            var excelFilePath = Path.Combine(_tempDir, "test_output.xlsx");
            await File.WriteAllTextAsync(invalidXmlPath, "invalid xml content");
            
            try
            {
                // Act & Assert
                await Assert.ThrowsAsync<ConversionException>(() => 
                    _converterService.ConvertXmlToExcelAsync(invalidXmlPath, excelFilePath));
            }
            finally
            {
                // 清理
                if (File.Exists(invalidXmlPath))
                {
                    File.Delete(invalidXmlPath);
                }
            }
        }

        [Fact]
        public async Task ConvertXmlToExcelAsync_WithNonexistentXml_ThrowsException()
        {
            // Arrange
            var nonexistentXmlPath = Path.Combine(_tempDir, "nonexistent.xml");
            var excelFilePath = Path.Combine(_tempDir, "test_output.xlsx");
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConversionException>(() => 
                _converterService.ConvertXmlToExcelAsync(nonexistentXmlPath, excelFilePath));
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        [Fact]
        public async Task ConvertExcelToXmlAsync_WithValidExcelAndModelType_CreatesValidXml()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            var xmlFilePath = Path.Combine(_tempDir, $"test_output_{Guid.NewGuid()}.xml");
            
            try
            {
                // Act
                var result = await _converterService.ConvertExcelToXmlAsync(excelFilePath, xmlFilePath, "action_types");
                
                // Assert
                Assert.True(result);
                Assert.True(File.Exists(xmlFilePath));
                
                // 验证 XML 文件内容
                var xmlContent = await File.ReadAllTextAsync(xmlFilePath);
                Assert.Contains("action_types", xmlContent);
                
                // 尝试反序列化验证格式
                var serializer = new XmlSerializer(typeof(ActionTypesDO));
                using var reader = new StringReader(xmlContent);
                var actionTypes = serializer.Deserialize(reader) as ActionTypesDO;
                
                Assert.NotNull(actionTypes);
                Assert.NotEmpty(actionTypes.Actions);
            }
            finally
            {
                // 清理
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
        public async Task ConvertExcelToXmlAsync_WithInvalidModelType_ThrowsException()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            var xmlFilePath = Path.Combine(_tempDir, "test_output.xml");
            
            try
            {
                // Act & Assert
                var exception = await Assert.ThrowsAsync<ConversionException>(() => 
                    _converterService.ConvertExcelToXmlAsync(excelFilePath, xmlFilePath, "InvalidModelType"));
                Assert.IsType<ArgumentException>(exception.InnerException);
            }
            finally
            {
                // 清理
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        [Fact]
        public async Task ConvertExcelToXmlAsync_WithNonexistentExcel_ThrowsException()
        {
            // Arrange
            var nonexistentExcelPath = Path.Combine(_tempDir, "nonexistent.xlsx");
            var xmlFilePath = Path.Combine(_tempDir, "test_output.xml");
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConversionException>(() => 
                _converterService.ConvertExcelToXmlAsync(nonexistentExcelPath, xmlFilePath, "action_types"));
            Assert.IsType<FileNotFoundException>(exception.InnerException);
        }

        [Fact]
        public async Task ValidateExcelFormatAsync_WithValidExcelAndModelType_ReturnsTrue()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            
            try
            {
                // Act
                var result = await _converterService.ValidateExcelFormatAsync(excelFilePath, "action_types");
                
                // Assert
                Assert.True(result);
            }
            finally
            {
                // 清理
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        [Fact]
        public async Task ValidateExcelFormatAsync_WithInvalidModelType_ThrowsException()
        {
            // Arrange
            var excelFilePath = CreateTestExcelFile();
            
            try
            {
                // Act & Assert
                var exception = await Assert.ThrowsAsync<ExcelOperationException>(() => 
                    _converterService.ValidateExcelFormatAsync(excelFilePath, "InvalidModelType"));
                Assert.IsType<ArgumentException>(exception.InnerException);
            }
            finally
            {
                // 清理
                if (File.Exists(excelFilePath))
                {
                    File.Delete(excelFilePath);
                }
            }
        }

        /// <summary>
        /// 创建测试用的 Excel 文件
        /// </summary>
        private string CreateTestExcelFile()
        {
            var filePath = Path.Combine(_tempDir, $"test_excel_{Guid.NewGuid()}.xlsx");
            
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Actions");
            
            // 添加表头
            worksheet.Cell(1, 1).Value = "name";
            worksheet.Cell(1, 2).Value = "type";
            worksheet.Cell(1, 3).Value = "usage_direction";
            worksheet.Cell(1, 4).Value = "action_stage";
            
            // 添加测试数据
            worksheet.Cell(2, 1).Value = "test_action_1";
            worksheet.Cell(2, 2).Value = "test_type";
            worksheet.Cell(2, 3).Value = "test_direction";
            worksheet.Cell(2, 4).Value = "test_stage";
            
            worksheet.Cell(3, 1).Value = "test_action_2";
            worksheet.Cell(3, 2).Value = "test_type_2";
            
            workbook.SaveAs(filePath);
            return filePath;
        }
    }
}