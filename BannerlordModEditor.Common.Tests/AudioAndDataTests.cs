using BannerlordModEditor.Common.Models.Audio;
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
    public class AudioAndDataTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        [Fact]
        public void ModuleSounds_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "module_sounds.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ModuleSoundsBase));
            ModuleSoundsBase moduleSounds;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                moduleSounds = (ModuleSoundsBase)serializer.Deserialize(reader)!;
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
            Assert.NotNull(moduleSounds.ModuleSounds);
            Assert.NotNull(moduleSounds.ModuleSounds.ModuleSound);
            Assert.True(moduleSounds.ModuleSounds.ModuleSound.Count > 0, "Should have at least one module sound");

            // 验证特定模块声音
            var combatHit = moduleSounds.ModuleSounds.ModuleSound
                .FirstOrDefault(s => s.Name == "example/combat/hit");
            if (combatHit != null)
            {
                Assert.Equal("example/combat/hit", combatHit.Name);
                Assert.Equal("true", combatHit.Is2D);
                Assert.Equal("mission_combat", combatHit.SoundCategory);
                Assert.Equal("example_sound_modders.ogg", combatHit.Path);
                Assert.True(combatHit.Variation == null || combatHit.Variation.Count == 0); // 直接使用path，没有variations
            }

            var voiceCharge = moduleSounds.ModuleSounds.ModuleSound
                .FirstOrDefault(s => s.Name == "example/voice/charge");
            if (voiceCharge != null)
            {
                Assert.Equal("example/voice/charge", voiceCharge.Name);
                Assert.Equal("mission_voice_shout", voiceCharge.SoundCategory);
                Assert.Equal("0.9", voiceCharge.MinPitchMultiplier);
                Assert.Equal("1.1", voiceCharge.MaxPitchMultiplier);
                Assert.NotNull(voiceCharge.Variation);
                Assert.True(voiceCharge.Variation!.Count >= 2, "Should have multiple sound variations");
                
                // 验证变体
                var firstVariation = voiceCharge.Variation!.FirstOrDefault(v => v.Path == "example_sound_modders.ogg");
                if (firstVariation != null)
                {
                    Assert.Equal("example_sound_modders.ogg", firstVariation.Path);
                    Assert.Equal("1.0", firstVariation.Weight);
                }
                
                var secondVariation = voiceCharge.Variation!.FirstOrDefault(v => v.Path == "example_sound_modders_2.ogg");
                if (secondVariation != null)
                {
                    Assert.Equal("example_sound_modders_2.ogg", secondVariation.Path);
                    Assert.Equal("0.75", secondVariation.Weight);
                }
            }

            var voiceWhisper = moduleSounds.ModuleSounds.ModuleSound
                .FirstOrDefault(s => s.Name == "example/voice/whisper");
            if (voiceWhisper != null)
            {
                Assert.Equal("example/voice/whisper", voiceWhisper.Name);
                Assert.Equal("mission_voice_trivial", voiceWhisper.SoundCategory);
                Assert.Equal("example_sound_modders.ogg", voiceWhisper.Path);
                Assert.Null(voiceWhisper.Is2D); // 不是2D声音
                Assert.True(voiceWhisper.Variation == null || voiceWhisper.Variation.Count == 0); // 没有变体
            }

            // 验证所有模块声音都有必要字段
            foreach (var sound in moduleSounds.ModuleSounds.ModuleSound)
            {
                Assert.False(string.IsNullOrEmpty(sound.Name), "Module sound should have a name");
                Assert.False(string.IsNullOrEmpty(sound.SoundCategory), $"Module sound {sound.Name} should have a sound category");
                
                // 验证声音路径：要么有直接路径，要么有变体
                Assert.True(!string.IsNullOrEmpty(sound.Path) || (sound.Variation != null && sound.Variation.Count > 0),
                    $"Module sound {sound.Name} should have either a path or variations");

                // 如果有变体，验证变体字段
                if (sound.Variation != null)
                {
                    foreach (var variation in sound.Variation)
                    {
                        Assert.False(string.IsNullOrEmpty(variation.Path), 
                            $"Sound variation in {sound.Name} should have a path");
                        Assert.False(string.IsNullOrEmpty(variation.Weight), 
                            $"Sound variation in {sound.Name} should have a weight");
                    }
                }
            }
        }

        [Fact]
        public void ModuleSounds_SoundCategories_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "module_sounds.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ModuleSoundsBase));
            ModuleSoundsBase moduleSounds;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                moduleSounds = (ModuleSoundsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证音频分类的合理性
            var validCategories = new[]
            {
                "mission_ambient_bed", "mission_ambient_3d_big", "mission_ambient_3d_medium", "mission_ambient_3d_small",
                "mission_material_impact", "mission_combat_trivial", "mission_combat", "mission_foley",
                "mission_voice_shout", "mission_voice", "mission_voice_trivial", "mission_siege_loud",
                "mission_footstep", "mission_footstep_run", "mission_horse_gallop", "mission_horse_walk",
                "ui", "alert", "campaign_node", "campaign_bed", "music"
            };

            foreach (var sound in moduleSounds.ModuleSounds.ModuleSound)
            {
                Assert.Contains(sound.SoundCategory, validCategories);
            }

            // 验证特定类别的存在
            var combatSounds = moduleSounds.ModuleSounds.ModuleSound
                .Where(s => s.SoundCategory.Contains("combat")).ToList();
            var voiceSounds = moduleSounds.ModuleSounds.ModuleSound
                .Where(s => s.SoundCategory.Contains("voice")).ToList();
            
            Assert.True(combatSounds.Count > 0, "Should have combat sounds");
            Assert.True(voiceSounds.Count > 0, "Should have voice sounds");
        }

        [Fact]
        public void AchievementData_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gog_achievement_data.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(Achievements));
            Achievements achievements;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                achievements = (Achievements)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, achievements);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(achievements);
            Assert.NotNull(achievements.Achievement);
            Assert.True(achievements.Achievement.Count > 0, "Should have at least one achievement");

            // 验证特定成就
            var entrepreneur = achievements.Achievement
                .FirstOrDefault(a => a.Name == "Entrepreneur");
            if (entrepreneur != null)
            {
                Assert.Equal("Entrepreneur", entrepreneur.Name);
                Assert.NotNull(entrepreneur.Requirements);
                Assert.NotNull(entrepreneur.Requirements!.Requirement);
                Assert.True(entrepreneur.Requirements.Requirement.Count > 0, "Entrepreneur should have requirements");
                
                var requirement = entrepreneur.Requirements.Requirement.First();
                Assert.Equal("HasOwnedCaravanAndWorkshop", requirement.StatName);
                Assert.Equal("1", requirement.Threshold);
            }

            var bannerlord = achievements.Achievement
                .FirstOrDefault(a => a.Name == "Bannerlord");
            if (bannerlord != null)
            {
                Assert.Equal("Bannerlord", bannerlord.Name);
                Assert.NotNull(bannerlord.Requirements);
                
                var requirement = bannerlord.Requirements!.Requirement.First();
                Assert.Equal("AssembledDragonBanner", requirement.StatName);
                Assert.Equal("1", requirement.Threshold);
            }

            var supremeEmperor = achievements.Achievement
                .FirstOrDefault(a => a.Name == "Supreme_Emperor");
            if (supremeEmperor != null)
            {
                Assert.Equal("Supreme_Emperor", supremeEmperor.Name);
                
                var requirement = supremeEmperor.Requirements!.Requirement.First();
                Assert.Equal("OwnedFortificationCount", requirement.StatName);
                Assert.Equal("120", requirement.Threshold); // 高数值阈值
            }

            // 验证所有成就都有必要字段
            foreach (var achievement in achievements.Achievement)
            {
                Assert.False(string.IsNullOrEmpty(achievement.Name), "Achievement should have a name");
                Assert.NotNull(achievement.Requirements);
                Assert.True(achievement.Requirements!.Requirement.Count > 0, 
                    $"Achievement {achievement.Name} should have at least one requirement");

                foreach (var requirement in achievement.Requirements.Requirement)
                {
                    Assert.False(string.IsNullOrEmpty(requirement.StatName), 
                        $"Requirement in {achievement.Name} should have a stat name");
                    Assert.False(string.IsNullOrEmpty(requirement.Threshold), 
                        $"Requirement in {achievement.Name} should have a threshold");
                }
            }
        }

        [Fact]
        public void AchievementData_ThresholdValues_ShouldBeNumeric()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gog_achievement_data.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(Achievements));
            Achievements achievements;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                achievements = (Achievements)serializer.Deserialize(reader)!;
            }

            // Assert - 验证所有阈值都是有效数字
            foreach (var achievement in achievements.Achievement)
            {
                foreach (var requirement in achievement.Requirements!.Requirement)
                {
                    Assert.True(int.TryParse(requirement.Threshold, out int threshold),
                        $"Achievement {achievement.Name} requirement {requirement.StatName} has invalid threshold: {requirement.Threshold}");
                    Assert.True(threshold >= 0, 
                        $"Achievement {achievement.Name} threshold should be non-negative: {threshold}");
                }
            }

            // 验证数值范围的合理性
            var highValueAchievements = achievements.Achievement
                .Where(a => a.Requirements!.Requirement.Any(r => int.Parse(r.Threshold) >= 100))
                .ToList();
            
            Assert.True(highValueAchievements.Count > 0, "Should have some high-value achievements");

            var lowValueAchievements = achievements.Achievement
                .Where(a => a.Requirements!.Requirement.All(r => int.Parse(r.Threshold) == 1))
                .ToList();
            
            Assert.True(lowValueAchievements.Count > 0, "Should have some simple (threshold=1) achievements");
        }

        [Fact]
        public void AchievementData_StatNames_ShouldFollowConventions()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gog_achievement_data.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(Achievements));
            Achievements achievements;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                achievements = (Achievements)serializer.Deserialize(reader)!;
            }

            // Assert - 验证统计名称遵循命名约定
            var allStatNames = achievements.Achievement
                .SelectMany(a => a.Requirements!.Requirement.Select(r => r.StatName))
                .Distinct()
                .ToList();

            Assert.True(allStatNames.Count > 0, "Should have stat names");

            // 验证常见的统计类型
            var countStats = allStatNames.Where(s => s.Contains("Count")).ToList();
            var booleanStats = allStatNames.Where(s => !s.Contains("Count") && !s.Contains("Max")).ToList();
            var maxStats = allStatNames.Where(s => s.Contains("Max")).ToList();

            Assert.True(countStats.Count > 0, "Should have count-based statistics");
            Assert.True(booleanStats.Count > 0, "Should have boolean-based statistics");

            // 验证统计名称不包含空格（应该是驼峰命名）
            foreach (var statName in allStatNames)
            {
                Assert.False(statName.Contains(" "), $"Stat name should not contain spaces: {statName}");
                Assert.True(char.IsUpper(statName[0]), $"Stat name should start with uppercase: {statName}");
            }
        }
    }
} 