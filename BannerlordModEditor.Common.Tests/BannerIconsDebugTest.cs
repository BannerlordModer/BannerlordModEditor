using System;
using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class BannerIconsDebugTest
    {
        [Fact]
        public void Debug_BannerIcons_RoundTrip()
        {
            var xmlPath = "TestData/banner_icons.xml";
            var xml = File.ReadAllText(xmlPath);

            Console.WriteLine("=== 开始BannerIcons调试测试 ===");
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<BannerIconsDO>(xml);
            
            // 输出模型信息
            Console.WriteLine($"模型类型: {model.GetType().Name}");
            Console.WriteLine($"HasBannerIconData: {model.HasBannerIconData}");
            Console.WriteLine($"BannerIconData: {(model.BannerIconData != null ? "存在" : "null")}");
            
            if (model.BannerIconData != null)
            {
                Console.WriteLine($"BannerIconGroups数量: {model.BannerIconData.BannerIconGroups.Count}");
                Console.WriteLine($"HasBannerColors: {model.BannerIconData.HasBannerColors}");
                Console.WriteLine($"BannerColors: {(model.BannerIconData.BannerColors != null ? "存在" : "null")}");
                
                if (model.BannerIconData.BannerColors != null)
                {
                    Console.WriteLine($"Colors数量: {model.BannerIconData.BannerColors.Colors.Count}");
                }
            }

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(model, xml);

            // 结构化对比
            var isEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            Console.WriteLine($"结构相等性: {isEqual}");
            
            // 如果不相等，输出差异信息
            if (!isEqual)
            {
                var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
                Console.WriteLine($"=== 结构相等性检查失败 ===");
                Console.WriteLine($"节点数量差异: {diff.NodeCountDifference}");
                Console.WriteLine($"属性数量差异: {diff.AttributeCountDifference}");
                Console.WriteLine($"缺失节点: {string.Join(", ", diff.MissingNodes)}");
                Console.WriteLine($"多余节点: {string.Join(", ", diff.ExtraNodes)}");
                Console.WriteLine($"节点名差异: {string.Join(", ", diff.NodeNameDifferences)}");
                Console.WriteLine($"缺失属性: {string.Join(", ", diff.MissingAttributes)}");
                Console.WriteLine($"多余属性: {string.Join(", ", diff.ExtraAttributes)}");
                Console.WriteLine($"属性值差异: {string.Join(", ", diff.AttributeValueDifferences)}");
                Console.WriteLine($"文本差异: {string.Join(", ", diff.TextDifferences)}");
                
                // 输出原始XML和序列化后的XML到文件用于调试
                File.WriteAllText("debug_original.xml", xml);
                File.WriteAllText("debug_serialized.xml", xml2);
                
                Console.WriteLine("调试文件已保存到 debug_original.xml 和 debug_serialized.xml");
            }
            
            Assert.True(isEqual);
        }
    }
}