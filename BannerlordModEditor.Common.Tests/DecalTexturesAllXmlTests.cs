using BannerlordModEditor.Common.Models.Engine;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class DecalTexturesAllXmlTests
    {
        [Fact]
        public void DecalTexturesAll_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "decal_textures_all.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(DecalTexturesRoot));
            DecalTexturesRoot? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as DecalTexturesRoot;
            }

            Assert.NotNull(model);
            Assert.Equal("decal_texture", model.Type);
            Assert.NotNull(model.DecalTextures);
            Assert.NotNull(model.DecalTextures.DecalTexture);
            Assert.True(model.DecalTextures.DecalTexture.Length > 0);

            // Verify some sample data
            var firstTexture = model.DecalTextures.DecalTexture[0];
            Assert.Equal("black", firstTexture.Name);
            Assert.Equal("0.990295, 0.624023, 0.000000, 0.001953", firstTexture.AtlasPos);
            Assert.Equal("false", firstTexture.IsDynamic);

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
    }
}