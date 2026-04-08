using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class HeroesRoundtripTests
    {
        [Fact]
        public async Task Heroes_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "heroes.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<Heroes>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task Heroes_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "heroes.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<Heroes>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task Heroes_ShouldHaveCharacterElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "heroes.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var characters = XmlTestUtils.Deserialize<Heroes>(xmlContent);

            Assert.NotNull(characters);
            Assert.NotNull(characters.HeroList);
            Assert.True(characters.HeroList.Count > 0);
        }
    }
}
