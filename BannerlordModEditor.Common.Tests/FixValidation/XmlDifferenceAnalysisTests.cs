using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.FixValidation
{
    /// <summary>
    /// XML差异分析测试
    /// </summary>
    public class XmlDifferenceAnalysisTests
    {
        [Fact]
        public async Task AnalyzeXmlDifferencesDetailed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = SiegeEnginesMapper.ToDTO(originalDo);
            var roundtripDo = SiegeEnginesMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // 测试标准化
            var normalizedOriginal = XmlTestUtils.NormalizeXml(xmlContent);
            var normalizedSerialized = XmlTestUtils.NormalizeXml(serializedXml);
            
            Console.WriteLine("=== 详细差异分析 ===");
            
            // 字符级别比较
            var originalLines = normalizedOriginal.Split('\n');
            var serializedLines = normalizedSerialized.Split('\n');
            
            Console.WriteLine($"原始XML行数: {originalLines.Length}");
            Console.WriteLine($"序列化XML行数: {serializedLines.Length}");
            
            // 比较每一行
            for (int i = 0; i < Math.Max(originalLines.Length, serializedLines.Length); i++)
            {
                if (i >= originalLines.Length)
                {
                    Console.WriteLine($"行 {i+1}: 只在序列化XML中存在: '{serializedLines[i]}'");
                }
                else if (i >= serializedLines.Length)
                {
                    Console.WriteLine($"行 {i+1}: 只在原始XML中存在: '{originalLines[i]}'");
                }
                else if (originalLines[i] != serializedLines[i])
                {
                    Console.WriteLine($"行 {i+1}: 不同");
                    Console.WriteLine($"  原始: '{originalLines[i]}'");
                    Console.WriteLine($"  序列化: '{serializedLines[i]}'");
                    
                    // 字符级别差异
                    var originalChars = originalLines[i].ToCharArray();
                    var serializedChars = serializedLines[i].ToCharArray();
                    
                    for (int j = 0; j < Math.Max(originalChars.Length, serializedChars.Length); j++)
                    {
                        if (j >= originalChars.Length)
                        {
                            Console.WriteLine($"    字符 {j+1}: 序列化XML多出 '{serializedChars[j]}' (ASCII: {(int)serializedChars[j]})");
                        }
                        else if (j >= serializedChars.Length)
                        {
                            Console.WriteLine($"    字符 {j+1}: 原始XML多出 '{originalChars[j]}' (ASCII: {(int)originalChars[j]})");
                        }
                        else if (originalChars[j] != serializedChars[j])
                        {
                            Console.WriteLine($"    字符 {j+1}: 不同 - 原始: '{originalChars[j]}' (ASCII: {(int)originalChars[j]}), 序列化: '{serializedChars[j]}' (ASCII: {(int)serializedChars[j]})");
                        }
                    }
                }
            }
            
            // 检查XML文档结构
            try
            {
                var doc1 = XDocument.Parse(normalizedOriginal);
                var doc2 = XDocument.Parse(normalizedSerialized);
                
                Console.WriteLine($"\n=== XML文档结构分析 ===");
                Console.WriteLine($"原始XML根元素: {doc1.Root?.Name}");
                Console.WriteLine($"序列化XML根元素: {doc2.Root?.Name}");
                
                if (doc1.Root != null && doc2.Root != null)
                {
                    var elements1 = doc1.Root.Elements().Count();
                    var elements2 = doc2.Root.Elements().Count();
                    Console.WriteLine($"原始XML子元素数量: {elements1}");
                    Console.WriteLine($"序列化XML子元素数量: {elements2}");
                    
                    // 比较每个元素
                    var elements1List = doc1.Root.Elements().ToList();
                    var elements2List = doc2.Root.Elements().ToList();
                    
                    for (int i = 0; i < Math.Max(elements1List.Count, elements2List.Count); i++)
                    {
                        if (i >= elements1List.Count)
                        {
                            Console.WriteLine($"元素 {i+1}: 只在序列化XML中存在: {elements2List[i].Name}");
                        }
                        else if (i >= elements2List.Count)
                        {
                            Console.WriteLine($"元素 {i+1}: 只在原始XML中存在: {elements1List[i].Name}");
                        }
                        else
                        {
                            var elem1 = elements1List[i];
                            var elem2 = elements2List[i];
                            
                            if (elem1.Name != elem2.Name)
                            {
                                Console.WriteLine($"元素 {i+1}: 名称不同 - 原始: {elem1.Name}, 序列化: {elem2.Name}");
                            }
                            else
                            {
                                // 比较属性
                                var attrs1 = elem1.Attributes().ToList();
                                var attrs2 = elem2.Attributes().ToList();
                                
                                if (attrs1.Count != attrs2.Count)
                                {
                                    Console.WriteLine($"元素 {i+1} ({elem1.Name}): 属性数量不同 - 原始: {attrs1.Count}, 序列化: {attrs2.Count}");
                                }
                                
                                for (int j = 0; j < Math.Max(attrs1.Count, attrs2.Count); j++)
                                {
                                    if (j >= attrs1.Count)
                                    {
                                        Console.WriteLine($"元素 {i+1} 属性 {j+1}: 只在序列化XML中存在: {attrs2[j].Name}='{attrs2[j].Value}'");
                                    }
                                    else if (j >= attrs2.Count)
                                    {
                                        Console.WriteLine($"元素 {i+1} 属性 {j+1}: 只在原始XML中存在: {attrs1[j].Name}='{attrs1[j].Value}'");
                                    }
                                    else
                                    {
                                        var attr1 = attrs1[j];
                                        var attr2 = attrs2[j];
                                        
                                        if (attr1.Name != attr2.Name)
                                        {
                                            Console.WriteLine($"元素 {i+1} 属性 {j+1}: 名称不同 - 原始: {attr1.Name}, 序列化: {attr2.Name}");
                                        }
                                        else if (attr1.Value != attr2.Value)
                                        {
                                            Console.WriteLine($"元素 {i+1} 属性 {j+1} ({attr1.Name}): 值不同 - 原始: '{attr1.Value}', 序列化: '{attr2.Value}'");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML解析错误: {ex.Message}");
            }
            
            Console.WriteLine($"\n=== 字符串比较结果 ===");
            Console.WriteLine($"标准化后是否相等: {normalizedOriginal == normalizedSerialized}");
            Console.WriteLine($"长度相同: {normalizedOriginal.Length == normalizedSerialized.Length}");
            Console.WriteLine($"原始长度: {normalizedOriginal.Length}");
            Console.WriteLine($"序列化长度: {normalizedSerialized.Length}");
            
            // 保存到文件
            await File.WriteAllTextAsync("tmp/detailed_normalized_original.xml", normalizedOriginal);
            await File.WriteAllTextAsync("tmp/detailed_normalized_serialized.xml", normalizedSerialized);
        }
    }
}