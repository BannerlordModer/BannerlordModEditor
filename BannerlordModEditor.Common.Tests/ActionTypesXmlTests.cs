using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionTypesXmlTests
    {
        private const string MainXmlPath = "BannerlordModEditor.Common.Tests/TestData/action_types.xml";
        private const string SubsetDir = "BannerlordModEditor.Common.Tests/TestSubsets/ActionTypes/";

        [Fact]
        public void ActionTypes_MainXml_RoundTrip_StructuralEquality()
        {
            string xml = File.ReadAllText(MainXmlPath);
            var obj = XmlTestUtils.Deserialize<ActionTypesRoot>(xml);
            string serialized = XmlTestUtils.Serialize(obj);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Theory]
        [MemberData(nameof(GetSubsetFiles))]
        public void ActionTypes_SubsetXmls_RoundTrip_StructuralEquality(string subsetPath)
        {
            string xml = File.ReadAllText(subsetPath);
            var obj = XmlTestUtils.Deserialize<ActionTypesRoot>(xml);
            string serialized = XmlTestUtils.Serialize(obj);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        public static System.Collections.Generic.IEnumerable<object[]> GetSubsetFiles()
        {
            if (!Directory.Exists(SubsetDir))
                yield break;

            var files = Directory.GetFiles(SubsetDir, "*.xml");
            foreach (var file in files.OrderBy(f => f))
            {
                yield return new object[] { file };
            }
        }
    }
}