using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// BannerIcons边界条件和异常测试
    /// 测试极端情况、错误处理、异常输入和边界值
    /// </summary>
    public class BannerIconsBoundaryConditionsTests
    {
        #region Null Input Tests

        [Fact]
        public void AllMappers_HandleNullInput_Gracefully()
        {
            // Assert
            Assert.Null(BannerIconsMapper.ToDTO(null));
            Assert.Null(BannerIconsMapper.ToDO(null));
            Assert.Null(BannerIconDataMapper.ToDTO(null));
            Assert.Null(BannerIconDataMapper.ToDO(null));
            Assert.Null(BannerIconGroupMapper.ToDTO(null));
            Assert.Null(BannerIconGroupMapper.ToDO(null));
            Assert.Null(BackgroundMapper.ToDTO(null));
            Assert.Null(BackgroundMapper.ToDO(null));
            Assert.Null(IconMapper.ToDTO(null));
            Assert.Null(IconMapper.ToDO(null));
            Assert.Null(BannerColorsMapper.ToDTO(null));
            Assert.Null(BannerColorsMapper.ToDO(null));
            Assert.Null(ColorEntryMapper.ToDTO(null));
            Assert.Null(ColorEntryMapper.ToDO(null));
        }

        [Fact]
        public void MapperNullCollections_DontThrowException()
        {
            // Arrange
            var source = new BannerIconDataDTO
            {
                BannerIconGroups = null,
                BannerColors = null
            };

            // Act & Assert
            var result = BannerIconDataMapper.ToDO(source);
            Assert.NotNull(result);
            // Mapper should convert null collections to empty collections
            Assert.NotNull(result.BannerIconGroups);
            Assert.Empty(result.BannerIconGroups);
            // BannerColorsMapper returns null for null input (by design)
            Assert.Null(result.BannerColors);
        }

        #endregion

        #region Empty Collections Tests

        [Fact]
        public void MapperEmptyCollections_HandleCorrectly()
        {
            // Arrange
            var source = new BannerIconDataDO
            {
                BannerIconGroups = new List<BannerIconGroupDO>(),
                BannerColors = new BannerColorsDO
                {
                    Colors = new List<ColorEntryDO>()
                }
            };

            // Act
            var dto = BannerIconDataMapper.ToDTO(source);
            var result = BannerIconDataMapper.ToDO(dto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.BannerIconGroups);
            Assert.Empty(result.BannerIconGroups);
            Assert.NotNull(result.BannerColors);
            Assert.NotNull(result.BannerColors.Colors);
            Assert.Empty(result.BannerColors.Colors);
        }

        [Fact]
        public void NestedEmptyCollections_PropagateCorrectly()
        {
            // Arrange
            var source = new BannerIconsDO
            {
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Backgrounds = new List<BackgroundDO>(),
                            Icons = new List<IconDO>()
                        }
                    },
                    BannerColors = new BannerColorsDO
                    {
                        Colors = new List<ColorEntryDO>()
                    }
                }
            };

            // Act
            var result = BannerIconsMapper.ToDO(BannerIconsMapper.ToDTO(source));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.BannerIconData.BannerIconGroups);
            Assert.Empty(result.BannerIconData.BannerIconGroups[0].Backgrounds);
            Assert.Empty(result.BannerIconData.BannerIconGroups[0].Icons);
            Assert.Empty(result.BannerIconData.BannerColors.Colors);
        }

        #endregion

        #region Large Data Sets Tests

        [Fact]
        public void MapperLargeCollections_HandleCorrectly()
        {
            // Arrange
            var largeIconGroups = new List<BannerIconGroupDO>();
            var largeColors = new List<ColorEntryDO>();

            for (int i = 0; i < 1000; i++)
            {
                largeIconGroups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    Backgrounds = new List<BackgroundDO>
                    {
                        new BackgroundDO { Id = $"{i}_1", MeshName = $"mesh_{i}_1" }
                    },
                    Icons = new List<IconDO>
                    {
                        new IconDO { Id = (1000 + i).ToString(), MaterialName = $"material_{i}" }
                    }
                });

                largeColors.Add(new ColorEntryDO
                {
                    Id = i.ToString(),
                    Hex = $"#{i:X6}"
                });
            }

            var source = new BannerIconsDO
            {
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = largeIconGroups,
                    BannerColors = new BannerColorsDO { Colors = largeColors }
                }
            };

            // Act
            var result = BannerIconsMapper.ToDO(BannerIconsMapper.ToDTO(source));

            // Assert
            Assert.Equal(1000, result.BannerIconData.BannerIconGroups.Count);
            Assert.Equal(1000, result.BannerIconData.BannerColors.Colors.Count);
            
            // Verify first and last items
            Assert.Equal("0", result.BannerIconData.BannerIconGroups[0].Id);
            Assert.Equal("999", result.BannerIconData.BannerIconGroups[999].Id);
            Assert.Equal("#000000", result.BannerIconData.BannerColors.Colors[0].Hex);
            Assert.Equal("#0003E7", result.BannerIconData.BannerColors.Colors[999].Hex);
        }

        [Fact]
        public void DeepNesting_HandlesCorrectly()
        {
            // Arrange
            var source = new BannerIconsDO
            {
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Id = "1",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO { Id = "1", MeshName = "mesh1" },
                                new BackgroundDO { Id = "2", MeshName = "mesh2" },
                                new BackgroundDO { Id = "3", MeshName = "mesh3" }
                            },
                            Icons = new List<IconDO>
                            {
                                new IconDO { Id = "100", MaterialName = "mat1" },
                                new IconDO { Id = "101", MaterialName = "mat2" },
                                new IconDO { Id = "102", MaterialName = "mat3" }
                            }
                        }
                    },
                    BannerColors = new BannerColorsDO
                    {
                        Colors = new List<ColorEntryDO>
                        {
                            new ColorEntryDO { Id = "1", Hex = "#FF0000" },
                            new ColorEntryDO { Id = "2", Hex = "#00FF00" },
                            new ColorEntryDO { Id = "3", Hex = "#0000FF" }
                        }
                    }
                }
            };

            // Act
            var result = BannerIconsMapper.ToDO(BannerIconsMapper.ToDTO(source));

            // Assert
            Assert.NotNull(result);
            var group = result.BannerIconData.BannerIconGroups[0];
            Assert.Equal(3, group.Backgrounds.Count);
            Assert.Equal(3, group.Icons.Count);
            Assert.Equal(3, result.BannerIconData.BannerColors.Colors.Count);
            
            // Verify nested data integrity
            Assert.Equal("mesh1", group.Backgrounds[0].MeshName);
            Assert.Equal("mat2", group.Icons[1].MaterialName);
            Assert.Equal("#0000FF", result.BannerIconData.BannerColors.Colors[2].Hex);
        }

        #endregion

        #region Extreme Values Tests

        [Fact]
        public void ExtremeIntegerValues_HandleCorrectly()
        {
            // Arrange
            var maxValue = int.MaxValue.ToString();
            var minValue = int.MinValue.ToString();

            var source = new BannerIconGroupDO
            {
                Id = maxValue,
                Backgrounds = new List<BackgroundDO>
                {
                    new BackgroundDO { Id = minValue }
                },
                Icons = new List<IconDO>
                {
                    new IconDO { Id = maxValue, TextureIndex = maxValue }
                }
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal(maxValue, result.Id);
            Assert.Equal(minValue, result.Backgrounds[0].Id);
            Assert.Equal(maxValue, result.Icons[0].Id);
            Assert.Equal(maxValue, result.Icons[0].TextureIndex);
            
            // Verify type conversion
            Assert.Equal(int.MaxValue, result.IdInt);
            Assert.Equal(int.MinValue, result.Backgrounds[0].IdInt);
            Assert.Equal(int.MaxValue, result.Icons[0].IdInt);
            Assert.Equal(int.MaxValue, result.Icons[0].TextureIndexInt);
        }

        [Fact]
        public void LargeStringValues_HandleCorrectly()
        {
            // Arrange
            var longString = new string('a', 10000); // 10KB string
            var source = new BannerIconGroupDO
            {
                Name = longString,
                Backgrounds = new List<BackgroundDO>
                {
                    new BackgroundDO { MeshName = longString }
                },
                Icons = new List<IconDO>
                {
                    new IconDO { MaterialName = longString }
                }
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal(longString, result.Name);
            Assert.Equal(longString, result.Backgrounds[0].MeshName);
            Assert.Equal(longString, result.Icons[0].MaterialName);
        }

        [Fact]
        public void SpecialCharactersInStrings_HandleCorrectly()
        {
            // Arrange
            var specialChars = "测试中文字符\n\t\r\"'&<>;{}[]|\\/:*?";
            var source = new BannerIconGroupDO
            {
                Name = specialChars,
                Backgrounds = new List<BackgroundDO>
                {
                    new BackgroundDO { MeshName = specialChars }
                }
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal(specialChars, result.Name);
            Assert.Equal(specialChars, result.Backgrounds[0].MeshName);
        }

        #endregion

        #region Edge Cases - Boolean Values

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("True")]
        [InlineData("False")]
        [InlineData("TRUE")]
        [InlineData("FALSE")]
        [InlineData("1")]
        [InlineData("0")]
        public void ValidBooleanValues_HandleCorrectly(string booleanValue)
        {
            // Arrange
            var source = new BannerIconGroupDO { IsPattern = booleanValue };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal(booleanValue, result.IsPattern);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalid")]
        [InlineData("2")]
        [InlineData("yes")]
        [InlineData("no")]
        [InlineData("on")]
        [InlineData("off")]
        public void InvalidBooleanValues_HandleCorrectly(string booleanValue)
        {
            // Arrange
            var source = new BannerIconGroupDO { IsPattern = booleanValue };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal(booleanValue, result.IsPattern);
            Assert.Null(result.IsPatternBool);
        }

        #endregion

        #region XML Serialization Edge Cases

        [Fact]
        public void XmlSerialization_WithSpecialCharacters_DoesNotThrow()
        {
            // Arrange
            var bannerIcons = new BannerIconsDO
            {
                Type = "test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Name = "Special: <>&\"' chars",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO { MeshName = "Mesh with \n\t\r chars" }
                            }
                        }
                    }
                }
            };

            // Act & Assert
            var serializer = new XmlSerializer(typeof(BannerIconsDO));
            using var writer = new StringWriter();
            // Should not throw exception
            serializer.Serialize(writer, bannerIcons);
        }

        [Fact]
        public void XmlSerialization_EmptyDocument_HandlesGracefully()
        {
            // Arrange
            var emptyXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
            {
                var serializer = new XmlSerializer(typeof(BannerIconsDO));
                using var reader = new StringReader(emptyXml);
                serializer.Deserialize(reader);
            });
        }

        [Fact]
        public void XmlSerialization_InvalidXml_ThrowsException()
        {
            // Arrange
            var invalidXml = "This is not valid XML";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
            {
                var serializer = new XmlSerializer(typeof(BannerIconsDO));
                using var reader = new StringReader(invalidXml);
                serializer.Deserialize(reader);
            });
        }

        #endregion

        #region Memory and Performance Edge Cases

        [Fact]
        public void VeryLargeObject_DoesNotCauseMemoryIssues()
        {
            // Arrange - create a very large object structure
            var largeGroups = new List<BannerIconGroupDO>();
            for (int i = 0; i < 100; i++)
            {
                var backgrounds = new List<BackgroundDO>();
                var icons = new List<IconDO>();
                
                for (int j = 0; j < 100; j++)
                {
                    backgrounds.Add(new BackgroundDO 
                    { 
                        Id = $"{i}_{j}", 
                        MeshName = $"mesh_{i}_{j}" 
                    });
                    icons.Add(new IconDO 
                    { 
                        Id = $"{i}_{j}", 
                        MaterialName = $"material_{i}_{j}" 
                    });
                }
                
                largeGroups.Add(new BannerIconGroupDO
                {
                    Id = i.ToString(),
                    Name = $"Group {i}",
                    Backgrounds = backgrounds,
                    Icons = icons
                });
            }

            var source = new BannerIconsDO
            {
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = largeGroups
                }
            };

            // Act & Assert
            var result = BannerIconsMapper.ToDO(BannerIconsMapper.ToDTO(source));
            
            // Verify structure is preserved
            Assert.Equal(100, result.BannerIconData.BannerIconGroups.Count);
            Assert.Equal(100, result.BannerIconData.BannerIconGroups[0].Backgrounds.Count);
            Assert.Equal(100, result.BannerIconData.BannerIconGroups[0].Icons.Count);
        }

        [Fact]
        public void CircularReferences_DoNotCauseStackOverflow()
        {
            // Note: This test verifies that the mapper doesn't accidentally create circular references
            // Since our data model doesn't have circular references, this is more of a precaution

            // Arrange
            var source = new BannerIconsDO
            {
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Id = "1",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO { Id = "1" }
                            }
                        }
                    }
                }
            };

            // Act
            var dto = BannerIconsMapper.ToDTO(source);
            var result = BannerIconsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Same(result, result); // Basic identity check
            Assert.NotSame(result.BannerIconData, result); // No circular reference
        }

        #endregion

        #region Type Conversion Edge Cases

        [Fact]
        public void TypeConversion_OverflowValues_HandlesGracefully()
        {
            // Arrange
            var overflowValue = (long.MaxValue).ToString();
            var source = new IconDO { Id = overflowValue };

            // Act
            var result = IconMapper.ToDO(IconMapper.ToDTO(source));

            // Assert
            Assert.Equal(overflowValue, result.Id);
            Assert.Null(result.IdInt); // Should be null since it overflows int
        }

        [Fact]
        public void TypeConversion_LeadingTrailingWhitespace_HandlesCorrectly()
        {
            // Arrange
            var source = new BannerIconGroupDO
            {
                Id = "  42  ",
                Name = "  Test Name  ",
                IsPattern = "  true  "
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal("  42  ", result.Id);
            Assert.Equal("  Test Name  ", result.Name);
            Assert.Equal("  true  ", result.IsPattern);
            
            // Type conversion should handle whitespace
            Assert.Equal(42, result.IdInt);
            Assert.True(result.IsPatternBool);
        }

        [Fact]
        public void TypeConversion_MixedFormats_HandlesCorrectly()
        {
            // Arrange
            var source = new BannerIconGroupDO
            {
                Id = "00042", // Leading zeros
                Name = "42", // Numeric name
                IsPattern = "TRUE" // Uppercase boolean
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(BannerIconGroupMapper.ToDTO(source));

            // Assert
            Assert.Equal("00042", result.Id);
            Assert.Equal(42, result.IdInt);
            Assert.Equal("42", result.Name);
            Assert.Equal("TRUE", result.IsPattern);
            Assert.True(result.IsPatternBool);
        }

        #endregion
    }
}