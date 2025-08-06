using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class PrerenderXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/prerender.xml";

        [Fact]
        public void Prerender_Xml_Serialization_Roundtrip_Should_Be_Structurally_Equal()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<Prerender>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}