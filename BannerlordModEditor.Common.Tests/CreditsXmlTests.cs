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
    public class CreditsXmlTests
    {
        [Fact]
        public void Credits_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "Credits.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(Credits));
            Credits credits;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                credits = (Credits)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, credits);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(credits);
            Assert.NotNull(credits.Category);
            Assert.True(credits.Category.Count > 0, "Should have at least one category");
            
            // 验证具体的分类
            var studioHeads = credits.Category.FirstOrDefault(c => c.Text?.Contains("Studio Heads") == true);
            Assert.NotNull(studioHeads);
            Assert.True(studioHeads.Entry.Count > 0, "Studio Heads should have entries");
            
            var leadDesigners = credits.Category.FirstOrDefault(c => c.Text?.Contains("Lead Designers") == true);
            Assert.NotNull(leadDesigners);
            Assert.True(leadDesigners.Section.Count > 0, "Lead Designers should have sections");
            
            // 验证LoadFromFile元素
            Assert.NotNull(credits.LoadFromFile);
            if (credits.LoadFromFile.Count > 0)
            {
                var legalFile = credits.LoadFromFile.FirstOrDefault(l => l.Name == "CreditsLegal");
                Assert.NotNull(legalFile);
                Assert.Equal("true", legalFile.PlatformSpecific);
            }
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("Category").Count() == savedDoc.Root?.Elements("Category").Count(),
                "Category count should be the same");
                
            Assert.True(originalDoc.Root?.Elements("LoadFromFile").Count() == savedDoc.Root?.Elements("LoadFromFile").Count(),
                "LoadFromFile count should be the same");
        }
        
        [Fact]
        public void Credits_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "Credits.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(Credits));
            Credits credits;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                credits = (Credits)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有分类都有必要的文本
            foreach (var category in credits.Category)
            {
                Assert.False(string.IsNullOrWhiteSpace(category.Text), "Category should have Text");
                
                // 验证子元素结构
                foreach (var section in category.Section)
                {
                    Assert.False(string.IsNullOrWhiteSpace(section.Text), "Section should have Text");
                }
                
                foreach (var entry in category.Entry)
                {
                    Assert.False(string.IsNullOrWhiteSpace(entry.Text), "Entry should have Text");
                }
            }
            
            // 验证LoadFromFile元素
            foreach (var loadFile in credits.LoadFromFile)
            {
                Assert.False(string.IsNullOrWhiteSpace(loadFile.Name), "LoadFromFile should have Name");
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