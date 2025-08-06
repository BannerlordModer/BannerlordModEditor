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
                NewLineChars = "\n",  // 使用Unix换行符匹配原始
                Encoding = new UTF8Encoding(true), // With BOM to match original
                OmitXmlDeclaration = false,
                NewLineOnAttributes = true  // 属性换行以匹配原始格式
            };

            // Save to temporary file and read back to ensure proper BOM handling
            var tempPath = Path.GetTempFileName();
            try
            {
                using (var xmlWriter = XmlWriter.Create(tempPath, settings))
                {
                    // 保留原始命名空间
                    var ns = new XmlSerializerNamespaces();
                    serializer.Serialize(xmlWriter, model, ns);
                }
                
                string serializedXml = File.ReadAllText(tempPath, Encoding.UTF8);
                
                // Debug output to check BOM
                Console.WriteLine($"Serialized XML length: {serializedXml.Length}");
                Console.WriteLine($"Serialized XML starts with BOM: {serializedXml.StartsWith("\uFEFF")}");
                
                // 直接比较文本内容而不是解析后的XML
                var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
                
                // 移除可能的BOM后直接比较文本
                string originalXmlContent = originalXml;
                if (originalXmlContent.StartsWith("\uFEFF"))
                {
                    originalXmlContent = originalXmlContent.Substring(1);
                }
                
                string serializedXmlContent = serializedXml;
                if (serializedXmlContent.StartsWith("\uFEFF"))
                {
                    serializedXmlContent = serializedXmlContent.Substring(1);
                }
                
                // 标准化换行符以忽略平台差异
                originalXmlContent = originalXmlContent.Replace("\r\n", "\n");
                serializedXmlContent = serializedXmlContent.Replace("\r\n", "\n");
                
                // 检查内容差异但允许格式差异（换行vs空格）
                if (originalXmlContent != serializedXmlContent)
                {
                    // 验证XML可以被正确解析
                    try
                    {
                        var originalDoc = XDocument.Parse(originalXml);
                        var serializedDoc = XDocument.Parse(serializedXml);
                        Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "XML逻辑不相等");
                    }
                    catch (Exception ex)
                    {
                        Assert.True(false, $"XML无法正确解析: {ex.Message}");
                    }
                }
                
                // 验证XML可以被正确解析并逻辑相等（忽略格式差异）
                try
                {
                    var originalDoc = XDocument.Parse(originalXml);
                    var serializedDoc = XDocument.Parse(serializedXml);
                    Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "XML逻辑不相等");
                }
                catch (Exception ex)
                {
                    Assert.True(false, $"XML无法正确解析: {ex.Message}");
                }
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
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