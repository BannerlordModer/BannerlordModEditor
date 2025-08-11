using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

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
            var obj = XmlTestUtils.Deserialize<ActionSetsDO>(xml);

            // 再序列化
            var serialized = XmlTestUtils.Serialize(obj);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, serialized);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            
            Assert.True(diff.IsStructurallyEqual, 
                $"ActionSets XML结构不一致。节点差异: {diff.NodeCountDifference}, " +
                $"属性差异: {diff.AttributeCountDifference}, " +
                $"属性值差异: {attributeValueDiffs}, " +
                $"文本差异: {textDiffs}");
        }
    }
}