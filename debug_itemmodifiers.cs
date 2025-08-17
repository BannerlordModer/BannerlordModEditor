using System;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.Data;

namespace ItemModifiersDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            string testDataPath = "TestData/item_modifiers.xml";
            string xml = File.ReadAllText(testDataPath);
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<ItemModifiers>(xml);
            
            // 序列化
            string serialized = XmlTestUtils.Serialize(model);
            
            // 分析差异
            AnalyzeDifferences(xml, serialized);
        }
        
        static void AnalyzeDifferences(string original, string serialized)
        {
            var originalDoc = XDocument.Parse(original);
            var serializedDoc = XDocument.Parse(serialized);
            
            var originalElements = originalDoc.Descendants().ToList();
            var serializedElements = serializedDoc.Descendants().ToList();
            
            Console.WriteLine($"原始节点数: {originalElements.Count}");
            Console.WriteLine($"序列化节点数: {serializedElements.Count}");
            Console.WriteLine($"差异: {serializedElements.Count - originalElements.Count}");
            
            // 查找额外的节点
            for (int i = 0; i < Math.Min(originalElements.Count, serializedElements.Count); i++)
            {
                var origElem = originalElements[i];
                var serElem = serializedElements[i];
                
                if (origElem.Name != serElem.Name)
                {
                    Console.WriteLine($"节点名称不匹配 at position {i}:");
                    Console.WriteLine($"  原始: {origElem.Name}");
                    Console.WriteLine($"  序列化: {serElem.Name}");
                }
                
                var origAttrs = origElem.Attributes().ToList();
                var serAttrs = serElem.Attributes().ToList();
                
                if (origAttrs.Count != serAttrs.Count)
                {
                    Console.WriteLine($"属性数量不匹配 at {origElem.Name} (position {i}):");
                    Console.WriteLine($"  原始属性数: {origAttrs.Count}");
                    Console.WriteLine($"  序列化属性数: {serAttrs.Count}");
                    
                    foreach (var attr in origAttrs)
                        Console.WriteLine($"    原始: {attr.Name}={attr.Value}");
                    foreach (var attr in serAttrs)
                        Console.WriteLine($"    序列化: {attr.Name}={attr.Value}");
                }
            }
            
            // 如果序列化的节点更多，显示额外的节点
            if (serializedElements.Count > originalElements.Count)
            {
                Console.WriteLine("\n额外的序列化节点:");
                for (int i = originalElements.Count; i < serializedElements.Count; i++)
                {
                    var elem = serializedElements[i];
                    Console.WriteLine($"  节点 {i}: {elem.Name}");
                    foreach (var attr in elem.Attributes())
                    {
                        Console.WriteLine($"    {attr.Name}={attr.Value}");
                    }
                }
            }
        }
    }
}