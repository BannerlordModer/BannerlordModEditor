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
    public class SkeletonsLayoutMinimalTests
    {
        [Fact]
        public void Test_XmlTestUtils_Special_Handling()
        {
            // Arrange - 创建一个简单的XML字符串，包含嵌套的treeview_context_menu
            string testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/1/XMLSchema""
    type=""string"">
    <layouts>
        <layout
            class=""hinge_joint""
            version=""0.1""
            xml_tag=""skeletons.hinge_joint""
            name_attribute=""name""
            use_in_treeview=""true"">
            <treeview_context_menu>
                <item
                    name=""Delete""
                    action_code=""delete_hinge"" />
                <item
                    name=""Change type"">
                    <treeview_context_menu>
                        <item
                            name=""D6 joint""
                            action_code=""convert_hinge_to_d6"" />
                    </treeview_context_menu>
                </item>
            </treeview_context_menu>
        </layout>
    </layouts>
</base>";

            // Act - 反序列化并让XmlTestUtils进行特殊处理
            var result = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(testXml);

            // Assert - 验证嵌套结构是否正确处理
            Assert.NotNull(result);
            Assert.True(result.HasLayouts);
            Assert.Single(result.Layouts.LayoutList);

            var layout = result.Layouts.LayoutList[0];
            Assert.True(layout.HasTreeviewContextMenu);
            Assert.NotNull(layout.TreeviewContextMenu);
            Assert.Equal(2, layout.TreeviewContextMenu.ItemList.Count);

            var item1 = layout.TreeviewContextMenu.ItemList[0];
            var item2 = layout.TreeviewContextMenu.ItemList[1];

            Assert.Equal("Delete", item1.Name);
            Assert.Equal("delete_hinge", item1.ActionCode);
            Assert.False(item1.HasTreeviewContextMenu); // 第一个项没有嵌套菜单

            Assert.Equal("Change type", item2.Name);
            Assert.True(item2.HasTreeviewContextMenu); // 第二个项有嵌套菜单
            Assert.NotNull(item2.TreeviewContextMenu);
            Assert.Single(item2.TreeviewContextMenu.ItemList);

            var nestedItem = item2.TreeviewContextMenu.ItemList[0];
            Assert.Equal("D6 joint", nestedItem.Name);
            Assert.Equal("convert_hinge_to_d6", nestedItem.ActionCode);
            Assert.False(nestedItem.HasTreeviewContextMenu);

            // 现在序列化回来
            string serializedXml = XmlTestUtils.Serialize(result);
            
            Console.WriteLine("=== Serialized XML ===");
            Console.WriteLine(serializedXml);

            // 比较是否结构相等
            bool areEqual = XmlTestUtils.AreStructurallyEqual(testXml, serializedXml);
            Console.WriteLine($"AreStructurallyEqual: {areEqual}");

            // 如果不相等，显示详细差异
            if (!areEqual)
            {
                Console.WriteLine("=== 详细差异分析 ===");
                
                var originalDoc = XDocument.Parse(testXml);
                var serializedDoc = XDocument.Parse(serializedXml);
                
                var originalItems = originalDoc.Descendants("item").ToList();
                var serializedItems = serializedDoc.Descendants("item").ToList();
                
                Console.WriteLine($"Original items: {originalItems.Count}, Serialized items: {serializedItems.Count}");
                
                for (int i = 0; i < Math.Min(originalItems.Count, serializedItems.Count); i++)
                {
                    var origItem = originalItems[i];
                    var serItem = serializedItems[i];
                    
                    var origName = origItem.Attribute("name")?.Value;
                    var serName = serItem.Attribute("name")?.Value;
                    
                    var origNested = origItem.Element("treeview_context_menu");
                    var serNested = serItem.Element("treeview_context_menu");
                    
                    Console.WriteLine($"Item {i}: {origName} vs {serName}, HasNested: {origNested != null} vs {serNested != null}");
                    
                    // 比较属性
                    var origAttrs = origItem.Attributes().ToList();
                    var serAttrs = serItem.Attributes().ToList();
                    
                    Console.WriteLine($"  Attributes: {origAttrs.Count} vs {serAttrs.Count}");
                    
                    foreach (var origAttr in origAttrs)
                    {
                        var serAttr = serAttrs.FirstOrDefault(a => a.Name == origAttr.Name);
                        if (serAttr == null)
                        {
                            Console.WriteLine($"    Missing attribute: {origAttr.Name}={origAttr.Value}");
                        }
                        else if (origAttr.Value != serAttr.Value)
                        {
                            Console.WriteLine($"    Attribute difference: {origAttr.Name}={origAttr.Value} vs {serAttr.Value}");
                        }
                    }
                    
                    foreach (var serAttr in serAttrs)
                    {
                        if (!origAttrs.Any(a => a.Name == serAttr.Name))
                        {
                            Console.WriteLine($"    Extra attribute: {serAttr.Name}={serAttr.Value}");
                        }
                    }
                }
                
                // 使用XmlTestUtils的详细比较功能
                var diffReport = XmlTestUtils.CompareXmlStructure(testXml, serializedXml);
                Console.WriteLine("=== XmlTestUtils 差异报告 ===");
                Console.WriteLine($"MissingNodes: {string.Join(", ", diffReport.MissingNodes)}");
                Console.WriteLine($"ExtraNodes: {string.Join(", ", diffReport.ExtraNodes)}");
                Console.WriteLine($"MissingAttributes: {string.Join(", ", diffReport.MissingAttributes)}");
                Console.WriteLine($"ExtraAttributes: {string.Join(", ", diffReport.ExtraAttributes)}");
                Console.WriteLine($"AttributeValueDifferences: {string.Join(", ", diffReport.AttributeValueDifferences)}");
                Console.WriteLine($"NodeCountDifference: {diffReport.NodeCountDifference}");
                Console.WriteLine($"AttributeCountDifference: {diffReport.AttributeCountDifference}");
            }

            // 断言
            Assert.True(areEqual);
        }
    }
}