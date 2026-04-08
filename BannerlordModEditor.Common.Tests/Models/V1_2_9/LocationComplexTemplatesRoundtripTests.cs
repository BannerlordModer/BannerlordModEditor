using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_2_9;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_2_9
{
    public class LocationComplexTemplatesRoundtripTests
    {
        [Fact]
        public async Task LocationComplexTemplates_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "location_complex_templates.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LocationComplexTemplates>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task LocationComplexTemplates_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "location_complex_templates.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LocationComplexTemplates>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }
    }
}
