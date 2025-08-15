using System;
using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class SimpleBannerIconsTest
    {
        [Fact]
        public void Simple_BannerIcons_Test()
        {
            var xmlPath = "TestData/banner_icons.xml";
            var xml = File.ReadAllText(xmlPath);
            
            Console.WriteLine($"XML Length: {xml.Length}");
            Console.WriteLine($"XML Start: {xml.Substring(0, Math.Min(100, xml.Length))}");
            
            // 反序列化
            var model = XmlTestUtils.Deserialize<BannerIconsRoot>(xml);
            
            Assert.NotNull(model);
            Assert.Equal("string", model.Type);
            Assert.NotNull(model.BannerIconData);
            
            Console.WriteLine($"BannerIconGroups count: {model.BannerIconData.BannerIconGroups?.Count ?? 0}");
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(model, xml);
            
            Console.WriteLine($"Serialized XML Length: {xml2.Length}");
            Console.WriteLine($"Serialized XML Start: {xml2.Substring(0, Math.Min(100, xml2.Length))}");
            
            // Check if they're structurally equal
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            Assert.True(areEqual, "XML structures should be equal");
        }
    }
}