using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Services;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class LanguageXmlAdaptationTests
    {
        [Fact]
        public void FileDiscoveryService_ShouldRecognizeLanguageXmlFilesAsAdapted()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, ".."));
            var xmlDir = Path.Combine(projectRoot, "example/ModuleData");
            var modelsDir = Path.Combine(projectRoot, "BannerlordModEditor.Common/Models");
            
            // Skip test if example directory doesn't exist (GitHub Actions environment)
            if (!Directory.Exists(xmlDir))
            {
                // 在GitHub Actions环境中跳过此测试
                return;
            }
            
            var service = new FileDiscoveryService(xmlDir, modelsDir);
            
            // Test language-related XML files that should be adapted
            var languageFiles = new[]
            {
                "std_functions.xml",
                "std_TaleWorlds_Core.xml",
                "std_common_strings_xml.xml",
                "std_global_strings_xml.xml",
                "std_module_strings_xml.xml",
                "std_native_strings_xml.xml",
                "std_multiplayer_strings_xml.xml",
                "std_crafting_pieces_xml.xml",
                "std_item_modifiers_xml.xml",
                "std_mpbadges_xml.xml",
                "std_mpcharacters_xml.xml",
                "std_mpclassdivisions_xml.xml",
                "std_mpitems_xml.xml",
                "std_photo_mode_strings_xml.xml",
                "std_siegeengines_xml.xml"
            };

            // Act & Assert
            foreach (var languageFile in languageFiles)
            {
                var isAdapted = service.IsFileAdapted(languageFile);
                Assert.True(isAdapted, $"Language file {languageFile} should be recognized as adapted");
            }
        }

        [Fact]
        public void FileDiscoveryService_ShouldConvertLanguageXmlNamesToLanguageBase()
        {
            // Arrange
            var service = new FileDiscoveryService();
            
            // Test language-related XML files that should map to LanguageBase
            var testCases = new[]
            {
                ("std_functions.xml", "LanguageBase"),
                ("std_TaleWorlds_Core.xml", "LanguageBase"),
                ("std_common_strings_xml.xml", "LanguageBase"),
                ("std_global_strings_xml.xml", "LanguageBase"),
                ("std_module_strings_xml.xml", "LanguageBase"),
                ("std_native_strings_xml.xml", "LanguageBase"),
                ("std_multiplayer_strings_xml.xml", "LanguageBase"),
                ("std_crafting_pieces_xml.xml", "LanguageBase"),
                ("std_item_modifiers_xml.xml", "LanguageBase"),
                ("std_mpbadges_xml.xml", "LanguageBase"),
                ("std_mpcharacters_xml.xml", "LanguageBase"),
                ("std_mpclassdivisions_xml.xml", "LanguageBase"),
                ("std_mpitems_xml.xml", "LanguageBase"),
                ("std_photo_mode_strings_xml.xml", "LanguageBase"),
                ("std_siegeengines_xml.xml", "LanguageBase")
            };

            // Act & Assert
            foreach (var (xmlFile, expectedModelName) in testCases)
            {
                var actualModelName = service.ConvertToModelName(xmlFile);
                Assert.Equal(expectedModelName, actualModelName);
            }
        }

        [Fact]
        public async Task FileDiscoveryService_ShouldNotIncludeLanguageFilesInUnadaptedList()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, ".."));
            var xmlDir = Path.Combine(projectRoot, "example/ModuleData");
            var modelsDir = Path.Combine(projectRoot, "BannerlordModEditor.Common/Models");
            
            // Skip test if example directory doesn't exist (GitHub Actions environment)
            if (!Directory.Exists(xmlDir))
            {
                // 在GitHub Actions环境中跳过此测试
                return;
            }
            
            var service = new FileDiscoveryService(xmlDir, modelsDir);
            
            // Act
            var unadaptedFiles = await service.FindUnadaptedFilesAsync();
            
            // Assert
            var languageFileNames = new[]
            {
                "std_functions.xml",
                "std_TaleWorlds_Core.xml",
                "std_common_strings_xml.xml",
                "std_global_strings_xml.xml",
                "std_module_strings_xml.xml",
                "std_native_strings_xml.xml",
                "std_multiplayer_strings_xml.xml",
                "std_crafting_pieces_xml.xml",
                "std_item_modifiers_xml.xml",
                "std_mpbadges_xml.xml",
                "std_mpcharacters_xml.xml",
                "std_mpclassdivisions_xml.xml",
                "std_mpitems_xml.xml",
                "std_photo_mode_strings_xml.xml",
                "std_siegeengines_xml.xml"
            };

            foreach (var languageFileName in languageFileNames)
            {
                var found = unadaptedFiles.Any(f => f.FileName.Equals(languageFileName, StringComparison.OrdinalIgnoreCase));
                Assert.False(found, $"Language file {languageFileName} should not be in unadapted files list");
            }
        }

        [Fact]
        public void NamingConventionMapper_ShouldMapLanguageFilesCorrectly()
        {
            // Arrange & Act & Assert
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_functions"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_TaleWorlds_Core"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_common_strings_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_global_strings_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_module_strings_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_native_strings_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_multiplayer_strings_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_crafting_pieces_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_item_modifiers_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_mpbadges_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_mpcharacters_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_mpclassdivisions_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_mpitems_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_photo_mode_strings_xml"));
            Assert.Equal("LanguageBase", NamingConventionMapper.GetMappedClassName("std_siegeengines_xml"));
        }
    }
}