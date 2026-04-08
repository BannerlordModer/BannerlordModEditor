using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class ConversationAnimationsRoundtripTests
    {
        [Fact]
        public async Task ConversationAnimations_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "conversation_animations.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ConversationAnimations>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task ConversationAnimations_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "conversation_animations.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<ConversationAnimations>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task ConversationAnimations_ShouldHaveAnimationElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "conversation_animations.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var conversationAnimations = XmlTestUtils.Deserialize<ConversationAnimations>(xmlContent);

            Assert.NotNull(conversationAnimations);
            Assert.NotNull(conversationAnimations.AnimationList);
            Assert.True(conversationAnimations.AnimationList.Count > 0);
        }
    }
}
