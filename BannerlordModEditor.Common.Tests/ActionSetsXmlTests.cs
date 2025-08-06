using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionSetsXmlTests
    {
        [Fact]
        public void ActionSets_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/action_sets.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ActionSetsData>(xml);

            // 再序列化
            var serialized = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}