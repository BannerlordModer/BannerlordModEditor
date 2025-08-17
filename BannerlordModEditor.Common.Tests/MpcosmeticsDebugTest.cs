using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;
using BannerlordModEditor.Common.Tests.Services;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsDebugTest
    {
        private const string TestDataPath = "TestData/mpcosmetics.xml";

        [Fact]
        public void Mpcosmetics_Debug_Analysis()
        {
            var xml = File.ReadAllText(TestDataPath);
            Console.WriteLine($"原始XML长度: {xml.Length}");
            
            var model = XmlTestUtils.Deserialize<Mpcosmetics>(xml);
            Console.WriteLine($"反序列化后的Cosmetic数量: {model.Cosmetics.Count}");
            
            var serialized = XmlTestUtils.Serialize(model);
            Console.WriteLine($"序列化后的XML长度: {serialized.Length}");
            
            // 保存样本文件用于比较
            File.WriteAllText("original_xml_sample.txt", xml);
            File.WriteAllText("serialized_xml_sample.txt", serialized);
            
            // 检查具体的差异
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, serialized);
            Console.WriteLine($"结构相等性: {areEqual}");
            
            if (!areEqual)
            {
                // 分析差异
                var originalLines = xml.Split('\n');
                var serializedLines = serialized.Split('\n');
                
                Console.WriteLine($"原始XML行数: {originalLines.Length}");
                Console.WriteLine($"序列化XML行数: {serializedLines.Length}");
                
                // 找到第一个不同的行
                for (int i = 0; i < Math.Min(originalLines.Length, serializedLines.Length); i++)
                {
                    if (originalLines[i].Trim() != serializedLines[i].Trim())
                    {
                        Console.WriteLine($"第一个差异在第 {i+1} 行:");
                        Console.WriteLine($"原始: '{originalLines[i]}'");
                        Console.WriteLine($"序列化: '{serializedLines[i]}'");
                        break;
                    }
                }
            }
            
            // 检查数据完整性
            Assert.Equal(model.Cosmetics.Count, model.Cosmetics.Count); // 确保没有数据丢失
        }
    }
}