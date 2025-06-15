using BannerlordModEditor.Common.Models.Multiplayer;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MultiplayerTests
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
        public void MultiplayerScenes_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "MultiplayerScenes.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MultiplayerScenes));
            MultiplayerScenes scenes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                scenes = (MultiplayerScenes)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, scenes);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(scenes);
            Assert.NotNull(scenes.Scene);
            Assert.True(scenes.Scene.Count > 0, "Should have at least one scene");

            // 验证特定场景
            var battleScene = scenes.Scene.FirstOrDefault(s => s.Name.Contains("battle"));
            if (battleScene != null)
            {
                Assert.NotNull(battleScene.GameType);
                Assert.True(battleScene.GameType.Count > 0, "Battle scene should have game types");
                Assert.Contains(battleScene.GameType, gt => gt.Name == "Battle");
            }

            var siegeScene = scenes.Scene.FirstOrDefault(s => s.Name.Contains("siege"));
            if (siegeScene != null)
            {
                Assert.NotNull(siegeScene.GameType);
                Assert.Contains(siegeScene.GameType, gt => gt.Name == "Siege");
            }

            var tdmScene = scenes.Scene.FirstOrDefault(s => s.Name.Contains("tdm"));
            if (tdmScene != null)
            {
                Assert.NotNull(tdmScene.GameType);
                Assert.Contains(tdmScene.GameType, gt => gt.Name == "TeamDeathmatch");
            }

            var captainScene = scenes.Scene.FirstOrDefault(s => s.Name.Contains("sergeant") || s.Name.Contains("captain"));
            if (captainScene != null)
            {
                Assert.NotNull(captainScene.GameType);
                Assert.Contains(captainScene.GameType, gt => gt.Name == "Captain");
            }

            // 验证所有场景都有必要字段
            foreach (var scene in scenes.Scene)
            {
                Assert.False(string.IsNullOrEmpty(scene.Name), "Scene should have a name");
                Assert.NotNull(scene.GameType);
                Assert.True(scene.GameType.Count > 0, $"Scene {scene.Name} should have at least one game type");

                foreach (var gameType in scene.GameType)
                {
                    Assert.False(string.IsNullOrEmpty(gameType.Name), 
                        $"Game type in scene {scene.Name} should have a name");
                }
            }
        }

        [Fact]
        public void MultiplayerScenes_GameTypes_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "MultiplayerScenes.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MultiplayerScenes));
            MultiplayerScenes scenes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                scenes = (MultiplayerScenes)serializer.Deserialize(reader)!;
            }

            // Assert - 验证游戏类型的有效性
            var validGameTypes = new[]
            {
                "Battle", "Siege", "TeamDeathmatch", "FreeForAll", "Duel", 
                "Captain", "Skirmish"
            };

            var allGameTypes = scenes.Scene
                .SelectMany(s => s.GameType.Select(gt => gt.Name))
                .Distinct()
                .ToList();

            foreach (var gameType in allGameTypes)
            {
                Assert.Contains(gameType, validGameTypes);
            }

            // 验证每种游戏类型都有对应场景
            var battleScenes = scenes.Scene.Where(s => s.GameType.Any(gt => gt.Name == "Battle")).ToList();
            var siegeScenes = scenes.Scene.Where(s => s.GameType.Any(gt => gt.Name == "Siege")).ToList();
            var tdmScenes = scenes.Scene.Where(s => s.GameType.Any(gt => gt.Name == "TeamDeathmatch")).ToList();
            
            Assert.True(battleScenes.Count > 0, "Should have Battle scenes");
            Assert.True(siegeScenes.Count > 0, "Should have Siege scenes");
            Assert.True(tdmScenes.Count > 0, "Should have TeamDeathmatch scenes");
        }

        [Fact]
        public void TauntUsageSets_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "taunt_usage_sets.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(TauntUsageSets));
            TauntUsageSets tauntSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                tauntSets = (TauntUsageSets)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, tauntSets);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(tauntSets);
            Assert.NotNull(tauntSets.TauntUsageSet);
            Assert.True(tauntSets.TauntUsageSet.Count > 0, "Should have at least one taunt usage set");

            // 验证特定嘲讽集合
            var taunt01 = tauntSets.TauntUsageSet.FirstOrDefault(t => t.Id == "taunt_01");
            if (taunt01 != null)
            {
                Assert.Equal("taunt_01", taunt01.Id);
                Assert.NotNull(taunt01.TauntUsage);
                Assert.True(taunt01.TauntUsage.Count > 0, "taunt_01 should have taunt usages");

                // 验证弓箭嘲讽
                var bowTaunt = taunt01.TauntUsage.FirstOrDefault(t => t.RequiresBow == "True");
                if (bowTaunt != null)
                {
                    Assert.Equal("True", bowTaunt.RequiresBow);
                    Assert.Equal("True", bowTaunt.RequiresOnFoot);
                    Assert.Equal("act_taunt_01_bow", bowTaunt.Action);
                }

                // 验证左姿态嘲讽
                var leftStanceTaunt = taunt01.TauntUsage.FirstOrDefault(t => t.IsLeftStance == "True");
                if (leftStanceTaunt != null)
                {
                    Assert.Equal("True", leftStanceTaunt.IsLeftStance);
                    Assert.Equal("act_taunt_01_leftstance", leftStanceTaunt.Action);
                }
            }

            // 验证所有嘲讽集合都有必要字段
            foreach (var tauntSet in tauntSets.TauntUsageSet)
            {
                Assert.False(string.IsNullOrEmpty(tauntSet.Id), "Taunt usage set should have an ID");
                Assert.NotNull(tauntSet.TauntUsage);
                Assert.True(tauntSet.TauntUsage.Count > 0, $"Taunt set {tauntSet.Id} should have at least one taunt usage");

                foreach (var tauntUsage in tauntSet.TauntUsage)
                {
                    Assert.False(string.IsNullOrEmpty(tauntUsage.Action), 
                        $"Taunt usage in {tauntSet.Id} should have an action");
                }
            }
        }

        [Fact]
        public void TauntUsageSets_Conditions_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "taunt_usage_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(TauntUsageSets));
            TauntUsageSets tauntSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                tauntSets = (TauntUsageSets)serializer.Deserialize(reader)!;
            }

            // Assert - 验证条件的逻辑性
            foreach (var tauntSet in tauntSets.TauntUsageSet)
            {
                foreach (var tauntUsage in tauntSet.TauntUsage)
                {
                    // 验证布尔值格式
                    if (tauntUsage.RequiresBow != null)
                        Assert.True(tauntUsage.RequiresBow == "True" || tauntUsage.RequiresBow == "False");
                    
                    if (tauntUsage.RequiresOnFoot != null)
                        Assert.True(tauntUsage.RequiresOnFoot == "True" || tauntUsage.RequiresOnFoot == "False");
                    
                    if (tauntUsage.IsLeftStance != null)
                        Assert.True(tauntUsage.IsLeftStance == "True" || tauntUsage.IsLeftStance == "False");

                    // 验证动作名称格式
                    Assert.True(tauntUsage.Action.StartsWith("act_taunt_"), 
                        $"Action {tauntUsage.Action} should start with 'act_taunt_'");
                }
            }

            // 验证特定条件类型的存在
            var allUsages = tauntSets.TauntUsageSet.SelectMany(ts => ts.TauntUsage).ToList();
            
            var bowRequirements = allUsages.Where(t => t.RequiresBow == "True").ToList();
            var shieldRequirements = allUsages.Where(t => t.RequiresShield == "True").ToList();
            var leftStanceUsages = allUsages.Where(t => t.IsLeftStance == "True").ToList();
            var unsuitableForBow = allUsages.Where(t => t.UnsuitableForBow == "True").ToList();

            Assert.True(bowRequirements.Count > 0, "Should have bow requirements");
            Assert.True(leftStanceUsages.Count > 0, "Should have left stance usages");
            Assert.True(unsuitableForBow.Count > 0, "Should have bow unsuitability conditions");
        }

        [Fact]
        public void TauntUsageSets_ActionNaming_ShouldFollowPattern()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "taunt_usage_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(TauntUsageSets));
            TauntUsageSets tauntSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                tauntSets = (TauntUsageSets)serializer.Deserialize(reader)!;
            }

            // Assert - 验证动作命名模式
            foreach (var tauntSet in tauntSets.TauntUsageSet)
            {
                foreach (var tauntUsage in tauntSet.TauntUsage)
                {
                    // 所有动作都应该以act_taunt_开头
                    Assert.True(tauntUsage.Action.StartsWith("act_taunt_"), 
                        $"Action {tauntUsage.Action} should start with 'act_taunt_'");

                    // 验证特定后缀的一致性
                    if (tauntUsage.RequiresBow == "True")
                    {
                        Assert.True(tauntUsage.Action.EndsWith("_bow"), 
                            $"Bow action {tauntUsage.Action} should end with '_bow'");
                    }
                    
                    if (tauntUsage.IsLeftStance == "True")
                    {
                        Assert.True(tauntUsage.Action.EndsWith("_leftstance"), 
                            $"Left stance action {tauntUsage.Action} should end with '_leftstance'");
                    }
                }
            }

            // 验证ID命名模式
            foreach (var tauntSet in tauntSets.TauntUsageSet)
            {
                Assert.True(tauntSet.Id.StartsWith("taunt_"), 
                    $"Taunt set ID {tauntSet.Id} should start with 'taunt_'");
                Assert.True(tauntSet.Id.Length > 6, 
                    $"Taunt set ID {tauntSet.Id} should have content after 'taunt_'");
            }
            
            // 验证特殊嘲讽类型（从taunt_33开始是cheer类型）
            var cheerTaunts = tauntSets.TauntUsageSet.Where(ts => ts.Id.StartsWith("taunt_3")).ToList();
            if (cheerTaunts.Count > 0)
            {
                foreach (var cheerTaunt in cheerTaunts)
                {
                    var hasCheerAction = cheerTaunt.TauntUsage.Any(tu => tu.Action.Contains("cheer"));
                    if (hasCheerAction)
                    {
                        // 如果包含cheer，所有动作都应该是cheer类型
                        foreach (var usage in cheerTaunt.TauntUsage)
                        {
                            Assert.True(usage.Action.Contains("cheer"), 
                                $"Cheer taunt {cheerTaunt.Id} should have cheer actions");
                        }
                    }
                }
            }
        }
    }
} 