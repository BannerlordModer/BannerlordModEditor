using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_2_9;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_2_9
{
    public class SpClansRoundtripTests
    {
        [Fact]
        public async Task SpClans_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            Assert.NotNull(spClans);
            Assert.NotEmpty(spClans!.Factions);
        }

        [Fact]
        public async Task SpClans_Roundtrip_ShouldPreserveAllData()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spclans.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(originalXml);

            var serializedXml = XmlTestUtils.Serialize(spClans, originalXml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public async Task SpClans_ShouldHaveCorrectFactionCount()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            Assert.NotNull(spClans);
            Assert.True(spClans!.Factions.Count > 0);
            Assert.Contains(spClans.Factions, f => f.Id == "player_faction");
            Assert.Contains(spClans.Factions, f => f.Id == "looters");
        }

        [Fact]
        public async Task SpClans_FactionProperties_ShouldBePreserved()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            var playerFaction = spClans!.Factions.First(f => f.Id == "player_faction");
            Assert.Equal("Hero.main_hero", playerFaction.Owner);
            Assert.Equal("Culture.battania", playerFaction.Culture);
            Assert.Equal("{=o0Xm5Rqk}Playerland", playerFaction.Name);
            Assert.Equal("0", playerFaction.Tier);
            Assert.Equal("true", playerFaction.IsMinorFaction);

            var lootersFaction = spClans.Factions.First(f => f.Id == "looters");
            Assert.Equal("true", lootersFaction.IsBandit);
            Assert.Equal("true", lootersFaction.IsOutlaw);
            Assert.Equal("PartyTemplate.looters_template", lootersFaction.DefaultPartyTemplate);
        }

        [Fact]
        public async Task SpClans_BanditFactions_ShouldBeParsed()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spclans.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var spClans = XmlTestUtils.Deserialize<SpClans>(xmlContent);

            var banditIds = new[] { "looters", "sea_raiders", "mountain_bandits", "forest_bandits", "desert_bandits", "steppe_bandits" };
            foreach (var id in banditIds)
            {
                Assert.Contains(spClans!.Factions, f => f.Id == id);
            }
        }
    }
}
