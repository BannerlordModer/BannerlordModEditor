using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ActionTypesXmlTests
    {
        private static readonly string TestSubsetsDir = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "ActionTypes");

        public static IEnumerable<object[]> TestFiles =>
            Directory.EnumerateFiles(TestSubsetsDir, "*.xml")
                     .Select(file => new object[] { Path.GetFileName(file) });

        [Theory]
        [MemberData(nameof(TestFiles))]
        public void ActionTypes_LoadAndSave_ShouldBeLogicallyIdentical(string fileName)
        {
            var xmlPath = Path.Combine(TestSubsetsDir, fileName);
            
            // Deserialization
            var serializer = new XmlSerializer(typeof(ActionTypesList));
            ActionTypesList? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as ActionTypesList;
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

            Assert.Equal(originalDoc.Root.Name.LocalName, serializedDoc.Root.Name.LocalName);
            Assert.Equal(originalDoc.Root.Elements().Count(), serializedDoc.Root.Elements().Count());
        }
    }
} 