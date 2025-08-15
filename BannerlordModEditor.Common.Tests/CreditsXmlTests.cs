using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsXmlTests
    {
        private const string TestDataPath = "TestData/Credits.xml";

        [Fact]
        public void CreditsXml_RoundTrip_StructuralEquality()
        {
            var xml = XmlTestUtils.ReadTestDataOrSkip(TestDataPath);
            if (xml == null) return;

            // 反序列化
            var model = XmlTestUtils.Deserialize<CreditsDO>(xml);

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(model, xml);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            Assert.True(diff.IsStructurallyEqual, $"Credits XML结构不一致。节点差异: {diff.NodeCountDifference}, 属性差异: {diff.AttributeCountDifference}, 属性值差异: {attributeValueDiffs}, 文本差异: {textDiffs}");
        }
    }
}