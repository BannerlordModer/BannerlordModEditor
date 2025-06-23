using BannerlordModEditor.Common.Models.Data;
using System;
using System.Collections.Generic;
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
        public void SoundFiles_ValidateFileNamingConventions()
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
            
            // Assert - 验证bank文件命名规范
            foreach (var bankFile in soundFiles.BankFiles!.File!)
            {
                Assert.True(bankFile.Name.EndsWith(".bank"), 
                           $"Bank file '{bankFile.Name}' should end with '.bank' extension");
                Assert.False(string.IsNullOrWhiteSpace(bankFile.Name));
                Assert.DoesNotContain(' ', bankFile.Name); // 文件名不应包含空格
                Assert.DoesNotContain('\\', bankFile.Name); // 不应包含路径分隔符
                Assert.DoesNotContain('/', bankFile.Name);
            }
            
            // Assert - 验证asset文件命名规范
            foreach (var assetFile in soundFiles.AssetFiles!.File!)
            {
                Assert.True(assetFile.Name.EndsWith(".assets.bank"), 
                           $"Asset file '{assetFile.Name}' should end with '.assets.bank' extension");
                Assert.False(string.IsNullOrWhiteSpace(assetFile.Name));
                Assert.DoesNotContain(' ', assetFile.Name);
                Assert.DoesNotContain('\\', assetFile.Name);
                Assert.DoesNotContain('/', assetFile.Name);
            }
        }
        
        [Fact]
        public void SoundFiles_ValidateDecompressSamplesValues()
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
            
            // Assert - 验证所有decompress_samples值都是有效的布尔字符串
            var validValues = new HashSet<string> { "true", "false" };
            
            foreach (var bankFile in soundFiles.BankFiles!.File!)
            {
                Assert.True(validValues.Contains(bankFile.DecompressSamples), 
                           $"Bank file '{bankFile.Name}' has invalid decompress_samples value: '{bankFile.DecompressSamples}'");
                Assert.False(string.IsNullOrWhiteSpace(bankFile.DecompressSamples));
            }
            
            foreach (var assetFile in soundFiles.AssetFiles!.File!)
            {
                Assert.True(validValues.Contains(assetFile.DecompressSamples), 
                           $"Asset file '{assetFile.Name}' has invalid decompress_samples value: '{assetFile.DecompressSamples}'");
                Assert.False(string.IsNullOrWhiteSpace(assetFile.DecompressSamples));
            }
        }
        
        [Fact]
        public void SoundFiles_ValidateBankAndAssetCorrespondence()
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
            
            // Assert - 验证大多数bank文件都有对应的asset文件
            var bankNames = soundFiles.BankFiles!.File!.Select(f => f.Name.Replace(".bank", "")).ToHashSet();
            var assetNames = soundFiles.AssetFiles!.File!.Select(f => f.Name.Replace(".assets.bank", "")).ToHashSet();
            
            // 特殊情况：MasterBank.strings.bank 不需要对应的asset文件
            bankNames.Remove("MasterBank.strings");
            
            // 大多数bank文件应该有对应的asset文件
            var banksWithAssets = bankNames.Intersect(assetNames).Count();
            var totalBanks = bankNames.Count;
            
            Assert.True(banksWithAssets >= totalBanks * 0.8, // 至少80%的bank文件有对应的asset文件
                       $"Only {banksWithAssets} out of {totalBanks} bank files have corresponding asset files");
        }
        
        [Fact]
        public void SoundFiles_ValidateSpecificAudioBanks()
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
            
            var bankDict = soundFiles.BankFiles!.File!.ToDictionary(f => f.Name, f => f.DecompressSamples);
            var assetDict = soundFiles.AssetFiles!.File!.ToDictionary(f => f.Name, f => f.DecompressSamples);
            
            // Assert - 验证关键音频银行存在
            var expectedBanks = new[] { "MasterBank.bank", "combat.bank", "ui.bank", "music.bank", "voice.bank" };
            foreach (var expectedBank in expectedBanks)
            {
                Assert.True(bankDict.ContainsKey(expectedBank), 
                           $"Essential bank file '{expectedBank}' is missing");
            }
            
            // Assert - 验证MasterBank是必需的且不应解压缩
            Assert.Equal("false", bankDict["MasterBank.bank"]);
            
            // Assert - 验证性能相关的音频银行设置
            // combat和voice应该解压缩以获得更好的性能
            Assert.Equal("true", bankDict["combat.bank"]);
            Assert.Equal("true", bankDict["voice.bank"]);
            
            // UI和music通常不解压缩以节省内存
            Assert.Equal("false", bankDict["ui.bank"]);
            Assert.Equal("false", bankDict["music.bank"]);
        }
        
        [Fact]
        public void SoundFiles_ValidateDecompressionConsistency()
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
            
            // Assert - 验证bank和对应的asset文件具有相同的解压缩设置
            var bankDict = soundFiles.BankFiles!.File!.ToDictionary(
                f => f.Name.Replace(".bank", ""), 
                f => f.DecompressSamples);
            var assetDict = soundFiles.AssetFiles!.File!.ToDictionary(
                f => f.Name.Replace(".assets.bank", ""), 
                f => f.DecompressSamples);
            
            foreach (var kvp in bankDict)
            {
                var bankName = kvp.Key;
                var bankDecompress = kvp.Value;
                
                // 跳过特殊的MasterBank.strings
                if (bankName == "MasterBank.strings") continue;
                
                if (assetDict.ContainsKey(bankName))
                {
                    Assert.True(bankDecompress == assetDict[bankName], 
                               $"Bank '{bankName}.bank' and asset '{bankName}.assets.bank' have inconsistent decompress_samples values");
                }
            }
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
            
            // 验证所有文件名和设置都保持不变
            var originalBanks = soundFiles.BankFiles!.File!.ToDictionary(f => f.Name, f => f.DecompressSamples);
            var reparsedBanks = reparsed.BankFiles!.File!.ToDictionary(f => f.Name, f => f.DecompressSamples);
            
            Assert.Equal(originalBanks.Count, reparsedBanks.Count);
            foreach (var kvp in originalBanks)
            {
                Assert.True(reparsedBanks.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, reparsedBanks[kvp.Key]);
            }
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
        
        [Fact]
        public void SoundFiles_ValidateFileNameUniqueness()
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
            
            // Assert - 验证bank文件名唯一性
            var bankNames = soundFiles.BankFiles!.File!.Select(f => f.Name).ToList();
            var uniqueBankNames = bankNames.Distinct().ToList();
            Assert.Equal(bankNames.Count, uniqueBankNames.Count);
            
            // Assert - 验证asset文件名唯一性
            var assetNames = soundFiles.AssetFiles!.File!.Select(f => f.Name).ToList();
            var uniqueAssetNames = assetNames.Distinct().ToList();
            Assert.Equal(assetNames.Count, uniqueAssetNames.Count);
        }
        
        [Fact]
        public void SoundFiles_ValidateXmlFormatting()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""sound"">
    <bank_files>
        <file name=""test.bank"" decompress_samples=""false"" />
    </bank_files>
    <asset_files>
        <file name=""test.assets.bank"" decompress_samples=""false"" />
    </asset_files>
</base>";

            var serializer = new XmlSerializer(typeof(SoundFilesRoot));

            // Act
            SoundFilesRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (SoundFilesRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("sound", result.Type);
            Assert.NotNull(result.BankFiles);
            Assert.Single(result.BankFiles.File!);
            Assert.Equal("test.bank", result.BankFiles.File![0].Name);
            
            Assert.NotNull(result.AssetFiles);
            Assert.Single(result.AssetFiles.File!);
            Assert.Equal("test.assets.bank", result.AssetFiles.File![0].Name);
        }

        [Fact]
        public void SoundFiles_EmptyCollections_HandledGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""sound"">
    <bank_files>
    </bank_files>
    <asset_files>
    </asset_files>
</base>";

            var serializer = new XmlSerializer(typeof(SoundFilesRoot));

            // Act
            SoundFilesRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (SoundFilesRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("sound", result.Type);
            Assert.NotNull(result.BankFiles);
            Assert.NotNull(result.AssetFiles);
            // 空集合应该被处理为null或空数组
            Assert.True(result.BankFiles.File == null || result.BankFiles.File.Length == 0);
            Assert.True(result.AssetFiles.File == null || result.AssetFiles.File.Length == 0);
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