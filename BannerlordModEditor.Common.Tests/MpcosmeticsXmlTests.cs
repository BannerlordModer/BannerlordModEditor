using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/mpcosmetics.xml";

        [Fact]
        public void Mpcosmetics_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<Mpcosmetics>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}