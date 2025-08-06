using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;
using BannerlordModEditor.Common.Tests.Services;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionSetsXmlTests
    {
        private static string ReadTestData(string fileName)
        {
            var path = Path.Combine("TestData", fileName);
            return File.ReadAllText(path);
        }

        [Theory]
        [InlineData("action_sets.xml")]
        [InlineData("action_sets_part1.xml")]
        public void ActionSets_RoundTrip_StructuralEquality(string fileName)
        {
            var xml = ReadTestData(fileName);
            var obj = XmlTestUtils.Deserialize<ActionSets>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2), $"结构不一致: {fileName}");
        }
    }
}