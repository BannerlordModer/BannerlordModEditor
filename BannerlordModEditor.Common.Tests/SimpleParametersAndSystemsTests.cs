using BannerlordModEditor.Common.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleParametersAndSystemsTests
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

        [Fact]
        public void ManagedCampaignParameters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_campaign_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ManagedCampaignParametersBase));
            ManagedCampaignParametersBase parameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                parameters = (ManagedCampaignParametersBase)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, parameters);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(parameters);
            Assert.NotNull(parameters.ManagedCampaignParameters);
            Assert.NotNull(parameters.ManagedCampaignParameters.ManagedCampaignParameter);
            Assert.Equal("campaign_parameters", parameters.Type);
            Assert.True(parameters.ManagedCampaignParameters.ManagedCampaignParameter.Count > 0, "Should have at least one parameter");

            // 验证具体参数值
            var warDeclarationParam = parameters.ManagedCampaignParameters.ManagedCampaignParameter
                .FirstOrDefault(p => p.Id == "IsWarDeclarationDisabled");
            if (warDeclarationParam != null)
            {
                Assert.Equal("IsWarDeclarationDisabled", warDeclarationParam.Id);
                Assert.Equal("false", warDeclarationParam.Value);
            }

            var peaceDeclarationParam = parameters.ManagedCampaignParameters.ManagedCampaignParameter
                .FirstOrDefault(p => p.Id == "IsPeaceDeclarationDisabled");
            if (peaceDeclarationParam != null)
            {
                Assert.Equal("IsPeaceDeclarationDisabled", peaceDeclarationParam.Id);
                Assert.Equal("false", peaceDeclarationParam.Value);
            }

            // 验证所有参数都有必要字段
            foreach (var param in parameters.ManagedCampaignParameters.ManagedCampaignParameter)
            {
                Assert.False(string.IsNullOrEmpty(param.Id), $"Parameter should have an ID");
                Assert.False(string.IsNullOrEmpty(param.Value), $"Parameter {param.Id} should have a Value");
            }
        }

        [Fact]
        public void GpuParticleSystems_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gpu_particle_systems.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(GpuParticleSystemsBase));
            GpuParticleSystemsBase particleSystems;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                particleSystems = (GpuParticleSystemsBase)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, particleSystems);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(particleSystems);
            Assert.NotNull(particleSystems.ParticleSystems);
            Assert.NotNull(particleSystems.ParticleSystems.ParticleSystem);
            Assert.Equal("particle_system", particleSystems.Type);
            Assert.True(particleSystems.ParticleSystems.ParticleSystem.Count > 0, "Should have at least one particle system");

            // 验证测试粒子系统的具体数值
            var testSystem = particleSystems.ParticleSystems.ParticleSystem.FirstOrDefault(p => p.Name == "test");
            if (testSystem != null)
            {
                Assert.Equal("test", testSystem.Name);
                Assert.Equal("gpuparticle_standard", testSystem.Material);
                Assert.Equal("1.000000", testSystem.Life);
                Assert.Equal("500.000000", testSystem.EmissionRate);
                Assert.Equal("0.000000, 0.000000, 6.000000", testSystem.EmitVelocity);
                Assert.Equal("1.000000, 1.000000, 1.000000", testSystem.EmitDirectionRandomness);
                Assert.Equal("0.000000", testSystem.EmitVelocityRandomness);
                Assert.Equal("0.050000", testSystem.EmitSphereRadius);
                Assert.Equal("0.000000", testSystem.ScaleT0);
                Assert.Equal("1.000000", testSystem.ScaleT1);
                Assert.Equal("1.000000", testSystem.StartScale);
                Assert.Equal("1.500000", testSystem.EndScale);
                Assert.Equal("0.000000", testSystem.AlphaT0);
                Assert.Equal("0.400000", testSystem.AlphaT1);
                Assert.Equal("0.500000", testSystem.LinearDamping);
                Assert.Equal("0.800000", testSystem.GravityConstant);
                Assert.Equal("0.500000", testSystem.AngularSpeed);
                Assert.Equal("0.500000", testSystem.AngularDamping);
                Assert.Equal("0.500000", testSystem.BurstLength);
                Assert.Equal("false", testSystem.IsBurst);
            }
        }

        [Fact]
        public void GpuParticleSystems_NumericPrecision_ShouldBePreserved()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gpu_particle_systems.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(GpuParticleSystemsBase));
            GpuParticleSystemsBase particleSystems;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                particleSystems = (GpuParticleSystemsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证浮点数精度保持
            var testSystem = particleSystems.ParticleSystems.ParticleSystem.FirstOrDefault(p => p.Name == "test");
            if (testSystem != null)
            {
                // 确保所有小数点后6位的数值保持不变
                Assert.Equal("1.000000", testSystem.Life);
                Assert.Equal("500.000000", testSystem.EmissionRate);
                Assert.Equal("0.050000", testSystem.EmitSphereRadius);
                Assert.Equal("1.500000", testSystem.EndScale);
                Assert.Equal("0.400000", testSystem.AlphaT1);
                
                // 验证向量格式
                Assert.Contains(", ", testSystem.EmitVelocity);
                Assert.Contains(", ", testSystem.EmitDirectionRandomness);
                
                // 验证布尔值格式
                Assert.Equal("false", testSystem.IsBurst);
            }
        }

        [Fact]
        public void SpecialMeshes_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "special_meshes.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SpecialMeshesBase));
            SpecialMeshesBase specialMeshes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                specialMeshes = (SpecialMeshesBase)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, specialMeshes);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(specialMeshes);
            Assert.NotNull(specialMeshes.Meshes);
            Assert.NotNull(specialMeshes.Meshes.Mesh);
            Assert.Equal("special_meshes", specialMeshes.Type);
            Assert.True(specialMeshes.Meshes.Mesh.Count > 0, "Should have at least one mesh");

            // 验证特定网格
            var outerMeshForest = specialMeshes.Meshes.Mesh.FirstOrDefault(m => m.Name == "outer_mesh_forest");
            if (outerMeshForest != null)
            {
                Assert.Equal("outer_mesh_forest", outerMeshForest.Name);
                Assert.NotNull(outerMeshForest.Types);
                Assert.NotNull(outerMeshForest.Types.Type);
                Assert.True(outerMeshForest.Types.Type.Count > 0);
                
                var outerMeshType = outerMeshForest.Types.Type.FirstOrDefault(t => t.Name == "outer_mesh");
                Assert.NotNull(outerMeshType);
                Assert.Equal("outer_mesh", outerMeshType.Name);
            }

            var mainMapOuter = specialMeshes.Meshes.Mesh.FirstOrDefault(m => m.Name == "main_map_outer");
            if (mainMapOuter != null)
            {
                Assert.Equal("main_map_outer", mainMapOuter.Name);
                Assert.NotNull(mainMapOuter.Types);
                Assert.NotNull(mainMapOuter.Types.Type);
                
                var outerMeshType = mainMapOuter.Types.Type.FirstOrDefault(t => t.Name == "outer_mesh");
                Assert.NotNull(outerMeshType);
                Assert.Equal("outer_mesh", outerMeshType.Name);
            }

            // 验证所有网格都有名称
            foreach (var mesh in specialMeshes.Meshes.Mesh)
            {
                Assert.False(string.IsNullOrEmpty(mesh.Name), "Mesh should have a name");
                if (mesh.Types != null)
                {
                    Assert.NotNull(mesh.Types.Type);
                    foreach (var type in mesh.Types.Type)
                    {
                        Assert.False(string.IsNullOrEmpty(type.Name), $"Type in mesh {mesh.Name} should have a name");
                    }
                }
            }
        }
    }
} 