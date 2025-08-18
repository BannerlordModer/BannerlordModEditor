using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Layouts;

namespace BannerlordModEditor.Common.Tests
{
    public class LayoutDebugTest
    {
        [Fact]
        public void Debug_FloraKindsLayout_XmlStructure()
        {
            Console.WriteLine("=== FloraKindsLayout XML结构调试 ===");
            
            var xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;
            
            Console.WriteLine($"XML文件路径: {xmlPath}");
            Console.WriteLine($"XML长度: {xml.Length}");
            Console.WriteLine($"XML前200字符: {xml.Substring(0, Math.Min(200, xml.Length))}");
            
            // 检查XML根元素
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            Console.WriteLine($"根元素名称: {doc.DocumentElement?.Name}");
            Console.WriteLine($"根元素命名空间: {doc.DocumentElement?.NamespaceURI}");
            
            // 检查FloraKindsLayoutDO的XmlRoot属性
            var xmlRootAttr = typeof(FloraKindsLayoutDO).GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (xmlRootAttr.Length > 0)
            {
                var rootAttr = (XmlRootAttribute)xmlRootAttr[0];
                Console.WriteLine($"FloraKindsLayoutDO XmlRoot: {rootAttr.ElementName}");
                Console.WriteLine($"FloraKindsLayoutDO XmlRoot Namespace: {rootAttr.Namespace}");
            }
            
            // 检查LayoutsBaseDO的XmlRoot属性
            var baseXmlRootAttr = typeof(LayoutsBaseDO).GetCustomAttributes(typeof(XmlRootAttribute), false);
            if (baseXmlRootAttr.Length > 0)
            {
                var baseRootAttr = (XmlRootAttribute)baseXmlRootAttr[0];
                Console.WriteLine($"LayoutsBaseDO XmlRoot: {baseRootAttr.ElementName}");
                Console.WriteLine($"LayoutsBaseDO XmlRoot Namespace: {baseRootAttr.Namespace}");
            }
            
            // 尝试直接反序列化
            try
            {
                var serializer = new XmlSerializer(typeof(FloraKindsLayoutDO));
                using var reader = new StringReader(xml);
                var obj = serializer.Deserialize(reader);
                Console.WriteLine("反序列化成功！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"反序列化失败: {ex.Message}");
                Console.WriteLine($"内部异常: {ex.InnerException?.Message}");
            }
        }
    }
}