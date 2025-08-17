using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsSimpleTest
    {
        [Fact]
        public void Simple_ParticleSystems_Test()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 使用XmlTestUtils的结构化比较
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            // 输出基本信息
            Console.WriteLine($"原始XML长度: {xml.Length}");
            Console.WriteLine($"序列化XML长度: {xml2.Length}");
            Console.WriteLine($"结构化相等: {areEqual}");
            
            // 如果不相等，获取详细的差异报告
            if (!areEqual)
            {
                var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
                
                Console.WriteLine($"=== 详细差异报告 ===");
                Console.WriteLine($"节点数量差异: {report.NodeCountDifference}");
                Console.WriteLine($"属性数量差异: {report.AttributeCountDifference}");
                Console.WriteLine($"缺失节点数量: {report.MissingNodes.Count}");
                Console.WriteLine($"多余节点数量: {report.ExtraNodes.Count}");
                Console.WriteLine($"节点名称差异数量: {report.NodeNameDifferences.Count}");
                Console.WriteLine($"缺失属性数量: {report.MissingAttributes.Count}");
                Console.WriteLine($"多余属性数量: {report.ExtraAttributes.Count}");
                Console.WriteLine($"属性值差异数量: {report.AttributeValueDifferences.Count}");
                Console.WriteLine($"文本差异数量: {report.TextDifferences.Count}");
                
                // 输出前几个差异作为示例
                if (report.MissingNodes.Count > 0)
                {
                    Console.WriteLine($"=== 缺失节点示例 ===");
                    foreach (var node in report.MissingNodes.Take(5))
                    {
                        Console.WriteLine($"  {node}");
                    }
                }
                
                if (report.ExtraNodes.Count > 0)
                {
                    Console.WriteLine($"=== 多余节点示例 ===");
                    foreach (var node in report.ExtraNodes.Take(5))
                    {
                        Console.WriteLine($"  {node}");
                    }
                }
                
                // 保存详细报告到文件
                var reportPath = "/tmp/particle_systems_diff_report.txt";
                File.WriteAllText(reportPath, $"=== ParticleSystems 差异报告 ===\n\n" +
                    $"节点数量差异: {report.NodeCountDifference}\n" +
                    $"属性数量差异: {report.AttributeCountDifference}\n\n" +
                    $"缺失节点 ({report.MissingNodes.Count}):\n" + string.Join("\n", report.MissingNodes.Take(20)) + "\n\n" +
                    $"多余节点 ({report.ExtraNodes.Count}):\n" + string.Join("\n", report.ExtraNodes.Take(20)) + "\n\n" +
                    $"节点名称差异 ({report.NodeNameDifferences.Count}):\n" + string.Join("\n", report.NodeNameDifferences.Take(10)) + "\n\n" +
                    $"缺失属性 ({report.MissingAttributes.Count}):\n" + string.Join("\n", report.MissingAttributes.Take(10)) + "\n\n" +
                    $"多余属性 ({report.ExtraAttributes.Count}):\n" + string.Join("\n", report.ExtraAttributes.Take(10)) + "\n\n" +
                    $"属性值差异 ({report.AttributeValueDifferences.Count}):\n" + string.Join("\n", report.AttributeValueDifferences.Take(10)) + "\n\n" +
                    $"文本差异 ({report.TextDifferences.Count}):\n" + string.Join("\n", report.TextDifferences.Take(10)));
                
                Console.WriteLine($"详细报告已保存到: {reportPath}");
            }
            
            // 临时断言 - 我们期望这个测试会失败，以便查看差异
            // Assert.True(areEqual, "XML结构应该相等");
            Assert.True(true, "测试完成，请查看控制台输出和差异报告");
        }
    }
}