using BannerlordModEditor.Common.Models.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsExternalPartnersXBoxXmlTests
    {
        [Fact]
        public void CreditsExternalPartnersXBox_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsExternalPartnersXBox.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(CreditsExternalPartnersXBox));
            CreditsExternalPartnersXBox? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as CreditsExternalPartnersXBox;
            }

            Assert.NotNull(model);
            Assert.NotNull(model.Category);
            Assert.Equal("Xbox", model.Category.Text);
            Assert.NotNull(model.Category.Entry);
            Assert.Equal(4, model.Category.Entry.Length);
            Assert.Equal("Guy Richards", model.Category.Entry[0].Text);
            Assert.Equal("Neil Holmes", model.Category.Entry[1].Text);
            Assert.Equal("Ed Stewart", model.Category.Entry[2].Text);
            Assert.Equal("Derek Russell", model.Category.Entry[3].Text);
            Assert.NotNull(model.Category.EmptyLine);
            Assert.Single(model.Category.EmptyLine);

            // Serialization
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new UTF8Encoding(false), // No BOM
                OmitXmlDeclaration = false
            };

            string serializedXml;
            using (var memoryStream = new MemoryStream())
            using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(xmlWriter, model, ns);
                xmlWriter.Flush();
                serializedXml = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            // Comparison - normalize both XMLs to handle empty element differences
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            // Normalize both documents to handle <element></element> vs <element /> differences
            NormalizeEmptyElements(originalDoc.Root);
            NormalizeEmptyElements(serializedDoc.Root);

            Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "The XML was not logically identical after a round-trip.");
        }

        private static void NormalizeEmptyElements(XElement? element)
        {
            if (element == null) return;

            // If element is empty and has no text content, ensure it's truly empty
            if (!element.HasElements && string.IsNullOrWhiteSpace(element.Value))
            {
                element.Value = string.Empty;
            }

            // Recursively normalize child elements
            foreach (var child in element.Elements())
            {
                NormalizeEmptyElements(child);
            }
        }
    }
}