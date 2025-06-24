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
    public class ModuleStringsXmlTests
    {
        [Fact]
        public void ModuleStrings_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "module_strings.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ModuleStrings));
            ModuleStrings moduleStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                moduleStrings = (ModuleStrings)serializer.Deserialize(reader);
            }
            
            // 验证反序列化的数据
            Assert.NotNull(moduleStrings);
            Assert.NotNull(moduleStrings.Strings);
            Assert.True(moduleStrings.Strings.Count > 0, "Should have at least one string entry");
            
            // 验证数据内容不为空
            Assert.True(moduleStrings.Strings.All(s => !string.IsNullOrEmpty(s.Id)), "All string entries should have valid IDs");
            Assert.True(moduleStrings.Strings.All(s => !string.IsNullOrEmpty(s.Text)), "All string entries should have valid text");
            
            // Act - 序列化回字符串
            string serializedXml;
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.UTF8
                }))
                {
                    serializer.Serialize(xmlWriter, moduleStrings);
                }
                serializedXml = stringWriter.ToString();
            }
            
            // Assert - 结构化比较
            var originalXml = File.ReadAllText(xmlPath);
            
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);
            
            // 验证根节点
            Assert.Equal(originalDoc.Root.Name.LocalName, serializedDoc.Root.Name.LocalName);
            
            // 验证字符串条目数量
            var originalStrings = originalDoc.Root.Elements("string").ToList();
            var serializedStrings = serializedDoc.Root.Elements("string").ToList();
            
            Assert.Equal(originalStrings.Count, serializedStrings.Count);
            
            // 验证每个字符串条目的完整性
            for (int i = 0; i < originalStrings.Count; i++)
            {
                var originalString = originalStrings[i];
                var serializedString = serializedStrings[i];
                
                // 验证 id 属性
                var originalId = originalString.Attribute("id")?.Value;
                var serializedId = serializedString.Attribute("id")?.Value;
                Assert.Equal(originalId, serializedId);
                
                // 验证 text 属性
                var originalText = originalString.Attribute("text")?.Value;
                var serializedText = serializedString.Attribute("text")?.Value;
                Assert.Equal(originalText, serializedText);
            }
            
            // 验证总体数据完整性
            Assert.True(moduleStrings.Strings.Count > 1000, $"Should have many string entries, got {moduleStrings.Strings.Count}");
        }
    }
}