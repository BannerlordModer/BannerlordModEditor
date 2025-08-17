using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsDetailedDebugTest
    {
        [Fact]
        public void Debug_Find_Problematic_Replace()
        {
            var xml = File.ReadAllText("TestData/mpcosmetics.xml");
            var model = XmlTestUtils.Deserialize<Mpcosmetics>(xml);
            
            Console.WriteLine($"总共Cosmetic数量: {model.Cosmetics.Count}");
            
            // 检查原始XML中的Replace元素数量
            var doc = XDocument.Parse(xml);
            var originalReplaceElements = doc.Descendants("Replace").ToList();
            Console.WriteLine($"原始XML中Replace元素数量: {originalReplaceElements.Count}");
            
            // 检查每个Replace元素的子元素数量
            for (int i = 0; i < originalReplaceElements.Count; i++)
            {
                var replace = originalReplaceElements[i];
                var childElements = replace.Elements().Count();
                if (childElements != 1) // 期望有1个Item元素
                {
                    Console.WriteLine($"问题Replace #{i}: 有 {childElements} 个子元素");
                    Console.WriteLine($"  父Cosmetic: {replace.Parent.Attribute("id")?.Value ?? "未知"}");
                    Console.WriteLine($"  内容: {replace}");
                }
            }
            
            // 检查模型中的Replace对象
            int modelsWithReplace = 0;
            int totalItems = 0;
            
            for (int i = 0; i < model.Cosmetics.Count; i++)
            {
                var cosmetic = model.Cosmetics[i];
                if (cosmetic.Replace != null)
                {
                    modelsWithReplace++;
                    totalItems += cosmetic.Replace.Items.Count;
                    
                    if (cosmetic.Replace.Items.Count == 0)
                    {
                        Console.WriteLine($"空的Replace在Cosmetic #{i}: id={cosmetic.Id}");
                    }
                }
            }
            
            Console.WriteLine($"模型中有Replace的Cosmetic数量: {modelsWithReplace}");
            Console.WriteLine($"模型中总Item数量: {totalItems}");
            
            // 比较数量
            if (originalReplaceElements.Count != modelsWithReplace)
            {
                Console.WriteLine($"数量不匹配! 原始: {originalReplaceElements.Count}, 模型: {modelsWithReplace}");
            }
            
            // 尝试找到具体哪个Cosmetic有问题
            for (int i = 0; i < Math.Min(originalReplaceElements.Count, model.Cosmetics.Count); i++)
            {
                var originalReplace = originalReplaceElements[i];
                var cosmetic = model.Cosmetics[i];
                
                var originalItemCount = originalReplace.Elements().Count();
                var modelItemCount = cosmetic.Replace?.Items.Count ?? 0;
                
                if (originalItemCount != modelItemCount)
                {
                    Console.WriteLine($"不匹配的Cosmetic #{i}:");
                    Console.WriteLine($"  ID: {cosmetic.Id}");
                    Console.WriteLine($"  原始Item数量: {originalItemCount}");
                    Console.WriteLine($"  模型Item数量: {modelItemCount}");
                    Console.WriteLine($"  原始Replace: {originalReplace}");
                }
            }
            
            Assert.True(originalReplaceElements.Count == modelsWithReplace, 
                $"Replace元素数量不匹配: 原始={originalReplaceElements.Count}, 模型={modelsWithReplace}");
        }
    }
}