using Xunit;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class GpuParticleSystemsTests
{
    [Fact]
    public void TestXmlSerialization()
    {
        var dto = new GpuParticleSystemsDTO
        {
            Type = "particle_system",
            ParticleSystems = new ParticleSystemsContainerDTO
            {
                Items = new List<ParticleSystemDTO>
                {
                    new ParticleSystemDTO
                    {
                        Name = "test",
                        Material = "gpuparticle_standard",
                        Life = "1.000000",
                        EmissionRate = "500.000000",
                        EmitVelocity = "0.000000, 0.000000, 6.000000",
                        EmitDirectionRandomness = "1.000000, 1.000000, 1.000000",
                        EmitVelocityRandomness = "0.000000",
                        EmitSphereRadius = "0.050000",
                        ScaleT0 = "0.000000",
                        ScaleT1 = "1.000000",
                        StartScale = "1.000000",
                        EndScale = "1.500000",
                        AlphaT0 = "0.000000",
                        AlphaT1 = "0.400000",
                        LinearDamping = "0.500000",
                        GravityConstant = "0.800000",
                        AngularSpeed = "0.500000",
                        AngularDamping = "0.500000",
                        BurstLength = "0.500000",
                        IsBurst = "false"
                    }
                }
            }
        };

        var xml = XmlTestUtils.Serialize(dto);
        var deserialized = XmlTestUtils.Deserialize<GpuParticleSystemsDTO>(xml);

        Assert.NotNull(deserialized);
        Assert.Equal(dto.Type, deserialized.Type);
        Assert.Equal(dto.ParticleSystems.Items.Count, deserialized.ParticleSystems.Items.Count);
        Assert.Equal("test", deserialized.ParticleSystems.Items[0].Name);
        Assert.Equal("gpuparticle_standard", deserialized.ParticleSystems.Items[0].Material);
    }

    [Fact]
    public void TestRoundTripWithTestData()
    {
        var testDataPath = Path.Combine("TestData", "gpu_particle_systems.xml");
        if (File.Exists(testDataPath))
        {
            var xml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<GpuParticleSystemsDTO>(xml);
            var serialized = XmlTestUtils.Serialize(dto, xml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}