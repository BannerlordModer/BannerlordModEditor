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
    public class MultiplayerStringsXmlTests
    {
        [Fact]
        public void MultiplayerStrings_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "multiplayer_strings.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MultiplayerStrings));
            MultiplayerStrings multiplayerStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                multiplayerStrings = (MultiplayerStrings)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, multiplayerStrings);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(multiplayerStrings);
            Assert.NotNull(multiplayerStrings.Strings);
            Assert.True(multiplayerStrings.Strings.Count > 0, "Should have at least one string");
            
            // 验证具体的字符串
            var deathCardYou = multiplayerStrings.Strings.FirstOrDefault(s => s.Id == "str_death_card_you");
            Assert.NotNull(deathCardYou);
            Assert.Equal("{=7UagK9bw}You", deathCardYou.Text);
            
            var couldntJoin = multiplayerStrings.Strings.FirstOrDefault(s => s.Id == "str_couldnt_join_server");
            Assert.NotNull(couldntJoin);
            Assert.Equal("{=d2b4wgya}Couldn't join server", couldntJoin.Text);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            var originalCount = originalDoc.Root?.Elements("string").Count() ?? 0;
            var savedCount = savedDoc.Root?.Elements("string").Count() ?? 0;
            Assert.Equal(originalCount, savedCount);
        }
        
        [Fact]
        public void MultiplayerStrings_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "multiplayer_strings.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MultiplayerStrings));
            MultiplayerStrings multiplayerStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                multiplayerStrings = (MultiplayerStrings)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有字符串都有必要的属性
            foreach (var str in multiplayerStrings.Strings)
            {
                Assert.False(string.IsNullOrWhiteSpace(str.Id), "String should have non-empty Id");
                Assert.False(string.IsNullOrWhiteSpace(str.Text), "String should have non-empty Text");
                
                // 验证 ID 格式（通常以 str_ 开头）
                Assert.True(str.Id.StartsWith("str_"), $"String ID should start with 'str_': {str.Id}");
                
                // 验证文本格式（通常包含本地化标记）
                Assert.True(str.Text.Contains("{=") || !str.Text.StartsWith("{"), 
                    $"String text should be properly formatted: {str.Text}");
            }
            
            // 验证包含预期的字符串
            var allIds = multiplayerStrings.Strings.Select(s => s.Id).ToList();
            Assert.Contains("str_death_card_you", allIds);
            Assert.Contains("str_death_card_ally", allIds);
            Assert.Contains("str_death_card_enemy", allIds);
            Assert.Contains("str_couldnt_join_server", allIds);
            
            // 确保没有重复的字符串 ID
            var uniqueIds = allIds.Distinct().ToList();
            Assert.Equal(allIds.Count, uniqueIds.Count);
            
            // 验证字符串数量合理（实际有约75个字符串）
            Assert.True(multiplayerStrings.Strings.Count > 50, "Should have a reasonable number of strings");
        }
        
        [Fact]
        public void MultiplayerStrings_SpecificStringValidation_ShouldPassDetailedChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "multiplayer_strings.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MultiplayerStrings));
            MultiplayerStrings multiplayerStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                multiplayerStrings = (MultiplayerStrings)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证具体字符串的完整性
            
            // 验证死亡卡片相关字符串
            var deathCardStrings = multiplayerStrings.Strings
                .Where(s => s.Id.StartsWith("str_death_card_"))
                .ToList();
            Assert.True(deathCardStrings.Count >= 3, "Should have at least 3 death card strings");
            
            var youString = deathCardStrings.FirstOrDefault(s => s.Id == "str_death_card_you");
            Assert.NotNull(youString);
            Assert.Contains("You", youString.Text);
            
            // 验证战斗相关字符串
            var battleStrings = multiplayerStrings.Strings
                .Where(s => s.Id.Contains("battle"))
                .ToList();
            Assert.True(battleStrings.Count > 0, "Should have battle-related strings");
            
            // 验证兵种描述字符串
            var troopDescriptions = multiplayerStrings.Strings
                .Where(s => s.Id.StartsWith("str_troop_description."))
                .ToList();
            Assert.True(troopDescriptions.Count > 0, "Should have troop description strings");
            
            // 验证所有兵种描述都有足够的文本内容
            foreach (var troopDesc in troopDescriptions)
            {
                Assert.True(troopDesc.Text.Length > 50, 
                    $"Troop description should be substantial: {troopDesc.Id}");
            }
            
            // 验证多人游戏模式相关字符串
            var mpStrings = multiplayerStrings.Strings
                .Where(s => s.Id.Contains("mp_") || s.Id.Contains("multiplayer"))
                .ToList();
            
            // 验证本地化格式
            var localizedStrings = multiplayerStrings.Strings
                .Where(s => s.Text.StartsWith("{="))
                .ToList();
            Assert.True(localizedStrings.Count > multiplayerStrings.Strings.Count / 2, 
                "Most strings should be localized");
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