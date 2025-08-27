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
    /// XML验证系统核心测试
    /// 测试完整的XML验证流程，包括依赖关系分析、隐式校验和Schema验证
    /// </summary>
    public class XmlValidationSystemCoreTests
    {
        private readonly XmlValidationSystemCore _validationSystem;
        private readonly TestHelper _testHelper;
        private readonly string _testDataPath;

        public XmlValidationSystemCoreTests()
        {
            _testHelper = new TestHelper();
            _testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            
            var fileDiscoveryService = new FileDiscoveryService();
            var dependencyAnalyzer = new XmlDependencyAnalyzer(fileDiscoveryService);
            var validationDetector = new ImplicitValidationDetector(fileDiscoveryService, dependencyAnalyzer);
            
            _validationSystem = new XmlValidationSystemCore(fileDiscoveryService, dependencyAnalyzer, validationDetector);
        }

        [Fact]
        public void ValidateModule_ShouldPerformCompleteValidation()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempCompleteValidation");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建测试文件
                CreateTestFiles(tempDir);
                
                // Act
                var result = _validationSystem.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalFiles > 0);
                Assert.NotNull(result.DependencyAnalysis);
                Assert.NotNull(result.ImplicitValidation);
                Assert.NotNull(result.SchemaValidation);
                Assert.NotNull(result.Summary);
                Assert.NotNull(result.FixSuggestions);
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
        public void ValidateModule_ShouldDetectMultipleIssues()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempMultipleIssues");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建包含多个问题的测试文件
                var problematicXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""duplicate_item"" weight=""-5"" value=""-100""/>
    <Item id=""duplicate_item"" weight=""2000"" value=""500""/>
    <Item id=""item_with_bad_ref"" item=""nonexistent.item""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "items.xml");
                File.WriteAllText(testFile, problematicXml);
                
                // Act
                var result = _validationSystem.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.False(result.IsValid);
                Assert.True(result.TotalErrors > 0);
                Assert.True(result.TotalWarnings > 0);
                
                // 验证检测到的问题类型
                Assert.Contains(result.Summary.Issues, i => i.Contains("错误"));
                Assert.Contains(result.Summary.Issues, i => i.Contains("警告"));
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
        public void ValidateSingleFile_ShouldWorkCorrectly()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempSingleFileValidation");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""10"" value=""100""/>
    <Item id=""test_item2"" weight=""-5"" value=""200""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "test_items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _validationSystem.ValidateSingleFile(testFile);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("test_items.xml", result.FileName);
                Assert.True(result.TotalWarnings > 0); // 应该检测到负重量
                Assert.NotNull(result.ImplicitValidation);
                Assert.NotNull(result.SchemaValidation);
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
        public void GetRecommendedLoadOrder_ShouldReturnValidOrder()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempLoadOrder");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                CreateTestFiles(tempDir);
                
                // Act
                var loadOrder = _validationSystem.GetRecommendedLoadOrder(tempDir);
                
                // Assert
                Assert.NotNull(loadOrder);
                Assert.True(loadOrder.Count > 0);
                
                // 验证顺序合理性
                Assert.Equal(loadOrder.Count, loadOrder.Distinct().Count());
                
                // 基础文件应该在前
                var basicIndex = loadOrder.IndexOf("managed_core_parameters");
                var advancedIndex = loadOrder.IndexOf("items");
                
                if (basicIndex != -1 && advancedIndex != -1)
                {
                    Assert.True(basicIndex < advancedIndex);
                }
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
        public void GetDependencyGraph_ShouldReturnValidGraph()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempDependencyGraph");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                CreateTestFiles(tempDir);
                
                // Act
                var graph = _validationSystem.GetDependencyGraph(tempDir);
                
                // Assert
                Assert.NotNull(graph);
                Assert.True(graph.Count > 0);
                
                // 验证图结构
                foreach (var kvp in graph)
                {
                    Assert.NotNull(kvp.Key);
                    Assert.NotNull(kvp.Value);
                }
                
                // 验证特定依赖关系
                if (graph.ContainsKey("items"))
                {
                    Assert.Contains("crafting_pieces", graph["items"]);
                }
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
        public void ValidateModule_ShouldGenerateFixSuggestions()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempFixSuggestions");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建包含循环依赖的测试文件
                var fileA = Path.Combine(tempDir, "file_a.xml");
                var fileB = Path.Combine(tempDir, "file_b.xml");
                
                File.WriteAllText(fileA, @"<root><item ref=""file_b.item1""/></root>");
                File.WriteAllText(fileB, @"<root><item ref=""file_a.item1""/></root>");
                
                // Act
                var result = _validationSystem.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.FixSuggestions.Count > 0);
                
                var circularDepSuggestion = result.FixSuggestions.FirstOrDefault(f => 
                    f.Category == "循环依赖");
                Assert.NotNull(circularDepSuggestion);
                Assert.Equal(FixPriority.High, circularDepSuggestion.Priority);
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
            var emptyDir = Path.Combine(_testDataPath, "EmptyValidationSystem");
            Directory.CreateDirectory(emptyDir);
            
            try
            {
                // Act
                var result = _validationSystem.ValidateModule(emptyDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(0, result.TotalFiles);
                Assert.Equal(0, result.TotalErrors);
                Assert.True(result.IsValid);
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
            var invalidDir = Path.Combine(_testDataPath, "InvalidValidationSystem");
            Directory.CreateDirectory(invalidDir);
            
            try
            {
                var invalidFile = Path.Combine(invalidDir, "invalid.xml");
                File.WriteAllText(invalidFile, "This is not valid XML content");
                
                // Act
                var result = _validationSystem.ValidateModule(invalidDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.False(result.IsValid);
                Assert.True(result.Errors.Count > 0);
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
        public async Task ValidateModuleAsync_ShouldWorkCorrectly()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempAsyncValidationSystem");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                CreateTestFiles(tempDir);
                
                // Act
                var result = await _validationSystem.ValidateModuleAsync(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalFiles > 0);
                Assert.NotNull(result.ValidationTimestamp);
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
        public void ValidateModule_ShouldProvideComprehensiveSummary()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempComprehensiveSummary");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建包含各种问题的测试文件
                CreateProblematicFiles(tempDir);
                
                // Act
                var result = _validationSystem.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                Assert.NotNull(result.Summary);
                Assert.True(result.Summary.Issues.Count > 0);
                
                // 验证摘要包含关键信息
                var summaryText = string.Join(" ", result.Summary.Issues);
                Assert.Contains("错误", summaryText);
                Assert.Contains("警告", summaryText);
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
        public void ValidateSingleFile_ShouldReturnDetailedResults()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempDetailedSingleFile");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""10"" value=""100""/>
    <Item id=""test_item"" weight=""15"" value=""200""/>
    <Item id=""item2"" weight=""-5"" value=""300""/>
</Items>";
                
                var testFile = Path.Combine(tempDir, "test_items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _validationSystem.ValidateSingleFile(testFile);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.TotalErrors > 0); // 重复ID
                Assert.True(result.TotalWarnings > 0); // 负重量
                Assert.NotNull(result.ImplicitValidation);
                Assert.NotNull(result.SchemaValidation);
                Assert.Equal(testFile, result.FilePath);
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
        public void ValidateModule_ShouldIntegrateAllValidationTypes()
        {
            // Arrange
            var tempDir = Path.Combine(_testDataPath, "TempIntegratedValidation");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                CreateTestFiles(tempDir);
                
                // Act
                var result = _validationSystem.ValidateModule(tempDir);
                
                // Assert
                Assert.NotNull(result);
                
                // 验证所有验证类型都被执行
                Assert.NotNull(result.DependencyAnalysis);
                Assert.NotNull(result.ImplicitValidation);
                Assert.NotNull(result.SchemaValidation);
                
                // 验证结果整合
                Assert.True(result.TotalFiles > 0);
                Assert.True(result.DependencyAnalysis.FileResults.Count > 0);
                Assert.True(result.ImplicitValidation.FileResults.Count > 0);
                Assert.True(result.SchemaValidation.FileResults.Count > 0);
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

        /// <summary>
        /// 创建测试文件
        /// </summary>
        private void CreateTestFiles(string directory)
        {
            // 创建基础文件
            var basicXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<managed_core_parameters>
    <parameter name=""test_param"" value=""1.0""/>
</managed_core_parameters>";
            
            File.WriteAllText(Path.Combine(directory, "managed_core_parameters.xml"), basicXml);
            
            // 创建制作部件文件
            var craftingXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CraftingPieces>
    <CraftingPiece id=""test_piece"" difficulty=""50"" damage=""10""/>
</CraftingPieces>";
            
            File.WriteAllText(Path.Combine(directory, "crafting_pieces.xml"), craftingXml);
            
            // 创建物品文件
            var itemsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" weight=""5"" value=""100""/>
    <Item id=""test_item2"" weight=""10"" value=""200""/>
</Items>";
            
            File.WriteAllText(Path.Combine(directory, "items.xml"), itemsXml);
        }

        /// <summary>
        /// 创建包含问题的测试文件
        /// </summary>
        private void CreateProblematicFiles(string directory)
        {
            // 创建重复ID的文件
            var duplicateIdXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""duplicate_item"" weight=""10"" value=""100""/>
    <Item id=""duplicate_item"" weight=""15"" value=""200""/>
</Items>";
            
            File.WriteAllText(Path.Combine(directory, "items.xml"), duplicateIdXml);
            
            // 创建无效数值的文件
            var invalidValueXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Characters>
    <Character id=""test_char"" level=""0"" occupation=""Soldier""/>
</Characters>";
            
            File.WriteAllText(Path.Combine(directory, "characters.xml"), invalidValueXml);
            
            // 创建循环依赖的文件
            var fileA = Path.Combine(directory, "file_a.xml");
            var fileB = Path.Combine(directory, "file_b.xml");
            
            File.WriteAllText(fileA, @"<root><item ref=""file_b.item1""/></root>");
            File.WriteAllText(fileB, @"<root><item ref=""file_a.item1""/></root>");
        }
    }
}