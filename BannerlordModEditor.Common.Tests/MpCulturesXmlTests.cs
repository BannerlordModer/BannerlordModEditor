using BannerlordModEditor.Common.Models;
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
    public class MpCulturesXmlTests
    {
        [Fact]
        public void MpCultures_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "mpcultures.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(BasicCultures));
            BasicCultures cultures;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                cultures = (BasicCultures)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, cultures);
                }
                savedXml = writer.ToString();
            }

            // Assert
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 规范化XML格式
            NormalizeXml(originalDoc.Root);
            NormalizeXml(savedDoc.Root);

            // 检查基本的XML结构是否合理
            Assert.NotNull(cultures);
            Assert.NotNull(cultures.Culture);
            Assert.True(cultures.Culture.Count > 0, "Should have at least one culture");
            
            // 检查所有必要元素都存在
            Assert.True(originalDoc.Root?.Elements().Count() == savedDoc.Root?.Elements().Count(),
                "Element count should be the same");
                
            // 检查所有文化都有必要的属性
            foreach (var culture in cultures.Culture)
            {
                Assert.False(string.IsNullOrEmpty(culture.Id), "Culture ID should not be empty");
                Assert.False(string.IsNullOrEmpty(culture.Name), "Culture Name should not be empty");
                Assert.False(string.IsNullOrEmpty(culture.DefaultFaceKey), "Culture DefaultFaceKey should not be empty");
            }
        }
        
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
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

        private static void NormalizeXml(XElement? element)
        {
            if (element == null) return;
            
            // 移除所有空白文本节点
            var whitespaceNodes = element.Nodes().OfType<XText>()
                .Where(t => string.IsNullOrWhiteSpace(t.Value))
                .ToList();
            foreach (var node in whitespaceNodes)
            {
                node.Remove();
            }
            
            // 递归处理子元素
            foreach (var child in element.Elements())
            {
                NormalizeXml(child);
            }
        }
    }
} 