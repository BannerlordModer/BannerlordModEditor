using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class TauntUsageSetsXmlTests
    {
        private const string TestDataPath = "TestData/taunt_usage_sets.xml";

        [Fact]
        public void TauntUsageSets_RoundTrip_StructuralEquality()
        {
            var xml = XmlTestUtils.ReadTestDataOrSkip(TestDataPath);
            if (xml == null)
            {
                return; // 跳过测试，如果文件不存在
            }

            // 反序列化
            var model = XmlTestUtils.Deserialize<TauntUsageSetsDO>(xml);

            // 再序列化
            var serialized = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}