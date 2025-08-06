using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphXmlTests
    {
        [Fact]
        public void BeforeTransparentsGraph_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/before_transparents_graph.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<BeforeTransparentsGraph>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}