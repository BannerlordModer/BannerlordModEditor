using BannerlordModEditor.Common.Models.Engine;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugBeforeTransparentsGraph
    {
        [Fact]
        public void Debug_SerializeBeforeTransparentsGraph()
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

            // 输出序列化结果用于调试
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
            
            // 写入文件用于比较
            File.WriteAllText("debug_original.xml", originalXml);
            File.WriteAllText("debug_serialized.xml", serializedXml);
            
            // 这个测试总是通过，只是为了输出调试信息
            Assert.True(true);
        }
    }
}