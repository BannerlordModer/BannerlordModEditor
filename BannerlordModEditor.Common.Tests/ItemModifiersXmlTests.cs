using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemModifiersXmlTests
    {
        private const string TestDataPath = "TestData/item_modifiers.xml";

        [Fact]
        public void ItemModifiers_RoundTrip_NodeAndAttributeCount()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<ItemModifiersDO>(xml);
            var serialized = XmlTestUtils.Serialize(model, xml);

            // 输出原始 XML 节点和属性
            // XmlTestUtils.LogAllNodesAndAttributes(xml, "原始XML");
            // 输出序列化后 XML 节点和属性
            // XmlTestUtils.LogAllNodesAndAttributes(serialized, "序列化XML");

            var (origNodeCount, origAttrCount) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (serNodeCount, serAttrCount) = XmlTestUtils.CountNodesAndAttributes(serialized);

            // 添加详细分析
            AnalyzeItemModifiersDifferences(xml, serialized);
            
            // 调试输出
            System.Console.WriteLine($"=== 调试信息 ===");
            System.Console.WriteLine($"原始节点数: {origNodeCount}, 序列化节点数: {serNodeCount}");
            System.Console.WriteLine($"原始属性数: {origAttrCount}, 序列化属性数: {serAttrCount}");
            System.Console.WriteLine($"属性差异: {serAttrCount - origAttrCount}");

            Assert.Equal(origNodeCount, serNodeCount);
            Assert.Equal(origAttrCount, serAttrCount);

            // 回退参数，保留原始结构比较，后续可扩展为更智能比较
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
            Assert.True(XmlTestUtils.NoNewNullAttributes(xml, serialized));
        }

        private void AnalyzeItemModifiersDifferences(string original, string serialized)
        {
            var originalDoc = System.Xml.Linq.XDocument.Parse(original);
            var serializedDoc = System.Xml.Linq.XDocument.Parse(serialized);

            var originalNodes = originalDoc.Descendants().ToList();
            var serializedNodes = serializedDoc.Descendants().ToList();

            // 按元素名称分组统计
            var originalGroups = originalNodes.GroupBy(n => n.Name.LocalName)
                                            .ToDictionary(g => g.Key, g => g.Count());
            var serializedGroups = serializedNodes.GroupBy(n => n.Name.LocalName)
                                              .ToDictionary(g => g.Key, g => g.Count());

            // 输出统计信息
            System.Console.WriteLine("=== ItemModifiers 节点差异分析 ===");
            System.Console.WriteLine($"原始节点总数: {originalNodes.Count}");
            System.Console.WriteLine($"序列化节点总数: {serializedNodes.Count}");
            System.Console.WriteLine($"节点差异: {serializedNodes.Count - originalNodes.Count}");

            System.Console.WriteLine("\n=== 按元素名称统计 ===");
            foreach (var name in originalGroups.Keys.Concat(serializedGroups.Keys)
                                                  .Distinct()
                                                  .OrderBy(n => n))
            {
                int origCount = originalGroups.GetValueOrDefault(name);
                int serCount = serializedGroups.GetValueOrDefault(name);
                if (origCount != serCount)
                {
                    System.Console.WriteLine($"{name}: 原始={origCount}, 序列化={serCount}, 差异={serCount - origCount}");
                }
            }

            // 检查XML声明差异
            var originalDecl = originalDoc.Declaration;
            var serializedDecl = serializedDoc.Declaration;
            if (originalDecl != null && serializedDecl != null)
            {
                System.Console.WriteLine("\n=== XML 声明比较 ===");
                System.Console.WriteLine($"原始: version={originalDecl.Version}, encoding={originalDecl.Encoding}, standalone={originalDecl.Standalone}");
                System.Console.WriteLine($"序列化: version={serializedDecl.Version}, encoding={serializedDecl.Encoding}, standalone={serializedDecl.Standalone}");
            }

            // 检查注释差异
            var originalComments = originalDoc.DescendantNodes().OfType<System.Xml.Linq.XComment>().Count();
            var serializedComments = serializedDoc.DescendantNodes().OfType<System.Xml.Linq.XComment>().Count();
            System.Console.WriteLine($"\n=== 注释比较 ===");
            System.Console.WriteLine($"原始注释数: {originalComments}");
            System.Console.WriteLine($"序列化注释数: {serializedComments}");
        }
    }
}