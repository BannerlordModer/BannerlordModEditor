using System;
using System.IO;
using System.Xml;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemHolstersLayoutSimpleDebugTest
    {
        [Fact]
        public void Simple_Debug_ItemHolstersLayout_XmlComparison()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            Console.WriteLine($"=== 原始XML长度: {originalXml.Length} ===");

            var original = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(originalXml);
            Console.WriteLine($"反序列化成功！Type: {original.Type}, HasLayouts: {original.HasLayouts}");

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);
            Console.WriteLine($"序列化完成！序列化XML长度: {serializedXml.Length}");

            // 简单比较
            bool areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            Console.WriteLine($"AreStructurallyEqual: {areEqual}");

            // 保存文件以便手动检查
            string debugDir = Path.Combine("BannerlordModEditor.Common.Tests", "bin", "Debug", "net9.0");
            Directory.CreateDirectory(debugDir);
            
            File.WriteAllText(Path.Combine(debugDir, "item_holsters_original.xml"), originalXml);
            File.WriteAllText(Path.Combine(debugDir, "item_holsters_serialized.xml"), serializedXml);
            Console.WriteLine("已保存原始和序列化XML文件以便分析");

            // 手动检查一些关键差异
            var originalDoc = System.Xml.Linq.XDocument.Parse(originalXml);
            var serializedDoc = System.Xml.Linq.XDocument.Parse(serializedXml);

            // 检查属性数量
            var originalAttrs = originalDoc.Root.Attributes().Count();
            var serializedAttrs = serializedDoc.Root.Attributes().Count();
            Console.WriteLine($"Root属性数量: Original={originalAttrs}, Serialized={serializedAttrs}");

            // 检查layout数量
            var originalLayouts = originalDoc.Root.Element("layouts")?.Elements("layout").Count() ?? 0;
            var serializedLayouts = serializedDoc.Root.Element("layouts")?.Elements("layout").Count() ?? 0;
            Console.WriteLine($"Layout数量: Original={originalLayouts}, Serialized={serializedLayouts}");

            // 检查第一个layout的items数量
            var firstOriginalItems = originalDoc.Root.Element("layouts")?.Element("layout")?.Element("items")?.Elements("item").Count() ?? 0;
            var firstSerializedItems = serializedDoc.Root.Element("layouts")?.Element("layout")?.Element("items")?.Elements("item").Count() ?? 0;
            Console.WriteLine($"第一个Layout的Items数量: Original={firstOriginalItems}, Serialized={firstSerializedItems}");

            // 检查所有item的properties
            var originalItems = originalDoc.Root.Element("layouts")?.Element("layout")?.Element("items")?.Elements("item").ToList();
            var serializedItems = serializedDoc.Root.Element("layouts")?.Element("layout")?.Element("items")?.Elements("item").ToList();

            if (originalItems != null && serializedItems != null && originalItems.Count == serializedItems.Count)
            {
                for (int i = 0; i < originalItems.Count; i++)
                {
                    var originalItem = originalItems[i];
                    var serializedItem = serializedItems[i];
                    
                    var originalProps = originalItem.Element("properties")?.Elements("property").ToList() ?? new System.Collections.Generic.List<System.Xml.Linq.XElement>();
                    var serializedProps = serializedItem.Element("properties")?.Elements("property").ToList() ?? new System.Collections.Generic.List<System.Xml.Linq.XElement>();
                    
                    if (originalProps.Count != serializedProps.Count)
                    {
                        var itemName = originalItem.Attribute("name")?.Value ?? "unknown";
                        Console.WriteLine($"Item {i} ({itemName}): Properties count mismatch - Original={originalProps.Count}, Serialized={serializedProps.Count}");
                    }
                }
            }

            // Assert
            Assert.True(areEqual);
        }
    }
}