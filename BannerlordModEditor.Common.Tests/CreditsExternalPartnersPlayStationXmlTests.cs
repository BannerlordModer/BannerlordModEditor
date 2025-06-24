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
    public class CreditsExternalPartnersPlayStationXmlTests
    {
        [Fact]
        public void CreditsExternalPartnersPlayStation_LoadAndSave_ShouldMaintainDataIntegrity()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsExternalPartnersPlayStation.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(Credits));
            Credits? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as Credits;
            }

            Assert.NotNull(model);

            // Verify data integrity after deserialization
            Assert.NotNull(model.Category);
            Assert.Single(model.Category);

            var category = model.Category[0];
            Assert.Equal("{=!}Sony PlayStation Third Party Relations", category.Text);
            Assert.Equal(2, category.Section.Count);
            Assert.Equal(2, category.EmptyLine.Count);
            Assert.Empty(category.Entry);
            Assert.Empty(category.Image);

            // Verify section data
            var firstSection = category.Section[0];
            Assert.Equal("{=!}Senior Account Manager", firstSection.Text);
            Assert.Single(firstSection.Entry);
            Assert.Equal("Liz Mercuri", firstSection.Entry[0].Text);

            var secondSection = category.Section[1];
            Assert.Equal("{=!}Senior Manager, Global Account Director", secondSection.Text);
            Assert.Single(secondSection.Entry);
            Assert.Equal("John Vega", secondSection.Entry[0].Text);

            // Test serialization produces valid XML
            string serializedXml;
            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, model);
                }
                serializedXml = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            // Verify serialized XML can be parsed and contains expected data
            var serializedDoc = XDocument.Parse(serializedXml);
            Assert.NotNull(serializedDoc.Root);
            Assert.Equal("Credits", serializedDoc.Root.Name.LocalName);

            // Test round-trip deserialization
            Credits? roundTripModel;
            using (var stringReader = new StringReader(serializedXml))
            {
                roundTripModel = serializer.Deserialize(stringReader) as Credits;
            }

            Assert.NotNull(roundTripModel);
            Assert.Single(roundTripModel.Category);
            Assert.Equal(category.Text, roundTripModel.Category[0].Text);
            Assert.Equal(category.Section.Count, roundTripModel.Category[0].Section.Count);
            Assert.Equal(category.EmptyLine.Count, roundTripModel.Category[0].EmptyLine.Count);
        }

        [Fact]
        public void CreditsExternalPartnersPlayStation_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsExternalPartnersPlayStation.xml");
            var serializer = new XmlSerializer(typeof(Credits));

            Credits? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as Credits;
            }

            Assert.NotNull(model);
            Assert.NotNull(model.Category);
            Assert.Single(model.Category);

            var category = model.Category[0];
            Assert.Equal("{=!}Sony PlayStation Third Party Relations", category.Text);
            
            // Should have 2 sections and 2 empty lines
            Assert.Equal(2, category.Section.Count);
            Assert.Equal(2, category.EmptyLine.Count);

            // First section
            var firstSection = category.Section[0];
            Assert.Equal("{=!}Senior Account Manager", firstSection.Text);
            Assert.NotNull(firstSection.Entry);
            Assert.Single(firstSection.Entry);
            Assert.Equal("Liz Mercuri", firstSection.Entry[0].Text);

            // Second section
            var secondSection = category.Section[1];
            Assert.Equal("{=!}Senior Manager, Global Account Director", secondSection.Text);
            Assert.NotNull(secondSection.Entry);
            Assert.Single(secondSection.Entry);
            Assert.Equal("John Vega", secondSection.Entry[0].Text);

            // Verify empty collections
            Assert.Empty(category.Entry);
            Assert.Empty(category.Image);
        }
    }
} 