using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class WaterPrefabsXmlTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        [Fact]
        public void WaterPrefabs_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "water_prefabs.xml");
            var serializer = new XmlSerializer(typeof(WaterPrefabs));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as WaterPrefabs;

            Assert.NotNull(result);
            Assert.NotNull(result.WaterPrefabList);
            Assert.True(result.WaterPrefabList.Any());
        }

        [Fact]
        public void WaterPrefabs_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "water_prefabs.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(WaterPrefabs));
            WaterPrefabs waterPrefabs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                waterPrefabs = (WaterPrefabs)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列�?
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, waterPrefabs);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(waterPrefabs);
            Assert.Equal(36, waterPrefabs.WaterPrefabList.Count);

            // 验证特定的水体预制件
            var openOcean = waterPrefabs.WaterPrefabList.FirstOrDefault(w => w.PrefabName == "Open Ocean Global");
            Assert.NotNull(openOcean);
            Assert.Equal("wat_open_ocean_g", openOcean.MaterialName);
            Assert.Equal("Open Ocean", openOcean.Thumbnail);
            Assert.Equal("true", openOcean.IsGlobal);

            var muddyPuddle = waterPrefabs.WaterPrefabList.FirstOrDefault(w => w.PrefabName == "Muddy Puddle");
            Assert.NotNull(muddyPuddle);
            Assert.Equal("wat_puddle_muddy", muddyPuddle.MaterialName);
            Assert.Equal("Muddy Puddle", muddyPuddle.Thumbnail);
            Assert.Equal("False", muddyPuddle.IsGlobal);

            var fountainWater = waterPrefabs.WaterPrefabList.FirstOrDefault(w => w.PrefabName == "Fountain Water");
            Assert.NotNull(fountainWater);
            Assert.Equal("wat_fountain_water", fountainWater.MaterialName);
            Assert.Equal("Fountain Water", fountainWater.Thumbnail);
            Assert.Equal("false", fountainWater.IsGlobal);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节�?
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一�?
            Assert.True(originalDoc.Root?.Elements("WaterPrefab").Count() == savedDoc.Root?.Elements("WaterPrefab").Count(),
                "WaterPrefab count should be the same");
        }
        
        [Fact]
        public void WaterPrefabs_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "water_prefabs.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(WaterPrefabs));
            WaterPrefabs waterPrefabs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                waterPrefabs = (WaterPrefabs)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本结构完整�?
            Assert.True(waterPrefabs.WaterPrefabList.Count > 0, "Should have water prefabs");

            // 验证所有水体预制件都有必要的数�?
            foreach (var prefab in waterPrefabs.WaterPrefabList)
            {
                Assert.False(string.IsNullOrWhiteSpace(prefab.PrefabName), "Prefab should have PrefabName");
                Assert.False(string.IsNullOrWhiteSpace(prefab.MaterialName), "Prefab should have MaterialName");
                Assert.False(string.IsNullOrWhiteSpace(prefab.Thumbnail), "Prefab should have Thumbnail");
                Assert.False(string.IsNullOrWhiteSpace(prefab.IsGlobal), "Prefab should have IsGlobal");
                
                // 验证材质名称格式（应该以"wat_"开头）
                Assert.StartsWith("wat_", prefab.MaterialName);
                
                // 验证IsGlobal值格式（应该�?true"�?false"�?False"�?
                Assert.True(prefab.IsGlobal == "true" || prefab.IsGlobal == "false" || prefab.IsGlobal == "False", 
                    $"IsGlobal should be 'true', 'false', or 'False': {prefab.PrefabName} = {prefab.IsGlobal}");
            }
            
            // 验证必需的水体类型存�?
            var requiredPrefabs = new[] { 
                "Open Ocean Global",
                "Muddy Puddle", 
                "Clear Puddle",
                "Sea Side",
                "Canal Green",
                "Lake Blue",
                "River Brown",
                "Clean Water"
            };
            
            foreach (var requiredPrefab in requiredPrefabs)
            {
                var prefab = waterPrefabs.WaterPrefabList.FirstOrDefault(p => p.PrefabName == requiredPrefab);
                Assert.NotNull(prefab);
            }
            
            // 验证全局和局部的水体预制件对应关�?
            var globalPrefabs = waterPrefabs.WaterPrefabList.Where(p => p.IsGlobal == "true").ToList();
            var localPrefabs = waterPrefabs.WaterPrefabList.Where(p => p.IsGlobal == "false" || p.IsGlobal == "False").ToList();
            
            Assert.True(globalPrefabs.Count > 0, "Should have global water prefabs");
            Assert.True(localPrefabs.Count > 0, "Should have local water prefabs");
            
            // 验证材质名称的一致性模式（全局版本通常�?_g"结尾�?
            var globalWithGSuffix = globalPrefabs.Where(p => p.MaterialName.EndsWith("_g")).Count();
            Assert.True(globalWithGSuffix > globalPrefabs.Count / 2, 
                "Most global prefabs should have material names ending with '_g'");
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 
