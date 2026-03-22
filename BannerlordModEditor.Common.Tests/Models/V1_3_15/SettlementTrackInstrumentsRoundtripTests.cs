using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class SettlementTrackInstrumentsRoundtripTests
    {
        [Fact]
        public async Task SettlementTrackInstruments_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "settlement_track_instruments.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);
            var obj = XmlTestUtils.Deserialize<SettlementTrackInstruments>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }
    }
}
