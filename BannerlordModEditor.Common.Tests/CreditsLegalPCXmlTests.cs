using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsLegalPCXmlTests
    {
        [Fact]
        public void CreditsLegalPC_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsLegalPC.xml");

            // Deserialization using the existing model
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
                ns.Add("", "");
                serializer.Serialize(xmlWriter, model, ns);
                serializedXml = stringWriter.ToString();
            }

            // Comparison
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);

            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "The XML was not logically identical after a round-trip.");
        }
    }
} 