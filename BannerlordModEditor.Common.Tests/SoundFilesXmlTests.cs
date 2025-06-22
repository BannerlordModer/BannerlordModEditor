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
    public class SoundFilesXmlTests
    {
        [Fact]
        public void SoundFiles_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SoundFilesRoot));
            SoundFilesRoot soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFilesRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(soundFiles);
            Assert.Equal("sound", soundFiles.Type);
            
            // 验证 bank_files
            Assert.NotNull(soundFiles.BankFiles);
            Assert.NotNull(soundFiles.BankFiles.File);
            Assert.Equal(12, soundFiles.BankFiles.File.Length);
            
            // 验证 asset_files  
            Assert.NotNull(soundFiles.AssetFiles);
            Assert.NotNull(soundFiles.AssetFiles.File);
            Assert.Equal(11, soundFiles.AssetFiles.File.Length);
            
            // 验证具体的音频文件
            var masterBank = soundFiles.BankFiles.File.FirstOrDefault(f => f.Name == "MasterBank.bank");
            Assert.NotNull(masterBank);
            Assert.Equal("false", masterBank.DecompressSamples);
            
            var combatBank = soundFiles.BankFiles.File.FirstOrDefault(f => f.Name == "combat.bank");
            Assert.NotNull(combatBank);
            Assert.Equal("true", combatBank.DecompressSamples);
        }
        
        [Fact]
        public void SoundFiles_CanRoundtripXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SoundFilesRoot));
            SoundFilesRoot soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFilesRoot)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, soundFiles);
                }
                savedXml = writer.ToString();
            }
            
            // Assert - 验证XML可以被重新解析
            SoundFilesRoot reparsed;
            using (var reader = new StringReader(savedXml))
            {
                reparsed = (SoundFilesRoot)serializer.Deserialize(reader)!;
            }
            
            Assert.Equal(soundFiles.Type, reparsed.Type);
            Assert.Equal(soundFiles.BankFiles?.File?.Length, reparsed.BankFiles?.File?.Length);
            Assert.Equal(soundFiles.AssetFiles?.File?.Length, reparsed.AssetFiles?.File?.Length);
        }
        
        [Fact]
        public void SoundFiles_ValidateStructure()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SoundFilesRoot));
            SoundFilesRoot soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFilesRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有bank文件都有有效数据
            Assert.True(soundFiles.BankFiles?.File?.Length > 0);
            foreach (var file in soundFiles.BankFiles!.File!)
            {
                Assert.False(string.IsNullOrWhiteSpace(file.Name));
                Assert.False(string.IsNullOrWhiteSpace(file.DecompressSamples));
                Assert.True(file.DecompressSamples == "true" || file.DecompressSamples == "false");
                Assert.True(file.Name.EndsWith(".bank"));
            }
            
            // Assert - 验证所有asset文件都有有效数据
            Assert.True(soundFiles.AssetFiles?.File?.Length > 0);
            foreach (var file in soundFiles.AssetFiles!.File!)
            {
                Assert.False(string.IsNullOrWhiteSpace(file.Name));
                Assert.False(string.IsNullOrWhiteSpace(file.DecompressSamples));
                Assert.True(file.DecompressSamples == "true" || file.DecompressSamples == "false");
                Assert.True(file.Name.EndsWith(".assets.bank"));
            }
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