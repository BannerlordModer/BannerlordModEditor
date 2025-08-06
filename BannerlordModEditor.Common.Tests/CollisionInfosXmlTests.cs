using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class CollisionInfosXmlTests
    {
        private static string TestDataPath =>
            Path.Combine("BannerlordModEditor.Common.Tests", "TestData", "collision_infos.xml");

        [Fact]
        public void CollisionInfos_CanBeDeserializedAndSerialized_StructurallyEqual()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<CollisionInfosRoot>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}