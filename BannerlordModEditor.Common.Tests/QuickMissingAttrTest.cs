using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class QuickMissingAttrTest
    {
        [Fact]
        public void Quick_Find_Missing_Attribute()
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
            
            Console.WriteLine($"Original elements: {originalElements.Count}, Serialized elements: {serializedElements.Count}");
            
            // 逐个比较元素
            for (int i = 0; i < Math.Min(originalElements.Count, serializedElements.Count); i++)
            {
                var origElem = originalElements[i];
                var serElem = serializedElements[i];
                
                var origAttrs = origElem.Attributes().ToList();
                var serAttrs = serElem.Attributes().ToList();
                
                if (origAttrs.Count != serAttrs.Count)
                {
                    Console.WriteLine($"Element {i}: {origElem.Name.LocalName}");
                    Console.WriteLine($"  Original attrs: {origAttrs.Count}, Serialized attrs: {serAttrs.Count}");
                    
                    var origAttrNames = origAttrs.Select(a => a.Name.LocalName).ToList();
                    var serAttrNames = serAttrs.Select(a => a.Name.LocalName).ToList();
                    
                    var missing = origAttrNames.Except(serAttrNames).ToList();
                    var extra = serAttrNames.Except(origAttrNames).ToList();
                    
                    if (missing.Any())
                    {
                        Console.WriteLine($"  Missing: {string.Join(", ", missing)}");
                    }
                    if (extra.Any())
                    {
                        Console.WriteLine($"  Extra: {string.Join(", ", extra)}");
                    }
                }
            }
            
            // 简单检查属性总数
            int totalOriginal = originalElements.Sum(e => e.Attributes().Count());
            int totalSerialized = serializedElements.Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"Total original attributes: {totalOriginal}");
            Console.WriteLine($"Total serialized attributes: {totalSerialized}");
            Console.WriteLine($"Difference: {totalOriginal - totalSerialized}");
            
            Assert.Equal(totalOriginal, totalSerialized);
        }
    }
}