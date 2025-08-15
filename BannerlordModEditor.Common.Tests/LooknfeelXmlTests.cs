using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class LooknfeelXmlTests
    {
        [Fact]
        public void Looknfeel_Roundtrip_StructuralEquality()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化为DO
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);

            // 转换为DTO用于精确的XML序列化控制
            var dto = LooknfeelMapper.ToDTO(obj);

            // 再序列化（使用DTO进行序列化以确保XML结构完整性）
            var xml2 = XmlTestUtils.Serialize(dto, xml);

            // 结构化对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            var attributeValueDiffs = diff.AttributeValueDifferences != null ? string.Join(", ", diff.AttributeValueDifferences) : "";
            var textDiffs = diff.TextDifferences != null ? string.Join(", ", diff.TextDifferences) : "";
            Assert.True(diff.IsStructurallyEqual, $"Looknfeel XML结构不一致。节点差异: {diff.NodeCountDifference}, 属性差异: {diff.AttributeCountDifference}, 属性值差异: {attributeValueDiffs}, 文本差异: {textDiffs}");
        }

        [Fact]
        public void Looknfeel_DO_TO_DTO_Mapping_Test()
        {
            var xmlPath = "TestData/looknfeel.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化为DO
            var obj = XmlTestUtils.Deserialize<LooknfeelDO>(xml);

            // DO -> DTO -> DO 转换测试
            var dto = LooknfeelMapper.ToDTO(obj);
            var obj2 = LooknfeelMapper.ToDO(dto);

            // 验证转换后的对象仍然可以正确序列化
            var xml2 = XmlTestUtils.Serialize(obj2, xml);

            // 基本结构对比
            var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
            Assert.True(string.IsNullOrEmpty(diff.NodeCountDifference), $"DO/DTO转换后节点数量不一致: {diff.NodeCountDifference}");
        }
    }
}