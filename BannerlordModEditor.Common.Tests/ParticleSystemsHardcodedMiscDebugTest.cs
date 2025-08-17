using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsHardcodedMiscDebugTest
    {
        [Fact]
        public void ParticleSystemsHardcodedMisc1_Debug_Analysis()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            Console.WriteLine($"=== 原始XML分析 ===");
            Console.WriteLine($"XML长度: {xml.Length}");
            Console.WriteLine($"XML前500字符: {xml.Substring(0, Math.Min(500, xml.Length))}");

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            Console.WriteLine($"=== 反序列化对象分析 ===");
            Console.WriteLine($"Effects数量: {obj.Effects.Count}");
            if (obj.Effects.Count > 0)
            {
                var firstEffect = obj.Effects[0];
                Console.WriteLine($"第一个Effect名称: {firstEffect.Name}");
                Console.WriteLine($"第一个Effect GUID: {firstEffect.Guid}");
                Console.WriteLine($"第一个Effect是否有Emitters: {firstEffect.Emitters != null}");
                
                if (firstEffect.Emitters != null)
                {
                    Console.WriteLine($"Emitters数量: {firstEffect.Emitters.EmitterList.Count}");
                    if (firstEffect.Emitters.EmitterList.Count > 0)
                    {
                        var firstEmitter = firstEffect.Emitters.EmitterList[0];
                        Console.WriteLine($"第一个Emitter名称: {firstEmitter.Name}");
                        Console.WriteLine($"第一个Emitter Index: {firstEmitter.Index}");
                        Console.WriteLine($"第一个Emitter是否有Children: {firstEmitter.Children != null}");
                        Console.WriteLine($"第一个Emitter是否有Flags: {firstEmitter.Flags != null}");
                        Console.WriteLine($"第一个Emitter是否有Parameters: {firstEmitter.Parameters != null}");
                        
                        if (firstEmitter.Flags != null)
                        {
                            Console.WriteLine($"Flags数量: {firstEmitter.Flags.FlagList.Count}");
                        }
                        
                        if (firstEmitter.Parameters != null)
                        {
                            Console.WriteLine($"Parameters数量: {firstEmitter.Parameters.ParameterList.Count}");
                        }
                    }
                }
            }

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            Console.WriteLine($"=== 序列化后XML分析 ===");
            Console.WriteLine($"序列化后XML长度: {xml2.Length}");
            Console.WriteLine($"序列化后XML前500字符: {xml2.Substring(0, Math.Min(500, xml2.Length))}");

            // 结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Console.WriteLine($"结构化对比结果: {areEqual}");
            
            if (!areEqual)
            {
                Console.WriteLine($"=== 差异分析 ===");
                var doc1 = XDocument.Parse(xml);
                var doc2 = XDocument.Parse(xml2);
                
                var elements1 = doc1.Descendants().Count();
                var elements2 = doc2.Descendants().Count();
                Console.WriteLine($"原始XML元素数量: {elements1}");
                Console.WriteLine($"序列化后XML元素数量: {elements2}");
                
                var attributes1 = doc1.Descendants().Sum(e => e.Attributes().Count());
                var attributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
                Console.WriteLine($"原始XML属性数量: {attributes1}");
                Console.WriteLine($"序列化后XML属性数量: {attributes2}");
            }
            
            // 临时输出结果以便查看
            File.WriteAllText("TestData/debug_original.xml", xml);
            File.WriteAllText("TestData/debug_serialized.xml", xml2);
            
            Assert.True(areEqual, "XML结构化对比失败");
        }

        [Fact]
        public void ParticleSystemsHardcodedMisc2_Debug_Analysis()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc2.xml";
            var xml = File.ReadAllText(xmlPath);

            Console.WriteLine($"=== 原始XML分析 ===");
            Console.WriteLine($"XML长度: {xml.Length}");
            Console.WriteLine($"XML前500字符: {xml.Substring(0, Math.Min(500, xml.Length))}");

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            Console.WriteLine($"=== 反序列化对象分析 ===");
            Console.WriteLine($"Effects数量: {obj.Effects.Count}");
            if (obj.Effects.Count > 0)
            {
                var firstEffect = obj.Effects[0];
                Console.WriteLine($"第一个Effect名称: {firstEffect.Name}");
                Console.WriteLine($"第一个Effect GUID: {firstEffect.Guid}");
                Console.WriteLine($"第一个Effect是否有Emitters: {firstEffect.Emitters != null}");
                
                if (firstEffect.Emitters != null)
                {
                    Console.WriteLine($"Emitters数量: {firstEffect.Emitters.EmitterList.Count}");
                    if (firstEffect.Emitters.EmitterList.Count > 0)
                    {
                        var firstEmitter = firstEffect.Emitters.EmitterList[0];
                        Console.WriteLine($"第一个Emitter名称: {firstEmitter.Name}");
                        Console.WriteLine($"第一个Emitter Index: {firstEmitter.Index}");
                        Console.WriteLine($"第一个Emitter是否有Children: {firstEmitter.Children != null}");
                        Console.WriteLine($"第一个Emitter是否有Flags: {firstEmitter.Flags != null}");
                        Console.WriteLine($"第一个Emitter是否有Parameters: {firstEmitter.Parameters != null}");
                        
                        if (firstEmitter.Flags != null)
                        {
                            Console.WriteLine($"Flags数量: {firstEmitter.Flags.FlagList.Count}");
                        }
                        
                        if (firstEmitter.Parameters != null)
                        {
                            Console.WriteLine($"Parameters数量: {firstEmitter.Parameters.ParameterList.Count}");
                        }
                    }
                }
            }

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            Console.WriteLine($"=== 序列化后XML分析 ===");
            Console.WriteLine($"序列化后XML长度: {xml2.Length}");
            Console.WriteLine($"序列化后XML前500字符: {xml2.Substring(0, Math.Min(500, xml2.Length))}");

            // 结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Console.WriteLine($"结构化对比结果: {areEqual}");
            
            if (!areEqual)
            {
                Console.WriteLine($"=== 差异分析 ===");
                var doc1 = XDocument.Parse(xml);
                var doc2 = XDocument.Parse(xml2);
                
                var elements1 = doc1.Descendants().Count();
                var elements2 = doc2.Descendants().Count();
                Console.WriteLine($"原始XML元素数量: {elements1}");
                Console.WriteLine($"序列化后XML元素数量: {elements2}");
                
                var attributes1 = doc1.Descendants().Sum(e => e.Attributes().Count());
                var attributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
                Console.WriteLine($"原始XML属性数量: {attributes1}");
                Console.WriteLine($"序列化后XML属性数量: {attributes2}");
            }
            
            // 临时输出结果以便查看
            File.WriteAllText("TestData/debug_original2.xml", xml);
            File.WriteAllText("TestData/debug_serialized2.xml", xml2);
            
            Assert.True(areEqual, "XML结构化对比失败");
        }
    }
}