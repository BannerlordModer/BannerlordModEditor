using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class ScenesXmlTests
    {
        [Fact]
        public void Deserialize_ScenesXml_ShouldSucceed()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "scenes.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ScenesDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Sites);
            Assert.NotNull(result.Sites.SiteList);
            Assert.NotEmpty(result.Sites.SiteList);
            
            // 验证基本属性
            Assert.Equal("scene", result.Type);
            
            // 验证第一个site
            var firstSite = result.Sites.SiteList[0];
            Assert.Equal("scn_world_map", firstSite.Id);
            Assert.Equal("world_map", firstSite.Name);
        }

        [Fact]
        public void Serialize_ScenesDO_ShouldMatchOriginal()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "scenes.xml");
            string? originalXml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (originalXml == null) return;

            var original = XmlTestUtils.Deserialize<ScenesDO>(originalXml);

            // Act
            string serializedXml = XmlTestUtils.Serialize(original);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void Mapper_ScenesDOToDTO_ShouldPreserveData()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "scenes.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            var originalDO = XmlTestUtils.Deserialize<ScenesDO>(xml);

            // Act
            var dto = ScenesMapper.ToDTO(originalDO);
            var convertedDO = ScenesMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(convertedDO);
            
            // 验证基本属性
            Assert.Equal(originalDO.Type, convertedDO.Type);
            
            // 验证Sites结构
            Assert.Equal(originalDO.Sites.SiteList.Count, convertedDO.Sites.SiteList.Count);
            
            // 验证第一个site
            var originalSite = originalDO.Sites.SiteList[0];
            var convertedSite = convertedDO.Sites.SiteList[0];
            
            Assert.Equal(originalSite.Id, convertedSite.Id);
            Assert.Equal(originalSite.Name, convertedSite.Name);
        }

        [Fact]
        public void ScenesDO_ShouldContainSpecificSites()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "scenes.xml");
            string? xml = XmlTestUtils.ReadTestDataOrSkip(xmlPath);
            if (xml == null) return;

            // Act
            var result = XmlTestUtils.Deserialize<ScenesDO>(xml);

            // Assert
            Assert.NotNull(result);
            
            var sites = result.Sites.SiteList;
            
            // 验证特定的site存在
            Assert.Contains(sites, s => s.Id == "scn_world_map");
            Assert.Contains(sites, s => s.Id == "scn_random_scene");
            Assert.Contains(sites, s => s.Id == "scn_conversation_scene");
            Assert.Contains(sites, s => s.Id == "scn_minigame_scene");
            Assert.Contains(sites, s => s.Id == "scn_water");
            
            // 验证特定site的属性
            var worldMap = sites.FirstOrDefault(s => s.Id == "scn_world_map");
            Assert.NotNull(worldMap);
            Assert.Equal("world_map", worldMap.Name);
            
            var randomScene = sites.FirstOrDefault(s => s.Id == "scn_random_scene");
            Assert.NotNull(randomScene);
            Assert.Equal("random_scene", randomScene.Name);
        }
    }
}