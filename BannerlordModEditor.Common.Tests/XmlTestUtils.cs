using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Tests
{
    public static class XmlTestUtils
    {
        public static T Deserialize<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader);
        }

        public static string Serialize<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "\t",
                Encoding = Encoding.UTF8,
                NewLineChars = "\n",
                NewLineOnAttributes = false
            };
            
            // 创建命名空间管理器，避免添加额外的命名空间
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", ""); // 清空默认命名空间
            
            using var sw = new Utf8StringWriter();
            using var writer = XmlWriter.Create(sw, settings);
            serializer.Serialize(writer, obj, namespaces);
            return sw.ToString();
        }

        public static bool AreStructurallyEqual(string xmlA, string xmlB)
        {
            // 移除注释和标准化空白字符后再比较
            var cleanXmlA = CleanXml(xmlA);
            var cleanXmlB = CleanXml(xmlB);
            
            var docA = XDocument.Parse(cleanXmlA);
            var docB = XDocument.Parse(cleanXmlB);
            
            return XNode.DeepEquals(docA, docB);
        }

        public static string CleanXmlForComparison(string xml)
        {
            return CleanXml(xml);
        }
        
        private static string CleanXml(string xml)
        {
            // 移除XML注释
            var doc = XDocument.Parse(xml);
            RemoveComments(doc);
            
            // 标准化自闭合标签格式
            NormalizeSelfClosingTags(doc);
            
            return doc.ToString();
        }
        
        private static void NormalizeSelfClosingTags(XNode node)
        {
            if (node is XElement element)
            {
                // 如果元素没有子元素且为空，将其转换为自闭合标签格式
                // 这样能确保原始XML和序列化XML的格式一致
                if (!element.HasElements && string.IsNullOrEmpty(element.Value) && !element.IsEmpty)
                {
                    // 移除所有子节点（如果有）
                    element.RemoveAll();
                    // 确保它被序列化为自闭合标签
                    element.IsEmpty = true;
                }
                
                // 递归处理子元素
                foreach (var child in element.Elements())
                {
                    NormalizeSelfClosingTags(child);
                }
            }
        }
        
        private static void RemoveComments(XNode node)
        {
            if (node is XContainer container)
            {
                var comments = container.Nodes().OfType<XComment>().ToList();
                foreach (var comment in comments)
                {
                    comment.Remove();
                }
                
                foreach (var child in container.Nodes())
                {
                    RemoveComments(child);
                }
            }
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}