using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.FixValidation
{
    /// <summary>
    /// SiegeEngines往返测试诊断
    /// </summary>
    public class SiegeEnginesDiagnosticTests
    {
        [Fact]
        public async Task DiagnoseSiegeEnginesRoundTripIssue()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            Console.WriteLine("=== 原始XML ===");
            Console.WriteLine(xmlContent);
            Console.WriteLine();
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = SiegeEnginesMapper.ToDTO(originalDo);
            var roundtripDo = SiegeEnginesMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            Console.WriteLine("=== 序列化后的XML ===");
            Console.WriteLine(serializedXml);
            Console.WriteLine();
            
            // 分析差异
            Console.WriteLine("=== 差异分析 ===");
            AnalyzeXmlDifferences(xmlContent, serializedXml);
            
            // 检查HasEmptySiegeEngines标志
            Console.WriteLine($"HasEmptySiegeEngines: {roundtripDo.HasEmptySiegeEngines}");
            Console.WriteLine($"SiegeEngines.Count: {roundtripDo.SiegeEngines.Count}");
            
            // 检查ShouldSerialize方法
            Console.WriteLine($"ShouldSerializeSiegeEngines(): {roundtripDo.ShouldSerializeSiegeEngines()}");
            
            // 保存到文件以便手动检查
            await File.WriteAllTextAsync("tmp/original_siegeengines.xml", xmlContent);
            await File.WriteAllTextAsync("tmp/serialized_siegeengines.xml", serializedXml);
        }
        
        [Fact]
        public async Task TestNormalization()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            var dto = SiegeEnginesMapper.ToDTO(originalDo);
            var roundtripDo = SiegeEnginesMapper.ToDO(dto);
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // 测试标准化
            var normalizedOriginal = XmlTestUtils.NormalizeXml(xmlContent);
            var normalizedSerialized = XmlTestUtils.NormalizeXml(serializedXml);
            
            Console.WriteLine("=== 标准化后的原始XML ===");
            Console.WriteLine(normalizedOriginal);
            Console.WriteLine();
            
            Console.WriteLine("=== 标准化后的序列化XML ===");
            Console.WriteLine(normalizedSerialized);
            Console.WriteLine();
            
            Console.WriteLine($"标准化后是否相等: {normalizedOriginal == normalizedSerialized}");
            
            await File.WriteAllTextAsync("tmp/normalized_original.xml", normalizedOriginal);
            await File.WriteAllTextAsync("tmp/normalized_serialized.xml", normalizedSerialized);
        }
        
        private void AnalyzeXmlDifferences(string xml1, string xml2)
        {
            try
            {
                var doc1 = XDocument.Parse(xml1);
                var doc2 = XDocument.Parse(xml2);
                
                var elements1 = doc1.Descendants().Count();
                var elements2 = doc2.Descendants().Count();
                
                var attributes1 = doc1.Descendants().Sum(e => e.Attributes().Count());
                var attributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
                
                Console.WriteLine($"元素数量: 原始={elements1}, 序列化={elements2}");
                Console.WriteLine($"属性数量: 原始={attributes1}, 序列化={attributes2}");
                
                // 检查根元素
                if (doc1.Root != null && doc2.Root != null)
                {
                    Console.WriteLine($"根元素名称: 原始={doc1.Root.Name}, 序列化={doc2.Root.Name}");
                    
                    // 检查SiegeEngineType元素数量
                    var siegeEngines1 = doc1.Root.Elements("SiegeEngineType").Count();
                    var siegeEngines2 = doc2.Root.Elements("SiegeEngineType").Count();
                    Console.WriteLine($"SiegeEngineType数量: 原始={siegeEngines1}, 序列化={siegeEngines2}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"分析差异时出错: {ex.Message}");
            }
        }
    }
}