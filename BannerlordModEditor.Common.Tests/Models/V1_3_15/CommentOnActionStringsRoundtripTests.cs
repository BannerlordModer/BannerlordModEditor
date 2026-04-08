using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class CommentOnActionStringsRoundtripTests
    {
        [Fact]
        public async Task CommentOnActionStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "comment_on_action_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<CommentOnActionStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task CommentOnActionStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "comment_on_action_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<CommentOnActionStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task CommentOnActionStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "comment_on_action_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var commentOnActionStrings = XmlTestUtils.Deserialize<CommentOnActionStrings>(xmlContent);

            Assert.NotNull(commentOnActionStrings);
            Assert.NotNull(commentOnActionStrings.Strings);
            Assert.True(commentOnActionStrings.Strings.Count > 0);
        }
    }
}
