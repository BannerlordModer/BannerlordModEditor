using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsXmlTests
    {
        [Theory]
        [MemberData(nameof(GetFloraKindsTestFiles))]
        public void FloraKinds_LoadAndSave_ShouldBeLogicallyIdentical(string fileName)
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "FloraKinds", fileName);

            // Deserialization
            var serializer = new XmlSerializer(typeof(FloraKinds));
            FloraKinds? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as FloraKinds;
            }

            Assert.NotNull(model);
            Assert.NotNull(model.FloraKind);
            Assert.True(model.FloraKind.Count > 0);

            // Verify some sample data from the first flora kind
            var firstFloraKind = model.FloraKind[0];
            Assert.NotNull(firstFloraKind.Name);
            Assert.NotNull(firstFloraKind.ViewDistance);

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

            // Comparison
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "The XML was not logically identical after a round-trip.");
        }

        public static IEnumerable<object[]> GetFloraKindsTestFiles()
        {
            var testSubsetsDir = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "FloraKinds");
            if (Directory.Exists(testSubsetsDir))
            {
                var files = Directory.GetFiles(testSubsetsDir, "flora_kinds_part_*.xml");
                foreach (var file in files)
                {
                    yield return new object[] { Path.GetFileName(file) };
                }
            }
        }
    }
}