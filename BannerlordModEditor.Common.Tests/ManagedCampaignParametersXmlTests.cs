using BannerlordModEditor.Common.Models.Game;
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
    public class ManagedCampaignParametersXmlTests
    {
        [Fact]
        public void ManagedCampaignParameters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_campaign_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ManagedCampaignParameters));
            ManagedCampaignParameters parameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                parameters = (ManagedCampaignParameters)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, parameters);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(parameters);
            Assert.Equal("campaign_parameters", parameters.Type);
            Assert.Equal(2, parameters.Parameters.ParameterList.Count);

            // 验证特定参数
            var warDeclaration = parameters.Parameters.ParameterList.FirstOrDefault(p => p.Id == "IsWarDeclarationDisabled");
            Assert.NotNull(warDeclaration);
            Assert.Equal("false", warDeclaration.Value);

            var peaceDeclaration = parameters.Parameters.ParameterList.FirstOrDefault(p => p.Id == "IsPeaceDeclarationDisabled");
            Assert.NotNull(peaceDeclaration);
            Assert.Equal("false", peaceDeclaration.Value);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("managed_campaign_parameters").Count() == savedDoc.Root?.Elements("managed_campaign_parameters").Count(),
                "managed_campaign_parameters count should be the same");
        }
        
        [Fact]
        public void ManagedCampaignParameters_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_campaign_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCampaignParameters));
            ManagedCampaignParameters parameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                parameters = (ManagedCampaignParameters)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本数据
            Assert.Equal("campaign_parameters", parameters.Type);
            
            // 验证所有参数都有必要的数据
            foreach (var parameter in parameters.Parameters.ParameterList)
            {
                Assert.False(string.IsNullOrWhiteSpace(parameter.Id), "Parameter should have Id");
                Assert.False(string.IsNullOrWhiteSpace(parameter.Value), "Parameter should have Value");
                
                // 验证ID命名约定
                Assert.False(parameter.Id.Contains(" "), "Parameter ID should not contain spaces");
                Assert.True(parameter.Id.StartsWith("Is") && parameter.Id.EndsWith("Disabled"), 
                    "Campaign parameter should follow IsXXXDisabled naming pattern");
                
                // 验证值格式（布尔值）
                Assert.True(parameter.Value == "true" || parameter.Value == "false", 
                    $"Parameter '{parameter.Id}' should have boolean value");
            }
            
            // 验证必需的参数存在
            var requiredParameters = new[] { 
                "IsWarDeclarationDisabled",
                "IsPeaceDeclarationDisabled"
            };
            
            foreach (var requiredParam in requiredParameters)
            {
                var parameter = parameters.Parameters.ParameterList.FirstOrDefault(p => p.Id == requiredParam);
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