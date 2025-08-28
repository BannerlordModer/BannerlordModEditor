using System.IO;
using System.Threading.Tasks;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.DO
{
    /// <summary>
    /// 攻城器械测试
    /// </summary>
    public class SiegeEnginesTests
    {
        [Fact]
        public async Task SiegeEngines_XmlSerialization_ShouldBeRoundTripValid()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = SiegeEnginesMapper.ToDTO(originalDo);
            var roundtripDo = SiegeEnginesMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(roundtripDo);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
        }

        [Fact]
        public async Task SiegeEngines_ShouldHaveCorrectStructure()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var siegeEngines = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            
            // Assert
            Assert.NotNull(siegeEngines);
            Assert.NotEmpty(siegeEngines.SiegeEngines);
            
            // Verify basic siege engines exist
            var ladder = siegeEngines.SiegeEngines.FirstOrDefault(se => se.Id == "ladder");
            Assert.NotNull(ladder);
            Assert.Equal("{=G0AWk1rX}Ladder", ladder.Name);
            
            // Verify siege tower exists
            var siegeTower = siegeEngines.SiegeEngines.FirstOrDefault(se => se.Id == "siege_tower_level1");
            Assert.NotNull(siegeTower);
            Assert.Equal("{=aXjlMBiE}Siege Tower", siegeTower.Name);
            Assert.Equal("5000", siegeTower.MaxHitPoints);
            
            // Verify ranged siege engines
            var rangedEngines = siegeEngines.SiegeEngines.Where(se => se.IsRanged == "true").ToList();
            Assert.NotEmpty(rangedEngines);
            
            // Verify ballista exists
            var ballista = siegeEngines.SiegeEngines.FirstOrDefault(se => se.Id == "ballista");
            Assert.NotNull(ballista);
            Assert.Equal("true", ballista.IsRanged);
            Assert.Equal("160", ballista.Damage);
            Assert.Equal("true", ballista.IsAntiPersonnel);
        }

        [Fact]
        public async Task SiegeEngines_Mapper_ShouldWorkCorrectly()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            var dto = SiegeEnginesMapper.ToDTO(originalDo);
            var convertedDo = SiegeEnginesMapper.ToDO(dto);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(convertedDo);
            
            Assert.Equal(originalDo.SiegeEngines.Count, dto.SiegeEngines.Count);
            Assert.Equal(originalDo.SiegeEngines.Count, convertedDo.SiegeEngines.Count);
            
            // Compare first siege engine
            if (originalDo.SiegeEngines.Count > 0)
            {
                var originalSiegeEngine = originalDo.SiegeEngines[0];
                var convertedSiegeEngine = convertedDo.SiegeEngines[0];
                
                Assert.Equal(originalSiegeEngine.Id, convertedSiegeEngine.Id);
                Assert.Equal(originalSiegeEngine.Name, convertedSiegeEngine.Name);
                Assert.Equal(originalSiegeEngine.Description, convertedSiegeEngine.Description);
            }
        }

        [Fact]
        public async Task SiegeEngines_ShouldContainAllExpectedTypes()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "siegeengines.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var siegeEngines = XmlTestUtils.Deserialize<SiegeEnginesDO>(xmlContent);
            
            // Assert
            Assert.NotNull(siegeEngines);
            
            // Expected siege engine IDs
            var expectedIds = new[]
            {
                "preparations", "ladder", "siege_tower_level1", "siege_tower_level2",
                "ballista", "fire_ballista", "catapult", "fire_catapult",
                "onager", "fire_onager", "bricole", "trebuchet", "fire_trebuchet",
                "ram", "improved_ram"
            };
            
            foreach (var expectedId in expectedIds)
            {
                var siegeEngine = siegeEngines.SiegeEngines.FirstOrDefault(se => se.Id == expectedId);
                Assert.NotNull(siegeEngine);
            }
        }
    }
}