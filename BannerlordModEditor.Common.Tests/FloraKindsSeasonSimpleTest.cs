using System;
using System.IO;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsSeasonSimpleTest
    {
        [Fact]
        public void Test_SeasonalAttributes_ExactComparison()
        {
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<FloraKindsDO>(xml);
            
            // 序列化
            var xml2 = XmlTestUtils.Serialize(obj);
            
            // 比较第一个flora_kind的季节属性
            var firstFloraKind = obj.FloraKindsList.FirstOrDefault(f => f.Name == "flora_grass_a_visualtest");
            Assert.NotNull(firstFloraKind);
            
            Console.WriteLine($"原始FloraKind名称: {firstFloraKind.Name}");
            Console.WriteLine($"SeasonalKinds数量: {firstFloraKind.SeasonalKinds.Count}");
            
            for (int i = 0; i < firstFloraKind.SeasonalKinds.Count; i++)
            {
                var season = firstFloraKind.SeasonalKinds[i].Season;
                Console.WriteLine($"  DO对象季节[{i}]: {season}");
            }
            
            // 解析序列化后的XML并检查季节
            var doc = System.Xml.Linq.XDocument.Parse(xml2);
            var serializedFloraKind = doc.Root?.Elements("flora_kind")
                .FirstOrDefault(e => e.Attribute("name")?.Value == "flora_grass_a_visualtest");
            
            Assert.NotNull(serializedFloraKind);
            
            var serializedSeasonalKinds = serializedFloraKind.Elements("seasonal_kind").ToList();
            Console.WriteLine($"序列化后SeasonalKinds数量: {serializedSeasonalKinds.Count}");
            
            for (int i = 0; i < serializedSeasonalKinds.Count; i++)
            {
                var season = serializedSeasonalKinds[i].Attribute("season")?.Value;
                Console.WriteLine($"  序列化后季节[{i}]: {season}");
            }
            
            // 检查季节是否一致
            Assert.Equal(firstFloraKind.SeasonalKinds.Count, serializedSeasonalKinds.Count);
            
            for (int i = 0; i < firstFloraKind.SeasonalKinds.Count; i++)
            {
                var expectedSeason = firstFloraKind.SeasonalKinds[i].Season;
                var actualSeason = serializedSeasonalKinds[i].Attribute("season")?.Value;
                Assert.Equal(expectedSeason, actualSeason);
            }
        }
    }
}