using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class WorldLoreStringsRoundtripTests
    {
        [Fact]
        public async Task WorldLoreStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "world_lore_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<WorldLoreStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task WorldLoreStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "world_lore_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<WorldLoreStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task WorldLoreStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "world_lore_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var worldLoreStrings = XmlTestUtils.Deserialize<WorldLoreStrings>(xmlContent);

            Assert.NotNull(worldLoreStrings);
            Assert.NotNull(worldLoreStrings.Strings);
            Assert.True(worldLoreStrings.Strings.Count > 0);
        }
    }
}
