using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.Multiplayer;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.Multiplayer
{
    public class MultiplayerScenesTests
    {
        [Fact]
        public async Task MultiplayerScenes_XmlSerialization_ShouldBeRoundTripValid()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "MultiplayerScenes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<MultiplayerScenesDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = MultiplayerScenesMapper.ToDTO(originalDo);
            var roundtripDo = MultiplayerScenesMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlPath);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(roundtripDo);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
        }

        [Fact]
        public async Task MultiplayerScenes_ShouldHaveCorrectStructure()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "MultiplayerScenes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var scenes = XmlTestUtils.Deserialize<MultiplayerScenesDO>(xmlContent);
            
            // Assert
            Assert.NotNull(scenes);
            Assert.NotEmpty(scenes.Scenes);
            
            // Verify test maps
            var testMaps = scenes.Scenes.Where(s => s.Name.Contains("test")).ToList();
            Assert.NotEmpty(testMaps);
            
            // Verify captain maps
            var captainMaps = scenes.Scenes.Where(s => 
                s.GameTypes.Any(gt => gt.Name == "Captain")).ToList();
            Assert.NotEmpty(captainMaps);
            
            // Verify battle/skirmish maps
            var battleMaps = scenes.Scenes.Where(s => 
                s.GameTypes.Any(gt => gt.Name == "Battle" || gt.Name == "Skirmish")).ToList();
            Assert.NotEmpty(battleMaps);
            
            // Verify siege maps
            var siegeMaps = scenes.Scenes.Where(s => 
                s.GameTypes.Any(gt => gt.Name == "Siege")).ToList();
            Assert.NotEmpty(siegeMaps);
        }

        [Fact]
        public async Task MultiplayerScenes_Mapper_ShouldWorkCorrectly()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "MultiplayerScenes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<MultiplayerScenesDO>(xmlContent);
            var dto = MultiplayerScenesMapper.ToDTO(originalDo);
            var convertedDo = MultiplayerScenesMapper.ToDO(dto);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(convertedDo);
            
            Assert.Equal(originalDo.Scenes.Count, dto.Scenes.Count);
            Assert.Equal(originalDo.Scenes.Count, convertedDo.Scenes.Count);
            
            // Compare first scene
            if (originalDo.Scenes.Count > 0)
            {
                var originalScene = originalDo.Scenes[0];
                var convertedScene = convertedDo.Scenes[0];
                
                Assert.Equal(originalScene.Name, convertedScene.Name);
                Assert.Equal(originalScene.GameTypes.Count, convertedScene.GameTypes.Count);
            }
        }
    }
}