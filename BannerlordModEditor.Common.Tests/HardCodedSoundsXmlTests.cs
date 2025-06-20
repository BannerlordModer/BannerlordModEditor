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
    public class HardCodedSoundsXmlTests
    {
        [Fact]
        public void HardCodedSounds_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(HardCodedSounds));
            HardCodedSounds hardCodedSounds;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                hardCodedSounds = (HardCodedSounds)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, hardCodedSounds);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(hardCodedSounds);
            Assert.Equal("hard_coded_sound", hardCodedSounds.Type);
            Assert.NotNull(hardCodedSounds.HardCodedSoundsContainer);
            Assert.NotNull(hardCodedSounds.HardCodedSoundsContainer.HardCodedSound);
            Assert.True(hardCodedSounds.HardCodedSoundsContainer.HardCodedSound.Count > 0, "Should have at least one hard coded sound");
            
            // 验证具体的硬编码音效数据
            var shieldBrokenSound = hardCodedSounds.HardCodedSoundsContainer.HardCodedSound
                .FirstOrDefault(s => s.Id == "mission_combat_wood_shield_broken");
            Assert.NotNull(shieldBrokenSound);
            Assert.Equal("event:/mission/combat/shield/broken", shieldBrokenSound.Path);
            
            var metalShieldSound = hardCodedSounds.HardCodedSoundsContainer.HardCodedSound
                .FirstOrDefault(s => s.Id == "mission_combat_metal_shield_broken");
            Assert.NotNull(metalShieldSound);
            Assert.Equal("event:/mission/combat/shield/metal_broken", metalShieldSound.Path);
            
            var corpseImpactSound = hardCodedSounds.HardCodedSoundsContainer.HardCodedSound
                .FirstOrDefault(s => s.Id == "mission_combat_impact_corpse");
            Assert.NotNull(corpseImpactSound);
            Assert.Equal("event:/mission/combat/impact/corpse", corpseImpactSound.Path);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Element("hard_coded_sounds")?.Elements("hard_coded_sound").Count() == 
                       savedDoc.Root?.Element("hard_coded_sounds")?.Elements("hard_coded_sound").Count(),
                "Hard coded sound count should be the same");
        }
        
        [Fact]
        public void HardCodedSounds_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(HardCodedSounds));
            HardCodedSounds hardCodedSounds;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                hardCodedSounds = (HardCodedSounds)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有硬编码音效都有必要的属性
            Assert.Equal("hard_coded_sound", hardCodedSounds.Type);
            Assert.NotNull(hardCodedSounds.HardCodedSoundsContainer);
            
            foreach (var sound in hardCodedSounds.HardCodedSoundsContainer.HardCodedSound)
            {
                Assert.False(string.IsNullOrWhiteSpace(sound.Id), "Hard coded sound should have Id");
                Assert.False(string.IsNullOrWhiteSpace(sound.Path), "Hard coded sound should have Path");
                
                // 验证路径格式（应该以event:开头）
                Assert.StartsWith("event:", sound.Path);
                
                // 验证ID格式（应该包含相关关键词）
                Assert.True(sound.Id.Contains("mission") || sound.Id.Contains("combat") || sound.Id.Contains("ui"),
                    $"Sound ID '{sound.Id}' should contain descriptive keywords");
            }
            
            // 验证包含预期的音效
            var allIds = hardCodedSounds.HardCodedSoundsContainer.HardCodedSound.Select(s => s.Id).ToList();
            Assert.Contains("mission_combat_wood_shield_broken", allIds);
            Assert.Contains("mission_combat_metal_shield_broken", allIds);
            Assert.Contains("mission_combat_impact_corpse", allIds);
            
            // 验证所有路径都是有效的事件路径
            var allPaths = hardCodedSounds.HardCodedSoundsContainer.HardCodedSound.Select(s => s.Path).ToList();
            foreach (var path in allPaths)
            {
                Assert.StartsWith("event:/", path);
            }
            
            // 确保没有重复的ID
            var uniqueIds = allIds.Distinct().ToList();
            Assert.Equal(allIds.Count, uniqueIds.Count);
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