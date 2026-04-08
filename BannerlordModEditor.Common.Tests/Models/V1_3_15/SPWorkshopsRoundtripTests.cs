using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class SPWorkshopsRoundtripTests
    {
        [Fact]
        public async Task SPWorkshops_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spworkshops.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<SPWorkshops>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task SPWorkshops_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spworkshops.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<SPWorkshops>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task SPWorkshops_ShouldHaveWorkshopTypeElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "spworkshops.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var workshops = XmlTestUtils.Deserialize<SPWorkshops>(xmlContent);

            Assert.NotNull(workshops);
            Assert.NotNull(workshops.WorkshopTypeList);
            Assert.True(workshops.WorkshopTypeList.Count > 0);
        }
    }
}
