using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemModifiersGroupsXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/item_modifiers_groups.xml";

        [Fact]
        public void ItemModifiersGroups_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ItemModifiersGroups>(xml);
            var serialized = XmlTestUtils.Serialize(model);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}