using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsLayoutDebugTests
    {
        [Fact]
        public void Debug_FloraKindsLayout_XmlComparison()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(originalXml);
            
            // Act
            string serializedXml = XmlTestUtils.Serialize(original);
            
            // 调试信息
            Console.WriteLine("=== FloraKindsLayout 调试信息 ===");
            
            // 比较节点和属性数量
            var (origNodes, origAttrs) = XmlTestUtils.CountNodesAndAttributes(originalXml);
            var (serNodes, serAttrs) = XmlTestUtils.CountNodesAndAttributes(serializedXml);
            
            Console.WriteLine($"原始XML: 节点数={origNodes}, 属性数={origAttrs}");
            Console.WriteLine($"序列化XML: 节点数={serNodes}, 属性数={serAttrs}");
            
            // 检查layout数量
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);
            
            var originalLayouts = originalDoc.Descendants("layout").ToList();
            var serializedLayouts = serializedDoc.Descendants("layout").ToList();
            
            Console.WriteLine($"原始XML layout数量: {originalLayouts.Count}");
            Console.WriteLine($"序列化XML layout数量: {serializedLayouts.Count}");
            
            // 检查每个layout的class属性
            for (int i = 0; i < Math.Min(originalLayouts.Count, serializedLayouts.Count); i++)
            {
                var origLayout = originalLayouts[i];
                var serLayout = serializedLayouts[i];
                
                var origClass = origLayout.Attribute("class")?.Value;
                var serClass = serLayout.Attribute("class")?.Value;
                
                Console.WriteLine($"Layout {i}: 原始={origClass}, 序列化={serClass}");
                
                // 检查items数量
                var origItems = origLayout.Descendants("item").ToList();
                var serItems = serLayout.Descendants("item").ToList();
                
                Console.WriteLine($"  Items: 原始={origItems.Count}, 序列化={serItems.Count}");
            }
            
            // 比较结构
            var report = XmlTestUtils.CompareXmlStructure(originalXml, serializedXml);
            Console.WriteLine($"结构相等: {report.IsStructurallyEqual}");
            
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
                Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
                Console.WriteLine($"缺失节点: {string.Join(", ", report.MissingNodes)}");
                Console.WriteLine($"多余节点: {string.Join(", ", report.ExtraNodes)}");
                Console.WriteLine($"缺失属性: {string.Join(", ", report.MissingAttributes)}");
                Console.WriteLine($"多余属性: {string.Join(", ", report.ExtraAttributes)}");
                Console.WriteLine($"属性值差异: {string.Join(", ", report.AttributeValueDifferences)}");
            }
            
            // 保存调试文件
            string debugPath = Path.Combine("Debug", "flora_kinds_layout_debug");
            Directory.CreateDirectory(debugPath);
            File.WriteAllText(Path.Combine(debugPath, "original.xml"), originalXml);
            File.WriteAllText(Path.Combine(debugPath, "serialized.xml"), serializedXml);
            
            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }
    }
}