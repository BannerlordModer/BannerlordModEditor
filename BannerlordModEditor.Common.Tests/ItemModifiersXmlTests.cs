using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemModifiersXmlTests
    {
        private const string TestDataPath = "TestData/item_modifiers.xml";

        [Fact]
        public void ItemModifiers_RoundTrip_NodeAndAttributeCount()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ItemModifiers>(xml);
            var serialized = XmlTestUtils.Serialize(model);

            // 输出原始 XML 节点和属性
            XmlTestUtils.LogAllNodesAndAttributes(xml, "原始XML");
            // 输出序列化后 XML 节点和属性
            XmlTestUtils.LogAllNodesAndAttributes(serialized, "序列化XML");

            var (origNodeCount, origAttrCount) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (serNodeCount, serAttrCount) = XmlTestUtils.CountNodesAndAttributes(serialized);

            Assert.Equal(origNodeCount, serNodeCount);
            Assert.Equal(origAttrCount, serAttrCount);

            // 检查序列化后没有属性从无变为 null
            Assert.True(XmlTestUtils.NoNewNullAttributes(xml, serialized));
        }
    }
}