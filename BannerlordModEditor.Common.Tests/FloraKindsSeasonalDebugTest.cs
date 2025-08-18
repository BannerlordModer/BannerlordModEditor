using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsSeasonalDebugTest
    {
        [Fact]
        public void Debug_SeasonalAttribute_Ordering()
        {
            var xmlPath = "TestData/flora_kinds.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 分析原始XML中的季节顺序
            var doc = XDocument.Parse(xml);
            var firstFloraKind = doc.Root?.Element("flora_kind");
            
            if (firstFloraKind != null)
            {
                var seasonalKinds = firstFloraKind.Elements("seasonal_kind").ToList();
                Console.WriteLine("原始XML中的季节顺序:");
                for (int i = 0; i < seasonalKinds.Count; i++)
                {
                    var season = seasonalKinds[i].Attribute("season")?.Value;
                    Console.WriteLine($"  [{i}] {season}");
                }
            }
            
            // 反序列化并检查DO对象中的季节顺序
            var obj = XmlTestUtils.Deserialize<FloraKindsDO>(xml);
            var firstFloraKindDO = obj.FloraKindsList.FirstOrDefault();
            
            if (firstFloraKindDO != null)
            {
                Console.WriteLine("DO对象中的季节顺序:");
                for (int i = 0; i < firstFloraKindDO.SeasonalKinds.Count; i++)
                {
                    var season = firstFloraKindDO.SeasonalKinds[i].Season;
                    Console.WriteLine($"  [{i}] {season}");
                }
            }
            
            // 序列化并检查结果
            var xml2 = XmlTestUtils.Serialize(obj);
            var doc2 = XDocument.Parse(xml2);
            var firstFloraKind2 = doc2.Root?.Element("flora_kind");
            
            if (firstFloraKind2 != null)
            {
                var seasonalKinds2 = firstFloraKind2.Elements("seasonal_kind").ToList();
                Console.WriteLine("序列化后XML中的季节顺序:");
                for (int i = 0; i < seasonalKinds2.Count; i++)
                {
                    var season = seasonalKinds2[i].Attribute("season")?.Value;
                    Console.WriteLine($"  [{i}] {season}");
                }
            }
        }
    }
}