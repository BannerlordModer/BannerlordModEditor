using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// BannerIconsMapper单元测试套件
    /// 测试所有映射器方法的正确性、类型转换逻辑、空值处理和边界条件
    /// </summary>
    public class BannerIconsMapperTests
    {
        #region BannerIconsMapper Tests

        [Fact]
        public void BannerIconsMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            BannerIconsDO source = null;

            // Act
            var result = BannerIconsMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BannerIconsMapper_ToDTO_ValidInput_ReturnsCorrectDTO()
        {
            // Arrange
            var source = new BannerIconsDO
            {
                Type = "test_type",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Id = "1",
                            Name = "Test Group",
                            IsPattern = "true",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO { Id = "1", MeshName = "test_mesh" }
                            }
                        }
                    },
                    BannerColors = new BannerColorsDO
                    {
                        Colors = new List<ColorEntryDO>
                        {
                            new ColorEntryDO { Id = "1", Hex = "#FFFFFF" }
                        }
                    }
                }
            };

            // Act
            var result = BannerIconsMapper.ToDTO(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Type, result.Type);
            Assert.NotNull(result.BannerIconData);
            Assert.Single(result.BannerIconData.BannerIconGroups);
            Assert.Equal("1", result.BannerIconData.BannerIconGroups[0].Id);
            Assert.Equal("Test Group", result.BannerIconData.BannerIconGroups[0].Name);
            Assert.NotNull(result.BannerIconData.BannerColors);
            Assert.Single(result.BannerIconData.BannerColors.Colors);
        }

        [Fact]
        public void BannerIconsMapper_ToDO_NullInput_ReturnsNull()
        {
            // Arrange
            BannerIconsDTO source = null;

            // Act
            var result = BannerIconsMapper.ToDO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BannerIconsMapper_ToDO_ValidInput_SetsHasBannerIconDataFlag()
        {
            // Arrange
            var source = new BannerIconsDTO
            {
                Type = "test_type",
                BannerIconData = new BannerIconDataDTO
                {
                    BannerIconGroups = new List<BannerIconGroupDTO>
                    {
                        new BannerIconGroupDTO { Id = "1", Name = "Test" }
                    }
                }
            };

            // Act
            var result = BannerIconsMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasBannerIconData);
            Assert.NotNull(result.BannerIconData);
        }

        [Fact]
        public void BannerIconsMapper_ToDO_NullBannerIconData_SetsHasBannerIconDataToFalse()
        {
            // Arrange
            var source = new BannerIconsDTO
            {
                Type = "test_type",
                BannerIconData = null
            };

            // Act
            var result = BannerIconsMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasBannerIconData);
            Assert.Null(result.BannerIconData);
        }

        #endregion

        #region BannerIconDataMapper Tests

        [Fact]
        public void BannerIconDataMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            BannerIconDataDO source = null;

            // Act
            var result = BannerIconDataMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BannerIconDataMapper_ToDTO_EmptyCollections_ReturnsEmptyLists()
        {
            // Arrange
            var source = new BannerIconDataDO
            {
                BannerIconGroups = new List<BannerIconGroupDO>(),
                BannerColors = null
            };

            // Act
            var result = BannerIconDataMapper.ToDTO(source);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.BannerIconGroups);
            Assert.Null(result.BannerColors);
        }

        [Fact]
        public void BannerIconDataMapper_ToDO_SetsEmptyFlagsCorrectly()
        {
            // Arrange
            var source = new BannerIconDataDTO
            {
                BannerIconGroups = new List<BannerIconGroupDTO>(),
                BannerColors = new BannerColorsDTO
                {
                    Colors = new List<ColorEntryDTO>()
                }
            };

            // Act
            var result = BannerIconDataMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasEmptyBannerIconGroups);
            Assert.True(result.HasBannerColors);
            Assert.NotNull(result.BannerColors);
            Assert.True(result.BannerColors.HasEmptyColors);
        }

        [Fact]
        public void BannerIconDataMapper_ToDO_NullCollections_SetsFlagsToFalse()
        {
            // Arrange
            var source = new BannerIconDataDTO
            {
                BannerIconGroups = null,
                BannerColors = null
            };

            // Act
            var result = BannerIconDataMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasEmptyBannerIconGroups);
            Assert.False(result.HasBannerColors);
            Assert.Null(result.BannerColors);
        }

        #endregion

        #region BannerIconGroupMapper Tests

        [Fact]
        public void BannerIconGroupMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            BannerIconGroupDO source = null;

            // Act
            var result = BannerIconGroupMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BannerIconGroupMapper_ToDTO_PreservesAllProperties()
        {
            // Arrange
            var source = new BannerIconGroupDO
            {
                Id = "42",
                Name = "Test Group",
                IsPattern = "false",
                Backgrounds = new List<BackgroundDO>
                {
                    new BackgroundDO { Id = "1", MeshName = "mesh1" },
                    new BackgroundDO { Id = "2", MeshName = "mesh2" }
                },
                Icons = new List<IconDO>
                {
                    new IconDO { Id = "100", MaterialName = "material1" }
                }
            };

            // Act
            var result = BannerIconGroupMapper.ToDTO(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.Name, result.Name);
            Assert.Equal(source.IsPattern, result.IsPattern);
            Assert.Equal(2, result.Backgrounds.Count);
            Assert.Single(result.Icons);
        }

        [Fact]
        public void BannerIconGroupMapper_ToDO_SetsEmptyFlagsCorrectly()
        {
            // Arrange
            var source = new BannerIconGroupDTO
            {
                Id = "1",
                Name = "Test",
                Backgrounds = new List<BackgroundDTO>(),
                Icons = new List<IconDTO>()
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasEmptyBackgrounds);
            Assert.True(result.HasEmptyIcons);
        }

        [Fact]
        public void BannerIconGroupMapper_ToDO_NullCollections_SetsFlagsToFalse()
        {
            // Arrange
            var source = new BannerIconGroupDTO
            {
                Id = "1",
                Name = "Test",
                Backgrounds = null,
                Icons = null
            };

            // Act
            var result = BannerIconGroupMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasEmptyBackgrounds);
            Assert.False(result.HasEmptyIcons);
        }

        #endregion

        #region BackgroundMapper Tests

        [Fact]
        public void BackgroundMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            BackgroundDO source = null;

            // Act
            var result = BackgroundMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BackgroundMapper_ToDTO_PreservesAllProperties()
        {
            // Arrange
            var source = new BackgroundDO
            {
                Id = "123",
                MeshName = "test_mesh",
                IsBaseBackground = "true"
            };

            // Act
            var result = BackgroundMapper.ToDTO(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.MeshName, result.MeshName);
            Assert.Equal(source.IsBaseBackground, result.IsBaseBackground);
        }

        [Fact]
        public void BackgroundMapper_ToDO_NullInput_ReturnsNull()
        {
            // Arrange
            BackgroundDTO source = null;

            // Act
            var result = BackgroundMapper.ToDO(source);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region IconMapper Tests

        [Fact]
        public void IconMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            IconDO source = null;

            // Act
            var result = IconMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void IconMapper_ToDTO_PreservesAllProperties()
        {
            // Arrange
            var source = new IconDO
            {
                Id = "999",
                MaterialName = "test_material",
                TextureIndex = "42",
                IsReserved = "false"
            };

            // Act
            var result = IconMapper.ToDTO(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.MaterialName, result.MaterialName);
            Assert.Equal(source.TextureIndex, result.TextureIndex);
            Assert.Equal(source.IsReserved, result.IsReserved);
        }

        [Fact]
        public void IconMapper_ToDO_NullInput_ReturnsNull()
        {
            // Arrange
            IconDTO source = null;

            // Act
            var result = IconMapper.ToDO(source);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region BannerColorsMapper Tests

        [Fact]
        public void BannerColorsMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            BannerColorsDO source = null;

            // Act
            var result = BannerColorsMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BannerColorsMapper_ToDO_SetsEmptyColorsFlag()
        {
            // Arrange
            var source = new BannerColorsDTO
            {
                Colors = new List<ColorEntryDTO>()
            };

            // Act
            var result = BannerColorsMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasEmptyColors);
            Assert.Empty(result.Colors);
        }

        [Fact]
        public void BannerColorsMapper_ToDO_NullColors_SetsFlagToFalse()
        {
            // Arrange
            var source = new BannerColorsDTO
            {
                Colors = null
            };

            // Act
            var result = BannerColorsMapper.ToDO(source);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasEmptyColors);
            Assert.NotNull(result.Colors); // Mapper creates empty list when source.Colors is null
            Assert.Empty(result.Colors);
        }

        #endregion

        #region ColorEntryMapper Tests

        [Fact]
        public void ColorEntryMapper_ToDTO_NullInput_ReturnsNull()
        {
            // Arrange
            ColorEntryDO source = null;

            // Act
            var result = ColorEntryMapper.ToDTO(source);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ColorEntryMapper_ToDTO_PreservesAllProperties()
        {
            // Arrange
            var source = new ColorEntryDO
            {
                Id = "1",
                Hex = "#FF0000",
                PlayerCanChooseForBackground = "true",
                PlayerCanChooseForSigil = "false"
            };

            // Act
            var result = ColorEntryMapper.ToDTO(source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(source.Id, result.Id);
            Assert.Equal(source.Hex, result.Hex);
            Assert.Equal(source.PlayerCanChooseForBackground, result.PlayerCanChooseForBackground);
            Assert.Equal(source.PlayerCanChooseForSigil, result.PlayerCanChooseForSigil);
        }

        [Fact]
        public void ColorEntryMapper_ToDO_NullInput_ReturnsNull()
        {
            // Arrange
            ColorEntryDTO source = null;

            // Act
            var result = ColorEntryMapper.ToDO(source);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region Round-trip Tests

        [Fact]
        public void BannerIconsMapper_RoundTrip_PreservesData()
        {
            // Arrange
            var original = CreateTestBannerIconsDO();

            // Act
            var dto = BannerIconsMapper.ToDTO(original);
            var result = BannerIconsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(original.Type, result.Type);
            Assert.Equal(original.BannerIconData.BannerIconGroups.Count, result.BannerIconData.BannerIconGroups.Count);
            Assert.Equal(original.BannerIconData.BannerColors.Colors.Count, result.BannerIconData.BannerColors.Colors.Count);
            
            // Verify deep equality
            for (int i = 0; i < original.BannerIconData.BannerIconGroups.Count; i++)
            {
                var originalGroup = original.BannerIconData.BannerIconGroups[i];
                var resultGroup = result.BannerIconData.BannerIconGroups[i];
                
                Assert.Equal(originalGroup.Id, resultGroup.Id);
                Assert.Equal(originalGroup.Name, resultGroup.Name);
                Assert.Equal(originalGroup.IsPattern, resultGroup.IsPattern);
            }
        }

        [Fact]
        public void BannerIconsMapper_RoundTripWithEmptyCollections_PreservesEmptyFlags()
        {
            // Arrange
            var original = new BannerIconsDO
            {
                Type = "test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>(),
                    BannerColors = new BannerColorsDO
                    {
                        Colors = new List<ColorEntryDO>()
                    }
                }
            };

            // Act
            var dto = BannerIconsMapper.ToDTO(original);
            var result = BannerIconsMapper.ToDO(dto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasBannerIconData);
            Assert.True(result.BannerIconData.HasEmptyBannerIconGroups);
            Assert.True(result.BannerIconData.HasBannerColors);
            Assert.True(result.BannerIconData.BannerColors.HasEmptyColors);
        }

        #endregion

        #region Helper Methods

        private BannerIconsDO CreateTestBannerIconsDO()
        {
            return new BannerIconsDO
            {
                Type = "test_type",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Id = "1",
                            Name = "Test Group",
                            IsPattern = "true",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO { Id = "1", MeshName = "mesh1", IsBaseBackground = "false" }
                            },
                            Icons = new List<IconDO>
                            {
                                new IconDO { Id = "100", MaterialName = "mat1", TextureIndex = "0", IsReserved = "false" }
                            }
                        }
                    },
                    BannerColors = new BannerColorsDO
                    {
                        Colors = new List<ColorEntryDO>
                        {
                            new ColorEntryDO 
                            { 
                                Id = "1", 
                                Hex = "#FFFFFF", 
                                PlayerCanChooseForBackground = "true", 
                                PlayerCanChooseForSigil = "true" 
                            }
                        }
                    }
                }
            };
        }

        #endregion
    }
}