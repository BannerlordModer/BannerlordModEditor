using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_2_9;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_2_9
{
    public class SettlementTracksRoundtripTests
    {
        [Fact]
        public async Task SettlementTracks_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_tracks.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var tracks = XmlTestUtils.Deserialize<SettlementTracks>(xmlContent);

            Assert.NotNull(tracks);
            Assert.NotEmpty(tracks!.Tracks);
        }

        [Fact]
        public async Task SettlementTracks_Roundtrip_ShouldPreserveAllData()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_tracks.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var tracks = XmlTestUtils.Deserialize<SettlementTracks>(originalXml);

            var serializedXml = XmlTestUtils.Serialize(tracks, originalXml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public async Task SettlementTracks_ShouldHaveCorrectTrackCount()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_tracks.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var tracks = XmlTestUtils.Deserialize<SettlementTracks>(xmlContent);

            Assert.NotNull(tracks);
            Assert.True(tracks!.Tracks.Count > 0);
        }

        [Fact]
        public async Task SettlementTracks_FirstTrack_ShouldHaveCorrectProperties()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_tracks.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var tracks = XmlTestUtils.Deserialize<SettlementTracks>(xmlContent);

            var firstTrack = tracks!.Tracks.First();
            Assert.Equal("aserai_01", firstTrack.Id);
            Assert.Equal("event:/music/musicians/aserai/01", firstTrack.EventId);
            Assert.Equal("Culture.aserai", firstTrack.Culture);
            Assert.Equal("lords_hall", firstTrack.Location);
            Assert.Equal("78", firstTrack.Tempo);
        }

        [Fact]
        public async Task SettlementTracks_TrackWithInstruments_ShouldBeParsed()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_tracks.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var tracks = XmlTestUtils.Deserialize<SettlementTracks>(xmlContent);

            var firstTrack = tracks!.Tracks.First();
            Assert.NotNull(firstTrack.Instruments);
            Assert.NotEmpty(firstTrack.Instruments!.InstrumentList);
            Assert.Equal("nay", firstTrack.Instruments.InstrumentList[0].Id);
            Assert.Equal("oud", firstTrack.Instruments.InstrumentList[1].Id);
            Assert.Equal("bodhran", firstTrack.Instruments.InstrumentList[2].Id);
        }
    }
}
