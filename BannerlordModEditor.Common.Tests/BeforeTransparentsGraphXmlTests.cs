using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphXmlTests
    {
        [Fact]
        public void BeforeTransparentsGraph_RoundTrip_StructuralEquality()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "before_transparents_graph.xml");
            var xml = File.ReadAllText(xmlPath);

            var obj = XmlTestUtils.Deserialize<BeforeTransparentsGraphDO>(xml);
            var xml2 = XmlTestUtils.Serialize(obj);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, xml2));
        }
        
        [Fact]
        public void BeforeTransparentsGraph_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "before_transparents_graph.xml");
            var xmlContent = File.ReadAllText(xmlPath);

            // Act
            var result = XmlTestUtils.Deserialize<BeforeTransparentsGraphDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.True(result.HasPostfxGraphs);
            Assert.NotNull(result.PostfxGraphs);
            Assert.NotNull(result.PostfxGraphs.PostfxGraphList);
            Assert.Single(result.PostfxGraphs.PostfxGraphList);

            var graph = result.PostfxGraphs.PostfxGraphList[0];
            Assert.Equal("before_transparents_graph", graph.Id);
            Assert.True(result.PostfxGraphs.HasEmptyPostfxGraphs || graph.PostfxNodes.Count > 0);
            Assert.NotNull(graph.PostfxNodes);
            Assert.True(graph.PostfxNodes.Count > 0);

            var firstNode = graph.PostfxNodes[0];
            Assert.Equal("SSSSS_x", firstNode.Id);
            Assert.Equal("rglSSS_fxnode", firstNode.Class);
            Assert.Equal("postfx_sssss_blurx", firstNode.Shader);
            Assert.Equal("R11G11B10F", firstNode.Format);
            Assert.Equal("relative", firstNode.Size);
            Assert.Equal("1.0", firstNode.Width);
            Assert.Equal("1.0", firstNode.Height);
            Assert.True(firstNode.HasEmptyInputs || firstNode.Inputs.Count > 0);
            Assert.True(firstNode.HasPreconditions);
        }
    }
}