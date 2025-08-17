using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class BannerIconsStandaloneTest
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

        private string TestDataPath => Path.Combine(FindSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData");

        [Fact]
        public void BannerIcons_XmlDeserializationWorks()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsDO));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsDO)serializer.Deserialize(fileStream);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BannerIconData);
            Assert.NotNull(result.BannerIconData.BannerIconGroups);
            Assert.True(result.BannerIconData.BannerIconGroups.Count >= 6);
            
            if (result.BannerIconData.BannerColors != null)
            {
                Assert.True(result.BannerIconData.BannerColors.Colors.Count > 100);
            }
        }

        [Fact]
        public void BannerIcons_GroupStructureIsCorrect()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsDO));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsDO)serializer.Deserialize(fileStream);

            // Assert
            foreach (var group in result.BannerIconData.BannerIconGroups)
            {
                Assert.False(string.IsNullOrWhiteSpace(group.Id));
                Assert.False(string.IsNullOrWhiteSpace(group.Name));
                
                if (group.IsPattern == "true")
                {
                    Assert.True(group.Backgrounds.Count > 0);
                }
                else
                {
                    Assert.True(group.Icons.Count > 0);
                }
            }
        }

        [Fact]
        public void BannerIcons_BackgroundGroupExists()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsDO));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsDO)serializer.Deserialize(fileStream);

            // Assert
            var backgroundGroup = result.BannerIconData.BannerIconGroups.FirstOrDefault(g => g.Id == "1");
            Assert.NotNull(backgroundGroup);
            Assert.True(backgroundGroup.IsPattern == "true");
            Assert.True(backgroundGroup.Backgrounds.Count >= 30);
            
            var baseBackground = backgroundGroup.Backgrounds.FirstOrDefault(b => b.IsBaseBackground == "true");
            Assert.NotNull(baseBackground);
            
            foreach (var background in backgroundGroup.Backgrounds)
            {
                Assert.False(string.IsNullOrWhiteSpace(background.Id));
                Assert.NotEmpty(background.MeshName);
            }
        }

        [Fact]
        public void BannerIcons_AnimalGroupExists()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsDO));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsDO)serializer.Deserialize(fileStream);

            // Assert
            var animalGroup = result.BannerIconData.BannerIconGroups.FirstOrDefault(g => g.Id == "2");
            Assert.NotNull(animalGroup);
            Assert.True(animalGroup.IsPattern == "false");
            Assert.True(animalGroup.Icons.Count >= 10);
            
            foreach (var icon in animalGroup.Icons)
            {
                Assert.False(string.IsNullOrWhiteSpace(icon.Id));
                Assert.NotEmpty(icon.MaterialName);
            }
        }

        [Fact]
        public void BannerIcons_ColorsHaveValidHexValues()
        {
            // Arrange
            var filePath = Path.Combine(TestDataPath, "banner_icons.xml");
            var serializer = new XmlSerializer(typeof(BannerIconsDO));

            // Act
            using var fileStream = new FileStream(filePath, FileMode.Open);
            var result = (BannerIconsDO)serializer.Deserialize(fileStream);

            // Assert
            if (result.BannerIconData.BannerColors != null)
            {
                foreach (var color in result.BannerIconData.BannerColors.Colors)
                {
                    Assert.False(string.IsNullOrWhiteSpace(color.Id));
                    Assert.NotEmpty(color.Hex);
                    Assert.StartsWith("0x", color.Hex);
                    Assert.True(color.Hex.Length == 10);
                }
            }
        }
    }
}