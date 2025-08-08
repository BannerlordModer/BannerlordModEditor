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
                Encoding = new UTF8Encoding(false), // 禁用 BOM，严格与原始 XML 一致
                NewLineChars = "\n",
                NewLineOnAttributes = false
            };

            // 创建命名空间管理器，避免添加额外的命名空间
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", ""); // 清空默认命名空间

            using var ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms, settings))
            {
                serializer.Serialize(writer, obj, namespaces);
            }
            ms.Position = 0;
            using var sr = new StreamReader(ms, Encoding.UTF8);
            return sr.ReadToEnd();
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

        // XML结构详细比较，区分属性为null与属性不存在，检测节点缺失/多余，返回详细差异报告
        public static XmlStructureDiffReport CompareXmlStructure(string xmlA, string xmlB)
        {
            var cleanXmlA = CleanXml(xmlA);
            var cleanXmlB = CleanXml(xmlB);

            var docA = XDocument.Parse(cleanXmlA);
            var docB = XDocument.Parse(cleanXmlB);

            var report = new XmlStructureDiffReport();
            var rootName = docA.Root?.Name.LocalName ?? "";
            CompareElements(docA.Root, docB.Root, rootName, report);

            // 节点和属性数量统计
            int nodeCountA = docA.Descendants().Count();
            int nodeCountB = docB.Descendants().Count();
            int attrCountA = docA.Descendants().Sum(e => e.Attributes().Count());
            int attrCountB = docB.Descendants().Sum(e => e.Attributes().Count());
            if (nodeCountA != nodeCountB)
                report.NodeCountDifference = $"节点数量不同: A={nodeCountA}, B={nodeCountB}";
            if (attrCountA != attrCountB)
                report.AttributeCountDifference = $"属性数量不同: A={attrCountA}, B={attrCountB}";

            return report;
        }

        private static void CompareElements(XElement? elemA, XElement? elemB, string path, XmlStructureDiffReport report)
        {
            if (elemA == null && elemB == null)
                return;
            if (elemA == null)
            {
                report.MissingNodes.Add($"{path} (A缺失)");
                return;
            }
            if (elemB == null)
            {
                report.ExtraNodes.Add($"{path} (B缺失)");
                return;
            }
            // 节点名不同
            if (elemA.Name != elemB.Name)
            {
                report.NodeNameDifferences.Add($"{path}: A={elemA.Name}, B={elemB.Name}");
            }

            // 属性比较
            var attrsA = elemA.Attributes();
            var attrsB = elemB.Attributes();
            var attrNames = new HashSet<string>(attrsA.Select(a => a.Name.LocalName).Concat(attrsB.Select(b => b.Name.LocalName)));

            foreach (var name in attrNames)
            {
                var attrA = attrsA.FirstOrDefault(a => a.Name.LocalName == name);
                var attrB = attrsB.FirstOrDefault(b => b.Name.LocalName == name);

                if (attrA == null && attrB == null)
                    continue;
                if (attrA == null)
                {
                    report.MissingAttributes.Add($"{path}@{name} (A缺失)");
                    continue;
                }
                if (attrB == null)
                {
                    report.ExtraAttributes.Add($"{path}@{name} (B缺失)");
                    continue;
                }
                // 区分null与空字符串
                if (attrA.Value != attrB.Value)
                {
                    string valA = attrA.Value == "" ? "空字符串" : attrA.Value ?? "null";
                    string valB = attrB.Value == "" ? "空字符串" : attrB.Value ?? "null";
                    report.AttributeValueDifferences.Add($"{path}@{name}: A={valA}, B={valB}");
                }
            }

            // 子节点比较
            var childrenA = elemA.Elements().ToList();
            var childrenB = elemB.Elements().ToList();

            int maxCount = Math.Max(childrenA.Count, childrenB.Count);
            for (int i = 0; i < maxCount; i++)
            {
                XElement? childA = i < childrenA.Count ? childrenA[i] : null;
                XElement? childB = i < childrenB.Count ? childrenB[i] : null;
                string nodeName = childA?.Name.LocalName ?? childB?.Name.LocalName ?? "?";
                string childPath = string.IsNullOrEmpty(path)
                    ? $"{nodeName}[{i}]"
                    : $"{path}/{nodeName}[{i}]";
                CompareElements(childA, childB, childPath, report);
            }

            // 文本内容比较（忽略空白）
            string textA = elemA.Value?.Trim() ?? "";
            string textB = elemB.Value?.Trim() ?? "";
            if (textA != textB)
            {
                report.TextDifferences.Add($"{path}: A文本='{textA}', B文本='{textB}'");
            }
        }

        // 差异报告对象
        public class XmlStructureDiffReport
        {
            public List<string> MissingNodes { get; } = new();
            public List<string> ExtraNodes { get; } = new();
            public List<string> NodeNameDifferences { get; } = new();
            public List<string> MissingAttributes { get; } = new();
            public List<string> ExtraAttributes { get; } = new();
            public List<string> AttributeValueDifferences { get; } = new();
            public List<string> TextDifferences { get; } = new();

            // 新增：节点和属性数量差异字段
            public string? NodeCountDifference { get; set; }
            public string? AttributeCountDifference { get; set; }

            public bool IsStructurallyEqual =>
                !MissingNodes.Any() &&
                !ExtraNodes.Any() &&
                !NodeNameDifferences.Any() &&
                !MissingAttributes.Any() &&
                !ExtraAttributes.Any() &&
                !AttributeValueDifferences.Any() &&
                !TextDifferences.Any() &&
                NodeCountDifference == null &&
                AttributeCountDifference == null;
        }

        public static string CleanXmlForComparison(string xml)
        {
            return CleanXml(xml);
        }

        // 节点和属性数量统计
        public static (int nodeCount, int attrCount) CountNodesAndAttributes(string xml)
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            int nodeCount = 0, attrCount = 0;
            foreach (var node in doc.Descendants())
            {
                nodeCount++;
                attrCount += node.Attributes().Count();
            }
            return (nodeCount, attrCount);
        }

        // 检查序列化后没有属性从无变为 null
        public static bool NoNewNullAttributes(string originalXml, string serializedXml)
        {
            var origDoc = System.Xml.Linq.XDocument.Parse(originalXml);
            var serDoc = System.Xml.Linq.XDocument.Parse(serializedXml);

            var origAttrs = new HashSet<string>();
            foreach (var node in origDoc.Descendants())
                foreach (var attr in node.Attributes())
                    origAttrs.Add($"{node.Name.LocalName}:{attr.Name.LocalName}");

            foreach (var node in serDoc.Descendants())
            {
                foreach (var attr in node.Attributes())
                {
                    var key = $"{node.Name.LocalName}:{attr.Name.LocalName}";
                    if (!origAttrs.Contains(key) && string.IsNullOrEmpty(attr.Value))
                        return false;
                }
            }
            return true;
        }

        // 调试辅助：输出所有节点和属性名
        public static void LogAllNodesAndAttributes(string xml, string tag)
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            Console.WriteLine($"--- {tag} 节点和属性 ---");
            foreach (var node in doc.Descendants())
            {
                Console.WriteLine($"节点: {node.Name.LocalName}");
                foreach (var attr in node.Attributes())
                {
                    Console.WriteLine($"  属性: {attr.Name.LocalName} = {attr.Value}");
                }
            }
        }

        private static string CleanXml(string xml)
        {
            // 移除XML注释
            var doc = XDocument.Parse(xml);
            RemoveComments(doc);

            // 对所有元素属性按名称排序，消除属性顺序影响
            SortAttributes(doc.Root);

            // 标准化自闭合标签格式
            NormalizeSelfClosingTags(doc);

            // 保留 XML 声明头（如 encoding="utf-8"），并输出根节点内容
            var declaration = doc.Declaration != null ? doc.Declaration.ToString() + "\n" : "";
            return declaration + doc.Root.ToString();
        }

        private static void SortAttributes(XElement element)
        {
            if (element == null) return;
            // 获取所有属性并按名称排序
            var sortedAttributes = element.Attributes().OrderBy(a => a.Name.ToString()).ToList();
            element.RemoveAttributes();
            foreach (var attr in sortedAttributes)
                element.Add(attr);
            // 递归对子元素排序
            foreach (var child in element.Elements())
                SortAttributes(child);
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
                    // 移除所有内容后，序列化时会自动变为自闭合标签
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