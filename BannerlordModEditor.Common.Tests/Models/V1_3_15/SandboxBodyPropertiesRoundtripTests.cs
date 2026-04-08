using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class SandboxBodyPropertiesRoundtripTests
    {
        [Fact]
        public async Task SandboxBodyProperties_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "sandbox_bodyproperties.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<SandboxBodyProperties>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task SandboxBodyProperties_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "sandbox_bodyproperties.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<SandboxBodyProperties>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task SandboxBodyProperties_ShouldHaveBodyPropertyElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "sandbox_bodyproperties.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var bodyProperties = XmlTestUtils.Deserialize<SandboxBodyProperties>(xmlContent);

            Assert.NotNull(bodyProperties);
            Assert.NotNull(bodyProperties.BodyPropertyList);
            Assert.True(bodyProperties.BodyPropertyList.Count > 0);
        }
    }
}
