using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class SubWidgetIndexAnalysisTest
    {
        [Fact]
        public void Analyze_SubWidget_Index_Issue()
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
            
            // 获取所有sub_widgets元素
            var allSubWidgets1 = doc1.Descendants("sub_widgets").ToList();
            var allSubWidgets2 = doc2.Descendants("sub_widgets").ToList();
            
            Console.WriteLine($"Original sub_widgets count: {allSubWidgets1.Count}");
            Console.WriteLine($"Serialized sub_widgets count: {allSubWidgets2.Count}");
            
            // 查找每个sub_widgets的详细信息
            for (int i = 0; i < Math.Max(allSubWidgets1.Count, allSubWidgets2.Count); i++)
            {
                if (i < allSubWidgets1.Count)
                {
                    var subWidgets1 = allSubWidgets1[i];
                    var parent1 = subWidgets1.Parent;
                    var path1 = GetElementPath(subWidgets1);
                    var subWidgetCount1 = subWidgets1.Elements("sub_widget").Count();
                    
                    Console.WriteLine($"Original[{i}]: {path1} -> {subWidgetCount1} sub_widgets");
                }
                
                if (i < allSubWidgets2.Count)
                {
                    var subWidgets2 = allSubWidgets2[i];
                    var parent2 = subWidgets2.Parent;
                    var path2 = GetElementPath(subWidgets2);
                    var subWidgetCount2 = subWidgets2.Elements("sub_widget").Count();
                    
                    Console.WriteLine($"Serialized[{i}]: {path2} -> {subWidgetCount2} sub_widgets");
                }
                
                if (i < allSubWidgets1.Count && i < allSubWidgets2.Count)
                {
                    var subWidgets1 = allSubWidgets1[i];
                    var subWidgets2 = allSubWidgets2[i];
                    var subWidgetCount1 = subWidgets1.Elements("sub_widget").Count();
                    var subWidgetCount2 = subWidgets2.Elements("sub_widget").Count();
                    
                    if (subWidgetCount1 != subWidgetCount2)
                    {
                        var path = GetElementPath(subWidgets1);
                        Console.WriteLine($"MISMATCH at {path}: {subWidgetCount1} vs {subWidgetCount2}");
                        
                        // 显示详细信息
                        Console.WriteLine("  Original sub_widgets:");
                        foreach (var subWidget in subWidgets1.Elements("sub_widget"))
                        {
                            Console.WriteLine($"    {subWidget.Attribute("name")?.Value ?? "no name"}");
                        }
                        
                        Console.WriteLine("  Serialized sub_widgets:");
                        foreach (var subWidget in subWidgets2.Elements("sub_widget"))
                        {
                            Console.WriteLine($"    {subWidget.Attribute("name")?.Value ?? "no name"}");
                        }
                    }
                }
            }
            
            // 检查是否存在空sub_widgets
            var emptySubWidgets1 = allSubWidgets1.Where(sw => !sw.Elements("sub_widget").Any()).ToList();
            var emptySubWidgets2 = allSubWidgets2.Where(sw => !sw.Elements("sub_widget").Any()).ToList();
            
            Console.WriteLine($"\nEmpty sub_widgets in original: {emptySubWidgets1.Count}");
            Console.WriteLine($"Empty sub_widgets in serialized: {emptySubWidgets2.Count}");
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