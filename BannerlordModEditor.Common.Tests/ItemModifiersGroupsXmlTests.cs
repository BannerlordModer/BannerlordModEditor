using BannerlordModEditor.Common.Models.Game;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemModifiersGroupsXmlTests
    {
        [Fact]
        public void ItemModifiersGroups_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers_groups.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemModifierGroup);
            Assert.Equal(20, result.ItemModifierGroup.Count);
        }

        [Fact]
        public void ItemModifiersGroups_FromActualFile_CanDeserializeCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers_groups.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups itemModifiersGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiersGroups = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert - 基本数据完整性
            Assert.NotNull(itemModifiersGroups);
            Assert.NotNull(itemModifiersGroups.ItemModifierGroup);
            Assert.True(itemModifiersGroups.ItemModifierGroup.Count >= 15, "应该有至少15个修饰符组");
            
            // 验证所有组都有必需的属性
            foreach (var group in itemModifiersGroups.ItemModifierGroup)
            {
                Assert.False(string.IsNullOrWhiteSpace(group.Id), "修饰符组必须有ID");
                Assert.False(string.IsNullOrWhiteSpace(group.NoModifierLootScore), "修饰符组必须有无修饰符掠夺得分");
                Assert.False(string.IsNullOrWhiteSpace(group.NoModifierProductionScore), "修饰符组必须有无修饰符生产得分");
            }
            
            // 验证关键修饰符组的存在
            var requiredGroups = new[] { "sword", "bow", "crossbow", "shield", "plate", "horse", "companion" };
            foreach (var requiredGroup in requiredGroups)
            {
                var group = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == requiredGroup);
                Assert.NotNull(group);
                Assert.Equal(requiredGroup, group.Id);
            }
        }

        [Fact]
        public void ItemModifiersGroups_ValidateScoreValues_HaveConsistentRanges()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers_groups.xml");
            
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups itemModifiersGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiersGroups = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert - 验证分数值的数值有效性和范围
            foreach (var group in itemModifiersGroups.ItemModifierGroup)
            {
                // 验证数值可以解析
                Assert.True(int.TryParse(group.NoModifierLootScore, NumberStyles.Integer, CultureInfo.InvariantCulture, out int lootScore),
                    $"无修饰符掠夺得分应该是有效整数: {group.NoModifierLootScore} (组: {group.Id})");
                Assert.True(int.TryParse(group.NoModifierProductionScore, NumberStyles.Integer, CultureInfo.InvariantCulture, out int productionScore),
                    $"无修饰符生产得分应该是有效整数: {group.NoModifierProductionScore} (组: {group.Id})");
                
                // 验证数值范围合理性
                Assert.True(lootScore > 0 && lootScore <= 100, $"掠夺得分应在1-100范围内: {lootScore} (组: {group.Id})");
                Assert.True(productionScore > 0 && productionScore <= 100, $"生产得分应在1-100范围内: {productionScore} (组: {group.Id})");
            }
            
            // 验证特殊组的分数模式
            var weaponGroups = itemModifiersGroups.ItemModifierGroup.Where(g => 
                new[] { "sword", "bow", "crossbow", "arrow", "bolt", "polearm", "mace", "axe" }.Contains(g.Id)).ToList();
            var armorGroups = itemModifiersGroups.ItemModifierGroup.Where(g => 
                new[] { "plate", "chain", "leather", "cloth", "shield" }.Contains(g.Id)).ToList();
            var specialGroups = itemModifiersGroups.ItemModifierGroup.Where(g => 
                new[] { "horse", "companion" }.Contains(g.Id)).ToList();
            
            // 武器组应该有相似的分数
            if (weaponGroups.Any())
            {
                var firstWeaponLoot = int.Parse(weaponGroups.First().NoModifierLootScore!);
                var firstWeaponProduction = int.Parse(weaponGroups.First().NoModifierProductionScore!);
                
                foreach (var weapon in weaponGroups)
                {
                    Assert.Equal(firstWeaponLoot.ToString(), weapon.NoModifierLootScore);
                    Assert.Equal(firstWeaponProduction.ToString(), weapon.NoModifierProductionScore);
                }
            }
            
            // 特殊组（马匹、同伴）应该有较低的分数
            foreach (var special in specialGroups)
            {
                var lootScore = int.Parse(special.NoModifierLootScore!);
                var productionScore = int.Parse(special.NoModifierProductionScore!);
                
                Assert.True(lootScore <= 10, $"特殊组的掠夺得分应该较低: {lootScore} (组: {special.Id})");
                Assert.True(productionScore <= 10, $"特殊组的生产得分应该较低: {productionScore} (组: {special.Id})");
            }
        }

        [Fact]
        public void ItemModifiersGroups_ValidateIdUniqueness()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers_groups.xml");
            
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups itemModifiersGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiersGroups = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert
            var allIds = itemModifiersGroups.ItemModifierGroup.Select(g => g.Id).ToList();
            var uniqueIds = allIds.Distinct().ToList();
            
            Assert.Equal(allIds.Count, uniqueIds.Count);
            
            // 验证ID命名约定
            foreach (var id in allIds)
            {
                Assert.False(string.IsNullOrWhiteSpace(id), "ID不能为空");
                Assert.True(id.All(c => char.IsLower(c) || c == '_'), $"ID应该使用小写字母和下划线: {id}");
                Assert.False(id.StartsWith("_") || id.EndsWith("_"), $"ID不应该以下划线开头或结尾: {id}");
            }
        }

        [Fact]
        public void ItemModifiersGroups_ValidateSpecificConfigurations()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers_groups.xml");
            
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups itemModifiersGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiersGroups = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert - 验证特定配置的正确性
            var swordGroup = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == "sword");
            Assert.NotNull(swordGroup);
            Assert.Equal("50", swordGroup.NoModifierLootScore);
            Assert.Equal("65", swordGroup.NoModifierProductionScore);
            
            var horseGroup = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == "horse");
            Assert.NotNull(horseGroup);
            Assert.Equal("1", horseGroup.NoModifierLootScore);
            Assert.Equal("1", horseGroup.NoModifierProductionScore);
            
            var companionGroup = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == "companion");
            Assert.NotNull(companionGroup);
            Assert.Equal("1", companionGroup.NoModifierLootScore);
            Assert.Equal("1", companionGroup.NoModifierProductionScore);
            
            // 验证武器类型的一致性
            var weaponTypes = new[] { "sword", "bow", "crossbow", "polearm", "mace", "axe" };
            foreach (var weaponType in weaponTypes)
            {
                var weapon = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == weaponType);
                if (weapon != null)
                {
                    Assert.Equal("50", weapon.NoModifierLootScore);
                    Assert.Equal("65", weapon.NoModifierProductionScore);
                }
            }
        }

        [Fact]
        public void ItemModifiersGroups_RoundTripSerialization_MaintainsDataIntegrity()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers_groups.xml");
            
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups original;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                original = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化然后反序列化
            string xmlString;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlString = writer.ToString();
            }
            
            ItemModifierGroups roundTrip;
            using (var reader = new StringReader(xmlString))
            {
                roundTrip = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.Equal(original.ItemModifierGroup.Count, roundTrip.ItemModifierGroup.Count);
            
            for (int i = 0; i < original.ItemModifierGroup.Count; i++)
            {
                var originalGroup = original.ItemModifierGroup[i];
                var roundTripGroup = roundTrip.ItemModifierGroup[i];
                
                Assert.Equal(originalGroup.Id, roundTripGroup.Id);
                Assert.Equal(originalGroup.NoModifierLootScore, roundTripGroup.NoModifierLootScore);
                Assert.Equal(originalGroup.NoModifierProductionScore, roundTripGroup.NoModifierProductionScore);
            }
        }

        [Fact]
        public void ItemModifiersGroups_WithHandCraftedXml_CanDeserialize()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ItemModifierGroups>
    <ItemModifierGroup id=""test_weapon"" no_modifier_loot_score=""75"" no_modifier_production_score=""80"" />
    <ItemModifierGroup id=""test_armor"" no_modifier_loot_score=""60"" no_modifier_production_score=""70"" />
    <ItemModifierGroup id=""test_special"" no_modifier_loot_score=""5"" no_modifier_production_score=""10"" />
</ItemModifierGroups>";

            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            
            // Act
            ItemModifierGroups result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemModifierGroup);
            Assert.Equal(3, result.ItemModifierGroup.Count);
            
            var testWeapon = result.ItemModifierGroup.FirstOrDefault(g => g.Id == "test_weapon");
            Assert.NotNull(testWeapon);
            Assert.Equal("75", testWeapon.NoModifierLootScore);
            Assert.Equal("80", testWeapon.NoModifierProductionScore);
        }

        [Fact]
        public void ItemModifiersGroups_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ItemModifierGroups>
</ItemModifierGroups>";

            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            
            // Act
            ItemModifierGroups result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ItemModifierGroup);
            Assert.Empty(result.ItemModifierGroup);
        }
    }
} 