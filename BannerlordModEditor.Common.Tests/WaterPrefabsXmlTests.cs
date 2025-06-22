using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class WaterPrefabsXmlTests
    {
        [Fact]
        public void WaterPrefabs_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WaterPrefabs>
	<WaterPrefab
		PrefabName=""Open Ocean Global""
		MaterialName=""wat_open_ocean_g""
		Thumbnail=""Open Ocean""
		IsGlobal=""true"" />
	<WaterPrefab
		PrefabName=""Muddy Puddle""
		MaterialName=""wat_puddle_muddy""
		Thumbnail=""Muddy Puddle""
		IsGlobal=""False"" />
	<WaterPrefab
		PrefabName=""Sea Side""
		MaterialName=""wat_sea_side_g""
		Thumbnail=""Sea Side""
		IsGlobal=""true"" />
	<WaterPrefab
		PrefabName=""Canal Green""
		MaterialName=""wat_canal_green""
		Thumbnail=""Canal Green""
		IsGlobal=""false"" />
	<WaterPrefab
		PrefabName=""River Blue""
		MaterialName=""wat_river_blue""
		Thumbnail=""River Blue""
		IsGlobal=""false"" />
</WaterPrefabs>";

            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act
            WaterPrefabs? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (WaterPrefabs?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.WaterPrefab);
            Assert.Equal(5, result.WaterPrefab.Length);

            // Test Open Ocean Global
            var openOcean = result.WaterPrefab[0];
            Assert.Equal("Open Ocean Global", openOcean.PrefabName);
            Assert.Equal("wat_open_ocean_g", openOcean.MaterialName);
            Assert.Equal("Open Ocean", openOcean.Thumbnail);
            Assert.Equal("true", openOcean.IsGlobal);

            // Test Muddy Puddle
            var muddyPuddle = result.WaterPrefab[1];
            Assert.Equal("Muddy Puddle", muddyPuddle.PrefabName);
            Assert.Equal("wat_puddle_muddy", muddyPuddle.MaterialName);
            Assert.Equal("Muddy Puddle", muddyPuddle.Thumbnail);
            Assert.Equal("False", muddyPuddle.IsGlobal);

            // Test Sea Side
            var seaSide = result.WaterPrefab[2];
            Assert.Equal("Sea Side", seaSide.PrefabName);
            Assert.Equal("wat_sea_side_g", seaSide.MaterialName);
            Assert.Equal("Sea Side", seaSide.Thumbnail);
            Assert.Equal("true", seaSide.IsGlobal);

            // Test Canal Green
            var canalGreen = result.WaterPrefab[3];
            Assert.Equal("Canal Green", canalGreen.PrefabName);
            Assert.Equal("wat_canal_green", canalGreen.MaterialName);
            Assert.Equal("Canal Green", canalGreen.Thumbnail);
            Assert.Equal("false", canalGreen.IsGlobal);

            // Test River Blue
            var riverBlue = result.WaterPrefab[4];
            Assert.Equal("River Blue", riverBlue.PrefabName);
            Assert.Equal("wat_river_blue", riverBlue.MaterialName);
            Assert.Equal("River Blue", riverBlue.Thumbnail);
            Assert.Equal("false", riverBlue.IsGlobal);
        }

        [Fact]
        public void WaterPrefabs_WithSpecialCases_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WaterPrefabs>
	<WaterPrefab
		PrefabName=""Clean Water Calm""
		MaterialName=""wat_clean_water_calm""
		Thumbnail=""Clean Water Calm""
		IsGlobal=""false"" />
	<WaterPrefab
		PrefabName=""Fountain Water""
		MaterialName=""wat_fountain_water""
		Thumbnail=""Fountain Water""
		IsGlobal=""false"" />
	<WaterPrefab
		PrefabName=""Main Map Ocean Global""
		MaterialName=""wat_main_map_ocean_g""
		Thumbnail=""Main Map Ocean""
		IsGlobal=""true"" />
</WaterPrefabs>";

            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act
            WaterPrefabs? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (WaterPrefabs?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.WaterPrefab);
            Assert.Equal(3, result.WaterPrefab.Length);

            var cleanWaterCalm = result.WaterPrefab[0];
            Assert.Equal("Clean Water Calm", cleanWaterCalm.PrefabName);
            Assert.Equal("wat_clean_water_calm", cleanWaterCalm.MaterialName);
            Assert.Equal("Clean Water Calm", cleanWaterCalm.Thumbnail);
            Assert.Equal("false", cleanWaterCalm.IsGlobal);

            var fountainWater = result.WaterPrefab[1];
            Assert.Equal("Fountain Water", fountainWater.PrefabName);
            Assert.Equal("wat_fountain_water", fountainWater.MaterialName);
            Assert.Equal("Fountain Water", fountainWater.Thumbnail);
            Assert.Equal("false", fountainWater.IsGlobal);

            var mainMapOcean = result.WaterPrefab[2];
            Assert.Equal("Main Map Ocean Global", mainMapOcean.PrefabName);
            Assert.Equal("wat_main_map_ocean_g", mainMapOcean.MaterialName);
            Assert.Equal("Main Map Ocean", mainMapOcean.Thumbnail);
            Assert.Equal("true", mainMapOcean.IsGlobal);
        }

        [Fact]
        public void WaterPrefabs_CanSerializeToXml()
        {
            // Arrange
            var waterPrefabs = new WaterPrefabs
            {
                WaterPrefab = new[]
                {
                    new WaterPrefab
                    {
                        PrefabName = "Test Ocean",
                        MaterialName = "wat_test_ocean",
                        Thumbnail = "Test Ocean Thumbnail",
                        IsGlobal = "true"
                    },
                    new WaterPrefab
                    {
                        PrefabName = "Test Lake",
                        MaterialName = "wat_test_lake",
                        Thumbnail = "Test Lake Thumbnail",
                        IsGlobal = "false"
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, waterPrefabs);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("PrefabName=\"Test Ocean\"", result);
            Assert.Contains("MaterialName=\"wat_test_ocean\"", result);
            Assert.Contains("Thumbnail=\"Test Ocean Thumbnail\"", result);
            Assert.Contains("IsGlobal=\"true\"", result);
            Assert.Contains("PrefabName=\"Test Lake\"", result);
            Assert.Contains("MaterialName=\"wat_test_lake\"", result);
            Assert.Contains("Thumbnail=\"Test Lake Thumbnail\"", result);
            Assert.Contains("IsGlobal=\"false\"", result);
        }

        [Fact]
        public void WaterPrefabs_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new WaterPrefabs
            {
                WaterPrefab = new[]
                {
                    new WaterPrefab
                    {
                        PrefabName = "Roundtrip Test Water",
                        MaterialName = "wat_roundtrip_test",
                        Thumbnail = "Roundtrip Test",
                        IsGlobal = "true"
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            WaterPrefabs? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (WaterPrefabs?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.WaterPrefab);
            Assert.Single(result.WaterPrefab);

            var waterPrefab = result.WaterPrefab[0];
            Assert.Equal("Roundtrip Test Water", waterPrefab.PrefabName);
            Assert.Equal("wat_roundtrip_test", waterPrefab.MaterialName);
            Assert.Equal("Roundtrip Test", waterPrefab.Thumbnail);
            Assert.Equal("true", waterPrefab.IsGlobal);
        }

        [Fact]
        public void WaterPrefabs_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WaterPrefabs>
</WaterPrefabs>";

            var serializer = new XmlSerializer(typeof(WaterPrefabs));

            // Act
            WaterPrefabs? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (WaterPrefabs?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
        }
    }
} 
