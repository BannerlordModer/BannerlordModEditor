using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class LooknfeelStructureAnalysisTest
    {
        [Fact]
        public void Analyze_Looknfeel_Property_Misalignment_Issue()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            Console.WriteLine("=== Looknfeel结构分析测试 ===");
            
            // 分析原始XML结构
            var doc = XDocument.Parse(xml);
            var allElements = doc.Descendants().ToList();
            
            // 找出所有包含特定属性的元素
            var refElements = allElements.Where(e => e.Attribute("ref") != null).ToList();
            var sizeElements = allElements.Where(e => e.Attribute("size") != null).ToList();
            var positionElements = allElements.Where(e => e.Attribute("position") != null).ToList();
            var styleElements = allElements.Where(e => e.Attribute("style") != null).ToList();
            
            Console.WriteLine($"包含ref属性的元素数量: {refElements.Count}");
            Console.WriteLine($"包含size属性的元素数量: {sizeElements.Count}");
            Console.WriteLine($"包含position属性的元素数量: {positionElements.Count}");
            Console.WriteLine($"包含style属性的元素数量: {styleElements.Count}");
            
            // 分析sub_widget元素
            var subWidgets = allElements.Where(e => e.Name.LocalName == "sub_widget").ToList();
            Console.WriteLine($"\\nsub_widget元素数量: {subWidgets.Count}");
            
            foreach (var subWidget in subWidgets.Take(3))
            {
                Console.WriteLine($"\\nsub_widget属性:");
                foreach (var attr in subWidget.Attributes())
                {
                    Console.WriteLine($"  {attr.Name}: {attr.Value}");
                }
            }
            
            // 分析meshes元素
            var meshesElements = allElements.Where(e => e.Name.LocalName == "meshes").ToList();
            Console.WriteLine($"\\nmeshes元素数量: {meshesElements.Count}");
            
            foreach (var meshes in meshesElements.Take(3))
            {
                Console.WriteLine($"\\nmeshes元素 (应该没有属性):");
                foreach (var attr in meshes.Attributes())
                {
                    Console.WriteLine($"  {attr.Name}: {attr.Value}");
                }
                
                // 分析meshes的子元素
                var children = meshes.Elements().ToList();
                Console.WriteLine($"  子元素: {string.Join(", ", children.Select(c => c.Name.LocalName))}");
            }
            
            // 反序列化并立即再序列化，看看会发生什么
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的结构
            var doc2 = XDocument.Parse(xml2);
            var allElements2 = doc2.Descendants().ToList();
            
            // 检查属性是否发生了错位
            var refElements2 = allElements2.Where(e => e.Attribute("ref") != null).ToList();
            var sizeElements2 = allElements2.Where(e => e.Attribute("size") != null).ToList();
            var positionElements2 = allElements2.Where(e => e.Attribute("position") != null).ToList();
            var styleElements2 = allElements2.Where(e => e.Attribute("style") != null).ToList();
            
            Console.WriteLine($"\\n=== 序列化后分析 ===");
            Console.WriteLine($"包含ref属性的元素数量: {refElements2.Count}");
            Console.WriteLine($"包含size属性的元素数量: {sizeElements2.Count}");
            Console.WriteLine($"包含position属性的元素数量: {positionElements2.Count}");
            Console.WriteLine($"包含style属性的元素数量: {styleElements2.Count}");
            
            // 检查meshes元素是否错误地获得了属性
            var meshesElements2 = allElements2.Where(e => e.Name.LocalName == "meshes").ToList();
            Console.WriteLine($"\\n序列化后的meshes元素:");
            foreach (var meshes in meshesElements2.Take(3))
            {
                var attrs = meshes.Attributes().ToList();
                if (attrs.Count > 0)
                {
                    Console.WriteLine($"  meshes元素错误地获得了属性:");
                    foreach (var attr in attrs)
                    {
                        Console.WriteLine($"    {attr.Name}: {attr.Value}");
                    }
                }
            }
            
            // 验证假设：检查sub_widget的属性是否跑到meshes里去了
            var meshesWithSubWidgetAttrs = meshesElements2.Where(m => 
                m.Attribute("ref") != null || m.Attribute("size") != null || 
                m.Attribute("position") != null || m.Attribute("style") != null).ToList();
            
            Console.WriteLine($"\\n=== 问题确认 ===");
            Console.WriteLine($"错误获得sub_widget属性的meshes元素数量: {meshesWithSubWidgetAttrs.Count}");
            
            if (meshesWithSubWidgetAttrs.Count > 0)
            {
                Console.WriteLine("确认：XML序列化存在严重的属性错位问题！");
                foreach (var meshes in meshesWithSubWidgetAttrs.Take(2))
                {
                    Console.WriteLine($"  meshes元素属性:");
                    foreach (var attr in meshes.Attributes())
                    {
                        Console.WriteLine($"    {attr.Name}: {attr.Value}");
                    }
                }
            }
            
            // 这个测试主要是为了分析问题，不进行断言
            Assert.True(true, "分析测试完成");
        }
    }
}