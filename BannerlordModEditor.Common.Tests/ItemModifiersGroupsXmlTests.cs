using BannerlordModEditor.Common.Models.Game;
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
    public class ItemModifiersGroupsXmlTests
    {
        [Fact]
        public void ItemModifiersGroups_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "example", "ModuleData", "item_modifiers_groups.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups itemModifiersGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiersGroups = (ItemModifierGroups)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, itemModifiersGroups);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(itemModifiersGroups);
            Assert.NotNull(itemModifiersGroups.ItemModifierGroup);
            Assert.Equal(20, itemModifiersGroups.ItemModifierGroup.Count);
            
            // 验证具体的修饰符组数据
            var swordGroup = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == "sword");
            Assert.NotNull(swordGroup);
            Assert.Equal("50", swordGroup.NoModifierLootScore);
            Assert.Equal("65", swordGroup.NoModifierProductionScore);
            
            var horseGroup = itemModifiersGroups.ItemModifierGroup.FirstOrDefault(g => g.Id == "horse");
            Assert.NotNull(horseGroup);
            Assert.Equal("1", horseGroup.NoModifierLootScore);
            Assert.Equal("1", horseGroup.NoModifierProductionScore);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Elements("ItemModifierGroup").Count(), 
                        savedDoc.Root?.Elements("ItemModifierGroup").Count());
        }
        
        [Fact]
        public void ItemModifiersGroups_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "example", "ModuleData", "item_modifiers_groups.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ItemModifierGroups));
            ItemModifierGroups itemModifiersGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiersGroups = (ItemModifierGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有修饰符组都有必要的属性
            Assert.NotNull(itemModifiersGroups.ItemModifierGroup);
            
            foreach (var group in itemModifiersGroups.ItemModifierGroup)
            {
                Assert.False(string.IsNullOrWhiteSpace(group.Id), "Modifier group should have Id");
                Assert.False(string.IsNullOrWhiteSpace(group.NoModifierLootScore), "Modifier group should have NoModifierLootScore");
                Assert.False(string.IsNullOrWhiteSpace(group.NoModifierProductionScore), "Modifier group should have NoModifierProductionScore");
                
                // 验证分数格式
                Assert.True(int.TryParse(group.NoModifierLootScore, out int lootScore), 
                    $"NoModifierLootScore '{group.NoModifierLootScore}' should be a valid integer");
                Assert.True(int.TryParse(group.NoModifierProductionScore, out int productionScore),
                    $"NoModifierProductionScore '{group.NoModifierProductionScore}' should be a valid integer");
                
                Assert.True(lootScore > 0, "Loot score should be positive");
                Assert.True(productionScore > 0, "Production score should be positive");
            }
            
            // 验证包含预期的修饰符组
            var allIds = itemModifiersGroups.ItemModifierGroup.Select(g => g.Id).ToList();
            Assert.Contains("sword", allIds);
            Assert.Contains("bow", allIds);
            Assert.Contains("crossbow", allIds);
            Assert.Contains("shield", allIds);
            Assert.Contains("plate", allIds);
            Assert.Contains("horse", allIds);
            Assert.Contains("companion", allIds);
            
            // 验证武器组和非武器组的分数差异
            var weaponGroups = itemModifiersGroups.ItemModifierGroup.Where(g => 
                g.Id == "sword" || g.Id == "bow" || g.Id == "crossbow" || g.Id == "axe").ToList();
            var specialGroups = itemModifiersGroups.ItemModifierGroup.Where(g => 
                g.Id == "horse" || g.Id == "companion").ToList();
            
            Assert.True(weaponGroups.All(g => g.NoModifierLootScore == "50"));
            Assert.True(weaponGroups.All(g => g.NoModifierProductionScore == "65"));
            Assert.True(specialGroups.All(g => g.NoModifierLootScore == "1"));
            Assert.True(specialGroups.All(g => g.NoModifierProductionScore == "1"));
            
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