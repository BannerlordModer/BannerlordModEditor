using Xunit;
using BannerlordModEditor.Common.Tests;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsQuickTest
    {
        [Fact]
        public void TestParticleSystemsBasicFunctionality()
        {
            // 简化的测试，验证基本的序列化/反序列化功能
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test_effect"" guid=""12345"">
        <emitters>
            <emitter name=""test_emitter"">
                <parameters>
                    <parameter name=""test_param"" value=""1.0"" />
                    <decal_materials>
                        <decal_material value=""test_material"" />
                    </decal_materials>
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            try
            {
                // 测试反序列化
                var particleSystems = XmlTestUtils.Deserialize<ParticleSystemsDO>(testXml);
                Assert.NotNull(particleSystems);
                Assert.NotNull(particleSystems.Effects);
                Assert.Single(particleSystems.Effects);
                
                var effect = particleSystems.Effects[0];
                Assert.Equal("test_effect", effect.Name);
                Assert.NotNull(effect.Emitters);
                Assert.Single(effect.Emitters.EmitterList);
                
                var emitter = effect.Emitters.EmitterList[0];
                Assert.Equal("test_emitter", emitter.Name);
                Assert.NotNull(emitter.Parameters);
                Assert.Single(emitter.Parameters.ParameterList);
                Assert.NotNull(emitter.Parameters.DecalMaterials);
                Assert.Single(emitter.Parameters.DecalMaterials.DecalMaterialList);
                
                // 测试序列化
                var serialized = XmlTestUtils.Serialize(particleSystems, testXml);
                Assert.NotNull(serialized);
                
                // 验证结构一致性
                var isStructurallyEqual = XmlTestUtils.AreStructurallyEqual(testXml, serialized);
                Assert.True(isStructurallyEqual, "XML结构应该一致");
                
                Console.WriteLine("✅ ParticleSystems XML序列化测试通过");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 测试失败: {ex.Message}");
                Assert.Fail($"测试失败: {ex.Message}");
            }
        }
    }
}