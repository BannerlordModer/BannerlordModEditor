using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Language;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class LanguageDataRoundtripTests
    {
        [Fact]
        public async Task LanguageData_1_3_15_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "language_data.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var languageData = XmlTestUtils.Deserialize<LanguageDataDO>(xmlContent);

            Assert.NotNull(languageData);
            Assert.Equal("English", languageData.Id);
        }

        [Fact]
        public async Task LanguageData_1_3_15_ShouldHaveVoiceFile()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "language_data.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var languageData = XmlTestUtils.Deserialize<LanguageDataDO>(xmlContent);

            Assert.NotNull(languageData.VoiceFiles);
            Assert.NotEmpty(languageData.VoiceFiles);
            Assert.Equal("VoicedLines/sandbox_voiced_lines_en.xml", languageData.VoiceFiles[0].XmlPath);
        }
    }
}
