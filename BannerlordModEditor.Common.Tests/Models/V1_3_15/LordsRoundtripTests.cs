using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class LordsRoundtripTests
    {
        [Fact]
        public async Task Lords_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "lords.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<NpcCharacters>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task Lords_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "lords.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<NpcCharacters>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task Lords_ShouldHaveNpcCharacterElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "lords.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var npcCharacters = XmlTestUtils.Deserialize<NpcCharacters>(xmlContent);

            Assert.NotNull(npcCharacters);
            Assert.NotNull(npcCharacters.NpcCharacterList);
            Assert.True(npcCharacters.NpcCharacterList.Count > 0);
        }
    }
}
