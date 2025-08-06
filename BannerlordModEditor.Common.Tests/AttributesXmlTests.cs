using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class AttributesXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/attributes.xml";

        [Fact]
        public void AttributesXml_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<AttributesDataModel>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}