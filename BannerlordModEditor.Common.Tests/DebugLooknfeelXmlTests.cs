using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugLooknfeelXmlTests
    {
        [Fact]
        public void Debug_Looknfeel_StructuralEquality()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            Console.WriteLine($"Original XML length: {xml.Length}");
            Console.WriteLine($"Original XML start: {xml.Substring(0, Math.Min(200, xml.Length))}");

            // 反序列化
            var obj = XmlTestUtils.Deserialize<Looknfeel>(xml);
            Console.WriteLine($"Deserialized object type: {obj.GetType().Name}");
            Console.WriteLine($"Type property: {obj.Type}");
            Console.WriteLine($"VirtualResolution property: {obj.VirtualResolution}");

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            Console.WriteLine($"Serialized XML length: {xml2.Length}");
            Console.WriteLine($"Serialized XML start: {xml2.Substring(0, Math.Min(200, xml2.Length))}");

            // 获取详细的结构比较报告
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Console.WriteLine($"IsStructurallyEqual: {report.IsStructurallyEqual}");
            
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine($"NodeCountDifference: {report.NodeCountDifference}");
                Console.WriteLine($"AttributeCountDifference: {report.AttributeCountDifference}");
                Console.WriteLine($"MissingNodes: {string.Join(", ", report.MissingNodes)}");
                Console.WriteLine($"ExtraNodes: {string.Join(", ", report.ExtraNodes)}");
                Console.WriteLine($"MissingAttributes: {string.Join(", ", report.MissingAttributes)}");
                Console.WriteLine($"ExtraAttributes: {string.Join(", ", report.ExtraAttributes)}");
                Console.WriteLine($"AttributeValueDifferences: {string.Join(", ", report.AttributeValueDifferences)}");
                Console.WriteLine($"TextDifferences: {string.Join(", ", report.TextDifferences)}");
            }

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}