using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class BannerIconsXmlTests
    {
        [Fact]
        public void BannerIcons_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/banner_icons.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var model = XmlTestUtils.Deserialize<BannerIconsRoot>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model);

            // 结构化对比
            var isEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            // 如果不相等，输出差异信息
            if (!isEqual)
            {
                var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
                System.Console.WriteLine($"=== 结构相等性检查失败 ===");
                System.Console.WriteLine($"节点数量差异: {diff.NodeCountDifference}");
                System.Console.WriteLine($"属性数量差异: {diff.AttributeCountDifference}");
                System.Console.WriteLine($"缺失节点: {string.Join(", ", diff.MissingNodes)}");
                System.Console.WriteLine($"多余节点: {string.Join(", ", diff.ExtraNodes)}");
                System.Console.WriteLine($"节点名差异: {string.Join(", ", diff.NodeNameDifferences)}");
                System.Console.WriteLine($"缺失属性: {string.Join(", ", diff.MissingAttributes)}");
                System.Console.WriteLine($"多余属性: {string.Join(", ", diff.ExtraAttributes)}");
                System.Console.WriteLine($"属性值差异: {string.Join(", ", diff.AttributeValueDifferences)}");
                System.Console.WriteLine($"文本差异: {string.Join(", ", diff.TextDifferences)}");
                
                // 输出原始XML和序列化后的XML到文件用于调试
                File.WriteAllText("debug_original.xml", xml);
                File.WriteAllText("debug_serialized.xml", xml2);
            }
            
            Assert.True(isEqual);
        }
    }
}