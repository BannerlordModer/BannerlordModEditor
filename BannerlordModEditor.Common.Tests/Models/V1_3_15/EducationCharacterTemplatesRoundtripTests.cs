using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class EducationCharacterTemplatesRoundtripTests
    {
        [Fact]
        public async Task EducationCharacterTemplates_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_character_templates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<EducationCharacterTemplates>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task EducationCharacterTemplates_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_character_templates.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<EducationCharacterTemplates>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task EducationCharacterTemplates_ShouldHaveNPCCharacterElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_character_templates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var templates = XmlTestUtils.Deserialize<EducationCharacterTemplates>(xmlContent);

            Assert.NotNull(templates);
            Assert.NotNull(templates.NPCCharacterList);
            Assert.True(templates.NPCCharacterList.Count > 0);
        }
    }
}
