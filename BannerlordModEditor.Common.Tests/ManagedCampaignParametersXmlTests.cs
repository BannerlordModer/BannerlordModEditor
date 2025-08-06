using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ManagedCampaignParametersXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/managed_campaign_parameters.xml";

        [Fact]
        public void ManagedCampaignParameters_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ManagedCampaignParameters>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}