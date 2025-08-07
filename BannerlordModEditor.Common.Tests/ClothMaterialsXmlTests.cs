using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ClothMaterialsXmlTests
    {
        private const string TestDataPath = "TestData/cloth_materials.xml";

        [Fact]
        public void ClothMaterials_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var obj = XmlTestUtils.Deserialize<ClothMaterialsRoot>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}