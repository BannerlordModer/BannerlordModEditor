using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Engine;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalTexturesXmlTests
    {
        [Fact]
        public void DecalTextures_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""decal_texture"">
	<decal_textures>
		<decal_texture
			name=""black""
			atlas_pos=""0.990295, 0.624023, 0.000000, 0.001953""
			is_dynamic=""false"" />
		<decal_texture
			name=""blob_shadow""
			atlas_pos=""0.516724, 0.793945, 0.015625, 0.031250""
			is_dynamic=""false"" />
		<decal_texture
			name=""cursor""
			atlas_pos=""0.990295, 0.614258, 0.001953, 0.003906""
			is_dynamic=""false"" />
		<decal_texture
			name=""blood_debug_d""
			atlas_pos=""0.751953, 0.541016, 0.031250, 0.062500""
			is_dynamic=""false"" />
		<decal_texture
			name=""road_01_decal_d""
			atlas_pos=""0.833008, 0.142578, 0.062500, 0.125000""
			is_dynamic=""false"" />
	</decal_textures>
</base>";

            var serializer = new XmlSerializer(typeof(DecalTexturesRoot));

            // Act
            DecalTexturesRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (DecalTexturesRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("decal_texture", result.Type);
            Assert.NotNull(result.DecalTextures);
            Assert.NotNull(result.DecalTextures.DecalTexture);
            Assert.Equal(5, result.DecalTextures.DecalTexture.Length);

            // Test first decal texture
            var blackTexture = result.DecalTextures.DecalTexture[0];
            Assert.Equal("black", blackTexture.Name);
            Assert.Equal("0.990295, 0.624023, 0.000000, 0.001953", blackTexture.AtlasPos);
            Assert.Equal("false", blackTexture.IsDynamic);

            // Test blob shadow texture
            var blobShadowTexture = result.DecalTextures.DecalTexture[1];
            Assert.Equal("blob_shadow", blobShadowTexture.Name);
            Assert.Equal("0.516724, 0.793945, 0.015625, 0.031250", blobShadowTexture.AtlasPos);
            Assert.Equal("false", blobShadowTexture.IsDynamic);

            // Test cursor texture
            var cursorTexture = result.DecalTextures.DecalTexture[2];
            Assert.Equal("cursor", cursorTexture.Name);
            Assert.Equal("0.990295, 0.614258, 0.001953, 0.003906", cursorTexture.AtlasPos);
            Assert.Equal("false", cursorTexture.IsDynamic);

            // Test blood debug texture
            var bloodDebugTexture = result.DecalTextures.DecalTexture[3];
            Assert.Equal("blood_debug_d", bloodDebugTexture.Name);
            Assert.Equal("0.751953, 0.541016, 0.031250, 0.062500", bloodDebugTexture.AtlasPos);
            Assert.Equal("false", bloodDebugTexture.IsDynamic);

            // Test road decal texture  
            var roadDecalTexture = result.DecalTextures.DecalTexture[4];
            Assert.Equal("road_01_decal_d", roadDecalTexture.Name);
            Assert.Equal("0.833008, 0.142578, 0.062500, 0.125000", roadDecalTexture.AtlasPos);
            Assert.Equal("false", roadDecalTexture.IsDynamic);
        }

        [Fact]
        public void DecalTextures_WithDynamicTexture_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""decal_texture"">
	<decal_textures>
		<decal_texture
			name=""dynamic_test""
			atlas_pos=""0.500000, 0.500000, 0.250000, 0.250000""
			is_dynamic=""true"" />
		<decal_texture
			name=""static_test""
			atlas_pos=""0.000000, 0.000000, 0.125000, 0.125000""
			is_dynamic=""false"" />
	</decal_textures>
</base>";

            var serializer = new XmlSerializer(typeof(DecalTexturesRoot));

            // Act
            DecalTexturesRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (DecalTexturesRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("decal_texture", result.Type);
            Assert.NotNull(result.DecalTextures);
            Assert.NotNull(result.DecalTextures.DecalTexture);
            Assert.Equal(2, result.DecalTextures.DecalTexture.Length);

            var dynamicTexture = result.DecalTextures.DecalTexture[0];
            Assert.Equal("dynamic_test", dynamicTexture.Name);
            Assert.Equal("0.500000, 0.500000, 0.250000, 0.250000", dynamicTexture.AtlasPos);
            Assert.Equal("true", dynamicTexture.IsDynamic);

            var staticTexture = result.DecalTextures.DecalTexture[1];
            Assert.Equal("static_test", staticTexture.Name);
            Assert.Equal("0.000000, 0.000000, 0.125000, 0.125000", staticTexture.AtlasPos);
            Assert.Equal("false", staticTexture.IsDynamic);
        }

        [Fact]
        public void DecalTextures_CanSerializeToXml()
        {
            // Arrange
            var decalTexturesRoot = new DecalTexturesRoot
            {
                Type = "decal_texture",
                DecalTextures = new DecalTextures
                {
                    DecalTexture = new[]
                    {
                        new DecalTexture
                        {
                            Name = "test_texture_1",
                            AtlasPos = "0.100000, 0.200000, 0.050000, 0.075000",
                            IsDynamic = "false"
                        },
                        new DecalTexture
                        {
                            Name = "test_texture_2",
                            AtlasPos = "0.300000, 0.400000, 0.025000, 0.050000",
                            IsDynamic = "true"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(DecalTexturesRoot));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, decalTexturesRoot);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("type=\"decal_texture\"", result);
            Assert.Contains("name=\"test_texture_1\"", result);
            Assert.Contains("atlas_pos=\"0.100000, 0.200000, 0.050000, 0.075000\"", result);
            Assert.Contains("is_dynamic=\"false\"", result);
            Assert.Contains("name=\"test_texture_2\"", result);
            Assert.Contains("atlas_pos=\"0.300000, 0.400000, 0.025000, 0.050000\"", result);
            Assert.Contains("is_dynamic=\"true\"", result);
        }

        [Fact]
        public void DecalTextures_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new DecalTexturesRoot
            {
                Type = "decal_texture",
                DecalTextures = new DecalTextures
                {
                    DecalTexture = new[]
                    {
                        new DecalTexture
                        {
                            Name = "roundtrip_test",
                            AtlasPos = "0.750000, 0.250000, 0.125000, 0.125000",
                            IsDynamic = "false"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(DecalTexturesRoot));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            DecalTexturesRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (DecalTexturesRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("decal_texture", result.Type);
            Assert.NotNull(result.DecalTextures);
            Assert.NotNull(result.DecalTextures.DecalTexture);
            Assert.Single(result.DecalTextures.DecalTexture);

            var texture = result.DecalTextures.DecalTexture[0];
            Assert.Equal("roundtrip_test", texture.Name);
            Assert.Equal("0.750000, 0.250000, 0.125000, 0.125000", texture.AtlasPos);
            Assert.Equal("false", texture.IsDynamic);
        }

        [Fact]
        public void DecalTextures_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""decal_texture"">
	<decal_textures>
	</decal_textures>
</base>";

            var serializer = new XmlSerializer(typeof(DecalTexturesRoot));

            // Act
            DecalTexturesRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (DecalTexturesRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("decal_texture", result.Type);
            Assert.NotNull(result.DecalTextures);
        }
    }
} 