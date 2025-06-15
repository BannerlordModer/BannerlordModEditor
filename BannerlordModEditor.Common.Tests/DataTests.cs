using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class DataTests
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

        private string TestDataPath => Path.Combine(FindSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData");

        [Fact]
        public void Skills_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var serializer = new XmlSerializer(typeof(ArrayOfSkillData));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ArrayOfSkillData)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SkillDataList);
            Assert.Equal(7, result.SkillDataList.Count);
        }

        [Fact]
        public void Skills_IronFleshSkillsAreCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var serializer = new XmlSerializer(typeof(ArrayOfSkillData));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ArrayOfSkillData)serializer.Deserialize(fileStream);

            // Assert
            var ironFleshSkills = result.SkillDataList.Where(s => s.Id.StartsWith("IronFlesh")).ToList();
            Assert.Equal(3, ironFleshSkills.Count);

            foreach (var skill in ironFleshSkills)
            {
                Assert.Equal("Iron Flesh " + skill.Id.Last(), skill.Name);
                Assert.Equal(1, skill.Modifiers.AttributeModifierList.Count);
                Assert.Equal("AgentHitPoints", skill.Modifiers.AttributeModifierList[0].AttribCode);
                Assert.Equal("Multiply", skill.Modifiers.AttributeModifierList[0].Modification);
                Assert.Equal("Iron flesh increases hit points", skill.Documentation);
            }
        }

        [Fact]
        public void Skills_PowerStrikeSkillsAreCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var serializer = new XmlSerializer(typeof(ArrayOfSkillData));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ArrayOfSkillData)serializer.Deserialize(fileStream);

            // Assert
            var powerStrikeSkills = result.SkillDataList.Where(s => s.Id.StartsWith("PowerStrike")).ToList();
            Assert.Equal(3, powerStrikeSkills.Count);

            foreach (var skill in powerStrikeSkills)
            {
                Assert.Equal("Power Strike", skill.Name);
                Assert.Equal(2, skill.Modifiers.AttributeModifierList.Count);
                
                var swingModifier = skill.Modifiers.AttributeModifierList.FirstOrDefault(m => m.AttribCode == "WeaponSwingDamage");
                var thrustModifier = skill.Modifiers.AttributeModifierList.FirstOrDefault(m => m.AttribCode == "WeaponThrustDamage");
                
                Assert.NotNull(swingModifier);
                Assert.NotNull(thrustModifier);
                Assert.Equal(swingModifier.Value, thrustModifier.Value);
                Assert.Equal("Multiply", swingModifier.Modification);
                Assert.Equal("Multiply", thrustModifier.Modification);
                Assert.Equal("Power Strike increases melee damage", skill.Documentation);
            }
        }

        [Fact]
        public void Skills_RunnerSkillIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var serializer = new XmlSerializer(typeof(ArrayOfSkillData));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ArrayOfSkillData)serializer.Deserialize(fileStream);

            // Assert
            var runnerSkill = result.SkillDataList.FirstOrDefault(s => s.Id == "Runner");
            Assert.NotNull(runnerSkill);
            Assert.Equal("Runner", runnerSkill.Name);
            Assert.Equal(1, runnerSkill.Modifiers.AttributeModifierList.Count);
            
            var modifier = runnerSkill.Modifiers.AttributeModifierList[0];
            Assert.Equal("AgentRunningSpeed", modifier.AttribCode);
            Assert.Equal("Multiply", modifier.Modification);
            Assert.Equal("2", modifier.Value);
            Assert.Equal("Runner increases running speed", runnerSkill.Documentation);
        }

        [Fact]
        public void Skills_AllSkillsHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var serializer = new XmlSerializer(typeof(ArrayOfSkillData));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ArrayOfSkillData)serializer.Deserialize(fileStream);

            // Assert
            foreach (var skill in result.SkillDataList)
            {
                Assert.NotEmpty(skill.Id);
                Assert.NotEmpty(skill.Name);
                Assert.NotNull(skill.Modifiers);
                Assert.NotEmpty(skill.Documentation);
                Assert.True(skill.Modifiers.AttributeModifierList.Count > 0);
            }
        }

        [Fact]
        public void Scenes_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "scenes.xml");
            var serializer = new XmlSerializer(typeof(ScenesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ScenesBase)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("scene", result.Type);
            Assert.NotNull(result.Sites);
            Assert.NotNull(result.Sites.SiteList);
            Assert.True(result.Sites.SiteList.Count > 500); // 有很多场景
        }

        [Fact]
        public void Scenes_AllSitesHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "scenes.xml");
            var serializer = new XmlSerializer(typeof(ScenesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ScenesBase)serializer.Deserialize(fileStream);

            // Assert
            foreach (var site in result.Sites.SiteList)
            {
                Assert.NotEmpty(site.Id);
                Assert.NotEmpty(site.Name);
                Assert.StartsWith("scn_", site.Id); // 所有场景ID应该以scn_开头
            }
        }

        [Fact]
        public void Scenes_SpecificScenesExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "scenes.xml");
            var serializer = new XmlSerializer(typeof(ScenesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ScenesBase)serializer.Deserialize(fileStream);

            // Assert
            var worldMapScene = result.Sites.SiteList.FirstOrDefault(s => s.Id == "scn_world_map");
            Assert.NotNull(worldMapScene);
            Assert.Equal("world_map", worldMapScene.Name);

            var conversationScene = result.Sites.SiteList.FirstOrDefault(s => s.Id == "scn_conversation_scene");
            Assert.NotNull(conversationScene);
            Assert.Equal("conversation_scene", conversationScene.Name);

            var randomScenes = result.Sites.SiteList.Where(s => s.Id.Contains("random_scene")).ToList();
            Assert.True(randomScenes.Count > 5); // 应该有多个随机场景
        }

        [Fact]
        public void Scenes_MultiplayerScenesExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "scenes.xml");
            var serializer = new XmlSerializer(typeof(ScenesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ScenesBase)serializer.Deserialize(fileStream);

            // Assert
            var multiScenes = result.Sites.SiteList.Where(s => s.Id.StartsWith("scn_multi_scene_")).ToList();
            Assert.True(multiScenes.Count >= 15); // 应该有至少15个多人游戏场景

            var quickBattleScenes = result.Sites.SiteList.Where(s => s.Id.Contains("quick_battle")).ToList();
            Assert.True(quickBattleScenes.Count > 5); // 应该有多个快速战斗场景
        }

        [Fact]
        public void Scenes_IdAndNameFormatIsConsistent()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "scenes.xml");
            var serializer = new XmlSerializer(typeof(ScenesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (ScenesBase)serializer.Deserialize(fileStream);

            // Assert
            foreach (var site in result.Sites.SiteList)
            {
                // ID应该以"scn_"开头，name不应该包含"scn_"前缀
                Assert.StartsWith("scn_", site.Id);
                Assert.DoesNotContain("scn_", site.Name);
                
                // 验证特定的已知对应关系
                if (site.Id == "scn_world_map")
                    Assert.Equal("world_map", site.Name);
                                 if (site.Id == "scn_conversation_scene")
                     Assert.Equal("conversation_scene", site.Name);
            }
        }

        [Fact]
        public void Voices_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "voices.xml");
            var serializer = new XmlSerializer(typeof(VoicesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (VoicesBase)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("face_animation_record", result.Type);
            Assert.NotNull(result.FaceAnimationRecords);
            Assert.NotNull(result.FaceAnimationRecords.FaceAnimationRecordList);
            Assert.True(result.FaceAnimationRecords.FaceAnimationRecordList.Count > 100); // 有很多面部动画记录
        }

        [Fact]
        public void Voices_AllRecordsHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "voices.xml");
            var serializer = new XmlSerializer(typeof(VoicesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (VoicesBase)serializer.Deserialize(fileStream);

            // Assert
            foreach (var record in result.FaceAnimationRecords.FaceAnimationRecordList)
            {
                Assert.NotEmpty(record.Id);
                Assert.NotEmpty(record.AnimationName);
                // 注意：有些记录可能没有flags，这是正常的
            }
        }

        [Fact]
        public void Voices_CommonFlagsExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "voices.xml");
            var serializer = new XmlSerializer(typeof(VoicesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (VoicesBase)serializer.Deserialize(fileStream);

            // Assert
            var recordsWithFlags = result.FaceAnimationRecords.FaceAnimationRecordList
                .Where(r => r.Flags != null && r.Flags.FlagList.Count > 0).ToList();
            
            Assert.True(recordsWithFlags.Count > 50); // 应该有很多带标志的记录

            // 检查常见的标志
            var doBlinkFlags = recordsWithFlags.SelectMany(r => r.Flags!.FlagList)
                .Where(f => f.Name == "do_blink").ToList();
            var lookAtPlayerFlags = recordsWithFlags.SelectMany(r => r.Flags!.FlagList)
                .Where(f => f.Name == "look_at_player").ToList();

            Assert.True(doBlinkFlags.Count > 30); // 应该有很多do_blink标志
            Assert.True(lookAtPlayerFlags.Count > 30); // 应该有很多look_at_player标志
        }

        [Fact]
        public void Voices_SpecificRecordsExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "voices.xml");
            var serializer = new XmlSerializer(typeof(VoicesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (VoicesBase)serializer.Deserialize(fileStream);

            // Assert
            var maleCustom = result.FaceAnimationRecords.FaceAnimationRecordList
                .FirstOrDefault(r => r.Id == "male_custom");
            Assert.NotNull(maleCustom);
            Assert.Equal("male_custom", maleCustom.AnimationName);
            Assert.NotNull(maleCustom.Flags);
            Assert.True(maleCustom.Flags.FlagList.Any(f => f.Name == "do_blink"));
            Assert.True(maleCustom.Flags.FlagList.Any(f => f.Name == "look_at_player"));

            var femaleCustom = result.FaceAnimationRecords.FaceAnimationRecordList
                .FirstOrDefault(r => r.Id == "female_custom");
            Assert.NotNull(femaleCustom);
            Assert.Equal("female_custom", femaleCustom.AnimationName);

            var portraits = result.FaceAnimationRecords.FaceAnimationRecordList
                .FirstOrDefault(r => r.Id == "portraits");
            Assert.NotNull(portraits);
            // portraits记录可能没有flags，这是正常的
        }

        [Fact]
        public void Voices_AnimationCategoriesExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "voices.xml");
            var serializer = new XmlSerializer(typeof(VoicesBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (VoicesBase)serializer.Deserialize(fileStream);

            // Assert
            // 检查不同类别的动画记录
            var facegenRecords = result.FaceAnimationRecords.FaceAnimationRecordList
                .Where(r => r.Id.Contains("facegen") || r.Id.Contains("custom")).ToList();
            Assert.True(facegenRecords.Count >= 4); // 应该有男女自定义和facegen记录

            var orderRecords = result.FaceAnimationRecords.FaceAnimationRecordList
                .Where(r => r.Id.Contains("archers") || r.Id.Contains("cavalry") || r.Id.Contains("infantry")).ToList();
            Assert.True(orderRecords.Count >= 3); // 应该有军事命令记录

            var talkingRecords = result.FaceAnimationRecords.FaceAnimationRecordList
                .Where(r => r.Id.Contains("talking")).ToList();
            Assert.True(talkingRecords.Count >= 3); // 应该有对话表情记录

            var poseRecords = result.FaceAnimationRecords.FaceAnimationRecordList
                .Where(r => r.Id.StartsWith("pose_")).ToList();
            Assert.True(poseRecords.Count >= 5); // 应该有姿势记录
        }
    }
} 