using System;
using System.IO;
using System.Xml.Linq;
using BannerlordModEditor.Common.Models.DO;

namespace TestDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ParticleSystems 曲线元素分析 ===");
            
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 分析原始XML中的曲线元素
            var doc = XDocument.Parse(xml);
            var curveElements = doc.Descendants("curve").ToList();
            var keysElements = doc.Descendants("keys").ToList();
            var keyElements = doc.Descendants("key").ToList();
            
            Console.WriteLine($"原始XML: curve={curveElements.Count}, keys={keysElements.Count}, key={keyElements.Count}");
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 分析DO对象中的曲线元素
            int curveCountInDO = 0;
            int keysCountInDO = 0;
            int keyCountInDO = 0;
            
            if (obj.Effects != null)
            {
                foreach (var effect in obj.Effects)
                {
                    if (effect.Emitters?.EmitterList != null)
                    {
                        foreach (var emitter in effect.Emitters.EmitterList)
                        {
                            if (emitter.Parameters?.ParameterList != null)
                            {
                                foreach (var parameter in emitter.Parameters.ParameterList)
                                {
                                    if (parameter.ParameterCurve != null)
                                    {
                                        curveCountInDO++;
                                        if (parameter.ParameterCurve.Keys != null)
                                        {
                                            keysCountInDO++;
                                            keyCountInDO += parameter.ParameterCurve.Keys.KeyList.Count;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine($"DO对象: curve={curveCountInDO}, keys={keysCountInDO}, key={keyCountInDO}");
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的XML
            var doc2 = XDocument.Parse(xml2);
            var curveElements2 = doc2.Descendants("curve").ToList();
            var keysElements2 = doc2.Descendants("keys").ToList();
            var keyElements2 = doc2.Descendants("key").ToList();
            
            Console.WriteLine($"序列化XML: curve={curveElements2.Count}, keys={keysElements2.Count}, key={keyElements2.Count}");
            
            Console.WriteLine($"差异: curve={curveElements.Count - curveElements2.Count}, keys={keysElements.Count - keysElements2.Count}, key={keyElements.Count - keyElements2.Count}");
            
            // 保存调试文件
            Directory.CreateDirectory("Debug");
            File.WriteAllText("Debug/original.xml", xml);
            File.WriteAllText("Debug/serialized.xml", xml2);
            
            Console.WriteLine("调试文件已保存到 Debug/ 目录");
        }
    }
}