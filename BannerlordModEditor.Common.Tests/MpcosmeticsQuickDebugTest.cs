using System;
using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class MpcosmeticsQuickDebugTest
    {
        [Fact]
        public void Debug_Replace_Serialization()
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Cosmetics>
    <Cosmetic type=""Clothing"" id=""mp_vlandia_chainmail"" rarity=""Rare"" cost=""500"">
        <Replace>
            <Item id=""mp_white_coat_over_mail"" />
        </Replace>
    </Cosmetic>
</Cosmetics>";

            Console.WriteLine("原始XML:");
            Console.WriteLine(xml);
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<Mpcosmetics>(xml);
            
            // 检查模型状态
            Console.WriteLine($"\n模型状态:");
            Console.WriteLine($"Cosmetics数量: {model.Cosmetics.Count}");
            Console.WriteLine($"第一个Cosmetic的Replace: {(model.Cosmetics[0].Replace != null ? "存在" : "不存在")}");
            if (model.Cosmetics[0].Replace != null)
            {
                Console.WriteLine($"Replace.Items数量: {model.Cosmetics[0].Replace.Items.Count}");
                if (model.Cosmetics[0].Replace.Items.Count > 0)
                {
                    Console.WriteLine($"第一个Item Id: {model.Cosmetics[0].Replace.Items[0].Id}");
                }
            }
            
            // 序列化
            var serialized = XmlTestUtils.Serialize(model, xml);
            
            Console.WriteLine("\n序列化XML:");
            Console.WriteLine(serialized);
            
            // 比较结构
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, serialized);
            Console.WriteLine($"\n结构相等: {areEqual}");
            
            if (!areEqual)
            {
                var report = XmlTestUtils.CompareXmlStructure(xml, serialized);
                Console.WriteLine("差异报告:");
                Console.WriteLine($"  节点数量差异: {report.NodeCountDifference}");
                Console.WriteLine($"  属性数量差异: {report.AttributeCountDifference}");
                Console.WriteLine($"  缺失节点: {string.Join(", ", report.MissingNodes)}");
                Console.WriteLine($"  多余节点: {string.Join(", ", report.ExtraNodes)}");
            }
            
            // 临时断言，看看是否通过了
            Assert.True(areEqual);
        }
    }
}