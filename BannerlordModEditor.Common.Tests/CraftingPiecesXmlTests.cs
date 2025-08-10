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
            bool areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            if (!areEqual)
            {
                // 详细比较
                var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
                
                // 输出节点和属性统计
                var stats1 = XmlTestUtils.CountNodesAndAttributes(xml);
                var stats2 = XmlTestUtils.CountNodesAndAttributes(xml2);
                
                // 输出调试信息
                Console.WriteLine($"原始XML: 节点数={stats1.nodeCount}, 属性数={stats1.attrCount}");
                Console.WriteLine($"序列化XML: 节点数={stats2.nodeCount}, 属性数={stats2.attrCount}");
                
                if (!string.IsNullOrEmpty(diff.NodeCountDifference))
                    Console.WriteLine($"节点数量差异: {diff.NodeCountDifference}");
                if (!string.IsNullOrEmpty(diff.AttributeCountDifference))
                    Console.WriteLine($"属性数量差异: {diff.AttributeCountDifference}");
                
                foreach (var missing in diff.MissingNodes)
                    Console.WriteLine($"缺失节点: {missing}");
                foreach (var extra in diff.ExtraNodes)
                    Console.WriteLine($"额外节点: {extra}");
                foreach (var missingAttr in diff.MissingAttributes)
                    Console.WriteLine($"缺失属性: {missingAttr}");
                foreach (var extraAttr in diff.ExtraAttributes)
                    Console.WriteLine($"额外属性: {extraAttr}");
                foreach (var valueDiff in diff.AttributeValueDifferences)
                    Console.WriteLine($"属性值差异: {valueDiff}");
                
                // 保存差异文件用于调试
                File.WriteAllText("original_crafting_pieces.xml", xml);
                File.WriteAllText("serialized_crafting_pieces.xml", xml2);
            }
            
            Assert.True(areEqual, $"XML结构不匹配，详细差异请查看控制台输出");
        }
    }
}