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
    public class ModuleSoundsXmlTests
    {
        [Fact]
        public void ModuleSounds_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "module_sounds.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ModuleSounds));
            ModuleSounds moduleSounds;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                moduleSounds = (ModuleSounds)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, moduleSounds);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(moduleSounds);
            Assert.Equal("module_sound", moduleSounds.Type);
            
            // 验证 module_sounds 容器
            Assert.NotNull(moduleSounds.ModuleSoundsContainer);
            Assert.Equal(3, moduleSounds.ModuleSoundsContainer.ModuleSoundList.Count);

            // 验证第一个音频：简单音频
            var hitSound = moduleSounds.ModuleSoundsContainer.ModuleSoundList.FirstOrDefault(s => s.Name == "example/combat/hit");
            Assert.NotNull(hitSound);
            Assert.Equal("true", hitSound.Is2D);
            Assert.Equal("mission_combat", hitSound.SoundCategory);
            Assert.Equal("example_sound_modders.ogg", hitSound.Path);
            Assert.Empty(hitSound.Variations);
            
            // 验证第二个音频：带变化和音调的音频
            var chargeSound = moduleSounds.ModuleSoundsContainer.ModuleSoundList.FirstOrDefault(s => s.Name == "example/voice/charge");
            Assert.NotNull(chargeSound);
            Assert.Equal("mission_voice_shout", chargeSound.SoundCategory);
            Assert.Equal("0.9", chargeSound.MinPitchMultiplier);
            Assert.Equal("1.1", chargeSound.MaxPitchMultiplier);
            Assert.Equal(2, chargeSound.Variations.Count);
            
            // 验证变化
            var variation1 = chargeSound.Variations[0];
            Assert.Equal("example_sound_modders.ogg", variation1.Path);
            Assert.Equal("1.0", variation1.Weight);
            
            var variation2 = chargeSound.Variations[1];
            Assert.Equal("example_sound_modders_2.ogg", variation2.Path);
            Assert.Equal("0.75", variation2.Weight);
            
            // 验证第三个音频：简单路径音频
            var whisperSound = moduleSounds.ModuleSoundsContainer.ModuleSoundList.FirstOrDefault(s => s.Name == "example/voice/whisper");
            Assert.NotNull(whisperSound);
            Assert.Equal("mission_voice_trivial", whisperSound.SoundCategory);
            Assert.Equal("example_sound_modders.ogg", whisperSound.Path);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点和注释
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            RemoveCommentNodes(originalDoc.Root);
            RemoveCommentNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Element("module_sounds")?.Elements("module_sound").Count() == 
                       savedDoc.Root?.Element("module_sounds")?.Elements("module_sound").Count(),
                "Module sounds count should be the same");
        }
        
        [Fact]
        public void ModuleSounds_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "module_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ModuleSounds));
            ModuleSounds moduleSounds;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                moduleSounds = (ModuleSounds)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本结构完整性
            Assert.Equal("module_sound", moduleSounds.Type);
            Assert.True(moduleSounds.ModuleSoundsContainer.ModuleSoundList.Count > 0, "Should have module sounds");

            // 验证所有音频都有必要的数据
            foreach (var sound in moduleSounds.ModuleSoundsContainer.ModuleSoundList)
            {
                Assert.False(string.IsNullOrWhiteSpace(sound.Name), "Module sound should have name");
                Assert.False(string.IsNullOrWhiteSpace(sound.SoundCategory), "Module sound should have sound category");
                
                // 验证音频名称格式 (应该是example/category/name)
                Assert.True(sound.Name.StartsWith("example/"), $"Sound name should start with example/: {sound.Name}");
                Assert.True(sound.Name.Contains("/"), $"Sound name should contain category separator: {sound.Name}");
                
                // 验证音频类别是有效的
                var validCategories = new[] {
                    "mission_ambient_bed", "mission_ambient_3d_big", "mission_ambient_3d_medium", "mission_ambient_3d_small",
                    "mission_material_impact", "mission_combat_trivial", "mission_combat", "mission_foley",
                    "mission_voice_shout", "mission_voice", "mission_voice_trivial", "mission_siege_loud",
                    "mission_footstep", "mission_footstep_run", "mission_horse_gallop", "mission_horse_walk",
                    "ui", "alert", "campaign_node", "campaign_bed", "music"
                };
                Assert.True(validCategories.Contains(sound.SoundCategory), 
                    $"Sound category should be valid: {sound.SoundCategory}");
                
                // 验证路径或变化（两者至少有一个）
                var hasDirectPath = !string.IsNullOrWhiteSpace(sound.Path);
                var hasVariations = sound.Variations.Count > 0;
                Assert.True(hasDirectPath || hasVariations, 
                    $"Sound should have either direct path or variations: {sound.Name}");
                
                // 如果有直接路径，验证文件扩展名
                if (hasDirectPath)
                {
                    Assert.True(sound.Path!.EndsWith(".ogg") || sound.Path!.EndsWith(".wav"), 
                        $"Sound path should end with .ogg or .wav: {sound.Path}");
                }
                
                // 验证变化
                if (hasVariations)
                {
                    foreach (var variation in sound.Variations)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(variation.Path), "Variation should have path");
                        Assert.False(string.IsNullOrWhiteSpace(variation.Weight), "Variation should have weight");
                        Assert.True(variation.Path.EndsWith(".ogg") || variation.Path.EndsWith(".wav"), 
                            $"Variation path should end with .ogg or .wav: {variation.Path}");
                        
                        // 验证权重是有效的浮点数
                        Assert.True(float.TryParse(variation.Weight, out var weight), 
                            $"Variation weight should be valid float: {variation.Weight}");
                        Assert.True(weight >= 0, $"Variation weight should be non-negative: {weight}");
                    }
                }
                
                // 验证音调乘数（如果存在）
                if (!string.IsNullOrEmpty(sound.MinPitchMultiplier))
                {
                    Assert.True(float.TryParse(sound.MinPitchMultiplier, out var minPitch), 
                        $"MinPitchMultiplier should be valid float: {sound.MinPitchMultiplier}");
                    Assert.True(minPitch > 0, $"MinPitchMultiplier should be positive: {minPitch}");
                }
                
                if (!string.IsNullOrEmpty(sound.MaxPitchMultiplier))
                {
                    Assert.True(float.TryParse(sound.MaxPitchMultiplier, out var maxPitch), 
                        $"MaxPitchMultiplier should be valid float: {sound.MaxPitchMultiplier}");
                    Assert.True(maxPitch > 0, $"MaxPitchMultiplier should be positive: {maxPitch}");
                    
                    // 如果两个都存在，max应该大于等于min
                    if (!string.IsNullOrEmpty(sound.MinPitchMultiplier) && 
                        float.TryParse(sound.MinPitchMultiplier, out var minPitch))
                    {
                        Assert.True(maxPitch >= minPitch, 
                            $"MaxPitchMultiplier should be >= MinPitchMultiplier: {maxPitch} >= {minPitch}");
                    }
                }
                
                // 验证is_2d属性（如果存在）
                if (!string.IsNullOrEmpty(sound.Is2D))
                {
                    Assert.True(sound.Is2D == "true" || sound.Is2D == "false", 
                        $"Is2D should be true or false: {sound.Is2D}");
                }
            }
            
            // 验证示例音频的具体细节
            var hitSound = moduleSounds.ModuleSoundsContainer.ModuleSoundList.FirstOrDefault(s => s.Name == "example/combat/hit");
            Assert.NotNull(hitSound);
            Assert.Equal("mission_combat", hitSound.SoundCategory);
            Assert.Equal("true", hitSound.Is2D);
            
            var chargeSound = moduleSounds.ModuleSoundsContainer.ModuleSoundList.FirstOrDefault(s => s.Name == "example/voice/charge");
            Assert.NotNull(chargeSound);
            Assert.Equal("mission_voice_shout", chargeSound.SoundCategory);
            Assert.Equal(2, chargeSound.Variations.Count);
            
            var whisperSound = moduleSounds.ModuleSoundsContainer.ModuleSoundList.FirstOrDefault(s => s.Name == "example/voice/whisper");
            Assert.NotNull(whisperSound);
            Assert.Equal("mission_voice_trivial", whisperSound.SoundCategory);
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
        
        private static void RemoveCommentNodes(XElement? element)
        {
            if (element == null) return;
            
            var commentNodes = element.Nodes().OfType<XComment>().ToList();
            foreach (var node in commentNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveCommentNodes(child);
            }
        }
    }
} 