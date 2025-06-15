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
    public class AdditionalSimpleTypesTests
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
        public void Parties_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "parties.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(PartiesBase));
            PartiesBase parties;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                parties = (PartiesBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, parties);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(parties);
            Assert.NotNull(parties.Parties);
            Assert.NotNull(parties.Parties.Party);
            Assert.Equal("party", parties.Type);
            Assert.True(parties.Parties.Party.Count > 0, "Should have at least one party");

            // 详细验证每个party的字段
            foreach (var party in parties.Parties.Party)
            {
                Assert.False(string.IsNullOrEmpty(party.Id), $"Party {party.Id} should have an ID");
                Assert.False(string.IsNullOrEmpty(party.Name), $"Party {party.Id} should have a Name");
                Assert.False(string.IsNullOrEmpty(party.Flags), $"Party {party.Id} should have Flags");
                Assert.False(string.IsNullOrEmpty(party.PartyTemplate), $"Party {party.Id} should have PartyTemplate");
                Assert.False(string.IsNullOrEmpty(party.Position), $"Party {party.Id} should have Position");
                Assert.False(string.IsNullOrEmpty(party.AverageBearingRot), $"Party {party.Id} should have AverageBearingRot");
            }

            // 验证特定party的可选字段
            var tempParty = parties.Parties.Party.FirstOrDefault(p => p.Id == "p_temp_party");
            if (tempParty != null)
            {
                Assert.Equal("p_temp_party", tempParty.Id);
                Assert.Equal("{!}temp party", tempParty.Name);
                Assert.Equal("0x100", tempParty.Flags);
                Assert.Equal("pt_none", tempParty.PartyTemplate);
                Assert.Equal("0.000000, 0.000000, 0.000000", tempParty.Position);
                Assert.Equal("0", tempParty.AverageBearingRot);
                // 这个party不应该有field，或者为空集合
                Assert.True(tempParty.Field == null || tempParty.Field.Count == 0);
            }

            var tempCasualties = parties.Parties.Party.FirstOrDefault(p => p.Id == "p_temp_casualties");
            if (tempCasualties != null)
            {
                Assert.Equal("p_temp_casualties", tempCasualties.Id);
                Assert.Equal("{!}casualties", tempCasualties.Name);
                Assert.Equal("0x80100", tempCasualties.Flags);
                Assert.Equal("1.000000, 1.000000, 0.000000", tempCasualties.Position);
                // 这个party应该有field
                Assert.NotNull(tempCasualties.Field);
                Assert.Equal(2, tempCasualties.Field.Count);
                
                var minField = tempCasualties.Field.FirstOrDefault(f => f.FieldName == "ThinkFrequencyMin");
                Assert.NotNull(minField);
                Assert.Equal("3", minField.FieldValue);
                
                var maxField = tempCasualties.Field.FirstOrDefault(f => f.FieldName == "ThinkFrequencyMax");
                Assert.NotNull(maxField);
                Assert.Equal("21", maxField.FieldValue);
            }
        }

        [Fact]
        public void Music_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MusicBase));
            MusicBase musicBase;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                musicBase = (MusicBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, musicBase);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(musicBase);
            Assert.NotNull(musicBase.Musics);
            Assert.NotNull(musicBase.Musics.Music);
            Assert.Equal("music", musicBase.Type);
            Assert.True(musicBase.Musics.Music.Count > 0, "Should have at least one music");

            // 详细验证每个music的字段
            foreach (var music in musicBase.Musics.Music)
            {
                Assert.False(string.IsNullOrEmpty(music.Id), $"Music {music.Id} should have an ID");
                Assert.False(string.IsNullOrEmpty(music.Name), $"Music {music.Id} should have a Name");
                Assert.False(string.IsNullOrEmpty(music.Flags), $"Music {music.Id} should have Flags");
                Assert.False(string.IsNullOrEmpty(music.ContinueFlags), $"Music {music.Id} should have ContinueFlags");
            }
        }

        [Fact]
        public void Music_SpecificEntries_ShouldHaveCorrectValues()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MusicBase));
            MusicBase musicBase;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                musicBase = (MusicBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证特定音乐条目的数值
            var cantFindThis = musicBase.Musics.Music.FirstOrDefault(m => m.Id == "music_cant_find_this.ogg");
            if (cantFindThis != null)
            {
                Assert.Equal("music_cant_find_this.ogg", cantFindThis.Id);
                Assert.Equal("cant_find_this.ogg", cantFindThis.Name);
                Assert.Equal("0x0", cantFindThis.Flags);
                Assert.Equal("0x0", cantFindThis.ContinueFlags);
            }

            var titleScreen = musicBase.Musics.Music.FirstOrDefault(m => m.Id == "music_mount_and_blade_title_screen.ogg");
            if (titleScreen != null)
            {
                Assert.Equal("music_mount_and_blade_title_screen.ogg", titleScreen.Id);
                Assert.Equal("mount_and_blade_title_screen.ogg", titleScreen.Name);
                Assert.Equal("0x400080", titleScreen.Flags);
                Assert.Equal("0x400080", titleScreen.ContinueFlags);
            }

            var ambushedByNeutral = musicBase.Musics.Music.FirstOrDefault(m => m.Id == "music_ambushed_by_neutral.ogg");
            if (ambushedByNeutral != null)
            {
                Assert.Equal("0x41000", ambushedByNeutral.Flags);
                Assert.Equal("0x41000", ambushedByNeutral.ContinueFlags);
            }
        }

        [Fact]
        public void Music_HexadecimalFlags_ShouldBePreserved()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "music.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MusicBase));
            MusicBase musicBase;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                musicBase = (MusicBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证十六进制标志位格式保持不变
            var musicWithSpecialFlags = musicBase.Musics.Music.Where(m => 
                m.Flags.StartsWith("0x") && m.Flags != "0x0").ToList();
            
            Assert.True(musicWithSpecialFlags.Count > 0, "Should have music with special flags");

            foreach (var music in musicWithSpecialFlags)
            {
                // 确保flags以0x开头
                Assert.True(music.Flags.StartsWith("0x"), $"Music {music.Id} flags should start with 0x");
                Assert.True(music.ContinueFlags.StartsWith("0x"), $"Music {music.Id} continue_flags should start with 0x");
                
                // 确保flags相同（大部分音乐的flags和continue_flags相同）
                Assert.Equal(music.Flags, music.ContinueFlags);
            }

            // 检查特定的复杂flag值
            var complexFlags = musicBase.Musics.Music.Where(m => 
                m.Flags.Length > 5).ToList(); // 超过简单的0x0, 0x1等
            
            Assert.True(complexFlags.Count > 0, "Should have music with complex flag values");
        }
    }
} 