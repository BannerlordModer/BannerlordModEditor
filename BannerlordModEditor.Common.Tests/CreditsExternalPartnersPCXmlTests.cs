using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsExternalPartnersPCXmlTests
    {
        [Fact]
        public void CreditsExternalPartnersPC_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsExternalPartnersPC.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(CreditsExternalPartnersPC));
            CreditsExternalPartnersPC? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as CreditsExternalPartnersPC;
            }

            Assert.NotNull(model);

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