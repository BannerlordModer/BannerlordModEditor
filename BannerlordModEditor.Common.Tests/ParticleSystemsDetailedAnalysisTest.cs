using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsDetailedAnalysisTest
    {
        [Fact]
        public void Analyze_ParticleSystems_Differences()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 分析差异
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            Console.WriteLine($"原始XML元素数量: {CountElements(doc1.Root)}");
            Console.WriteLine($"序列化后XML元素数量: {CountElements(doc2.Root)}");
            Console.WriteLine($"原始XML属性数量: {CountAttributes(doc1.Root)}");
            Console.WriteLine($"序列化后XML属性数量: {CountAttributes(doc2.Root)}");

            // 检查具体的缺失元素
            var missingElements = FindMissingElements(doc1.Root, doc2.Root);
            Console.WriteLine($"缺失的元素类型统计:");
            foreach (var group in missingElements.GroupBy(e => e.Name.LocalName))
            {
                Console.WriteLine($"  {group.Key}: {group.Count()}");
            }

            // 检查curve元素的具体情况
            var originalCurves = doc1.Root.Descendants().Where(e => e.Name.LocalName == "curve").ToList();
            var serializedCurves = doc2.Root.Descendants().Where(e => e.Name.LocalName == "curve").ToList();
            
            Console.WriteLine($"原始curve元素数量: {originalCurves.Count}");
            Console.WriteLine($"序列化后curve元素数量: {serializedCurves.Count}");

            // 检查keys元素
            var originalKeys = doc1.Root.Descendants().Where(e => e.Name.LocalName == "keys").ToList();
            var serializedKeys = doc2.Root.Descendants().Where(e => e.Name.LocalName == "keys").ToList();
            
            Console.WriteLine($"原始keys元素数量: {originalKeys.Count}");
            Console.WriteLine($"序列化后keys元素数量: {serializedKeys.Count}");

            // 检查key元素
            var originalKey = doc1.Root.Descendants().Where(e => e.Name.LocalName == "key").ToList();
            var serializedKey = doc2.Root.Descendants().Where(e => e.Name.LocalName == "key").ToList();
            
            Console.WriteLine($"原始key元素数量: {originalKey.Count}");
            Console.WriteLine($"序列化后key元素数量: {serializedKey.Count}");

            // 检查是否有空的curve或keys元素
            var emptyCurves = originalCurves.Where(c => !c.Elements().Any()).ToList();
            var emptyKeys = originalKeys.Where(k => !k.Elements().Any()).ToList();
            
            Console.WriteLine($"空的curve元素数量: {emptyCurves.Count}");
            Console.WriteLine($"空的keys元素数量: {emptyKeys.Count}");

            // 检查一个具体的例子
            if (originalCurves.Count > 0)
            {
                var firstCurve = originalCurves.First();
                Console.WriteLine($"第一个curve元素:");
                Console.WriteLine($"  父元素: {firstCurve.Parent.Name.LocalName}");
                Console.WriteLine($"  属性: {string.Join(", ", firstCurve.Attributes().Select(a => $"{a.Name}={a.Value}"))}");
                Console.WriteLine($"  子元素: {string.Join(", ", firstCurve.Elements().Select(e => e.Name.LocalName))}");
                Console.WriteLine($"  是否为空: {!firstCurve.Elements().Any()}");
            }

            // 临时断言 - 用于查看分析结果
            Assert.True(true, "分析完成，请查看控制台输出");
        }

        private int CountElements(XElement element)
        {
            int count = 1; // count the element itself
            foreach (var child in element.Elements())
            {
                count += CountElements(child);
            }
            return count;
        }

        private int CountAttributes(XElement element)
        {
            int count = element.Attributes().Count();
            foreach (var child in element.Elements())
            {
                count += CountAttributes(child);
            }
            return count;
        }

        private List<XElement> FindMissingElements(XElement original, XElement serialized)
        {
            var missing = new List<XElement>();
            
            // 比较当前元素的子元素
            var originalChildren = original.Elements().ToList();
            var serializedChildren = serialized.Elements().ToList();
            
            foreach (var origChild in originalChildren)
            {
                var matchingChild = serializedChildren.FirstOrDefault(s => 
                    s.Name.LocalName == origChild.Name.LocalName && 
                    AttributesMatch(origChild, s));
                
                if (matchingChild == null)
                {
                    missing.Add(origChild);
                }
                else
                {
                    // 递归检查子元素
                    missing.AddRange(FindMissingElements(origChild, matchingChild));
                }
            }
            
            return missing;
        }

        private bool AttributesMatch(XElement a, XElement b)
        {
            var aAttrs = a.Attributes().ToDictionary(attr => attr.Name.LocalName, attr => attr.Value);
            var bAttrs = b.Attributes().ToDictionary(attr => attr.Name.LocalName, attr => attr.Value);
            
            return aAttrs.Count == bAttrs.Count && 
                   aAttrs.All(kv => bAttrs.ContainsKey(kv.Key) && bAttrs[kv.Key] == kv.Value);
        }
    }
}