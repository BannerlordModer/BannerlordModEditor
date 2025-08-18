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
    public class ItemHolstersLayoutDebugTest
    {
        [Fact]
        public void Debug_ItemHolstersLayout_XmlDifferences()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "item_holsters_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            Console.WriteLine($"=== 原始XML长度: {originalXml.Length} ===");
            Console.WriteLine($"=== 开始序列化测试 ===");

            var original = XmlTestUtils.Deserialize<ItemHolstersLayoutDO>(originalXml);
            Console.WriteLine($"反序列化成功！Type: {original.Type}, HasLayouts: {original.HasLayouts}");

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);
            Console.WriteLine($"序列化完成！序列化XML长度: {serializedXml.Length}");

            // 比较差异
            var report = XmlTestUtils.CompareXmlStructure(originalXml, serializedXml);
            Console.WriteLine($"=== XML比较结果 ===");
            Console.WriteLine($"是否结构相等: {report.IsStructurallyEqual}");
            Console.WriteLine($"缺失的属性数量: {report.MissingAttributes.Count}");
            Console.WriteLine($"多余的属性数量: {report.ExtraAttributes.Count}");
            Console.WriteLine($"缺失的节点数量: {report.MissingNodes.Count}");
            Console.WriteLine($"多余的节点数量: {report.ExtraNodes.Count}");

            if (report.MissingAttributes.Count > 0)
            {
                Console.WriteLine("前5个缺失的属性:");
                foreach (var attr in report.MissingAttributes.Take(5))
                {
                    Console.WriteLine($"  - {attr}");
                }
            }

            if (report.ExtraAttributes.Count > 0)
            {
                Console.WriteLine("前5个多余的属性:");
                foreach (var attr in report.ExtraAttributes.Take(5))
                {
                    Console.WriteLine($"  - {attr}");
                }
            }

            // 保存调试文件
            string debugDir = Path.Combine("BannerlordModEditor.Common.Tests", "bin", "Debug", "net9.0");
            Directory.CreateDirectory(debugDir);
            
            File.WriteAllText(Path.Combine(debugDir, "item_holsters_layout_original.xml"), originalXml);
            File.WriteAllText(Path.Combine(debugDir, "item_holsters_layout_serialized.xml"), serializedXml);
            Console.WriteLine("已保存原始和序列化XML文件以便分析");

            // 详细分析结构
            Console.WriteLine("=== 原始XML和序列化XML的比较 ===");
            var originalDoc = System.Xml.Linq.XDocument.Parse(originalXml);
            var serializedDoc = System.Xml.Linq.XDocument.Parse(serializedXml);

            Console.WriteLine($"Root Name: Original={originalDoc.Root.Name}, Serialized={serializedDoc.Root.Name}");
            Console.WriteLine($"Root Type attribute: Original={originalDoc.Root.Attribute("type")?.Value}, Serialized={serializedDoc.Root.Attribute("type")?.Value}");

            var originalLayouts = originalDoc.Root.Element("layouts")?.Elements("layout").ToList();
            var serializedLayouts = serializedDoc.Root.Element("layouts")?.Elements("layout").ToList();
            
            Console.WriteLine($"Layout count: Original={originalLayouts?.Count ?? 0}, Serialized={serializedLayouts?.Count ?? 0}");

            if (originalLayouts != null && serializedLayouts != null && originalLayouts.Count == serializedLayouts.Count)
            {
                for (int i = 0; i < originalLayouts.Count; i++)
                {
                    var originalLayout = originalLayouts[i];
                    var serializedLayout = serializedLayouts[i];
                    
                    Console.WriteLine($"Layout {i}:");
                    Console.WriteLine($"  Class: Original={originalLayout.Attribute("class")?.Value}, Serialized={serializedLayout.Attribute("class")?.Value}");
                    Console.WriteLine($"  Version: Original={originalLayout.Attribute("version")?.Value}, Serialized={serializedLayout.Attribute("version")?.Value}");
                    
                    // 比较items
                    var originalItems = originalLayout.Element("items")?.Elements("item").ToList();
                    var serializedItems = serializedLayout.Element("items")?.Elements("item").ToList();
                    
                    Console.WriteLine($"  Items count: Original={originalItems?.Count ?? 0}, Serialized={serializedItems?.Count ?? 0}");
                    
                    if (originalItems != null && serializedItems != null && originalItems.Count == serializedItems.Count)
                    {
                        for (int j = 0; j < originalItems.Count; j++)
                        {
                            var originalItem = originalItems[j];
                            var serializedItem = serializedItems[j];
                            
                            // 检查properties
                            var originalProperties = originalItem.Element("properties");
                            var serializedProperties = serializedItem.Element("properties");
                            
                            if (originalProperties != null && serializedProperties != null)
                            {
                                var originalProps = originalProperties.Elements("property").ToList();
                                var serializedProps = serializedProperties.Elements("property").ToList();
                                
                                if (originalProps.Count != serializedProps.Count)
                                {
                                    Console.WriteLine($"    Item {j} ({originalItem.Attribute("name")?.Value}): Properties count mismatch - Original={originalProps.Count}, Serialized={serializedProps.Count}");
                                    
                                    // 详细显示属性
                                    Console.WriteLine($"      Original properties:");
                                    foreach (var prop in originalProps)
                                    {
                                        Console.WriteLine($"        {prop.Attribute("name")?.Value}={prop.Attribute("value")?.Value}");
                                    }
                                    
                                    Console.WriteLine($"      Serialized properties:");
                                    foreach (var prop in serializedProps)
                                    {
                                        Console.WriteLine($"        {prop.Attribute("name")?.Value}={prop.Attribute("value")?.Value}");
                                    }
                                }
                            }
                            else if (originalProperties != null && serializedProperties == null)
                            {
                                Console.WriteLine($"    Item {j} ({originalItem.Attribute("name")?.Value}): Missing properties in serialized version");
                            }
                            else if (originalProperties == null && serializedProperties != null)
                            {
                                Console.WriteLine($"    Item {j} ({serializedItem.Attribute("name")?.Value}): Extra properties in serialized version");
                            }
                        }
                    }
                }
            }

            // 最后测试是否结构相等
            bool areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            Console.WriteLine($"AreStructurallyEqual: {areEqual}");
            
            // 如果不相等，保存差异详情
            if (!areEqual)
            {
                File.WriteAllText(Path.Combine(debugDir, "item_holsters_layout_differences.txt"), 
                    $"是否结构相等: {report.IsStructurallyEqual}\n" +
                    $"缺失的属性数量: {report.MissingAttributes.Count}\n" +
                    $"多余的属性数量: {report.ExtraAttributes.Count}\n" +
                    $"缺失的节点数量: {report.MissingNodes.Count}\n" +
                    $"多余的节点数量: {report.ExtraNodes.Count}\n\n" +
                    "缺失的属性:\n" + string.Join("\n", report.MissingAttributes.Take(20)) + "\n\n" +
                    "多余的属性:\n" + string.Join("\n", report.ExtraAttributes.Take(20)) + "\n\n" +
                    "缺失的节点:\n" + string.Join("\n", report.MissingNodes.Take(20)) + "\n\n" +
                    "多余的节点:\n" + string.Join("\n", report.ExtraNodes.Take(20)));
            }

            // Assert
            Assert.True(areEqual);
        }
    }
}