using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_2_9;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_2_9
{
    public class SettlementTrackInstrumentsRoundtripTests
    {
        [Fact]
        public async Task SettlementTrackInstruments_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_track_instruments.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var instruments = XmlTestUtils.Deserialize<SettlementTrackInstruments>(xmlContent);

            Assert.NotNull(instruments);
            Assert.NotEmpty(instruments!.Instruments);
        }

        [Fact]
        public async Task SettlementTrackInstruments_Roundtrip_ShouldPreserveAllData()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_track_instruments.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var instruments = XmlTestUtils.Deserialize<SettlementTrackInstruments>(originalXml);

            var serializedXml = XmlTestUtils.Serialize(instruments, originalXml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public async Task SettlementTrackInstruments_ShouldHaveCorrectInstrumentCount()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_track_instruments.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var instruments = XmlTestUtils.Deserialize<SettlementTrackInstruments>(xmlContent);

            Assert.NotNull(instruments);
            Assert.True(instruments!.Instruments.Count > 0);
        }

        [Fact]
        public async Task SettlementTrackInstruments_FirstInstrument_ShouldHaveCorrectProperties()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_track_instruments.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var instruments = XmlTestUtils.Deserialize<SettlementTrackInstruments>(xmlContent);

            var firstInstrument = instruments!.Instruments.First();
            Assert.Equal("nay", firstInstrument.Id);
            Assert.Equal("act_play_nay_sitting", firstInstrument.SittingAction);
            Assert.Equal("act_play_nay_stand", firstInstrument.StandingAction);
            Assert.Equal("musician", firstInstrument.Tag);
        }

        [Fact]
        public async Task SettlementTrackInstruments_InstrumentWithEntities_ShouldBeParsed()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "settlement_track_instruments.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var instruments = XmlTestUtils.Deserialize<SettlementTrackInstruments>(xmlContent);

            var harpInstrument = instruments!.Instruments.First(i => i.Id == "harp");
            Assert.NotNull(harpInstrument.Entities);
            Assert.NotEmpty(harpInstrument.Entities!.EntityList);
            Assert.Equal("musical_instrument_harp", harpInstrument.Entities.EntityList[0].Name);
            Assert.Equal("Spine1", harpInstrument.Entities.EntityList[0].Bone);
        }
    }
}
