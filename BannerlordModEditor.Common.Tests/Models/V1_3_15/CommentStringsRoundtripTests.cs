using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.V1_3_15;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.V1_3_15
{
    public class CommentStringsRoundtripTests
    {
        [Fact]
        public async Task CommentStrings_Deserialize_ShouldWorkCorrectly()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "comment_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<CommentStrings>(xmlContent);
            Assert.NotNull(obj);
        }

        [Fact]
        public async Task CommentStrings_Roundtrip_ShouldPreserveData()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "comment_strings.xml");
            var originalXml = await File.ReadAllTextAsync(xmlPath);

            var obj = XmlTestUtils.Deserialize<CommentStrings>(originalXml);
            var serialized = XmlTestUtils.Serialize(obj, originalXml);
            
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serialized));
        }

        [Fact]
        public async Task CommentStrings_ShouldHaveStringElements()
        {
            var xmlPath = Path.Combine("TestData", "V1_3_15", "comment_strings.xml");
            var xmlContent = await File.ReadAllTextAsync(xmlPath);

            var commentStrings = XmlTestUtils.Deserialize<CommentStrings>(xmlContent);

            Assert.NotNull(commentStrings);
            Assert.NotNull(commentStrings.Strings);
            Assert.True(commentStrings.Strings.Count > 0);
        }
    }
}
