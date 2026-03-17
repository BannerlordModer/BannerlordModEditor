using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class SettlementsRoundtripTests
    {
        [Fact]
        public async Task Settlements_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "settlements.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<Settlements>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task Settlements_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "settlements.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<Settlements>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task Settlements_ShouldHaveSettlementElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "settlements.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var settlements = XmlTestUtils.Deserialize<Settlements>(xmlContent);

            Assert.NotNull(settlements);
            Assert.NotNull(settlements.SettlementList);
            Assert.True(settlements.SettlementList.Count > 0);
        }
    }
}
