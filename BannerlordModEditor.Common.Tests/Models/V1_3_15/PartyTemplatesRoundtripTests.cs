using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class PartyTemplatesRoundtripTests
    {
        [Fact]
        public async Task PartyTemplates_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "partyTemplates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<PartyTemplates>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task PartyTemplates_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "partyTemplates.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<PartyTemplates>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task PartyTemplates_ShouldHaveTemplateElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "partyTemplates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var templates = XmlTestUtils.Deserialize<PartyTemplates>(xmlContent);

            Assert.NotNull(templates);
            Assert.NotNull(templates.PartyTemplateList);
            Assert.True(templates.PartyTemplateList.Count > 0);
        }
    }
}
