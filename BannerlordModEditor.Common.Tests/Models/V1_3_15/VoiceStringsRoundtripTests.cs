using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class VoiceStringsRoundtripTests
    {
        [Fact]
        public async Task VoiceStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "voice_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<VoiceStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task VoiceStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "voice_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<VoiceStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task VoiceStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "voice_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var voiceStrings = XmlTestUtils.Deserialize<VoiceStrings>(xmlContent);

            Assert.NotNull(voiceStrings);
            Assert.NotNull(voiceStrings.Strings);
            Assert.True(voiceStrings.Strings.Count > 0);
        }
    }
}
