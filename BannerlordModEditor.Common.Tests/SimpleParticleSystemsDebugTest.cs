using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleParticleSystemsDebugTest
    {
        [Fact]
        public void Simple_Debug_Test()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            Console.WriteLine("=== 开始简单调试测试 ===");
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            Console.WriteLine($"反序列化成功: Effects数量 = {obj.Effects.Count}");
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            Console.WriteLine($"再序列化成功: 长度 = {xml2.Length}");
            
            // 比较节点和属性数量
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);
            
            var elements1 = doc1.Descendants().Count();
            var elements2 = doc2.Descendants().Count();
            var attributes1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            var attributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Console.WriteLine($"原始XML: 元素={elements1}, 属性={attributes1}");
            Console.WriteLine($"序列化XML: 元素={elements2}, 属性={attributes2}");
            Console.WriteLine($"差异: 元素={elements2-elements1}, 属性={attributes2-attributes1}");
            
            // 检查结构化相等性
            bool areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Console.WriteLine($"结构化相等: {areEqual}");
            
            if (!areEqual)
            {
                Console.WriteLine("=== 发现差异，分析详情 ===");
                
                // 比较根元素
                var root1 = doc1.Root;
                var root2 = doc2.Root;
                
                if (root1 != null && root2 != null)
                {
                    Console.WriteLine($"根元素名称: 原始={root1.Name}, 序列化={root2.Name}");
                    Console.WriteLine($"根元素属性: 原始={root1.Attributes().Count()}, 序列化={root2.Attributes().Count()}");
                    
                    // 比较effect元素
                    var effects1 = root1.Elements("effect").ToList();
                    var effects2 = root2.Elements("effect").ToList();
                    
                    Console.WriteLine($"Effect元素数量: 原始={effects1.Count}, 序列化={effects2.Count}");
                    
                    if (effects1.Count > 0 && effects2.Count > 0)
                    {
                        var firstEffect1 = effects1[0];
                        var firstEffect2 = effects2[0];
                        
                        Console.WriteLine($"第一个Effect属性: 原始={firstEffect1.Attributes().Count()}, 序列化={firstEffect2.Attributes().Count()}");
                        
                        // 比较emitters
                        var emitters1 = firstEffect1.Element("emitters");
                        var emitters2 = firstEffect2.Element("emitters");
                        
                        if (emitters1 != null && emitters2 != null)
                        {
                            Console.WriteLine($"Emitters元素存在");
                            var emitterList1 = emitters1.Elements("emitter").ToList();
                            var emitterList2 = emitters2.Elements("emitter").ToList();
                            
                            Console.WriteLine($"Emitter元素数量: 原始={emitterList1.Count}, 序列化={emitterList2.Count}");
                            
                            if (emitterList1.Count > 0 && emitterList2.Count > 0)
                            {
                                var firstEmitter1 = emitterList1[0];
                                var firstEmitter2 = emitterList2[0];
                                
                                Console.WriteLine($"第一个Emitter属性: 原始={firstEmitter1.Attributes().Count()}, 序列化={firstEmitter2.Attributes().Count()}");
                                
                                // 检查子元素
                                var children1 = firstEmitter1.Element("children");
                                var children2 = firstEmitter2.Element("children");
                                var flags1 = firstEmitter1.Element("flags");
                                var flags2 = firstEmitter2.Element("flags");
                                var parameters1 = firstEmitter1.Element("parameters");
                                var parameters2 = firstEmitter2.Element("parameters");
                                
                                Console.WriteLine($"Children: 原始={children1 != null}, 序列化={children2 != null}");
                                Console.WriteLine($"Flags: 原始={flags1 != null}, 序列化={flags2 != null}");
                                Console.WriteLine($"Parameters: 原始={parameters1 != null}, 序列化={parameters2 != null}");
                                
                                if (flags1 != null && flags2 != null)
                                {
                                    var flagList1 = flags1.Elements("flag").ToList();
                                    var flagList2 = flags2.Elements("flag").ToList();
                                    Console.WriteLine($"Flag元素数量: 原始={flagList1.Count}, 序列化={flagList2.Count}");
                                }
                            }
                        }
                    }
                }
            }
            
            // 输出文件用于手动检查
            File.WriteAllText("/tmp/original_particlesystems.xml", xml);
            File.WriteAllText("/tmp/serialized_particlesystems.xml", xml2);
            Console.WriteLine("文件已输出到 /tmp/ 目录");
            
            Assert.True(areEqual, "XML结构化对比失败");
        }
    }
}