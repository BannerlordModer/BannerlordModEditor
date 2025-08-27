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
    /// XML依赖关系分析器测试
    /// 测试基于Mount & Blade源码分析的依赖关系检测功能
    /// </summary>
    public class XmlDependencyAnalyzerTests
    {
        private readonly XmlDependencyAnalyzer _analyzer;
        private readonly TestHelper _testHelper;
        private readonly string _testDataPath;

        public XmlDependencyAnalyzerTests()
        {
            _testHelper = new TestHelper();
            _testDataPath = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
            
            var fileDiscoveryService = new FileDiscoveryService();
            
            _analyzer = new XmlDependencyAnalyzer(fileDiscoveryService);
        }

        [Fact]
        public void AnalyzeDependencies_ShouldDetectBasicDependencies()
        {
            // Arrange
            var testXmlPath = Path.Combine(_testDataPath, "items.xml");
            
            // Act
            var result = _analyzer.AnalyzeDependencies(_testDataPath);
            
            // Assert
            Assert.NotNull(result);
            Assert.True(result.TotalFiles > 0);
            Assert.NotNull(result.FileResults);
            Assert.NotNull(result.LoadOrder);
        }

        [Fact]
        public void AnalyzeDependencies_ShouldDetectItemDependencies()
        {
            // Arrange
            var itemsPath = Path.Combine(_testDataPath, "items.xml");
            var craftingPiecesPath = Path.Combine(_testDataPath, "crafting_pieces.xml");
            
            // Act
            var result = _analyzer.AnalyzeDependencies(_testDataPath);
            var itemsResult = result.FileResults.FirstOrDefault(f => f.FileName == "items.xml");
            
            // Assert
            Assert.NotNull(itemsResult);
            Assert.Contains("crafting_pieces", itemsResult.AllDependencies);
        }

        [Fact]
        public void AnalyzeDependencies_ShouldDetectCircularDependencies()
        {
            // Arrange - 创建测试用的循环依赖文件
            var tempDir = Path.Combine(_testDataPath, "TempCircular");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                // 创建两个相互依赖的文件
                var fileA = Path.Combine(tempDir, "file_a.xml");
                var fileB = Path.Combine(tempDir, "file_b.xml");
                
                File.WriteAllText(fileA, @"<root><item ref=""file_b.item1""/></root>");
                File.WriteAllText(fileB, @"<root><item ref=""file_a.item1""/></root>");
                
                // Act
                var result = _analyzer.AnalyzeDependencies(tempDir);
                
                // Assert
                Assert.True(result.CircularDependencies.Count > 0);
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
        public void AnalyzeDependencies_ShouldCalculateCorrectLoadOrder()
        {
            // Arrange
            var result = _analyzer.AnalyzeDependencies(_testDataPath);
            
            // Act
            var loadOrder = result.LoadOrder;
            
            // Assert
            Assert.NotNull(loadOrder);
            Assert.True(loadOrder.Count > 0);
            
            // 验证基础文件在前面
            var basicFiles = new[] { "managed_core_parameters", "physics_materials" };
            foreach (var basicFile in basicFiles)
            {
                var basicIndex = loadOrder.IndexOf(basicFile);
                var dependentIndex = loadOrder.IndexOf("items");
                
                if (basicIndex != -1 && dependentIndex != -1)
                {
                    Assert.True(basicIndex < dependentIndex, 
                        $"基础文件 {basicFile} 应该在依赖文件之前加载");
                }
            }
        }

        [Fact]
        public void AnalyzeFileDependencies_ShouldDetectContentReferences()
        {
            // Arrange
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" item=""crafting_pieces.test_piece"" character=""test_hero""/>
</Items>";
            
            var tempDir = Path.Combine(_testDataPath, "TempContent");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testFile = Path.Combine(tempDir, "test_items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _analyzer.AnalyzeDependencies(tempDir);
                var fileResult = result.FileResults.FirstOrDefault(f => f.FileName == "test_items.xml");
                
                // Assert
                Assert.NotNull(fileResult);
                Assert.Contains("crafting_pieces", fileResult.ContentDependencies);
                Assert.Contains("characters", fileResult.ContentDependencies);
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
        public void AnalyzeFileDependencies_ShouldDetectMissingDependencies()
        {
            // Arrange
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Items>
    <Item id=""test_item"" item=""nonexistent.test_item""/>
</Items>";
            
            var tempDir = Path.Combine(_testDataPath, "TempMissing");
            Directory.CreateDirectory(tempDir);
            
            try
            {
                var testFile = Path.Combine(tempDir, "test_items.xml");
                File.WriteAllText(testFile, testXml);
                
                // Act
                var result = _analyzer.AnalyzeDependencies(tempDir);
                var fileResult = result.FileResults.FirstOrDefault(f => f.FileName == "test_items.xml");
                
                // Assert
                Assert.NotNull(fileResult);
                Assert.Contains("nonexistent", fileResult.MissingDependencies);
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
            // Act
            var loadOrder = _analyzer.GetRecommendedLoadOrder(_testDataPath);
            
            // Assert
            Assert.NotNull(loadOrder);
            Assert.True(loadOrder.Count > 0);
            
            // 验证没有重复
            Assert.Equal(loadOrder.Count, loadOrder.Distinct().Count());
        }

        [Fact]
        public void GetDependencyGraph_ShouldReturnValidGraph()
        {
            // Act
            var graph = _analyzer.GetDependencyGraph(_testDataPath);
            
            // Assert
            Assert.NotNull(graph);
            Assert.True(graph.Count > 0);
            
            // 验证图结构
            foreach (var kvp in graph)
            {
                Assert.NotNull(kvp.Key);
                Assert.NotNull(kvp.Value);
            }
        }

        [Fact]
        public async Task AnalyzeDependenciesAsync_ShouldWorkCorrectly()
        {
            // Act
            var result = await Task.Run(() => _analyzer.AnalyzeDependencies(_testDataPath));
            
            // Assert
            Assert.NotNull(result);
            Assert.True(result.TotalFiles > 0);
            Assert.True(result.FileResults.Count > 0);
        }

        [Theory]
        [InlineData("items.xml")]
        [InlineData("characters.xml")]
        [InlineData("crafting_pieces.xml")]
        public void AnalyzeSpecificFile_ShouldReturnValidResult(string fileName)
        {
            // Arrange
            var filePath = Path.Combine(_testDataPath, fileName);
            
            // Act
            var result = _analyzer.AnalyzeDependencies(_testDataPath);
            var fileResult = result.FileResults.FirstOrDefault(f => f.FileName == fileName);
            
            // Assert
            Assert.NotNull(fileResult);
            Assert.Equal(fileName, fileResult.FileName);
            Assert.NotNull(fileResult.AllDependencies);
        }

        [Fact]
        public void AnalyzeDependencies_ShouldHandleEmptyDirectory()
        {
            // Arrange
            var emptyDir = Path.Combine(_testDataPath, "EmptyDir");
            Directory.CreateDirectory(emptyDir);
            
            try
            {
                // Act
                var result = _analyzer.AnalyzeDependencies(emptyDir);
                
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
        public void AnalyzeDependencies_ShouldHandleInvalidXml()
        {
            // Arrange
            var invalidDir = Path.Combine(_testDataPath, "InvalidXml");
            Directory.CreateDirectory(invalidDir);
            
            try
            {
                var invalidFile = Path.Combine(invalidDir, "invalid.xml");
                File.WriteAllText(invalidFile, "This is not valid XML content");
                
                // Act
                var result = _analyzer.AnalyzeDependencies(invalidDir);
                
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
    }
}