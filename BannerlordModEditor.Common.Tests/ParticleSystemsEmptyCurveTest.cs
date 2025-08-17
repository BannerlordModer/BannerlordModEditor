using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsEmptyCurveTest
    {
        [Fact]
        public void Test_Empty_Curve_Elements()
        {
            // 测试带有空curve元素的参数
            var emptyCurveXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test_effect"" guid=""{TEST-GUID-0000-0000-000000000001}"">
        <emitters>
            <emitter name=""test_emitter"" _index_=""0"">
                <parameters>
                    <parameter name=""test_parameter"" value=""1.0"">
                        <curve name=""test_curve"" version=""1"">
                            <keys/>
                        </curve>
                    </parameter>
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(emptyCurveXml);
            
            Console.WriteLine($"=== 反序列化对象分析 ===");
            Console.WriteLine($"Effects数量: {obj.Effects.Count}");
            if (obj.Effects.Count > 0)
            {
                var effect = obj.Effects[0];
                Console.WriteLine($"Effect名称: {effect.Name}");
                if (effect.Emitters != null && effect.Emitters.EmitterList.Count > 0)
                {
                    var emitter = effect.Emitters.EmitterList[0];
                    Console.WriteLine($"Emitter名称: {emitter.Name}");
                    if (emitter.Parameters != null && emitter.Parameters.ParameterList.Count > 0)
                    {
                        var parameter = emitter.Parameters.ParameterList[0];
                        Console.WriteLine($"Parameter名称: {parameter.Name}");
                        Console.WriteLine($"Parameter是否有空Curve: {parameter.HasEmptyCurve}");
                        if (parameter.ParameterCurve != null)
                        {
                            Console.WriteLine($"Curve名称: {parameter.ParameterCurve.Name}");
                            Console.WriteLine($"Curve是否有空Keys: {parameter.ParameterCurve.HasEmptyKeys}");
                        }
                    }
                }
            }

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, emptyCurveXml);
            
            Console.WriteLine($"=== 序列化后XML ===");
            Console.WriteLine(xml2);

            // 结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(emptyCurveXml, xml2);
            Console.WriteLine($"结构化对比结果: {areEqual}");
            
            if (!areEqual)
            {
                var doc1 = XDocument.Parse(emptyCurveXml);
                var doc2 = XDocument.Parse(xml2);
                
                var curves1 = doc1.Descendants("curve").Count();
                var curves2 = doc2.Descendants("curve").Count();
                Console.WriteLine($"原始curve数: {curves1}");
                Console.WriteLine($"序列化curve数: {curves2}");
                
                var keys1 = doc1.Descendants("keys").Count();
                var keys2 = doc2.Descendants("keys").Count();
                Console.WriteLine($"原始keys数: {keys1}");
                Console.WriteLine($"序列化keys数: {keys2}");
            }
            
            Assert.True(areEqual, "空curve元素序列化失败");
        }
    }
}