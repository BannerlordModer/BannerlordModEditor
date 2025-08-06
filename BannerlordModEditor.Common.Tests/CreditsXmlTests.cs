using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/Credits.xml";

        [Fact]
        public void CreditsXml_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<CreditsXmlModel>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}