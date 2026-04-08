using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class MeetingScenesRoundtripTests
    {
        [Fact]
        public async Task MeetingScenes_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "meeting_scenes.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<MeetingScenes>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task MeetingScenes_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "meeting_scenes.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<MeetingScenes>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task MeetingScenes_ShouldHaveSceneElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "meeting_scenes.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var meetingScenes = XmlTestUtils.Deserialize<MeetingScenes>(xmlContent);

            Assert.NotNull(meetingScenes);
            Assert.NotNull(meetingScenes.SceneList);
            Assert.True(meetingScenes.SceneList.Count > 0);
        }
    }
}
