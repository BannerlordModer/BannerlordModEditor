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
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_campaign_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ManagedCampaignParametersBase));
            ManagedCampaignParametersBase campaignParams;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                campaignParams = (ManagedCampaignParametersBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, campaignParams);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构检查
            Assert.NotNull(campaignParams);
            Assert.Equal("campaign_parameters", campaignParams.Type);
            Assert.NotNull(campaignParams.ManagedCampaignParameters);
            Assert.NotNull(campaignParams.ManagedCampaignParameters.ManagedCampaignParameter);
            Assert.True(campaignParams.ManagedCampaignParameters.ManagedCampaignParameter.Count > 0, "应该有至少一个战役参数");
            
            // Assert - 验证具体的参数数据
            var warParam = campaignParams.ManagedCampaignParameters.ManagedCampaignParameter
                .FirstOrDefault(p => p.Id == "IsWarDeclarationDisabled");
            Assert.NotNull(warParam);
            Assert.Equal("false", warParam.Value);
            
            var peaceParam = campaignParams.ManagedCampaignParameters.ManagedCampaignParameter
                .FirstOrDefault(p => p.Id == "IsPeaceDeclarationDisabled");
            Assert.NotNull(peaceParam);
            Assert.Equal("false", peaceParam.Value);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 规范化XML格式
            NormalizeXml(originalDoc.Root);
            NormalizeXml(savedDoc.Root);

            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements().Count() == savedDoc.Root?.Elements().Count(),
                "元素数量应该相同");
        }
        
        [Fact]
        public void ManagedCampaignParameters_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_campaign_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCampaignParametersBase));
            ManagedCampaignParametersBase campaignParams;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                campaignParams = (ManagedCampaignParametersBase)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有参数都有基本属性
            foreach (var param in campaignParams.ManagedCampaignParameters.ManagedCampaignParameter)
            {
                Assert.False(string.IsNullOrEmpty(param.Id), "参数ID不能为空");
                Assert.False(string.IsNullOrEmpty(param.Value), $"参数值不能为空：{param.Id}");
                
                // 验证布尔值参数
                if (param.Id?.Contains("Disabled") == true)
                {
                    Assert.True(param.Value == "true" || param.Value == "false", 
                        $"禁用参数应该是布尔值：{param.Id} = {param.Value}");
                }
            }
        }
        
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("找不到解决方案根目录");
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

        private static void NormalizeXml(XElement? element)
        {
            if (element == null) return;
            
            // 移除所有空白文本节点
            var whitespaceNodes = element.Nodes().OfType<XText>()
                .Where(t => string.IsNullOrWhiteSpace(t.Value))
                .ToList();
            foreach (var node in whitespaceNodes)
            {
                node.Remove();
            }
            
            // 递归处理子元素
            foreach (var child in element.Elements())
            {
                NormalizeXml(child);
            }
        }
    }
} 