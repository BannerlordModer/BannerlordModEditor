using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CraftingPiecesXmlTests
    {
        private const string TestDataPath = "TestData/crafting_pieces.xml";

        [Fact]
        public void CraftingPieces_RoundTrip_StructuralEquality()
        {
            // 反序列化
            var xml = File.ReadAllText(TestDataPath);
            var obj = XmlTestUtils.Deserialize<CraftingPieces>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}