using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ClothBodiesXmlTests
    {
        [Fact]
        public void ClothBodies_RoundTrip_StructuralEquality()
        {
            // 读取原始 XML
            var xmlPath = Path.Combine("TestData", "cloth_bodies.xml");
            var xml = File.ReadAllText(xmlPath);

            // 反序列化为模型对象
            var model = XmlTestUtils.Deserialize<ClothBodiesModel>(xml);

            // 再序列化为字符串
            var xmlRoundTrip = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xmlRoundTrip));
        }
    }
}