using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class AdjustablesXmlTests
    {
        private const string TestDataPath = "TestData/Adjustables.xml";

        [Fact]
        public void Adjustables_Xml_RoundTrip_StructuralEquality()
        {
            var xml = XmlTestUtils.ReadTestDataOrSkip(TestDataPath);
            if (xml == null) return; // 文件不存在则跳过

            // 反序列化
            var model = XmlTestUtils.Deserialize<AdjustablesDO>(xml);

            // 再序列化
            var serialized = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}