using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class Widget111ProblemTest
    {
        [Fact]
        public void Analyze_Widget111_Problem()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<Looknfeel>(xml);
            
            // 检查对象模型中的widget[111]和widget[112]
            if (obj.Widgets?.WidgetList != null && obj.Widgets.WidgetList.Count > 112)
            {
                var widget111 = obj.Widgets.WidgetList[111];
                var widget112 = obj.Widgets.WidgetList[112];
                
                Console.WriteLine($"Widget 111 in object: name={widget111.Name}, type={widget111.Type}");
                Console.WriteLine($"Widget 112 in object: name={widget112.Name}, type={widget112.Type}");
                
                // 检查widget[111]的sub_widgets
                if (widget111.SubWidgets != null && widget111.SubWidgets.SubWidgetList != null)
                {
                    Console.WriteLine($"Widget 111 sub_widgets count: {widget111.SubWidgets.SubWidgetList.Count}");
                    for (int i = 0; i < widget111.SubWidgets.SubWidgetList.Count; i++)
                    {
                        var subWidget = widget111.SubWidgets.SubWidgetList[i];
                        Console.WriteLine($"  SubWidget[{i}]: name={subWidget.Name}, ref={subWidget.Ref}");
                    }
                }
                
                // 检查widget[112]的sub_widgets
                if (widget112.SubWidgets != null && widget112.SubWidgets.SubWidgetList != null)
                {
                    Console.WriteLine($"Widget 112 sub_widgets count: {widget112.SubWidgets.SubWidgetList.Count}");
                    for (int i = 0; i < widget112.SubWidgets.SubWidgetList.Count; i++)
                    {
                        var subWidget = widget112.SubWidgets.SubWidgetList[i];
                        Console.WriteLine($"  SubWidget[{i}]: name={subWidget.Name}, ref={subWidget.Ref}");
                    }
                }
            }
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的XML
            var doc2 = XDocument.Parse(xml2);
            var widgets2 = doc2.Descendants("widget").ToList();
            
            if (widgets2.Count > 112)
            {
                var widget111_2 = widgets2[111];
                var widget112_2 = widgets2[112];
                
                Console.WriteLine($"\nWidget 111 in serialized XML: name={widget111_2.Attribute("name")?.Value}");
                Console.WriteLine($"Widget 112 in serialized XML: name={widget112_2.Attribute("name")?.Value}");
                
                // 检查widget[111]的sub_widgets
                var subWidgets111_2 = widget111_2.Element("sub_widgets");
                if (subWidgets111_2 != null)
                {
                    var subWidgetList111_2 = subWidgets111_2.Elements("sub_widget").ToList();
                    Console.WriteLine($"Widget 111 sub_widgets count in serialized: {subWidgetList111_2.Count}");
                    for (int i = 0; i < subWidgetList111_2.Count; i++)
                    {
                        var subWidget = subWidgetList111_2[i];
                        Console.WriteLine($"  SubWidget[{i}]: name={subWidget.Attribute("name")?.Value}, ref={subWidget.Attribute("ref")?.Value}");
                    }
                }
            }
        }
    }
}