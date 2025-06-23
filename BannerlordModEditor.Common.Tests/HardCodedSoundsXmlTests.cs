using BannerlordModEditor.Common.Models.Data;
using System;
using System.Collections.Generic;
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
        public void HardCodedSounds_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));
            HardCodedSoundsRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("hard_coded_sound", result.Type);
            Assert.NotNull(result.HardCodedSounds);
            Assert.NotNull(result.HardCodedSounds.HardCodedSound);
            Assert.Equal(3, result.HardCodedSounds.HardCodedSound.Length);
            
            // 验证具体的硬编码音效数据
            var shieldBrokenSound = result.HardCodedSounds.HardCodedSound
                .FirstOrDefault(s => s.Id == "mission_combat_wood_shield_broken");
            Assert.NotNull(shieldBrokenSound);
            Assert.Equal("event:/mission/combat/shield/broken", shieldBrokenSound.Path);
            
            var metalShieldSound = result.HardCodedSounds.HardCodedSound
                .FirstOrDefault(s => s.Id == "mission_combat_metal_shield_broken");
            Assert.NotNull(metalShieldSound);
            Assert.Equal("event:/mission/combat/shield/metal_broken", metalShieldSound.Path);
            
            var corpseImpactSound = result.HardCodedSounds.HardCodedSound
                .FirstOrDefault(s => s.Id == "mission_combat_impact_corpse");
            Assert.NotNull(corpseImpactSound);
            Assert.Equal("event:/mission/combat/impact/corpse", corpseImpactSound.Path);
        }
        
        [Fact]
        public void HardCodedSounds_ValidateDataIntegrity()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));
            HardCodedSoundsRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有硬编码音效都有必要的属性
            Assert.Equal("hard_coded_sound", result.Type);
            Assert.NotNull(result.HardCodedSounds);
            Assert.NotNull(result.HardCodedSounds.HardCodedSound);
            Assert.True(result.HardCodedSounds.HardCodedSound.Length > 0);
            
            foreach (var sound in result.HardCodedSounds.HardCodedSound)
            {
                Assert.False(string.IsNullOrWhiteSpace(sound.Id), "Hard coded sound should have Id");
                Assert.False(string.IsNullOrWhiteSpace(sound.Path), "Hard coded sound should have Path");
                
                // 验证路径格式（应该以event:开头）
                Assert.StartsWith("event:", sound.Path);
                
                // 验证ID格式（应该包含相关关键词）
                Assert.True(sound.Id.Contains("mission") || sound.Id.Contains("combat") || sound.Id.Contains("ui"),
                    $"Sound ID '{sound.Id}' should contain descriptive keywords");
            }
        }
        
        [Fact]
        public void HardCodedSounds_ValidateEventPathFormats()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));
            HardCodedSoundsRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有路径都是有效的FMOD事件路径
            foreach (var sound in result.HardCodedSounds!.HardCodedSound!)
            {
                Assert.StartsWith("event:/", sound.Path);
                Assert.DoesNotContain(" ", sound.Path); // 路径不应包含空格
                Assert.DoesNotContain("\\", sound.Path); // 应使用正斜杠
                
                // 验证路径结构合理（包含至少两个层级）
                var pathParts = sound.Path.Replace("event:/", "").Split('/');
                Assert.True(pathParts.Length >= 2, $"Event path '{sound.Path}' should have at least 2 levels");
                
                // 验证所有路径部分都非空
                foreach (var part in pathParts)
                {
                    Assert.False(string.IsNullOrWhiteSpace(part), $"Path part should not be empty in '{sound.Path}'");
                }
            }
        }
        
        [Fact]
        public void HardCodedSounds_ValidateIdUniqueness()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));
            HardCodedSoundsRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证ID唯一性
            var allIds = result.HardCodedSounds!.HardCodedSound!.Select(s => s.Id).ToList();
            var uniqueIds = allIds.Distinct().ToList();
            Assert.Equal(allIds.Count, uniqueIds.Count);
            
            // 验证路径唯一性
            var allPaths = result.HardCodedSounds.HardCodedSound.Select(s => s.Path).ToList();
            var uniquePaths = allPaths.Distinct().ToList();
            Assert.Equal(allPaths.Count, uniquePaths.Count);
        }
        
        [Fact]
        public void HardCodedSounds_ValidateSpecificSounds()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));
            HardCodedSoundsRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证预期的关键音效存在
            var soundDict = result.HardCodedSounds!.HardCodedSound!.ToDictionary(s => s.Id, s => s.Path);
            
            // 验证盾牌破碎音效
            Assert.True(soundDict.ContainsKey("mission_combat_wood_shield_broken"));
            Assert.Equal("event:/mission/combat/shield/broken", soundDict["mission_combat_wood_shield_broken"]);
            
            Assert.True(soundDict.ContainsKey("mission_combat_metal_shield_broken"));
            Assert.Equal("event:/mission/combat/shield/metal_broken", soundDict["mission_combat_metal_shield_broken"]);
            
            // 验证尸体撞击音效
            Assert.True(soundDict.ContainsKey("mission_combat_impact_corpse"));
            Assert.Equal("event:/mission/combat/impact/corpse", soundDict["mission_combat_impact_corpse"]);
            
            // 验证音效分类合理
            var combatSounds = soundDict.Keys.Where(id => id.Contains("combat")).Count();
            Assert.True(combatSounds >= 3, "Should have at least 3 combat-related sounds");
        }
        
        [Fact]
        public void HardCodedSounds_CanRoundtripXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "hard_coded_sounds.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));
            HardCodedSoundsRoot original;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                original = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string serializedXml;
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
                    serializer.Serialize(xmlWriter, original);
                }
                serializedXml = writer.ToString();
            }
            
            // Act - 重新反序列化
            HardCodedSoundsRoot roundTripped;
            using (var reader = new StringReader(serializedXml))
            {
                roundTripped = (HardCodedSoundsRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.Equal(original.Type, roundTripped.Type);
            Assert.Equal(original.HardCodedSounds?.HardCodedSound?.Length, 
                        roundTripped.HardCodedSounds?.HardCodedSound?.Length);
            
            // 验证每个音效都保持不变
            var originalDict = original.HardCodedSounds!.HardCodedSound!.ToDictionary(s => s.Id, s => s.Path);
            var roundTrippedDict = roundTripped.HardCodedSounds!.HardCodedSound!.ToDictionary(s => s.Id, s => s.Path);
            
            Assert.Equal(originalDict.Count, roundTrippedDict.Count);
            foreach (var kvp in originalDict)
            {
                Assert.True(roundTrippedDict.ContainsKey(kvp.Key!));
                Assert.Equal(kvp.Value, roundTrippedDict[kvp.Key!]);
            }
        }
        
        [Fact]
        public void HardCodedSounds_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""hard_coded_sound"">
    <hard_coded_sounds>
    </hard_coded_sounds>
</base>";

            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));

            // Act
            HardCodedSoundsRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (HardCodedSoundsRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("hard_coded_sound", result.Type);
            Assert.NotNull(result.HardCodedSounds);
            // 空的音效数组应该为null或空数组
            Assert.True(result.HardCodedSounds.HardCodedSound == null || 
                       result.HardCodedSounds.HardCodedSound.Length == 0);
        }
        
        [Fact]
        public void HardCodedSounds_ValidateXmlFormatting()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""hard_coded_sound"">
    <hard_coded_sounds>
        <hard_coded_sound id=""test_sound"" path=""event:/test/path"" />
    </hard_coded_sounds>
</base>";

            var serializer = new XmlSerializer(typeof(HardCodedSoundsRoot));

            // Act
            HardCodedSoundsRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (HardCodedSoundsRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("hard_coded_sound", result.Type);
            Assert.NotNull(result.HardCodedSounds);
            Assert.Single(result.HardCodedSounds.HardCodedSound!);
            Assert.Equal("test_sound", result.HardCodedSounds.HardCodedSound[0].Id);
            Assert.Equal("event:/test/path", result.HardCodedSounds.HardCodedSound[0].Path);
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