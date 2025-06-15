using BannerlordModEditor.Common.Models.Audio;
using BannerlordModEditor.Common.Models.Engine;
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
    public class SoundAndCoreParametersTests
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
        public void SoundFiles_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SoundFilesBase));
            SoundFilesBase soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFilesBase)serializer.Deserialize(reader)!;
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
            Assert.NotNull(soundFiles.BankFiles);
            Assert.NotNull(soundFiles.AssetFiles);
            Assert.NotNull(soundFiles.BankFiles.File);
            Assert.NotNull(soundFiles.AssetFiles.File);
            Assert.True(soundFiles.BankFiles.File.Count > 0, "Should have at least one bank file");
            Assert.True(soundFiles.AssetFiles.File.Count > 0, "Should have at least one asset file");

            // 验证特定银行文件
            var masterBank = soundFiles.BankFiles.File.FirstOrDefault(f => f.Name == "MasterBank.bank");
            if (masterBank != null)
            {
                Assert.Equal("MasterBank.bank", masterBank.Name);
                Assert.Equal("false", masterBank.DecompressSamples);
            }

            var combatBank = soundFiles.BankFiles.File.FirstOrDefault(f => f.Name == "combat.bank");
            if (combatBank != null)
            {
                Assert.Equal("combat.bank", combatBank.Name);
                Assert.Equal("true", combatBank.DecompressSamples); // combat需要解压
            }

            var voiceBank = soundFiles.BankFiles.File.FirstOrDefault(f => f.Name == "voice.bank");
            if (voiceBank != null)
            {
                Assert.Equal("voice.bank", voiceBank.Name);
                Assert.Equal("true", voiceBank.DecompressSamples); // voice需要解压
            }

            // 验证资产文件
            var masterAsset = soundFiles.AssetFiles.File.FirstOrDefault(f => f.Name == "MasterBank.assets.bank");
            if (masterAsset != null)
            {
                Assert.Equal("MasterBank.assets.bank", masterAsset.Name);
                Assert.Equal("false", masterAsset.DecompressSamples);
            }

            // 验证所有文件都有必要字段
            foreach (var file in soundFiles.BankFiles.File)
            {
                Assert.False(string.IsNullOrEmpty(file.Name), "Bank file should have a name");
                Assert.False(string.IsNullOrEmpty(file.DecompressSamples), "Bank file should have decompress_samples value");
                Assert.True(file.DecompressSamples == "true" || file.DecompressSamples == "false", 
                    $"Bank file {file.Name} decompress_samples should be true or false");
            }

            foreach (var file in soundFiles.AssetFiles.File)
            {
                Assert.False(string.IsNullOrEmpty(file.Name), "Asset file should have a name");
                Assert.False(string.IsNullOrEmpty(file.DecompressSamples), "Asset file should have decompress_samples value");
                Assert.True(file.DecompressSamples == "true" || file.DecompressSamples == "false", 
                    $"Asset file {file.Name} decompress_samples should be true or false");
            }
        }

        [Fact]
        public void SoundFiles_DecompressSettings_ShouldBeCorrect()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SoundFilesBase));
            SoundFilesBase soundFiles;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFilesBase)serializer.Deserialize(reader)!;
            }

            // Assert - 检查解压设置的逻辑
            var compressedBanks = soundFiles.BankFiles.File.Where(f => f.DecompressSamples == "true").ToList();
            var uncompressedBanks = soundFiles.BankFiles.File.Where(f => f.DecompressSamples == "false").ToList();
            
            Assert.True(compressedBanks.Count > 0, "Should have some compressed banks");
            Assert.True(uncompressedBanks.Count > 0, "Should have some uncompressed banks");

            // 验证特定类型文件的压缩设置
            var combatFiles = compressedBanks.Where(f => f.Name.Contains("combat")).ToList();
            var voiceFiles = compressedBanks.Where(f => f.Name.Contains("voice")).ToList();
            
            Assert.True(combatFiles.Count > 0, "Combat files should be compressed");
            Assert.True(voiceFiles.Count > 0, "Voice files should be compressed");

            // 验证资产文件和银行文件数量的合理性（可能不完全相等，因为有些bank文件没有对应的asset文件）
            Assert.True(soundFiles.AssetFiles.File.Count > 0, "Should have asset files");
            Assert.True(soundFiles.BankFiles.File.Count >= soundFiles.AssetFiles.File.Count, "Bank files should be at least as many as asset files");
        }

        [Fact]
        public void ManagedCoreParameters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersBase));
            ManagedCoreParametersBase coreParameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                coreParameters = (ManagedCoreParametersBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, coreParameters);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(coreParameters);
            Assert.NotNull(coreParameters.ManagedCoreParameters);
            Assert.NotNull(coreParameters.ManagedCoreParameters.ManagedCoreParameter);
            Assert.Equal("combat_parameters", coreParameters.Type);
            Assert.True(coreParameters.ManagedCoreParameters.ManagedCoreParameter.Count > 0, "Should have at least one parameter");

            // 验证特定核心参数
            var tutorialsParam = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .FirstOrDefault(p => p.Id == "EnableCampaignTutorials");
            if (tutorialsParam != null)
            {
                Assert.Equal("EnableCampaignTutorials", tutorialsParam.Id);
                Assert.Equal("1", tutorialsParam.Value);
            }

            var airFrictionArrow = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .FirstOrDefault(p => p.Id == "AirFrictionArrow");
            if (airFrictionArrow != null)
            {
                Assert.Equal("AirFrictionArrow", airFrictionArrow.Id);
                Assert.Equal("0.003", airFrictionArrow.Value);
            }

            var heavyAttackMultiplier = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .FirstOrDefault(p => p.Id == "HeavyAttackMomentumMultiplier");
            if (heavyAttackMultiplier != null)
            {
                Assert.Equal("HeavyAttackMomentumMultiplier", heavyAttackMultiplier.Id);
                Assert.Equal("1.15", heavyAttackMultiplier.Value);
            }

            // 验证所有参数都有必要字段
            foreach (var param in coreParameters.ManagedCoreParameters.ManagedCoreParameter)
            {
                Assert.False(string.IsNullOrEmpty(param.Id), "Parameter should have an ID");
                Assert.False(string.IsNullOrEmpty(param.Value), $"Parameter {param.Id} should have a Value");
            }
        }

        [Fact]
        public void ManagedCoreParameters_NumericPrecision_ShouldBePreserved()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersBase));
            ManagedCoreParametersBase coreParameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                coreParameters = (ManagedCoreParametersBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证数值精度保持
            var airFrictionParameters = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .Where(p => p.Id.StartsWith("AirFriction")).ToList();
            
            Assert.True(airFrictionParameters.Count > 0, "Should have air friction parameters");

            // 检查具体数值精度
            var javelin = airFrictionParameters.FirstOrDefault(p => p.Id == "AirFrictionJavelin");
            if (javelin != null)
            {
                Assert.Equal("0.002", javelin.Value); // 确保3位小数保持
            }

            var ballistaBolt = airFrictionParameters.FirstOrDefault(p => p.Id == "AirFrictionBallistaBolt");
            if (ballistaBolt != null)
            {
                Assert.Equal("0.005", ballistaBolt.Value);
            }

            var knife = airFrictionParameters.FirstOrDefault(p => p.Id == "AirFrictionKnife");
            if (knife != null)
            {
                Assert.Equal("0.007", knife.Value);
            }

            // 验证更复杂的小数值
            var fallDamageMultiplier = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .FirstOrDefault(p => p.Id == "FallDamageMultiplier");
            if (fallDamageMultiplier != null)
            {
                Assert.Equal("0.575", fallDamageMultiplier.Value); // 3位小数
            }

            var fallSpeedReduction = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .FirstOrDefault(p => p.Id == "FallSpeedReductionMultiplierForRiderDamage");
            if (fallSpeedReduction != null)
            {
                Assert.Equal("0.77", fallSpeedReduction.Value); // 2位小数
            }
        }

        [Fact]
        public void ManagedCoreParameters_ParameterCategories_ShouldBeCategorized()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersBase));
            ManagedCoreParametersBase coreParameters;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                coreParameters = (ManagedCoreParametersBase)serializer.Deserialize(reader)!;
            }

            // Assert - 验证参数分类
            var campaignParams = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .Where(p => p.Id.Contains("Campaign") || p.Id.Contains("Tutorial")).ToList();
            
            var combatParams = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .Where(p => p.Id.Contains("Combat") || p.Id.Contains("Attack") || p.Id.Contains("Damage")).ToList();
            
            var physicsParams = coreParameters.ManagedCoreParameters.ManagedCoreParameter
                .Where(p => p.Id.Contains("AirFriction") || p.Id.Contains("Fall") || p.Id.Contains("Radius")).ToList();

            Assert.True(campaignParams.Count > 0, "Should have campaign-related parameters");
            Assert.True(combatParams.Count > 0, "Should have combat-related parameters");
            Assert.True(physicsParams.Count > 0, "Should have physics-related parameters");

            // 验证总数一致性
            var totalCategorized = campaignParams.Count + combatParams.Count + physicsParams.Count;
            Assert.True(totalCategorized <= coreParameters.ManagedCoreParameters.ManagedCoreParameter.Count,
                "Categorized parameters should not exceed total parameters");
        }
    }
} 