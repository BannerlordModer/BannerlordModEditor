using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MultiplayerScenesXmlTests
    {
        private const string TestDataPath = "TestData/MultiplayerScenes.xml";

        [Fact]
        public void MultiplayerScenes_RoundTrip_StructuralEquality()
        {
            // 读取原始 XML
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化为模型对象
            var obj = XmlTestUtils.Deserialize<MultiplayerScenes>(xml);

            // 再序列化为字符串
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}