using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class TargetedMissingAttrTest
    {
        [Fact]
        public void Targeted_Find_Missing_Attribute()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<Looknfeel>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析差异
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            // 按元素路径组织属性
            var originalElements = doc1.Descendants().ToList();
            var serializedElements = doc2.Descendants().ToList();
            
            // 查找有差异的元素
            for (int i = 0; i < Math.Min(originalElements.Count, serializedElements.Count); i++)
            {
                var origElem = originalElements[i];
                var serElem = serializedElements[i];
                
                var origAttrs = origElem.Attributes().ToList();
                var serAttrs = serElem.Attributes().ToList();
                
                if (origAttrs.Count != serAttrs.Count)
                {
                    Console.WriteLine($"Found difference at element {i}");
                    Console.WriteLine($"Element: {origElem.Name.LocalName}");
                    Console.WriteLine($"Original attrs count: {origAttrs.Count}");
                    Console.WriteLine($"Serialized attrs count: {serAttrs.Count}");
                    
                    // 显示具体属性
                    Console.WriteLine("Original attributes:");
                    foreach (var attr in origAttrs)
                    {
                        Console.WriteLine($"  {attr.Name.LocalName}={attr.Value}");
                    }
                    
                    Console.WriteLine("Serialized attributes:");
                    foreach (var attr in serAttrs)
                    {
                        Console.WriteLine($"  {attr.Name.LocalName}={attr.Value}");
                    }
                    
                    // 找出缺失的属性
                    var origAttrNames = origAttrs.Select(a => a.Name.LocalName).ToHashSet();
                    var serAttrNames = serAttrs.Select(a => a.Name.LocalName).ToHashSet();
                    
                    var missing = origAttrNames.Except(serAttrNames).ToList();
                    var extra = serAttrNames.Except(origAttrNames).ToList();
                    
                    if (missing.Any())
                    {
                        Console.WriteLine($"Missing attributes: {string.Join(", ", missing)}");
                    }
                    
                    if (extra.Any())
                    {
                        Console.WriteLine($"Extra attributes: {string.Join(", ", extra)}");
                    }
                    
                    break; // 找到第一个就停止
                }
            }
            
            // 检查总数
            int totalOriginal = originalElements.Sum(e => e.Attributes().Count());
            int totalSerialized = serializedElements.Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"Total difference: {totalOriginal - totalSerialized}");
            
            // 暂时不断言，先看看结果
            if (totalOriginal != totalSerialized)
            {
                Console.WriteLine("WARNING: Attribute count mismatch detected!");
            }
        }
    }
}