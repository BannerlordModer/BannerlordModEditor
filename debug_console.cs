using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.Data;

namespace DebugTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<Looknfeel>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 简单比较：检查节点数量和属性数量
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            int nodeCount1 = doc1.Descendants().Count();
            int nodeCount2 = doc2.Descendants().Count();
            
            int attrCount1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            int attrCount2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"Original nodes: {nodeCount1}, Serialized nodes: {nodeCount2}");
            Console.WriteLine($"Original attrs: {attrCount1}, Serialized attrs: {attrCount2}");
            
            if (nodeCount1 != nodeCount2)
            {
                Console.WriteLine("NODE COUNT DIFFERENCE DETECTED!");
                var originalElements = doc1.Descendants().Select(d => d.Name.LocalName).ToList();
                var serializedElements = doc2.Descendants().Select(d => d.Name.LocalName).ToList();
                
                var missing = originalElements.Except(serializedElements).Distinct().ToList();
                var extra = serializedElements.Except(originalElements).Distinct().ToList();
                
                if (missing.Any())
                    Console.WriteLine("Missing elements: " + string.Join(", ", missing));
                if (extra.Any())
                    Console.WriteLine("Extra elements: " + string.Join(", ", extra));
            }
            
            if (attrCount1 != attrCount2)
            {
                Console.WriteLine("ATTRIBUTE COUNT DIFFERENCE DETECTED!");
            }
            
            // 保存调试文件
            File.WriteAllText("debug_original.xml", xml);
            File.WriteAllText("debug_serialized.xml", xml2);
            Console.WriteLine("Debug files saved as debug_original.xml and debug_serialized.xml");
        }
    }
}