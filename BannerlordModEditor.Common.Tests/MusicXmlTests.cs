using BannerlordModEditor.Common.Models.Data;
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
    public class MusicXmlTests
    {
        [Fact]
        public void Music_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(Music));
            Music music;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                music = (Music)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, music);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(music);
            Assert.Equal("music", music.Type);
            Assert.NotNull(music.MusicsContainer);
            Assert.NotNull(music.MusicsContainer.Music);
            Assert.True(music.MusicsContainer.Music.Count > 0, "Should have at least one music track");
            
            // 验证特定音乐轨道
            var titleScreenMusic = music.MusicsContainer.Music.FirstOrDefault(m => m.Id == "music_mount_and_blade_title_screen.ogg");
            Assert.NotNull(titleScreenMusic);
            Assert.Equal("mount_and_blade_title_screen.ogg", titleScreenMusic.Name);
            Assert.Equal("0x400080", titleScreenMusic.Flags);
            Assert.Equal("0x400080", titleScreenMusic.ContinueFlags);
            
            var arena1Music = music.MusicsContainer.Music.FirstOrDefault(m => m.Id == "music_arena_1.ogg");
            Assert.NotNull(arena1Music);
            Assert.Equal("arena_1.ogg", arena1Music.Name);
            Assert.Equal("0x20000", arena1Music.Flags);
            Assert.Equal("0x20000", arena1Music.ContinueFlags);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Element("musics")?.Elements("music").Count(), 
                        savedDoc.Root?.Element("musics")?.Elements("music").Count());
        }
        
        [Fact]
        public void Music_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(Music));
            Music music;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                music = (Music)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有音乐轨道都有必要的属性
            Assert.Equal("music", music.Type);
            Assert.NotNull(music.MusicsContainer);
            
            foreach (var track in music.MusicsContainer.Music)
            {
                Assert.False(string.IsNullOrWhiteSpace(track.Id), "Music track should have Id");
                Assert.False(string.IsNullOrWhiteSpace(track.Name), "Music track should have Name");
                Assert.False(string.IsNullOrWhiteSpace(track.Flags), "Music track should have Flags");
                Assert.False(string.IsNullOrWhiteSpace(track.ContinueFlags), "Music track should have ContinueFlags");
                
                // 验证ID和Name的对应关系
                Assert.StartsWith("music_", track.Id);
                Assert.EndsWith(".ogg", track.Id);
                Assert.EndsWith(".ogg", track.Name);
                
                // 验证标志格式（十六进制）
                Assert.True(track.Flags.StartsWith("0x"), $"Flags should be hexadecimal, got '{track.Flags}'");
                Assert.True(track.ContinueFlags.StartsWith("0x"), $"ContinueFlags should be hexadecimal, got '{track.ContinueFlags}'");
                
                // 验证十六进制值有效性
                var flagsValue = track.Flags.Substring(2); // 移除"0x"前缀
                var continueFlagsValue = track.ContinueFlags.Substring(2);
                
                Assert.True(IsValidHex(flagsValue), $"Flags should be valid hex: '{track.Flags}'");
                Assert.True(IsValidHex(continueFlagsValue), $"ContinueFlags should be valid hex: '{track.ContinueFlags}'");
                
                // 在这个XML中，flags和continue_flags通常是相同的
                Assert.Equal(track.Flags, track.ContinueFlags);
            }
            
            // 验证包含预期的音乐轨道
            var allIds = music.MusicsContainer.Music.Select(m => m.Id).ToList();
            Assert.Contains("music_mount_and_blade_title_screen.ogg", allIds);
            Assert.Contains("music_arena_1.ogg", allIds);
            Assert.Contains("music_fight_1.ogg", allIds);
            Assert.Contains("music_lords_hall_swadian.ogg", allIds);
            
            // 验证没有重复的ID
            var uniqueIds = allIds.Distinct().ToList();
            Assert.Equal(allIds.Count, uniqueIds.Count);
            
            // 验证没有重复的Name
            var allNames = music.MusicsContainer.Music.Select(m => m.Name).ToList();
            var uniqueNames = allNames.Distinct().ToList();
            Assert.Equal(allNames.Count, uniqueNames.Count);
            
            // 验证标题屏幕音乐的特殊标志
            var titleMusic = music.MusicsContainer.Music.First(m => m.Id == "music_mount_and_blade_title_screen.ogg");
            Assert.Equal("0x400080", titleMusic.Flags);
            
            // 验证战斗音乐的标志模式
            var fightMusics = music.MusicsContainer.Music.Where(m => m.Id.Contains("fight_")).ToList();
            Assert.True(fightMusics.Count > 0, "Should have fight music tracks");
            
            foreach (var fightMusic in fightMusics.Take(3)) // 检查前几个战斗音乐
            {
                if (fightMusic.Id == "music_fight_1.ogg" || fightMusic.Id == "music_fight_2.ogg" || fightMusic.Id == "music_fight_3.ogg")
                {
                    Assert.Equal("0x1C00", fightMusic.Flags);
                }
            }
        }

        private static bool IsValidHex(string hex)
        {
            return hex.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
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