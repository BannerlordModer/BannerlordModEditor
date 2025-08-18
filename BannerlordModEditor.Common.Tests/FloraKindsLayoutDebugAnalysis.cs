using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsLayoutDebugAnalysis
    {
        [Fact]
        public void Debug_FloraKindsLayout_XmlComparison()
        {
            Console.WriteLine("=== FloraKindsLayout 调试分析 ===");
            
            var xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            var xml = File.ReadAllText(xmlPath);
            var obj = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"原始XML大小: {xml.Length} 字符");
            Console.WriteLine($"序列化XML大小: {xml2.Length} 字符");
            Console.WriteLine($"Layout数量: {obj.Layouts.LayoutList.Count}");
            
            // 详细分析差异
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
            Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
            
            if (report.AttributeValueDifferences.Count > 0)
            {
                Console.WriteLine($"属性值差异数量: {report.AttributeValueDifferences.Count}");
                foreach (var diff in report.AttributeValueDifferences)
                {
                    Console.WriteLine($"  - {diff}");
                }
            }
            
            // 保存原始和序列化的XML进行对比
            File.WriteAllText("flora_kinds_layout_original.xml", xml);
            File.WriteAllText("flora_kinds_layout_serialized.xml", xml2);
            
            // 尝试找到具体的差异
            var originalDoc = XDocument.Parse(xml);
            var serializedDoc = XDocument.Parse(xml2);
            
            // 比较根元素
            var originalRoot = originalDoc.Root;
            var serializedRoot = serializedDoc.Root;
            
            Console.WriteLine($"原始根元素属性数量: {originalRoot.Attributes().Count()}");
            Console.WriteLine($"序列化根元素属性数量: {serializedRoot.Attributes().Count()}");
            
            // 比较layout元素
            var originalLayouts = originalRoot.Element("layouts")?.Elements("layout").ToList();
            var serializedLayouts = serializedRoot.Element("layouts")?.Elements("layout").ToList();
            
            Console.WriteLine($"原始layout元素数量: {originalLayouts?.Count ?? 0}");
            Console.WriteLine($"序列化layout元素数量: {serializedLayouts?.Count ?? 0}");
            
            // 查找属性数量差异
            int originalAttrCount = 0;
            int serializedAttrCount = 0;
            
            foreach (var element in originalDoc.Descendants())
            {
                originalAttrCount += element.Attributes().Count();
            }
            
            foreach (var element in serializedDoc.Descendants())
            {
                serializedAttrCount += element.Attributes().Count();
            }
            
            Console.WriteLine($"原始总属性数量: {originalAttrCount}");
            Console.WriteLine($"序列化总属性数量: {serializedAttrCount}");
            
            // 尝试找到具体的差异元素
            if (originalLayouts != null && serializedLayouts != null)
            {
                for (int i = 0; i < Math.Min(originalLayouts.Count, serializedLayouts.Count); i++)
                {
                    var original = originalLayouts[i];
                    var serialized = serializedLayouts[i];
                    
                    var originalAttrCountInElement = original.Attributes().Count();
                    var serializedAttrCountInElement = serialized.Attributes().Count();
                    
                    if (originalAttrCountInElement != serializedAttrCountInElement)
                    {
                        Console.WriteLine($"发现差异元素 {i}:");
                        Console.WriteLine($"  原始属性数量: {originalAttrCountInElement}");
                        Console.WriteLine($"  序列化属性数量: {serializedAttrCountInElement}");
                        Console.WriteLine($"  元素class: {original.Attribute("class")?.Value}");
                        
                        // 输出具体属性
                        Console.WriteLine("  原始属性:");
                        foreach (var attr in original.Attributes())
                        {
                            Console.WriteLine($"    {attr.Name}=\"{attr.Value}\"");
                        }
                        
                        Console.WriteLine("  序列化属性:");
                        foreach (var attr in serialized.Attributes())
                        {
                            Console.WriteLine($"    {attr.Name}=\"{attr.Value}\"");
                        }
                        
                        break;
                    }
                }
            }
            
            // 比较items元素
            var originalItems = originalDoc.Descendants("item").ToList();
            var serializedItems = serializedDoc.Descendants("item").ToList();
            
            Console.WriteLine($"原始item元素数量: {originalItems.Count}");
            Console.WriteLine($"序列化item元素数量: {serializedItems.Count}");
            
            // 查找具体的item差异
            for (int i = 0; i < Math.Min(originalItems.Count, serializedItems.Count); i++)
            {
                var original = originalItems[i];
                var serialized = serializedItems[i];
                
                var originalAttrCountInItem = original.Attributes().Count();
                var serializedAttrCountInItem = serialized.Attributes().Count();
                
                if (originalAttrCountInItem != serializedAttrCountInItem)
                {
                    Console.WriteLine($"发现差异item {i}:");
                    Console.WriteLine($"  原始属性数量: {originalAttrCountInItem}");
                    Console.WriteLine($"  序列化属性数量: {serializedAttrCountInItem}");
                    Console.WriteLine($"  item名称: {original.Attribute("name")?.Value}");
                    
                    // 输出具体属性
                    Console.WriteLine("  原始属性:");
                    foreach (var attr in original.Attributes())
                    {
                        Console.WriteLine($"    {attr.Name}=\"{attr.Value}\"");
                    }
                    
                    Console.WriteLine("  序列化属性:");
                    foreach (var attr in serialized.Attributes())
                    {
                        Console.WriteLine($"    {attr.Name}=\"{attr.Value}\"");
                    }
                    
                    break;
                }
            }
            
            // 最终断言 - 用于测试是否相等
            bool areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Console.WriteLine($"XML结构相等: {areEqual}");
            
            // 如果不相等，输出更多调试信息
            if (!areEqual)
            {
                Console.WriteLine("=== 详细差异分析 ===");
                
                // 比较每一行的差异
                var originalLines = xml.Split('\n');
                var serializedLines = xml2.Split('\n');
                
                for (int i = 0; i < Math.Min(originalLines.Length, serializedLines.Length); i++)
                {
                    if (originalLines[i] != serializedLines[i])
                    {
                        Console.WriteLine($"行 {i+1} 差异:");
                        Console.WriteLine($"  原始: {originalLines[i]}");
                        Console.WriteLine($"  序列化: {serializedLines[i]}");
                    }
                }
            }
            
            Assert.True(areEqual, "XML结构应该相等");
        }
    }
}