using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsOutdoorXmlTests
    {
        private const string TestDataPath = "TestData/particle_systems_outdoor.xml";

        [Fact]
        public void ParticleSystemsOutdoor_Roundtrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            var serialized = XmlTestUtils.Serialize(model, xml);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}