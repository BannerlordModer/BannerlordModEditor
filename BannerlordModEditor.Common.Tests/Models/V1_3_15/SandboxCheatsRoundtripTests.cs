using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class SandboxCheatsRoundtripTests
    {
        [Fact]
        public async Task SandboxCheats_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "sandbox_cheats.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);
            var obj = XmlTestUtils.Deserialize<SandboxCheats>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }
    }
}
