using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsBasicXmlTests
    {
        private const string TestDataPath = "TestData/particle_systems_basic.xml";

        [Fact]
        public void ParticleSystemsBasic_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystems>(xml);

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}