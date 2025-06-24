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
    public class CreditsLegalPCXmlTests
    {
        [Fact]
        public void CreditsLegalPC_LoadAndSave_ShouldMaintainDataIntegrity()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsLegalPC.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(Credits));
            Credits? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as Credits;
            }

            Assert.NotNull(model);

            // Serialization
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

            // Validate data integrity - deserialize the serialized XML
            Credits? roundTripModel;
            using (var stringReader = new StringReader(serializedXml))
            {
                roundTripModel = serializer.Deserialize(stringReader) as Credits;
            }

            Assert.NotNull(roundTripModel);

            // Verify core data structure
            Assert.Single(model.Category);
            Assert.Single(roundTripModel.Category);
            
            var originalCategory = model.Category[0];
            var roundTripCategory = roundTripModel.Category[0];
            
            Assert.Equal(originalCategory.Text, roundTripCategory.Text);
            Assert.Equal("{=!}Legal Notices", originalCategory.Text);
            
            // Verify the sections count (MIT License, Simplified BSD License, Modified BSD License, Apache License)
            Assert.True(originalCategory.Section.Count >= 4, "Should have at least 4 license sections");
            Assert.Equal(originalCategory.Section.Count, roundTripCategory.Section.Count);
            
            // Verify entries count 
            Assert.True(originalCategory.Entry.Count > 10, "Should have multiple legal entries");
            Assert.Equal(originalCategory.Entry.Count, roundTripCategory.Entry.Count);
            
            // Verify empty lines count
            Assert.True(originalCategory.EmptyLine.Count > 5, "Should have multiple empty lines for formatting");
            Assert.Equal(originalCategory.EmptyLine.Count, roundTripCategory.EmptyLine.Count);
            
            // Verify images count
            Assert.True(originalCategory.Image.Count >= 5, "Should have multiple company logos");
            Assert.Equal(originalCategory.Image.Count, roundTripCategory.Image.Count);
        }

        [Fact]
        public void CreditsLegalPC_ValidateStructure_ShouldHaveCorrectLegalSections()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsLegalPC.xml");

            var serializer = new XmlSerializer(typeof(Credits));
            Credits? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as Credits;
            }

            Assert.NotNull(model);
            Assert.Single(model.Category);
            
            var legalCategory = model.Category[0];
            Assert.Equal("{=!}Legal Notices", legalCategory.Text);
            
            // Verify key license sections exist
            var sectionTexts = legalCategory.Section.Select(s => s.Text).ToList();
            Assert.Contains("{=!}MIT License", sectionTexts);
            Assert.Contains("{=!}Simplified BSD License", sectionTexts);
            Assert.Contains("{=!}Modified BSD License", sectionTexts);
            Assert.Contains("{=!}Apache License", sectionTexts);
            
            // Verify key company/technology images
            var imageTexts = legalCategory.Image.Select(i => i.Text).ToList();
            Assert.Contains("intel", imageTexts);
            Assert.Contains("nvidia", imageTexts);
            Assert.Contains("speedtree", imageTexts);
        }
    }
} 