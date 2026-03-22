using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class EducationEquipmentTemplatesRoundtripTests
    {
        [Fact]
        public async Task EducationEquipmentTemplates_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "education_equipment_templates.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<EducationEquipmentTemplates>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }
    }
}
