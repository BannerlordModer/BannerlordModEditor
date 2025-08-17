using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Mappers;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsDOPerformanceTests
    {
        [Fact]
        public void ParticleSystems_Performance_LargeDataHandling()
        {
            // Arrange - 创建大型粒子系统数据
            var largeParticleSystem = CreateLargeParticleSystem(1000, 10);
            
            // Act - 测量序列化性能
            var serializeStopwatch = Stopwatch.StartNew();
            var xml = XmlTestUtils.Serialize(largeParticleSystem);
            serializeStopwatch.Stop();
            
            // Act - 测量反序列化性能
            var deserializeStopwatch = Stopwatch.StartNew();
            var deserialized = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            deserializeStopwatch.Stop();
            
            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(largeParticleSystem.Effects.Count, deserialized.Effects.Count);
            
            // 性能断言 - 这些应该根据实际情况调整
            Assert.True(serializeStopwatch.ElapsedMilliseconds < 5000, $"序列化耗时过长: {serializeStopwatch.ElapsedMilliseconds}ms");
            Assert.True(deserializeStopwatch.ElapsedMilliseconds < 5000, $"反序列化耗时过长: {deserializeStopwatch.ElapsedMilliseconds}ms");
            
            // 输出性能指标
            Console.WriteLine($"序列化耗时: {serializeStopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"反序列化耗时: {deserializeStopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"XML大小: {xml.Length} 字符");
            Console.WriteLine($"效果数量: {largeParticleSystem.Effects.Count}");
            Console.WriteLine($"发射器总数: {largeParticleSystem.Effects.Sum(e => e.Emitters?.EmitterList?.Count ?? 0)}");
        }

        [Fact]
        public void ParticleSystems_Performance_MemoryUsage()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(true);
            var particleSystem = CreateLargeParticleSystem(500, 5);
            
            // Act - 创建多个实例测试内存使用
            var instances = new List<ParticleSystemsDO>();
            for (int i = 0; i < 10; i++)
            {
                var xml = XmlTestUtils.Serialize(particleSystem);
                var deserialized = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
                instances.Add(deserialized);
            }
            
            var afterMemory = GC.GetTotalMemory(true);
            var memoryIncrease = afterMemory - initialMemory;
            
            // Assert
            Assert.Equal(10, instances.Count);
            Assert.True(memoryIncrease < 100 * 1024 * 1024, $"内存使用过多: {memoryIncrease / 1024 / 1024}MB");
            
            Console.WriteLine($"内存增长: {memoryIncrease / 1024}KB");
        }

        [Fact]
        public void ParticleSystems_Performance_CurveOptimization()
        {
            // Arrange - 创建包含大量曲线的粒子系统
            var particleSystem = CreateCurveHeavyParticleSystem(100);
            
            // Act
            var stopwatch = Stopwatch.StartNew();
            var xml = XmlTestUtils.Serialize(particleSystem);
            var deserialized = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            stopwatch.Stop();
            
            // Assert
            Assert.NotNull(deserialized);
            
            var totalCurves = deserialized.Effects
                .Sum(e => e.Emitters?.EmitterList?.Sum(em => em.Parameters?.ParameterList?.Sum(p => p.ParameterCurves?.Count ?? 0) ?? 0) ?? 0);
            
            Assert.Equal(100, totalCurves);
            Assert.True(stopwatch.ElapsedMilliseconds < 2000, $"曲线处理耗时过长: {stopwatch.ElapsedMilliseconds}ms");
            
            Console.WriteLine($"处理 {totalCurves} 条曲线耗时: {stopwatch.ElapsedMilliseconds}ms");
        }

        [Fact]
        public void ParticleSystems_Performance_EmptyElementHandling()
        {
            // Arrange - 创建包含大量空元素的粒子系统
            var particleSystem = CreateEmptyElementHeavyParticleSystem(200);
            
            // Act
            var stopwatch = Stopwatch.StartNew();
            var xml = XmlTestUtils.Serialize(particleSystem);
            var deserialized = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            stopwatch.Stop();
            
            // Assert
            Assert.NotNull(deserialized);
            Assert.True(stopwatch.ElapsedMilliseconds < 1000, $"空元素处理耗时过长: {stopwatch.ElapsedMilliseconds}ms");
            
            Console.WriteLine($"空元素处理耗时: {stopwatch.ElapsedMilliseconds}ms");
        }

        [Fact]
        public void ParticleSystems_Performance_MapperConversion()
        {
            // Arrange
            var particleSystem = CreateLargeParticleSystem(100, 3);
            
            // Act - 测试DO/DTO转换性能
            var stopwatch = Stopwatch.StartNew();
            
            for (int i = 0; i < 100; i++)
            {
                var dto = ParticleSystemsMapper.ToDTO(particleSystem);
                var dobj = ParticleSystemsMapper.ToDO(dto);
            }
            
            stopwatch.Stop();
            
            // Assert
            Assert.True(stopwatch.ElapsedMilliseconds < 2000, $"映射转换耗时过长: {stopwatch.ElapsedMilliseconds}ms");
            
            Console.WriteLine($"100次DO/DTO转换耗时: {stopwatch.ElapsedMilliseconds}ms");
        }

        #region Helper Methods

        private ParticleSystemsDO CreateLargeParticleSystem(int effectCount, int emittersPerEffect)
        {
            var particleSystem = new ParticleSystemsDO();
            
            for (int i = 0; i < effectCount; i++)
            {
                var effect = new EffectDO
                {
                    Name = $"effect_{i}",
                    Guid = Guid.NewGuid().ToString(),
                    SoundCode = i % 2 == 0 ? "sound_test" : null,
                    Emitters = CreateEmitters(emittersPerEffect)
                };
                
                particleSystem.Effects.Add(effect);
            }
            
            return particleSystem;
        }

        private EmittersDO CreateEmitters(int count)
        {
            var emitters = new EmittersDO();
            
            for (int i = 0; i < count; i++)
            {
                var emitter = new EmitterDO
                {
                    Name = $"emitter_{i}",
                    Index = i.ToString(),
                    Flags = CreateFlags(),
                    Parameters = CreateParameters(),
                    Children = i % 3 == 0 ? CreateChildren(2) : null
                };
                
                emitters.EmitterList.Add(emitter);
            }
            
            return emitters;
        }

        private ChildrenDO CreateChildren(int count)
        {
            var children = new ChildrenDO();
            
            for (int i = 0; i < count; i++)
            {
                var child = new EmitterDO
                {
                    Name = $"child_{i}",
                    Index = i.ToString(),
                    Parameters = CreateParameters()
                };
                
                children.EmitterList.Add(child);
            }
            
            return children;
        }

        private ParticleFlagsDO CreateFlags()
        {
            var flags = new ParticleFlagsDO();
            
            flags.FlagList.AddRange(new[]
            {
                new ParticleFlagDO { Name = "flag1", Value = "true" },
                new ParticleFlagDO { Name = "flag2", Value = "false" }
            });
            
            return flags;
        }

        private ParametersDO CreateParameters()
        {
            var parameters = new ParametersDO();
            
            parameters.ParameterList.AddRange(new[]
            {
                new ParameterDO
                {
                    Name = "param1",
                    Value = "1.0",
                    Base = "0.5",
                    Bias = "0.1",
                    Curve = "linear"
                },
                new ParameterDO
                {
                    Name = "param2",
                    Value = "2.0",
                    ColorElement = new ColorDO
                    {
                        Keys = new KeysDO
                        {
                            KeyList = new List<KeyDO>
                            {
                                new KeyDO { Time = "0.0", Value = "1.0" },
                                new KeyDO { Time = "1.0", Value = "0.0" }
                            }
                        }
                    }
                }
            });
            
            return parameters;
        }

        private ParticleSystemsDO CreateCurveHeavyParticleSystem(int curveCount)
        {
            var particleSystem = new ParticleSystemsDO();
            var effect = new EffectDO
            {
                Name = "curve_test",
                Emitters = new EmittersDO()
            };
            
            var emitter = new EmitterDO
            {
                Name = "curve_emitter",
                Parameters = new ParametersDO()
            };
            
            for (int i = 0; i < curveCount; i++)
            {
                var parameter = new ParameterDO
                {
                    Name = $"curve_param_{i}",
                    Value = i.ToString(),
                    ParameterCurves = new List<CurveDO>
                    {
                        new CurveDO
                        {
                            Name = $"curve_{i}",
                            Version = "1",
                            Default = "1.0",
                            Keys = new KeysDO
                            {
                                KeyList = new List<KeyDO>
                                {
                                    new KeyDO { Time = "0.0", Value = "0.0" },
                                    new KeyDO { Time = "1.0", Value = "1.0" }
                                }
                            }
                        }
                    }
                };
                
                emitter.Parameters.ParameterList.Add(parameter);
            }
            
            effect.Emitters.EmitterList.Add(emitter);
            particleSystem.Effects.Add(effect);
            
            return particleSystem;
        }

        private ParticleSystemsDO CreateEmptyElementHeavyParticleSystem(int emptyElementCount)
        {
            var particleSystem = new ParticleSystemsDO();
            
            for (int i = 0; i < emptyElementCount; i++)
            {
                var effect = new EffectDO
                {
                    Name = $"empty_effect_{i}",
                    Emitters = new EmittersDO()
                };
                
                var emitter = new EmitterDO
                {
                    Name = $"empty_emitter_{i}",
                    HasEmptyChildren = i % 2 == 0,
                    HasEmptyFlags = i % 3 == 0,
                    HasEmptyParameters = i % 4 == 0
                };
                
                effect.Emitters.EmitterList.Add(emitter);
                particleSystem.Effects.Add(effect);
            }
            
            return particleSystem;
        }

        #endregion
    }
}