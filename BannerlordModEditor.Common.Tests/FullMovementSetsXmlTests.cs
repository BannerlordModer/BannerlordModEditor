using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FullMovementSetsXmlTests
    {
        [Fact]
        public void FullMovementSets_CanBeDeserialized()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "full_movement_sets.xml");
            var serializer = new XmlSerializer(typeof(FullMovementSets));

            FullMovementSets? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as FullMovementSets;
            }

            Assert.NotNull(model);
            Assert.NotNull(model.FullMovementSet);
            Assert.True(model.FullMovementSet.Count > 0);

            var firstSet = model.FullMovementSet[0];
            Assert.Equal("no_weapon", firstSet.Id);
            Assert.NotNull(firstSet.MovementSet);
            Assert.True(firstSet.MovementSet.Count > 0);

            var firstMovement = firstSet.MovementSet[0];
            Assert.Equal("walk_unarmed", firstMovement.Id);
            Assert.Equal("False", firstMovement.LeftStance);
            Assert.Equal("walking", firstMovement.MovementMode);

            // Test a set with base_set attribute
            var inheritedSet = model.FullMovementSet.FirstOrDefault(s => !string.IsNullOrEmpty(s.BaseSet));
            Assert.NotNull(inheritedSet);
            Assert.NotNull(inheritedSet.BaseSet);
        }

        [Fact]
        public void FullMovementSets_RoundTrip_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "full_movement_sets.xml");

            var serializer = new XmlSerializer(typeof(FullMovementSets));

            FullMovementSets model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = (FullMovementSets)serializer.Deserialize(fileStream)!;
            }

            string serializedXml;
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new UTF8Encoding(false)
            }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(xmlWriter, model, ns);
                serializedXml = stringWriter.ToString();
            }

            var originalDoc = XDocument.Load(xmlPath);
            var serializedDoc = XDocument.Parse(serializedXml);

            Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "XML documents should be logically identical");
        }
    }
} 