using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

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
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 将原始和序列化的XML写入文件以便分析
            File.WriteAllText("original_looknfeel.xml", xml);
            File.WriteAllText("serialized_looknfeel.xml", xml2);
            Console.WriteLine("原始XML已写入: original_looknfeel.xml");
            Console.WriteLine("序列化XML已写入: serialized_looknfeel.xml");
            
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
            
            // 详细分析属性差异
            Console.WriteLine("\n=== 详细属性差异分析 ===");
            
            // 分析原始XML中的属性分布
            var originalElementsByType = originalElements.GroupBy(e => e.Name.LocalName).ToDictionary(g => g.Key, g => g.ToList());
            var serializedElementsByType = serializedElements.GroupBy(e => e.Name.LocalName).ToDictionary(g => g.Key, g => g.ToList());
            
            foreach (var elemType in originalElementsByType.Keys)
            {
                var origTypeElements = originalElementsByType[elemType];
                var serTypeElements = serializedElementsByType.ContainsKey(elemType) ? serializedElementsByType[elemType] : new List<XElement>();
                
                var origTypeAttrCount = origTypeElements.Sum(e => e.Attributes().Count());
                var serTypeAttrCount = serTypeElements.Sum(e => e.Attributes().Count());
                
                if (origTypeAttrCount != serTypeAttrCount)
                {
                    Console.WriteLine($"{elemType}: 原始 {origTypeAttrCount} 属性, 序列化 {serTypeAttrCount} 属性, 差异 {origTypeAttrCount - serTypeAttrCount}");
                    
                    // 详细分析每个元素的属性
                    for (int i = 0; i < Math.Min(origTypeElements.Count, serTypeElements.Count); i++)
                    {
                        var origElem = origTypeElements[i];
                        var serElem = serTypeElements[i];
                        
                        var origAttrs = origElem.Attributes().ToList();
                        var serAttrs = serElem.Attributes().ToList();
                        
                        if (origAttrs.Count != serAttrs.Count)
                        {
                            Console.WriteLine($"  {elemType}[{i}]: 原始 {origAttrs.Count}, 序列化 {serAttrs.Count}");
                            
                            var origAttrNames = origAttrs.Select(a => a.Name.LocalName).ToList();
                            var serAttrNames = serAttrs.Select(a => a.Name.LocalName).ToList();
                            
                            var missing = origAttrNames.Except(serAttrNames).ToList();
                            var extra = serAttrNames.Except(origAttrNames).ToList();
                            
                            if (missing.Any())
                            {
                                Console.WriteLine($"    缺失: {string.Join(", ", missing)}");
                            }
                            if (extra.Any())
                            {
                                Console.WriteLine($"    多余: {string.Join(", ", extra)}");
                            }
                        }
                    }
                }
            }
            
            Assert.Equal(totalOriginal, totalSerialized);
        }

        [Fact]
        public void Test_HorizontalAligment_Mapping()
        {
            // 测试XML中的错误拼写horizontal_aligment是否能正确映射到horizontal_alignment
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <widgets>
        <widget name=""test"" horizontal_aligment=""center"" />
    </widgets>
</base>";

            // 反序列化
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(testXml);
            
            // 验证映射是否正确
            Assert.Equal("center", obj.Widgets.WidgetList[0].HorizontalAlignment);
            
            // 序列化
            var serialized = XmlTestUtils.Serialize(obj, testXml);
            
            // 验证序列化结果中不包含错误拼写
            Assert.DoesNotContain("horizontal_aligment", serialized);
            
            // 验证序列化结果中包含正确拼写
            Assert.Contains("horizontal_alignment", serialized);
            
            Console.WriteLine("✅ horizontal_aligment映射测试通过！");
        }
    }
}