using BannerlordModEditor.Common.Models.Audio;
using System.IO;
using System.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SoundFilesXmlTests
    {
        [Fact]
        public void SoundFiles_Load_ShouldSucceed()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "soundfiles.xml");
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SoundFilesBase));

            // Act
            SoundFilesBase soundFiles;
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                soundFiles = (SoundFilesBase)serializer.Deserialize(reader)!;
            }

            // Assert
            Assert.NotNull(soundFiles);
            Assert.Equal("sound", soundFiles.Type);

            Assert.NotNull(soundFiles.BankFiles);
            Assert.NotNull(soundFiles.BankFiles.File);
            Assert.True(soundFiles.BankFiles.File.Count > 0);

            var combatBank = soundFiles.BankFiles.File.FirstOrDefault(f => f.Name == "combat.bank");
            Assert.NotNull(combatBank);
            Assert.Equal("true", combatBank.DecompressSamples);

            Assert.NotNull(soundFiles.AssetFiles);
            Assert.NotNull(soundFiles.AssetFiles.File);
            Assert.True(soundFiles.AssetFiles.File.Count > 0);

            var combatAsset = soundFiles.AssetFiles.File.FirstOrDefault(f => f.Name == "combat.assets.bank");
            Assert.NotNull(combatAsset);
            Assert.Equal("true", combatAsset.DecompressSamples);
        }
    }
} 