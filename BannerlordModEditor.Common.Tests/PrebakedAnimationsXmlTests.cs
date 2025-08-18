using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class PrebakedAnimationsXmlTests
    {
        [Fact]
        public void PrebakedAnimations_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/prebaked_animations.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<PrebakedAnimationsDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}