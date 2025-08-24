using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Services;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class LanguageXmlAdaptationTests
    {
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