using System.IO;
using System.Reflection;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class CollisionInfosXmlTests
    {
        private static string TestDataPath
        {
            get
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var assemblyDir = Path.GetDirectoryName(assemblyLocation);
                return Path.Combine(assemblyDir, "TestData", "collision_infos.xml");
            }
        }

        [Fact]
        public void CollisionInfos_CanBeDeserializedAndSerialized_StructurallyEqual()
        {
            var xml = File.ReadAllText(TestDataPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<CollisionInfosRootDO>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model);

            // 结构化对比
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
    }
}