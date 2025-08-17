using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsSimpleCurveTest
    {
        [Fact]
        public void Test_Simple_Curve_Serialization()
        {
            // 创建一个简单的曲线结构
            var simpleCurve = new ParticleSystemsDO
            {
                Effects = new List<EffectDO>
                {
                    new EffectDO
                    {
                        Name = "test_effect",
                        Guid = "{TEST-GUID-0000-0000-000000000001}",
                        Emitters = new EmittersDO
                        {
                            EmitterList = new List<EmitterDO>
                            {
                                new EmitterDO
                                {
                                    Name = "test_emitter",
                                    Index = "0",
                                    Parameters = new ParametersDO
                                    {
                                        ParameterList = new List<ParameterDO>
                                        {
                                            new ParameterDO
                                            {
                                                Name = "test_parameter",
                                                Value = "1.0",
                                                ParameterCurve = new CurveDO
                                                {
                                                    Name = "test_curve",
                                                    Version = "1",
                                                    Keys = new KeysDO
                                                    {
                                                        KeyList = new List<KeyDO>
                                                        {
                                                            new KeyDO { Time = "0", Value = "0" },
                                                            new KeyDO { Time = "1", Value = "1" }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // 序列化
            var xml = XmlTestUtils.Serialize(simpleCurve);
            Console.WriteLine($"=== 简化曲线序列化结果 ===");
            Console.WriteLine(xml);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再次序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Console.WriteLine($"结构化对比结果: {areEqual}");
            
            if (!areEqual)
            {
                var doc1 = XDocument.Parse(xml);
                var doc2 = XDocument.Parse(xml2);
                
                var elements1 = doc1.Descendants().Count();
                var elements2 = doc2.Descendants().Count();
                Console.WriteLine($"第一次序列化元素数: {elements1}");
                Console.WriteLine($"第二次序列化元素数: {elements2}");
                
                var curves1 = doc1.Descendants("curve").Count();
                var curves2 = doc2.Descendants("curve").Count();
                Console.WriteLine($"第一次序列化curve数: {curves1}");
                Console.WriteLine($"第二次序列化curve数: {curves2}");
                
                var keys1 = doc1.Descendants("key").Count();
                var keys2 = doc2.Descendants("key").Count();
                Console.WriteLine($"第一次序列化key数: {keys1}");
                Console.WriteLine($"第二次序列化key数: {keys2}");
            }
            
            Assert.True(areEqual, "简化曲线序列化失败");
        }

        [Fact]
        public void Test_Parameter_With_Empty_Curve()
        {
            // 测试带有空曲线的参数
            var parameterWithEmptyCurve = new ParticleSystemsDO
            {
                Effects = new List<EffectDO>
                {
                    new EffectDO
                    {
                        Name = "test_effect",
                        Guid = "{TEST-GUID-0000-0000-000000000002}",
                        Emitters = new EmittersDO
                        {
                            EmitterList = new List<EmitterDO>
                            {
                                new EmitterDO
                                {
                                    Name = "test_emitter",
                                    Index = "0",
                                    Parameters = new ParametersDO
                                    {
                                        ParameterList = new List<ParameterDO>
                                        {
                                            new ParameterDO
                                            {
                                                Name = "test_parameter",
                                                Value = "1.0",
                                                // 故意不设置ParameterCurve，测试空值处理
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // 序列化
            var xml = XmlTestUtils.Serialize(parameterWithEmptyCurve);
            Console.WriteLine($"=== 空曲线参数序列化结果 ===");
            Console.WriteLine(xml);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再次序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Console.WriteLine($"结构化对比结果: {areEqual}");
            
            Assert.True(areEqual, "空曲线参数序列化失败");
        }
    }
}