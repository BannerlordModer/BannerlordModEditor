using BannerlordModEditor.Common.Models.Engine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsXmlTests
    {
        [Theory]
        [MemberData(nameof(GetFloraKindsTestFiles))]
        public void FloraKinds_LoadAndSave_ShouldBeLogicallyIdentical(string fileName)
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "FloraKinds", fileName);

            // Deserialization
            var serializer = new XmlSerializer(typeof(FloraKinds));
            FloraKinds? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as FloraKinds;
            }

            Assert.NotNull(model);
            Assert.NotNull(model.FloraKind);
            Assert.True(model.FloraKind.Count > 0);

            // Verify some sample data from the first flora kind
            var firstFloraKind = model.FloraKind[0];
            Assert.NotNull(firstFloraKind.Name);
            Assert.NotNull(firstFloraKind.ViewDistance);

            // Read original XML to determine format
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
            var hasXmlDeclaration = originalXml.StartsWith("<?xml");
            
            // 检查原始XML格式是否为紧凑格式（根元素和子元素在同一行且没有内部缩进）
            // 通过检查文件中是否包含明显的缩进字符（多个制表符或空格）来判断
            var hasIndentation = originalXml.Contains("\n\t\t") || originalXml.Contains("\n    ");
            var isCompactFormat = originalXml.Contains("<flora_kinds><flora_kind") && !hasIndentation;

            string serializedXml;
            
            if (isCompactFormat)
            {
                // 简化实现：对于紧凑格式的XML，使用自定义序列化来匹配原始格式
                // 此方法处理特殊的FloraKinds格式，其中根元素和第一个子元素在同一行
                var compactSettings = new XmlWriterSettings
                {
                    Indent = false, // 无缩进以保持紧凑格式
                    Encoding = new UTF8Encoding(false), // No BOM
                    OmitXmlDeclaration = !hasXmlDeclaration, // 保持XML声明一致性
                    NewLineHandling = System.Xml.NewLineHandling.Replace, // 保持换行符处理
                    NewLineChars = "\n" // 使用标准换行符
                };

                using (var memoryStream = new MemoryStream())
                using (var xmlWriter = XmlWriter.Create(memoryStream, compactSettings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, model, ns);
                    xmlWriter.Flush();
                    serializedXml = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                
                // 后处理：确保XML声明和根元素之间有换行符（如果原始XML有的话）
                if (hasXmlDeclaration && !serializedXml.Contains("?>\n<"))
                {
                    serializedXml = serializedXml.Replace("?>", "?>\n");
                }
            }
            else
            {
                // 完整实现：对于非紧凑格式的XML，使用标准缩进
                var settings = new XmlWriterSettings
                {
                    Indent = true, // 标准缩进
                    IndentChars = "\t", // 使用制表符缩进
                    NewLineChars = "\n", // 使用标准换行符
                    Encoding = new UTF8Encoding(false), // No BOM
                    OmitXmlDeclaration = !hasXmlDeclaration, // 保持XML声明一致性
                    NewLineOnAttributes = false // 属性在同一行
                };

                using (var memoryStream = new MemoryStream())
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    serializer.Serialize(xmlWriter, model, ns);
                    xmlWriter.Flush();
                    serializedXml = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }

            // Comparison - 使用XNode.DeepEquals进行逻辑比较
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            Assert.True(XNode.DeepEquals(originalDoc, serializedDoc), "The XML was not logically identical after a round-trip.");
        }

        public static IEnumerable<object[]> GetFloraKindsTestFiles()
        {
            var testSubsetsDir = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestSubsets", "FloraKinds");
            if (Directory.Exists(testSubsetsDir))
            {
                var files = Directory.GetFiles(testSubsetsDir, "flora_kinds_part_*.xml");
                foreach (var file in files)
                {
                    yield return new object[] { Path.GetFileName(file) };
                }
            }
        }
    }
}