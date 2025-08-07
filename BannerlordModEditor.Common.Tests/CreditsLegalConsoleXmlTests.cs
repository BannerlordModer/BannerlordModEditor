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
    public class CreditsLegalConsoleXmlTests
    {
        [Fact]
        public void CreditsLegalConsole_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsLegalConsole.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(CreditsLegalConsole));
            CreditsLegalConsole? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as CreditsLegalConsole;
            }

            Assert.NotNull(model);

            // Serialization
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new UTF8Encoding(false),
                OmitXmlDeclaration = false
            };
            
            string serializedXml;
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Remove default namespaces
                serializer.Serialize(xmlWriter, model, ns);
                serializedXml = stringWriter.ToString();
            }

            // Comparison
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);

            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "The XML was not logically identical after a round-trip.");
        }
        
        [Fact]
        public void CreditsLegalConsole_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "CreditsLegalConsole.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(CreditsLegalConsole));
            CreditsLegalConsole credits;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                credits = (CreditsLegalConsole)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证Category存在
            Assert.NotNull(credits.Category);
            Assert.False(string.IsNullOrWhiteSpace(credits.Category.Text), "Category应该有Text属性");
            
            // 验证所有Section都有Text属性
            foreach (var section in credits.Category.Items.OfType<CreditsSectionLegal>())
            {
                Assert.False(string.IsNullOrWhiteSpace(section.Text), "Section应该有Text属性");
            }
            
            // 验证所有Entry都有Text属性
            foreach (var entry in credits.Category.Items.OfType<CreditsEntryLegal>())
            {
                Assert.False(string.IsNullOrWhiteSpace(entry.Text), "Entry应该有Text属性");
            }
            
            // 验证所有Image都有Text属性
            foreach (var image in credits.Category.Items.OfType<CreditsImageLegal>())
            {
                Assert.False(string.IsNullOrWhiteSpace(image.Text), "Image应该有Text属性");
            }
            
            // 验证必需的图像存在
            var requiredImages = new[] { "nvidia", "nvidia_physx", "simplygon", "speedtree", "telemetry" };
            foreach (var requiredImage in requiredImages)
            {
                var image = credits.Category.Items.OfType<CreditsImageLegal>().FirstOrDefault(i => i.Text == requiredImage);
                Assert.NotNull(image);
            }
            
            // 验证必需的许可证Section存在
            var requiredSections = new[] { 
                "{=!}MIT License", 
                "{=!}Simplified BSD License", 
                "{=!}Modified BSD License", 
                "{=!}Apache License" 
            };
            
            foreach (var requiredSection in requiredSections)
            {
                var section = credits.Category.Items.OfType<CreditsSectionLegal>().FirstOrDefault(s => s.Text == requiredSection);
                Assert.NotNull(section);
            }
        }

        [Fact]
        public void CreditsLegalConsole_SerializeAndDeserialize_ShouldMaintainImageElements()
        {
            // Arrange
            var originalCredits = new CreditsLegalConsole();
            var category = new CreditsCategoryLegal
            {
                Text = "{=!}Test Category"
            };
            
            // 添加Image元素
            category.Items.Add(new CreditsImageLegal { Text = "test_image_1" });
            category.Items.Add(new CreditsImageLegal { Text = "test_image_2" });
            
            // 添加其他元素
            category.Items.Add(new CreditsEntryLegal { Text = "Test Entry" });
            category.Items.Add(new CreditsEmptyLineLegal());
            
            originalCredits.Category = category;
            
            // Act - 序列化
            string xml;
            var serializer = new XmlSerializer(typeof(CreditsLegalConsole));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, originalCredits);
                xml = writer.ToString();
            }
            
            // Act - 反序列化
            CreditsLegalConsole deserializedCredits;
            using (var reader = new StringReader(xml))
            {
                deserializedCredits = (CreditsLegalConsole)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(deserializedCredits);
            Assert.NotNull(deserializedCredits.Category);
            
            var testCategory = deserializedCredits.Category;
            Assert.Equal("{=!}Test Category", testCategory.Text);
            
            var images = testCategory.Items.OfType<CreditsImageLegal>().ToList();
            Assert.Equal(2, images.Count);
            Assert.Equal("test_image_1", images[0].Text);
            Assert.Equal("test_image_2", images[1].Text);
            
            var entries = testCategory.Items.OfType<CreditsEntryLegal>().ToList();
            Assert.Single(entries);
            Assert.Equal("Test Entry", entries[0].Text);
            
            var emptyLines = testCategory.Items.OfType<CreditsEmptyLineLegal>().ToList();
            Assert.Single(emptyLines);
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