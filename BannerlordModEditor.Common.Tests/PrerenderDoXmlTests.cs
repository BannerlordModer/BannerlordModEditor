using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class PrerenderDoXmlTests
    {
        [Fact]
        public void PrerenderDO_XmlSerialization_ShouldPreserveStructure()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            var xml = File.ReadAllText(xmlPath);

            // Act
            var obj = XmlTestUtils.Deserialize<PrerenderDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(obj);

            // Assert
            Assert.NotNull(obj);
            Assert.Equal("particle_sysm", obj.Type);
            Assert.True(obj.HasPostfxGraphs);
            Assert.NotNull(obj.PostfxGraphs);
            Assert.NotNull(obj.PostfxGraphs.PostfxGraphList);
            Assert.True(obj.PostfxGraphs.PostfxGraphList.Count > 0);

            // Basic structural equality check
            Assert.Contains("base", serializedXml);
            Assert.Contains("postfx_graphs", serializedXml);
        }

        [Fact]
        public void PrerenderDO_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            var xmlContent = File.ReadAllText(xmlPath);

            // Act
            var result = XmlTestUtils.Deserialize<PrerenderDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_sysm", result.Type);
            Assert.True(result.HasPostfxGraphs);
            Assert.NotNull(result.PostfxGraphs);
            Assert.NotNull(result.PostfxGraphs.PostfxGraphList);
            Assert.Equal(4, result.PostfxGraphs.PostfxGraphList.Count); // XML文件中有4个postfx_graph节点

            var graph = result.PostfxGraphs.PostfxGraphList[0];
            Assert.Equal("ambient_occlusion_graph", graph.Id);
            Assert.NotNull(graph.PostfxNodes);
            Assert.True(graph.PostfxNodes.Count > 0);

            var firstNode = graph.PostfxNodes[0];
            Assert.Equal("depth_chain", firstNode.Id);
            Assert.Equal("rglDepth_chain_node", firstNode.Class);
            Assert.Equal("postfx_depth_downsample_cs", firstNode.Shader);
            Assert.Equal("R32F", firstNode.Format);
            Assert.Equal("relative", firstNode.Size);
            Assert.Equal("0.5", firstNode.Width);
            Assert.Equal("0.5", firstNode.Height);
            Assert.True(firstNode.IsComputeNode());
            Assert.True(firstNode.HasEmptyInputs || firstNode.Inputs.Count > 0);
        }

        [Fact]
        public void PrerenderDO_WithBusinessLogic_ShouldWorkCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_sysm"">
    <postfx_graphs>
        <postfx_graph id=""test_graph"">
            <postfx_node id=""compute_node"" class=""rglComputeNode"" compute=""true"">
                <input index=""0"" type=""provided"" source=""gbuffer_depth"" />
            </postfx_node>
            <postfx_node id=""regular_node"" class=""rglRegularNode"">
                <input index=""0"" type=""node"" source=""compute_node"" />
            </postfx_node>
        </postfx_graph>
    </postfx_graphs>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<PrerenderDO>(xmlContent);
            result.InitializeIndexes();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsValid());

            // Test graph lookup
            var graph = result.GetGraphById("test_graph");
            Assert.NotNull(graph);
            Assert.Equal("test_graph", graph.Id);

            // Test node lookup
            var nodes = result.GetNodesByGraphId("test_graph");
            Assert.Equal(2, nodes.Count);

            // Test input source lookup
            var depthNodes = result.GetNodesByInputSource("gbuffer_depth");
            Assert.Single(depthNodes);
            Assert.Equal("compute_node", depthNodes[0].Id);

            // Test compute graph detection
            var computeGraphs = result.GetComputeGraphs();
            Assert.Single(computeGraphs);
            Assert.Equal("test_graph", computeGraphs[0].Id);

            // Test compute node detection
            var computeNodes = graph.GetComputeNodes();
            Assert.Single(computeNodes);
            Assert.Equal("compute_node", computeNodes[0].Id);

            // Test performance analysis
            Assert.Equal(2, result.GetTotalNodeCount());
            Assert.Equal(2.0, result.GetAverageNodesPerGraph()); // 1个graph有2个nodes，平均值应该是2.0
        }

        [Fact]
        public void PrerenderDO_RoundTrip_ShouldPreserveAllData()
        {
            // Arrange
            var original = new PrerenderDO
            {
                Type = "test_type",
                HasPostfxGraphs = true,
                PostfxGraphs = new PrerenderPostfxGraphsDO
                {
                    PostfxGraphList = new List<PrerenderPostfxGraphDO>
                    {
                        new PrerenderPostfxGraphDO
                        {
                            Id = "test_graph",
                            PostfxNodes = new List<PrerenderPostfxNodeDO>
                            {
                                new PrerenderPostfxNodeDO
                                {
                                    Id = "test_node",
                                    Class = "TestClass",
                                    Shader = "TestShader",
                                    Format = "R8G8B8A8",
                                    Size = "relative",
                                    Width = "1.0",
                                    Height = "1.0",
                                    Compute = "true",
                                    ComputeTgSizeX = "8",
                                    ComputeTgSizeY = "8",
                                    Inputs = new List<InputDO>
                                    {
                                        new InputDO
                                        {
                                            Index = "0",
                                            Type = "provided",
                                            Source = "test_source"
                                        }
                                    },
                                    HasPreconditions = true,
                                    Preconditions = new PreconditionsDO
                                    {
                                        Configs = new List<ConfigDO>
                                        {
                                            new ConfigDO { Name = "test_config" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var xml = XmlTestUtils.Serialize(original);
            var deserialized = XmlTestUtils.Deserialize<PrerenderDO>(xml);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(original.Type, deserialized.Type);
            Assert.True(deserialized.HasPostfxGraphs);
            Assert.Single(deserialized.PostfxGraphs.PostfxGraphList);

            var graph = deserialized.PostfxGraphs.PostfxGraphList[0];
            Assert.Equal("test_graph", graph.Id);
            Assert.Single(graph.PostfxNodes);

            var node = graph.PostfxNodes[0];
            Assert.Equal("test_node", node.Id);
            Assert.Equal("TestClass", node.Class);
            Assert.Equal("TestShader", node.Shader);
            Assert.True(node.IsComputeNode());
            Assert.Single(node.Inputs);
            Assert.True(node.HasPreconditions);
            Assert.Single(node.Preconditions.Configs);
        }
    }
}