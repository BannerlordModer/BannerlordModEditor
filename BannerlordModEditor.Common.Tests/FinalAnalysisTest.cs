using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class FinalAnalysisTest
    {
        [Fact]
        public void Final_Analysis()
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
            
            // 获取所有sub_widget元素
            var subWidgets1 = doc1.Descendants("sub_widget").ToList();
            var subWidgets2 = doc2.Descendants("sub_widget").ToList();
            
            Console.WriteLine($"Original sub_widgets count: {subWidgets1.Count}");
            Console.WriteLine($"Serialized sub_widgets count: {subWidgets2.Count}");
            
            // 检查每个sub_widget的name属性
            for (int i = 0; i < Math.Min(subWidgets1.Count, subWidgets2.Count); i++)
            {
                var subWidget1 = subWidgets1[i];
                var subWidget2 = subWidgets2[i];
                
                var name1 = subWidget1.Attribute("name")?.Value;
                var name2 = subWidget2.Attribute("name")?.Value;
                
                var size1 = subWidget1.Attribute("size")?.Value;
                var size2 = subWidget2.Attribute("size")?.Value;
                
                var path1 = GetElementPath(subWidget1);
                var path2 = GetElementPath(subWidget2);
                
                if (name1 != name2)
                {
                    Console.WriteLine($"NAME MISMATCH at index {i}:");
                    Console.WriteLine($"  Path1: {path1}");
                    Console.WriteLine($"  Path2: {path2}");
                    Console.WriteLine($"  Original: {name1}");
                    Console.WriteLine($"  Serialized: {name2}");
                    Console.WriteLine();
                }
                
                if (size1 != size2)
                {
                    Console.WriteLine($"SIZE MISMATCH at index {i}:");
                    Console.WriteLine($"  Path1: {path1}");
                    Console.WriteLine($"  Path2: {path2}");
                    Console.WriteLine($"  Original: {size1}");
                    Console.WriteLine($"  Serialized: {size2}");
                    Console.WriteLine();
                }
            }
            
            // 检查sub_widget总数差异
            if (subWidgets1.Count != subWidgets2.Count)
            {
                Console.WriteLine($"SUB_WIDGET COUNT MISMATCH: {subWidgets1.Count} vs {subWidgets2.Count}");
                
                if (subWidgets1.Count > subWidgets2.Count)
                {
                    Console.WriteLine("Missing sub_widgets in serialized:");
                    for (int i = subWidgets2.Count; i < subWidgets1.Count; i++)
                    {
                        var subWidget = subWidgets1[i];
                        var path = GetElementPath(subWidget);
                        var name = subWidget.Attribute("name")?.Value;
                        Console.WriteLine($"  {path}: {name}");
                    }
                }
                else
                {
                    Console.WriteLine("Extra sub_widgets in serialized:");
                    for (int i = subWidgets1.Count; i < subWidgets2.Count; i++)
                    {
                        var subWidget = subWidgets2[i];
                        var path = GetElementPath(subWidget);
                        var name = subWidget.Attribute("name")?.Value;
                        Console.WriteLine($"  {path}: {name}");
                    }
                }
            }
        }
        
        private string GetElementPath(XElement element)
        {
            var path = new System.Collections.Generic.List<string>();
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