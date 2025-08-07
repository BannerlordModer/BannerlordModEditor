using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.Data;

namespace BannerlordModEditor.Common.Tests
{
    public class GogAchievementDataXmlTests
    {
        [Fact]
        public void GogAchievementData_RoundTrip_StructuralEquality()
        {
            var xmlPath = "TestData/gog_achievement_data.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<GogAchievementData>(xml);

            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj);

            // 结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            // 如果不相等，输出详细差异信息
            if (!areEqual)
            {
                var cleanXml1 = XmlTestUtils.CleanXmlForComparison(xml);
                var cleanXml2 = XmlTestUtils.CleanXmlForComparison(xml2);
                
                var doc1 = XDocument.Parse(cleanXml1);
                var doc2 = XDocument.Parse(cleanXml2);
                
                File.WriteAllText("debug_original.xml", xml);
                File.WriteAllText("debug_serialized.xml", xml2);
                File.WriteAllText("debug_clean_original.xml", cleanXml1);
                File.WriteAllText("debug_clean_serialized.xml", cleanXml2);
                
                // 输出差异信息
                var originalLines = cleanXml1.Split('\n');
                var serializedLines = cleanXml2.Split('\n');
                
                for (int i = 0; i < Math.Max(originalLines.Length, serializedLines.Length); i++)
                {
                    if (i >= originalLines.Length)
                    {
                        Console.WriteLine($"+ {serializedLines[i]}");
                    }
                    else if (i >= serializedLines.Length)
                    {
                        Console.WriteLine($"- {originalLines[i]}");
                    }
                    else if (originalLines[i] != serializedLines[i])
                    {
                        Console.WriteLine($"- {originalLines[i]}");
                        Console.WriteLine($"+ {serializedLines[i]}");
                    }
                }
            }
            
            Assert.True(areEqual);
        }
    }
}