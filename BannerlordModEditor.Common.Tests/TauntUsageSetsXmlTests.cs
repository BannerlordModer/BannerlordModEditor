using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class TauntUsageSetsXmlTests
    {
        private const string TestDataPath = "BannerlordModEditor.Common.Tests/TestData/taunt_usage_sets.xml";

        [Fact]
        public void TauntUsageSets_RoundTrip_StructuralEquality()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<TauntUsageSets>(xml);

            // 再序列化
            var serialized = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}