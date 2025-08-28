using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.Multiplayer;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.Multiplayer
{
    public class TauntUsageSetsTests
    {
        [Fact]
        public async Task TauntUsageSets_XmlSerialization_ShouldBeRoundTripValid()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "taunt_usage_sets.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = TauntUsageSetsMapper.ToDTO(originalDo);
            var roundtripDo = TauntUsageSetsMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // Assert - XML序列化器不支持注释，所以我们需要忽略注释进行比较
            var options = new XmlTestUtils.XmlComparisonOptions 
            { 
                IgnoreComments = true,  // XML序列化器不支持注释，这是已知限制
                IgnoreWhitespace = true,
                IgnoreAttributeOrder = true
            };
            Assert.NotNull(originalDo);
            Assert.NotNull(roundtripDo);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml, options));
        }

        [Fact]
        public async Task TauntUsageSets_ShouldHaveCorrectStructure()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "taunt_usage_sets.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var tauntSets = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(tauntSets);
            Assert.NotEmpty(tauntSets.TauntUsageSetList);
            
            // Verify we have taunt sets
            Assert.Equal(36, tauntSets.TauntUsageSetList.Count); // Based on the XML file
            
            // Verify first taunt set has correct structure
            var firstSet = tauntSets.TauntUsageSetList[0];
            Assert.Equal("taunt_01", firstSet.Id);
            Assert.NotEmpty(firstSet.TauntUsages);
            
            // Verify taunt usage properties
            var firstUsage = firstSet.TauntUsages[0];
            Assert.Equal("act_taunt_01_bow", firstUsage.Action);
            Assert.Equal("True", firstUsage.RequiresBow); // 原始XML中的值是大写的True
            Assert.Equal("True", firstUsage.RequiresOnFoot); // 原始XML中的值是大写的True
        }

        [Fact]
        public async Task TauntUsageSets_Mapper_ShouldWorkCorrectly()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "taunt_usage_sets.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xmlContent);
            var dto = TauntUsageSetsMapper.ToDTO(originalDo);
            var convertedDo = TauntUsageSetsMapper.ToDO(dto);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(convertedDo);
            
            Assert.Equal(originalDo.TauntUsageSetList.Count, dto.TauntUsageSetList.Count);
            Assert.Equal(originalDo.TauntUsageSetList.Count, convertedDo.TauntUsageSetList.Count);
            
            // Compare first taunt set
            if (originalDo.TauntUsageSetList.Count > 0)
            {
                var originalSet = originalDo.TauntUsageSetList[0];
                var convertedSet = convertedDo.TauntUsageSetList[0];
                
                Assert.Equal(originalSet.Id, convertedSet.Id);
                Assert.Equal(originalSet.TauntUsages.Count, convertedSet.TauntUsages.Count);
            }
        }

        [Fact]
        public async Task TauntUsageSets_ShouldHaveVariousActionTypes()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "taunt_usage_sets.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var tauntSets = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(tauntSets);
            
            // Check for different action types
            var allActions = tauntSets.TauntUsageSetList
                .SelectMany(set => set.TauntUsages)
                .Select(usage => usage.Action)
                .Distinct()
                .ToList();
            
            Assert.Contains("act_taunt_01", allActions);
            Assert.Contains("act_taunt_cheer_1", allActions);
            Assert.Contains("act_taunt_01_bow", allActions);
            Assert.Contains("act_taunt_01_leftstance", allActions);
        }

        [Fact]
        public async Task TauntUsageSets_ShouldHaveVariousRequirements()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "taunt_usage_sets.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var tauntSets = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(tauntSets);
            
            // Check for different requirement combinations
            var allUsages = tauntSets.TauntUsageSetList
                .SelectMany(set => set.TauntUsages)
                .ToList();
            
            // Should have bow requirements
            var bowRequired = allUsages.Any(u => u.RequiresBow == "True");
            Assert.True(bowRequired);
            
            // Should have shield requirements
            var shieldRequired = allUsages.Any(u => u.RequiresShield == "True");
            Assert.True(shieldRequired);
            
            // Should have left stance requirements
            var leftStanceRequired = allUsages.Any(u => u.IsLeftStance == "True");
            Assert.True(leftStanceRequired);
            
            // Should have on foot requirements
            var onFootRequired = allUsages.Any(u => u.RequiresOnFoot == "True");
            Assert.True(onFootRequired);
        }
    }
}