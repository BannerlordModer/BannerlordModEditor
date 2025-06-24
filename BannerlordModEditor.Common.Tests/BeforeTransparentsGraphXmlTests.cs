using BannerlordModEditor.Common.Models.Engine;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphXmlTests
    {
        private readonly ITestOutputHelper _output;

        public BeforeTransparentsGraphXmlTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BeforeTransparentsGraph_CanBeDeserialized()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "before_transparents_graph.xml");
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));

            PostfxGraphsBase? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as PostfxGraphsBase;
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

            var firstNode = graph.PostfxNodeList[0];
            Assert.Equal("SSSSS_x", firstNode.Id);
            Assert.Equal("rglSSS_fxnode", firstNode.Class);
            Assert.Equal("postfx_sssss_blurx", firstNode.Shader);
            Assert.NotNull(firstNode.Inputs);
            Assert.Equal(2, firstNode.Inputs.Count);
            Assert.Empty(firstNode.Outputs);
            Assert.NotNull(firstNode.Preconditions);
            Assert.NotNull(firstNode.Preconditions.Config);
            Assert.Equal("sssss", firstNode.Preconditions.Config.Name);
        }
    }
} 