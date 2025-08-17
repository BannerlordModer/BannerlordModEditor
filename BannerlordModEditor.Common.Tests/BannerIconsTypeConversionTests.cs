using System;
using System.Collections.Generic;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// BannerIcons数据模型类型转换测试
    /// 测试字符串到int/bool的类型转换逻辑、便捷属性的正确性
    /// </summary>
    public class BannerIconsTypeConversionTests
    {
        #region BannerIconGroup Type Conversion Tests

        [Theory]
        [InlineData("1", 1)]
        [InlineData("42", 42)]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        [InlineData("999", 999)]
        public void BannerIconGroup_IdInt_ValidString_ReturnsCorrectInt(string stringValue, int expectedInt)
        {
            // Arrange
            var group = new BannerIconGroupDO { Id = stringValue };

            // Act
            var result = group.IdInt;

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("1.5")]
        [InlineData("abc")]
        [InlineData("true")]
        public void BannerIconGroup_IdInt_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var group = new BannerIconGroupDO { Id = stringValue };

            // Act
            var result = group.IdInt;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("TRUE", true)]
        [InlineData("FALSE", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void BannerIconGroup_IsPatternBool_ValidString_ReturnsCorrectBool(string stringValue, bool expectedBool)
        {
            // Arrange
            var group = new BannerIconGroupDO { IsPattern = stringValue };

            // Act
            var result = group.IsPatternBool;

            // Assert
            Assert.Equal(expectedBool, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("2")]
        [InlineData("yes")]
        [InlineData("no")]
        public void BannerIconGroup_IsPatternBool_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var group = new BannerIconGroupDO { IsPattern = stringValue };

            // Act
            var result = group.IsPatternBool;

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Background Type Conversion Tests

        [Theory]
        [InlineData("1", 1)]
        [InlineData("100", 100)]
        [InlineData("0", 0)]
        [InlineData("50", 50)]
        public void Background_IdInt_ValidString_ReturnsCorrectInt(string stringValue, int expectedInt)
        {
            // Arrange
            var background = new BackgroundDO { Id = stringValue };

            // Act
            var result = background.IdInt;

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("1.5")]
        public void Background_IdInt_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var background = new BackgroundDO { Id = stringValue };

            // Act
            var result = background.IdInt;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void Background_IsBaseBackgroundBool_ValidString_ReturnsCorrectBool(string stringValue, bool expectedBool)
        {
            // Arrange
            var background = new BackgroundDO { IsBaseBackground = stringValue };

            // Act
            var result = background.IsBaseBackgroundBool;

            // Assert
            Assert.Equal(expectedBool, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("2")]
        public void Background_IsBaseBackgroundBool_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var background = new BackgroundDO { IsBaseBackground = stringValue };

            // Act
            var result = background.IsBaseBackgroundBool;

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Icon Type Conversion Tests

        [Theory]
        [InlineData("1", 1)]
        [InlineData("1000", 1000)]
        [InlineData("0", 0)]
        [InlineData("255", 255)]
        public void Icon_IdInt_ValidString_ReturnsCorrectInt(string stringValue, int expectedInt)
        {
            // Arrange
            var icon = new IconDO { Id = stringValue };

            // Act
            var result = icon.IdInt;

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("1.5")]
        public void Icon_IdInt_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var icon = new IconDO { Id = stringValue };

            // Act
            var result = icon.IdInt;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        [InlineData("10", 10)]
        [InlineData("255", 255)]
        public void Icon_TextureIndexInt_ValidString_ReturnsCorrectInt(string stringValue, int expectedInt)
        {
            // Arrange
            var icon = new IconDO { TextureIndex = stringValue };

            // Act
            var result = icon.TextureIndexInt;

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("1.5")]
        [InlineData("-1")]
        public void Icon_TextureIndexInt_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var icon = new IconDO { TextureIndex = stringValue };

            // Act
            var result = icon.TextureIndexInt;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void Icon_IsReservedBool_ValidString_ReturnsCorrectBool(string stringValue, bool expectedBool)
        {
            // Arrange
            var icon = new IconDO { IsReserved = stringValue };

            // Act
            var result = icon.IsReservedBool;

            // Assert
            Assert.Equal(expectedBool, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("2")]
        public void Icon_IsReservedBool_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var icon = new IconDO { IsReserved = stringValue };

            // Act
            var result = icon.IsReservedBool;

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region ColorEntry Type Conversion Tests

        [Theory]
        [InlineData("1", 1)]
        [InlineData("100", 100)]
        [InlineData("0", 0)]
        [InlineData("50", 50)]
        public void ColorEntry_IdInt_ValidString_ReturnsCorrectInt(string stringValue, int expectedInt)
        {
            // Arrange
            var color = new ColorEntryDO { Id = stringValue };

            // Act
            var result = color.IdInt;

            // Assert
            Assert.Equal(expectedInt, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("1.5")]
        public void ColorEntry_IdInt_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var color = new ColorEntryDO { Id = stringValue };

            // Act
            var result = color.IdInt;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void ColorEntry_PlayerCanChooseForBackgroundBool_ValidString_ReturnsCorrectBool(string stringValue, bool expectedBool)
        {
            // Arrange
            var color = new ColorEntryDO { PlayerCanChooseForBackground = stringValue };

            // Act
            var result = color.PlayerCanChooseForBackgroundBool;

            // Assert
            Assert.Equal(expectedBool, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("2")]
        public void ColorEntry_PlayerCanChooseForBackgroundBool_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var color = new ColorEntryDO { PlayerCanChooseForBackground = stringValue };

            // Act
            var result = color.PlayerCanChooseForBackgroundBool;

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void ColorEntry_PlayerCanChooseForSigilBool_ValidString_ReturnsCorrectBool(string stringValue, bool expectedBool)
        {
            // Arrange
            var color = new ColorEntryDO { PlayerCanChooseForSigil = stringValue };

            // Act
            var result = color.PlayerCanChooseForSigilBool;

            // Assert
            Assert.Equal(expectedBool, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData("2")]
        public void ColorEntry_PlayerCanChooseForSigilBool_InvalidString_ReturnsNull(string stringValue)
        {
            // Arrange
            var color = new ColorEntryDO { PlayerCanChooseForSigil = stringValue };

            // Act
            var result = color.PlayerCanChooseForSigilBool;

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Integration Tests - Combined Type Conversion

        [Fact]
        public void BannerIconGroup_AllTypeConversions_WorkTogether()
        {
            // Arrange
            var group = new BannerIconGroupDO
            {
                Id = "42",
                Name = "Test Group",
                IsPattern = "true"
            };

            // Act & Assert
            Assert.Equal(42, group.IdInt);
            Assert.True(group.IsPatternBool);
            Assert.Equal("Test Group", group.Name);
        }

        [Fact]
        public void Background_AllTypeConversions_WorkTogether()
        {
            // Arrange
            var background = new BackgroundDO
            {
                Id = "25",
                MeshName = "test_mesh",
                IsBaseBackground = "false"
            };

            // Act & Assert
            Assert.Equal(25, background.IdInt);
            Assert.Equal("test_mesh", background.MeshName);
            Assert.False(background.IsBaseBackgroundBool);
        }

        [Fact]
        public void Icon_AllTypeConversions_WorkTogether()
        {
            // Arrange
            var icon = new IconDO
            {
                Id = "150",
                MaterialName = "test_material",
                TextureIndex = "5",
                IsReserved = "true"
            };

            // Act & Assert
            Assert.Equal(150, icon.IdInt);
            Assert.Equal("test_material", icon.MaterialName);
            Assert.Equal(5, icon.TextureIndexInt);
            Assert.True(icon.IsReservedBool);
        }

        [Fact]
        public void ColorEntry_AllTypeConversions_WorkTogether()
        {
            // Arrange
            var color = new ColorEntryDO
            {
                Id = "10",
                Hex = "#FF0000",
                PlayerCanChooseForBackground = "true",
                PlayerCanChooseForSigil = "false"
            };

            // Act & Assert
            Assert.Equal(10, color.IdInt);
            Assert.Equal("#FF0000", color.Hex);
            Assert.True(color.PlayerCanChooseForBackgroundBool);
            Assert.False(color.PlayerCanChooseForSigilBool);
        }

        #endregion

        #region Edge Cases and Boundary Values

        [Fact]
        public void TypeConversion_MaxIntegerValues_HandledCorrectly()
        {
            // Arrange
            var maxValue = int.MaxValue.ToString();
            var minValue = int.MinValue.ToString();

            // Act & Assert
            var icon = new IconDO { Id = maxValue };
            Assert.Equal(int.MaxValue, icon.IdInt);

            icon = new IconDO { Id = minValue };
            Assert.Equal(int.MinValue, icon.IdInt);
        }

        [Fact]
        public void TypeConversion_LargeTextureIndex_HandledCorrectly()
        {
            // Arrange
            var largeIndex = "9999";

            // Act
            var icon = new IconDO { TextureIndex = largeIndex };

            // Assert
            Assert.Equal(9999, icon.TextureIndexInt);
        }

        [Fact]
        public void TypeConversion_WhitespaceString_HandledCorrectly()
        {
            // Arrange
            var whitespace = "   ";

            // Act & Assert
            var group = new BannerIconGroupDO { Id = whitespace };
            Assert.Null(group.IdInt);

            group = new BannerIconGroupDO { IsPattern = whitespace };
            Assert.Null(group.IsPatternBool);
        }

        [Fact]
        public void TypeConversion_MixedCaseBoolean_HandledCorrectly()
        {
            // Arrange
            var mixedCases = new[] { "TrUe", "FaLsE", "tRuE", "fAlSe" };

            // Act & Assert
            foreach (var testCase in mixedCases)
            {
                var group = new BannerIconGroupDO { IsPattern = testCase };
                // Should return null for non-standard boolean formats
                Assert.Null(group.IsPatternBool);
            }
        }

        #endregion
    }
}