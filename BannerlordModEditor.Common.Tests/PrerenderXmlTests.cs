using BannerlordModEditor.Common.Models.Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class PrerenderXmlTests
    {
        [Fact]
        public void Prerender_CanDeserializeFromXml()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_sysm", result.Type);
            Assert.NotNull(result.PostfxGraphs);
            Assert.NotNull(result.PostfxGraphs.PostfxGraphList);
            Assert.True(result.PostfxGraphs.PostfxGraphList.Count >= 2, "应该有至少2个后期处理图形");
        }

        [Fact]
        public void Prerender_FromActualFile_CanDeserializeCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert - 基本数据完整性
            Assert.NotNull(prerenderGraphs);
            Assert.NotNull(prerenderGraphs.PostfxGraphs);
            Assert.NotNull(prerenderGraphs.PostfxGraphs.PostfxGraphList);
            Assert.True(prerenderGraphs.PostfxGraphs.PostfxGraphList.Count >= 2, "应该有至少2个后期处理图形");
            
            // 验证所有图形都有必需的属性
            foreach (var graph in prerenderGraphs.PostfxGraphs.PostfxGraphList)
            {
                Assert.False(string.IsNullOrWhiteSpace(graph.Id), "后期处理图形必须有ID");
                Assert.NotNull(graph.PostfxNodeList);
                Assert.True(graph.PostfxNodeList.Count > 0, $"图形 '{graph.Id}' 应该有至少一个节点");
            }
            
            // 验证关键图形的存在
            var requiredGraphs = new[] { "ambient_occlusion_graph", "ssr_graph" };
            foreach (var requiredGraph in requiredGraphs)
            {
                var graph = prerenderGraphs.PostfxGraphs.PostfxGraphList.FirstOrDefault(g => g.Id == requiredGraph);
                Assert.NotNull(graph);
                Assert.Equal(requiredGraph, graph.Id);
            }
        }

        [Fact]
        public void Prerender_ValidateNodeConfiguration_HaveConsistentProperties()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert - 验证节点配置的一致性和合理性
            foreach (var graph in prerenderGraphs.PostfxGraphs.PostfxGraphList)
            {
                foreach (var node in graph.PostfxNodeList)
                {
                    // 验证必需属性
                    Assert.False(string.IsNullOrWhiteSpace(node.Id), $"节点必须有ID (图形: {graph.Id})");
                    Assert.False(string.IsNullOrWhiteSpace(node.Shader), $"节点必须有着色器 (节点: {node.Id})");
                    Assert.False(string.IsNullOrWhiteSpace(node.Format), $"节点必须有格式 (节点: {node.Id})");
                    Assert.False(string.IsNullOrWhiteSpace(node.Size), $"节点必须有尺寸类型 (节点: {node.Id})");
                    
                    // 验证尺寸配置
                    if (node.Size == "relative")
                    {
                        if (!string.IsNullOrEmpty(node.Width))
                        {
                            Assert.True(double.TryParse(node.Width, NumberStyles.Float, CultureInfo.InvariantCulture, out double width),
                                $"宽度应该是有效数字: {node.Width} (节点: {node.Id})");
                            Assert.True(width > 0 && width <= 1, $"相对宽度应在0-1范围内: {width} (节点: {node.Id})");
                        }
                        
                        if (!string.IsNullOrEmpty(node.Height))
                        {
                            Assert.True(double.TryParse(node.Height, NumberStyles.Float, CultureInfo.InvariantCulture, out double height),
                                $"高度应该是有效数字: {node.Height} (节点: {node.Id})");
                            Assert.True(height > 0 && height <= 1, $"相对高度应在0-1范围内: {height} (节点: {node.Id})");
                        }
                    }
                    
                    // 验证计算配置
                    if (!string.IsNullOrEmpty(node.Compute))
                    {
                        Assert.True(node.Compute == "true" || node.Compute == "false",
                            $"计算属性应该是true或false: {node.Compute} (节点: {node.Id})");
                        
                        // 如果是计算节点，应该有计算组大小
                        if (node.Compute == "true")
                        {
                            Assert.False(string.IsNullOrEmpty(node.ComputeTgSizeX), $"计算节点应该有ComputeTgSizeX (节点: {node.Id})");
                            Assert.False(string.IsNullOrEmpty(node.ComputeTgSizeY), $"计算节点应该有ComputeTgSizeY (节点: {node.Id})");
                            
                            Assert.True(int.TryParse(node.ComputeTgSizeX, out int sizeX),
                                $"ComputeTgSizeX应该是有效整数: {node.ComputeTgSizeX} (节点: {node.Id})");
                            Assert.True(sizeX > 0 && sizeX <= 32, $"ComputeTgSizeX应在合理范围内: {sizeX} (节点: {node.Id})");
                            
                            Assert.True(int.TryParse(node.ComputeTgSizeY, out int sizeY),
                                $"ComputeTgSizeY应该是有效整数: {node.ComputeTgSizeY} (节点: {node.Id})");
                            Assert.True(sizeY > 0 && sizeY <= 32, $"ComputeTgSizeY应在合理范围内: {sizeY} (节点: {node.Id})");
                        }
                    }
                    
                    // 验证格式有效性 - 检查是否为有效的图形格式
                    Assert.True(!string.IsNullOrEmpty(node.Format), $"节点格式不能为空 (节点: {node.Id})");
                    // 简单验证格式字符串包含常见的图形格式模式
                    Assert.True(node.Format.Contains("R") || node.Format.Contains("G") || node.Format.Contains("B") || node.Format.Contains("A"),
                        $"格式应该是有效的图形格式: {node.Format} (节点: {node.Id})");
                }
            }
        }

        [Fact]
        public void Prerender_ValidateInputConfiguration_HaveValidReferences()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert - 验证输入配置的有效性
            foreach (var graph in prerenderGraphs.PostfxGraphs.PostfxGraphList)
            {
                // 收集图形中的所有节点ID
                var nodeIds = graph.PostfxNodeList.Select(n => n.Id).ToHashSet();
                
                foreach (var node in graph.PostfxNodeList)
                {
                    foreach (var input in node.Inputs)
                    {
                        // 验证输入属性
                        Assert.False(string.IsNullOrWhiteSpace(input.Index), $"输入必须有索引 (节点: {node.Id})");
                        Assert.False(string.IsNullOrWhiteSpace(input.Type), $"输入必须有类型 (节点: {node.Id})");
                        Assert.False(string.IsNullOrWhiteSpace(input.Source), $"输入必须有来源 (节点: {node.Id})");
                        
                        // 验证索引
                        Assert.True(int.TryParse(input.Index, out int index),
                            $"输入索引应该是有效整数: {input.Index} (节点: {node.Id})");
                        Assert.True(index >= 0, $"输入索引应该非负: {index} (节点: {node.Id})");
                        
                        // 验证类型
                        var validTypes = new[] { "provided", "node" };
                        Assert.Contains(input.Type, validTypes);
                        
                        // 验证来源引用
                        if (input.Type == "node")
                        {
                            Assert.Contains(input.Source, nodeIds);
                        }
                        else if (input.Type == "provided")
                        {
                            // provided输入应该是系统提供的缓冲区 - 验证源名称格式合理
                            Assert.True(!string.IsNullOrWhiteSpace(input.Source), $"Provided输入源不能为空 (节点: {node.Id})");
                            // 简单验证源名称包含常见的缓冲区名称模式
                            var isValidSource = input.Source.Contains("gbuffer") || input.Source.Contains("buffer") || 
                                              input.Source.Contains("depth") || input.Source.Contains("color") ||
                                              input.Source.Contains("screen") || input.Source.Contains("rt");
                            Assert.True(isValidSource, $"Provided源应该是有效的系统缓冲区: {input.Source} (节点: {node.Id})");
                        }
                    }
                }
            }
        }

        [Fact]
        public void Prerender_ValidateSpecificGraphConfigurations()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert - 验证特定图形配置的正确性
            var aoGraph = prerenderGraphs.PostfxGraphs.PostfxGraphList.FirstOrDefault(g => g.Id == "ambient_occlusion_graph");
            Assert.NotNull(aoGraph);
            
            // 验证AO图形的关键节点
            var requiredAONodes = new[] { "depth_chain", "sao_small", "sao_large", "sao_upsample" };
            var aoNodeIds = aoGraph.PostfxNodeList.Select(n => n.Id).ToList();
            foreach (var requiredNode in requiredAONodes)
            {
                Assert.Contains(requiredNode, aoNodeIds);
            }
            
            // 验证深度链节点配置
            var depthChainNode = aoGraph.PostfxNodeList.FirstOrDefault(n => n.Id == "depth_chain");
            Assert.NotNull(depthChainNode);
            Assert.Equal("rglDepth_chain_node", depthChainNode.Class);
            Assert.Equal("postfx_depth_downsample_cs", depthChainNode.Shader);
            Assert.Equal("R32F", depthChainNode.Format);
            Assert.Equal("0.5", depthChainNode.Width);
            Assert.Equal("0.5", depthChainNode.Height);
            
            // 验证SSR图形
            var ssrGraph = prerenderGraphs.PostfxGraphs.PostfxGraphList.FirstOrDefault(g => g.Id == "ssr_graph");
            Assert.NotNull(ssrGraph);
            Assert.True(ssrGraph.PostfxNodeList.Count > 0, "SSR图形应该有节点");
        }

        [Fact]
        public void Prerender_ValidateGraphIdUniqueness()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Act & Assert
            var allGraphIds = prerenderGraphs.PostfxGraphs.PostfxGraphList.Select(g => g.Id).ToList();
            var uniqueGraphIds = allGraphIds.Distinct().ToList();
            
            Assert.Equal(allGraphIds.Count, uniqueGraphIds.Count);
            
            // 在每个图形中验证节点ID的唯一性
            foreach (var graph in prerenderGraphs.PostfxGraphs.PostfxGraphList)
            {
                var nodeIds = graph.PostfxNodeList.Select(n => n.Id).ToList();
                var uniqueNodeIds = nodeIds.Distinct().ToList();
                
                Assert.Equal(nodeIds.Count, uniqueNodeIds.Count);
                
                // 验证ID命名约定
                foreach (var nodeId in nodeIds)
                {
                    Assert.False(string.IsNullOrWhiteSpace(nodeId), "节点ID不能为空");
                    Assert.True(nodeId.All(c => char.IsLower(c) || c == '_'), $"节点ID应该使用小写字母和下划线: {nodeId}");
                }
            }
        }

        [Fact]
        public void Prerender_RoundTripSerialization_MaintainsDataIntegrity()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "prerender.xml");
            
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase original;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                original = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化然后反序列化
            string xmlString;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlString = writer.ToString();
            }
            
            PostfxGraphsBase roundTrip;
            using (var reader = new StringReader(xmlString))
            {
                roundTrip = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.Equal(original.Type, roundTrip.Type);
            Assert.Equal(original.PostfxGraphs.PostfxGraphList.Count, roundTrip.PostfxGraphs.PostfxGraphList.Count);
            
            for (int i = 0; i < original.PostfxGraphs.PostfxGraphList.Count; i++)
            {
                var originalGraph = original.PostfxGraphs.PostfxGraphList[i];
                var roundTripGraph = roundTrip.PostfxGraphs.PostfxGraphList[i];
                
                Assert.Equal(originalGraph.Id, roundTripGraph.Id);
                Assert.Equal(originalGraph.PostfxNodeList.Count, roundTripGraph.PostfxNodeList.Count);
                
                for (int j = 0; j < originalGraph.PostfxNodeList.Count; j++)
                {
                    var originalNode = originalGraph.PostfxNodeList[j];
                    var roundTripNode = roundTripGraph.PostfxNodeList[j];
                    
                    Assert.Equal(originalNode.Id, roundTripNode.Id);
                    Assert.Equal(originalNode.Shader, roundTripNode.Shader);
                    Assert.Equal(originalNode.Format, roundTripNode.Format);
                    Assert.Equal(originalNode.Width, roundTripNode.Width);
                    Assert.Equal(originalNode.Height, roundTripNode.Height);
                }
            }
        }

        [Fact]
        public void Prerender_WithHandCraftedXml_CanDeserialize()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_sysm"">
    <postfx_graphs>
        <postfx_graph id=""test_graph"">
            <postfx_node id=""test_node"" shader=""test_shader"" format=""R8G8B8A8"" size=""relative"" width=""1"" height=""1"" compute=""true"" compute_tg_size_x=""8"" compute_tg_size_y=""8"">
                <input index=""0"" type=""provided"" source=""gbuffer_depth"" />
            </postfx_node>
        </postfx_graph>
    </postfx_graphs>
</base>";

            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            
            // Act
            PostfxGraphsBase result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_sysm", result.Type);
            Assert.NotNull(result.PostfxGraphs);
            Assert.Single(result.PostfxGraphs.PostfxGraphList);
            
            var graph = result.PostfxGraphs.PostfxGraphList[0];
            Assert.Equal("test_graph", graph.Id);
            Assert.Single(graph.PostfxNodeList);
            
            var node = graph.PostfxNodeList[0];
            Assert.Equal("test_node", node.Id);
            Assert.Equal("test_shader", node.Shader);
            Assert.Equal("R8G8B8A8", node.Format);
        }

        [Fact]
        public void Prerender_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""particle_sysm"">
    <postfx_graphs>
    </postfx_graphs>
</base>";

            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            
            // Act
            PostfxGraphsBase result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("particle_sysm", result.Type);
            Assert.NotNull(result.PostfxGraphs);
            Assert.NotNull(result.PostfxGraphs.PostfxGraphList);
            Assert.Empty(result.PostfxGraphs.PostfxGraphList);
        }
    }
} 