using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraLayerSetsXmlTests
    {
        [Fact]
        public void FloraLayerSets_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/flora_layer_sets.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<FloraLayerSetsDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}