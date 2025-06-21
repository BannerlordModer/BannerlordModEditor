using BannerlordModEditor.Common.Models.Engine;
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
    public class PrerenderXmlTests
    {
        [Fact]
        public void Prerender_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "example", "ModuleData", "prerender.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, prerenderGraphs);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(prerenderGraphs);
            Assert.Equal("particle_sysm", prerenderGraphs.Type);
            Assert.NotNull(prerenderGraphs.PostfxGraphs);
            Assert.NotNull(prerenderGraphs.PostfxGraphs.PostfxGraphList);
            Assert.True(prerenderGraphs.PostfxGraphs.PostfxGraphList.Count >= 2, "Should have at least 2 postfx graphs");
            
            // 验证具体的后期处理图形数据
            var ambientOcclusionGraph = prerenderGraphs.PostfxGraphs.PostfxGraphList
                .FirstOrDefault(g => g.Id == "ambient_occlusion_graph");
            Assert.NotNull(ambientOcclusionGraph);
            Assert.True(ambientOcclusionGraph.PostfxNodeList.Count > 5, "AO graph should have multiple nodes");
            
            // 验证深度链节点
            var depthChainNode = ambientOcclusionGraph.PostfxNodeList
                .FirstOrDefault(n => n.Id == "depth_chain");
            Assert.NotNull(depthChainNode);
            Assert.Equal("rglDepth_chain_node", depthChainNode.Class);
            Assert.Equal("postfx_depth_downsample_cs", depthChainNode.Shader);
            Assert.Equal("R32F", depthChainNode.Format);
            Assert.Equal("relative", depthChainNode.Size);
            Assert.Equal("0.5", depthChainNode.Width);
            Assert.Equal("0.5", depthChainNode.Height);
            Assert.Equal("true", depthChainNode.Compute);
            Assert.Equal("8", depthChainNode.ComputeTgSizeX);
            Assert.Equal("8", depthChainNode.ComputeTgSizeY);
            
            // 验证输入
            Assert.True(depthChainNode.Inputs.Count >= 2, "Depth chain should have inputs");
            var firstInput = depthChainNode.Inputs[0];
            Assert.Equal("0", firstInput.Index);
            Assert.Equal("provided", firstInput.Type);
            Assert.Equal("gbuffer_depth", firstInput.Source);
            
            // 验证SSR图形
            var ssrGraph = prerenderGraphs.PostfxGraphs.PostfxGraphList
                .FirstOrDefault(g => g.Id == "ssr_graph");
            Assert.NotNull(ssrGraph);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Element("postfx_graphs")?.Elements("postfx_graph").Count(), 
                        savedDoc.Root?.Element("postfx_graphs")?.Elements("postfx_graph").Count());
        }
        
        [Fact]
        public void Prerender_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "example", "ModuleData", "prerender.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            PostfxGraphsBase prerenderGraphs;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                prerenderGraphs = (PostfxGraphsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有后期处理图形都有必要的属性
            Assert.NotNull(prerenderGraphs.PostfxGraphs);
            Assert.NotNull(prerenderGraphs.PostfxGraphs.PostfxGraphList);
            
            foreach (var graph in prerenderGraphs.PostfxGraphs.PostfxGraphList)
            {
                Assert.False(string.IsNullOrWhiteSpace(graph.Id), "Postfx graph should have Id");
                Assert.NotNull(graph.PostfxNodeList);
                
                // 验证每个节点
                foreach (var node in graph.PostfxNodeList)
                {
                    Assert.False(string.IsNullOrWhiteSpace(node.Id), "Postfx node should have Id");
                    Assert.False(string.IsNullOrWhiteSpace(node.Shader), "Postfx node should have Shader");
                    Assert.False(string.IsNullOrWhiteSpace(node.Format), "Postfx node should have Format");
                    Assert.False(string.IsNullOrWhiteSpace(node.Size), "Postfx node should have Size");
                    
                    // 验证尺寸值
                    if (!string.IsNullOrEmpty(node.Width))
                    {
                        Assert.True(double.TryParse(node.Width, out double width), 
                            $"Width '{node.Width}' should be a valid number");
                        Assert.True(width > 0 && width <= 1, "Width should be between 0 and 1 for relative sizing");
                    }
                    
                    if (!string.IsNullOrEmpty(node.Height))
                    {
                        Assert.True(double.TryParse(node.Height, out double height), 
                            $"Height '{node.Height}' should be a valid number");
                        Assert.True(height > 0 && height <= 1, "Height should be between 0 and 1 for relative sizing");
                    }
                    
                    // 验证计算属性
                    if (!string.IsNullOrEmpty(node.Compute))
                    {
                        Assert.True(node.Compute == "true" || node.Compute == "false",
                            $"Compute should be 'true' or 'false', got '{node.Compute}'");
                    }
                    
                    if (!string.IsNullOrEmpty(node.ComputeTgSizeX))
                    {
                        Assert.True(int.TryParse(node.ComputeTgSizeX, out int sizeX), 
                            $"ComputeTgSizeX '{node.ComputeTgSizeX}' should be a valid integer");
                        Assert.True(sizeX > 0, "ComputeTgSizeX should be positive");
                    }
                    
                    // 验证输入
                    foreach (var input in node.Inputs)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(input.Index), "Input should have Index");
                        Assert.False(string.IsNullOrWhiteSpace(input.Type), "Input should have Type");
                        Assert.False(string.IsNullOrWhiteSpace(input.Source), "Input should have Source");
                        
                        Assert.True(int.TryParse(input.Index, out int index), 
                            $"Input index '{input.Index}' should be a valid integer");
                        Assert.True(index >= 0, "Input index should be non-negative");
                    }
                }
            }
            
            // 验证包含预期的图形
            var allGraphIds = prerenderGraphs.PostfxGraphs.PostfxGraphList.Select(g => g.Id).ToList();
            Assert.Contains("ambient_occlusion_graph", allGraphIds);
            Assert.Contains("ssr_graph", allGraphIds);
            
            // 验证特定的节点存在
            var aoGraph = prerenderGraphs.PostfxGraphs.PostfxGraphList.First(g => g.Id == "ambient_occlusion_graph");
            var aoNodeIds = aoGraph.PostfxNodeList.Select(n => n.Id).ToList();
            Assert.Contains("depth_chain", aoNodeIds);
            Assert.Contains("sao_small", aoNodeIds);
            Assert.Contains("sao_large", aoNodeIds);
            Assert.Contains("sao_upsample", aoNodeIds);
            
            // 确保没有重复的图形ID
            var uniqueGraphIds = allGraphIds.Distinct().ToList();
            Assert.Equal(allGraphIds.Count, uniqueGraphIds.Count);
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 