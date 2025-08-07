using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsExternalPartnersPCXmlTests
    {
        private const string TestDataPath = "TestData/CreditsExternalPartnersPC.xml";

        [Fact]
        public void CreditsExternalPartnersPC_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<CreditsExternalPartnersPC>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}