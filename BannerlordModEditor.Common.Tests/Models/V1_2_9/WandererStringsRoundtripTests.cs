using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_2_9;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_2_9
{
    public class WandererStringsRoundtripTests
    {
        [Fact]
        public async Task WandererStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "wanderer_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<WandererStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task WandererStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "wanderer_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<WandererStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }
    }
}
