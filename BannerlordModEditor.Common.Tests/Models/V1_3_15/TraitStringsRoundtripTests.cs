using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class TraitStringsRoundtripTests
    {
        [Fact]
        public async Task TraitStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "trait_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<TraitStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task TraitStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "trait_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<TraitStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task TraitStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "trait_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var traitStrings = XmlTestUtils.Deserialize<TraitStrings>(xmlContent);

            Assert.NotNull(traitStrings);
            Assert.NotNull(traitStrings.Strings);
            Assert.True(traitStrings.Strings.Count > 0);
        }
    }
}
