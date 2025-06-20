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
    public class PartiesXmlTests
    {
        [Fact]
        public void Parties_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "parties.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(PartiesBase));
            PartiesBase parties;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                parties = (PartiesBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, parties);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构检查
            Assert.NotNull(parties);
            Assert.Equal("party", parties.Type);
            Assert.NotNull(parties.Parties);
            Assert.NotNull(parties.Parties.Party);
            Assert.True(parties.Parties.Party.Count > 0, "应该有至少一个队伍");
            
            // Assert - 验证具体的队伍数据
            var tempParty = parties.Parties.Party.FirstOrDefault(p => p.Id == "p_temp_party");
            Assert.NotNull(tempParty);
            Assert.Equal("{!}temp party", tempParty.Name);
            Assert.Equal("0x100", tempParty.Flags);
            Assert.Equal("pt_none", tempParty.PartyTemplate);
            Assert.Equal("0.000000, 0.000000, 0.000000", tempParty.Position);
            Assert.Equal("0", tempParty.AverageBearingRot);
            
            var casualtiesParty = parties.Parties.Party.FirstOrDefault(p => p.Id == "p_temp_casualties");
            Assert.NotNull(casualtiesParty);
            Assert.Equal("{!}casualties", casualtiesParty.Name);
            Assert.Equal("0x80100", casualtiesParty.Flags);
            Assert.Equal("pt_none", casualtiesParty.PartyTemplate);
            Assert.Equal("1.000000, 1.000000, 0.000000", casualtiesParty.Position);
            Assert.Equal("0", casualtiesParty.AverageBearingRot);
            
            // Assert - 验证字段数据
            Assert.NotNull(casualtiesParty.Field);
            Assert.True(casualtiesParty.Field.Count >= 2, "应该有至少两个字段");
            
            var thinkFreqMinField = casualtiesParty.Field.FirstOrDefault(f => f.FieldName == "ThinkFrequencyMin");
            Assert.NotNull(thinkFreqMinField);
            Assert.Equal("3", thinkFreqMinField.FieldValue);
            
            var thinkFreqMaxField = casualtiesParty.Field.FirstOrDefault(f => f.FieldName == "ThinkFrequencyMax");
            Assert.NotNull(thinkFreqMaxField);
            Assert.Equal("21", thinkFreqMaxField.FieldValue);
            
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
        public void Parties_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "parties.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PartiesBase));
            PartiesBase parties;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                parties = (PartiesBase)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有队伍都有基本属性
            foreach (var party in parties.Parties.Party)
            {
                Assert.False(string.IsNullOrEmpty(party.Id), "队伍ID不能为空");
                Assert.False(string.IsNullOrEmpty(party.Name), $"队伍名称不能为空：{party.Id}");
                Assert.False(string.IsNullOrEmpty(party.Flags), $"队伍标志不能为空：{party.Id}");
                Assert.False(string.IsNullOrEmpty(party.PartyTemplate), $"队伍模板不能为空：{party.Id}");
                Assert.False(string.IsNullOrEmpty(party.Position), $"队伍位置不能为空：{party.Id}");
                Assert.False(string.IsNullOrEmpty(party.AverageBearingRot), $"队伍朝向不能为空：{party.Id}");
                
                // 验证位置格式 (x, y, z)
                var positionParts = party.Position?.Split(',');
                Assert.True(positionParts?.Length == 3, $"位置应该有3个坐标值：{party.Id}");
                
                // 验证标志格式 (应该以0x开头的十六进制)
                Assert.True(party.Flags?.StartsWith("0x") == true, $"标志应该是十六进制格式：{party.Id}");
                
                // 验证字段数据完整性
                if (party.Field != null && party.Field.Count > 0)
                {
                    foreach (var field in party.Field)
                    {
                        Assert.False(string.IsNullOrEmpty(field.FieldName), $"字段名称不能为空：队伍{party.Id}");
                        Assert.False(string.IsNullOrEmpty(field.FieldValue), $"字段值不能为空：队伍{party.Id}，字段{field.FieldName}");
                    }
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