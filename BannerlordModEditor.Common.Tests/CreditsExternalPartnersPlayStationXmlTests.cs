using System.IO;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsExternalPartnersPlayStationXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/CreditsExternalPartnersPlayStation.xml";

        [Fact]
        public void CreditsExternalPartnersPlayStation_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<CreditsExternalPartnersPlayStation>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}
