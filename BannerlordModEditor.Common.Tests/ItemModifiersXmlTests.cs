using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemModifiersXmlTests
    {
        private const string TestDataPath = "TestData/item_modifiers.xml";

        [Fact]
        public void ItemModifiers_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ItemModifiers>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}