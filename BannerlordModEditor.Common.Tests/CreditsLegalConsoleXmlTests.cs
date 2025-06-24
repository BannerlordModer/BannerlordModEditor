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
            var serializer = new XmlSerializer(typeof(Credits));
            Credits credits;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                credits = (Credits)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有Category都有Text属性
            foreach (var category in credits.Category)
            {
                Assert.False(string.IsNullOrWhiteSpace(category.Text), "Category应该有Text属性");
            }
            
            // 验证所有Section都有Text属性
            foreach (var category in credits.Category)
            {
                foreach (var section in category.Section)
                {
                    Assert.False(string.IsNullOrWhiteSpace(section.Text), "Section应该有Text属性");
                }
            }
            
            // 验证所有Entry都有Text属性
            foreach (var category in credits.Category)
            {
                foreach (var entry in category.Entry)
                {
                    Assert.False(string.IsNullOrWhiteSpace(entry.Text), "Entry应该有Text属性");
                }
                
                foreach (var section in category.Section)
                {
                    foreach (var entry in section.Entry)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(entry.Text), "Section Entry应该有Text属性");
                    }
                }
            }
            
            // 验证所有Image都有Text属性
            foreach (var category in credits.Category)
            {
                foreach (var image in category.Image)
                {
                    Assert.False(string.IsNullOrWhiteSpace(image.Text), "Image应该有Text属性");
                }
            }
            
            // 验证必需的图像存在
            var legalCategory = credits.Category.FirstOrDefault(c => c.Text == "{=!}Legal Notices");
            Assert.NotNull(legalCategory);
            
            var requiredImages = new[] { "nvidia", "nvidia_physx", "simplygon", "speedtree", "telemetry" };
            foreach (var requiredImage in requiredImages)
            {
                var image = legalCategory.Image.FirstOrDefault(i => i.Text == requiredImage);
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
                var section = legalCategory.Section.FirstOrDefault(s => s.Text == requiredSection);
                Assert.NotNull(section);
            }
        }

        [Fact]
        public void CreditsLegalConsole_SerializeAndDeserialize_ShouldMaintainImageElements()
        {
            // Arrange
            var originalCredits = new Credits();
            var category = new CreditsCategory
            {
                Text = "{=!}Test Category"
            };
            
            // 添加Image元素
            category.Image.Add(new CreditsImage { Text = "test_image_1" });
            category.Image.Add(new CreditsImage { Text = "test_image_2" });
            
            // 添加其他元素
            category.Entry.Add(new CreditsEntry { Text = "Test Entry" });
            category.EmptyLine.Add(new EmptyLine());
            
            originalCredits.Category.Add(category);
            
            // Act - 序列化
            string xml;
            var serializer = new XmlSerializer(typeof(Credits));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, originalCredits);
                xml = writer.ToString();
            }
            
            // Act - 反序列化
            Credits deserializedCredits;
            using (var reader = new StringReader(xml))
            {
                deserializedCredits = (Credits)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(deserializedCredits);
            Assert.Single(deserializedCredits.Category);
            
            var testCategory = deserializedCredits.Category[0];
            Assert.Equal("{=!}Test Category", testCategory.Text);
            Assert.Equal(2, testCategory.Image.Count);
            Assert.Equal("test_image_1", testCategory.Image[0].Text);
            Assert.Equal("test_image_2", testCategory.Image[1].Text);
            Assert.Single(testCategory.Entry);
            Assert.Single(testCategory.EmptyLine);
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