using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DO.Language;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.FixValidation
{
    /// <summary>
    /// LanguageBase往返测试诊断
    /// </summary>
    public class LanguageBaseDiagnosticTests
    {
        [Fact]
        public void DiagnoseLanguageBaseEmptyTagsIssue()
        {
            // Arrange
            string xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
  </tags>
</base>";
            
            Console.WriteLine("=== 原始XML ===");
            Console.WriteLine(xmlContent);
            Console.WriteLine();
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = LanguageBaseMapper.ToDTO(originalDo);
            var roundtripDo = LanguageBaseMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            Console.WriteLine("=== 序列化后的XML ===");
            Console.WriteLine(serializedXml);
            Console.WriteLine();
            
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
            Console.WriteLine($"原始长度: {xmlContent.Length}");
            Console.WriteLine($"序列化长度: {serializedXml.Length}");
            Console.WriteLine($"标准化原始长度: {normalizedOriginal.Length}");
            Console.WriteLine($"标准化序列化长度: {normalizedSerialized.Length}");
            
            // 保存到文件
            File.WriteAllText("tmp/languagebase_original.xml", xmlContent);
            File.WriteAllText("tmp/languagebase_serialized.xml", serializedXml);
            File.WriteAllText("tmp/languagebase_normalized_original.xml", normalizedOriginal);
            File.WriteAllText("tmp/languagebase_normalized_serialized.xml", normalizedSerialized);
        }
        
        [Fact]
        public void DiagnoseLanguageBaseStringsIssue()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "language_data.xml");
            string xmlContent = File.ReadAllText(xmlPath);
            
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
            var originalDo = XmlTestUtils.Deserialize<LanguageDataDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = LanguageDataMapper.ToDTO(originalDo);
            var roundtripDo = LanguageDataMapper.ToDO(dto);
            
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
        }
    }
}