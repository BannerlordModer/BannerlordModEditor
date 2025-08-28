using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.Multiplayer;
using BannerlordModEditor.Common.Models.DTO.Multiplayer;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.FixValidation
{
    /// <summary>
    /// TauntUsageSets往返测试诊断
    /// </summary>
    public class TauntUsageSetsDiagnosticTests
    {
        [Fact]
        public void DiagnoseTauntUsageSetsIssue()
        {
            // Arrange - 查找测试数据文件
            string testDataDir = Path.Combine("TestData");
            string[] possibleFiles = {
                "taunt_usage_sets.xml",
                "tauntusage.xml", 
                "tauntusagesets.xml"
            };
            
            string xmlContent = null;
            foreach (var file in possibleFiles)
            {
                string filePath = Path.Combine(testDataDir, file);
                if (File.Exists(filePath))
                {
                    xmlContent = File.ReadAllText(filePath);
                    Console.WriteLine($"找到测试文件: {file}");
                    break;
                }
            }
            
            if (xmlContent == null)
            {
                // 查看测试文件了解实际使用的数据
                Console.WriteLine("未找到测试数据文件，请查看TauntUsageSetsTests.cs了解测试数据");
                return;
            }
            
            Console.WriteLine("=== 原始XML信息 ===");
            Console.WriteLine($"文件长度: {xmlContent.Length}");
            Console.WriteLine($"行数: {xmlContent.Split('\n').Length}");
            
            // 只显示前几行
            var firstLines = xmlContent.Split('\n').Take(10).ToArray();
            Console.WriteLine("前10行:");
            for (int i = 0; i < firstLines.Length; i++)
            {
                Console.WriteLine($"{i+1}: {firstLines[i]}");
            }
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = TauntUsageSetsMapper.ToDTO(originalDo);
            var roundtripDo = TauntUsageSetsMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            Console.WriteLine("=== 序列化后的XML信息 ===");
            Console.WriteLine($"序列化长度: {serializedXml.Length}");
            Console.WriteLine($"序列化行数: {serializedXml.Split('\n').Length}");
            Console.WriteLine($"长度差异: {xmlContent.Length - serializedXml.Length}");
            
            // 测试标准化
            var normalizedOriginal = XmlTestUtils.NormalizeXml(xmlContent);
            var normalizedSerialized = XmlTestUtils.NormalizeXml(serializedXml);
            
            Console.WriteLine($"标准化后是否相等: {normalizedOriginal == normalizedSerialized}");
            Console.WriteLine($"标准化原始长度: {normalizedOriginal.Length}");
            Console.WriteLine($"标准化序列化长度: {normalizedSerialized.Length}");
            Console.WriteLine($"标准化长度差异: {normalizedOriginal.Length - normalizedSerialized.Length}");
            
            // 保存到文件
            File.WriteAllText("tmp/tauntusage_original.xml", xmlContent);
            File.WriteAllText("tmp/tauntusage_serialized.xml", serializedXml);
            File.WriteAllText("tmp/tauntusage_normalized_original.xml", normalizedOriginal);
            File.WriteAllText("tmp/tauntusage_normalized_serialized.xml", normalizedSerialized);
        }
    }
}