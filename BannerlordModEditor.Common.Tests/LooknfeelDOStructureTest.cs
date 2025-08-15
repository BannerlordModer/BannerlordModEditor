using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class LooknfeelDOStructureTest
    {
        [Fact]
        public void LooknfeelDO_Element_Count_Analysis()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 分析原始XML结构
            var doc1 = XDocument.Parse(xml);
            var originalNodes = doc1.Descendants().ToList();
            var originalAttrs = doc1.Descendants().SelectMany(e => e.Attributes()).ToList();
            
            Console.WriteLine($"=== 原始XML分析 ===");
            Console.WriteLine($"总节点数: {originalNodes.Count}");
            Console.WriteLine($"总属性数: {originalAttrs.Count}");
            
            // 按元素类型分组统计
            var elementGroups = originalNodes.GroupBy(e => e.Name.LocalName)
                                           .Select(g => new { Element = g.Key, Count = g.Count() })
                                           .OrderByDescending(x => x.Count);
            
            Console.WriteLine("\n=== 元素类型分布 ===");
            foreach (var group in elementGroups)
            {
                Console.WriteLine($"{group.Element}: {group.Count}");
            }
            
            // 反序列化为DO
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的XML结构
            var doc2 = XDocument.Parse(xml2);
            var serializedNodes = doc2.Descendants().ToList();
            var serializedAttrs = doc2.Descendants().SelectMany(e => e.Attributes()).ToList();
            
            Console.WriteLine($"\n=== 序列化后XML分析 ===");
            Console.WriteLine($"总节点数: {serializedNodes.Count}");
            Console.WriteLine($"总属性数: {serializedAttrs.Count}");
            
            // 按元素类型分组统计
            var serializedElementGroups = serializedNodes.GroupBy(e => e.Name.LocalName)
                                                        .Select(g => new { Element = g.Key, Count = g.Count() })
                                                        .OrderByDescending(x => x.Count);
            
            Console.WriteLine("\n=== 序列化后元素类型分布 ===");
            foreach (var group in serializedElementGroups)
            {
                Console.WriteLine($"{group.Element}: {group.Count}");
            }
            
            // 找出差异
            Console.WriteLine("\n=== 节点差异分析 ===");
            var originalElementDict = originalNodes.GroupBy(e => e.Name.LocalName)
                                                   .ToDictionary(g => g.Key, g => g.Count());
            var serializedElementDict = serializedNodes.GroupBy(e => e.Name.LocalName)
                                                      .ToDictionary(g => g.Key, g => g.Count());
            
            foreach (var element in originalElementDict.Keys.Concat(serializedElementDict.Keys).Distinct())
            {
                int originalCount = originalElementDict.GetValueOrDefault(element);
                int serializedCount = serializedElementDict.GetValueOrDefault(element);
                if (originalCount != serializedCount)
                {
                    Console.WriteLine($"{element}: 原始={originalCount}, 序列化={serializedCount}, 差异={originalCount - serializedCount}");
                }
            }
            
            // 详细分析丢失的节点
            Console.WriteLine("\n=== 详细节点分析 ===");
            for (int i = 0; i < Math.Min(originalNodes.Count, serializedNodes.Count); i++)
            {
                var originalNode = originalNodes[i];
                var serializedNode = serializedNodes[i];
                
                if (originalNode.Name.LocalName != serializedNode.Name.LocalName)
                {
                    Console.WriteLine($"位置 {i}: 原始={originalNode.Name.LocalName}, 序列化={serializedNode.Name.LocalName}");
                }
            }
            
            // 如果节点数量不同，显示多出的节点
            if (originalNodes.Count > serializedNodes.Count)
            {
                Console.WriteLine($"\n=== 原始XML多出的 {originalNodes.Count - serializedNodes.Count} 个节点 ===");
                for (int i = serializedNodes.Count; i < originalNodes.Count; i++)
                {
                    var node = originalNodes[i];
                    Console.WriteLine($"位置 {i}: {node.Name.LocalName} (父节点: {node.Parent?.Name.LocalName})");
                }
            }
            else if (serializedNodes.Count > originalNodes.Count)
            {
                Console.WriteLine($"\n=== 序列化XML多出的 {serializedNodes.Count - originalNodes.Count} 个节点 ===");
                for (int i = originalNodes.Count; i < serializedNodes.Count; i++)
                {
                    var node = serializedNodes[i];
                    Console.WriteLine($"位置 {i}: {node.Name.LocalName} (父节点: {node.Parent?.Name.LocalName})");
                }
            }
        }
    }
}