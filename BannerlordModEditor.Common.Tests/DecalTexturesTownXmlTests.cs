using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalTexturesTownXmlTests
    {
        [Fact]
        public void DecalTexturesTown_RoundTrip_StructuralEquality()
        {
            var xmlPath = "BannerlordModEditor.Common.Tests/TestData/decal_textures_town.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<DecalTexturesTown>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}