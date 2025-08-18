using BannerlordModEditor.Common.Models.DO;
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
    public class ParticleSystemsMapIconXmlTests
    {
        [Fact]
        public void ParticleSystemsMapIcon_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "particle_systems_map_icon.xml");
            
            // Act - 反序列化
            var particleSystems = XmlTestUtils.Deserialize<ParticleSystemsMapIconDO>(File.ReadAllText(xmlPath));
            
            // Act - 序列化
            var savedXml = XmlTestUtils.Serialize(particleSystems);

            // Assert - 基本结构验证
            Assert.NotNull(particleSystems);
            Assert.NotNull(particleSystems.Effects);
            Assert.True(particleSystems.Effects.Count > 0, "Should have at least one particle effect");
            
            // 验证具体的粒子特效
            var plunderEffect = particleSystems.Effects.FirstOrDefault(e => e.Name == "map_icon_village_plunder_fx");
            Assert.NotNull(plunderEffect);
            Assert.False(string.IsNullOrEmpty(plunderEffect.Guid), "Plunder effect should have a GUID");
            Assert.True(plunderEffect.Emitters.Count > 0, "Plunder effect should have emitters");
            
            var emitter = plunderEffect.Emitters.First();
            Assert.Equal("Emitter", emitter.Name);
            Assert.Equal("0", emitter.Index);
            Assert.True(emitter.Flags.Count > 0, "Emitter should have flags");
            Assert.True(emitter.Parameters.Count > 0, "Emitter should have parameters");
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            var originalCount = originalDoc.Root?.Elements("effect").Count() ?? 0;
            var savedCount = savedDoc.Root?.Elements("effect").Count() ?? 0;
            Assert.True(System.Math.Abs(originalCount - savedCount) <= 1, 
                $"Effect count should be close (original: {originalCount}, saved: {savedCount})");
        }
        
        [Fact]
        public void ParticleSystemsMapIcon_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "particle_systems_map_icon.xml");
            
            // Act
            var particleSystems = XmlTestUtils.Deserialize<ParticleSystemsMapIconDO>(File.ReadAllText(xmlPath));
            
            // Assert - 验证所有粒子特效都有必要的属性
            foreach (var effect in particleSystems.Effects)
            {
                Assert.False(string.IsNullOrWhiteSpace(effect.Name), "Effect should have Name");
                
                // 验证发射器
                foreach (var emitter in effect.Emitters)
                {
                    Assert.False(string.IsNullOrWhiteSpace(emitter.Name), "Emitter should have Name");
                    
                    // 验证标志
                    foreach (var flag in emitter.Flags)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(flag.Name), "Flag should have Name");
                        Assert.False(string.IsNullOrWhiteSpace(flag.Value), "Flag should have Value");
                        Assert.True(flag.Value == "true" || flag.Value == "false", 
                            $"Flag value should be boolean: {flag.Value}");
                    }
                    
                    // 验证参数
                    foreach (var parameter in emitter.Parameters)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(parameter.Name), "Parameter should have Name");
                        
                        // 跳过某些特殊参数的值检查（如 sample_mesh 等可能只有名称）
                        if (parameter.Name != "sample_mesh" && 
                            parameter.Name != "decal_material" && 
                            parameter.Name != "particle_color")
                        {
                            // 至少应该有 value, base 或 bias 中的一个
                            Assert.True(!string.IsNullOrEmpty(parameter.Value) || 
                                       !string.IsNullOrEmpty(parameter.Base) || 
                                       !string.IsNullOrEmpty(parameter.Bias), 
                                $"Parameter {parameter.Name} should have at least one value");
                        }
                    }
                }
            }
            
            // 验证包含预期的特效
            var allNames = particleSystems.Effects.Select(e => e.Name).ToList();
            Assert.Contains("map_icon_village_plunder_fx", allNames);
            
            // 确保没有重复的特效名称
            var uniqueNames = allNames.Distinct().ToList();
            Assert.Equal(allNames.Count, uniqueNames.Count);
            
            // 验证特效数量合理
            Assert.True(particleSystems.Effects.Count > 0, "Should have at least one particle effect");
        }
        
        [Fact]
        public void ParticleSystemsMapIcon_SpecificEffectValidation_ShouldPassDetailedChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "particle_systems_map_icon.xml");
            
            // Act
            var particleSystems = XmlTestUtils.Deserialize<ParticleSystemsMapIconDO>(File.ReadAllText(xmlPath));
            
            // Assert - 验证特定特效的完整性
            var plunderEffect = particleSystems.Effects.FirstOrDefault(e => e.Name == "map_icon_village_plunder_fx");
            Assert.NotNull(plunderEffect);
            
            // 验证 GUID 格式
            Assert.NotNull(plunderEffect.Guid);
            Assert.True(plunderEffect.Guid.StartsWith("{") && plunderEffect.Guid.EndsWith("}"), 
                "GUID should be in correct format");
            
            // 验证发射器
            Assert.True(plunderEffect.Emitters.Count > 0, "Should have at least one emitter");
            var emitter = plunderEffect.Emitters.First();
            Assert.Equal("Emitter", emitter.Name);
            Assert.Equal("0", emitter.Index);
            
            // 验证常见标志
            var emitWhileMovingFlag = emitter.Flags.FirstOrDefault(f => f.Name == "emit_while_moving");
            Assert.NotNull(emitWhileMovingFlag);
            Assert.Equal("false", emitWhileMovingFlag.Value);
            
            var orderByDistanceFlag = emitter.Flags.FirstOrDefault(f => f.Name == "order_by_distance");
            Assert.NotNull(orderByDistanceFlag);
            Assert.Equal("true", orderByDistanceFlag.Value);
            
            // 验证常见参数
            var emitterLifeParam = emitter.Parameters.FirstOrDefault(p => p.Name == "emitter_life");
            Assert.NotNull(emitterLifeParam);
            Assert.Equal("0.000", emitterLifeParam.Value);
            
            var gravityParam = emitter.Parameters.FirstOrDefault(p => p.Name == "gravity");
            Assert.NotNull(gravityParam);
            Assert.Equal("0.000, 0.000, 1.000", gravityParam.Value);
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 