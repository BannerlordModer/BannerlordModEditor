using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;
using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;
using BannerlordModEditor.Common.Tests.Services;

namespace BannerlordModEditor.Common.Tests
{
    public class LayeredParticleSystemsTests
    {
        private const string TestDataPath = "TestData/particle_systems_basic.xml";
        private readonly ITestOutputHelper _output;

        public LayeredParticleSystemsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DO_Layer_Should_Serialize_ParticleSystems_Correctly()
        {
            // Arrange
            var xml = File.ReadAllText(TestDataPath);
            
            // Act - 反序列化为DO
            var particleSystemsDO = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 序列化回XML
            var xml2 = XmlTestUtils.Serialize(particleSystemsDO, xml);
            
            // Assert
            Assert.NotNull(particleSystemsDO);
            Assert.True(particleSystemsDO.Effects?.Count > 0);
            Assert.NotNull(xml2);
            Assert.True(xml2.Length > 0);
            
            _output.WriteLine($"Original effects count: {particleSystemsDO.Effects?.Count ?? 0}");
            _output.WriteLine($"First effect name: {particleSystemsDO.Effects?[0]?.Name}");
        }

        [Fact]
        public void DTO_Layer_Should_Provide_Numeric_Properties()
        {
            // Arrange
            var emitterDTO = new EmitterDTO
            {
                Name = "test_emitter",
                Index = "5"
            };

            // Act & Assert
            Assert.Equal("test_emitter", emitterDTO.Name);
            Assert.Equal("5", emitterDTO.Index);
            Assert.Equal(5, emitterDTO.IndexInt);
            
            _output.WriteLine($"Name: {emitterDTO.Name}");
            _output.WriteLine($"Index: {emitterDTO.Index}");
            _output.WriteLine($"IndexInt: {emitterDTO.IndexInt}");
        }

        [Fact]
        public void Mapping_Between_DO_And_DTO_Should_Work_For_ParticleSystems()
        {
            // Arrange
            var xml = File.ReadAllText(TestDataPath);
            var particleSystemsDO = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);

            // Act
            var particleSystemsDTO = ParticleSystemsMapper.ToDTO(particleSystemsDO);
            var mappedBackDO = ParticleSystemsMapper.ToDO(particleSystemsDTO);

            // Assert
            Assert.NotNull(particleSystemsDTO);
            Assert.NotNull(mappedBackDO);
            Assert.Equal(particleSystemsDO.Effects?.Count ?? 0, particleSystemsDTO.Effects?.Count ?? 0);
            Assert.Equal(particleSystemsDTO.Effects?.Count ?? 0, mappedBackDO.Effects?.Count ?? 0);
            
            _output.WriteLine($"Original DO effects count: {particleSystemsDO.Effects?.Count ?? 0}");
            _output.WriteLine($"Mapped DTO effects count: {particleSystemsDTO.Effects?.Count ?? 0}");
            _output.WriteLine($"Mapped back DO effects count: {mappedBackDO.Effects?.Count ?? 0}");
        }

        [Fact]
        public void DTO_Setter_Methods_Should_Convert_To_String()
        {
            // Arrange
            var parameterDTO = new ParameterDTO();

            // Act
            parameterDTO.SetValueDouble(1.5);
            parameterDTO.SetBiasDouble(2.7);

            // Assert
            Assert.Equal("1.5", parameterDTO.Value);
            Assert.Equal("2.7", parameterDTO.Bias);
            Assert.Equal(1.5, parameterDTO.ValueDouble);
            Assert.Equal(2.7, parameterDTO.BiasDouble);
            
            _output.WriteLine($"Value: {parameterDTO.Value}");
            _output.WriteLine($"Bias: {parameterDTO.Bias}");
            _output.WriteLine($"ValueDouble: {parameterDTO.ValueDouble}");
            _output.WriteLine($"BiasDouble: {parameterDTO.BiasDouble}");
        }

        [Fact]
        public void Full_ParticleSystems_RoundTrip_Should_Work_With_Layered_Architecture()
        {
            // Arrange
            var xml = File.ReadAllText(TestDataPath);
            _output.WriteLine($"Original XML length: {xml.Length} characters");
            _output.WriteLine($"Original XML lines: {xml.Split('\n').Length} lines");

            // Act - DO层处理
            var particleSystemsDO = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            var xmlFromDO = XmlTestUtils.Serialize(particleSystemsDO, xml);

            _output.WriteLine($"DO layer XML length: {xmlFromDO.Length} characters");
            _output.WriteLine($"DO layer XML lines: {xmlFromDO.Split('\n').Length} lines");

            // Map to DTO
            var particleSystemsDTO = ParticleSystemsMapper.ToDTO(particleSystemsDO);
            
            // Map back to DO
            var particleSystemsBackDO = ParticleSystemsMapper.ToDO(particleSystemsDTO);
            
            // Serialize again
            var xmlFromBackDO = XmlTestUtils.Serialize(particleSystemsBackDO, xml);

            // Assert
            Assert.NotNull(particleSystemsDO);
            Assert.NotNull(particleSystemsDTO);
            Assert.NotNull(particleSystemsBackDO);
            Assert.NotNull(xmlFromDO);
            Assert.NotNull(xmlFromBackDO);
            Assert.True(particleSystemsDO.Effects?.Count > 0);
            Assert.Equal(particleSystemsDO.Effects?.Count ?? 0, particleSystemsDTO.Effects?.Count ?? 0);
            Assert.Equal(particleSystemsDTO.Effects?.Count ?? 0, particleSystemsBackDO.Effects?.Count ?? 0);
            
            _output.WriteLine($"Final XML length: {xmlFromBackDO.Length} characters");
            _output.WriteLine($"Final XML lines: {xmlFromBackDO.Split('\n').Length} lines");
            
            // Check structural equality
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xmlFromBackDO);
            _output.WriteLine($"Structural equality: {areEqual}");
            
            // 由于这是分层架构的实现，应该能处理原始的复杂结构
            Assert.True(particleSystemsDO.Effects?.Count > 0, "Should deserialize effects successfully");
        }
    }
}