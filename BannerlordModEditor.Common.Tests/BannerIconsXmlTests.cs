using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class BannerIconsXmlTests
    {
        [Fact]
        public void BannerIcons_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/banner_icons.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<BannerIconsRoot>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}