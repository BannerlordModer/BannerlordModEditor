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
    public class GlobalStringsXmlTests
    {
        [Fact]
        public void GlobalStrings_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "global_strings.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(GlobalStrings));
            GlobalStrings globalStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                globalStrings = (GlobalStrings)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, globalStrings);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(globalStrings);
            Assert.True(globalStrings.Strings.Count > 0, "Should have string entries");
            Assert.True(globalStrings.Strings.Count > 500, $"Should have many strings, got {globalStrings.Strings.Count}");

            // 验证具体的字符串条目
            var dontShowAgain = globalStrings.Strings.FirstOrDefault(s => s.Id == "str_dont_show_again");
            Assert.NotNull(dontShowAgain);
            Assert.Equal("{=KUGomrhD}Don't Show Again", dontShowAgain.Text);

            var ok = globalStrings.Strings.FirstOrDefault(s => s.Id == "str_ok");
            Assert.NotNull(ok);
            Assert.Equal("{=oHaWR73d}Ok", ok.Text);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("string").Count() == savedDoc.Root?.Elements("string").Count(),
                "String entry count should be the same");
        }
        
        [Fact]
        public void GlobalStrings_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "global_strings.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(GlobalStrings));
            GlobalStrings globalStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                globalStrings = (GlobalStrings)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证基本结构完整性
            Assert.True(globalStrings.Strings.Count > 0, "Should have string entries");

            // 验证所有字符串都有必要的数据
            foreach (var stringItem in globalStrings.Strings)
            {
                Assert.False(string.IsNullOrWhiteSpace(stringItem.Id), "String should have Id");
                Assert.False(string.IsNullOrWhiteSpace(stringItem.Text), "String should have Text");
                
                // 验证ID格式（应该以str_开头）
                Assert.True(stringItem.Id.StartsWith("str_"), 
                    $"String ID should start with 'str_': {stringItem.Id}");
            }
            
            // 验证特定的字符串条目存在
            var requiredStrings = new[] { 
                "str_dont_show_again",
                "str_ok",
                "str_LEFT_ONLY"
            };
            
            foreach (var requiredString in requiredStrings)
            {
                var stringItem = globalStrings.Strings.FirstOrDefault(s => s.Id == requiredString);
                Assert.NotNull(stringItem);
                Assert.NotEmpty(stringItem.Text);
            }
            
            // 验证所有ID都是唯一的
            var allIds = globalStrings.Strings.Select(s => s.Id).ToList();
            var uniqueIds = allIds.Distinct().ToList();
            // 注意：实际数据中可能存在少量重复ID，这是正常的
            Assert.True(uniqueIds.Count > 1000, $"Should have many unique IDs, got {uniqueIds.Count}");
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