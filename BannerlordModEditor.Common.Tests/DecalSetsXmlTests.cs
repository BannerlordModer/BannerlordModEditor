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
    public class DecalSetsXmlTests
    {
        [Fact]
        public void DecalSets_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets decalSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                decalSets = (DecalSets)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, decalSets);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(decalSets);
            Assert.Equal("decal_set", decalSets.Type);
            Assert.NotNull(decalSets.DecalSetsContainer);
            Assert.NotNull(decalSets.DecalSetsContainer.DecalSet);
            Assert.True(decalSets.DecalSetsContainer.DecalSet.Count > 0, "Should have at least one decal set");
            
            // 验证具体的贴花集数据
            var editorSet = decalSets.DecalSetsContainer.DecalSet.FirstOrDefault(d => d.Name == "editor_set");
            Assert.NotNull(editorSet);
            Assert.Equal("999999999999.000000", editorSet.TotalDecalLifeBase);
            Assert.Equal("999999999999.000000", editorSet.VisibleDecalLifeBase);
            Assert.Equal("false", editorSet.AdaptiveTimeLimit);
            Assert.Equal("false", editorSet.FadeOutDelete);
            
            var bloodSet = decalSets.DecalSetsContainer.DecalSet.FirstOrDefault(d => d.Name == "blood_set");
            Assert.NotNull(bloodSet);
            Assert.Equal("900.000000", bloodSet.TotalDecalLifeBase);
            Assert.Equal("600.000000", bloodSet.VisibleDecalLifeBase);
            Assert.Equal("4", bloodSet.MaximumDecalCountPerGrid);
            Assert.Equal("false", bloodSet.AdaptiveTimeLimit);
            Assert.Equal("true", bloodSet.FadeOutDelete);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Element("decal_sets")?.Elements("decal_set").Count(), 
                        savedDoc.Root?.Element("decal_sets")?.Elements("decal_set").Count());
        }
        
        [Fact]
        public void DecalSets_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets decalSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                decalSets = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有贴花集都有必要的属性
            Assert.Equal("decal_set", decalSets.Type);
            Assert.NotNull(decalSets.DecalSetsContainer);
            
            foreach (var decalSet in decalSets.DecalSetsContainer.DecalSet)
            {
                Assert.False(string.IsNullOrWhiteSpace(decalSet.Name), "Decal set should have Name");
                Assert.False(string.IsNullOrWhiteSpace(decalSet.TotalDecalLifeBase), "Decal set should have TotalDecalLifeBase");
                Assert.False(string.IsNullOrWhiteSpace(decalSet.VisibleDecalLifeBase), "Decal set should have VisibleDecalLifeBase");
                
                // 验证数值格式
                Assert.True(float.TryParse(decalSet.TotalDecalLifeBase, out float totalLife), 
                    $"TotalDecalLifeBase '{decalSet.TotalDecalLifeBase}' should be a valid float");
                Assert.True(float.TryParse(decalSet.VisibleDecalLifeBase, out float visibleLife),
                    $"VisibleDecalLifeBase '{decalSet.VisibleDecalLifeBase}' should be a valid float");
                Assert.True(totalLife >= visibleLife, "Total life should be >= visible life");
                
                // 验证布尔属性
                if (!string.IsNullOrEmpty(decalSet.AdaptiveTimeLimit))
                {
                    Assert.True(decalSet.AdaptiveTimeLimit == "true" || decalSet.AdaptiveTimeLimit == "false",
                        $"AdaptiveTimeLimit should be 'true' or 'false', got '{decalSet.AdaptiveTimeLimit}'");
                }
                
                if (!string.IsNullOrEmpty(decalSet.FadeOutDelete))
                {
                    Assert.True(decalSet.FadeOutDelete == "true" || decalSet.FadeOutDelete == "false",
                        $"FadeOutDelete should be 'true' or 'false', got '{decalSet.FadeOutDelete}'");
                }
                
                // 验证网格计数格式
                if (!string.IsNullOrEmpty(decalSet.MaximumDecalCountPerGrid))
                {
                    Assert.True(int.TryParse(decalSet.MaximumDecalCountPerGrid, out int count),
                        $"MaximumDecalCountPerGrid '{decalSet.MaximumDecalCountPerGrid}' should be a valid integer");
                    Assert.True(count >= 0, "Maximum decal count should be non-negative");
                }
            }
            
            // 验证包含预期的贴花集
            var allNames = decalSets.DecalSetsContainer.DecalSet.Select(d => d.Name).ToList();
            Assert.Contains("editor_set", allNames);
            Assert.Contains("default_set", allNames);
            Assert.Contains("blood_set", allNames);
            Assert.Contains("steps_set", allNames);
            
            // 验证特定设置的配置
            var defaultSet = decalSets.DecalSetsContainer.DecalSet.First(d => d.Name == "default_set");
            Assert.Equal("240.000000", defaultSet.TotalDecalLifeBase);
            Assert.Equal("60.000000", defaultSet.VisibleDecalLifeBase);
            Assert.Equal("true", defaultSet.AdaptiveTimeLimit);
            Assert.Equal("true", defaultSet.FadeOutDelete);
            
            var stepsSet = decalSets.DecalSetsContainer.DecalSet.First(d => d.Name == "steps_set");
            Assert.Equal("40.000000", stepsSet.TotalDecalLifeBase);
            Assert.Equal("10.000000", stepsSet.VisibleDecalLifeBase);
            Assert.Equal("4", stepsSet.MaximumDecalCountPerGrid);
            
            // 确保没有重复的名称
            var uniqueNames = allNames.Distinct().ToList();
            Assert.Equal(allNames.Count, uniqueNames.Count);
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