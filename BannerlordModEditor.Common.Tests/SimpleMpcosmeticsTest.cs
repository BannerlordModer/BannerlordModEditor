using System;
using System.IO;
using System.Xml.Serialization;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleMpcosmeticsTest
    {
        private const string TestDataPath = "TestData/mpcosmetics.xml";

        [Fact]
        public void Simple_Check_First_Cosmetic()
        {
            var xml = File.ReadAllText(TestDataPath);
            var model = XmlTestUtils.Deserialize<Mpcosmetics>(xml);
            
            Console.WriteLine($"Cosmetic数量: {model.Cosmetics.Count}");
            
            if (model.Cosmetics.Count > 0)
            {
                var first = model.Cosmetics[0];
                Console.WriteLine($"第一个Cosmetic:");
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
        }
    }
}