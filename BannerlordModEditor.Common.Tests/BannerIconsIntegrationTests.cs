using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// BannerIcons集成测试套件
    /// 测试完整的DO/DTO模型序列化/反序列化流程、XML处理工具的增强功能
    /// </summary>
    public class BannerIconsIntegrationTests
    {
        #region Complete Round-trip Tests

        [Fact]
        public void CompleteRoundTrip_RealXml_PreservesStructure()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                // Skip test if test data not available
                return;
            }

            var originalXml = File.ReadAllText(xmlPath);

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(model, originalXml);
            var isEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);

            // Assert
            Assert.True(isEqual, "XML structure should be preserved after round-trip serialization");
        }

        [Fact]
        public void CompleteRoundTrip_ComplexObject_PreservesAllData()
        {
            // Arrange
            var original = CreateComplexBannerIconsDO();

            // Act
            var dto = BannerIconsMapper.ToDTO(original);
            var result = BannerIconsMapper.ToDO(dto);

            // Assert
            AssertDeepEquality(original, result);
        }

        [Fact]
        public void CompleteRoundTrip_WithEmptyElements_PreservesEmptyFlags()
        {
            // Arrange
            var original = new BannerIconsDO
            {
                Type = "test",
                HasBannerIconData = true,
                BannerIconData = new BannerIconDataDO
                {
                    HasEmptyBannerIconGroups = true,
                    BannerIconGroups = new List<BannerIconGroupDO>(),
                    HasBannerColors = true,
                    BannerColors = new BannerColorsDO
                    {
                        HasEmptyColors = true,
                        Colors = new List<ColorEntryDO>()
                    }
                }
            };

            // Act
            var dto = BannerIconsMapper.ToDTO(original);
            var result = BannerIconsMapper.ToDO(dto);

            // Assert
            Assert.True(result.HasBannerIconData);
            Assert.True(result.BannerIconData.HasEmptyBannerIconGroups);
            Assert.True(result.BannerIconData.HasBannerColors);
            Assert.True(result.BannerIconData.BannerColors.HasEmptyColors);
        }

        #endregion

        #region XML Serialization Integration Tests

        [Fact]
        public void XmlSerialization_RealBannerIconsXml_MatchesExpectedStructure()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                return; // Skip if test data not available
            }

            var xml = File.ReadAllText(xmlPath);

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(xml);

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.BannerIconData);
            Assert.NotEmpty(model.BannerIconData.BannerIconGroups);
            Assert.NotNull(model.BannerIconData.BannerColors);
            Assert.NotEmpty(model.BannerIconData.BannerColors.Colors);

            // Verify specific structure elements
            VerifyBannerIconsStructure(model);
        }

        [Fact]
        public void XmlSerialization_DeserializeAndSerialize_ProducesIdenticalStructure()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                return; // Skip if test data not available
            }

            var originalXml = File.ReadAllText(xmlPath);

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(model, originalXml);

            // Assert
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            // Compare basic structure
            Assert.Equal(originalDoc.Root?.Name, serializedDoc.Root?.Name);
            
            // Compare type attribute
            var originalType = originalDoc.Root?.Attribute("type")?.Value;
            var serializedType = serializedDoc.Root?.Attribute("type")?.Value;
            Assert.Equal(originalType, serializedType);

            // Compare BannerIconData presence
            var originalHasData = originalDoc.Root?.Element("BannerIconData") != null;
            var serializedHasData = serializedDoc.Root?.Element("BannerIconData") != null;
            Assert.Equal(originalHasData, serializedHasData);
        }

        [Fact]
        public void XmlTestUtils_EnhancedFeatures_WorkWithBannerIcons()
        {
            // Arrange
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""1"" name=""Test"" is_pattern=""true"">
            <Backgrounds/>
            <Icons/>
        </BannerIconGroup>
        <BannerColors>
            <Colors/>
        </BannerColors>
    </BannerIconData>
</base>";

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(testXml);

            // Assert
            Assert.NotNull(model);
            Assert.True(model.HasBannerIconData);
            Assert.True(model.BannerIconData.HasEmptyBannerIconGroups);
            Assert.True(model.BannerIconData.HasBannerColors);
            Assert.True(model.BannerIconData.BannerColors.HasEmptyColors);
            
            // Verify that empty elements are properly detected
            Assert.Empty(model.BannerIconData.BannerIconGroups);
            Assert.Empty(model.BannerIconData.BannerColors.Colors);
        }

        #endregion

        #region XmlTestUtils Integration Tests

        [Fact]
        public void XmlTestUtils_CompareXmlStructure_DetectsBannerIconsDifferences()
        {
            // Arrange
            var xml1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""1"" name=""Test"" is_pattern=""true""/>
    </BannerIconData>
</base>";

            var xml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""2"" name=""Test"" is_pattern=""true""/>
    </BannerIconData>
</base>";

            // Act
            var isEqual = XmlTestUtils.AreStructurallyEqual(xml1, xml2);
            var report = XmlTestUtils.CompareXmlStructure(xml1, xml2);

            // Assert
            Assert.False(isEqual);
            Assert.Contains("id", report.AttributeValueDifferences[0]);
        }

        [Fact]
        public void XmlTestUtils_CleanXmlForComparison_HandlesBannerIconsCorrectly()
        {
            // Arrange
            var xmlWithComments = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- This is a comment -->
<base type=""test"">
    <BannerIconData>
        <!-- Another comment -->
        <BannerIconGroup id=""1"" name=""Test"" is_pattern=""true""/>
    </BannerIconData>
</base>";

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""1"" name=""Test"" is_pattern=""true""/>
    </BannerIconData>
</base>";

            // Act
            var cleanedXml = XmlTestUtils.CleanXmlForComparison(xmlWithComments);

            // Assert
            Assert.DoesNotContain("<!--", cleanedXml);
            Assert.Contains("BannerIconGroup", cleanedXml);
        }

        [Fact]
        public void XmlTestUtils_BooleanNormalization_HandlesBannerIconsBooleans()
        {
            // Arrange
            var xmlWithMixedBooleans = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""1"" name=""Test1"" is_pattern=""True""/>
        <BannerIconGroup id=""2"" name=""Test2"" is_pattern=""false""/>
        <BannerIconGroup id=""3"" name=""Test3"" is_pattern=""1""/>
        <BannerIconGroup id=""4"" name=""Test4"" is_pattern=""0""/>
    </BannerIconData>
</base>";

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(xmlWithMixedBooleans);
            var serializedXml = XmlTestUtils.Serialize(model, xmlWithMixedBooleans);

            // Assert
            // Verify that boolean values are preserved
            Assert.Contains("is_pattern=\"True\"", serializedXml);
            Assert.Contains("is_pattern=\"false\"", serializedXml);
            Assert.Contains("is_pattern=\"1\"", serializedXml);
            Assert.Contains("is_pattern=\"0\"", serializedXml);
        }

        #endregion

        #region Real-world Data Integration Tests

        [Fact]
        public void RealDataIntegration_LargeBannerIconsFile_HandlesCorrectly()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                return; // Skip if test data not available
            }

            var xml = File.ReadAllText(xmlPath);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(model, xml);
            var isEqual = XmlTestUtils.AreStructurallyEqual(xml, serializedXml);
            stopwatch.Stop();

            // Assert
            Assert.True(isEqual);
            Assert.True(stopwatch.ElapsedMilliseconds < 5000, $"Deserialization took too long: {stopwatch.ElapsedMilliseconds}ms");
            
            // Verify that all data is preserved
            Assert.NotNull(model.BannerIconData);
            Assert.True(model.BannerIconData.BannerIconGroups.Count > 0);
            Assert.NotNull(model.BannerIconData.BannerColors);
            Assert.True(model.BannerIconData.BannerColors.Colors.Count > 0);
        }

        [Fact]
        public void RealDataIntegration_EnhancedXmlTestUtils_PreservesStructure()
        {
            // Arrange
            var xmlPath = "TestData/banner_icons.xml";
            if (!File.Exists(xmlPath))
            {
                return; // Skip if test data not available
            }

            var originalXml = File.ReadAllText(xmlPath);

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(originalXml);
            
            // Verify that enhanced features work correctly
            Assert.True(model.HasBannerIconData);
            
            // Verify empty element detection
            if (model.BannerIconData.BannerIconGroups.Count == 0)
            {
                Assert.True(model.BannerIconData.HasEmptyBannerIconGroups);
            }
            
            if (model.BannerIconData.BannerColors.Colors.Count == 0)
            {
                Assert.True(model.BannerIconData.BannerColors.HasEmptyColors);
            }

            // Serialize back and verify structure
            var serializedXml = XmlTestUtils.Serialize(model, originalXml);
            var isEqual = XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml);

            // Assert
            Assert.True(isEqual);
        }

        #endregion

        #region Error Handling and Recovery Tests

        [Fact]
        public void ErrorHandling_MalformedXml_ThrowsAppropriateException()
        {
            // Arrange
            var malformedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""1"" name=""Test"" is_pattern=""true"">
        </BannerIconGroup> <!-- Missing closing tag for nested elements -->
    </BannerIconData>
</base>";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
            {
                XmlTestUtils.Deserialize<BannerIconsDO>(malformedXml);
            });
        }

        [Fact]
        public void ErrorHandling_InvalidAttributeValues_HandlesGracefully()
        {
            // Arrange
            var xmlWithInvalidValues = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData>
        <BannerIconGroup id=""invalid_id"" name=""Test"" is_pattern=""maybe""/>
    </BannerIconData>
</base>";

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(xmlWithInvalidValues);

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(model.BannerIconData);
            Assert.Single(model.BannerIconData.BannerIconGroups);
            
            var group = model.BannerIconData.BannerIconGroups[0];
            Assert.Equal("invalid_id", group.Id);
            Assert.Null(group.IdInt); // Should be null for invalid integer
            Assert.Equal("maybe", group.IsPattern);
            Assert.Null(group.IsPatternBool); // Should be null for invalid boolean
        }

        [Fact]
        public void ErrorHandling_MissingRequiredElements_HandlesGracefully()
        {
            // Arrange
            var minimalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""test"">
    <BannerIconData/>
</base>";

            // Act
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(minimalXml);

            // Assert
            Assert.NotNull(model);
            Assert.True(model.HasBannerIconData);
            Assert.NotNull(model.BannerIconData);
            Assert.Empty(model.BannerIconData.BannerIconGroups);
            Assert.Null(model.BannerIconData.BannerColors);
        }

        #endregion

        #region Helper Methods

        private BannerIconsDO CreateComplexBannerIconsDO()
        {
            return new BannerIconsDO
            {
                Type = "complex_test",
                BannerIconData = new BannerIconDataDO
                {
                    BannerIconGroups = new List<BannerIconGroupDO>
                    {
                        new BannerIconGroupDO
                        {
                            Id = "1",
                            Name = "{=str_banner_editor_background}Background",
                            IsPattern = "true",
                            Backgrounds = new List<BackgroundDO>
                            {
                                new BackgroundDO 
                                { 
                                    Id = "1", 
                                    MeshName = "banner_background_test_1",
                                    IsBaseBackground = "false"
                                },
                                new BackgroundDO 
                                { 
                                    Id = "2", 
                                    MeshName = "banner_background_test_2",
                                    IsBaseBackground = "true"
                                }
                            },
                            Icons = new List<IconDO>
                            {
                                new IconDO 
                                { 
                                    Id = "100", 
                                    MaterialName = "custom_banner_icons_01",
                                    TextureIndex = "10",
                                    IsReserved = "false"
                                },
                                new IconDO 
                                { 
                                    Id = "101", 
                                    MaterialName = "custom_banner_icons_02",
                                    TextureIndex = "15",
                                    IsReserved = "true"
                                }
                            }
                        },
                        new BannerIconGroupDO
                        {
                            Id = "2",
                            Name = "{=str_banner_editor_animal}Animal",
                            IsPattern = "false",
                            Backgrounds = new List<BackgroundDO>(),
                            Icons = new List<IconDO>
                            {
                                new IconDO 
                                { 
                                    Id = "200", 
                                    MaterialName = "animal_icons_01",
                                    TextureIndex = "0",
                                    IsReserved = "false"
                                }
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
                            },
                            new ColorEntryDO 
                            { 
                                Id = "2", 
                                Hex = "#000000",
                                PlayerCanChooseForBackground = "true",
                                PlayerCanChooseForSigil = "false"
                            },
                            new ColorEntryDO 
                            { 
                                Id = "3", 
                                Hex = "#FF0000",
                                PlayerCanChooseForBackground = "false",
                                PlayerCanChooseForSigil = "true"
                            }
                        }
                    }
                }
            };
        }

        private void AssertDeepEquality(BannerIconsDO expected, BannerIconsDO actual)
        {
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.HasBannerIconData, actual.HasBannerIconData);
            
            if (expected.BannerIconData == null)
            {
                Assert.Null(actual.BannerIconData);
                return;
            }
            
            Assert.NotNull(actual.BannerIconData);
            Assert.Equal(expected.BannerIconData.HasEmptyBannerIconGroups, actual.BannerIconData.HasEmptyBannerIconGroups);
            Assert.Equal(expected.BannerIconData.HasBannerColors, actual.BannerIconData.HasBannerColors);
            
            // Compare BannerIconGroups
            Assert.Equal(expected.BannerIconData.BannerIconGroups.Count, actual.BannerIconData.BannerIconGroups.Count);
            for (int i = 0; i < expected.BannerIconData.BannerIconGroups.Count; i++)
            {
                var expectedGroup = expected.BannerIconData.BannerIconGroups[i];
                var actualGroup = actual.BannerIconData.BannerIconGroups[i];
                
                Assert.Equal(expectedGroup.Id, actualGroup.Id);
                Assert.Equal(expectedGroup.Name, actualGroup.Name);
                Assert.Equal(expectedGroup.IsPattern, actualGroup.IsPattern);
                Assert.Equal(expectedGroup.HasEmptyBackgrounds, actualGroup.HasEmptyBackgrounds);
                Assert.Equal(expectedGroup.HasEmptyIcons, actualGroup.HasEmptyIcons);
                
                // Compare Backgrounds
                Assert.Equal(expectedGroup.Backgrounds.Count, actualGroup.Backgrounds.Count);
                for (int j = 0; j < expectedGroup.Backgrounds.Count; j++)
                {
                    var expectedBg = expectedGroup.Backgrounds[j];
                    var actualBg = actualGroup.Backgrounds[j];
                    
                    Assert.Equal(expectedBg.Id, actualBg.Id);
                    Assert.Equal(expectedBg.MeshName, actualBg.MeshName);
                    Assert.Equal(expectedBg.IsBaseBackground, actualBg.IsBaseBackground);
                }
                
                // Compare Icons
                Assert.Equal(expectedGroup.Icons.Count, actualGroup.Icons.Count);
                for (int j = 0; j < expectedGroup.Icons.Count; j++)
                {
                    var expectedIcon = expectedGroup.Icons[j];
                    var actualIcon = actualGroup.Icons[j];
                    
                    Assert.Equal(expectedIcon.Id, actualIcon.Id);
                    Assert.Equal(expectedIcon.MaterialName, actualIcon.MaterialName);
                    Assert.Equal(expectedIcon.TextureIndex, actualIcon.TextureIndex);
                    Assert.Equal(expectedIcon.IsReserved, actualIcon.IsReserved);
                }
            }
            
            // Compare BannerColors
            if (expected.BannerIconData.BannerColors == null)
            {
                Assert.Null(actual.BannerIconData.BannerColors);
            }
            else
            {
                Assert.NotNull(actual.BannerIconData.BannerColors);
                Assert.Equal(expected.BannerIconData.BannerColors.HasEmptyColors, actual.BannerIconData.BannerColors.HasEmptyColors);
                Assert.Equal(expected.BannerIconData.BannerColors.Colors.Count, actual.BannerIconData.BannerColors.Colors.Count);
                
                for (int i = 0; i < expected.BannerIconData.BannerColors.Colors.Count; i++)
                {
                    var expectedColor = expected.BannerIconData.BannerColors.Colors[i];
                    var actualColor = actual.BannerIconData.BannerColors.Colors[i];
                    
                    Assert.Equal(expectedColor.Id, actualColor.Id);
                    Assert.Equal(expectedColor.Hex, actualColor.Hex);
                    Assert.Equal(expectedColor.PlayerCanChooseForBackground, actualColor.PlayerCanChooseForBackground);
                    Assert.Equal(expectedColor.PlayerCanChooseForSigil, actualColor.PlayerCanChooseForSigil);
                }
            }
        }

        private void VerifyBannerIconsStructure(BannerIconsDO model)
        {
            Assert.NotNull(model.BannerIconData);
            
            // Verify at least one group exists
            Assert.True(model.BannerIconData.BannerIconGroups.Count > 0);
            
            // Verify group structure
            foreach (var group in model.BannerIconData.BannerIconGroups)
            {
                Assert.NotNull(group.Id);
                Assert.NotNull(group.Name);
                Assert.NotNull(group.IsPattern);
                Assert.NotNull(group.Backgrounds);
                Assert.NotNull(group.Icons);
            }
            
            // Verify colors structure
            Assert.NotNull(model.BannerIconData.BannerColors);
            Assert.NotNull(model.BannerIconData.BannerColors.Colors);
            
            foreach (var color in model.BannerIconData.BannerColors.Colors)
            {
                Assert.NotNull(color.Id);
                Assert.NotNull(color.Hex);
            }
        }

        #endregion
    }
}