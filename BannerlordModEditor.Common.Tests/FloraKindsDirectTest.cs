using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDirectTest
    {
        [Fact]
        public void FloraKinds_DirectAnalysis()
        {
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"=== FloraKinds 分析开始 ===");
            Console.WriteLine($"XML文件大小: {xml.Length} 字符");
            
            // 1. 测试反序列化
            var obj = XmlTestUtils.Deserialize<FloraKinds>(xml);
            Console.WriteLine($"反序列化成功: {obj != null}");
            Console.WriteLine($"FloraKind数量: {obj.FloraKindList.Count}");
            
            // 2. 测试序列化
            var xml2 = XmlTestUtils.Serialize(obj);
            Console.WriteLine($"序列化成功: {xml2 != null}");
            Console.WriteLine($"序列化XML大小: {xml2.Length} 字符");
            
            // 3. 结构比较
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Console.WriteLine($"结构相等: {report.IsStructurallyEqual}");
            Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
            Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
            
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine($"缺失节点数量: {report.MissingNodes.Count}");
                Console.WriteLine($"多余节点数量: {report.ExtraNodes.Count}");
                Console.WriteLine($"节点名差异数量: {report.NodeNameDifferences.Count}");
                Console.WriteLine($"缺失属性数量: {report.MissingAttributes.Count}");
                Console.WriteLine($"多余属性数量: {report.ExtraAttributes.Count}");
                Console.WriteLine($"属性值差异数量: {report.AttributeValueDifferences.Count}");
                Console.WriteLine($"文本差异数量: {report.TextDifferences.Count}");
                
                // 显示一些具体的差异
                if (report.AttributeValueDifferences.Count > 0)
                {
                    Console.WriteLine("\n=== 属性值差异示例 ===");
                    for (int i = 0; i < Math.Min(5, report.AttributeValueDifferences.Count); i++)
                    {
                        Console.WriteLine($"  {report.AttributeValueDifferences[i]}");
                    }
                }
                
                if (report.MissingAttributes.Count > 0)
                {
                    Console.WriteLine("\n=== 缺失属性示例 ===");
                    for (int i = 0; i < Math.Min(5, report.MissingAttributes.Count); i++)
                    {
                        Console.WriteLine($"  {report.MissingAttributes[i]}");
                    }
                }
                
                // 分析第一个flora_kind的详细差异
                Console.WriteLine("\n=== 第一个FloraKind详细分析 ===");
                var doc1 = XDocument.Parse(xml);
                var doc2 = XDocument.Parse(xml2);
                
                var firstFk1 = doc1.Root.Element("flora_kind");
                var firstFk2 = doc2.Root.Element("flora_kind");
                
                if (firstFk1 != null && firstFk2 != null)
                {
                    Console.WriteLine($"原始属性数量: {firstFk1.Attributes().Count()}");
                    Console.WriteLine($"序列化属性数量: {firstFk2.Attributes().Count()}");
                    
                    foreach (var attr in firstFk1.Attributes())
                    {
                        var attr2 = firstFk2.Attribute(attr.Name);
                        if (attr2 == null)
                        {
                            Console.WriteLine($"  缺失属性: {attr.Name}={attr.Value}");
                        }
                        else if (attr.Value != attr2.Value)
                        {
                            Console.WriteLine($"  属性值不同: {attr.Name} - 原始: {attr.Value}, 序列化: {attr2.Value}");
                        }
                    }
                }
            }
            
            Console.WriteLine($"=== FloraKinds 分析结束 ===");
            
            // 这个测试只用来分析，不验证结果
            Assert.True(true);
        }
    }
}