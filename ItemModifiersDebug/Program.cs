using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using BannerlordModEditor.Common.Tests;
using BannerlordModEditor.Common.Models.Data;

namespace ItemModifiersDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            string testDataPath = "../BannerlordModEditor.Common.Tests/TestData/item_modifiers.xml";
            string xml = File.ReadAllText(testDataPath);
            
            Console.WriteLine("=== ItemModifiers 测试失败分析 ===");
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<ItemModifiers>(xml);
            
            // 序列化
            string serialized = XmlTestUtils.Serialize(model);
            
            // 保存序列化结果到文件以便检查
            File.WriteAllText("serialized_item_modifiers.xml", serialized);
            
            // 分析差异
            AnalyzeDifferences(xml, serialized);
            
            Console.WriteLine("\n序列化结果已保存到: serialized_item_modifiers.xml");
        }
        
        static void AnalyzeDifferences(string original, string serialized)
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
            
            Console.WriteLine("\n=== 按元素名称统计 ===");
            foreach (var name in originalGroups.Keys.Concat(serializedGroups.Keys).Distinct().OrderBy(n => n))
            {
                int origCount = originalGroups.GetValueOrDefault(name);
                int serCount = serializedGroups.GetValueOrDefault(name);
                if (origCount != serCount)
                {
                    Console.WriteLine($"{name}: 原始={origCount}, 序列化={serCount}, 差异={serCount - origCount}");
                }
            }
            
            // 检查XML声明
            Console.WriteLine("\n=== XML 声明检查 ===");
            Console.WriteLine($"原始XML声明: {originalDoc.Declaration?.Version} {originalDoc.Declaration?.Encoding}");
            Console.WriteLine($"序列化XML声明: {serializedDoc.Declaration?.Version} {serializedDoc.Declaration?.Encoding}");
            
            // 检查根元素
            Console.WriteLine("\n=== 根元素检查 ===");
            Console.WriteLine($"原始根元素: {originalDoc.Root?.Name}");
            Console.WriteLine($"序列化根元素: {serializedDoc.Root?.Name}");
            
            // 检查是否有注释
            var originalComments = originalDoc.DescendantNodes().OfType<XComment>().Count();
            var serializedComments = serializedDoc.DescendantNodes().OfType<XComment>().Count();
            Console.WriteLine($"\n=== 注释检查 ===");
            Console.WriteLine($"原始注释数: {originalComments}");
            Console.WriteLine($"序列化注释数: {serializedComments}");
            
            // 检查ItemModifier元素数量
            var originalItemModifiers = originalNodes.Where(n => n.Name.LocalName == "ItemModifier").Count();
            var serializedItemModifiers = serializedNodes.Where(n => n.Name.LocalName == "ItemModifier").Count();
            Console.WriteLine($"\n=== ItemModifier元素检查 ===");
            Console.WriteLine($"原始ItemModifier数: {originalItemModifiers}");
            Console.WriteLine($"序列化ItemModifier数: {serializedItemModifiers}");
        }
    }
}