using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsDebugTest
    {
        [Fact]
        public void Debug_ParticleSystems_Differences()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 解析XML以便比较
            var doc1 = XDocument.Parse(xml);
            var doc2 = XDocument.Parse(xml2);

            // 比较根元素
            var root1 = doc1.Root;
            var root2 = doc2.Root;

            // 统计effect元素数量
            var effects1 = root1.Elements("effect").Count();
            var effects2 = root2.Elements("effect").Count();

            // 输出一些调试信息
            File.WriteAllText("/tmp/particle_systems_original.xml", xml);
            File.WriteAllText("/tmp/particle_systems_serialized.xml", xml2);

            // 简单的差异分析
            var differences = AnalyzeDifferences(doc1, doc2);
            File.WriteAllText("/tmp/particle_systems_differences.txt", differences);

            Assert.True(effects1 == effects2, $"Effect count mismatch: original={effects1}, serialized={effects2}");
        }

        private string AnalyzeDifferences(XDocument doc1, XDocument doc2)
        {
            var result = new System.Text.StringBuilder();
            
            var root1 = doc1.Root;
            var root2 = doc2.Root;
            
            // 比较effect数量
            var effects1 = root1.Elements("effect").Count();
            var effects2 = root2.Elements("effect").Count();
            result.AppendLine($"Effect count: original={effects1}, serialized={effects2}");
            
            // 检查每个effect
            var effectsList1 = root1.Elements("effect").ToList();
            var effectsList2 = root2.Elements("effect").ToList();
            
            for (int i = 0; i < Math.Min(effects1, effects2); i++)
            {
                var effect1 = effectsList1[i];
                var effect2 = effectsList2[i];
                var name1 = effect1.Attribute("name")?.Value;
                var name2 = effect2.Attribute("name")?.Value;
                
                if (name1 != name2)
                {
                    result.AppendLine($"Effect {i} name mismatch: {name1} vs {name2}");
                }
                
                // 检查emitters
                var emitters1 = effect1.Element("emitters")?.Elements("emitter").Count() ?? 0;
                var emitters2 = effect2.Element("emitters")?.Elements("emitter").Count() ?? 0;
                
                if (emitters1 != emitters2)
                {
                    result.AppendLine($"Effect {name1} emitter count mismatch: {emitters1} vs {emitters2}");
                }
            }
            
            return result.ToString();
        }
    }
}