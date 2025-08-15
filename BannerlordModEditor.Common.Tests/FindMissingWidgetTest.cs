using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class FindMissingWidgetTest
    {
        [Fact]
        public void Find_Missing_Widget()
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
            
            // 获取所有widget元素
            var widgets1 = doc1.Descendants("widget").ToList();
            var widgets2 = doc2.Descendants("widget").ToList();
            
            Console.WriteLine($"Original widgets count: {widgets1.Count}");
            Console.WriteLine($"Serialized widgets count: {widgets2.Count}");
            
            // 找出哪个widget丢失了
            for (int i = 0; i < Math.Max(widgets1.Count, widgets2.Count); i++)
            {
                if (i < widgets1.Count && i < widgets2.Count)
                {
                    var widget1 = widgets1[i];
                    var widget2 = widgets2[i];
                    var name1 = widget1.Attribute("name")?.Value;
                    var name2 = widget2.Attribute("name")?.Value;
                    
                    if (name1 != name2)
                    {
                        Console.WriteLine($"MISMATCH at index {i}:");
                        Console.WriteLine($"  Original: {name1}");
                        Console.WriteLine($"  Serialized: {name2}");
                    }
                }
                else if (i < widgets1.Count)
                {
                    var widget = widgets1[i];
                    var name = widget.Attribute("name")?.Value;
                    Console.WriteLine($"Missing in serialized at index {i}: {name}");
                }
                else if (i < widgets2.Count)
                {
                    var widget = widgets2[i];
                    var name = widget.Attribute("name")?.Value;
                    Console.WriteLine($"Extra in serialized at index {i}: {name}");
                }
            }
            
            // 检查widget[107]附近的详细信息
            Console.WriteLine("\n=== Detailed analysis around index 107 ===");
            for (int i = 105; i < 115; i++)
            {
                if (i < widgets1.Count)
                {
                    var widget = widgets1[i];
                    var name = widget.Attribute("name")?.Value;
                    var type = widget.Attribute("type")?.Value;
                    Console.WriteLine($"Original[{i}]: {name} (type: {type})");
                }
                
                if (i < widgets2.Count)
                {
                    var widget = widgets2[i];
                    var name = widget.Attribute("name")?.Value;
                    var type = widget.Attribute("type")?.Value;
                    Console.WriteLine($"Serialized[{i}]: {name} (type: {type})");
                }
                
                Console.WriteLine();
            }
        }
    }
}