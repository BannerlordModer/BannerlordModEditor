using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsObjectStateTest
    {
        private const string TestDataPath = "TestData/mpcosmetics.xml";

        [Fact]
        public void Check_Object_State_After_Deserialization()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<Mpcosmetics>(xml);
            
            Console.WriteLine($"反序列化后的Cosmetic数量: {model.Cosmetics.Count}");
            
            // 检查前几个Cosmetic对象的状态
            for (int i = 0; i < Math.Min(5, model.Cosmetics.Count); i++)
            {
                var cosmetic = model.Cosmetics[i];
                Console.WriteLine($"Cosmetic {i + 1}:");
                Console.WriteLine($"  Type: {cosmetic.Type}");
                Console.WriteLine($"  Id: {cosmetic.Id}");
                Console.WriteLine($"  Rarity: {cosmetic.Rarity}");
                Console.WriteLine($"  Cost: {cosmetic.Cost}");
                Console.WriteLine($"  Replace: {(cosmetic.Replace != null ? "存在" : "不存在")}");
                
                if (cosmetic.Replace != null)
                {
                    Console.WriteLine($"  Replace.Items数量: {cosmetic.Replace.Items.Count}");
                    for (int j = 0; j < cosmetic.Replace.Items.Count; j++)
                    {
                        Console.WriteLine($"    Item {j + 1}: {cosmetic.Replace.Items[j].Id}");
                    }
                }
                Console.WriteLine();
            }
            
            // 检查序列化后的XML
            var serialized = XmlTestUtils.Serialize(model);
            Console.WriteLine($"序列化后的XML长度: {serialized.Length}");
            
            // 检查序列化后的XML中的第一个Cosmetic
            Console.WriteLine("序列化后的XML开头:");
            Console.WriteLine(serialized.Substring(0, Math.Min(500, serialized.Length)));
        }
    }
}