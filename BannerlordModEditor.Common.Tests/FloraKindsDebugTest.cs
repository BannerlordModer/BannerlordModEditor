using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDebugTest
    {
        [Fact]
        public void RunDebugAnalysis()
        {
            Console.WriteLine("=== FloraKinds 调试分析 ===");
            
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            var obj = XmlTestUtils.Deserialize<FloraKindsDO>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);
            
            Console.WriteLine($"原始XML大小: {xml.Length} 字符");
            Console.WriteLine($"序列化XML大小: {xml2.Length} 字符");
            Console.WriteLine($"FloraKind数量: {obj.FloraKindsList.Count}");
            
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
            File.WriteAllText("flora_kinds_original.xml", xml);
            File.WriteAllText("flora_kinds_serialized.xml", xml2);
            
            // 尝试找到具体的差异
            var originalDoc = XDocument.Parse(xml);
            var serializedDoc = XDocument.Parse(xml2);
            
            // 比较根元素
            var originalRoot = originalDoc.Root;
            var serializedRoot = serializedDoc.Root;
            
            Console.WriteLine($"原始根元素属性数量: {originalRoot.Attributes().Count()}");
            Console.WriteLine($"序列化根元素属性数量: {serializedRoot.Attributes().Count()}");
            
            // 比较flora_kind元素
            var originalFloraKinds = originalRoot.Elements("flora_kind").ToList();
            var serializedFloraKinds = serializedRoot.Elements("flora_kind").ToList();
            
            Console.WriteLine($"原始flora_kind元素数量: {originalFloraKinds.Count}");
            Console.WriteLine($"序列化flora_kind元素数量: {serializedFloraKinds.Count}");
            
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
            for (int i = 0; i < Math.Min(originalFloraKinds.Count, serializedFloraKinds.Count); i++)
            {
                var original = originalFloraKinds[i];
                var serialized = serializedFloraKinds[i];
                
                var originalAttrCountInElement = original.Attributes().Count();
                var serializedAttrCountInElement = serialized.Attributes().Count();
                
                if (originalAttrCountInElement != serializedAttrCountInElement)
                {
                    Console.WriteLine($"发现差异元素 {i}:");
                    Console.WriteLine($"  原始属性数量: {originalAttrCountInElement}");
                    Console.WriteLine($"  序列化属性数量: {serializedAttrCountInElement}");
                    Console.WriteLine($"  元素名称: {original.Attribute("name")?.Value}");
                    
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
    }
}