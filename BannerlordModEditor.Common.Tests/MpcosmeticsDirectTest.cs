using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsDirectTest
    {
        private const string TestDataPath = "TestData/mpcosmetics.xml";

        [Fact]
        public void Test_Direct_Xml_Serialization()
        {
            var xml = File.ReadAllText(TestDataPath);
            
            // 使用标准的XmlSerializer直接测试
            var serializer = new XmlSerializer(typeof(Mpcosmetics));
            using var reader = new StringReader(xml);
            var model = (Mpcosmetics)serializer.Deserialize(reader);
            
            Console.WriteLine($"直接反序列化的Cosmetic数量: {model.Cosmetics.Count}");
            
            if (model.Cosmetics.Count > 0)
            {
                var first = model.Cosmetics[0];
                Console.WriteLine($"第一个Cosmetic (直接反序列化):");
                Console.WriteLine($"  Type: {first.Type}");
                Console.WriteLine($"  Id: {first.Id}");
                Console.WriteLine($"  Replace存在: {first.Replace != null}");
                
                if (first.Replace != null)
                {
                    Console.WriteLine($"  Items数量: {first.Replace.Items.Count}");
                    if (first.Replace.Items.Count > 0)
                    {
                        Console.WriteLine($"  第一个Item: {first.Replace.Items[0].Id}");
                    }
                }
            }
            
            // 直接序列化回去
            using var writer = new StringWriter();
            serializer.Serialize(writer, model);
            var directSerialized = writer.ToString();
            
            Console.WriteLine($"直接序列化的XML长度: {directSerialized.Length}");
            Console.WriteLine("直接序列化的XML开头:");
            Console.WriteLine(directSerialized.Substring(0, Math.Min(200, directSerialized.Length)));
        }
    }
}