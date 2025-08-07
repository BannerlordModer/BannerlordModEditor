using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalSetsXmlTests
    {
        private const string TestDataPath = "TestData/decal_sets.xml";

        [Fact]
        public void DecalSets_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<DecalSetsRoot>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}