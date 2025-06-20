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
        public void SoundFiles_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SoundFiles));
            SoundFiles soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFiles)serializer.Deserialize(reader)!;
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

            // Assert - 基本结构验证
            Assert.NotNull(soundFiles);
            Assert.Equal("sound", soundFiles.Type);
            
            // 验证 bank_files
            Assert.NotNull(soundFiles.BankFiles);
            Assert.Equal(12, soundFiles.BankFiles.Files.Count);
            
            // 验证 asset_files
            Assert.NotNull(soundFiles.AssetFiles);
            Assert.Equal(11, soundFiles.AssetFiles.Files.Count);

            // 验证具体的音频文件
            var masterBank = soundFiles.BankFiles.Files.FirstOrDefault(f => f.Name == "MasterBank.bank");
            Assert.NotNull(masterBank);
            Assert.Equal("false", masterBank.DecompressSamples);
            
            var combatBank = soundFiles.BankFiles.Files.FirstOrDefault(f => f.Name == "combat.bank");
            Assert.NotNull(combatBank);
            Assert.Equal("true", combatBank.DecompressSamples);
            
            var voiceAsset = soundFiles.AssetFiles.Files.FirstOrDefault(f => f.Name == "voice.assets.bank");
            Assert.NotNull(voiceAsset);
            Assert.Equal("true", voiceAsset.DecompressSamples);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Element("bank_files")?.Elements("file").Count() == 
                       savedDoc.Root?.Element("bank_files")?.Elements("file").Count(),
                "Bank files count should be the same");
            Assert.True(originalDoc.Root?.Element("asset_files")?.Elements("file").Count() == 
                       savedDoc.Root?.Element("asset_files")?.Elements("file").Count(),
                "Asset files count should be the same");
        }
        
        [Fact]
        public void SoundFiles_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SoundFiles));
            SoundFiles soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFiles)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本结构完整性
            Assert.Equal("sound", soundFiles.Type);
            Assert.True(soundFiles.BankFiles.Files.Count > 0, "Should have bank files");
            Assert.True(soundFiles.AssetFiles.Files.Count > 0, "Should have asset files");

            // 验证所有音频文件都有必要的数据
            foreach (var file in soundFiles.BankFiles.Files)
            {
                Assert.False(string.IsNullOrWhiteSpace(file.Name), "Bank file should have name");
                Assert.False(string.IsNullOrWhiteSpace(file.DecompressSamples), "Bank file should have decompress_samples");
                Assert.True(file.DecompressSamples == "true" || file.DecompressSamples == "false",
                    $"DecompressSamples should be true or false: {file.DecompressSamples}");
                Assert.True(file.Name.EndsWith(".bank"), $"Bank file should end with .bank: {file.Name}");
            }

            foreach (var file in soundFiles.AssetFiles.Files)
            {
                Assert.False(string.IsNullOrWhiteSpace(file.Name), "Asset file should have name");
                Assert.False(string.IsNullOrWhiteSpace(file.DecompressSamples), "Asset file should have decompress_samples");
                Assert.True(file.DecompressSamples == "true" || file.DecompressSamples == "false",
                    $"DecompressSamples should be true or false: {file.DecompressSamples}");
                Assert.True(file.Name.EndsWith(".assets.bank"), $"Asset file should end with .assets.bank: {file.Name}");
            }
            
            // 验证特定音频文件存在
            var requiredBankFiles = new[] { 
                "MasterBank.bank", "combat.bank", "music.bank", "voice.bank",
                "ambient.bank", "campaign.bank", "foley.bank", "footsteps.bank",
                "physics.bank", "siege.bank", "ui.bank"
            };
            
            foreach (var requiredFile in requiredBankFiles)
            {
                var file = soundFiles.BankFiles.Files.FirstOrDefault(f => f.Name == requiredFile);
                Assert.NotNull(file);
            }
            
            var requiredAssetFiles = new[] { 
                "MasterBank.assets.bank", "combat.assets.bank", "music.assets.bank", "voice.assets.bank",
                "ambient.assets.bank", "campaign.assets.bank", "foley.assets.bank", "footsteps.assets.bank",
                "physics.assets.bank", "siege.assets.bank", "ui.assets.bank"
            };
            
            foreach (var requiredFile in requiredAssetFiles)
            {
                var file = soundFiles.AssetFiles.Files.FirstOrDefault(f => f.Name == requiredFile);
                Assert.NotNull(file);
            }
            
            // 验证解压缩配置合理性
            // combat 和 voice 相关的文件通常需要解压缩以提高性能
            var combatBank = soundFiles.BankFiles.Files.FirstOrDefault(f => f.Name == "combat.bank");
            Assert.Equal("true", combatBank?.DecompressSamples);
            
            var voiceBank = soundFiles.BankFiles.Files.FirstOrDefault(f => f.Name == "voice.bank");
            Assert.Equal("true", voiceBank?.DecompressSamples);
            
            var combatAsset = soundFiles.AssetFiles.Files.FirstOrDefault(f => f.Name == "combat.assets.bank");
            Assert.Equal("true", combatAsset?.DecompressSamples);
            
            var voiceAsset = soundFiles.AssetFiles.Files.FirstOrDefault(f => f.Name == "voice.assets.bank");
            Assert.Equal("true", voiceAsset?.DecompressSamples);
            
            // 验证MasterBank通常不需要解压缩
            var masterBank = soundFiles.BankFiles.Files.FirstOrDefault(f => f.Name == "MasterBank.bank");
            Assert.Equal("false", masterBank?.DecompressSamples);
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