using System.IO;
using Xunit;

using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsOutdoorXmlTests
    {
        private const string TestDataPath = "TestData/particle_systems_outdoor.xml";

        [Fact]
        public void ParticleSystemsOutdoor_Roundtrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ParticleSystems>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}