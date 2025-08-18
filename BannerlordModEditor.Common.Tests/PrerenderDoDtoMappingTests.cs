using System;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using BannerlordModEditor.Common.Mappers;

namespace BannerlordModEditor.Common.Tests
{
    public class PrerenderDoDtoMappingTests
    {
        [Fact]
        public void PrerenderDO_Mapping_DoToDtoAndBack_ShouldPreserveData()
        {
            // Arrange
            var original = new PrerenderDO
            {
                Type = "particle_sysm",
                HasPostfxGraphs = true,
                PostfxGraphs = new PrerenderPostfxGraphsDO
                {
                    PostfxGraphList = new List<PrerenderPostfxGraphDO>
                    {
                        new PrerenderPostfxGraphDO
                        {
                            Id = "ambient_occlusion_graph",
                            PostfxNodes = new List<PrerenderPostfxNodeDO>
                            {
                                new PrerenderPostfxNodeDO
                                {
                                    Id = "depth_chain",
                                    Class = "rglDepth_chain_node",
                                    Shader = "postfx_depth_downsample_cs",
                                    Format = "R32F",
                                    Size = "relative",
                                    Width = "0.5",
                                    Height = "0.5",
                                    Compute = "true",
                                    ComputeTgSizeX = "8",
                                    ComputeTgSizeY = "8",
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
            var dto = PrerenderMapper.ToDTO(original);
            var back = PrerenderMapper.ToDO(dto);

            // Assert
            Assert.NotNull(dto);
            Assert.NotNull(back);
            
            Assert.Equal(original.Type, back.Type);
            Assert.Equal(original.HasPostfxGraphs, back.HasPostfxGraphs);
            Assert.Equal(original.PostfxGraphs.PostfxGraphList.Count, back.PostfxGraphs.PostfxGraphList.Count);

            var originalGraph = original.PostfxGraphs.PostfxGraphList[0];
            var backGraph = back.PostfxGraphs.PostfxGraphList[0];
            
            Assert.Equal(originalGraph.Id, backGraph.Id);
            Assert.Equal(originalGraph.PostfxNodes.Count, backGraph.PostfxNodes.Count);

            var originalNode = originalGraph.PostfxNodes[0];
            var backNode = backGraph.PostfxNodes[0];
            
            Assert.Equal(originalNode.Id, backNode.Id);
            Assert.Equal(originalNode.Class, backNode.Class);
            Assert.Equal(originalNode.Shader, backNode.Shader);
            Assert.Equal(originalNode.IsComputeNode(), backNode.IsComputeNode());
            Assert.Equal(originalNode.Inputs.Count, backNode.Inputs.Count);
        }

        [Fact]
        public void PrerenderMapper_WithNullValues_ShouldHandleGracefully()
        {
            // Arrange
            PrerenderDO nullDo = null;
            PrerenderDTO nullDto = null;

            // Act & Assert
            Assert.Null(PrerenderMapper.ToDTO(nullDo));
            Assert.Null(PrerenderMapper.ToDO(nullDto));
            Assert.NotNull(PrerenderMapper.ToDTO(new PrerenderDO()));
            Assert.NotNull(PrerenderMapper.ToDO(new PrerenderDTO()));
        }

        [Fact]
        public void PrerenderMapper_Validation_ShouldWorkCorrectly()
        {
            // Arrange
            var validPrerender = new PrerenderDO
            {
                Type = "test",
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
                                    Class = "TestClass"
                                }
                            }
                        }
                    }
                }
            };

            var invalidPrerender = new PrerenderDO
            {
                Type = "", // Invalid: empty type
                HasPostfxGraphs = false, // Invalid: no graphs
                PostfxGraphs = new PrerenderPostfxGraphsDO
                {
                    PostfxGraphList = new List<PrerenderPostfxGraphDO>() // Invalid: empty list
                }
            };

            // Act & Assert
            Assert.True(PrerenderMapper.Validate(validPrerender));
            Assert.False(PrerenderMapper.Validate(invalidPrerender));

            var errors = PrerenderMapper.GetValidationErrors(invalidPrerender);
            Assert.Contains("Type is required", errors);
            Assert.Contains("At least one postfx_graph is required", errors);
        }

        [Fact]
        public void PrerenderMapper_DeepCopy_ShouldCreateIndependentCopy()
        {
            // Arrange
            var original = new PrerenderDO
            {
                Type = "test_type",
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
                                    Class = "TestClass"
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var copy = PrerenderMapper.DeepCopy(original);

            // Modify original
            original.Type = "modified_type";
            original.PostfxGraphs.PostfxGraphList[0].Id = "modified_graph";
            original.PostfxGraphs.PostfxGraphList[0].PostfxNodes[0].Id = "modified_node";

            // Assert
            Assert.NotEqual(original.Type, copy.Type);
            Assert.NotEqual(original.PostfxGraphs.PostfxGraphList[0].Id, copy.PostfxGraphs.PostfxGraphList[0].Id);
            Assert.NotEqual(original.PostfxGraphs.PostfxGraphList[0].PostfxNodes[0].Id, copy.PostfxGraphs.PostfxGraphList[0].PostfxNodes[0].Id);
        }

        [Fact]
        public void PrerenderMapper_Merge_ShouldCombineObjectsCorrectly()
        {
            // Arrange
            var target = new PrerenderDO
            {
                Type = "target_type",
                PostfxGraphs = new PrerenderPostfxGraphsDO
                {
                    PostfxGraphList = new List<PrerenderPostfxGraphDO>
                    {
                        new PrerenderPostfxGraphDO
                        {
                            Id = "existing_graph",
                            PostfxNodes = new List<PrerenderPostfxNodeDO>
                            {
                                new PrerenderPostfxNodeDO
                                {
                                    Id = "existing_node",
                                    Class = "ExistingClass"
                                }
                            }
                        }
                    }
                }
            };

            var source = new PrerenderDO
            {
                Type = "source_type",
                PostfxGraphs = new PrerenderPostfxGraphsDO
                {
                    PostfxGraphList = new List<PrerenderPostfxGraphDO>
                    {
                        new PrerenderPostfxGraphDO
                        {
                            Id = "existing_graph", // Same ID as target - should merge
                            PostfxNodes = new List<PrerenderPostfxNodeDO>
                            {
                                new PrerenderPostfxNodeDO
                                {
                                    Id = "new_node",
                                    Class = "NewClass"
                                }
                            }
                        },
                        new PrerenderPostfxGraphDO
                        {
                            Id = "new_graph", // New ID - should be added
                            PostfxNodes = new List<PrerenderPostfxNodeDO>
                            {
                                new PrerenderPostfxNodeDO
                                {
                                    Id = "new_graph_node",
                                    Class = "NewGraphClass"
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var merged = PrerenderMapper.Merge(target, source);

            // Assert
            Assert.Equal("source_type", merged.Type); // Source type should override
            Assert.Equal(2, merged.PostfxGraphs.PostfxGraphList.Count); // Should have both graphs

            var existingGraph = merged.PostfxGraphs.PostfxGraphList.FirstOrDefault(g => g.Id == "existing_graph");
            Assert.NotNull(existingGraph);
            Assert.Equal(2, existingGraph.PostfxNodes.Count); // Should have both existing and new nodes

            var newGraph = merged.PostfxGraphs.PostfxGraphList.FirstOrDefault(g => g.Id == "new_graph");
            Assert.NotNull(newGraph);
            Assert.Single(newGraph.PostfxNodes);
        }

        [Fact]
        public void PrerenderMapper_BatchConversion_ShouldWorkCorrectly()
        {
            // Arrange
            var doList = new List<PrerenderDO>
            {
                new PrerenderDO { Type = "test1" },
                new PrerenderDO { Type = "test2" },
                null // Should handle null gracefully
            };

            // Act
            var dtoList = PrerenderMapper.ToDTO(doList);
            var convertedBack = PrerenderMapper.ToDO(dtoList);

            // Assert
            Assert.Equal(2, dtoList.Count);
            Assert.Equal(2, convertedBack.Count);
            Assert.Equal("test1", convertedBack[0].Type);
            Assert.Equal("test2", convertedBack[1].Type);
        }

        [Fact]
        public void InputMapper_ShouldPreserveInputProperties()
        {
            // Arrange
            var original = new InputDO
            {
                Index = "1",
                Type = "provided",
                Source = "gbuffer_depth"
            };

            // Act
            var dto = InputMapper.ToDTO(original);
            var back = InputMapper.ToDO(dto);

            // Assert
            Assert.Equal(original.Index, back.Index);
            Assert.Equal(original.Type, back.Type);
            Assert.Equal(original.Source, back.Source);
            Assert.True(back.IsProvidedInput());
            Assert.False(back.IsNodeInput());
        }

        [Fact]
        public void PrerenderPostfxNodeDO_BusinessLogic_ShouldWorkAfterMapping()
        {
            // Arrange
            var original = new PrerenderPostfxNodeDO
            {
                Id = "compute_test",
                Class = "rglComputeNode",
                Shader = "test_shader",
                Compute = "true",
                ComputeTgSizeX = "16",
                ComputeTgSizeY = "16",
                Size = "relative",
                Width = "1.0",
                Height = "1.0",
                Inputs = new List<InputDO>
                {
                    new InputDO { Index = "0", Type = "provided", Source = "input1" },
                    new InputDO { Index = "1", Type = "node", Source = "input2" }
                },
                HasPreconditions = true,
                Preconditions = new PreconditionsDO
                {
                    Configs = new List<ConfigDO>
                    {
                        new ConfigDO { Name = "quality_config" }
                    }
                }
            };

            // Act
            var dto = PrerenderPostfxNodeMapper.ToDTO(original);
            var back = PrerenderPostfxNodeMapper.ToDO(dto);

            // Assert
            Assert.True(back.IsComputeNode());
            Assert.True(back.IsRelativeSize());
            Assert.Equal(16, back.GetComputeGroupSizeX());
            Assert.Equal(16, back.GetComputeGroupSizeY());
            Assert.Equal(1.0, back.GetWidthValue());
            Assert.Equal(1.0, back.GetHeightValue());
            Assert.Equal(2, back.Inputs.Count);
            Assert.Single(back.GetProvidedInputs());
            Assert.Single(back.GetNodeInputs());
            Assert.True(back.HasPreconditions);
            Assert.Single(back.Preconditions.Configs);
            Assert.Equal(6, back.GetEstimatedComplexity()); // 1 (base) * 2 (compute) + 2 (inputs) + 2 (preconditions) = 6
        }
    }
}