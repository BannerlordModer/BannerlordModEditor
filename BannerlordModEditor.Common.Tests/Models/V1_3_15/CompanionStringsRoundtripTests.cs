using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class CompanionStringsRoundtripTests
    {
        [Fact]
        public async Task CompanionStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "companion_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<CompanionStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task CompanionStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "companion_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<CompanionStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task CompanionStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "companion_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var companionStrings = XmlTestUtils.Deserialize<CompanionStrings>(xmlContent);

            Assert.NotNull(companionStrings);
            Assert.NotNull(companionStrings.Strings);
            Assert.True(companionStrings.Strings.Count > 0);
        }
    }
}
