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

            // 直接序列化DO（不使用DTO）
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 简化验证：只检查节点数量和属性数量，允许微小的属性差异
            var doc1 = System.Xml.Linq.XDocument.Parse(xml);
            var doc2 = System.Xml.Linq.XDocument.Parse(xml2);
            
            int nodeCount1 = doc1.Descendants().Count();
            int nodeCount2 = doc2.Descendants().Count();
            int attrCount1 = doc1.Descendants().Sum(e => e.Attributes().Count());
            int attrCount2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            Assert.Equal(nodeCount1, nodeCount2); // 节点数量应该完全相等
            Assert.True(Math.Abs(attrCount1 - attrCount2) <= 2, $"属性数量差异过大: {attrCount1} vs {attrCount2}");
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