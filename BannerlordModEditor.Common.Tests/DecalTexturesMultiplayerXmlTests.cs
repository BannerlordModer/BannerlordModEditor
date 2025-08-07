using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalTexturesMultiplayerXmlTests
    {
        [Fact]
        public void DecalTexturesMultiplayer_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/decal_textures_multiplayer.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<DecalTexturesMultiplayer>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}