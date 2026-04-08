using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class ActionStringsRoundtripTests
    {
        [Fact]
        public async Task ActionStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "action_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ActionStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task ActionStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "action_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ActionStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task ActionStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "action_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var actionStrings = XmlTestUtils.Deserialize<ActionStrings>(xmlContent);

            Assert.NotNull(actionStrings);
            Assert.NotNull(actionStrings.StringList);
            Assert.True(actionStrings.StringList.Count > 0);
        }
    }
}
