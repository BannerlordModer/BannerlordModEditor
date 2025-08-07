using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalTexturesWorldmapXmlTests
    {
        [Fact]
        public void DecalTexturesWorldmap_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/decal_textures_worldmap.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<DecalTexturesWorldmap>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}