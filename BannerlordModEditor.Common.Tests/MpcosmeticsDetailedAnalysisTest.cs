using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsDetailedAnalysisTest
    {
        private const string TestDataPath = "TestData/mpcosmetics.xml";

        [Fact]
        public void Analyze_Mpcosmetics_Item_vs_Itemless_Issue()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<MpcosmeticsDO>(xml);
            var serialized = XmlTestUtils.Serialize(model, xml);
            
            // 分析原始XML中的Item和Itemless元素
            var originalDoc = new XmlDocument();
            originalDoc.LoadXml(xml);
            
            var serializedDoc = new XmlDocument();
            serializedDoc.LoadXml(serialized);
            
            // 统计原始XML中的元素
            var originalItemNodes = originalDoc.GetElementsByTagName("Item").Cast<XmlNode>().ToList();
            var originalItemlessNodes = originalDoc.GetElementsByTagName("Itemless").Cast<XmlNode>().ToList();
            
            // 统计序列化XML中的元素
            var serializedItemNodes = serializedDoc.GetElementsByTagName("Item").Cast<XmlNode>().ToList();
            var serializedItemlessNodes = serializedDoc.GetElementsByTagName("Itemless").Cast<XmlNode>().ToList();
            
            Console.WriteLine($"=== 元素统计 ===");
            Console.WriteLine($"原始XML - Item元素: {originalItemNodes.Count}, Itemless元素: {originalItemlessNodes.Count}");
            Console.WriteLine($"序列化XML - Item元素: {serializedItemNodes.Count}, Itemless元素: {serializedItemlessNodes.Count}");
            
            // 分析模型中的数据
            int totalItems = 0;
            int totalItemless = 0;
            
            foreach (var cosmetic in model.Cosmetics)
            {
                if (cosmetic.Replace != null)
                {
                    totalItems += cosmetic.Replace.Items.Count;
                    totalItemless += cosmetic.Replace.ItemlessList.Count;
                }
            }
            
            Console.WriteLine($"模型数据 - Items: {totalItems}, Itemless: {totalItemless}");
            
            // 检查具体的差异
            if (originalItemNodes.Count != serializedItemNodes.Count || 
                originalItemlessNodes.Count != serializedItemlessNodes.Count)
            {
                Console.WriteLine($"=== 发现元素数量不匹配 ===");
                
                // 分析前几个Item元素
                Console.WriteLine($"\\n=== 原始Item元素示例 ===");
                for (int i = 0; i < Math.Min(3, originalItemNodes.Count); i++)
                {
                    var node = originalItemNodes[i];
                    Console.WriteLine($"Item {i+1}: {node.OuterXml}");
                }
                
                Console.WriteLine($"\\n=== 序列化Item元素示例 ===");
                for (int i = 0; i < Math.Min(3, serializedItemNodes.Count); i++)
                {
                    var node = serializedItemNodes[i];
                    Console.WriteLine($"Item {i+1}: {node.OuterXml}");
                }
                
                // 检查是否有Item被错误地序列化为Itemless
                Console.WriteLine($"\\n=== 检查Item/Itemless混淆 ===");
                var allOriginalItems = originalItemNodes.Select(n => n.Attributes?["id"]?.Value).Where(id => !string.IsNullOrEmpty(id)).ToList();
                var allSerializedItems = serializedItemNodes.Select(n => n.Attributes?["id"]?.Value).Where(id => !string.IsNullOrEmpty(id)).ToList();
                
                var missingItems = allOriginalItems.Except(allSerializedItems).ToList();
                var extraItems = allSerializedItems.Except(allOriginalItems).ToList();
                
                if (missingItems.Count > 0)
                {
                    Console.WriteLine($"缺失的Item IDs: {string.Join(", ", missingItems.Take(5))}");
                }
                if (extraItems.Count > 0)
                {
                    Console.WriteLine($"多余的Item IDs: {string.Join(", ", extraItems.Take(5))}");
                }
            }
            
            // 检查XML结构差异
            Console.WriteLine($"\\n=== XML结构分析 ===");
            Console.WriteLine($"原始XML根节点子元素数量: {originalDoc.DocumentElement.ChildNodes.Count}");
            Console.WriteLine($"序列化XML根节点子元素数量: {serializedDoc.DocumentElement.ChildNodes.Count}");
            
            // 计算实际的Cosmetic元素数量
            var originalCosmetics = originalDoc.DocumentElement.ChildNodes
                .Cast<XmlNode>()
                .Where(n => n.NodeType == XmlNodeType.Element && n.LocalName == "Cosmetic")
                .Count();
            var serializedCosmetics = serializedDoc.DocumentElement.ChildNodes
                .Cast<XmlNode>()
                .Where(n => n.NodeType == XmlNodeType.Element && n.LocalName == "Cosmetic")
                .Count();
            
            Console.WriteLine($"原始Cosmetic元素: {originalCosmetics}");
            Console.WriteLine($"序列化Cosmetic元素: {serializedCosmetics}");
            Console.WriteLine($"模型Cosmetic对象: {model.Cosmetics.Count}");
            
            // 临时断言，用于分析问题
            Assert.Equal(originalCosmetics, serializedCosmetics);
            Assert.Equal(originalCosmetics, model.Cosmetics.Count);
        }
        
        [Fact]
        public void Test_Mpcosmetics_Replace_Structure()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<MpcosmeticsDO>(xml);
            
            // 查找包含Replace的Cosmetic
            var cosmeticsWithReplace = model.Cosmetics.Where(c => c.Replace != null).ToList();
            Console.WriteLine($"包含Replace的Cosmetic数量: {cosmeticsWithReplace.Count}");
            
            // 分析Replace结构
            int replaceWithItems = 0;
            int replaceWithItemless = 0;
            int replaceWithBoth = 0;
            int replaceEmpty = 0;
            
            foreach (var cosmetic in cosmeticsWithReplace)
            {
                bool hasItems = cosmetic.Replace.Items.Count > 0;
                bool hasItemless = cosmetic.Replace.ItemlessList.Count > 0;
                
                if (hasItems && hasItemless)
                {
                    replaceWithBoth++;
                    Console.WriteLine($"Cosmetic {cosmetic.Id} 同时包含Items({cosmetic.Replace.Items.Count})和Itemless({cosmetic.Replace.ItemlessList.Count})");
                }
                else if (hasItems)
                {
                    replaceWithItems++;
                }
                else if (hasItemless)
                {
                    replaceWithItemless++;
                }
                else
                {
                    replaceEmpty++;
                }
            }
            
            Console.WriteLine($"Replace统计:");
            Console.WriteLine($"  只有Items: {replaceWithItems}");
            Console.WriteLine($"  只有Itemless: {replaceWithItemless}");
            Console.WriteLine($"  两者都有: {replaceWithBoth}");
            Console.WriteLine($"  空Replace: {replaceEmpty}");
            
            // 验证基本的反序列化是否正确
            Assert.True(cosmeticsWithReplace.Count > 0);
        }
    }
}