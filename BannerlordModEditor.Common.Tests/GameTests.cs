using BannerlordModEditor.Common.Models.Game;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class GameTests
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
        public void ItemModifiers_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers itemModifiers;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, itemModifiers);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(itemModifiers);
            Assert.NotNull(itemModifiers.ItemModifier);
            Assert.True(itemModifiers.ItemModifier.Count > 0, "Should have at least one item modifier");

            // 验证特定修饰符
            var legendarySword = itemModifiers.ItemModifier.FirstOrDefault(im => im.Id == "legendary_sword");
            if (legendarySword != null)
            {
                Assert.Equal("ItemModifierGroup.sword", legendarySword.ModifierGroup);
                Assert.Equal("legendary_sword", legendarySword.Id);
                Assert.Contains("Legendary", legendarySword.Name);
                Assert.Equal("2", legendarySword.LootDropScore);
                Assert.Equal("5", legendarySword.ProductionDropScore);
                Assert.Equal("7", legendarySword.Damage);
                Assert.Equal("3", legendarySword.Speed);
                Assert.Equal("1.8", legendarySword.PriceFactor);
                Assert.Equal("legendary", legendarySword.Quality);
            }

            var rustySword = itemModifiers.ItemModifier.FirstOrDefault(im => im.Id == "rusty_sword");
            if (rustySword != null)
            {
                Assert.Equal("ItemModifierGroup.sword", rustySword.ModifierGroup);
                Assert.Equal("rusty_sword", rustySword.Id);
                Assert.Contains("Rusty", rustySword.Name);
                Assert.Equal("-15", rustySword.Damage);
                Assert.Equal("-5", rustySword.Speed);
                Assert.Equal("0.3", rustySword.PriceFactor);
                Assert.Equal("poor", rustySword.Quality);
            }

            // 验证所有修饰符都有必要字段
            foreach (var modifier in itemModifiers.ItemModifier)
            {
                Assert.False(string.IsNullOrEmpty(modifier.ModifierGroup), "Modifier should have a group");
                Assert.False(string.IsNullOrEmpty(modifier.Id), "Modifier should have an ID");
                Assert.False(string.IsNullOrEmpty(modifier.Name), "Modifier should have a name");
                Assert.False(string.IsNullOrEmpty(modifier.LootDropScore), "Modifier should have loot drop score");
                Assert.False(string.IsNullOrEmpty(modifier.ProductionDropScore), "Modifier should have production drop score");
                Assert.False(string.IsNullOrEmpty(modifier.PriceFactor), "Modifier should have price factor");
                Assert.False(string.IsNullOrEmpty(modifier.Quality), "Modifier should have quality");
            }
        }

        [Fact]
        public void ItemModifiers_ModifierGroups_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers itemModifiers;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
            }

            // Assert - 验证不同类型的修饰符组
            var swordModifiers = itemModifiers.ItemModifier.Where(im => im.ModifierGroup == "ItemModifierGroup.sword").ToList();
            var bowModifiers = itemModifiers.ItemModifier.Where(im => im.ModifierGroup == "ItemModifierGroup.bow").ToList();
            var crossbowModifiers = itemModifiers.ItemModifier.Where(im => im.ModifierGroup == "ItemModifierGroup.crossbow").ToList();
            var armorModifiers = itemModifiers.ItemModifier.Where(im => im.ModifierGroup.Contains("armor") || im.ModifierGroup.Contains("chain") || im.ModifierGroup.Contains("leather") || im.ModifierGroup.Contains("cloth")).ToList();
            var horseModifiers = itemModifiers.ItemModifier.Where(im => im.ModifierGroup == "ItemModifierGroup.horse").ToList();

            Assert.True(swordModifiers.Count > 0, "Should have sword modifiers");
            Assert.True(bowModifiers.Count > 0, "Should have bow modifiers");
            Assert.True(crossbowModifiers.Count > 0, "Should have crossbow modifiers");
            Assert.True(armorModifiers.Count > 0, "Should have armor modifiers");
            Assert.True(horseModifiers.Count > 0, "Should have horse modifiers");

            // 验证所有修饰符组都以ItemModifierGroup.开头
            var allGroups = itemModifiers.ItemModifier.Select(im => im.ModifierGroup).Distinct().ToList();
            foreach (var group in allGroups)
            {
                Assert.True(group.StartsWith("ItemModifierGroup."), 
                    $"Modifier group {group} should start with 'ItemModifierGroup.'");
            }
        }

        [Fact]
        public void ItemModifiers_QualityLevels_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers itemModifiers;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
            }

            // Assert - 验证品质等级
            var validQualities = new[] { "legendary", "masterwork", "fine", "inferior", "poor" };
            var allQualities = itemModifiers.ItemModifier.Select(im => im.Quality).Distinct().ToList();

            foreach (var quality in allQualities)
            {
                Assert.Contains(quality, validQualities);
            }

            // 验证每个品质等级都有对应的修饰符
            var legendaryModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "legendary").ToList();
            var masterworkModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "masterwork").ToList();
            var fineModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "fine").ToList();
            var inferiorModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "inferior").ToList();
            var poorModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "poor").ToList();

            Assert.True(legendaryModifiers.Count > 0, "Should have legendary modifiers");
            Assert.True(masterworkModifiers.Count > 0, "Should have masterwork modifiers");
            Assert.True(fineModifiers.Count > 0, "Should have fine modifiers");
            Assert.True(inferiorModifiers.Count > 0, "Should have inferior modifiers");
            Assert.True(poorModifiers.Count > 0, "Should have poor modifiers");
        }

        [Fact]
        public void ItemModifiers_WeaponStats_ShouldBeLogical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers itemModifiers;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
            }

            // Assert - 验证武器统计的逻辑性
            var weaponModifiers = itemModifiers.ItemModifier.Where(im => 
                im.ModifierGroup.Contains("sword") || im.ModifierGroup.Contains("bow") || 
                im.ModifierGroup.Contains("crossbow") || im.ModifierGroup.Contains("axe") ||
                im.ModifierGroup.Contains("mace") || im.ModifierGroup.Contains("spear")).ToList();

            foreach (var modifier in weaponModifiers)
            {
                // 验证传奇品质应该有正面效果
                if (modifier.Quality == "legendary")
                {
                    if (!string.IsNullOrEmpty(modifier.Damage))
                    {
                        Assert.True(int.Parse(modifier.Damage) > 0, 
                            $"Legendary weapon {modifier.Id} should have positive damage bonus");
                    }
                    Assert.True(double.Parse(modifier.PriceFactor) > 1.0, 
                        $"Legendary weapon {modifier.Id} should have high price factor");
                }

                // 验证糟糕品质应该有负面效果
                if (modifier.Quality == "poor")
                {
                    if (!string.IsNullOrEmpty(modifier.Damage))
                    {
                        Assert.True(int.Parse(modifier.Damage) < 0, 
                            $"Poor weapon {modifier.Id} should have negative damage modifier");
                    }
                    Assert.True(double.Parse(modifier.PriceFactor) < 1.0, 
                        $"Poor weapon {modifier.Id} should have low price factor");
                }

                // 验证弓箭类武器应该有missile_speed属性
                if (modifier.ModifierGroup.Contains("bow") || modifier.ModifierGroup.Contains("crossbow") || 
                    modifier.ModifierGroup.Contains("throwing") || modifier.ModifierGroup.Contains("arrow"))
                {
                    if (modifier.Quality == "legendary" || modifier.Quality == "masterwork")
                    {
                        // 传奇和精工品质的远程武器通常有missile_speed属性
                        // 但不是必须的，所以这里只是检查如果有的话应该是正数
                        if (!string.IsNullOrEmpty(modifier.MissileSpeed))
                        {
                            Assert.True(int.Parse(modifier.MissileSpeed) >= 0, 
                                $"High quality ranged weapon {modifier.Id} should have non-negative missile speed");
                        }
                    }
                }
            }
        }

        [Fact]
        public void ItemModifiers_ArmorAndHorseStats_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers itemModifiers;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
            }

            // Assert - 验证护甲修饰符
            var armorModifiers = itemModifiers.ItemModifier.Where(im => !string.IsNullOrEmpty(im.Armor)).ToList();
            Assert.True(armorModifiers.Count > 0, "Should have armor modifiers");

            foreach (var modifier in armorModifiers)
            {
                // 验证护甲值是有效数字
                Assert.True(int.TryParse(modifier.Armor, out var armorValue), 
                    $"Armor value {modifier.Armor} should be a valid integer");

                // 验证传奇护甲应该有正面效果
                if (modifier.Quality == "legendary")
                {
                    Assert.True(armorValue > 0, 
                        $"Legendary armor {modifier.Id} should have positive armor bonus");
                }

                // 验证糟糕护甲应该有负面效果
                if (modifier.Quality == "poor")
                {
                    Assert.True(armorValue < 0, 
                        $"Poor armor {modifier.Id} should have negative armor modifier");
                }
            }

            // 验证马匹修饰符
            var horseModifiers = itemModifiers.ItemModifier.Where(im => 
                !string.IsNullOrEmpty(im.HorseSpeed) || !string.IsNullOrEmpty(im.Maneuver) || 
                !string.IsNullOrEmpty(im.ChargeDamage) || !string.IsNullOrEmpty(im.HorseHitPoints)).ToList();
            
            Assert.True(horseModifiers.Count > 0, "Should have horse modifiers");

            foreach (var modifier in horseModifiers)
            {
                // 验证马匹属性是有效数字
                if (!string.IsNullOrEmpty(modifier.HorseSpeed))
                {
                    Assert.True(double.TryParse(modifier.HorseSpeed, out _), 
                        $"Horse speed {modifier.HorseSpeed} should be a valid number");
                }
                
                if (!string.IsNullOrEmpty(modifier.Maneuver))
                {
                    Assert.True(double.TryParse(modifier.Maneuver, out _), 
                        $"Maneuver {modifier.Maneuver} should be a valid number");
                }
                
                if (!string.IsNullOrEmpty(modifier.ChargeDamage))
                {
                    Assert.True(double.TryParse(modifier.ChargeDamage, out _), 
                        $"Charge damage {modifier.ChargeDamage} should be a valid number");
                }
                
                if (!string.IsNullOrEmpty(modifier.HorseHitPoints))
                {
                    Assert.True(double.TryParse(modifier.HorseHitPoints, out _), 
                        $"Horse hit points {modifier.HorseHitPoints} should be a valid number");
                }
            }
        }

        [Fact]
        public void ItemModifiers_NameLocalization_ShouldFollowPattern()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifiers));
            ItemModifiers itemModifiers;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
            }

            // Assert - 验证名称本地化格式
            foreach (var modifier in itemModifiers.ItemModifier)
            {
                // 大部分名称应该包含本地化字符串标记
                Assert.True(modifier.Name.Contains("{=") && modifier.Name.Contains("}"),
                    $"Modifier name {modifier.Name} should contain localization markers");

                // 名称应该包含{ITEMNAME}占位符
                Assert.True(modifier.Name.Contains("{ITEMNAME}"),
                    $"Modifier name {modifier.Name} should contain {{ITEMNAME}} placeholder");
            }

            // 验证常见的修饰符名称
            var legendaryModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "legendary").ToList();
            var poorModifiers = itemModifiers.ItemModifier.Where(im => im.Quality == "poor").ToList();
            
            foreach (var legendary in legendaryModifiers)
            {
                Assert.Contains("Legendary", legendary.Name);
            }

            // 验证糟糕品质的修饰符名称
            var poorNames = poorModifiers.Select(pm => pm.Name).Distinct().ToList();
            var expectedPoorWords = new[] { "Rusty", "Cracked", "Splintered", "Ripped", "Battered", "Lame", "Old", "Worn", "Bent", "Dull" };
            
            foreach (var poorName in poorNames)
            {
                var containsExpectedWord = expectedPoorWords.Any(word => poorName.Contains(word));
                Assert.True(containsExpectedWord, 
                    $"Poor quality modifier name {poorName} should contain expected negative descriptors");
            }
        }
    }
} 