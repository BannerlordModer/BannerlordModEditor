using System.IO;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDebugTest
    {
        [Fact]
        public void FloraKinds_DetailedAnalysis()
        {
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 1. 测试反序列化
            var obj = XmlTestUtils.Deserialize<FloraKinds>(xml);
            Assert.NotNull(obj);
            
            // 2. 测试序列化
            var xml2 = XmlTestUtils.Serialize(obj);
            Assert.NotNull(xml2);
            
            // 3. 比较文件大小
            Console.WriteLine($"原始XML大小: {xml.Length} 字符");
            Console.WriteLine($"序列化XML大小: {xml2.Length} 字符");
            
            // 4. 解析XML文档进行比较
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            // 5. 比较根节点
            var root1 = doc1.Root;
            var root2 = doc2.Root;
            
            Console.WriteLine($"原始根节点名称: {root1.Name}");
            Console.WriteLine($"序列化根节点名称: {root2.Name}");
            
            // 6. 比较flora_kind节点数量
            var floraKinds1 = root1.Elements("flora_kind").ToList();
            var floraKinds2 = root2.Elements("flora_kind").ToList();
            
            Console.WriteLine($"原始flora_kind节点数量: {floraKinds1.Count}");
            Console.WriteLine($"序列化flora_kind节点数量: {floraKinds2.Count}");
            
            // 7. 详细比较第一个flora_kind节点
            if (floraKinds1.Count > 0 && floraKinds2.Count > 0)
            {
                var first1 = floraKinds1.First();
                var first2 = floraKinds2.First();
                
                Console.WriteLine("\n第一个flora_kind节点比较:");
                Console.WriteLine($"原始属性数量: {first1.Attributes().Count()}");
                Console.WriteLine($"序列化属性数量: {first2.Attributes().Count()}");
                
                foreach (var attr in first1.Attributes())
                {
                    var attr2 = first2.Attribute(attr.Name);
                    if (attr2 == null)
                    {
                        Console.WriteLine($"缺失属性: {attr.Name}={attr.Value}");
                    }
                    else if (attr.Value != attr2.Value)
                    {
                        Console.WriteLine($"属性值不同: {attr.Name} - 原始: {attr.Value}, 序列化: {attr2.Value}");
                    }
                }
                
                // 比较子元素
                var children1 = first1.Elements().ToList();
                var children2 = first2.Elements().ToList();
                
                Console.WriteLine($"原始子元素数量: {children1.Count}");
                Console.WriteLine($"序列化子元素数量: {children2.Count}");
                
                foreach (var child in children1)
                {
                    var child2 = children2.FirstOrDefault(c => c.Name == child.Name);
                    if (child2 == null)
                    {
                        Console.WriteLine($"缺失子元素: {child.Name}");
                    }
                }
            }
            
            // 8. 尝试找到具体的差异
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Console.WriteLine($"\n差异报告:");
            Console.WriteLine($"IsStructurallyEqual: {report.IsStructurallyEqual}");
            Console.WriteLine($"MissingNodes: {string.Join(", ", report.MissingNodes)}");
            Console.WriteLine($"ExtraNodes: {string.Join(", ", report.ExtraNodes)}");
            Console.WriteLine($"NodeNameDifferences: {string.Join(", ", report.NodeNameDifferences)}");
            Console.WriteLine($"MissingAttributes: {string.Join(", ", report.MissingAttributes)}");
            Console.WriteLine($"ExtraAttributes: {string.Join(", ", report.ExtraAttributes)}");
            Console.WriteLine($"AttributeValueDifferences: {string.Join(", ", report.AttributeValueDifferences)}");
            Console.WriteLine($"TextDifferences: {string.Join(", ", report.TextDifferences)}");
            Console.WriteLine($"NodeCountDifference: {report.NodeCountDifference}");
            Console.WriteLine($"AttributeCountDifference: {report.AttributeCountDifference}");
            
            // 9. 检查是否有空元素处理问题
            var originalDoc = XDocument.Parse(xml);
            var serializedDoc = XDocument.Parse(xml2);
            
            var emptyElementsInOriginal = originalDoc.Descendants()
                .Where(d => !d.HasElements && !d.HasAttributes && !d.Value.Any())
                .ToList();
            
            var emptyElementsInSerialized = serializedDoc.Descendants()
                .Where(d => !d.HasElements && !d.HasAttributes && !d.Value.Any())
                .ToList();
            
            Console.WriteLine($"\n原始XML中空元素数量: {emptyElementsInOriginal.Count}");
            Console.WriteLine($"序列化XML中空元素数量: {emptyElementsInSerialized.Count}");
            
            // 10. 保存序列化结果到文件以便手动检查
            File.WriteAllText("flora_kinds_serialized.xml", xml2);
            Console.WriteLine("序列化结果已保存到 flora_kinds_serialized.xml");
        }
    }
}