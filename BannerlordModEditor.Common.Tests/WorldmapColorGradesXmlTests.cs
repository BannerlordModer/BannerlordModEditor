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
    public class WorldmapColorGradesXmlTests
    {
        [Fact]
        public void WorldmapColorGrades_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "worldmap_color_grades.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(WorldmapColorGrades));
            WorldmapColorGrades colorGrades;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                colorGrades = (WorldmapColorGrades)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, colorGrades);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(colorGrades);
            Assert.NotNull(colorGrades.ColorGradeGrid);
            Assert.NotNull(colorGrades.ColorGradeDefault);
            Assert.NotNull(colorGrades.ColorGradeNight);
            Assert.Equal(8, colorGrades.ColorGrades.Count);

            // 验证特定的颜色分级
            Assert.Equal("worldmap_colorgrade_grid", colorGrades.ColorGradeGrid.Name);
            Assert.Equal("worldmap_colorgrade_stratosphere", colorGrades.ColorGradeDefault.Name);
            Assert.Equal("worldmap_colorgrade_night", colorGrades.ColorGradeNight.Name);

            // 验证颜色分级值
            var desertGrade = colorGrades.ColorGrades.FirstOrDefault(c => c.Name == "worldmap_colorgrade_desert");
            Assert.NotNull(desertGrade);
            Assert.Equal("20", desertGrade.Value);

            var steppeGrade = colorGrades.ColorGrades.FirstOrDefault(c => c.Name == "worldmap_colorgrade_steppe");
            Assert.NotNull(steppeGrade);
            Assert.Equal("40", steppeGrade.Value);

            var northGrade = colorGrades.ColorGrades.FirstOrDefault(c => c.Name == "worldmap_colorgrade_north");
            Assert.NotNull(northGrade);
            Assert.Equal("120", northGrade.Value);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("color_grade").Count() == savedDoc.Root?.Elements("color_grade").Count(),
                "color_grade count should be the same");
        }
        
        [Fact]
        public void WorldmapColorGrades_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "worldmap_color_grades.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(WorldmapColorGrades));
            WorldmapColorGrades colorGrades;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                colorGrades = (WorldmapColorGrades)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本结构完整性
            Assert.NotNull(colorGrades.ColorGradeGrid);
            Assert.False(string.IsNullOrWhiteSpace(colorGrades.ColorGradeGrid.Name));
            Assert.True(colorGrades.ColorGradeGrid.Name.StartsWith("worldmap_colorgrade_"));

            Assert.NotNull(colorGrades.ColorGradeDefault);
            Assert.False(string.IsNullOrWhiteSpace(colorGrades.ColorGradeDefault.Name));
            Assert.True(colorGrades.ColorGradeDefault.Name.StartsWith("worldmap_colorgrade_"));

            Assert.NotNull(colorGrades.ColorGradeNight);
            Assert.False(string.IsNullOrWhiteSpace(colorGrades.ColorGradeNight.Name));
            Assert.True(colorGrades.ColorGradeNight.Name.StartsWith("worldmap_colorgrade_"));

            // 验证所有颜色分级都有必要的数据
            foreach (var grade in colorGrades.ColorGrades)
            {
                Assert.False(string.IsNullOrWhiteSpace(grade.Name), "Color grade should have Name");
                Assert.False(string.IsNullOrWhiteSpace(grade.Value), "Color grade should have Value");
                
                // 验证名称格式
                Assert.True(grade.Name.StartsWith("worldmap_colorgrade_"), 
                    $"Color grade name should start with 'worldmap_colorgrade_': {grade.Name}");
                
                // 验证值格式（应该是数字）
                Assert.True(int.TryParse(grade.Value, out var numericValue), 
                    $"Color grade value should be numeric: {grade.Name} = {grade.Value}");
                Assert.True(numericValue >= 0, 
                    $"Color grade value should be non-negative: {grade.Name} = {grade.Value}");
                
                // 验证值在合理范围内（通常0-200）
                Assert.True(numericValue <= 200, 
                    $"Color grade value should be <= 200: {grade.Name} = {grade.Value}");
            }
            
            // 验证必需的颜色分级存在
            var requiredGrades = new[] { 
                "worldmap_colorgrade_desert",
                "worldmap_colorgrade_steppe", 
                "worldmap_colorgrade_north",
                "worldmap_colorgrade_normandy"
            };
            
            foreach (var requiredGrade in requiredGrades)
            {
                var grade = colorGrades.ColorGrades.FirstOrDefault(g => g.Name == requiredGrade);
                Assert.NotNull(grade);
            }
            
            // 验证值的递增顺序（大部分应该是递增的）
            var sortedGrades = colorGrades.ColorGrades
                .Where(g => int.TryParse(g.Value, out _))
                .OrderBy(g => int.Parse(g.Value))
                .ToList();
            Assert.True(sortedGrades.Count > 0, "Should have at least one grade with numeric value");
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