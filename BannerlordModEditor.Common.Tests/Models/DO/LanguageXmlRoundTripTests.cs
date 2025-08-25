using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Language;
using BannerlordModEditor.Common.Tests;
using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.Common.Tests.Models.DO
{
    public class LanguageXmlRoundTripTests
    {
        private readonly ITestOutputHelper _output;
        
        public LanguageXmlRoundTripTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        private void WriteOutput(string message)
        {
            _output.WriteLine(message);
        }

        [Theory]
        [InlineData("std_functions.xml")]
        [InlineData("std_TaleWorlds_Core.xml")]
        [InlineData("std_common_strings_xml.xml")]
        [InlineData("std_global_strings_xml.xml")]
        [InlineData("std_module_strings_xml.xml")]
        [InlineData("std_native_strings_xml.xml")]
        [InlineData("std_multiplayer_strings_xml.xml")]
        [InlineData("std_crafting_pieces_xml.xml")]
        [InlineData("std_item_modifiers_xml.xml")]
        [InlineData("std_mpbadges_xml.xml")]
        [InlineData("std_mpcharacters_xml.xml")]
        [InlineData("std_mpclassdivisions_xml.xml")]
        [InlineData("std_mpitems_xml.xml")]
        [InlineData("std_photo_mode_strings_xml.xml")]
        [InlineData("std_siegeengines_xml.xml")]
        public async Task LanguageXmlFiles_ShouldPassRoundTripTest(string xmlFileName)
        {
            // Arrange
            string xmlPath = Path.Combine("example", "ModuleData", "Languages", xmlFileName);
            if (!File.Exists(xmlPath))
            {
                WriteOutput($"Skipping test for {xmlFileName} - file not found");
                return;
            }

            string xml = await File.ReadAllTextAsync(xmlPath);
            WriteOutput($"Testing {xmlFileName} ({xml.Length} characters)");

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            
            var isEqual = XmlTestUtils.AreStructurallyEqual(xml, serialized);
            Assert.True(isEqual, $"Round-trip should preserve XML structure for {xmlFileName}");
            
            // 验证基本属性
            Assert.Equal("string", deserialized.Type);
            Assert.Equal("string", roundTripResult.Type);
            
            // 验证标签
            if (deserialized.HasTags)
            {
                Assert.True(roundTripResult.HasTags, "Round-trip should preserve tags");
                Assert.Equal(deserialized.Tags.Tags.Count, roundTripResult.Tags.Tags.Count);
                
                for (int i = 0; i < deserialized.Tags.Tags.Count; i++)
                {
                    Assert.Equal(deserialized.Tags.Tags[i].Language, roundTripResult.Tags.Tags[i].Language);
                }
            }
            
            // 验证函数
            if (deserialized.HasFunctions)
            {
                Assert.True(roundTripResult.HasFunctions, "Round-trip should preserve functions");
                Assert.Equal(deserialized.Functions.Count, roundTripResult.Functions.Count);
                
                for (int i = 0; i < deserialized.Functions.Count; i++)
                {
                    Assert.Equal(deserialized.Functions[i].FunctionName, roundTripResult.Functions[i].FunctionName);
                    Assert.Equal(deserialized.Functions[i].FunctionBody, roundTripResult.Functions[i].FunctionBody);
                }
            }
            
            // 验证字符串
            if (deserialized.HasStrings)
            {
                Assert.True(roundTripResult.HasStrings, "Round-trip should preserve strings");
                Assert.Equal(deserialized.Strings.Count, roundTripResult.Strings.Count);
                
                for (int i = 0; i < deserialized.Strings.Count; i++)
                {
                    Assert.Equal(deserialized.Strings[i].Id, roundTripResult.Strings[i].Id);
                    Assert.Equal(deserialized.Strings[i].Text, roundTripResult.Strings[i].Text);
                }
            }
            
            WriteOutput($"✅ {xmlFileName} passed round-trip test");
        }

        [Fact]
        public async Task AllLanguageXmlFilesInExample_ShouldPassRoundTripTest()
        {
            // Arrange
            string languagesDir = Path.Combine("example", "ModuleData", "Languages");
            if (!Directory.Exists(languagesDir))
            {
                WriteOutput($"Languages directory not found: {languagesDir}");
                return;
            }

            // 获取所有语言相关的XML文件
            var languageFiles = Directory.GetFiles(languagesDir, "*.xml", SearchOption.AllDirectories)
                .Where(f => Path.GetFileName(f).StartsWith("std_"))
                .ToList();

            WriteOutput($"Found {languageFiles.Count} language XML files to test");

            int passed = 0;
            int failed = 0;
            int skipped = 0;

            // 测试每个文件
            foreach (var xmlFile in languageFiles)
            {
                try
                {
                    string xml = await File.ReadAllTextAsync(xmlFile);
                    var fileName = Path.GetFileName(xmlFile);

                    // Act - Round Trip Test
                    var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
                    var serialized = XmlTestUtils.Serialize(deserialized);
                    var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

                    // Assert
                    Assert.NotNull(deserialized);
                    Assert.NotNull(roundTripResult);
                    
                    var isEqual = XmlTestUtils.AreStructurallyEqual(xml, serialized);
                    Assert.True(isEqual, $"Round-trip should preserve XML structure for {fileName}");

                    passed++;
                    WriteOutput($"✅ {fileName} passed round-trip test");
                }
                catch (Exception ex)
                {
                    failed++;
                    WriteOutput($"❌ {Path.GetFileName(xmlFile)} failed: {ex.Message}");
                }
            }

            // 总结结果
            WriteOutput($"");
            WriteOutput($"=== Language XML Round-Trip Test Summary ===");
            WriteOutput($"Total files tested: {languageFiles.Count}");
            WriteOutput($"✅ Passed: {passed}");
            WriteOutput($"❌ Failed: {failed}");
            WriteOutput($"⏭️ Skipped: {skipped}");

            // 如果有失败的测试，抛出异常
            if (failed > 0)
            {
                Assert.True(false, $"{failed} language XML files failed round-trip test");
            }

            Assert.True(passed > 0, "At least one language XML file should be tested");
        }

        [Fact]
        public async Task LanguageXmlWithMixedContent_ShouldHandleCorrectly()
        {
            // Arrange - 创建一个包含所有元素类型的复杂语言XML
            string complexXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <functions>
    <function functionName=""PLURAL"" functionBody=""{?$0.Plural}{$0.Plural}{?}{$0}{.s}{?}""/>
    <function functionName=""MAX"" functionBody=""{?$0>$1}{$0}{?}{$1}{?}""/>
  </functions>
  <strings>
    <string id=""hello"" text=""Hello"" />
    <string id=""world"" text=""World"" />
    <string id=""test"" text=""{PLUS_OR_MINUS}{BONUSEFFECT}"" />
  </strings>
</base>";

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(complexXml);
            var serialized = XmlTestUtils.Serialize(deserialized, complexXml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            
            var isEqual = XmlTestUtils.AreStructurallyEqual(complexXml, serialized);
            Assert.True(isEqual, "Complex language XML should pass round-trip test");

            // 验证所有内容都被正确保留
            Assert.True(deserialized.HasTags);
            Assert.True(deserialized.HasFunctions);
            Assert.True(deserialized.HasStrings);
            
            Assert.Equal("English", deserialized.Tags.Tags[0].Language);
            Assert.Equal(2, deserialized.Functions.Count);
            Assert.Equal(3, deserialized.Strings.Count);
            
            WriteOutput("✅ Complex language XML with mixed content passed round-trip test");
        }
    }
}