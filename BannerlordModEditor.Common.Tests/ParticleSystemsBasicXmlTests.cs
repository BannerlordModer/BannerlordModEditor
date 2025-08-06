using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsBasicXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/particle_systems_basic.xml";

        [Fact]
        public void ParticleSystemsBasic_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsBasic>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}