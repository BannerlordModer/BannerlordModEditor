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
    /// 水体预制体测试
    /// </summary>
    public class WaterPrefabsTests
    {
        [Fact]
        public async Task WaterPrefabs_XmlSerialization_ShouldBeRoundTripValid()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = WaterPrefabsMapper.ToDTO(originalDo);
            var roundtripDo = WaterPrefabsMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(roundtripDo);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
        }

        [Fact]
        public async Task WaterPrefabs_ShouldHaveCorrectStructure()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var waterPrefabs = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(waterPrefabs);
            Assert.NotEmpty(waterPrefabs.WaterPrefabs);
            
            // Verify first water prefab
            var firstPrefab = waterPrefabs.WaterPrefabs.FirstOrDefault();
            Assert.NotNull(firstPrefab);
            Assert.Equal("Open Ocean Global", firstPrefab.PrefabName);
            Assert.Equal("wat_open_ocean_g", firstPrefab.MaterialName);
            Assert.Equal("Open Ocean", firstPrefab.Thumbnail);
            
            // 验证IsGlobal属性值（允许大小写不敏感）
            Assert.NotNull(firstPrefab.IsGlobal);
            Assert.True(XmlTestUtils.CommonBooleanTrueValues.Contains(firstPrefab.IsGlobal, StringComparer.OrdinalIgnoreCase), 
                $"Expected IsGlobal to be a boolean true value, but was '{firstPrefab.IsGlobal}'");
        }

        [Fact]
        public async Task WaterPrefabs_Mapper_ShouldWorkCorrectly()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            var dto = WaterPrefabsMapper.ToDTO(originalDo);
            var convertedDo = WaterPrefabsMapper.ToDO(dto);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(convertedDo);
            
            Assert.Equal(originalDo.WaterPrefabs.Count, dto.WaterPrefabs.Count);
            Assert.Equal(originalDo.WaterPrefabs.Count, convertedDo.WaterPrefabs.Count);
            
            // Compare first water prefab
            if (originalDo.WaterPrefabs.Count > 0)
            {
                var originalPrefab = originalDo.WaterPrefabs[0];
                var convertedPrefab = convertedDo.WaterPrefabs[0];
                
                Assert.Equal(originalPrefab.PrefabName, convertedPrefab.PrefabName);
                Assert.Equal(originalPrefab.MaterialName, convertedPrefab.MaterialName);
                Assert.Equal(originalPrefab.Thumbnail, convertedPrefab.Thumbnail);
                Assert.Equal(originalPrefab.IsGlobal, convertedPrefab.IsGlobal);
            }
        }

        [Fact]
        public async Task WaterPrefabs_ShouldContainGlobalAndNonGlobalPrefabs()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var waterPrefabs = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(waterPrefabs);
            
            // Verify global prefabs exist (使用大小写不敏感的比较)
            var globalPrefabs = waterPrefabs.WaterPrefabs.Where(wp => 
                XmlTestUtils.CommonBooleanTrueValues.Contains(wp.IsGlobal, StringComparer.OrdinalIgnoreCase)).ToList();
            Assert.NotEmpty(globalPrefabs);
            
            // Verify non-global prefabs exist (使用大小写不敏感的比较)
            var nonGlobalPrefabs = waterPrefabs.WaterPrefabs.Where(wp => 
                XmlTestUtils.CommonBooleanFalseValues.Contains(wp.IsGlobal, StringComparer.OrdinalIgnoreCase)).ToList();
            Assert.NotEmpty(nonGlobalPrefabs);
            
            // Verify specific global prefabs
            var oceanGlobal = globalPrefabs.FirstOrDefault(wp => wp.PrefabName == "Open Ocean Global");
            Assert.NotNull(oceanGlobal);
            Assert.Equal("wat_open_ocean_g", oceanGlobal.MaterialName);
        }

        [Fact]
        public async Task WaterPrefabs_ShouldHaveConsistentNaming()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var waterPrefabs = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(waterPrefabs);
            
            // All prefabs should have required properties
            foreach (var prefab in waterPrefabs.WaterPrefabs)
            {
                Assert.NotNull(prefab.PrefabName);
                Assert.NotNull(prefab.MaterialName);
                Assert.NotNull(prefab.Thumbnail);
                Assert.NotNull(prefab.IsGlobal);
                
                // Material name should start with "wat_"
                Assert.StartsWith("wat_", prefab.MaterialName);
                
                // IsGlobal should be a valid boolean value (大小写不敏感)
                Assert.True(XmlTestUtils.CommonBooleanTrueValues.Contains(prefab.IsGlobal, StringComparer.OrdinalIgnoreCase) ||
                           XmlTestUtils.CommonBooleanFalseValues.Contains(prefab.IsGlobal, StringComparer.OrdinalIgnoreCase), 
                    $"IsGlobal should be a boolean value, but was '{prefab.IsGlobal}'");
            }
        }

        [Fact]
        public async Task WaterPrefabs_ShouldContainExpectedTypes()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "water_prefabs.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var waterPrefabs = XmlTestUtils.Deserialize<WaterPrefabsDO>(xmlContent);
            
            // Assert
            Assert.NotNull(waterPrefabs);
            
            // Verify different water types exist
            var oceanPrefabs = waterPrefabs.WaterPrefabs.Where(wp => wp.PrefabName.Contains("Ocean")).ToList();
            var lakePrefabs = waterPrefabs.WaterPrefabs.Where(wp => wp.PrefabName.Contains("Lake")).ToList();
            var riverPrefabs = waterPrefabs.WaterPrefabs.Where(wp => wp.PrefabName.Contains("River")).ToList();
            var canalPrefabs = waterPrefabs.WaterPrefabs.Where(wp => wp.PrefabName.Contains("Canal")).ToList();
            
            Assert.NotEmpty(oceanPrefabs);
            Assert.NotEmpty(lakePrefabs);
            Assert.NotEmpty(riverPrefabs);
            Assert.NotEmpty(canalPrefabs);
        }
    }
}