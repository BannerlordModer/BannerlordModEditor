using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BannerlordModEditor.Common.Services;
using BannerlordModEditor.Tests.Helpers;

namespace BannerlordModEditor.Common.Tests.Services
{
    /// <summary>
    /// 隐式校验逻辑检测器测试
    /// 测试基于Mount & Blade源码分析的隐式校验规则检测功能
    /// </summary>
    public class ImplicitValidationDetectorTests
    {
        private readonly ImplicitValidationDetector _detector;
        private readonly TestHelper _testHelper;
        private readonly string _testDataPath;

        public ImplicitValidationDetectorTests()
        {
            _testHelper = new TestHelper();
            _testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            
            var fileDiscoveryService = new FileDiscoveryService(_testDataPath);
            var dependencyAnalyzer = new XmlDependencyAnalyzer(fileDiscoveryService);
            
            _detector = new ImplicitValidationDetector(fileDiscoveryService, dependencyAnalyzer);
        }

        [Fact]
        public void ValidateModule_ShouldDetectDuplicateIds()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempDuplicateIds");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""duplicate_item"" name=""Test Item 1""/>
    <Item id=""duplicate_item"" name=""Test Item 2""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalErrors > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "ID_Unique_Required" && r.Message.Contains("duplicate_item")));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldDetectInvalidItemWeight()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempInvalidWeight");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""-5"" value=""100""/>
    <Item id=""test_item2"" weight=""1500"" value=""200""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalWarnings > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "Item_Weight_Valid" && r.Severity == ValidationSeverity.Warning));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldDetectInvalidItemValue()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempInvalidValue");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" value=""-100""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalErrors > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "Item_Value_Valid" && r.Severity == ValidationSeverity.Error));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldDetectInvalidCharacterLevel()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempInvalidLevel");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Characters>
    <Character id=""test_char"" level=""0"" occupation=""Soldier""/>
    <Character id=""test_char2"" level=""100"" occupation=""Soldier""/>
</Characters>";
                
                var testFile = Path.Combine(tempDir, "characters.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalErrors > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "Character_Level_Valid" && r.Severity == ValidationSeverity.Error));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldDetectInvalidCraftingDifficulty()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempInvalidCrafting");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CraftingPieces>
    <CraftingPiece id=""test_piece"" difficulty=""-10""/>
    <CraftingPiece id=""test_piece2"" difficulty=""500""/>
</CraftingPieces>";
                
                var testFile = Path.Combine(tempDir, "crafting_pieces.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalWarnings > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "Crafting_Piece_Difficulty_Valid" && r.Severity == ValidationSeverity.Warning));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldDetectInvalidIdFormat()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempInvalidIdFormat");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""Invalid-ID@Format""/>
    <Item id=""Valid Id""/>
    <Item id=""valid_id""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalWarnings > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "ID_Format_Valid" && r.Severity == ValidationSeverity.Warning));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldDetectReferenceIntegrityIssues()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempReferenceIntegrity");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建一个引用不存在对象的文件
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<TestData>
    <TestItem id=""test_item"" item=""nonexistent.item"" character=""missing.hero""/>
</TestData>";
                
                var testFile = Path.Combine(tempDir, "test_data.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalErrors > 0);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => 
                    r.RuleName == "Reference_Integrity_Valid" && r.Severity == ValidationSeverity.Error));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateXmlFile_ShouldValidateSingleFile()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempSingleFile");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""10"" value=""100""/>
    <Item id=""test_item2"" weight=""-5"" value=""200""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                var context = new ValidationContext();
                
                // Act
                var result = _detector.ValidateXmlFile(testFile, context);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("items.xml", result.FileName);
                Assert.True(result.WarningCount > 0); // 应该检测到负重量
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldHandleEmptyDirectory()
        {
            // Arrange
            var emptyDir = Path.Combine(_testDataPath, "EmptyValidation");
            Directory.CreateDirectory(emptyDir);
            
            try
            {
                // Act
                var result = _detector.ValidateModule(emptyDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(0, result.TotalFiles);
                Assert.Empty(result.FileResults);
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(emptyDir))
                {
                    Directory.Delete(emptyDir);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldHandleInvalidXml()
        {
            // Arrange
            var invalidDir = Path.Combine(_testDataPath, "InvalidValidation");
            Directory.CreateDirectory(invalidDir);
            
            try
            {
                var invalidFile = Path.Combine(invalidDir, "invalid.xml");
                File.WriteAllText(invalidFile, "This is not valid XML content");
                
                // Act
                var result = _detector.ValidateModule(invalidDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.Errors.Count > 0);
                Assert.False(result.IsValid);
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(invalidDir))
                {
                    Directory.Delete(invalidDir, true);
                }
            }
        }

        [Fact]
        public void ValidateModule_ShouldProvideDetailedResults()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempDetailedResults");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""10"" value=""100""/>
    <Item id=""test_item"" weight=""15"" value=""200""/>
    <Item id=""item2"" weight=""-5"" value=""-50""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.TotalFiles);
                Assert.True(result.TotalErrors > 0); // 重复ID和负价值
                Assert.True(result.TotalWarnings > 0); // 负重量
                
                var fileResult = result.FileResults.First();
                Assert.True(fileResult.ErrorCount > 0);
                Assert.True(fileResult.WarningCount > 0);
                Assert.NotNull(fileResult.Results);
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Theory]
        [InlineData("items.xml", "Item_Weight_Valid")]
        [InlineData("characters.xml", "Character_Level_Valid")]
        [InlineData("crafting_pieces.xml", "Crafting_Piece_Difficulty_Valid")]
        public void ValidateSpecificFileTypes_ShouldApplyCorrectRules(string fileName, string expectedRule)
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, $"TempSpecific_{Path.GetFileNameWithoutExtension(fileName)}");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建对应类型的测试文件
                string testContent = fileName switch
                {
                    "items.xml" => @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""-5"" value=""100""/>
</Items>",
                    "characters.xml" => @"<?xml version=""1.0"" encoding=""utf-8""?>
<Characters>
    <Character id=""test_char"" level=""0"" occupation=""Soldier""/>
</Characters>",
                    "crafting_pieces.xml" => @"<?xml version=""1.0"" encoding=""utf-8""?>
<CraftingPieces>
    <CraftingPiece id=""test_piece"" difficulty=""-10""/>
</CraftingPieces>",
                    _ => throw new ArgumentException($"Unsupported file type: {fileName}")
                };
                
                var testFile = Path.Combine(tempDir, fileName);
                File.WriteAllText(testFile, testContent);
                
                // Act
                var result = _detector.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.Contains(result.FileResults, f => f.Results.Any(r => r.RuleName == expectedRule));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        [Fact]
        public async Task ValidateModuleAsync_ShouldWorkCorrectly()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempAsyncValidation");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""10"" value=""100""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = await Task.Run(() => _detector.ValidateModule(tempDir));
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.TotalFiles);
                Assert.True(result.IsValid);
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }
    }
}