using BannerlordModEditor.Common.Models.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public void DecalSets_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("decal_set", result.Type);
            Assert.NotNull(result.DecalSetsContainer);
            Assert.NotNull(result.DecalSetsContainer.DecalSet);
            Assert.Equal(4, result.DecalSetsContainer.DecalSet.Count);
            
            // 验证具体的贴花集数据
            var editorSet = result.DecalSetsContainer.DecalSet.FirstOrDefault(d => d.Name == "editor_set");
            Assert.NotNull(editorSet);
            Assert.Equal("999999999999.000000", editorSet.TotalDecalLifeBase);
            Assert.Equal("999999999999.000000", editorSet.VisibleDecalLifeBase);
            Assert.Equal("false", editorSet.AdaptiveTimeLimit);
            Assert.Equal("false", editorSet.FadeOutDelete);
            
            var bloodSet = result.DecalSetsContainer.DecalSet.FirstOrDefault(d => d.Name == "blood_set");
            Assert.NotNull(bloodSet);
            Assert.Equal("900.000000", bloodSet.TotalDecalLifeBase);
            Assert.Equal("600.000000", bloodSet.VisibleDecalLifeBase);
            Assert.Equal("4", bloodSet.MaximumDecalCountPerGrid);
            Assert.Equal("false", bloodSet.AdaptiveTimeLimit);
            Assert.Equal("true", bloodSet.FadeOutDelete);
        }
        
        [Fact]
        public void DecalSets_ValidateDataIntegrity()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有贴花集都有必要的属性
            Assert.Equal("decal_set", result.Type);
            Assert.NotNull(result.DecalSetsContainer);
            Assert.True(result.DecalSetsContainer.DecalSet.Count > 0);
            
            foreach (var decalSet in result.DecalSetsContainer.DecalSet)
            {
                Assert.False(string.IsNullOrWhiteSpace(decalSet.Name), "Decal set should have Name");
                Assert.False(string.IsNullOrWhiteSpace(decalSet.TotalDecalLifeBase), "Decal set should have TotalDecalLifeBase");
                Assert.False(string.IsNullOrWhiteSpace(decalSet.VisibleDecalLifeBase), "Decal set should have VisibleDecalLifeBase");
                
                // 验证数值格式
                Assert.True(float.TryParse(decalSet.TotalDecalLifeBase, NumberStyles.Float, CultureInfo.InvariantCulture, out float totalLife), 
                    $"TotalDecalLifeBase '{decalSet.TotalDecalLifeBase}' should be a valid float");
                Assert.True(float.TryParse(decalSet.VisibleDecalLifeBase, NumberStyles.Float, CultureInfo.InvariantCulture, out float visibleLife),
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
        }
        
        [Fact]
        public void DecalSets_ValidatePerformanceCharacteristics()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证性能相关的配置合理性
            foreach (var decalSet in result.DecalSetsContainer!.DecalSet)
            {
                var totalLife = float.Parse(decalSet.TotalDecalLifeBase!, CultureInfo.InvariantCulture);
                var visibleLife = float.Parse(decalSet.VisibleDecalLifeBase!, CultureInfo.InvariantCulture);
                
                // 验证生命周期的合理性
                Assert.True(totalLife > 0, $"Total life should be positive for {decalSet.Name}");
                Assert.True(visibleLife > 0, $"Visible life should be positive for {decalSet.Name}");
                Assert.True(totalLife >= visibleLife, $"Total life should be >= visible life for {decalSet.Name}");
                
                // 验证特定配置的合理性
                if (decalSet.Name == "editor_set")
                {
                    // 编辑器集应该有非常长的生命周期
                    Assert.True(totalLife > 1000000, "Editor set should have very long life");
                    Assert.Equal("false", decalSet.AdaptiveTimeLimit); // 编辑器不应该有自适应限制
                }
                else if (decalSet.Name == "steps_set")
                {
                    // 脚步集应该有较短的生命周期
                    Assert.True(totalLife < 100, "Steps set should have short life for performance");
                    Assert.Equal("true", decalSet.AdaptiveTimeLimit); // 脚步应该有自适应限制
                }
                else if (decalSet.Name == "blood_set")
                {
                    // 血迹集应该有中等生命周期
                    Assert.True(totalLife > 100 && totalLife < 10000, "Blood set should have medium life");
                    Assert.NotNull(decalSet.MaximumDecalCountPerGrid);
                    var maxCount = int.Parse(decalSet.MaximumDecalCountPerGrid);
                    Assert.True(maxCount > 0, "Blood set should have count limit for performance");
                }
            }
        }
        
        [Fact]
        public void DecalSets_ValidateSpecificSets()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证预期的关键配置集存在
            var setDict = result.DecalSetsContainer!.DecalSet.ToDictionary(s => s.Name, s => s);
            
            // 验证编辑器集
            Assert.True(setDict.ContainsKey("editor_set"));
            var editorSet = setDict["editor_set"];
            Assert.Equal("999999999999.000000", editorSet.TotalDecalLifeBase);
            Assert.Equal("0", editorSet.MaximumDecalCountPerGrid);
            Assert.Equal("false", editorSet.AdaptiveTimeLimit);
            
            // 验证默认集
            Assert.True(setDict.ContainsKey("default_set"));
            var defaultSet = setDict["default_set"];
            Assert.Equal("240.000000", defaultSet.TotalDecalLifeBase);
            Assert.Equal("60.000000", defaultSet.VisibleDecalLifeBase);
            Assert.Equal("true", defaultSet.AdaptiveTimeLimit);
            
            // 验证血迹集
            Assert.True(setDict.ContainsKey("blood_set"));
            var bloodSet = setDict["blood_set"];
            Assert.Equal("900.000000", bloodSet.TotalDecalLifeBase);
            Assert.Equal("600.000000", bloodSet.VisibleDecalLifeBase);
            Assert.Equal("4", bloodSet.MaximumDecalCountPerGrid);
            
            // 验证脚步集
            Assert.True(setDict.ContainsKey("steps_set"));
            var stepsSet = setDict["steps_set"];
            Assert.Equal("40.000000", stepsSet.TotalDecalLifeBase);
            Assert.Equal("10.000000", stepsSet.VisibleDecalLifeBase);
            Assert.Equal("4", stepsSet.MaximumDecalCountPerGrid);
        }
        
        [Fact]
        public void DecalSets_ValidateNameUniqueness()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证集合名称唯一性
            var allNames = result.DecalSetsContainer!.DecalSet.Select(s => s.Name).ToList();
            var uniqueNames = allNames.Distinct().ToList();
            Assert.Equal(allNames.Count, uniqueNames.Count);
            
            // 验证名称格式（应该是snake_case格式）
            foreach (var name in allNames)
            {
                Assert.False(string.IsNullOrWhiteSpace(name));
                Assert.DoesNotContain(' ', name!); // 不应包含空格
                Assert.DoesNotContain('-', name); // 不应包含连字符
                Assert.True(name.ToLower() == name, $"Name '{name}' should be lowercase"); // 应该是小写
                Assert.True(name.Contains('_'), $"Name '{name}' should use underscore format"); // 应该包含下划线
            }
        }
        
        [Fact]
        public void DecalSets_ValidateVisibilityAreaFormat()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证最小可见区域格式
            foreach (var decalSet in result.DecalSetsContainer!.DecalSet)
            {
                Assert.False(string.IsNullOrWhiteSpace(decalSet.MinVisibilityArea));
                
                // 验证格式应该是 "x, y"
                var parts = decalSet.MinVisibilityArea!.Split(',');
                Assert.Equal(2, parts.Length);
                
                Assert.True(float.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float x),
                    $"X coordinate in MinVisibilityArea should be valid float: {decalSet.MinVisibilityArea}");
                Assert.True(float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float y),
                    $"Y coordinate in MinVisibilityArea should be valid float: {decalSet.MinVisibilityArea}");
                
                // 坐标应该是非负的
                Assert.True(x >= 0, $"X coordinate should be non-negative: {x}");
                Assert.True(y >= 0, $"Y coordinate should be non-negative: {y}");
            }
        }
        
        [Fact]
        public void DecalSets_CanRoundtripXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets original;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                original = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string serializedXml;
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
                    serializer.Serialize(xmlWriter, original);
                }
                serializedXml = writer.ToString();
            }
            
            // Act - 重新反序列化
            DecalSets roundTripped;
            using (var reader = new StringReader(serializedXml))
            {
                roundTripped = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.Equal(original.Type, roundTripped.Type);
            Assert.Equal(original.DecalSetsContainer?.DecalSet?.Count, 
                        roundTripped.DecalSetsContainer?.DecalSet?.Count);
            
            // 验证每个配置集都保持不变
            var originalDict = original.DecalSetsContainer!.DecalSet.ToDictionary(s => s.Name, s => s);
            var roundTrippedDict = roundTripped.DecalSetsContainer!.DecalSet.ToDictionary(s => s.Name, s => s);
            
            Assert.Equal(originalDict.Count, roundTrippedDict.Count);
            foreach (var kvp in originalDict)
            {
                Assert.True(roundTrippedDict.ContainsKey(kvp.Key!));
                var original_set = kvp.Value;
                var roundTripped_set = roundTrippedDict[kvp.Key!];
                
                Assert.Equal(original_set.TotalDecalLifeBase, roundTripped_set.TotalDecalLifeBase);
                Assert.Equal(original_set.VisibleDecalLifeBase, roundTripped_set.VisibleDecalLifeBase);
                Assert.Equal(original_set.MaximumDecalCountPerGrid, roundTripped_set.MaximumDecalCountPerGrid);
                Assert.Equal(original_set.MinVisibilityArea, roundTripped_set.MinVisibilityArea);
                Assert.Equal(original_set.AdaptiveTimeLimit, roundTripped_set.AdaptiveTimeLimit);
                Assert.Equal(original_set.FadeOutDelete, roundTripped_set.FadeOutDelete);
            }
        }
        
        [Fact]
        public void DecalSets_ValidateConfigurationConsistency()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "decal_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(DecalSets));
            DecalSets result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (DecalSets)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证配置的逻辑一致性
            foreach (var decalSet in result.DecalSetsContainer!.DecalSet)
            {
                var totalLife = float.Parse(decalSet.TotalDecalLifeBase!, CultureInfo.InvariantCulture);
                var visibleLife = float.Parse(decalSet.VisibleDecalLifeBase!, CultureInfo.InvariantCulture);
                
                // 总生命周期应该大于等于可见生命周期
                Assert.True(totalLife >= visibleLife, 
                    $"Total life ({totalLife}) should be >= visible life ({visibleLife}) for {decalSet.Name}");
                
                // 如果设置了自适应时间限制，通常意味着是高频率的贴花
                if (decalSet.AdaptiveTimeLimit == "true")
                {
                    Assert.True(totalLife <= 1000, 
                        $"Sets with adaptive time limit should have shorter life for performance: {decalSet.Name}");
                }
                
                // 如果设置了网格限制，应该有合理的计数
                if (!string.IsNullOrEmpty(decalSet.MaximumDecalCountPerGrid))
                {
                    var maxCount = int.Parse(decalSet.MaximumDecalCountPerGrid);
                    if (maxCount > 0)
                    {
                        Assert.True(maxCount <= 10, 
                            $"Maximum decal count per grid should be reasonable: {maxCount} for {decalSet.Name}");
                    }
                }
                
                // 编辑器集应该有特殊配置
                if (decalSet.Name == "editor_set")
                {
                    Assert.Equal("false", decalSet.AdaptiveTimeLimit);
                    Assert.Equal("false", decalSet.FadeOutDelete);
                    Assert.Equal("0", decalSet.MaximumDecalCountPerGrid);
                }
            }
        }
        
        [Fact]
        public void DecalSets_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""decal_set"">
    <decal_sets>
    </decal_sets>
</base>";

            var serializer = new XmlSerializer(typeof(DecalSets));

            // Act
            DecalSets? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (DecalSets?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("decal_set", result.Type);
            Assert.NotNull(result.DecalSetsContainer);
            // 空的贴花集应该被处理为空集合
            Assert.NotNull(result.DecalSetsContainer.DecalSet);
            Assert.Empty(result.DecalSetsContainer.DecalSet);
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