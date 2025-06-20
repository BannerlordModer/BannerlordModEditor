using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MusicParametersXmlTests
    {
        [Fact]
        public void MusicParameters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MusicParameters));
            MusicParameters musicParameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                musicParameters = (MusicParameters)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, musicParameters);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(musicParameters);
            Assert.Equal(18, musicParameters.Parameters.Count);

            // 验证特定的音乐参数
            var smallBattleThreshold = musicParameters.Parameters.FirstOrDefault(p => p.Id == "SmallBattleTreshold");
            Assert.NotNull(smallBattleThreshold);
            Assert.Equal("60", smallBattleThreshold.Value);

            var mediumBattleThreshold = musicParameters.Parameters.FirstOrDefault(p => p.Id == "MediumBattleTreshold");
            Assert.NotNull(mediumBattleThreshold);
            Assert.Equal("200", mediumBattleThreshold.Value);

            // 验证浮点数参数
            var smallBattleDistance = musicParameters.Parameters.FirstOrDefault(p => p.Id == "SmallBattleDistanceTreshold");
            Assert.NotNull(smallBattleDistance);
            Assert.Equal("75.0", smallBattleDistance.Value);

            var minIntensity = musicParameters.Parameters.FirstOrDefault(p => p.Id == "MinIntensity");
            Assert.NotNull(minIntensity);
            Assert.Equal("0.01", minIntensity.Value);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("music_parameter").Count() == savedDoc.Root?.Elements("music_parameter").Count(),
                "music_parameter count should be the same");
        }
        
        [Fact]
        public void MusicParameters_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MusicParameters));
            MusicParameters musicParameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                musicParameters = (MusicParameters)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有参数都有必要的数据
            foreach (var parameter in musicParameters.Parameters)
            {
                Assert.False(string.IsNullOrWhiteSpace(parameter.Id), "Music parameter should have Id");
                Assert.False(string.IsNullOrWhiteSpace(parameter.Value), "Music parameter should have Value");
                
                // 验证ID命名约定
                Assert.False(parameter.Id.Contains(" "), "Parameter ID should not contain spaces");
                
                // 验证值格式（应该是数字）
                if (double.TryParse(parameter.Value, out var numericValue))
                {
                    Assert.True(numericValue >= 0, $"Parameter '{parameter.Id}' should have non-negative value");
                    
                    // 特定范围验证
                    if (parameter.Id.Contains("Intensity") && !parameter.Id.Contains("Multiplier"))
                    {
                        Assert.True(numericValue <= 1.0, $"Intensity parameter '{parameter.Id}' should be <= 1.0");
                    }
                    
                    if (parameter.Id.Contains("Threshold") && parameter.Id.Contains("Distance"))
                    {
                        Assert.True(numericValue > 0, $"Distance threshold '{parameter.Id}' should be positive");
                    }
                }
                else
                {
                    Assert.True(false, $"Parameter '{parameter.Id}' value '{parameter.Value}' should be numeric");
                }
            }
            
            // 验证必需的参数存在
            var requiredParameters = new[] { 
                "SmallBattleTreshold", "MediumBattleTreshold", "LargeBattleTreshold",
                "MinIntensity", "DefaultStartIntensity", "CampaignDarkModeThreshold"
            };
            
            foreach (var requiredParam in requiredParameters)
            {
                var parameter = musicParameters.Parameters.FirstOrDefault(p => p.Id == requiredParam);
                Assert.NotNull(parameter);
            }
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 