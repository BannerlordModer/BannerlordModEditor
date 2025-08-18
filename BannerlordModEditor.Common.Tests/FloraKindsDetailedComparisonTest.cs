using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDetailedComparisonTest
    {
        [Fact]
        public void Test_FindSingleAttributeDifference()
        {
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<FloraKindsDO>(xml);
            
            // 序列化
            var xml2 = XmlTestUtils.Serialize(obj);
            
            // 解析两个XML文档
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
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
            var floraKinds1 = doc1.Root?.Elements("flora_kind").ToList();
            var floraKinds2 = doc2.Root?.Elements("flora_kind").ToList();
            
            Console.WriteLine($"原始FloraKind数量: {floraKinds1?.Count ?? 0}");
            Console.WriteLine($"序列化FloraKind数量: {floraKinds2?.Count ?? 0}");
            
            // 检查每个FloraKind的季节属性
            for (int i = 0; i < Math.Min(floraKinds1?.Count ?? 0, floraKinds2?.Count ?? 0); i++)
            {
                var fk1 = floraKinds1![i];
                var fk2 = floraKinds2![i];
                
                var name1 = fk1.Attribute("name")?.Value;
                var name2 = fk2.Attribute("name")?.Value;
                
                if (name1 != name2)
                {
                    Console.WriteLine($"FloraKind名称不匹配: {name1} != {name2}");
                    continue;
                }
                
                var seasonalKinds1 = fk1.Elements("seasonal_kind").ToList();
                var seasonalKinds2 = fk2.Elements("seasonal_kind").ToList();
                
                if (seasonalKinds1.Count != seasonalKinds2.Count)
                {
                    Console.WriteLine($"FloraKind {name1} 的SeasonalKinds数量不匹配: {seasonalKinds1.Count} != {seasonalKinds2.Count}");
                    continue;
                }
                
                // 检查季节属性
                for (int j = 0; j < seasonalKinds1.Count; j++)
                {
                    var sk1 = seasonalKinds1[j];
                    var sk2 = seasonalKinds2[j];
                    
                    var season1 = sk1.Attribute("season")?.Value;
                    var season2 = sk2.Attribute("season")?.Value;
                    
                    if (season1 != season2)
                    {
                        Console.WriteLine($"FloraKind {name1} 的季节属性不匹配: {season1} != {season2}");
                    }
                }
            }
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