using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Engine;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class PostfxGraphsXmlTests
    {
        [Fact]
        public void PostfxGraphs_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_system"">
    <postfx_graphs>
        <postfx_graph id=""default"">
            <postfx_node
                id=""external_screen_injection""
                shader=""postfx_screen_injection""
                format=""R11G11B10F""
                size=""relative""
                width=""1.0""
                height=""1.0"">
                <output index=""0"" type=""provided"" name=""screen_rt"" />
                <input index=""0"" type=""provided"" source=""external_screen_input"" />
            </postfx_node>
            <postfx_node
                id=""histogram""
                class=""rglHistogram_node""
                shader=""postfx_eye_adaptation""
                format=""R16F""
                size=""absolute""
                width=""1""
                height=""1"">
                <input index=""1"" type=""provided"" source=""screen_rt"" />
                <input index=""14"" type=""node"" source=""SSSSS_specular_add"" />
                <preconditions>
                    <config name=""dlss"" />
                </preconditions>
            </postfx_node>
        </postfx_graph>
    </postfx_graphs>
</base>";

            var result = XmlTestUtils.Deserialize<PostfxGraphsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.True(result.HasPostfxGraphs);
            Assert.NotNull(result.PostfxGraphs);
            Assert.Single(result.PostfxGraphs.Graphs);
            
            var graph = result.PostfxGraphs.Graphs[0];
            Assert.Equal("default", graph.Id);
            Assert.Equal(2, graph.Nodes.Count);
            
            var firstNode = graph.Nodes[0];
            Assert.Equal("external_screen_injection", firstNode.Id);
            Assert.Equal("postfx_screen_injection", firstNode.Shader);
            Assert.Equal("R11G11B10F", firstNode.Format);
            Assert.Equal("relative", firstNode.Size);
            Assert.Equal("1.0", firstNode.Width);
            Assert.Equal("1.0", firstNode.Height);
            Assert.Single(firstNode.Outputs);
            Assert.Single(firstNode.Inputs);
            Assert.False(firstNode.HasPreconditions);
            
            var output = firstNode.Outputs[0];
            Assert.Equal("0", output.Index);
            Assert.Equal("provided", output.Type);
            Assert.Equal("screen_rt", output.Name);
            
            var input = firstNode.Inputs[0];
            Assert.Equal("0", input.Index);
            Assert.Equal("provided", input.Type);
            Assert.Equal("external_screen_input", input.Source);
            
            var secondNode = graph.Nodes[1];
            Assert.Equal("histogram", secondNode.Id);
            Assert.Equal("rglHistogram_node", secondNode.Class);
            Assert.Equal("postfx_eye_adaptation", secondNode.Shader);
            Assert.Equal("R16F", secondNode.Format);
            Assert.Equal("absolute", secondNode.Size);
            Assert.Equal("1", secondNode.Width);
            Assert.Equal("1", secondNode.Height);
            Assert.Empty(secondNode.Outputs);
            Assert.Equal(2, secondNode.Inputs.Count);
            Assert.True(secondNode.HasPreconditions);
            
            var precondition = secondNode.Preconditions.Configs[0];
            Assert.Equal("dlss", precondition.Name);
        }

        [Fact]
        public void PostfxGraphs_CanSerializeToXml()
        {
            // Arrange
            var model = new PostfxGraphsDO
            {
                Type = "particle_system",
                HasPostfxGraphs = true,
                PostfxGraphs = new PostfxGraphsContainerDO
                {
                    Graphs = new List<PostfxGraphDO>
                    {
                        new PostfxGraphDO
                        {
                            Id = "default",
                            Nodes = new List<PostfxNodeDO>
                            {
                                new PostfxNodeDO
                                {
                                    Id = "external_screen_injection",
                                    Shader = "postfx_screen_injection",
                                    Format = "R11G11B10F",
                                    Size = "relative",
                                    Width = "1.0",
                                    Height = "1.0",
                                    Outputs = new List<PostfxOutputDO>
                                    {
                                        new PostfxOutputDO
                                        {
                                            Index = "0",
                                            Type = "provided",
                                            Name = "screen_rt"
                                        }
                                    },
                                    Inputs = new List<PostfxInputDO>
                                    {
                                        new PostfxInputDO
                                        {
                                            Index = "0",
                                            Type = "provided",
                                            Source = "external_screen_input"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var xml = XmlTestUtils.Serialize(model);

            // Assert
            Assert.Contains("particle_system", xml);
            Assert.Contains("postfx_graphs", xml);
            Assert.Contains("external_screen_injection", xml);
            Assert.Contains("postfx_screen_injection", xml);
            Assert.Contains("screen_rt", xml);
        }

        [Fact]
        public void PostfxGraphs_EmptyXmlHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<base type=""particle_system""></base>";

            var result = XmlTestUtils.Deserialize<PostfxGraphsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.False(result.HasPostfxGraphs);
            Assert.NotNull(result.PostfxGraphs);
            Assert.Empty(result.PostfxGraphs.Graphs);
        }

        [Fact]
        public void PostfxGraphs_ComplexStructureHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_system"">
    <postfx_graphs>
        <postfx_graph id=""complex"">
            <postfx_node id=""node1"" shader=""shader1"">
                <output index=""0"" type=""provided"" name=""output1"" />
                <output index=""1"" type=""provided"" name=""output2"" />
                <input index=""0"" type=""provided"" source=""input1"" />
                <input index=""1"" type=""node"" source=""node2"" />
                <input index=""2"" type=""provided"" source=""input3"" />
            </postfx_node>
            <postfx_node id=""node2"" class=""TestClass"" shader=""shader2"">
                <preconditions>
                    <config name=""dlss"" />
                    <config name=""taa"" />
                </preconditions>
            </postfx_node>
            <postfx_node id=""node3"" format=""R16F"" size=""absolute"" width=""512"" height=""512"" />
        </postfx_graph>
    </postfx_graphs>
</base>";

            var result = XmlTestUtils.Deserialize<PostfxGraphsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasPostfxGraphs);
            Assert.Single(result.PostfxGraphs.Graphs);
            
            var graph = result.PostfxGraphs.Graphs[0];
            Assert.Equal("complex", graph.Id);
            Assert.Equal(3, graph.Nodes.Count);
            
            var node1 = graph.Nodes[0];
            Assert.Equal("node1", node1.Id);
            Assert.Equal("shader1", node1.Shader);
            Assert.Equal(2, node1.Outputs.Count);
            Assert.Equal(3, node1.Inputs.Count);
            Assert.False(node1.HasPreconditions);
            
            var node2 = graph.Nodes[1];
            Assert.Equal("node2", node2.Id);
            Assert.Equal("TestClass", node2.Class);
            Assert.Equal("shader2", node2.Shader);
            Assert.True(node2.HasPreconditions);
            Assert.Equal(2, node2.Preconditions.Configs.Count);
            
            var node3 = graph.Nodes[2];
            Assert.Equal("node3", node3.Id);
            Assert.Equal("R16F", node3.Format);
            Assert.Equal("absolute", node3.Size);
            Assert.Equal("512", node3.Width);
            Assert.Equal("512", node3.Height);
            Assert.Empty(node3.Outputs);
            Assert.Empty(node3.Inputs);
            Assert.False(node3.HasPreconditions);
        }
    }
}