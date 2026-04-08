using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class EducationEquipmentTemplatesRoundtripTests
    {
        [Fact]
        public async Task EducationEquipmentTemplates_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_equipment_templates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<EducationEquipmentTemplates>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task EducationEquipmentTemplates_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_equipment_templates.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<EducationEquipmentTemplates>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task EducationEquipmentTemplates_ShouldHaveRosterElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_equipment_templates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var templates = XmlTestUtils.Deserialize<EducationEquipmentTemplates>(xmlContent);

            Assert.NotNull(templates);
            Assert.NotNull(templates.EquipmentRosterList);
            Assert.True(templates.EquipmentRosterList.Count > 0);
        }
    }
}
