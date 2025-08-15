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
                    .SelectMany(e => e.Attributes().Select(a => $"{GetElementPath(e)}@{a.Name.LocalName}={a.Value}"))
                    .ToList();
                    
                var serializedAttrs = doc2.Descendants()
                    .SelectMany(e => e.Attributes().Select(a => $"{GetElementPath(e)}@{a.Name.LocalName}={a.Value}"))
                    .ToList();
                
                var missingAttrs = originalAttrs.Except(serializedAttrs).ToList();
                var extraAttrs = serializedAttrs.Except(originalAttrs).ToList();
                
                Console.WriteLine($"Missing attributes count: {missingAttrs.Count}");
                Console.WriteLine($"Extra attributes count: {extraAttrs.Count}");
                
                if (missingAttrs.Any())
                {
                    Console.WriteLine("Missing attributes:");
                    foreach (var attr in missingAttrs)
                    {
                        Console.WriteLine($"  {attr}");
                    }
                }
                
                if (extraAttrs.Any())
                {
                    Console.WriteLine("Extra attributes:");
                    foreach (var attr in extraAttrs)
                    {
                        Console.WriteLine($"  {attr}");
                    }
                }
            }
        }
        
        private static string GetElementPath(XElement element)
        {
            var path = new System.Collections.Generic.List<string>();
            var current = element;
            
            while (current != null)
            {
                var index = current.Parent != null ? 
                    current.Parent.Elements(current.Name).ToList().IndexOf(current) : -1;
                
                if (index >= 0)
                {
                    path.Insert(0, $"{current.Name.LocalName}[{index}]");
                }
                else
                {
                    path.Insert(0, current.Name.LocalName);
                }
                
                current = current.Parent;
            }
            
            return string.Join("/", path);
        }
    }
}