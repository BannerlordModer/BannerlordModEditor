using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DetailedSubWidgetTest
    {
        [Fact]
        public void Analyze_SubWidget_Structure()
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
            
            // 找到widget[107]
            var widget107_1 = doc1.Descendants("widget").ElementAt(107);
            var widget107_2 = doc2.Descendants("widget").ElementAt(107);
            
            Console.WriteLine($"Widget 107 original name: {widget107_1.Attribute("name")?.Value}");
            Console.WriteLine($"Widget 107 serialized name: {widget107_2.Attribute("name")?.Value}");
            
            // 分析sub_widgets结构
            var subWidgets1 = widget107_1.Element("sub_widgets");
            var subWidgets2 = widget107_2.Element("sub_widgets");
            
            Console.WriteLine($"Original sub_widgets: {(subWidgets1 != null ? "exists" : "missing")}");
            Console.WriteLine($"Serialized sub_widgets: {(subWidgets2 != null ? "exists" : "missing")}");
            
            if (subWidgets1 != null)
            {
                var subWidgetList1 = subWidgets1.Elements("sub_widget").ToList();
                Console.WriteLine($"Original sub_widgets count: {subWidgetList1.Count}");
                
                for (int i = 0; i < subWidgetList1.Count; i++)
                {
                    var subWidget = subWidgetList1[i];
                    Console.WriteLine($"  Original sub_widget[{i}]: name={subWidget.Attribute("name")?.Value}, ref={subWidget.Attribute("ref")?.Value}");
                    
                    var nestedSubWidgets = subWidget.Element("sub_widgets");
                    if (nestedSubWidgets != null)
                    {
                        var nestedList = nestedSubWidgets.Elements("sub_widget").ToList();
                        Console.WriteLine($"    Nested sub_widgets count: {nestedList.Count}");
                    }
                }
            }
            
            if (subWidgets2 != null)
            {
                var subWidgetList2 = subWidgets2.Elements("sub_widget").ToList();
                Console.WriteLine($"Serialized sub_widgets count: {subWidgetList2.Count}");
                
                for (int i = 0; i < subWidgetList2.Count; i++)
                {
                    var subWidget = subWidgetList2[i];
                    Console.WriteLine($"  Serialized sub_widget[{i}]: name={subWidget.Attribute("name")?.Value}, ref={subWidget.Attribute("ref")?.Value}");
                    
                    var nestedSubWidgets = subWidget.Element("sub_widgets");
                    if (nestedSubWidgets != null)
                    {
                        var nestedList = nestedSubWidgets.Elements("sub_widget").ToList();
                        Console.WriteLine($"    Nested sub_widgets count: {nestedList.Count}");
                    }
                }
            }
            
            // 检查对象模型中的结构
            Console.WriteLine("\n=== Object Model Analysis ===");
            if (obj.Widgets?.WidgetList != null && obj.Widgets.WidgetList.Count > 107)
            {
                var widget107 = obj.Widgets.WidgetList[107];
                Console.WriteLine($"Widget 107 in object: name={widget107.Name}, type={widget107.Type}");
                
                if (widget107.SubWidgets != null && widget107.SubWidgets.SubWidgetList != null)
                {
                    Console.WriteLine($"SubWidgets in object: count={widget107.SubWidgets.SubWidgetList.Count}");
                    
                    for (int i = 0; i < widget107.SubWidgets.SubWidgetList.Count; i++)
                    {
                        var subWidget = widget107.SubWidgets.SubWidgetList[i];
                        Console.WriteLine($"  SubWidget[{i}]: name={subWidget.Name}, ref={subWidget.Ref}");
                        
                        if (subWidget.SubWidgets != null && subWidget.SubWidgets.SubWidgetList != null)
                        {
                            Console.WriteLine($"    Nested SubWidgets: count={subWidget.SubWidgets.SubWidgetList.Count}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("SubWidgets in object: null or empty");
                }
            }
        }
    }
}