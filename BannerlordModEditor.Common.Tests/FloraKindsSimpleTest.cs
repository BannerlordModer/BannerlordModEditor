using System;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.Data;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsSimpleTest
    {
        [Fact]
        public void FloraKinds_SimpleAnalysis()
        {
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"XML文件大小: {xml.Length} 字符");
            
            // 1. 测试反序列化
            var obj = XmlTestUtils.Deserialize<FloraKinds>(xml);
            Assert.NotNull(obj);
            Console.WriteLine($"反序列化成功: FloraKind数量 = {obj.FloraKindList.Count}");
            
            // 2. 测试序列化
            var xml2 = XmlTestUtils.Serialize(obj);
            Assert.NotNull(xml2);
            Console.WriteLine($"序列化成功: 大小 = {xml2.Length} 字符");
            
            // 3. 结构比较
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Console.WriteLine($"结构相等: {report.IsStructurallyEqual}");
            
            if (!report.IsStructurallyEqual)
            {
                Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
                Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
                
                // 只显示前几个差异，避免输出过多
                if (report.MissingNodes.Count > 0)
                    Console.WriteLine($"缺失节点数量: {report.MissingNodes.Count}");
                if (report.ExtraNodes.Count > 0)
                    Console.WriteLine($"多余节点数量: {report.ExtraNodes.Count}");
                if (report.AttributeValueDifferences.Count > 0)
                    Console.WriteLine($"属性值差异数量: {report.AttributeValueDifferences.Count}");
                
                // 保存详细结果到文件
                var resultPath = "flora_kinds_analysis_result.txt";
                File.WriteAllText(resultPath, $"FloraKinds分析结果:\n" +
                    $"结构相等: {report.IsStructurallyEqual}\n" +
                    $"节点数量差异: {report.NodeCountDifference}\n" +
                    $"属性数量差异: {report.AttributeCountDifference}\n" +
                    $"缺失节点: {string.Join(", ", report.MissingNodes)}\n" +
                    $"多余节点: {string.Join(", ", report.ExtraNodes)}\n" +
                    $"属性值差异: {string.Join(", ", report.AttributeValueDifferences)}\n");
                Console.WriteLine($"详细结果已保存到: {resultPath}");
            }
        }
    }
}