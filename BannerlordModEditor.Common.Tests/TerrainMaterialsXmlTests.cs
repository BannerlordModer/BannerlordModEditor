using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Engine;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class TerrainMaterialsXmlTests
    {
        [Fact]
        public void TerrainMaterials_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            var originalXml = File.ReadAllText(xmlPath);
            
            // Act - 反序列化
            var terrainMaterials = XmlTestUtils.Deserialize<TerrainMaterialsDO>(originalXml);
            
            // Act - 序列化
            var savedXml = XmlTestUtils.Serialize(terrainMaterials, originalXml);
            
            // Assert - 基本结构验证
            Assert.NotNull(terrainMaterials);
            Assert.NotNull(terrainMaterials.TerrainMaterialList);
            Assert.True(terrainMaterials.TerrainMaterialList.Count > 0, "Should have at least one terrain material");
            
            // 验证具体的地形材质数据
            var defaultMaterial = terrainMaterials.TerrainMaterialList.FirstOrDefault(t => t.Name == "default");
            Assert.NotNull(defaultMaterial);
            Assert.Equal("true", defaultMaterial.IsEnabled);
            Assert.Equal("false", defaultMaterial.IsFloraLayer);
            Assert.Equal("false", defaultMaterial.IsMeshBlendLayer);
            Assert.Equal("soil", defaultMaterial.PhysicsMaterial);
            Assert.Equal("0.000, 0.000, 0.000", defaultMaterial.PitchRollYaw);
            Assert.Equal("5.000, 5.000", defaultMaterial.Scale);
            
            // 验证纹理容器
            Assert.NotNull(defaultMaterial.Textures);
            Assert.NotNull(defaultMaterial.Textures.TextureList);
            Assert.True(defaultMaterial.Textures.TextureList.Count >= 5, "Should have at least 5 textures");
            
            var diffuseTexture = defaultMaterial.Textures.TextureList.FirstOrDefault(t => t.Type == "diffusemap");
            Assert.NotNull(diffuseTexture);
            Assert.Equal("editor_grid_8", diffuseTexture.Name);
            
            // 验证层标志容器
            Assert.NotNull(defaultMaterial.LayerFlags);
            Assert.NotNull(defaultMaterial.LayerFlags.FlagList);
            Assert.True(defaultMaterial.LayerFlags.FlagList.Count >= 7, "Should have at least 7 layer flags");
            
            var parallaxFlag = defaultMaterial.LayerFlags.FlagList.FirstOrDefault(f => f.Name == "use_parallax");
            Assert.NotNull(parallaxFlag);
            Assert.Equal("false", parallaxFlag.Value);
            
            // 验证网格容器（default材质应该有空的meshes容器）
            Assert.NotNull(defaultMaterial.Meshes);
            Assert.NotNull(defaultMaterial.Meshes.MeshList);
            Assert.Empty(defaultMaterial.Meshes.MeshList);
            
            // 验证有网格的材质
            var floraMaterial = terrainMaterials.TerrainMaterialList.FirstOrDefault(t => t.Name == "flora_habitat_a");
            Assert.NotNull(floraMaterial);
            Assert.NotNull(floraMaterial.Meshes);
            Assert.NotNull(floraMaterial.Meshes.MeshList);
            Assert.True(floraMaterial.Meshes.MeshList.Count > 0, "Flora material should have meshes");
            
            var floraMesh = floraMaterial.Meshes.MeshList.FirstOrDefault(m => m.Name == "flora_grass_a_non_shadow");
            Assert.NotNull(floraMesh);
            Assert.Equal("55", floraMesh.Density);
            Assert.Equal("1", floraMesh.SeedIndex);
            Assert.Equal("0.650, 0.650, 0.650", floraMesh.SizeMin);
            Assert.Equal("1.000, 1.000, 1.000", floraMesh.SizeMax);
            
            // 验证可选属性的处理
            var rockCliffMaterial = terrainMaterials.TerrainMaterialList.FirstOrDefault(t => t.Name == "rock_cliff");
            Assert.NotNull(rockCliffMaterial);
            Assert.Equal("0", rockCliffMaterial.DetailLevelAdjustment); // 这个材质有detail_level_adjustment
            Assert.Equal("1.000", rockCliffMaterial.GroundSlopeScale); // 这个材质有ground_slope_scale
            
            // 验证序列化的XML不包含null值属性
            Assert.DoesNotContain("detail_level_adjustment=\"\"", savedXml);
            Assert.DoesNotContain("ground_slope_scale=\"\"", savedXml);
            Assert.DoesNotContain("albedo_factor_color=\"\"", savedXml);
            
            // 验证序列化包含必要的属性
            Assert.Contains("name=\"default\"", savedXml);
            Assert.Contains("physics_material=\"soil\"", savedXml);
            Assert.Contains("<texture", savedXml);
            Assert.Contains("<flag", savedXml);
        }
        
        [Fact]
        public void TerrainMaterials_ShouldHandleOptionalAttributes()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(TerrainMaterialsDO));
            TerrainMaterialsDO terrainMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                terrainMaterials = (TerrainMaterialsDO)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证默认材质没有可选属性
            var defaultMaterial = terrainMaterials.TerrainMaterialList.FirstOrDefault(t => t.Name == "default");
            Assert.NotNull(defaultMaterial);
            Assert.Null(defaultMaterial.DetailLevelAdjustment);
            Assert.Null(defaultMaterial.GroundSlopeScale);
            Assert.Null(defaultMaterial.AlbedoFactorColor);
            Assert.Null(defaultMaterial.AlbedoFactorMode);
            Assert.Null(defaultMaterial.SmoothBlendAmount);
            
            // 验证有可选属性的材质
            var mudCliffMaterial = terrainMaterials.TerrainMaterialList.FirstOrDefault(t => t.Name == "mud_cliff");
            Assert.NotNull(mudCliffMaterial);
            Assert.NotNull(mudCliffMaterial.AlbedoFactorColor);
            Assert.NotNull(mudCliffMaterial.AlbedoFactorMode);
            Assert.NotNull(mudCliffMaterial.SmoothBlendAmount);
            Assert.Equal("1.000, 1.000, 1.000", mudCliffMaterial.AlbedoFactorColor);
            Assert.Equal("0", mudCliffMaterial.AlbedoFactorMode);
            Assert.Equal("1.000", mudCliffMaterial.SmoothBlendAmount);
        }

        [Fact]
        public void TerrainMaterials_ShouldHandleEmptyMeshes()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(TerrainMaterialsDO));
            TerrainMaterialsDO terrainMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                terrainMaterials = (TerrainMaterialsDO)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证空meshes和有内容的meshes都能正确处理
            var materialsWithEmptyMeshes = terrainMaterials.TerrainMaterialList.Where(t => t.Meshes?.MeshList.Count == 0).ToList();
            var materialsWithMeshes = terrainMaterials.TerrainMaterialList.Where(t => t.Meshes?.MeshList.Count > 0).ToList();
            
            Assert.True(materialsWithEmptyMeshes.Count > 0, "Should have materials with empty meshes");
            Assert.True(materialsWithMeshes.Count > 0, "Should have materials with meshes");
            
            // 验证空meshes的材质
            var emptyMeshesMaterial = materialsWithEmptyMeshes.First();
            Assert.NotNull(emptyMeshesMaterial.Meshes);
            Assert.NotNull(emptyMeshesMaterial.Meshes.MeshList);
            Assert.Empty(emptyMeshesMaterial.Meshes.MeshList);
            
            // 验证有meshes的材质
            var meshesMaterial = materialsWithMeshes.First();
            Assert.NotNull(meshesMaterial.Meshes);
            Assert.NotNull(meshesMaterial.Meshes.MeshList);
            Assert.True(meshesMaterial.Meshes.MeshList.Count > 0);
            
            var firstMesh = meshesMaterial.Meshes.MeshList.First();
            Assert.NotNull(firstMesh.Name);
            Assert.NotNull(firstMesh.Density);
            Assert.NotNull(firstMesh.SeedIndex);
            Assert.NotNull(firstMesh.ColonyRadius);
            Assert.NotNull(firstMesh.ColonyThreshold);
            Assert.NotNull(firstMesh.SizeMin);
            Assert.NotNull(firstMesh.SizeMax);
            Assert.NotNull(firstMesh.AlbedoMultiplier);
            Assert.NotNull(firstMesh.WeightOffset);
        }

        [Fact]
        public void TerrainMaterials_ShouldPreserveNumericPrecision()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "terrain_materials.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(TerrainMaterialsDO));
            TerrainMaterialsDO terrainMaterials;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                terrainMaterials = (TerrainMaterialsDO)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证数值精度保持
            var summerMaterial = terrainMaterials.TerrainMaterialList.FirstOrDefault(t => t.Name == "summerFpreset");
            Assert.NotNull(summerMaterial);
            Assert.Equal("3.600, 3.600", summerMaterial.Scale);
            Assert.Equal("0.200", summerMaterial.ElevationAmount);
            Assert.Equal("-0.200", summerMaterial.ParallaxAmount);
            Assert.Equal("2.700", summerMaterial.GroundSlopeScale);
            Assert.Equal("2", summerMaterial.BigDetailMapMode);
            Assert.Equal("0.500", summerMaterial.BigDetailMapWeight);
            Assert.Equal("0.001", summerMaterial.BigDetailMapScaleX);
            Assert.Equal("0.001", summerMaterial.BigDetailMapScaleY);
        }

        [Fact]
        public void TerrainMaterials_ShouldSerializeWithoutNullAttributes()
        {
            // Arrange - 创建一个只有必要属性的地形材质
            var terrainMaterials = new TerrainMaterialsDO();
            var material = new TerrainMaterialDO
            {
                Name = "test_material",
                PhysicsMaterial = "stone",
                // 故意不设置可选属性，它们应该保持null
                DetailLevelAdjustment = null,
                GroundSlopeScale = null,
                AlbedoFactorColor = null,
                AlbedoFactorMode = null,
                SmoothBlendAmount = null
            };
            terrainMaterials.TerrainMaterialList.Add(material);
            
            // Act - 序列化
            var serializer = new XmlSerializer(typeof(TerrainMaterialsDO));
            string serializedXml;
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings 
            { 
                Indent = true, 
                OmitXmlDeclaration = false
            }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(xmlWriter, terrainMaterials, ns);
                serializedXml = stringWriter.ToString();
            }
            
            // Assert - 验证null属性不会出现在XML中
            Assert.DoesNotContain("detail_level_adjustment=", serializedXml);
            Assert.DoesNotContain("ground_slope_scale=", serializedXml);
            Assert.DoesNotContain("albedo_factor_color=", serializedXml);
            Assert.DoesNotContain("albedo_factor_mode=", serializedXml);
            Assert.DoesNotContain("smooth_blend_amount=", serializedXml);
            
            // 验证必要属性存在
            Assert.Contains("name=\"test_material\"", serializedXml);
            Assert.Contains("physics_material=\"stone\"", serializedXml);
        }
    }
} 