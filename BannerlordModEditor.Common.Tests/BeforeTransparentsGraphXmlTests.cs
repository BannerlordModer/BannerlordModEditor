using BannerlordModEditor.Common.Models.Engine;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphXmlTests
    {
        [Fact]
        public void BeforeTransparentsGraph_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "before_transparents_graph.xml");

            // Deserialization
            var serializer = new XmlSerializer(typeof(BeforeTransparentsGraph));
            BeforeTransparentsGraph? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as BeforeTransparentsGraph;
            }

            Assert.NotNull(model);
            Assert.Equal("particle_system", model.Type);
            Assert.NotNull(model.PostfxGraphs);
            Assert.NotNull(model.PostfxGraphs.PostfxGraphList);
            Assert.Single(model.PostfxGraphs.PostfxGraphList);

            var graph = model.PostfxGraphs.PostfxGraphList[0];
            Assert.Equal("before_transparents_graph", graph.Id);
            Assert.NotNull(graph.PostfxNodeList);
            Assert.Equal(3, graph.PostfxNodeList.Count);

            // 验证第一个节点
            var firstNode = graph.PostfxNodeList[0];
            Assert.Equal("SSSSS_x", firstNode.Id);
            Assert.Equal("rglSSS_fxnode", firstNode.Class);
            Assert.Equal("postfx_sssss_blurx", firstNode.Shader);
            Assert.Equal("R11G11B10F", firstNode.Format);
            Assert.Equal("relative", firstNode.Size);
            Assert.Equal("1.0", firstNode.Width);
            Assert.Equal("1.0", firstNode.Height);
            Assert.NotNull(firstNode.Inputs);
            Assert.Equal(2, firstNode.Inputs.Count);
            Assert.NotNull(firstNode.Preconditions);
            Assert.NotNull(firstNode.Preconditions.Config);
            Assert.Single(firstNode.Preconditions.Config);
            Assert.Equal("sssss", firstNode.Preconditions.Config[0].Name);

            // 验证第二个节点
            var secondNode = graph.PostfxNodeList[1];
            Assert.Equal("SSSSS_y", secondNode.Id);
            Assert.Equal("rglSSS_fxnode", secondNode.Class);
            Assert.Equal("postfx_sssss_blury", secondNode.Shader);

            // 验证第三个节点
            var thirdNode = graph.PostfxNodeList[2];
            Assert.Equal("SSSSS_specular_add", thirdNode.Id);
            Assert.Equal("rglSSS_specular_add_fxnode", thirdNode.Class);
            Assert.Equal("pbr_deferred", thirdNode.Shader);
            Assert.NotNull(thirdNode.Outputs);
            Assert.Single(thirdNode.Outputs);
            Assert.Equal("0", thirdNode.Outputs[0].Index);
            Assert.Equal("provided", thirdNode.Outputs[0].Type);
            Assert.Equal("screen_rt", thirdNode.Outputs[0].Name);

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