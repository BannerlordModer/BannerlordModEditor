using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class SpClansRoundtripTests
    {
        [Fact]
        public async Task SpClans_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            Assert.NotNull(spClans);
            Assert.NotEmpty(spClans!.Factions);
        }

        [Fact]
        public async Task SpClans_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spclans.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(originalXml);
            var serialized = XmlTestUtils.Serialize(spClans, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task SpClans_ShouldHaveCorrectFactionCount()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            Assert.NotNull(spClans);
            Assert.True(spClans!.Factions.Count > 0);
            Assert.Contains(spClans.Factions, f => f.Id == "player_faction");
        }

        [Fact]
        public async Task SpClans_PlayerFaction_ShouldHaveExpectedProperties()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            var playerFaction = spClans!.Factions.First(f => f.Id == "player_faction");
            Assert.NotNull(playerFaction.Owner);
            Assert.NotNull(playerFaction.Culture);
        }
    }
}
