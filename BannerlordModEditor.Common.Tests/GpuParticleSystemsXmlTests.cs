using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Engine;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class GpuParticleSystemsXmlTests
    {
        [Fact]
        public void GpuParticleSystems_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
	type=""particle_system"">
	<particle_systems>
		<particle_system
			name=""test""
			material=""gpuparticle_standard""
			life=""1.000000""
			emission_rate=""500.000000""
			emit_velocity=""0.000000, 0.000000, 6.000000""
			emit_direction_randomness=""1.000000, 1.000000, 1.000000""
			emit_velocity_randomness=""0.000000""
			emit_sphere_radius=""0.050000""
			scale_t0=""0.000000""
			scale_t1=""1.000000""
			start_scale=""1.000000""
			end_scale=""1.500000""
			alpha_t0=""0.000000""
			alpha_t1=""0.400000""
			linear_damping=""0.500000""
			gravity_constant=""0.800000""
			angular_speed=""0.500000""
			angular_damping=""0.500000""
			burst_length=""0.500000""
			is_burst=""false"" />
	</particle_systems>
</base>";

            var serializer = new XmlSerializer(typeof(GpuParticleSystemsRoot));

            // Act
            GpuParticleSystemsRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (GpuParticleSystemsRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.NotNull(result.ParticleSystems);
            Assert.NotNull(result.ParticleSystems.ParticleSystem);
            Assert.Single(result.ParticleSystems.ParticleSystem);

            var particleSystem = result.ParticleSystems.ParticleSystem[0];
            Assert.Equal("test", particleSystem.Name);
            Assert.Equal("gpuparticle_standard", particleSystem.Material);
            Assert.Equal("1.000000", particleSystem.Life);
            Assert.Equal("500.000000", particleSystem.EmissionRate);
            Assert.Equal("0.000000, 0.000000, 6.000000", particleSystem.EmitVelocity);
            Assert.Equal("1.000000, 1.000000, 1.000000", particleSystem.EmitDirectionRandomness);
            Assert.Equal("0.000000", particleSystem.EmitVelocityRandomness);
            Assert.Equal("0.050000", particleSystem.EmitSphereRadius);
            Assert.Equal("0.000000", particleSystem.ScaleT0);
            Assert.Equal("1.000000", particleSystem.ScaleT1);
            Assert.Equal("1.000000", particleSystem.StartScale);
            Assert.Equal("1.500000", particleSystem.EndScale);
            Assert.Equal("0.000000", particleSystem.AlphaT0);
            Assert.Equal("0.400000", particleSystem.AlphaT1);
            Assert.Equal("0.500000", particleSystem.LinearDamping);
            Assert.Equal("0.800000", particleSystem.GravityConstant);
            Assert.Equal("0.500000", particleSystem.AngularSpeed);
            Assert.Equal("0.500000", particleSystem.AngularDamping);
            Assert.Equal("0.500000", particleSystem.BurstLength);
            Assert.Equal("false", particleSystem.IsBurst);
        }

        [Fact]
        public void GpuParticleSystems_WithMultipleSystems_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_system"">
	<particle_systems>
		<particle_system
			name=""fire_effect""
			material=""gpuparticle_fire""
			life=""2.000000""
			emission_rate=""300.000000""
			emit_velocity=""0.000000, 1.000000, 0.000000""
			is_burst=""false"" />
		<particle_system
			name=""smoke_effect""
			material=""gpuparticle_smoke""
			life=""3.000000""
			emission_rate=""100.000000""
			emit_velocity=""0.000000, 2.000000, 0.000000""
			is_burst=""true"" />
	</particle_systems>
</base>";

            var serializer = new XmlSerializer(typeof(GpuParticleSystemsRoot));

            // Act
            GpuParticleSystemsRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (GpuParticleSystemsRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.NotNull(result.ParticleSystems);
            Assert.NotNull(result.ParticleSystems.ParticleSystem);
            Assert.Equal(2, result.ParticleSystems.ParticleSystem.Length);

            var fireEffect = result.ParticleSystems.ParticleSystem[0];
            Assert.Equal("fire_effect", fireEffect.Name);
            Assert.Equal("gpuparticle_fire", fireEffect.Material);
            Assert.Equal("2.000000", fireEffect.Life);
            Assert.Equal("300.000000", fireEffect.EmissionRate);
            Assert.Equal("0.000000, 1.000000, 0.000000", fireEffect.EmitVelocity);
            Assert.Equal("false", fireEffect.IsBurst);

            var smokeEffect = result.ParticleSystems.ParticleSystem[1];
            Assert.Equal("smoke_effect", smokeEffect.Name);
            Assert.Equal("gpuparticle_smoke", smokeEffect.Material);
            Assert.Equal("3.000000", smokeEffect.Life);
            Assert.Equal("100.000000", smokeEffect.EmissionRate);
            Assert.Equal("0.000000, 2.000000, 0.000000", smokeEffect.EmitVelocity);
            Assert.Equal("true", smokeEffect.IsBurst);
        }

        [Fact]
        public void GpuParticleSystems_CanSerializeToXml()
        {
            // Arrange
            var gpuParticleSystemsRoot = new GpuParticleSystemsRoot
            {
                Type = "particle_system",
                ParticleSystems = new GpuParticleSystems
                {
                    ParticleSystem = new[]
                    {
                        new GpuParticleSystem
                        {
                            Name = "test_system",
                            Material = "test_material",
                            Life = "1.500000",
                            EmissionRate = "200.000000",
                            EmitVelocity = "1.000000, 0.000000, 0.000000",
                            EmitDirectionRandomness = "0.500000, 0.500000, 0.500000",
                            EmitVelocityRandomness = "0.100000",
                            EmitSphereRadius = "0.075000",
                            StartScale = "0.800000",
                            EndScale = "1.200000",
                            LinearDamping = "0.300000",
                            GravityConstant = "0.900000",
                            IsBurst = "true"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(GpuParticleSystemsRoot));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, gpuParticleSystemsRoot);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("type=\"particle_system\"", result);
            Assert.Contains("name=\"test_system\"", result);
            Assert.Contains("material=\"test_material\"", result);
            Assert.Contains("life=\"1.500000\"", result);
            Assert.Contains("emission_rate=\"200.000000\"", result);
            Assert.Contains("emit_velocity=\"1.000000, 0.000000, 0.000000\"", result);
            Assert.Contains("emit_direction_randomness=\"0.500000, 0.500000, 0.500000\"", result);
            Assert.Contains("emit_velocity_randomness=\"0.100000\"", result);
            Assert.Contains("emit_sphere_radius=\"0.075000\"", result);
            Assert.Contains("start_scale=\"0.800000\"", result);
            Assert.Contains("end_scale=\"1.200000\"", result);
            Assert.Contains("linear_damping=\"0.300000\"", result);
            Assert.Contains("gravity_constant=\"0.900000\"", result);
            Assert.Contains("is_burst=\"true\"", result);
        }

        [Fact]
        public void GpuParticleSystems_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new GpuParticleSystemsRoot
            {
                Type = "particle_system",
                ParticleSystems = new GpuParticleSystems
                {
                    ParticleSystem = new[]
                    {
                        new GpuParticleSystem
                        {
                            Name = "roundtrip_test",
                            Material = "roundtrip_material",
                            Life = "2.500000",
                            EmissionRate = "150.000000",
                            EmitVelocity = "0.000000, 0.000000, 1.000000",
                            IsBurst = "false"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(GpuParticleSystemsRoot));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            GpuParticleSystemsRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (GpuParticleSystemsRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.NotNull(result.ParticleSystems);
            Assert.NotNull(result.ParticleSystems.ParticleSystem);
            Assert.Single(result.ParticleSystems.ParticleSystem);

            var system = result.ParticleSystems.ParticleSystem[0];
            Assert.Equal("roundtrip_test", system.Name);
            Assert.Equal("roundtrip_material", system.Material);
            Assert.Equal("2.500000", system.Life);
            Assert.Equal("150.000000", system.EmissionRate);
            Assert.Equal("0.000000, 0.000000, 1.000000", system.EmitVelocity);
            Assert.Equal("false", system.IsBurst);
        }

        [Fact]
        public void GpuParticleSystems_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_system"">
	<particle_systems>
	</particle_systems>
</base>";

            var serializer = new XmlSerializer(typeof(GpuParticleSystemsRoot));

            // Act
            GpuParticleSystemsRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (GpuParticleSystemsRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.NotNull(result.ParticleSystems);
        }
    }
} 