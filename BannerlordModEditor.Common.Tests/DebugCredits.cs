using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Tests
{
    public class DebugCredits
    {
        public static void DebugCreditsSerializationDifference()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsExternalPartnersPC.xml");

            // 读取原始 XML
            var originalXml = File.ReadAllText(xmlPath, Encoding.UTF8);
            Console.WriteLine("原始 XML:");
            Console.WriteLine(originalXml);
            Console.WriteLine();

            // 反序列化
            var serializer = new XmlSerializer(typeof(Credits));
            Credits? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as Credits;
            }

            Console.WriteLine($"反序列化结果: model != null: {model != null}");
            if (model != null)
            {
                Console.WriteLine($"Category count: {model.Category?.Count}");
                Console.WriteLine($"LoadFromFile count: {model.LoadFromFile?.Count}");
            }
            Console.WriteLine();

            // 序列化
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                Encoding = new UTF8Encoding(false),
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

            Console.WriteLine("序列化后的 XML:");
            Console.WriteLine(serializedXml);
            Console.WriteLine();

            // 比较
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);

            Console.WriteLine("原始 XML (格式化):");
            Console.WriteLine(originalDoc.ToString());
            Console.WriteLine();

            Console.WriteLine("序列化 XML (格式化):");
            Console.WriteLine(serializedDoc.ToString());
            Console.WriteLine();

            Console.WriteLine($"XNode.DeepEquals result: {XNode.DeepEquals(originalDoc, serializedDoc)}");
        }
    }
} 