using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleLayoutDebugTest
    {
        [Fact]
        public void Simple_FloraKindsLayout_Test()
        {
            Console.WriteLine("=== 简单FloraKindsLayout测试 ===");
            
            var xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;
            
            Console.WriteLine($"XML长度: {xml.Length}");
            
            // 尝试反序列化
            try
            {
                var obj = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);
                Console.WriteLine("反序列化成功！");
                
                if (obj is FloraKindsLayoutDO floraLayout)
                {
                    Console.WriteLine($"Type: {floraLayout.Type}");
                    Console.WriteLine($"HasLayouts: {floraLayout.HasLayouts}");
                    Console.WriteLine($"Layouts count: {floraLayout.Layouts.LayoutList.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"反序列化失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
            }
        }
    }
}