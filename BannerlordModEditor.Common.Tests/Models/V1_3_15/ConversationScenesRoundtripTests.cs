using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class ConversationScenesRoundtripTests
    {
        [Fact]
        public async Task ConversationScenes_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "conversation_scenes.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ConversationScenes>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task ConversationScenes_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "conversation_scenes.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ConversationScenes>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task ConversationScenes_ShouldHaveSceneElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "conversation_scenes.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var conversationScenes = XmlTestUtils.Deserialize<ConversationScenes>(xmlContent);

            Assert.NotNull(conversationScenes);
            Assert.NotNull(conversationScenes.SceneList);
            Assert.True(conversationScenes.SceneList.Count > 0);
        }
    }
}
