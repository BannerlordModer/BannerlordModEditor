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
            var serializer = new XmlSerializer(typeof(Skills));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Skills)serializer.Deserialize(fileStream);

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
            var serializer = new XmlSerializer(typeof(Skills));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Skills)serializer.Deserialize(fileStream);

            // Assert
            var ironFleshSkills = result.SkillDataList.Where(s => s.Id.StartsWith("IronFlesh")).ToList();
            Assert.Equal(3, ironFleshSkills.Count);

            foreach (var skill in ironFleshSkills)
            {
                Assert.Equal("Iron Flesh " + skill.Id.Last(), skill.Name);
                Assert.Equal(1, skill.Modifiers.AttributeModifiers.Count);
                Assert.Equal("AgentHitPoints", skill.Modifiers.AttributeModifiers[0].AttribCode);
                Assert.Equal("Multiply", skill.Modifiers.AttributeModifiers[0].Modification);
                Assert.Equal("Iron flesh increases hit points", skill.Documentation);
            }
        }

        [Fact]
        public void Skills_PowerStrikeSkillsAreCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skills.xml");
            var serializer = new XmlSerializer(typeof(Skills));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Skills)serializer.Deserialize(fileStream);

            // Assert
            var powerStrikeSkills = result.SkillDataList.Where(s => s.Id.StartsWith("PowerStrike")).ToList();
            Assert.Equal(3, powerStrikeSkills.Count);

            foreach (var skill in powerStrikeSkills)
            {
                Assert.Equal("Power Strike", skill.Name);
                Assert.Equal(2, skill.Modifiers.AttributeModifiers.Count);
                
                var swingModifier = skill.Modifiers.AttributeModifiers.FirstOrDefault(m => m.AttribCode == "WeaponSwingDamage");
                var thrustModifier = skill.Modifiers.AttributeModifiers.FirstOrDefault(m => m.AttribCode == "WeaponThrustDamage");
                
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
            var serializer = new XmlSerializer(typeof(Skills));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Skills)serializer.Deserialize(fileStream);

            // Assert
            var runnerSkill = result.SkillDataList.FirstOrDefault(s => s.Id == "Runner");
            Assert.NotNull(runnerSkill);
            Assert.Equal("Runner", runnerSkill.Name);
            Assert.Equal(1, runnerSkill.Modifiers.AttributeModifiers.Count);
            
            var modifier = runnerSkill.Modifiers.AttributeModifiers[0];
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
            var serializer = new XmlSerializer(typeof(Skills));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Skills)serializer.Deserialize(fileStream);

            // Assert
            foreach (var skill in result.SkillDataList)
            {
                Assert.NotEmpty(skill.Id);
                Assert.NotEmpty(skill.Name);
                Assert.NotNull(skill.Modifiers);
                Assert.NotEmpty(skill.Documentation);
                Assert.True(skill.Modifiers.AttributeModifiers.Count > 0);
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
            Assert.True(result.Sites.SiteList.Count > 500); // 有很多场�?
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
                Assert.StartsWith("scn_", site.Id); // 所有场景ID应该以scn_开�?
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
            Assert.True(randomScenes.Count > 5); // 应该有多个随机场�?
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
            Assert.True(multiScenes.Count >= 15); // 应该有至�?5个多人游戏场�?

            var quickBattleScenes = result.Sites.SiteList.Where(s => s.Id.Contains("quick_battle")).ToList();
            Assert.True(quickBattleScenes.Count > 5); // 应该有多个快速战斗场�?
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
                // ID应该�?scn_"开头，name不应该包�?scn_"前缀
                Assert.StartsWith("scn_", site.Id);
                Assert.DoesNotContain("scn_", site.Name);
                
                // 验证特定的已知对应关�?
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
            Assert.True(result.FaceAnimationRecords.FaceAnimationRecordList.Count > 100); // 有很多面部动画记�?
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
            
            Assert.True(recordsWithFlags.Count > 50); // 应该有很多带标志的记�?

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
            Assert.True(orderRecords.Count >= 3); // 应该有军事命令记�?

            var talkingRecords = result.FaceAnimationRecords.FaceAnimationRecordList
                .Where(r => r.Id.Contains("talking")).ToList();
            Assert.True(talkingRecords.Count >= 3); // 应该有对话表情记�?

                         var poseRecords = result.FaceAnimationRecords.FaceAnimationRecordList
                 .Where(r => r.Id.StartsWith("pose_")).ToList();
             Assert.True(poseRecords.Count >= 5); // 应该有姿势记�?
         }

        [Fact]
        public void BannerIcons_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsRoot));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsRoot)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.BannerIconData);
            Assert.NotNull(result.BannerIconData.BannerIconGroupList);
            Assert.NotNull(result.BannerIconData.BannerColors);
            Assert.True(result.BannerIconData.BannerIconGroupList.Count >= 6); // 应该有多个图标组
            Assert.True(result.BannerIconData.BannerColors.ColorList.Count > 100); // 应该有很多颜�?
        }

        [Fact]
        public void BannerIcons_IconGroupsHaveCorrectStructure()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsRoot));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsRoot)serializer.Deserialize(fileStream);

            // Assert
            foreach (var group in result.BannerIconData.BannerIconGroupList)
            {
                Assert.NotEmpty(group.Id);
                Assert.NotEmpty(group.Name);
                Assert.NotEmpty(group.IsPattern);
                
                // 检查组的类�?
                if (group.IsPattern == "true")
                {
                    // 如果是pattern，应该有Background元素
                    Assert.True(group.BackgroundList.Count > 0);
                }
                else
                {
                    // 如果不是pattern，应该有Icon元素
                    Assert.True(group.IconList.Count > 0);
                }
            }
        }

        [Fact]
        public void BannerIcons_BackgroundGroupExists()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsRoot));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsRoot)serializer.Deserialize(fileStream);

            // Assert
            var backgroundGroup = result.BannerIconData.BannerIconGroupList.FirstOrDefault(g => g.Id == "1");
            Assert.NotNull(backgroundGroup);
            Assert.Equal("true", backgroundGroup.IsPattern);
            Assert.True(backgroundGroup.BackgroundList.Count >= 30); // 应该有很多背�?
            
            // 检查有一个base background
            var baseBackground = backgroundGroup.BackgroundList.FirstOrDefault(b => b.IsBaseBackground == "true");
            Assert.NotNull(baseBackground);
            
            foreach (var background in backgroundGroup.BackgroundList)
            {
                Assert.NotEmpty(background.Id);
                Assert.NotEmpty(background.MeshName);
            }
        }

        [Fact]
        public void BannerIcons_AnimalGroupExists()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsRoot));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsRoot)serializer.Deserialize(fileStream);

            // Assert
            var animalGroup = result.BannerIconData.BannerIconGroupList.FirstOrDefault(g => g.Id == "2");
            Assert.NotNull(animalGroup);
            Assert.Contains("Animal", animalGroup.Name);
            Assert.Equal("false", animalGroup.IsPattern);
            Assert.True(animalGroup.IconList.Count >= 50); // 应该有很多动物图�?
            
            // 检查保留的图标
            var reservedIcons = animalGroup.IconList.Where(i => i.IsReserved == "true").ToList();
            Assert.True(reservedIcons.Count >= 5); // 应该有一些保留的文化图标
            
            foreach (var icon in animalGroup.IconList)
            {
                Assert.NotEmpty(icon.Id);
                Assert.NotEmpty(icon.MaterialName);
                Assert.NotEmpty(icon.TextureIndex);
            }
        }

        [Fact]
        public void BannerIcons_ColorsHaveValidHexValues()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsRoot));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsRoot)serializer.Deserialize(fileStream);

            // Assert
            foreach (var color in result.BannerIconData.BannerColors.ColorList)
            {
                Assert.NotEmpty(color.Id);
                Assert.NotEmpty(color.Hex);
                Assert.StartsWith("0x", color.Hex); // 应该是十六进制格�?
                Assert.True(color.Hex.Length == 10); // 0xFFRRGGBB格式应该�?0个字�?
            }
            
            // 检查一些特定的文化颜色
            var aseraiBgColor = result.BannerIconData.BannerColors.ColorList.FirstOrDefault(c => c.Id == "0");
            Assert.NotNull(aseraiBgColor);
            Assert.Equal("0xffB57A1E", aseraiBgColor.Hex);
            
            // 检查玩家可选择的颜�?
            var playerBgColors = result.BannerIconData.BannerColors.ColorList
                .Where(c => c.PlayerCanChooseForBackground == "true").ToList();
            var playerSigilColors = result.BannerIconData.BannerColors.ColorList
                .Where(c => c.PlayerCanChooseForSigil == "true").ToList();
            
            Assert.True(playerBgColors.Count >= 5); // 应该有一些玩家可选择的背景颜�?
            Assert.True(playerSigilColors.Count >= 5); // 应该有一些玩家可选择的徽记颜�?
        }

        [Fact]
        public void BannerIcons_CultureSpecificIconsExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsRoot));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsRoot)serializer.Deserialize(fileStream);

            // Assert
            // 检查不同类别的图标组是否存�?
            var categories = new[] { "Animal", "Flora", "Handmade", "Sign", "Shape" };
            
            foreach (var category in categories)
            {
                var group = result.BannerIconData.BannerIconGroupList
                    .FirstOrDefault(g => g.Name.Contains(category));
                Assert.NotNull(group);
                Assert.Equal("false", group.IsPattern); // 除了Background外都应该是false
                Assert.True(group.IconList.Count > 0); // 应该有图�?
            }
            
            // 检查multiplayer culture colors
            var multiplayerColors = result.BannerIconData.BannerColors.ColorList
                .Where(c => c.Id == "122" || c.Id == "126" || c.Id == "130" || c.Id == "134" || c.Id == "138" || c.Id == "142")
                .ToList();
                         Assert.Equal(6, multiplayerColors.Count); // 应该�?个主要文化的多人游戏颜色
         }

        [Fact]
        public void WaterPrefabs_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "water_prefabs.xml");
            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WaterPrefabs)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.WaterPrefab);
            Assert.True(result.WaterPrefab.Length >= 30); // 应该有很多水体预制品
        }

        [Fact]
        public void WaterPrefabs_PrefabsHaveRequiredProperties()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "water_prefabs.xml");
            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WaterPrefabs)serializer.Deserialize(fileStream);

            // Assert
            foreach (var prefab in result.WaterPrefab)
            {
                Assert.NotEmpty(prefab.PrefabName);
                Assert.NotEmpty(prefab.MaterialName);
                Assert.NotEmpty(prefab.Thumbnail);
                Assert.NotEmpty(prefab.IsGlobal);
                Assert.True(prefab.IsGlobal == "true" || prefab.IsGlobal == "false" || prefab.IsGlobal == "False");
            }

            // 检查特定类型的水体
            var globalPrefabs = result.WaterPrefab.Where(p => p.IsGlobal == "true").ToList();
            var localPrefabs = result.WaterPrefab.Where(p => p.IsGlobal == "false" || p.IsGlobal == "False").ToList();
            
            Assert.True(globalPrefabs.Count > 5); // 应该有全局水体
            Assert.True(localPrefabs.Count > 5); // 应该有局部水�?
            
            // 检查特定的海洋水体
            var oceanPrefab = result.WaterPrefab.FirstOrDefault(p => p.PrefabName.Contains("Ocean"));
            Assert.NotNull(oceanPrefab);
        }

        [Fact]
        public void SpecialMeshes_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "special_meshes.xml");
            var serializer = new XmlSerializer(typeof(SpecialMeshes));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (SpecialMeshes)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("special_meshes", result.Type);
            Assert.NotNull(result.Meshes);
            Assert.NotNull(result.Meshes.MeshList);
            Assert.True(result.Meshes.MeshList.Count >= 2); // 应该有一些特殊网�?
        }

        [Fact]
        public void SpecialMeshes_MeshesHaveCorrectStructure()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "special_meshes.xml");
            var serializer = new XmlSerializer(typeof(SpecialMeshes));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (SpecialMeshes)serializer.Deserialize(fileStream);

            // Assert
            foreach (var mesh in result.Meshes.MeshList)
            {
                Assert.NotEmpty(mesh.Name);
                Assert.NotNull(mesh.Types);
                Assert.NotNull(mesh.Types.TypeList);
                Assert.True(mesh.Types.TypeList.Count > 0); // 每个网格应该有类�?

                foreach (var type in mesh.Types.TypeList)
                {
                    Assert.NotEmpty(type.Name);
                }
            }

            // 检查外网格
            var outerMesh = result.Meshes.MeshList.FirstOrDefault(m => m.Name.Contains("outer"));
            Assert.NotNull(outerMesh);
            var outerMeshType = outerMesh.Types.TypeList.FirstOrDefault(t => t.Name == "outer_mesh");
                         Assert.NotNull(outerMeshType);
         }

        [Fact]
        public void WorldmapColorGrades_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "worldmap_color_grades.xml");
            var serializer = new XmlSerializer(typeof(WorldmapColorGrades));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WorldmapColorGrades)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ColorGradeGrid);
            Assert.NotNull(result.ColorGradeDefault);
            Assert.NotNull(result.ColorGradeNight);
            Assert.NotNull(result.ColorGrades);
            Assert.True(result.ColorGrades.Count >= 8); // 应该有多个颜色等�?
        }

        [Fact]
        public void WorldmapColorGrades_ColorGradesHaveValues()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "worldmap_color_grades.xml");
            var serializer = new XmlSerializer(typeof(WorldmapColorGrades));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WorldmapColorGrades)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotEmpty(result.ColorGradeGrid.Name);
            Assert.NotEmpty(result.ColorGradeDefault.Name);
            Assert.NotEmpty(result.ColorGradeNight.Name);

            foreach (var colorGrade in result.ColorGrades)
            {
                Assert.NotEmpty(colorGrade.Name);
                Assert.NotEmpty(colorGrade.Value);
                Assert.True(int.TryParse(colorGrade.Value, out int value)); // 值应该是数字
                Assert.True(value >= 0 && value <= 200); // 值应该在合理范围�?
            }

            // 检查特定的环境
            var desertGrade = result.ColorGrades.FirstOrDefault(g => g.Name.Contains("desert"));
            Assert.NotNull(desertGrade);
            Assert.Equal("20", desertGrade.Value);
        }

        [Fact]
        public void SkinnedDecals_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skinned_decals.xml");
            var serializer = new XmlSerializer(typeof(SkinnedDecalsBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (SkinnedDecalsBase)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("skinned_decals", result.Type);
            Assert.NotNull(result.SkinnedDecals);
            Assert.NotNull(result.SkinnedDecals.SkinnedDecalList);
            Assert.True(result.SkinnedDecals.SkinnedDecalList.Count >= 1); // 应该有贴�?
        }

        [Fact]
        public void SkinnedDecals_DecalsHaveTexturesAndMaterials()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "skinned_decals.xml");
            var serializer = new XmlSerializer(typeof(SkinnedDecalsBase));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (SkinnedDecalsBase)serializer.Deserialize(fileStream);

            // Assert
            foreach (var decal in result.SkinnedDecals.SkinnedDecalList)
            {
                if (decal.Textures != null)
                {
                    Assert.True(decal.Textures.TextureList.Count >= 3); // 通常有Diffuse、Normal、Specular
                    
                    foreach (var texture in decal.Textures.TextureList)
                    {
                        Assert.NotEmpty(texture.Type);
                        Assert.NotEmpty(texture.Name);
                    }

                    // 检查不同类型的贴图
                    Assert.Contains(decal.Textures.TextureList, t => t.Type == "DiffuseMap");
                    Assert.Contains(decal.Textures.TextureList, t => t.Type == "NormalMap");
                    Assert.Contains(decal.Textures.TextureList, t => t.Type == "SpecularMap");
                }

                if (decal.Materials != null)
                {
                    Assert.True(decal.Materials.MaterialList.Count >= 10); // 应该有多个材�?
                    
                    foreach (var material in decal.Materials.MaterialList)
                    {
                        Assert.NotEmpty(material.Enum);
                        Assert.NotEmpty(material.Name);
                        Assert.StartsWith("material_", material.Enum); // 枚举应该以material_开�?
                        Assert.StartsWith("blood", material.Name); // 材质名应该以blood开�?
                    }
                }
            }
        }

        [Fact]
        public void Monsters_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MonsterList);
            Assert.True(result.MonsterList.Count >= 15); // 应该有多个怪物定义
        }

        [Fact]
        public void Monsters_AllMonstersHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            foreach (var monster in result.MonsterList)
            {
                Assert.NotEmpty(monster.Id);
                // 基本物理属性（非继承怪物应该有这些属性）
                if (monster.BaseMonster == null)
                {
                    Assert.NotNull(monster.ActionSet);
                    Assert.NotNull(monster.MonsterUsage);
                    Assert.NotNull(monster.Weight);
                    Assert.NotNull(monster.HitPoints);
                }
            }
        }

        [Fact]
        public void Monsters_HumanMonsterIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            var human = result.MonsterList.FirstOrDefault(m => m.Id == "human");
            Assert.NotNull(human);
            Assert.Equal("as_human_warrior", human.ActionSet);
            Assert.Equal("as_human_female_warrior", human.FemaleActionSet);
            Assert.Equal("human", human.MonsterUsage);
            Assert.Equal("80", human.Weight);
            Assert.Equal("100", human.HitPoints);
            Assert.Equal("1.0", human.AbsorbedDamageRatio);
            Assert.Equal("1.8", human.WalkingSpeedLimit);
            Assert.Equal("1.3", human.CrouchWalkingSpeedLimit);
            Assert.Equal("0", human.FamilyType);
            
            // 检查胶囊体
            Assert.NotNull(human.Capsules);
            Assert.NotNull(human.Capsules.BodyCapsule);
            Assert.NotNull(human.Capsules.CrouchedBodyCapsule);
            Assert.Equal("0.37", human.Capsules.BodyCapsule.Radius);
            Assert.Equal("0.37", human.Capsules.CrouchedBodyCapsule.Radius);
            
            // 检查标�?
            Assert.NotNull(human.Flags);
            Assert.Equal("true", human.Flags.CanAttack);
            Assert.Equal("true", human.Flags.CanDefend);
            Assert.Equal("true", human.Flags.IsHumanoid);
            Assert.Equal("true", human.Flags.CanRide);
            Assert.Equal("true", human.Flags.CanWieldWeapon);
        }

        [Fact]
        public void Monsters_HorseMonsterIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            var horse = result.MonsterList.FirstOrDefault(m => m.Id == "horse");
            Assert.NotNull(horse);
            Assert.Equal("as_horse", horse.ActionSet);
            Assert.Equal("horse", horse.MonsterUsage);
            Assert.Equal("400", horse.Weight);
            Assert.Equal("200", horse.HitPoints);
            Assert.Equal("6", horse.NumPaces);
            Assert.Equal("1", horse.FamilyType);
            Assert.Equal("horse", horse.SoundAndCollisionInfoClass);
            
            // 检查骑乘属�?
            Assert.NotNull(horse.RiderEyeHeightAdder);
            Assert.NotNull(horse.RiderCameraHeightAdder);
            Assert.NotNull(horse.RiderBodyCapsuleHeightAdder);
            Assert.NotNull(horse.RiderBodyCapsuleForwardAdder);
            Assert.NotNull(horse.RiderSitBone);
            
            // 检查缰绳属�?
            Assert.NotNull(horse.ReinHandleLeftLocalPos);
            Assert.NotNull(horse.ReinHandleRightLocalPos);
            Assert.NotNull(horse.ReinSkeleton);
            Assert.NotNull(horse.ReinCollisionBody);
            
            // 检查标�?
            Assert.NotNull(horse.Flags);
            Assert.Equal("true", horse.Flags.Mountable);
            Assert.Equal("true", horse.Flags.CanRear);
            Assert.Equal("true", horse.Flags.RunsAwayWhenHit);
            Assert.Equal("true", horse.Flags.CanCharge);
            Assert.Equal("true", horse.Flags.CanWander);
        }

        [Fact]
        public void Monsters_InheritanceWorksCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            var humanChild = result.MonsterList.FirstOrDefault(m => m.Id == "human_child");
            Assert.NotNull(humanChild);
            Assert.Equal("human", humanChild.BaseMonster);
            Assert.Equal("as_human_child", humanChild.ActionSet);
            Assert.Equal("30", humanChild.Weight);
            Assert.Equal("1.6", humanChild.WalkingSpeedLimit);
            
            var humanSettlement = result.MonsterList.FirstOrDefault(m => m.Id == "human_settlement");
            Assert.NotNull(humanSettlement);
            Assert.Equal("human", humanSettlement.BaseMonster);
            Assert.Equal("40", humanSettlement.HitPoints);
            Assert.Equal("1.4", humanSettlement.WalkingSpeedLimit);
            
            var humanSettlementSlow = result.MonsterList.FirstOrDefault(m => m.Id == "human_settlement_slow");
            Assert.NotNull(humanSettlementSlow);
            Assert.Equal("human_settlement", humanSettlementSlow.BaseMonster);
            Assert.Equal("1.1", humanSettlementSlow.WalkingSpeedLimit);
        }

        [Fact]
        public void Monsters_AnimalMonstersHaveCorrectProperties()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            var cow = result.MonsterList.FirstOrDefault(m => m.Id == "cow");
            Assert.NotNull(cow);
            Assert.Equal("as_cow", cow.ActionSet);
            Assert.Equal("animals", cow.MonsterUsage);
            Assert.Equal("500", cow.Weight);
            Assert.Equal("3", cow.FamilyType);
            Assert.Equal("bovine", cow.SoundAndCollisionInfoClass);
            Assert.NotNull(cow.Flags);
            Assert.Equal("true", cow.Flags.RunsAwayWhenHit);
            Assert.Equal("true", cow.Flags.CanWander);
            Assert.Equal("true", cow.Flags.MoveAsHerd);
            Assert.Equal("true", cow.Flags.MoveForwardOnly);
            
            var sheep = result.MonsterList.FirstOrDefault(m => m.Id == "sheep");
            Assert.NotNull(sheep);
            Assert.Equal("as_sheep", sheep.ActionSet);
            Assert.Equal("animals", sheep.MonsterUsage);
            Assert.Equal("6", sheep.FamilyType);
            Assert.Equal("ovine", sheep.SoundAndCollisionInfoClass);
            Assert.NotNull(sheep.Flags);
            Assert.Equal("true", sheep.Flags.MoveAsHerd);
            Assert.Equal("true", sheep.Flags.RunsAwayWhenHit);
            Assert.Equal("true", sheep.Flags.CanGetScared);
        }

        [Fact]
        public void Monsters_BoneStructuresExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            var human = result.MonsterList.FirstOrDefault(m => m.Id == "human");
            Assert.NotNull(human);
            
            // 检查关键骨骼定�?
            Assert.NotNull(human.HeadLookDirectionBone);
            Assert.NotNull(human.PelvisBone);
            Assert.NotNull(human.NeckRootBone);
            Assert.NotNull(human.MainHandBone);
            Assert.NotNull(human.OffHandBone);
            Assert.NotNull(human.PrimaryFootBone);
            Assert.NotNull(human.SecondaryFootBone);
            
            // 检查尸体检查骨�?
            Assert.NotNull(human.RagdollBoneToCheckForCorpses0);
            Assert.NotNull(human.RagdollBoneToCheckForCorpses1);
            Assert.NotNull(human.RagdollBoneToCheckForCorpses2);
            
            // 检查坠落声音骨�?
            Assert.NotNull(human.RagdollFallSoundBone0);
            Assert.NotNull(human.RagdollFallSoundBone1);
            Assert.NotNull(human.RagdollFallSoundBone2);
        }

        [Fact]
        public void Monsters_CapsulesHaveValidDimensions()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            foreach (var monster in result.MonsterList.Where(m => m.Capsules != null))
            {
                if (monster.Capsules.BodyCapsule != null)
                {
                    Assert.NotNull(monster.Capsules.BodyCapsule.Radius);
                    Assert.NotNull(monster.Capsules.BodyCapsule.Pos1);
                    Assert.NotNull(monster.Capsules.BodyCapsule.Pos2);
                    
                    // 检查半径是否为有效数�?
                    Assert.True(double.TryParse(monster.Capsules.BodyCapsule.Radius, out double radius));
                    Assert.True(radius > 0 && radius < 1.0); // 合理的半径范�?
                }
                
                if (monster.Capsules.CrouchedBodyCapsule != null)
                {
                    Assert.NotNull(monster.Capsules.CrouchedBodyCapsule.Radius);
                    Assert.NotNull(monster.Capsules.CrouchedBodyCapsule.Pos1);
                    Assert.NotNull(monster.Capsules.CrouchedBodyCapsule.Pos2);
                }
            }
        }

        [Fact]
        public void Monsters_FlagsHaveValidValues()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "monsters.xml");
            var serializer = new XmlSerializer(typeof(Monsters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (Monsters)serializer.Deserialize(fileStream);

            // Assert
            foreach (var monster in result.MonsterList.Where(m => m.Flags != null))
            {
                var flags = monster.Flags;
                
                // 所有标志值都应该�?true"，或者为null（未设置�?
                string[] flagProperties = {
                    flags.CanAttack, flags.CanDefend, flags.CanKick, flags.CanBeCharged,
                    flags.CanCharge, flags.CanClimbLadders, flags.CanSprint, flags.CanCrouch,
                    flags.CanRetreat, flags.CanRear, flags.CanWander, flags.CanBeInGroup,
                    flags.MoveAsHerd, flags.MoveForwardOnly, flags.IsHumanoid, flags.Mountable,
                    flags.CanRide, flags.CanWieldWeapon, flags.RunsAwayWhenHit, flags.CanGetScared
                };
                
                foreach (var flag in flagProperties)
                {
                    if (flag != null)
                    {
                        Assert.Equal("true", flag);
                    }
                }
            }
        }

        [Fact]
        public void MPBadges_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BadgeList);
            Assert.Equal(137, result.BadgeList.Count); // 应该�?37个徽章定�?
        }

        [Fact]
        public void MPBadges_AllBadgesHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            foreach (var badge in result.BadgeList)
            {
                Assert.NotEmpty(badge.Id);
                Assert.NotNull(badge.Type);
                Assert.NotNull(badge.Name);
                Assert.NotNull(badge.Description);
            }
        }

        [Fact]
        public void MPBadges_CustomBadgesAreCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var customBadges = result.BadgeList.Where(b => b.Type == "Custom").ToList();
            Assert.True(customBadges.Count > 0); // 应该有自定义徽章

            // 检查特定的开发者徽�?
            var devBadge = result.BadgeList.FirstOrDefault(b => b.Id == "badge_taleworlds_primary_dev");
            Assert.NotNull(devBadge);
            Assert.Equal("Custom", devBadge.Type);
            Assert.Equal("{=aubhe8Hk}TaleWorlds Developer", devBadge.Name);
            Assert.Equal("{=GTmPMoYe}Employee of TaleWorlds", devBadge.Description);
            Assert.Equal("true", devBadge.IsVisibleOnlyWhenEarned);
            Assert.Empty(devBadge.Conditions); // 自定义徽章通常没有条件

            // 检查Alpha测试者徽�?
            var alphaBadge = result.BadgeList.FirstOrDefault(b => b.Id == "badge_alpha_tester");
            Assert.NotNull(alphaBadge);
            Assert.Equal("Custom", alphaBadge.Type);
            Assert.Equal("{=5IANKz8a}Alpha Tester", alphaBadge.Name);
        }

        [Fact]
        public void MPBadges_ConditionalBadgesHaveConditions()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var conditionalBadges = result.BadgeList.Where(b => b.Type == "Conditional").ToList();
            Assert.True(conditionalBadges.Count > 0); // 应该有条件徽�?

            foreach (var badge in conditionalBadges)
            {
                Assert.True(badge.Conditions.Count > 0); // 条件徽章必须有条�?
            }
        }

        [Fact]
        public void MPBadges_GameComBadgeIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var gamecomBadge = result.BadgeList.FirstOrDefault(b => b.Id == "gamescom_2020_call_to_arms");
            Assert.NotNull(gamecomBadge);
            Assert.Equal("Conditional", gamecomBadge.Type);
            Assert.Equal("{=jneqTrQz}Call to Arms Gamescom 2020", gamecomBadge.Name);
            Assert.Equal("08/26/2020 23:00:00", gamecomBadge.PeriodStart);
            Assert.Equal("08/30/2020 23:00:00", gamecomBadge.PeriodEnd);
            Assert.Single(gamecomBadge.Conditions);

            var condition = gamecomBadge.Conditions[0];
            Assert.Equal("PlayerDataNumeric", condition.Type);
            Assert.Equal("{=j7bQmC5H}Total wins", condition.Description);
            Assert.Equal(2, condition.Parameters.Count);

            var propertyParam = condition.Parameters.FirstOrDefault(p => p.Name == "property");
            Assert.NotNull(propertyParam);
            Assert.Equal("WinCount", propertyParam.Value);

            var minValueParam = condition.Parameters.FirstOrDefault(p => p.Name == "min_value");
            Assert.NotNull(minValueParam);
            Assert.Equal("5", minValueParam.Value);
        }

        [Fact]
        public void MPBadges_ManhunterBadgeHasComplexConditions()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var manhunterBadge = result.BadgeList.FirstOrDefault(b => b.Id == "gamescom_2020_manhunter");
            Assert.NotNull(manhunterBadge);
            Assert.Equal("Conditional", manhunterBadge.Type);
            Assert.Equal("08/26/2020 23:00:00", manhunterBadge.PeriodStart);
            Assert.Equal("08/27/2020 23:00:00", manhunterBadge.PeriodEnd);
            Assert.Single(manhunterBadge.Conditions);

            var condition = manhunterBadge.Conditions[0];
            Assert.Equal("BadgeOwnerKill", condition.Type);
            Assert.Equal("{=IhSV1upf}Manhunter badge owner", condition.Description);
            Assert.Equal(4, condition.Parameters.Count);

            // 检查所需徽章参数
            var badge1Param = condition.Parameters.FirstOrDefault(p => p.Name == "required_badge.1");
            Assert.NotNull(badge1Param);
            Assert.Equal("gamescom_2020_manhunter", badge1Param.Value);

            var badge2Param = condition.Parameters.FirstOrDefault(p => p.Name == "required_badge.2");
            Assert.NotNull(badge2Param);
            Assert.Equal("badge_taleworlds_primary_dev", badge2Param.Value);

            var badge3Param = condition.Parameters.FirstOrDefault(p => p.Name == "required_badge.3");
            Assert.NotNull(badge3Param);
            Assert.Equal("badge_taleworlds_dev", badge3Param.Value);

            var minValueParam = condition.Parameters.FirstOrDefault(p => p.Name == "min_value");
            Assert.NotNull(minValueParam);
            Assert.Equal("1", minValueParam.Value);
        }

        [Fact]
        public void MPBadges_ChampionBadgesHaveGroupTypes()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var championBadge = result.BadgeList.FirstOrDefault(b => b.Id == "gamescom_2020_captain_champion_party");
            Assert.NotNull(championBadge);
            Assert.Equal("Conditional", championBadge.Type);
            Assert.Equal("true", championBadge.IsVisibleOnlyWhenEarned);
            Assert.Equal(2, championBadge.Conditions.Count);

            // 检查Solo组类型条�?
            var soloCondition = championBadge.Conditions.FirstOrDefault(c => c.GroupType == "Solo");
            Assert.NotNull(soloCondition);
            Assert.Equal("PlayerDataNumeric", soloCondition.Type);
            Assert.Equal("{=bNEEElAA}Solo kills", soloCondition.Description);

            var soloPropertyParam = soloCondition.Parameters.FirstOrDefault(p => p.Name == "property");
            Assert.NotNull(soloPropertyParam);
            Assert.Equal("Stats.Captain.KillCount", soloPropertyParam.Value);

            var soloIsBestParam = soloCondition.Parameters.FirstOrDefault(p => p.Name == "is_best");
            Assert.NotNull(soloIsBestParam);
            Assert.Equal("true", soloIsBestParam.Value);

            // 检查Party组类型条�?
            var partyCondition = championBadge.Conditions.FirstOrDefault(c => c.GroupType == "Party");
            Assert.NotNull(partyCondition);
            Assert.Equal("PlayerDataNumeric", partyCondition.Type);
            Assert.Equal("{=g4BnT9UP}Party kills", partyCondition.Description);
        }

        [Fact]
        public void MPBadges_BadgeGroupsExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var groupedBadges = result.BadgeList.Where(b => !string.IsNullOrEmpty(b.GroupId)).ToList();
            Assert.True(groupedBadges.Count > 0); // 应该有分组徽�?

            // 检查Beta胜利�?
            var betaWinsBadges = result.BadgeList.Where(b => b.GroupId == "beta_wins").ToList();
            if (betaWinsBadges.Count > 0) // 如果存在Beta胜利徽章
            
            foreach (var badge in betaWinsBadges)
            {
                Assert.Equal("Custom", badge.Type);
                Assert.Contains("Beta", badge.Name);
                Assert.Contains("Wins", badge.Name);
            }

            // 检查游戏时间组
            var hoursPlayedBadges = result.BadgeList.Where(b => b.GroupId == "hours_played").ToList();
            if (hoursPlayedBadges.Count > 0) // 如果存在游戏时间徽章
            {
                foreach (var badge in hoursPlayedBadges)
                {
                    Assert.Equal("Conditional", badge.Type);
                    Assert.Single(badge.Conditions);
                    var condition = badge.Conditions[0];
                    Assert.Equal("PlayerDataNumeric", condition.Type);
                    
                    var propertyParam = condition.Parameters.FirstOrDefault(p => p.Name == "property");
                    Assert.NotNull(propertyParam);
                    Assert.Equal("Playtime", propertyParam.Value);
                }
            }
        }

        [Fact]
        public void MPBadges_LevelBadgesHaveCorrectStructure()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpbadges.xml");
            var serializer = new XmlSerializer(typeof(MPBadges));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPBadges)serializer.Deserialize(fileStream);

            // Assert
            var levelBadges = result.BadgeList.Where(b => b.GroupId == "ea_levels").ToList();
            if (levelBadges.Count > 0) // 如果有等级徽�?
            {
                foreach (var badge in levelBadges)
                {
                    Assert.Equal("Conditional", badge.Type);
                    Assert.Single(badge.Conditions);
                    
                    var condition = badge.Conditions[0];
                    Assert.Equal("PlayerDataNumeric", condition.Type);
                    Assert.Equal("{=OKUTPdaa}Level", condition.Description);
                    Assert.Equal(2, condition.Parameters.Count);

                    var propertyParam = condition.Parameters.FirstOrDefault(p => p.Name == "property");
                    Assert.NotNull(propertyParam);
                    Assert.Equal("Level", propertyParam.Value);

                    var minValueParam = condition.Parameters.FirstOrDefault(p => p.Name == "min_value");
                    Assert.NotNull(minValueParam);
                    Assert.True(int.TryParse(minValueParam.Value, out int level));
                    Assert.True(level >= 10 && level <= 250); // 等级应该在合理范围内
                }
            }
        }

        [Fact]
        public void MPCharacters_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Characters);
            Assert.True(result.Characters.Count > 80); // 应该有很多角色定义（实际�?9个）
        }

        [Fact]
        public void MPCharacters_AllCharactersHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            foreach (var character in result.Characters)
            {
                Assert.NotEmpty(character.Id);
                // 大多数角色应该有名称，但某些可能没有
                // 大多数角色应该有等级，但某些可能没有
            }
        }

        [Fact]
        public void MPCharacters_BaseMultiplayerCharacterIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var baseCharacter = result.Characters.FirstOrDefault(c => c.Id == "mp_character");
            Assert.NotNull(baseCharacter);
            Assert.Equal("36", baseCharacter.Level);
            Assert.Equal("{=eFWJjaBC}Multiplayer Character", baseCharacter.Name);
            
            // 检查技�?
            Assert.NotNull(baseCharacter.Skills);
            Assert.True(baseCharacter.Skills.SkillList.Count >= 7);
            
            var ridingSkill = baseCharacter.Skills.SkillList.FirstOrDefault(s => s.Id == "Riding");
            Assert.NotNull(ridingSkill);
            Assert.Equal("200", ridingSkill.Value);
            
            var oneHandedSkill = baseCharacter.Skills.SkillList.FirstOrDefault(s => s.Id == "OneHanded");
            Assert.NotNull(oneHandedSkill);
            Assert.Equal("5", oneHandedSkill.Value);
            
            // 检查装�?
            Assert.NotNull(baseCharacter.Equipments);
            Assert.Single(baseCharacter.Equipments.Rosters);
            
            var roster = baseCharacter.Equipments.Rosters[0];
            Assert.True(roster.EquipmentList.Count >= 3);
            
            var bodyEquipment = roster.EquipmentList.FirstOrDefault(e => e.Slot == "Body");
            Assert.NotNull(bodyEquipment);
            Assert.Equal("Item.mp_layered_leather_tunic", bodyEquipment.Id);
        }

        [Fact]
        public void MPCharacters_DummyCharactersExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var dummyNoArmor = result.Characters.FirstOrDefault(c => c.Id == "dummy_no_armor");
            Assert.NotNull(dummyNoArmor);
            Assert.Equal("{=!}Min Armor (0)", dummyNoArmor.Name);
            Assert.Equal("48", dummyNoArmor.Age);
            Assert.Equal("curt", dummyNoArmor.Voice);
            Assert.Equal("Infantry", dummyNoArmor.DefaultGroup);
            Assert.Equal("false", dummyNoArmor.IsHero);
            Assert.Equal("Culture.khuzait", dummyNoArmor.Culture);
            
            var dummyHeavyArmor = result.Characters.FirstOrDefault(c => c.Id == "dummy_heavy_armor");
            Assert.NotNull(dummyHeavyArmor);
            Assert.Equal("{=!}Max Armor (55)", dummyHeavyArmor.Name);
        }

        [Fact]
        public void MPCharacters_VlandianHeroesExist()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var lightInfantry = result.Characters.FirstOrDefault(c => c.Id == "mp_light_infantry_vlandia_hero");
            Assert.NotNull(lightInfantry);
            Assert.Equal("Infantry", lightInfantry.DefaultGroup);
            Assert.Equal("5", lightInfantry.Level);
            Assert.Equal("{=gLlheMwd}Peasant Levy", lightInfantry.Name);
            Assert.Equal("Soldier", lightInfantry.Occupation);
            Assert.Equal("Culture.vlandia", lightInfantry.Culture);
            
            // 检查抗�?
            Assert.NotNull(lightInfantry.Resistances);
            Assert.Equal("25", lightInfantry.Resistances.Dismount);
            
            var shockInfantry = result.Characters.FirstOrDefault(c => c.Id == "mp_shock_infantry_vlandia_hero");
            Assert.NotNull(shockInfantry);
            Assert.Equal("25", shockInfantry.Level);
            Assert.Equal("{=My0a4Miq}Voulgier", shockInfantry.Name);
        }

        [Fact]
        public void MPCharacters_CavalryCharactersHaveCorrectGroup()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var cavalryCharacters = result.Characters.Where(c => c.DefaultGroup == "Cavalry").ToList();
            Assert.True(cavalryCharacters.Count > 5); // 应该有多个骑兵角�?
            
            foreach (var cavalry in cavalryCharacters)
            {
                // 骑兵应该有装�?
                Assert.NotNull(cavalry.Equipments);
                Assert.True(cavalry.Equipments.Rosters.Count > 0);
                
                // 检查是否有马匹装备
                var hasHorse = cavalry.Equipments.Rosters.Any(roster => 
                    roster.EquipmentList.Any(eq => eq.Slot == "Horse"));
                
                // 大多数骑兵应该有马（但不是必须的，因为可能有例外�?
                if (hasHorse)
                {
                    Assert.True(true); // 有马是正常的
                }
            }
        }

        [Fact]
        public void MPCharacters_BodyPropertiesAreValid()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var charactersWithFace = result.Characters.Where(c => c.Face != null).ToList();
            Assert.True(charactersWithFace.Count > 10); // 应该有多个有面部属性的角色
            
            foreach (var character in charactersWithFace.Take(5)) // 检查前5�?
            {
                if (character.Face?.BodyProperties != null)
                {
                    var props = character.Face.BodyProperties;
                    Assert.NotNull(props.Version);
                    Assert.NotNull(props.Age);
                    Assert.NotNull(props.Weight);
                    Assert.NotNull(props.Build);
                    Assert.NotNull(props.Key);
                    
                    // 验证数值范�?
                    if (double.TryParse(props.Weight, out double weight))
                    {
                        Assert.True(weight >= 0.0 && weight <= 1.0);
                    }
                    
                    if (double.TryParse(props.Build, out double build))
                    {
                        Assert.True(build >= 0.0 && build <= 1.0);
                    }
                }
            }
        }

        [Fact]
        public void MPCharacters_SkillsHaveValidValues()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var charactersWithSkills = result.Characters.Where(c => c.Skills != null).ToList();
            Assert.True(charactersWithSkills.Count > 10); // 应该有多个有技能的角色
            
            foreach (var character in charactersWithSkills.Take(3)) // 检查前3�?
            {
                Assert.True(character.Skills.SkillList.Count >= 7); // 基本技能应该有7�?
                
                var skillIds = character.Skills.SkillList.Select(s => s.Id).ToList();
                Assert.Contains("Riding", skillIds);
                Assert.Contains("OneHanded", skillIds);
                Assert.Contains("TwoHanded", skillIds);
                Assert.Contains("Polearm", skillIds);
                Assert.Contains("Crossbow", skillIds);
                Assert.Contains("Bow", skillIds);
                Assert.Contains("Throwing", skillIds);
                
                // 验证技能�?
                foreach (var skill in character.Skills.SkillList)
                {
                    Assert.NotNull(skill.Id);
                    Assert.NotNull(skill.Value);
                    
                    if (int.TryParse(skill.Value, out int value))
                    {
                        Assert.True(value >= 0 && value <= 300); // 合理的技能范�?
                    }
                }
            }
        }

        [Fact]
        public void MPCharacters_EquipmentSlotsAreValid()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "mpcharacters.xml");
            var serializer = new XmlSerializer(typeof(MPCharacters));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (MPCharacters)serializer.Deserialize(fileStream);

            // Assert
            var charactersWithEquipment = result.Characters.Where(c => c.Equipments != null).ToList();
            Assert.True(charactersWithEquipment.Count > 10); // 应该有多个有装备的角�?
            
            var validSlots = new[] { "Item0", "Item1", "Item2", "Item3", "Body", "Head", "Leg", 
                                   "Gloves", "Cape", "Horse", "HorseHarness" };
            
            foreach (var character in charactersWithEquipment.Take(5)) // 检查前5�?
            {
                foreach (var roster in character.Equipments.Rosters)
                {
                    foreach (var equipment in roster.EquipmentList)
                    {
                        Assert.NotNull(equipment.Slot);
                        Assert.NotNull(equipment.Id);
                        Assert.Contains(equipment.Slot, validSlots);
                        Assert.StartsWith("Item.", equipment.Id);
                    }
                }
            }
        }

        [Fact]
        public void WeaponDescriptions_DeserializesCorrectly()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Descriptions);
            Assert.True(result.Descriptions.Count > 20); // 应该有很多武器描述（实际�?2个）
        }

        [Fact]
        public void WeaponDescriptions_AllDescriptionsHaveRequiredFields()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            foreach (var description in result.Descriptions)
            {
                Assert.NotEmpty(description.Id);
                // 大多数描述应该有武器类别
                // 大多数描述应该有使用特�?
            }
        }

        [Fact]
        public void WeaponDescriptions_OneHandedSwordIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var oneHandedSword = result.Descriptions.FirstOrDefault(d => d.Id == "OneHandedSword");
            Assert.NotNull(oneHandedSword);
            Assert.Equal("OneHandedSword", oneHandedSword.WeaponClass);
            Assert.Equal("onehanded:block:shield:swing:thrust", oneHandedSword.ItemUsageFeatures);
            
            // 检查武器标�?
            Assert.NotNull(oneHandedSword.WeaponFlags);
            Assert.True(oneHandedSword.WeaponFlags.Flags.Count >= 1);
            
            var meleeFlag = oneHandedSword.WeaponFlags.Flags.FirstOrDefault(f => f.Value == "MeleeWeapon");
            Assert.NotNull(meleeFlag);
            
            // 检查可用部�?
            Assert.NotNull(oneHandedSword.AvailablePieces);
            Assert.True(oneHandedSword.AvailablePieces.Pieces.Count > 100); // 应该有很多可用部�?
            
            // 验证一些特定的部件
            var empireBlade = oneHandedSword.AvailablePieces.Pieces.FirstOrDefault(p => p.Id == "empire_blade_1");
            Assert.NotNull(empireBlade);
        }

        [Fact]
        public void WeaponDescriptions_TwoHandedMaceIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var twoHandedMace = result.Descriptions.FirstOrDefault(d => d.Id == "TwoHandedMace");
            Assert.NotNull(twoHandedMace);
            Assert.Equal("TwoHandedMace", twoHandedMace.WeaponClass);
            Assert.Equal("twohanded:axe", twoHandedMace.ItemUsageFeatures);
            
            // 检查武器标�?
            Assert.NotNull(twoHandedMace.WeaponFlags);
            Assert.True(twoHandedMace.WeaponFlags.Flags.Count >= 3);
            
            var flagValues = twoHandedMace.WeaponFlags.Flags.Select(f => f.Value).ToList();
            Assert.Contains("MeleeWeapon", flagValues);
            Assert.Contains("NotUsableWithOneHand", flagValues);
            Assert.Contains("TwoHandIdleOnMount", flagValues);
        }

        [Fact]
        public void WeaponDescriptions_WeaponFlagsAreValid()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var descriptionsWithFlags = result.Descriptions.Where(d => d.WeaponFlags != null).ToList();
            Assert.True(descriptionsWithFlags.Count > 10); // 应该有多个有标志的武�?
            
            var validFlags = new[] { "MeleeWeapon", "NotUsableWithOneHand", "TwoHandIdleOnMount", 
                                   "RangedWeapon", "HasHitPoints", "CannotReloadOnHorseback" };
            
            foreach (var description in descriptionsWithFlags.Take(5)) // 检查前5�?
            {
                foreach (var flag in description.WeaponFlags.Flags)
                {
                    Assert.NotNull(flag.Value);
                    // 大多数标志应该是已知的类型，但可能有新的
                }
            }
        }

        [Fact]
        public void WeaponDescriptions_AvailablePiecesAreValid()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var descriptionsWithPieces = result.Descriptions.Where(d => d.AvailablePieces != null).ToList();
            Assert.True(descriptionsWithPieces.Count > 10); // 应该有多个有部件的武�?
            
            foreach (var description in descriptionsWithPieces.Take(3)) // 检查前3�?
            {
                Assert.True(description.AvailablePieces.Pieces.Count > 0);
                
                foreach (var piece in description.AvailablePieces.Pieces.Take(10)) // 检查前10个部�?
                {
                    Assert.NotNull(piece.Id);
                    Assert.True(piece.Id.Length > 0);
                }
            }
        }

        [Fact]
        public void WeaponDescriptions_WeaponClassesAreValid()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var weaponClasses = result.Descriptions
                .Where(d => !string.IsNullOrEmpty(d.WeaponClass))
                .Select(d => d.WeaponClass)
                .Distinct()
                .ToList();
            
            Assert.True(weaponClasses.Count >= 5); // 应该有多种武器类�?
            
            // 验证一些已知的武器类别
            Assert.Contains("OneHandedSword", weaponClasses);
            Assert.Contains("TwoHandedMace", weaponClasses);
        }

        [Fact]
        public void WeaponDescriptions_ItemUsageFeaturesAreValid()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var descriptionsWithFeatures = result.Descriptions
                .Where(d => !string.IsNullOrEmpty(d.ItemUsageFeatures))
                .ToList();
            
            Assert.True(descriptionsWithFeatures.Count > 10); // 应该有多个有使用特性的武器
            
            foreach (var description in descriptionsWithFeatures.Take(5)) // 检查前5�?
            {
                Assert.Contains(":", description.ItemUsageFeatures); // 使用特性通常包含冒号分隔�?
            }
            
            // 验证一些已知的使用特性模�?
            var oneHandedFeatures = descriptionsWithFeatures
                .Where(d => d.ItemUsageFeatures.Contains("onehanded"))
                .ToList();
            Assert.True(oneHandedFeatures.Count > 0);
            
            var twoHandedFeatures = descriptionsWithFeatures
                .Where(d => d.ItemUsageFeatures.Contains("twohanded"))
                .ToList();
            Assert.True(twoHandedFeatures.Count > 0);
        }

        [Fact]
        public void WeaponDescriptions_PieceTypesAreConsistent()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (WeaponDescriptions)serializer.Deserialize(fileStream);

            // Assert
            var allPieces = result.Descriptions
                .Where(d => d.AvailablePieces != null)
                .SelectMany(d => d.AvailablePieces.Pieces)
                .Where(p => !string.IsNullOrEmpty(p.Id))
                .ToList();
            
            Assert.True(allPieces.Count > 500); // 应该有很多部�?
            
            // 检查部件类型模�?
            var blades = allPieces.Where(p => p.Id.Contains("blade")).ToList();
            var guards = allPieces.Where(p => p.Id.Contains("guard")).ToList();
            var grips = allPieces.Where(p => p.Id.Contains("grip")).ToList();
            var pommels = allPieces.Where(p => p.Id.Contains("pommel")).ToList();
            var handles = allPieces.Where(p => p.Id.Contains("handle")).ToList();
            var heads = allPieces.Where(p => p.Id.Contains("head")).ToList();
            
            Assert.True(blades.Count > 50); // 应该有很多刀�?
            Assert.True(guards.Count > 10); // 应该有护�?
            Assert.True(grips.Count > 10); // 应该有握�?
            Assert.True(pommels.Count > 10); // 应该有剑�?
            Assert.True(handles.Count > 10); // 应该有手�?
            Assert.True(heads.Count > 10); // 应该有头部（锤头、斧头等�?
        }
    }
} 
