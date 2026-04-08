using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_2_9;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_2_9
{
    public class SPProjectsRoundtripTests
    {
        [Fact]
        public async Task SPProjects_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spprojects.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<SPProjects>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task SPProjects_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_2_9", "spprojects.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<SPProjects>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }
    }
}
