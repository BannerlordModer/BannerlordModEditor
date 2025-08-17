using System;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Tests;
using BannerlordModEditor.Common.Models.Data;

namespace ItemModifiersAnalysis
{
    class Program
    {
        static void Main()
        {
            string testDataPath = "BannerlordModEditor.Common.Tests/TestData/item_modifiers.xml";
            string xml = File.ReadAllText(testDataPath);
            
            Console.WriteLine("=== ItemModifiers 测试失败分析 ===");
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<ItemModifiers>(xml);
            
            // 序列化
            string serialized = XmlTestUtils.Serialize(model);
            
            // 分析节点差异
            AnalyzeNodeDifferences(xml, serialized);
        }
        
        static void AnalyzeNodeDifferences(string original, string serialized)
        {
            var originalDoc = XDocument.Parse(original);
            var serializedDoc = XDocument.Parse(serialized);
            
            var originalNodes = originalDoc.Descendants().ToList();
            var serializedNodes = serializedDoc.Descendants().ToList();
            
            Console.WriteLine($"原始节点总数: {originalNodes.Count}");
            Console.WriteLine($"序列化节点总数: {serializedNodes.Count}");
            Console.WriteLine($"节点差异: {serializedNodes.Count - originalNodes.Count}");
            
            // 按元素名称分组统计
            var originalGroups = originalNodes.GroupBy(n => n.Name.LocalName).ToDictionary(g => g.Key, g => g.Count());
            var serializedGroups = serializedNodes.GroupBy(n => n.Name.LocalName).ToDictionary(g => g.Key, g => g.Count());
            
            Console.WriteLine("\n=== 元素名称统计 ===");
            foreach (var name in originalGroups.Keys.Concat(serializedGroups.Keys).Distinct().OrderBy(n => n))
            {
                int origCount = originalGroups.GetValueOrDefault(name);
                int serCount = serializedGroups.GetValueOrDefault(name);
                if (origCount != serCount)
                {
                    Console.WriteLine($"{name}: 原始={origCount}, 序列化={serCount}, 差异={serCount - origCount}");
                }
            }
            
            // 检查每个ItemModifier的属性
            Console.WriteLine("\n=== ItemModifier 属性分析 ===");
            var originalModifiers = originalNodes.Where(n => n.Name.LocalName == "ItemModifier").ToList();
            var serializedModifiers = serializedNodes.Where(n => n.Name.LocalName == "ItemModifier").ToList();
            
            if (originalModifiers.Count == serializedModifiers.Count)
            {
                for (int i = 0; i < originalModifiers.Count; i++)
                {
                    var orig = originalModifiers[i];
                    var ser = serializedModifiers[i];
                    
                    var origAttrs = orig.Attributes().ToList();
                    var serAttrs = ser.Attributes().ToList();
                    
                    if (origAttrs.Count != serAttrs.Count)
                    {
                        Console.WriteLine($"ItemModifier {i} ({orig.Attribute("id")?.Value}): 属性数不匹配");
                        Console.WriteLine($"  原始属性 ({origAttrs.Count}): {string.Join(", ", origAttrs.Select(a => $"{a.Name}={a.Value}"))}");
                        Console.WriteLine($"  序列化属性 ({serAttrs.Count}): {string.Join(", ", serAttrs.Select(a => $"{a.Name}={a.Value}"))}");
                    }
                }
            }
        }
    }
}