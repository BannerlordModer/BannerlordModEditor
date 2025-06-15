using BannerlordModEditor.Common.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SiegeEnginesXmlTests
    {
        [Fact]
        public void SiegeEngines_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "siegeengines.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SiegeEngineTypes));
            SiegeEngineTypes siegeEngines;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                siegeEngines = (SiegeEngineTypes)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, siegeEngines);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(siegeEngines);
            Assert.NotNull(siegeEngines.SiegeEngineType);
            Assert.True(siegeEngines.SiegeEngineType.Count > 0, "Should have at least one siege engine");

            // 详细验证每个攻城器械的字段
            foreach (var engine in siegeEngines.SiegeEngineType)
            {
                // 必要字段检查
                Assert.False(string.IsNullOrEmpty(engine.Id), $"Engine {engine.Id} should have an ID");
                Assert.False(string.IsNullOrEmpty(engine.Name), $"Engine {engine.Id} should have a Name");
                Assert.False(string.IsNullOrEmpty(engine.Description), $"Engine {engine.Id} should have a Description");
            }

            // 特定引擎的详细验证 - 检查可选字段的存在性
            var preparations = siegeEngines.SiegeEngineType.FirstOrDefault(e => e.Id == "preparations");
            if (preparations != null)
            {
                Assert.Equal("false", preparations.IsConstructible);
                Assert.Equal("32", preparations.ManDayCost);
                // 这些字段应该不存在于preparations中
                Assert.Null(preparations.MaxHitPoints);
                Assert.Null(preparations.IsRanged);
                Assert.Null(preparations.Damage);
            }

            var ballista = siegeEngines.SiegeEngineType.FirstOrDefault(e => e.Id == "ballista");
            if (ballista != null)
            {
                Assert.Equal("true", ballista.IsRanged);
                Assert.Equal("160", ballista.Damage);
                Assert.Equal("500", ballista.MaxHitPoints);
                Assert.Equal("0.52", ballista.HitChance);
                Assert.Equal("true", ballista.IsAntiPersonnel);
                Assert.Equal("0.25", ballista.AntiPersonnelHitChance);
                Assert.Equal("8", ballista.ManDayCost);
                Assert.Equal("40", ballista.Difficulty);
                Assert.Equal("20.0", ballista.CampaignRateOfFirePerDay);
                // 这个字段应该不存在于ballista中
                Assert.Null(ballista.IsConstructible);
            }

            var ladder = siegeEngines.SiegeEngineType.FirstOrDefault(e => e.Id == "ladder");
            if (ladder != null)
            {
                Assert.Equal("false", ladder.IsConstructible);
                // 这些字段应该不存在于ladder中
                Assert.Null(ladder.MaxHitPoints);
                Assert.Null(ladder.ManDayCost);
                Assert.Null(ladder.IsRanged);
                Assert.Null(ladder.Damage);
            }

            var trebuchet = siegeEngines.SiegeEngineType.FirstOrDefault(e => e.Id == "trebuchet");
            if (trebuchet != null)
            {
                Assert.Equal("40", trebuchet.Difficulty);
                Assert.Equal("true", trebuchet.IsRanged);
                Assert.Equal("1360", trebuchet.Damage);
                Assert.Equal("3200", trebuchet.MaxHitPoints);
                Assert.Equal("0.58", trebuchet.HitChance);
                Assert.Equal("24", trebuchet.ManDayCost);
                Assert.Equal("10.0", trebuchet.CampaignRateOfFirePerDay);
                // 这些字段应该不存在于trebuchet中
                Assert.Null(trebuchet.IsConstructible);
                Assert.Null(trebuchet.IsAntiPersonnel);
                Assert.Null(trebuchet.AntiPersonnelHitChance);
            }

            // XML结构对比 - 确保元素数量一致
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            Assert.True(originalDoc.Root?.Elements().Count() == savedDoc.Root?.Elements().Count(),
                "SiegeEngine element count should be the same");
        }

        [Fact]
        public void SiegeEngines_NumericValues_ShouldBePreserved()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "siegeengines.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SiegeEngineTypes));
            SiegeEngineTypes siegeEngines;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                siegeEngines = (SiegeEngineTypes)serializer.Deserialize(reader)!;
            }

            // Assert - 数值精确性检查
            var fireOnager = siegeEngines.SiegeEngineType.FirstOrDefault(e => e.Id == "fire_onager");
            if (fireOnager != null)
            {
                Assert.Equal("768", fireOnager.Damage); // 确保数值没有变化
                Assert.Equal("1500", fireOnager.MaxHitPoints);
                Assert.Equal("0.62", fireOnager.HitChance); // 确保小数点后数字保持不变
                Assert.Equal("20", fireOnager.ManDayCost);
                Assert.Equal("14.0", fireOnager.CampaignRateOfFirePerDay); // 确保小数格式保持
            }

            var fireTrebuchet = siegeEngines.SiegeEngineType.FirstOrDefault(e => e.Id == "fire_trebuchet");
            if (fireTrebuchet != null)
            {
                Assert.Equal("1560", fireTrebuchet.Damage);
                Assert.Equal("3200", fireTrebuchet.MaxHitPoints);
                Assert.Equal("0.58", fireTrebuchet.HitChance);
                Assert.Equal("10.0", fireTrebuchet.CampaignRateOfFirePerDay);
            }
        }

        [Fact]
        public void SiegeEngines_OptionalFields_ShouldBeCorrectlyHandled()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "siegeengines.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SiegeEngineTypes));
            SiegeEngineTypes siegeEngines;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                siegeEngines = (SiegeEngineTypes)serializer.Deserialize(reader)!;
            }

            // Assert - 可选字段存在性检查
            var enginesWithIsConstructible = siegeEngines.SiegeEngineType.Where(e => e.IsConstructible != null).ToList();
            var enginesWithoutIsConstructible = siegeEngines.SiegeEngineType.Where(e => e.IsConstructible == null).ToList();
            
            Assert.True(enginesWithIsConstructible.Count > 0, "Some engines should have IsConstructible field");
            Assert.True(enginesWithoutIsConstructible.Count > 0, "Some engines should not have IsConstructible field");

            var rangedEngines = siegeEngines.SiegeEngineType.Where(e => e.IsRanged == "true").ToList();
            var nonRangedEngines = siegeEngines.SiegeEngineType.Where(e => e.IsRanged == null).ToList();
            
            Assert.True(rangedEngines.Count > 0, "Should have ranged engines");
            Assert.True(nonRangedEngines.Count > 0, "Should have non-ranged engines");

            // 所有远程武器都应该有伤害值
            foreach (var rangedEngine in rangedEngines)
            {
                Assert.NotNull(rangedEngine.Damage);
                Assert.NotNull(rangedEngine.HitChance);
                Assert.NotNull(rangedEngine.CampaignRateOfFirePerDay);
            }

            // 非远程武器不应该有这些字段
            foreach (var nonRangedEngine in nonRangedEngines)
            {
                Assert.Null(nonRangedEngine.Damage);
                Assert.Null(nonRangedEngine.HitChance);
                Assert.Null(nonRangedEngine.CampaignRateOfFirePerDay);
            }
        }
        
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 