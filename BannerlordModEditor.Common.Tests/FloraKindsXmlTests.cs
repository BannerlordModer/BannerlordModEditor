using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsXmlTests
    {
        [Theory]
        [InlineData("TestData/flora_kinds.xml")]
        public void FloraKinds_RoundTrip_StructuralEquality(string xmlPath)
        {
            var xml = File.ReadAllText(xmlPath);
            var obj = XmlTestUtils.Deserialize<FloraKinds>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            // 调试信息：分析具体差异
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine($"XML文件大小: {xml.Length} 字符");
                Console.WriteLine($"序列化XML大小: {xml2.Length} 字符");
                Console.WriteLine($"FloraKind数量: {obj.FloraKindList.Count}");
                Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
                Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
                
                if (report.AttributeValueDifferences.Count > 0)
                {
                    Console.WriteLine($"属性值差异数量: {report.AttributeValueDifferences.Count}");
                    // 只显示前几个差异
                    foreach (var diff in report.AttributeValueDifferences.Take(3))
                    {
                        Console.WriteLine($"  - {diff}");
                    }
                }
                
                // 保存分析结果
                File.WriteAllText("flora_kinds_debug_analysis.txt", 
                    $"FloraKinds测试失败分析:\n" +
                    $"原始大小: {xml.Length}\n" +
                    $"序列化大小: {xml2.Length}\n" +
                    $"FloraKind数量: {obj.FloraKindList.Count}\n" +
                    $"节点数量差异: {report.NodeCountDifference}\n" +
                    $"属性数量差异: {report.AttributeCountDifference}\n" +
                    $"属性值差异: {string.Join("\n", report.AttributeValueDifferences)}\n");
            }

            // 回退参数，保留原始结构比较，后续可扩展为更智能比较
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}