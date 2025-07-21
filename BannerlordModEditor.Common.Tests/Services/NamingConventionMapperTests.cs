using BannerlordModEditor.Common.Services;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class NamingConventionMapperTests
    {
        [Theory]
        [InlineData("action", "ActionType")]
        [InlineData("object", "GameObjectType")]
        [InlineData("mp_crafting_pieces", "MpCraftingPieces")]
        [InlineData("before_transparents_graph", "BeforeTransparentsGraph")]
        [InlineData("particle_systems2", "ParticleSystems2")]
        [InlineData("looknfeel", "LookAndFeel")]
        [InlineData("simple_name", "SimpleName")]
        public void GetMappedClassName_WithSpecialCases_ShouldReturnCorrectMapping(string input, string expected)
        {
            // Act
            var result = NamingConventionMapper.GetMappedClassName(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("class", "ClassType")]
        [InlineData("string", "StringType")]
        [InlineData("int", "IntType")]
        [InlineData("void", "VoidType")]
        public void GetMappedClassName_WithReservedKeywords_ShouldAppendType(string input, string expected)
        {
            // Act
            var result = NamingConventionMapper.GetMappedClassName(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("normal_name", "NormalName")]
        [InlineData("another_test_case", "AnotherTestCase")]
        [InlineData("single", "Single")]
        public void GetMappedClassName_WithNormalNames_ShouldConvertToPascalCase(string input, string expected)
        {
            // Act
            var result = NamingConventionMapper.GetMappedClassName(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("action")]
        [InlineData("class")]
        [InlineData("mp_crafting_pieces")]
        [InlineData("before_transparents_graph")]
        public void RequiresSpecialHandling_WithSpecialCases_ShouldReturnTrue(string input)
        {
            // Act
            var result = NamingConventionMapper.RequiresSpecialHandling(input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("normal_name")]
        [InlineData("simple_case")]
        [InlineData("regular_file")]
        public void RequiresSpecialHandling_WithNormalNames_ShouldReturnFalse(string input)
        {
            // Act
            var result = NamingConventionMapper.RequiresSpecialHandling(input);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("particle_systems", "Engine")]
        [InlineData("decal_textures", "Engine")]
        [InlineData("mp_items", "Multiplayer")]
        [InlineData("credits_legal", "Configuration")]
        [InlineData("flora_kinds", "Map")]
        [InlineData("animation_data", "Engine")]
        [InlineData("regular_data", "Data")]
        public void GetSuggestedNamespace_ShouldReturnCorrectNamespace(string fileName, string expected)
        {
            // Act
            var result = NamingConventionMapper.GetSuggestedNamespace(fileName);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetMappedClassName_WithEmptyString_ShouldReturnEmpty()
        {
            // Act
            var result = NamingConventionMapper.GetMappedClassName("");

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void GetMappedClassName_WithNull_ShouldReturnEmpty()
        {
            // Act
            var result = NamingConventionMapper.GetMappedClassName(null!);

            // Assert
            Assert.Equal("", result);
        }
    }
}