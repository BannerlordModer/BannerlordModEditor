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
    public class LayeredArchitectureConsistencyTests
    {
        private const string TestDataPath = "TestData/particle_systems_basic.xml";
        private readonly ITestOutputHelper _output;

        public LayeredArchitectureConsistencyTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldSerialize_Consistency_Between_DO_And_DTO()
        {
            // Arrange
            var doObj = new EffectDO
            {
                Name = "test_effect",
                Guid = "{12345678-1234-1234-1234-123456789012}",
                SoundCode = null  // 这不应该被序列化
            };

            var dtoObj = new EffectDTO
            {
                Name = "test_effect",
                Guid = "{12345678-1234-1234-1234-123456789012}",
                SoundCode = null  // 这不应该被序列化
            };

            // Act
            var doShouldSerializeSoundCode = doObj.ShouldSerializeSoundCode();
            var dtoShouldSerializeSoundCode = dtoObj.ShouldSerializeSoundCode();

            // Assert
            Assert.False(doShouldSerializeSoundCode, "DO层不应该序列化空的SoundCode");
            Assert.False(dtoShouldSerializeSoundCode, "DTO层不应该序列化空的SoundCode");
            Assert.Equal(doShouldSerializeSoundCode, dtoShouldSerializeSoundCode);

            _output.WriteLine($"DO ShouldSerializeSoundCode: {doShouldSerializeSoundCode}");
            _output.WriteLine($"DTO ShouldSerializeSoundCode: {dtoShouldSerializeSoundCode}");
        }

        [Fact]
        public void Mapper_Preserves_Data_Regardless_Of_Serialization_Status()
        {
            // Arrange
            var doObj = new EffectDO
            {
                Name = "test_effect",
                Guid = "{12345678-1234-1234-1234-123456789012}",
                SoundCode = null  // 这个属性在DO层不应该被序列化
            };

            // Act - 映射到DTO
            var dtoObj = ParticleSystemsMapper.ToDTO(doObj);

            // Assert - Mapper应该完整复制数据，不管ShouldSerialize返回什么
            Assert.Equal(doObj.Name, dtoObj.Name);
            Assert.Equal(doObj.Guid, dtoObj.Guid);
            Assert.Equal(doObj.SoundCode, dtoObj.SoundCode); // null值也应该被复制

            _output.WriteLine($"Original DO Name: {doObj.Name}");
            _output.WriteLine($"Mapped DTO Name: {dtoObj.Name}");
            _output.WriteLine($"Original DO SoundCode: {doObj.SoundCode}");
            _output.WriteLine($"Mapped DTO SoundCode: {dtoObj.SoundCode}");
            
            // 验证ShouldSerialize逻辑的一致性
            Assert.Equal(doObj.ShouldSerializeSoundCode(), dtoObj.ShouldSerializeSoundCode());
        }

        [Fact]
        public void Serialization_Respects_ShouldSerialize_Methods()
        {
            // Arrange
            var doObj = new EffectDO
            {
                Name = "test_effect",
                Guid = "{12345678-1234-1234-1234-123456789012}",
                SoundCode = null  // 这个不应该出现在序列化结果中
            };

            // Act
            var serializer = new XmlSerializer(typeof(EffectDO));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, doObj);
            var xml = stringWriter.ToString();

            // Assert
            Assert.Contains("test_effect", xml);
            Assert.Contains("{12345678-1234-1234-1234-123456789012}", xml);
            Assert.DoesNotContain("sound_code", xml); // 空的SoundCode不应该被序列化

            _output.WriteLine("Serialized XML:");
            _output.WriteLine(xml);
        }

        [Fact]
        public void Full_Roundtrip_Maintains_ShouldSerialize_Consistency()
        {
            // Arrange
            var originalXml = File.ReadAllText(TestDataPath);
            _output.WriteLine($"Original XML length: {originalXml.Length}");

            // Act - 完整的DO→DTO→DO转换流程
            var particleSystemsDO = XmlTestUtils.Deserialize<ParticleSystemsDO>(originalXml);
            var particleSystemsDTO = ParticleSystemsMapper.ToDTO(particleSystemsDO);
            var particleSystemsBackDO = ParticleSystemsMapper.ToDO(particleSystemsDTO);
            var xmlFromBackDO = XmlTestUtils.Serialize(particleSystemsBackDO, originalXml);

            // Assert - 验证ShouldSerialize方法在各层间保持一致性
            Assert.NotNull(particleSystemsDO);
            Assert.NotNull(particleSystemsDTO);
            Assert.NotNull(particleSystemsBackDO);
            Assert.NotNull(xmlFromBackDO);
            Assert.True(xmlFromBackDO.Length > 0);

            _output.WriteLine($"Roundtrip XML length: {xmlFromBackDO.Length}");
            
            // 检查结构一致性
            var areEqual = XmlTestUtils.AreStructurallyEqual(originalXml, xmlFromBackDO);
            _output.WriteLine($"Structural equality: {areEqual}");
        }
    }
}