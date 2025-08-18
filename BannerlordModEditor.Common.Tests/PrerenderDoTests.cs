using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class PrerenderDoTests
    {
        [Fact]
        public void PrerenderDO_BasicProperties_ShouldWork()
        {
            // Arrange & Act
            var prerender = new PrerenderDO();

            // Assert
            Assert.NotNull(prerender.PostfxGraphs);
            Assert.NotNull(prerender.GraphsById);
            Assert.NotNull(prerender.NodesByGraphId);
            Assert.NotNull(prerender.NodesByInputSource);
        }

        [Fact]
        public void PrerenderDO_InitializeIndexes_ShouldCreateCorrectIndexes()
        {
            // Arrange
            var prerender = new PrerenderDO
            {
                Type = "particle_sysm",
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
                                    Class = "rglTestNode",
                                    Inputs = new List<InputDO>
                                    {
                                        new InputDO
                                        {
                                            Index = "0",
                                            Type = "provided",
                                            Source = "gbuffer_depth"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            prerender.InitializeIndexes();

            // Assert
            Assert.Single(prerender.GraphsById);
            Assert.True(prerender.GraphsById.ContainsKey("test_graph"));
            
            Assert.Single(prerender.NodesByGraphId);
            Assert.True(prerender.NodesByGraphId.ContainsKey("test_graph"));
            Assert.Single(prerender.NodesByGraphId["test_graph"]);
            
            Assert.Single(prerender.NodesByInputSource);
            Assert.True(prerender.NodesByInputSource.ContainsKey("gbuffer_depth"));
        }

        [Fact]
        public void PrerenderPostfxNodeDO_ComputeNodeDetection_ShouldWork()
        {
            // Arrange
            var computeNode = new PrerenderPostfxNodeDO
            {
                Compute = "true"
            };

            var regularNode = new PrerenderPostfxNodeDO
            {
                Compute = "false"
            };

            // Act & Assert
            Assert.True(computeNode.IsComputeNode());
            Assert.False(regularNode.IsComputeNode());
        }

        [Fact]
        public void PrerenderPostfxNodeDO_SizeTypeDetection_ShouldWork()
        {
            // Arrange
            var relativeNode = new PrerenderPostfxNodeDO
            {
                Size = "relative"
            };

            var fixedNode = new PrerenderPostfxNodeDO
            {
                Size = "fixed"
            };

            // Act & Assert
            Assert.True(relativeNode.IsRelativeSize());
            Assert.True(fixedNode.IsFixedSize());
        }

        [Fact]
        public void PrerenderPostfxNodeDO_GetEstimatedComplexity_ShouldCalculateCorrectly()
        {
            // Arrange
            var simpleNode = new PrerenderPostfxNodeDO();
            var computeNode = new PrerenderPostfxNodeDO { Compute = "true" };
            var nodeWithInputs = new PrerenderPostfxNodeDO
            {
                Inputs = new List<InputDO> { new InputDO(), new InputDO() }
            };
            var nodeWithPreconditions = new PrerenderPostfxNodeDO { HasPreconditions = true };

            // Act & Assert
            Assert.Equal(1, simpleNode.GetEstimatedComplexity());
            Assert.Equal(2, computeNode.GetEstimatedComplexity());
            Assert.Equal(3, nodeWithInputs.GetEstimatedComplexity());
            Assert.Equal(3, nodeWithPreconditions.GetEstimatedComplexity());
        }

        [Fact]
        public void InputDO_InputTypeDetection_ShouldWork()
        {
            // Arrange
            var providedInput = new InputDO { Type = "provided" };
            var nodeInput = new InputDO { Type = "node" };
            var textureInput = new InputDO { Type = "texture" };

            // Act & Assert
            Assert.True(providedInput.IsProvidedInput());
            Assert.True(nodeInput.IsNodeInput());
            Assert.True(textureInput.IsTextureInput());
        }

        [Fact]
        public void ConfigDO_ConfigTypeDetection_ShouldWork()
        {
            // Arrange
            var qualityConfig = new ConfigDO { Name = "high_quality" };
            var performanceConfig = new ConfigDO { Name = "performance_mode" };
            var enableConfig = new ConfigDO { Name = "enable_feature" };

            // Act & Assert
            Assert.True(qualityConfig.IsQualityConfig());
            Assert.True(performanceConfig.IsPerformanceConfig());
            Assert.True(enableConfig.IsEnabledConfig());
        }
    }
}