using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalTexturesBattleXmlTests
    {
        [Fact]
        public void DecalTexturesBattle_RoundTrip_StructuralEquality()
        {
            var xmlPath = "BannerlordModEditor.Common.Tests/TestData/decal_textures_battle.xml";
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<DecalTexturesBattle>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}