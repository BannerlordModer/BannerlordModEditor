using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CombatParametersXmlTests
    {
        [Fact]
        public void CombatParameters_RoundTrip_StructuralEquality()
        {
            var xmlPath = "BannerlordModEditor.Common.Tests/TestData/combat_parameters.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParameters>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }

        [Theory]
        [InlineData("BannerlordModEditor.Common.Tests/TestData/combat_parameters_part1.xml")]
        [InlineData("BannerlordModEditor.Common.Tests/TestData/combat_parameters_part2.xml")]
        public void CombatParameters_PartFiles_RoundTrip_StructuralEquality(string xmlPath)
        {
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<CombatParameters>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}