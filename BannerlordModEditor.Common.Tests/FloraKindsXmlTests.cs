using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsXmlTests
    {
        [Theory]
        [MemberData(nameof(GetFloraKindsTestFiles))]
        public void FloraKinds_LoadAndSave_ShouldBeLogicallyIdentical(string fileName)
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "FloraKinds", fileName);

            // Deserialization
            var serializer = new XmlSerializer(typeof(FloraKinds));
            FloraKinds? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as FloraKinds;
            }

            Assert.NotNull(model);
            Assert.NotNull(model.FloraKind);
            Assert.True(model.FloraKind.Count > 0);

            // Verify some sample data from the first flora kind
            var firstFloraKind = model.FloraKind[0];
            Assert.NotNull(firstFloraKind.Name);
            Assert.NotNull(firstFloraKind.ViewDistance);

            // Read original XML to determine format
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
            var hasXmlDeclaration = originalXml.StartsWith("<?xml");
            
            // 检查原始XML格式是否为紧凑格式（根元素和子元素在同一行且没有内部缩进）
            // 通过检查文件中是否包含明显的缩进字符（多个制表符或空格）来判断
            var hasIndentation = originalXml.Contains("\n\t\t") || originalXml.Contains("\n    ");
            var isCompactFormat = originalXml.Contains("<flora_kinds><flora_kind") && !hasIndentation;

            string serializedXml;
            
            if (isCompactFormat)
            {
                // 简化实现：对于紧凑格式的XML，使用自定义序列化来匹配原始格式
                // 此方法处理特殊的FloraKinds格式，其中根元素和第一个子元素在同一行
                var compactSettings = new XmlWriterSettings
                {
                    Indent = false, // 无缩进以保持紧凑格式
                    Encoding = new UTF8Encoding(false), // No BOM
                    OmitXmlDeclaration = !hasXmlDeclaration, // 保持XML声明一致性
                    NewLineHandling = System.Xml.NewLineHandling.Replace, // 保持换行符处理
                    NewLineChars = "\n" // 使用标准换行符
                };

                using (var memoryStream = new MemoryStream())
                using (var xmlWriter = XmlWriter.Create(memoryStream, compactSettings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, model, ns);
                    xmlWriter.Flush();
                    serializedXml = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                
                // 后处理：确保XML声明和根元素之间有换行符（如果原始XML有的话）
                if (hasXmlDeclaration && !serializedXml.Contains("?>\n<"))
                {
                    serializedXml = serializedXml.Replace("?>", "?>\n");
                }
            }
            else
            {
                // 完整实现：对于非紧凑格式的XML，使用标准缩进
                var settings = new XmlWriterSettings
                {
                    Indent = true, // 标准缩进
                    IndentChars = "\t", // 使用制表符缩进
                    NewLineChars = "\n", // 使用标准换行符
                    Encoding = new UTF8Encoding(false), // No BOM
                    OmitXmlDeclaration = !hasXmlDeclaration, // 保持XML声明一致性
                    NewLineOnAttributes = false // 属性在同一行
                };

                using (var memoryStream = new MemoryStream())
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, model, ns);
                    xmlWriter.Flush();
                    serializedXml = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }

            // 使用正确的XML测试模式 - 验证关键数据内容一致
            var reparsingSerializer = new XmlSerializer(typeof(FloraKinds));
            
            // 反序列化序列化后的XML
            FloraKinds reparsedFloraKinds;
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedXml)))
            {
                reparsedFloraKinds = (FloraKinds)reparsingSerializer.Deserialize(memoryStream)!;
            }
            
            Assert.NotNull(reparsedFloraKinds);
            
            // 验证基本结构
            Assert.Equal(model.FloraKind.Count, reparsedFloraKinds.FloraKind.Count);
            
            // 验证每个FloraKind的关键属性
            for (int i = 0; i < model.FloraKind.Count; i++)
            {
                var originalFloraKind = model.FloraKind[i];
                var reparsedFloraKind = reparsedFloraKinds.FloraKind[i];
                
                Assert.Equal(originalFloraKind.Name, reparsedFloraKind.Name);
                Assert.Equal(originalFloraKind.ViewDistance, reparsedFloraKind.ViewDistance);
                
                // 验证flags
                if (originalFloraKind.Flags?.Flag != null)
                {
                    Assert.NotNull(reparsedFloraKind.Flags?.Flag);
                    Assert.Equal(originalFloraKind.Flags.Flag.Count, reparsedFloraKind.Flags.Flag.Count);
                    
                    for (int j = 0; j < originalFloraKind.Flags.Flag.Count; j++)
                    {
                        Assert.Equal(originalFloraKind.Flags.Flag[j].Name, reparsedFloraKind.Flags.Flag[j].Name);
                        Assert.Equal(originalFloraKind.Flags.Flag[j].Value, reparsedFloraKind.Flags.Flag[j].Value);
                    }
                }
                
                // 验证seasonal_kind
                if (originalFloraKind.SeasonalKind != null)
                {
                    Assert.NotNull(reparsedFloraKind.SeasonalKind);
                    Assert.Equal(originalFloraKind.SeasonalKind.Count, reparsedFloraKind.SeasonalKind.Count);
                    
                    for (int j = 0; j < originalFloraKind.SeasonalKind.Count; j++)
                    {
                        var originalSeasonal = originalFloraKind.SeasonalKind[j];
                        var reparsedSeasonal = reparsedFloraKind.SeasonalKind[j];
                        
                        Assert.Equal(originalSeasonal.Season, reparsedSeasonal.Season);
                        
                        // 验证flora_variations
                        if (originalSeasonal.FloraVariations?.FloraVariation != null)
                        {
                            Assert.NotNull(reparsedSeasonal.FloraVariations?.FloraVariation);
                            Assert.Equal(originalSeasonal.FloraVariations.FloraVariation.Count, reparsedSeasonal.FloraVariations.FloraVariation.Count);
                            
                            for (int k = 0; k < originalSeasonal.FloraVariations.FloraVariation.Count; k++)
                            {
                                var originalVariation = originalSeasonal.FloraVariations.FloraVariation[k];
                                var reparsedVariation = reparsedSeasonal.FloraVariations.FloraVariation[k];
                                
                                Assert.Equal(originalVariation.Name, reparsedVariation.Name);
                                Assert.Equal(originalVariation.BodyName, reparsedVariation.BodyName);
                                Assert.Equal(originalVariation.DensityMultiplier, reparsedVariation.DensityMultiplier);
                                Assert.Equal(originalVariation.BbRadius, reparsedVariation.BbRadius);
                                
                                // 验证mesh
                                if (originalVariation.Mesh != null)
                                {
                                    Assert.NotNull(reparsedVariation.Mesh);
                                    Assert.Equal(originalVariation.Mesh.Count, reparsedVariation.Mesh.Count);
                                    
                                    for (int m = 0; m < originalVariation.Mesh.Count; m++)
                                    {
                                        Assert.Equal(originalVariation.Mesh[m].Name, reparsedVariation.Mesh[m].Name);
                                        Assert.Equal(originalVariation.Mesh[m].Material, reparsedVariation.Mesh[m].Material);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> GetFloraKindsTestFiles()
        {
            var testSubsetsDir = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "FloraKinds");
            if (Directory.Exists(testSubsetsDir))
            {
                var files = Directory.GetFiles(testSubsetsDir, "flora_kinds_part_*.xml");
                foreach (var file in files)
                {
                    yield return new object[] { Path.GetFileName(file) };
                }
            }
        }
    }
}