using System.IO;
using System.Threading.Tasks;
using System.Linq;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.DO
{
    /// <summary>
    /// 特殊网格测试
    /// </summary>
    public class SpecialMeshesTests
    {
        [Fact]
        public async Task SpecialMeshes_XmlSerialization_ShouldBeRoundTripValid()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "special_meshes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act - Deserialize
            var originalDo = XmlTestUtils.Deserialize<SpecialMeshesDO>(xmlContent);
            
            // Convert to DTO and back to DO
            var dto = SpecialMeshesMapper.ToDTO(originalDo);
            var roundtripDo = SpecialMeshesMapper.ToDO(dto);
            
            // Act - Serialize
            string serializedXml = XmlTestUtils.Serialize(roundtripDo, xmlContent);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(roundtripDo);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlContent, serializedXml));
        }

        [Fact]
        public async Task SpecialMeshes_ShouldHaveCorrectStructure()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "special_meshes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var specialMeshes = XmlTestUtils.Deserialize<SpecialMeshesDO>(xmlContent);
            
            // Assert
            Assert.NotNull(specialMeshes);
            Assert.Equal("special_meshes", specialMeshes.Type);
            Assert.NotNull(specialMeshes.Meshes);
            Assert.NotEmpty(specialMeshes.Meshes.MeshList);
            
            // Verify mesh structure
            var firstMesh = specialMeshes.Meshes.MeshList.FirstOrDefault();
            Assert.NotNull(firstMesh);
            Assert.NotNull(firstMesh.Name);
            Assert.NotNull(firstMesh.Types);
            Assert.NotEmpty(firstMesh.Types.TypeList);
            
            // Verify type structure
            var firstType = firstMesh.Types.TypeList.FirstOrDefault();
            Assert.NotNull(firstType);
            Assert.Equal("outer_mesh", firstType.Name);
        }

        [Fact]
        public async Task SpecialMeshes_Mapper_ShouldWorkCorrectly()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "special_meshes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var originalDo = XmlTestUtils.Deserialize<SpecialMeshesDO>(xmlContent);
            var dto = SpecialMeshesMapper.ToDTO(originalDo);
            var convertedDo = SpecialMeshesMapper.ToDO(dto);
            
            // Assert
            Assert.NotNull(originalDo);
            Assert.NotNull(dto);
            Assert.NotNull(convertedDo);
            
            Assert.Equal(originalDo.Type, convertedDo.Type);
            Assert.Equal(originalDo.Meshes?.MeshList?.Count, convertedDo.Meshes?.MeshList?.Count);
            
            // Compare mesh structure
            if (originalDo.Meshes?.MeshList?.Count > 0)
            {
                var originalMesh = originalDo.Meshes.MeshList[0];
                var convertedMesh = convertedDo.Meshes.MeshList[0];
                
                Assert.Equal(originalMesh.Name, convertedMesh.Name);
                Assert.Equal(originalMesh.Types?.TypeList?.Count, convertedMesh.Types?.TypeList?.Count);
            }
        }

        [Fact]
        public async Task SpecialMeshes_ShouldContainExpectedMeshes()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "special_meshes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var specialMeshes = XmlTestUtils.Deserialize<SpecialMeshesDO>(xmlContent);
            
            // Assert
            Assert.NotNull(specialMeshes);
            
            // Expected mesh names (excluding commented ones)
            var expectedMeshNames = new[]
            {
                "outer_mesh_forest",
                "main_map_outer"
            };
            
            foreach (var expectedName in expectedMeshNames)
            {
                var mesh = specialMeshes.Meshes.MeshList.FirstOrDefault(m => m.Name == expectedName);
                Assert.NotNull(mesh);
            }
        }

        [Fact]
        public async Task SpecialMeshes_TypesShouldHaveCorrectStructure()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "special_meshes.xml");
            string xmlContent = await File.ReadAllTextAsync(xmlPath);
            
            // Act
            var specialMeshes = XmlTestUtils.Deserialize<SpecialMeshesDO>(xmlContent);
            
            // Assert
            Assert.NotNull(specialMeshes);
            
            // All meshes should have types
            foreach (var mesh in specialMeshes.Meshes.MeshList)
            {
                Assert.NotNull(mesh.Types);
                Assert.NotEmpty(mesh.Types.TypeList);
                
                // All types should have names
                foreach (var type in mesh.Types.TypeList)
                {
                    Assert.NotNull(type.Name);
                    Assert.NotEmpty(type.Name);
                }
            }
        }
    }
}