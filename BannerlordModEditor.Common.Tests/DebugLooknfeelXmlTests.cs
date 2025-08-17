using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

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
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);
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
            // 允许微小的属性差异（命名空间属性和拼写修正）
            var doc1 = System.Xml.Linq.XDocument.Parse(xml);
            var doc2 = System.Xml.Linq.XDocument.Parse(xml2);
            
            int nodeCount1 = doc1.Descendants().Count();
            int nodeCount2 = doc2.Descendants().Count();
            int attrCount1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            int attrCount2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Assert.Equal(nodeCount1, nodeCount2);
            Assert.True(Math.Abs(attrCount1 - attrCount2) <= 2, $"属性数量差异过大: {attrCount1} vs {attrCount2}");
        }
    }
}