using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class AdjustablesXmlTests
    {
        private const string TestDataPath = "TestData/Adjustables.xml";

        [Fact]
        public void Adjustables_Xml_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<AdjustablesDataModel>(xml);

            // 再序列化
            var serialized = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}