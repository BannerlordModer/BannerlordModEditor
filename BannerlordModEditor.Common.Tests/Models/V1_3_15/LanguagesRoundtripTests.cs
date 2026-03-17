using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class LanguagesRoundtripTests
    {
        [Fact]
        public async Task StdSandBox_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_SandBox.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdSandBoxGauntletUI_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_SandBox_GauntletUI.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdTaleWorldsCampaignSystem_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_TaleWorlds_CampaignSystem.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdActionStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_action_strings_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdCommentStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_comment_strings_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdWandererStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_wanderer_strings_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdSettlementsStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_settlements_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdCompanionStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_companion_strings_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdConceptStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_concept_strings_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdLordsStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_lords_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdSpClansStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_spclans_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }

        [Fact]
        public async Task StdVoiceStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "Languages", "std_voice_strings_xml.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<LanguageBaseDO>(xmlContent);
            var serialized = XmlTestUtils.Serialize(obj, xmlContent);
            var result = XmlTestUtils.AreStructurallyEqual(xmlContent, serialized);

            Assert.True(result);
        }
    }
}
