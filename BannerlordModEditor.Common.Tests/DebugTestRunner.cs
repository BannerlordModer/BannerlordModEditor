using System;
using System.IO;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public static class DebugTestRunner
    {
        public static void DebugBannerIcons()
        {
            try
            {
                var xmlPath = "TestData/banner_icons.xml";
                var xml = File.ReadAllText(xmlPath);
                
                Console.WriteLine("=== ORIGINAL XML ===");
                Console.WriteLine(xml);
                
                // 反序列化
                var model = XmlTestUtils.Deserialize<BannerIconsRoot>(xml);
                
                Console.WriteLine("=== DESERIALIZED MODEL ===");
                Console.WriteLine($"Type: {model.Type}");
                Console.WriteLine($"BannerIconData null: {model.BannerIconData == null}");
                if (model.BannerIconData != null)
                {
                    Console.WriteLine($"BannerIconGroups count: {model.BannerIconData.BannerIconGroups?.Count ?? 0}");
                    Console.WriteLine($"BannerColors null: {model.BannerIconData.BannerColors == null}");
                }
                
                // 再序列化
                var xml2 = XmlTestUtils.Serialize(model, xml);
                
                Console.WriteLine("=== SERIALIZED XML ===");
                Console.WriteLine(xml2);
                
                // 比较差异
                var diff = XmlTestUtils.CompareXmlStructure(xml, xml2);
                
                Console.WriteLine("=== DIFFERENCES ===");
                Console.WriteLine($"Structurally Equal: {diff.IsStructurallyEqual}");
                
                foreach (var item in diff.MissingAttributes)
                    Console.WriteLine($"Missing Attribute: {item}");
                foreach (var item in diff.ExtraAttributes)
                    Console.WriteLine($"Extra Attribute: {item}");
                foreach (var item in diff.AttributeValueDifferences)
                    Console.WriteLine($"Attribute Value: {item}");
                
                Console.WriteLine($"Node count diff: {diff.NodeCountDifference}");
                Console.WriteLine($"Attribute count diff: {diff.AttributeCountDifference}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}