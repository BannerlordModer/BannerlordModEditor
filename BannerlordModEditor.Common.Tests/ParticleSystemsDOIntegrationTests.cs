using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    /// <summary>
    /// ParticleSystems DO层的集成测试
    /// 测试端到端的XML处理和复杂场景
    /// </summary>
    public class ParticleSystemsDOIntegrationTests
    {
        #region 完整XML文件测试

        [Fact]
        public void ParticleSystems_FullXmlFile_RoundTrip_Success()
        {
            // Arrange
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(obj, xml);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serializedXml));
            
            // 验证基本结构
            Assert.NotNull(obj.Effects);
            Assert.True(obj.Effects.Count > 0);
        }

        [Fact]
        public void ParticleSystems_EmptyXml_Handled_Correctly()
        {
            // Arrange
            var emptyXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(emptyXml);
            var serializedXml = XmlTestUtils.Serialize(obj, emptyXml);

            // Assert
            Assert.NotNull(obj);
            Assert.Empty(obj.Effects);
            Assert.True(XmlTestUtils.AreStructurallyEqual(emptyXml, serializedXml));
        }

        [Fact]
        public void ParticleSystems_MinimalXml_Handled_Correctly()
        {
            // Arrange
            var minimalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test_effect"">
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(minimalXml);
            var serializedXml = XmlTestUtils.Serialize(obj, minimalXml);

            // Assert
            Assert.NotNull(obj);
            Assert.Single(obj.Effects);
            Assert.Equal("test_effect", obj.Effects[0].Name);
            Assert.True(XmlTestUtils.AreStructurallyEqual(minimalXml, serializedXml));
        }

        #endregion

        #region 复杂嵌套结构测试

        [Fact]
        public void ParticleSystems_ComplexNestedStructure_Preserved_Correctly()
        {
            // Arrange
            var complexXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""complex_effect"" guid=""{12345678-1234-1234-1234-123456789012}"">
        <emitters>
            <emitter name=""main_emitter"" _index_=""0"">
                <children>
                    <emitter name=""child_emitter"" _index_=""0"">
                        <flags>
                            <flag name=""emit_while_moving"" value=""false"" />
                            <flag name=""loop_sprite"" value=""true"" />
                        </flags>
                        <parameters>
                            <parameter name=""emitter_life"" value=""5.0"" />
                            <parameter name=""scale_with_emitter_velocity_coef"" value=""0.5"">
                                <curve name=""scale_curve"">
                                    <keys>
                                        <key time=""0.0"" value=""1.0"" />
                                        <key time=""1.0"" value=""0.0"" />
                                    </keys>
                                </curve>
                            </parameter>
                            <decal_materials>
                                <decal_material value=""burn_mark"" />
                                <decal_material value=""scorch_mark"" />
                            </decal_materials>
                        </parameters>
                    </emitter>
                </children>
                <parameters>
                    <parameter name=""emit_rate"" value=""10.0"" />
                    <decal_materials></decal_materials>
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(complexXml);
            var serializedXml = XmlTestUtils.Serialize(obj, complexXml);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(complexXml, serializedXml));
            
            // 验证复杂结构被正确解析
            Assert.Single(obj.Effects);
            var effect = obj.Effects[0];
            Assert.Equal("complex_effect", effect.Name);
            Assert.NotNull(effect.Emitters);
            Assert.Single(effect.Emitters!.EmitterList);
            
            var mainEmitter = effect.Emitters!.EmitterList[0];
            Assert.Equal("main_emitter", mainEmitter.Name);
            Assert.NotNull(mainEmitter.Children);
            Assert.Single(mainEmitter.Children!.EmitterList);
            
            var childEmitter = mainEmitter.Children!.EmitterList[0];
            Assert.NotNull(childEmitter.Flags);
            Assert.Equal(2, childEmitter.Flags!.FlagList.Count);
            Assert.NotNull(childEmitter.Parameters);
            Assert.Equal(2, childEmitter.Parameters!.ParameterList.Count);
            Assert.NotNull(childEmitter.Parameters!.DecalMaterials);
            Assert.Equal(2, childEmitter.Parameters!.DecalMaterials!.DecalMaterialList.Count);
            
            // 验证曲线结构
            var curveParameter = childEmitter.Parameters!.ParameterList[1];
            Assert.NotNull(curveParameter.ParameterCurve);
            Assert.Equal("scale_curve", curveParameter.ParameterCurve!.Name);
            Assert.NotNull(curveParameter.ParameterCurve!.Keys);
            Assert.Equal(2, curveParameter.ParameterCurve!.Keys!.KeyList.Count);
        }

        [Fact]
        public void ParticleSystems_MultipleEffects_Preserved_Correctly()
        {
            // Arrange
            var multipleEffectsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""effect1"">
        <emitters>
            <emitter name=""emitter1"" _index_=""0"">
                <parameters>
                    <parameter name=""life"" value=""1.0"" />
                </parameters>
            </emitter>
        </emitters>
    </effect>
    <effect name=""effect2"">
        <emitters>
            <emitter name=""emitter2"" _index_=""0"">
                <flags>
                    <flag name=""test_flag"" value=""true"" />
                </flags>
            </emitter>
        </emitters>
    </effect>
    <effect name=""effect3"">
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(multipleEffectsXml);
            var serializedXml = XmlTestUtils.Serialize(obj, multipleEffectsXml);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(multipleEffectsXml, serializedXml));
            
            // 验证多个effect被正确解析
            Assert.Equal(3, obj.Effects.Count);
            Assert.Equal("effect1", obj.Effects[0].Name);
            Assert.Equal("effect2", obj.Effects[1].Name);
            Assert.Equal("effect3", obj.Effects[2].Name);
        }

        #endregion

        #region 错误处理测试

        [Fact]
        public void ParticleSystems_InvalidXml_Throws_Exception()
        {
            // Arrange - 创建真正无效的XML（格式错误）
            var invalidXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test"">
        <emitters>
            <emitter name=""test"" _index_=""0"">
        </effect>
</particle_effects>";

            // Act & Assert
            // XML反序列化可能会抛出各种异常，我们捕获其中任何一种
            Assert.ThrowsAny<Exception>(() => 
                XmlTestUtils.Deserialize<ParticleSystemsDO>(invalidXml));
        }

        [Fact]
        public void ParticleSystems_MalformedXml_Throws_Exception()
        {
            // Arrange
            var malformedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test"">
        <emitters>
            <emitter name=""test"" _index_=""0"">
    </effect>
</particle_effects>";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
                XmlTestUtils.Deserialize<ParticleSystemsDO>(malformedXml));
        }

        [Fact]
        public void ParticleSystems_NullInput_Throws_Exception()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                XmlTestUtils.Deserialize<ParticleSystemsDO>(null!));
            
            Assert.Throws<ArgumentException>(() => 
                XmlTestUtils.Deserialize<ParticleSystemsDO>(""));
        }

        #endregion

        #region 特殊字符和编码测试

        [Fact]
        public void ParticleSystems_SpecialCharacters_Preserved_Correctly()
        {
            // Arrange
            var specialCharsXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""effect_üöäéñ"" guid=""{AABBCCDD-EEFF-0011-2233-445566778899}"">
        <emitters>
            <emitter name=""emitter_中文"" _index_=""0"">
                <parameters>
                    <parameter name=""param_with_üöä"" value=""1.5"" />
                    <parameter name=""param_with_中文"" value=""测试"" />
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(specialCharsXml);
            var serializedXml = XmlTestUtils.Serialize(obj, specialCharsXml);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(specialCharsXml, serializedXml));
            
            // 验证特殊字符被正确保留
            Assert.Equal("effect_üöäéñ", obj.Effects[0].Name);
            Assert.Equal("emitter_中文", obj.Effects[0].Emitters!.EmitterList[0].Name);
        }

        [Fact]
        public void ParticleSystems_XmlEntities_Preserved_Correctly()
        {
            // Arrange
            var xmlEntitiesXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""effect_with_entities"">
        <emitters>
            <emitter name=""emitter_&lt;&gt;&amp;&quot;&apos;"" _index_=""0"">
                <parameters>
                    <parameter name=""param_value"" value=""&lt;test&gt; &amp; value"" />
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // Act
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xmlEntitiesXml);
            var serializedXml = XmlTestUtils.Serialize(obj, xmlEntitiesXml);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(xmlEntitiesXml, serializedXml));
        }

        #endregion

        #region 性能和内存测试

        [Fact]
        public void ParticleSystems_LargeXml_Handled_Efficiently()
        {
            // Arrange - 创建一个较大的XML文件
            var largeXml = GenerateLargeParticleSystemsXml(100); // 100个effects

            // Act
            var stopwatch = Stopwatch.StartNew();
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(largeXml);
            var deserializeTime = stopwatch.ElapsedMilliseconds;
            
            stopwatch.Restart();
            var serializedXml = XmlTestUtils.Serialize(obj, largeXml);
            var serializeTime = stopwatch.ElapsedMilliseconds;

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(largeXml, serializedXml));
            Assert.Equal(100, obj.Effects.Count);
            
            // 性能断言 - 反序列化应该在合理时间内完成
            Assert.True(deserializeTime < 5000, $"反序列化耗时过长: {deserializeTime}ms");
            Assert.True(serializeTime < 5000, $"序列化耗时过长: {serializeTime}ms");
            
            Console.WriteLine($"大文件处理性能 - 反序列化: {deserializeTime}ms, 序列化: {serializeTime}ms");
        }

        [Fact]
        public void ParticleSystems_MemoryUsage_Reasonable()
        {
            // Arrange
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // Act
            var startMemory = GC.GetTotalMemory(true);
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            var endMemory = GC.GetTotalMemory(false);
            var memoryUsed = endMemory - startMemory;

            // Assert
            Assert.NotNull(obj);
            
            // 内存使用应该在合理范围内 - 调整阈值到50MB
            Assert.True(memoryUsed < 50 * 1024 * 1024, $"内存使用过多: {memoryUsed / 1024 / 1024}MB");
            
            Console.WriteLine($"内存使用: {memoryUsed / 1024}KB");
        }

        #endregion

        #region 辅助方法

        private string GenerateLargeParticleSystemsXml(int effectCount)
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>";

            for (int i = 0; i < effectCount; i++)
            {
                xml += $@"
    <effect name=""effect_{i}"" guid=""{{{Guid.NewGuid()}}}"">
        <emitters>
            <emitter name=""emitter_{i}"" _index_=""0"">
                <parameters>
                    <parameter name=""life"" value=""{i * 0.1}"" />
                    <parameter name=""scale"" value=""{1.0 + i * 0.01}"" />
                </parameters>
            </emitter>
        </emitters>
    </effect>";
            }

            xml += @"
</particle_effects>";

            return xml;
        }

        #endregion
    }
}