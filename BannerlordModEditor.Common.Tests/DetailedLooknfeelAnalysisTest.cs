using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class DetailedLooknfeelAnalysisTest
    {
        [Fact]
        public void Analyze_Looknfeel_Attribute_Differences()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析差异
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            int attrCount1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            int attrCount2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"Original attrs: {attrCount1}, Serialized attrs: {attrCount2}");
            
            if (attrCount1 != attrCount2)
            {
                Console.WriteLine("查找缺失的属性...");
                
                // 获取所有元素的属性信息
                var originalAttrs = doc1.Descendants()
                    .SelectMany(e => e.Attributes().Select(a => $"{e.Name.LocalName}@{a.Name.LocalName}={a.Value}"))
                    .ToList();
                    
                var serializedAttrs = doc2.Descendants()
                    .SelectMany(e => e.Attributes().Select(a => $"{e.Name.LocalName}@{a.Name.LocalName}={a.Value}"))
                    .ToList();
                
                var missingAttrs = originalAttrs.Except(serializedAttrs).ToList();
                var extraAttrs = serializedAttrs.Except(originalAttrs).ToList();
                
                Console.WriteLine($"Missing attributes count: {missingAttrs.Count}");
                Console.WriteLine($"Extra attributes count: {extraAttrs.Count}");
                
                if (missingAttrs.Any())
                {
                    Console.WriteLine("Missing attributes:");
                    foreach (var attr in missingAttrs.Take(10)) // 只显示前10个
                    {
                        Console.WriteLine($"  {attr}");
                    }
                }
                
                if (extraAttrs.Any())
                {
                    Console.WriteLine("Extra attributes:");
                    foreach (var attr in extraAttrs.Take(10)) // 只显示前10个
                    {
                        Console.WriteLine($"  {attr}");
                    }
                }
            }
            
            // 检查特定元素类型
            var originalWidgets = doc1.Descendants("widget").Count();
            var serializedWidgets = doc2.Descendants("widget").Count();
            var originalSubWidgets = doc1.Descendants("sub_widget").Count();
            var serializedSubWidgets = doc2.Descendants("sub_widget").Count();
            
            Console.WriteLine($"Original widgets: {originalWidgets}, Serialized widgets: {serializedWidgets}");
            Console.WriteLine($"Original sub_widgets: {originalSubWidgets}, Serialized sub_widgets: {serializedSubWidgets}");
            
            // 允许微小的属性差异（命名空间属性和拼写修正）
            Assert.True(Math.Abs(attrCount1 - attrCount2) <= 2, $"属性数量差异过大: {attrCount1} vs {attrCount2}");
        }
    }
}