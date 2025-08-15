using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class FindMissingAttributeTest
    {
        [Fact]
        public void Find_Single_Missing_Attribute()
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
            
            // 按元素路径组织属性
            var originalAttrsByPath = new Dictionary<string, List<string>>();
            var serializedAttrsByPath = new Dictionary<string, List<string>>();
            
            void ProcessAttributes(XDocument doc, Dictionary<string, List<string>> dict)
            {
                foreach (var element in doc.Descendants())
                {
                    var path = GetElementPath(element);
                    if (!dict.ContainsKey(path))
                    {
                        dict[path] = new List<string>();
                    }
                    
                    foreach (var attr in element.Attributes())
                    {
                        dict[path].Add($"{attr.Name.LocalName}={attr.Value}");
                    }
                }
            }
            
            ProcessAttributes(doc1, originalAttrsByPath);
            ProcessAttributes(doc2, serializedAttrsByPath);
            
            // 找出差异
            foreach (var path in originalAttrsByPath.Keys)
            {
                if (serializedAttrsByPath.TryGetValue(path, out var serializedAttrs))
                {
                    var missing = originalAttrsByPath[path].Except(serializedAttrs).ToList();
                    var extra = serializedAttrs.Except(originalAttrsByPath[path]).ToList();
                    
                    if (missing.Any())
                    {
                        Console.WriteLine($"Path: {path}");
                        Console.WriteLine($"  Missing attributes: {string.Join(", ", missing)}");
                    }
                    
                    if (extra.Any())
                    {
                        Console.WriteLine($"Path: {path}");
                        Console.WriteLine($"  Extra attributes: {string.Join(", ", extra)}");
                    }
                }
                else
                {
                    Console.WriteLine($"Missing entire path: {path}");
                }
            }
            
            // 简单检查属性总数
            int totalOriginal = originalAttrsByPath.Values.Sum(list => list.Count);
            int totalSerialized = serializedAttrsByPath.Values.Sum(list => list.Count);
            
            Console.WriteLine($"Total original attributes: {totalOriginal}");
            Console.WriteLine($"Total serialized attributes: {totalSerialized}");
            Console.WriteLine($"Difference: {totalOriginal - totalSerialized}");
        }
        
        private string GetElementPath(XElement element)
        {
            var path = new List<string>();
            var current = element;
            
            while (current != null)
            {
                if (current.Parent != null)
                {
                    var siblings = current.Parent.Elements(current.Name).ToList();
                    var index = siblings.IndexOf(current);
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