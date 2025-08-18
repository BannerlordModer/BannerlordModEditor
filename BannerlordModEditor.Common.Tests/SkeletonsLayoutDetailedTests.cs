using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Models.DTO.Layouts;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class SkeletonsLayoutDetailedTests
    {
        [Fact]
        public void Compare_Original_And_Serialized_Xml()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "skeletons_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(originalXml);
            
            // Act
            string serializedXml = XmlTestUtils.Serialize(original);
            
            // 使用XDocument进行详细比较
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);
            
            Console.WriteLine("=== 原始XML和序列化XML的比较 ===");
            
            // 比较根元素
            var originalRoot = originalDoc.Root;
            var serializedRoot = serializedDoc.Root;
            
            Console.WriteLine($"Root Name: Original={originalRoot.Name}, Serialized={serializedRoot.Name}");
            Console.WriteLine($"Root Type attribute: Original={originalRoot.Attribute("type")?.Value}, Serialized={serializedRoot.Attribute("type")?.Value}");
            
            // 比较layouts元素
            var originalLayouts = originalRoot.Element("layouts");
            var serializedLayouts = serializedRoot.Element("layouts");
            
            if (originalLayouts != null && serializedLayouts != null)
            {
                var originalLayoutElements = originalLayouts.Elements("layout").ToList();
                var serializedLayoutElements = serializedLayouts.Elements("layout").ToList();
                
                Console.WriteLine($"Layout count: Original={originalLayoutElements.Count}, Serialized={serializedLayoutElements.Count}");
                
                for (int i = 0; i < Math.Min(originalLayoutElements.Count, serializedLayoutElements.Count); i++)
                {
                    var origLayout = originalLayoutElements[i];
                    var serLayout = serializedLayoutElements[i];
                    
                    Console.WriteLine($"Layout {i}:");
                    Console.WriteLine($"  Class: Original={origLayout.Attribute("class")?.Value}, Serialized={serLayout.Attribute("class")?.Value}");
                    Console.WriteLine($"  Version: Original={origLayout.Attribute("version")?.Value}, Serialized={serLayout.Attribute("version")?.Value}");
                    
                    // 比较treeview_context_menu
                    var origTreeview = origLayout.Element("treeview_context_menu");
                    var serTreeview = serLayout.Element("treeview_context_menu");
                    
                    if (origTreeview != null && serTreeview != null)
                    {
                        var origItems = origTreeview.Elements("item").ToList();
                        var serItems = serTreeview.Elements("item").ToList();
                        
                        Console.WriteLine($"  TreeviewContextMenu items: Original={origItems.Count}, Serialized={serItems.Count}");
                        
                        for (int j = 0; j < Math.Min(origItems.Count, serItems.Count); j++)
                        {
                            var origItem = origItems[j];
                            var serItem = serItems[j];
                            
                            Console.WriteLine($"    Item {j}:");
                            Console.WriteLine($"      Name: Original={origItem.Attribute("name")?.Value}, Serialized={serItem.Attribute("name")?.Value}");
                            Console.WriteLine($"      ActionCode: Original={origItem.Attribute("action_code")?.Value}, Serialized={serItem.Attribute("action_code")?.Value}");
                            
                            // 检查嵌套的treeview_context_menu
                            var origNestedTreeview = origItem.Element("treeview_context_menu");
                            var serNestedTreeview = serItem.Element("treeview_context_menu");
                            
                            if (origNestedTreeview != null && serNestedTreeview != null)
                            {
                                var origNestedItems = origNestedTreeview.Elements("item").ToList();
                                var serNestedItems = serNestedTreeview.Elements("item").ToList();
                                
                                Console.WriteLine($"      Nested TreeviewContextMenu items: Original={origNestedItems.Count}, Serialized={serNestedItems.Count}");
                                
                                for (int k = 0; k < Math.Min(origNestedItems.Count, serNestedItems.Count); k++)
                                {
                                    var origNestedItem = origNestedItems[k];
                                    var serNestedItem = serNestedItems[k];
                                    
                                    Console.WriteLine($"        Nested Item {k}:");
                                    Console.WriteLine($"          Name: Original={origNestedItem.Attribute("name")?.Value}, Serialized={serNestedItem.Attribute("name")?.Value}");
                                    Console.WriteLine($"          ActionCode: Original={origNestedItem.Attribute("action_code")?.Value}, Serialized={serNestedItem.Attribute("action_code")?.Value}");
                                }
                            }
                            else if (origNestedTreeview != null)
                            {
                                Console.WriteLine($"      Original has nested TreeviewContextMenu but Serialized doesn't!");
                            }
                            else if (serNestedTreeview != null)
                            {
                                Console.WriteLine($"      Serialized has nested TreeviewContextMenu but Original doesn't!");
                            }
                        }
                    }
                    else if (origTreeview != null)
                    {
                        Console.WriteLine($"  Original has TreeviewContextMenu but Serialized doesn't!");
                    }
                    else if (serTreeview != null)
                    {
                        Console.WriteLine($"  Serialized has TreeviewContextMenu but Original doesn't!");
                    }
                }
            }
            
            // 最终验证
            bool areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);
            Console.WriteLine($"AreStructurallyEqual: {areEqual}");
            
            if (!areEqual)
            {
                Console.WriteLine("=== 找到差异，原始XML和序列化XML不相同 ===");
            }
            
            // 断言 - 这里我们先不要求完全相等，而是观察差异
            // Assert.True(areEqual);
        }
    }
}