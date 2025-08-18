using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Tests
{
    public class SkeletonsLayoutDebugTest
    {
        [Fact]
        public void Debug_SkeletonsLayout_XmlDifferences()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "Layouts", "skeletons_layout.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<SkeletonsLayoutDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // 解析两个XML文档
            var doc1 = XDocument.Parse(originalXml);
            var doc2 = XDocument.Parse(serializedXml);
            
            // 获取所有属性
            var attributes1 = GetAllAttributes(doc1);
            var attributes2 = GetAllAttributes(doc2);
            
            Console.WriteLine($"原始XML属性数量: {attributes1.Count}");
            Console.WriteLine($"序列化XML属性数量: {attributes2.Count}");
            
            // 找出差异
            var differences = attributes1.Except(attributes2).ToList();
            var extraAttributes = attributes2.Except(attributes1).ToList();
            
            Console.WriteLine($"缺失的属性数量: {differences.Count}");
            Console.WriteLine($"多余的属性数量: {extraAttributes.Count}");
            
            if (differences.Count > 0)
            {
                Console.WriteLine("缺失的属性:");
                foreach (var attr in differences.Take(10))
                {
                    Console.WriteLine($"  {attr}");
                }
            }
            
            if (extraAttributes.Count > 0)
            {
                Console.WriteLine("多余的属性:");
                foreach (var attr in extraAttributes.Take(10))
                {
                    Console.WriteLine($"  {attr}");
                }
            }
            
            // 检查特定问题
            var layouts1 = doc1.Root?.Element("layouts")?.Elements("layout").ToList();
            var layouts2 = doc2.Root?.Element("layouts")?.Elements("layout").ToList();
            
            Console.WriteLine($"原始Layout数量: {layouts1?.Count ?? 0}");
            Console.WriteLine($"序列化Layout数量: {layouts2?.Count ?? 0}");
            
            // 检查每个Layout的TreeviewContextMenu
            for (int i = 0; i < Math.Min(layouts1?.Count ?? 0, layouts2?.Count ?? 0); i++)
            {
                var layout1 = layouts1![i];
                var layout2 = layouts2![i];
                
                var class1 = layout1.Attribute("class")?.Value;
                var class2 = layout2.Attribute("class")?.Value;
                
                if (class1 != class2)
                {
                    Console.WriteLine($"Layout类不匹配: {class1} != {class2}");
                    continue;
                }
                
                var menu1 = layout1.Element("treeview_context_menu")?.Elements("item").ToList();
                var menu2 = layout2.Element("treeview_context_menu")?.Elements("item").ToList();
                
                if (menu1?.Count != menu2?.Count)
                {
                    Console.WriteLine($"Layout {class1} 的菜单项数量不匹配: {menu1?.Count ?? 0} != {menu2?.Count ?? 0}");
                    continue;
                }
                
                // 检查每个菜单项
                for (int j = 0; j < (menu1?.Count ?? 0); j++)
                {
                    var item1 = menu1![j];
                    var item2 = menu2![j];
                    
                    var name1 = item1.Attribute("name")?.Value;
                    var name2 = item2.Attribute("name")?.Value;
                    
                    var action1 = item1.Attribute("action_code")?.Value;
                    var action2 = item2.Attribute("action_code")?.Value;
                    
                    if (name1 != name2)
                    {
                        Console.WriteLine($"菜单项名称不匹配: {name1} != {name2}");
                    }
                    
                    if (action1 != action2)
                    {
                        Console.WriteLine($"菜单项ActionCode不匹配: {name1} - {action1} != {action2}");
                    }
                }
            }
            
            // 保存差异文件以便分析
            File.WriteAllText("original_skeletons.xml", originalXml);
            File.WriteAllText("serialized_skeletons.xml", serializedXml);
            
            Console.WriteLine("已保存原始和序列化XML文件以便分析");
        }
        
        private List<string> GetAllAttributes(XDocument doc)
        {
            var attributes = new List<string>();
            
            foreach (var element in doc.Descendants())
            {
                foreach (var attribute in element.Attributes())
                {
                    attributes.Add($"{element.Name}.{attribute.Name}={attribute.Value}");
                }
            }
            
            return attributes;
        }
    }
}