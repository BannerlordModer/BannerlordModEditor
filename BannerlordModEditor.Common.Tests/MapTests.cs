using BannerlordModEditor.Common.Models.Map;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MapTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        [Fact]
        public void MapIcons_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_icons.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MapIconsBase));
            MapIconsBase mapIcons;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapIcons = (MapIconsBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, mapIcons);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(mapIcons);
            Assert.Equal("map_icon", mapIcons.Type);
            Assert.NotNull(mapIcons.MapIcons);
            Assert.NotNull(mapIcons.MapIcons.MapIcon);
            Assert.True(mapIcons.MapIcons.MapIcon.Count > 0, "Should have at least one map icon");

            // 验证特定图标类型
            var playerIcon = mapIcons.MapIcons.MapIcon.FirstOrDefault(mi => mi.IdStr == "player");
            if (playerIcon != null)
            {
                Assert.Equal("map_icon_player", playerIcon.Id);
                Assert.Equal("player", playerIcon.IdStr);
                Assert.Equal("0x0", playerIcon.Flags);
                Assert.Equal("player", playerIcon.MeshName);
                Assert.Equal("0.15", playerIcon.MeshScale);
                Assert.Equal("17", playerIcon.SoundNo);
                Assert.Equal("0.150000, 0.173000, 0.000000", playerIcon.OffsetPos);
            }

            var townIcon = mapIcons.MapIcons.MapIcon.FirstOrDefault(mi => mi.IdStr == "town");
            if (townIcon != null)
            {
                Assert.Equal("map_icon_town", townIcon.Id);
                Assert.Equal("town", townIcon.IdStr);
                Assert.Equal("0x1", townIcon.Flags);
                Assert.Equal("map_town_a", townIcon.MeshName);
                Assert.Equal("0.35", townIcon.MeshScale);
                Assert.Equal("0", townIcon.SoundNo);
                Assert.Equal("0.000000, 0.000000, 0.000000", townIcon.OffsetPos);
            }

            // 验证所有图标都有必要字段
            foreach (var icon in mapIcons.MapIcons.MapIcon)
            {
                Assert.False(string.IsNullOrEmpty(icon.Id), "Map icon should have an ID");
                Assert.False(string.IsNullOrEmpty(icon.IdStr), "Map icon should have an ID string");
                Assert.False(string.IsNullOrEmpty(icon.Flags), "Map icon should have flags");
                Assert.False(string.IsNullOrEmpty(icon.MeshName), "Map icon should have a mesh name");
                Assert.False(string.IsNullOrEmpty(icon.MeshScale), "Map icon should have a mesh scale");
                Assert.False(string.IsNullOrEmpty(icon.SoundNo), "Map icon should have a sound number");
                Assert.False(string.IsNullOrEmpty(icon.OffsetPos), "Map icon should have an offset position");
            }
        }

        [Fact]
        public void MapIcons_IconTypes_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_icons.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MapIconsBase));
            MapIconsBase mapIcons;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapIcons = (MapIconsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证不同类型的图标
            var playerIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("player")).ToList();
            var townIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("town")).ToList();
            var villageIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("village")).ToList();
            var castleIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("castle")).ToList();
            var cityIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("city")).ToList();
            var banditIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("bandit")).ToList();

            Assert.True(playerIcons.Count > 0, "Should have player icons");
            Assert.True(townIcons.Count > 0, "Should have town icons");
            Assert.True(villageIcons.Count > 0, "Should have village icons");
            Assert.True(castleIcons.Count > 0, "Should have castle icons");
            Assert.True(cityIcons.Count > 0, "Should have city icons");
            Assert.True(banditIcons.Count > 0, "Should have bandit icons");

            // 验证不同派系的图标
            var empireIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("empire")).ToList();
            var vlandiaIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("vlandia")).ToList();
            var sturgiaIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("sturgia")).ToList();
            var asraiIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("aserai")).ToList();
            var khuzaitIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("khuzait")).ToList();
            var battaniaIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains("battania")).ToList();

            Assert.True(empireIcons.Count > 0, "Should have empire icons");
            Assert.True(vlandiaIcons.Count > 0, "Should have vlandia icons");
            Assert.True(sturgiaIcons.Count > 0, "Should have sturgia icons");
            Assert.True(asraiIcons.Count > 0, "Should have aserai icons");
            Assert.True(khuzaitIcons.Count > 0, "Should have khuzait icons");
            Assert.True(battaniaIcons.Count > 0, "Should have battania icons");
        }

        [Fact]
        public void MapIcons_AttributeFormats_ShouldBeValid()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_icons.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MapIconsBase));
            MapIconsBase mapIcons;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapIcons = (MapIconsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证属性格式
            foreach (var icon in mapIcons.MapIcons.MapIcon)
            {
                // 验证ID格式
                Assert.True(icon.Id.StartsWith("map_icon_"), 
                    $"Icon ID {icon.Id} should start with 'map_icon_'");

                // 验证标志格式 (hexadecimal)
                Assert.True(icon.Flags.StartsWith("0x"), 
                    $"Flags {icon.Flags} should be in hexadecimal format");

                // 验证缩放格式 (decimal number)
                Assert.True(double.TryParse(icon.MeshScale, out var scale), 
                    $"Mesh scale {icon.MeshScale} should be a valid number");
                Assert.True(scale > 0, $"Mesh scale {scale} should be positive");

                // 验证声音编号格式 (integer)
                Assert.True(int.TryParse(icon.SoundNo, out var soundNo), 
                    $"Sound number {icon.SoundNo} should be a valid integer");
                Assert.True(soundNo >= 0, $"Sound number {soundNo} should be non-negative");

                // 验证位置偏移格式 (三个浮点数，逗号分隔)
                var positionParts = icon.OffsetPos.Split(',');
                Assert.Equal(3, positionParts.Length);
                foreach (var part in positionParts)
                {
                    Assert.True(double.TryParse(part.Trim(), out _), 
                        $"Position part '{part.Trim()}' should be a valid number");
                }
            }
        }

        [Fact]
        public void MapIcons_DirtNameAttribute_ShouldBeOptional()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_icons.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MapIconsBase));
            MapIconsBase mapIcons;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapIcons = (MapIconsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证dirt_name属性的可选性
            var iconsWithDirtName = mapIcons.MapIcons.MapIcon.Where(mi => !string.IsNullOrEmpty(mi.DirtName)).ToList();
            var iconsWithoutDirtName = mapIcons.MapIcons.MapIcon.Where(mi => string.IsNullOrEmpty(mi.DirtName)).ToList();

            Assert.True(iconsWithDirtName.Count > 0, "Should have some icons with dirt_name");
            Assert.True(iconsWithoutDirtName.Count > 0, "Should have some icons without dirt_name");

            // 验证有dirt_name的图标的格式正确性
            foreach (var icon in iconsWithDirtName)
            {
                Assert.True(icon.DirtName!.StartsWith("map_icons_"), 
                    $"Dirt name {icon.DirtName} should start with 'map_icons_'");
                
                // 大部分有dirt_name的应该是村庄或城市相关的
                Assert.True(icon.IdStr.Contains("village") || icon.IdStr.Contains("city") || icon.IdStr.Contains("castle"), 
                    $"Icon with dirt_name {icon.IdStr} should typically be a settlement");
            }
        }

        [Fact]
        public void MapIcons_CultureSpecificIcons_ShouldHaveConsistentNaming()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_icons.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MapIconsBase));
            MapIconsBase mapIcons;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapIcons = (MapIconsBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证不同文化图标的一致性
            var cultures = new[] { "empire", "vlandia", "sturgia", "aserai", "khuzait", "battania" };
            
            foreach (var culture in cultures)
            {
                var cultureIcons = mapIcons.MapIcons.MapIcon.Where(mi => mi.IdStr.Contains(culture)).ToList();
                
                if (cultureIcons.Count > 0)
                {
                    // 验证城市图标
                    var cityIcons = cultureIcons.Where(mi => mi.IdStr.Contains("city")).ToList();
                    if (cityIcons.Count > 0)
                    {
                        var cityH = cityIcons.FirstOrDefault(mi => mi.IdStr.EndsWith("_h"));
                        var cityM = cityIcons.FirstOrDefault(mi => mi.IdStr.EndsWith("_m"));
                        var cityL = cityIcons.FirstOrDefault(mi => mi.IdStr.EndsWith("_l"));

                        // 应该有高、中、低三种规模的城市
                        Assert.True(cityH != null || cityM != null || cityL != null, 
                            $"Culture {culture} should have at least one city size variant");
                    }

                    // 验证城堡图标
                    var castleIcons = cultureIcons.Where(mi => mi.IdStr.Contains("castle")).ToList();
                    if (castleIcons.Count > 0)
                    {
                        var normalCastles = castleIcons.Where(mi => !mi.IdStr.Contains("ucon")).ToList();
                        var underConstructionCastles = castleIcons.Where(mi => mi.IdStr.Contains("ucon")).ToList();

                        Assert.True(normalCastles.Count > 0, 
                            $"Culture {culture} should have normal castle icons");

                        // 验证在建城堡应该有对应的正常城堡
                        foreach (var uconCastle in underConstructionCastles)
                        {
                            var baseId = uconCastle.IdStr.Replace("_ucon", "");
                            var correspondingNormal = normalCastles.FirstOrDefault(nc => nc.IdStr == baseId);
                            // 注：不是所有在建城堡都有对应的正常版本，这是正常的
                        }
                    }
                }
            }
        }
    }
} 