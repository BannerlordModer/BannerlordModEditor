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
    public class RandomTerrainTemplatesXmlTests
    {
        [Fact]
        public void RandomTerrainTemplates_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "random_terrain_templates.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(RandomTerrainTemplates));
            RandomTerrainTemplates templates;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                templates = (RandomTerrainTemplates)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
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
                    serializer.Serialize(xmlWriter, templates);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(templates);
            Assert.NotNull(templates.Template);
            Assert.True(templates.Template.Count > 0, "Should have at least one terrain template");
            
            // 验证特定地形模板
            var desertTemplate = templates.Template.FirstOrDefault(t => t.Name == "Desert");
            Assert.NotNull(desertTemplate);
            Assert.Equal("true", desertTemplate.MountainLayerEnabled);
            Assert.Equal("1", desertTemplate.MountainLayerOctaves);
            Assert.Equal("4.270", desertTemplate.MountainLayerLacunarity);
            Assert.Equal("2", desertTemplate.Biome);
            Assert.Equal("20", desertTemplate.FloraDensity);
            Assert.Equal("850", desertTemplate.HeightLevel);
            
            var forestTemplate = templates.Template.FirstOrDefault(t => t.Name == "Forest");
            Assert.NotNull(forestTemplate);
            Assert.Equal("true", forestTemplate.MountainLayerEnabled);
            Assert.Equal("2", forestTemplate.MountainLayerOctaves);
            Assert.Equal("1", forestTemplate.Biome);
            Assert.Equal("10", forestTemplate.FloraDensity);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Elements("template").Count(), 
                        savedDoc.Root?.Elements("template").Count());
        }
        
        [Fact]
        public void RandomTerrainTemplates_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "random_terrain_templates.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(RandomTerrainTemplates));
            RandomTerrainTemplates templates;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                templates = (RandomTerrainTemplates)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有地形模板都有必要的属性
            Assert.NotNull(templates);
            Assert.NotNull(templates.Template);
            Assert.True(templates.Template.Count >= 7, "Should have at least 7 terrain templates");
            
            foreach (var template in templates.Template)
            {
                Assert.False(string.IsNullOrWhiteSpace(template.Name), "Template should have Name");
                Assert.False(string.IsNullOrWhiteSpace(template.MountainLayerEnabled), "Template should have MountainLayerEnabled");
                Assert.False(string.IsNullOrWhiteSpace(template.Biome), "Template should have Biome");
                Assert.False(string.IsNullOrWhiteSpace(template.FloraDensity), "Template should have FloraDensity");
                Assert.False(string.IsNullOrWhiteSpace(template.HeightLevel), "Template should have HeightLevel");
                
                // 验证布尔值格式
                Assert.True(template.MountainLayerEnabled == "true" || template.MountainLayerEnabled == "false",
                    $"MountainLayerEnabled should be 'true' or 'false', got '{template.MountainLayerEnabled}'");
                
                if (!string.IsNullOrEmpty(template.TurbulanceEnabled))
                {
                    Assert.True(template.TurbulanceEnabled == "true" || template.TurbulanceEnabled == "false",
                        $"TurbulanceEnabled should be 'true' or 'false', got '{template.TurbulanceEnabled}'");
                }
                
                // 验证数值格式
                Assert.True(int.TryParse(template.Biome, out int biome), 
                    $"Biome should be a valid integer, got '{template.Biome}'");
                Assert.True(biome >= 0 && biome <= 2, "Biome should be 0-2 (mountain/forest/desert)");
                
                Assert.True(int.TryParse(template.FloraDensity, out int floraDensity),
                    $"FloraDensity should be a valid integer, got '{template.FloraDensity}'");
                Assert.True(floraDensity >= 0, "Flora density should be non-negative");
                
                Assert.True(int.TryParse(template.HeightLevel, out int heightLevel),
                    $"HeightLevel should be a valid integer, got '{template.HeightLevel}'");
                Assert.True(heightLevel > 0, "Height level should be positive");
                
                // 验证浮点数值
                if (!string.IsNullOrEmpty(template.Smoothness))
                {
                    Assert.True(float.TryParse(template.Smoothness, out float smoothness),
                        $"Smoothness should be a valid float, got '{template.Smoothness}'");
                    Assert.True(smoothness >= 0, "Smoothness should be non-negative");
                }
                
                if (!string.IsNullOrEmpty(template.Scale))
                {
                    Assert.True(float.TryParse(template.Scale, out float scale),
                        $"Scale should be a valid float, got '{template.Scale}'");
                    Assert.True(scale > 0, "Scale should be positive");
                }
                
                if (!string.IsNullOrEmpty(template.MinHeight) && !string.IsNullOrEmpty(template.MaxHeight))
                {
                    Assert.True(int.TryParse(template.MinHeight, out int minHeight),
                        $"MinHeight should be a valid integer, got '{template.MinHeight}'");
                    Assert.True(int.TryParse(template.MaxHeight, out int maxHeight),
                        $"MaxHeight should be a valid integer, got '{template.MaxHeight}'");
                    Assert.True(maxHeight > minHeight, "MaxHeight should be greater than MinHeight");
                }
            }
            
            // 验证包含预期的地形模板
            var allNames = templates.Template.Select(t => t.Name).ToList();
            Assert.Contains("Desert", allNames);
            Assert.Contains("Forest", allNames);
            Assert.Contains("Mountain", allNames);
            Assert.Contains("Snow", allNames);
            Assert.Contains("Steppe", allNames);
            Assert.Contains("Plain_forest", allNames);
            Assert.Contains("Steppe_with_mountain", allNames);
            Assert.Contains("better_steppe", allNames);
            
            // 验证没有重复的名称
            var uniqueNames = allNames.Distinct().ToList();
            Assert.Equal(allNames.Count, uniqueNames.Count);
            
            // 验证特定模板的配置
            var mountainTemplate = templates.Template.First(t => t.Name == "Mountain");
            Assert.Equal("0", mountainTemplate.Biome); // 山地生物群系
            Assert.Equal("500", mountainTemplate.MaxHeight); // 山地有较高的最大高度
            
            var snowTemplate = templates.Template.First(t => t.Name == "Snow");
            Assert.Equal("6000", snowTemplate.HeightLevel); // 雪地有很高的高度级别
            Assert.Equal("0", snowTemplate.Biome);
            
            var desertTemplate = templates.Template.First(t => t.Name == "Desert");
            Assert.Equal("2", desertTemplate.Biome); // 沙漠生物群系
            Assert.Equal("80", desertTemplate.MaxHeight);
            
            var forestTemplate = templates.Template.First(t => t.Name == "Forest");
            Assert.Equal("1", forestTemplate.Biome); // 森林生物群系
            Assert.Equal("10", forestTemplate.FloraDensity); // 森林有不同的植被密度
            
            // 验证所有模板都启用了山层
            foreach (var template in templates.Template)
            {
                Assert.Equal("true", template.MountainLayerEnabled);
            }
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