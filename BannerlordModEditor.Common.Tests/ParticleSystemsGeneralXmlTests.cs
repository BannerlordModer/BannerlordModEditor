using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsGeneralXmlTests
    {
        private const string TestDataPath = "TestData/particle_systems_general.xml";

        [Fact]
        public void ParticleSystemsGeneral_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystems>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}