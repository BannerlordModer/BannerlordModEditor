using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemHolstersXmlTests
    {
        [Fact]
        public void ItemHolsters_RoundTrip_StructuralEquality()
        {
            var xmlPath = "BannerlordModEditor.Common.Tests/TestData/item_holsters.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ItemHolsters>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}