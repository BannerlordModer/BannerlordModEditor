using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsHardcodedMisc2XmlTests
    {
        [Fact]
        public void ParticleSystemsHardcodedMisc2_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc2.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystems>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}