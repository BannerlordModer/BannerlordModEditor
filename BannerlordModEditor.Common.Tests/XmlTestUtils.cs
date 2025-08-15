using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.Linq;
using BannerlordModEditor.Common.Models;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public static class XmlTestUtils
    {
        public enum ComparisonMode
        {
            Strict,
            Logical,
            Loose
        }

        public class XmlComparisonOptions
        {
            public ComparisonMode Mode { get; set; } = ComparisonMode.Logical;
            public bool IgnoreComments { get; set; } = true;
            public bool IgnoreWhitespace { get; set; } = true;
            public bool IgnoreAttributeOrder { get; set; } = true;
            public bool AllowCaseInsensitiveBooleans { get; set; } = true;
            public bool AllowNumericTolerance { get; set; } = true;
            public double NumericTolerance { get; set; } = 0.0001;
        }

        public static IReadOnlyList<string> CommonBooleanTrueValues = 
            new[] { "true", "True", "TRUE", "1", "yes", "Yes", "YES", "on", "On", "ON" };
        
        public static IReadOnlyList<string> CommonBooleanFalseValues = 
            new[] { "false", "False", "FALSE", "0", "no", "No", "NO", "off", "Off", "OFF" };
        public static string? ReadTestDataOrSkip(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null; // Signal caller to skip
            }
            return File.ReadAllText(filePath);
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException("XML cannot be null or empty", nameof(xml));
                
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            var obj = (T)serializer.Deserialize(reader)!;
            
            // 特殊处理CombatParametersDO来检测是否有definitions元素和空的combat_parameters元素
            if (obj is BannerlordModEditor.Common.Models.DO.CombatParametersDO combatParams)
            {
                var doc = XDocument.Parse(xml);
                combatParams.HasDefinitions = doc.Root?.Element("definitions") != null;
                var combatParamsElement = doc.Root?.Element("combat_parameters");
                combatParams.HasEmptyCombatParameters = combatParamsElement != null && 
                    (combatParamsElement.Elements().Count() == 0 || combatParamsElement.Elements("combat_parameter").Count() == 0);
            }
            
            // 特殊处理ItemHolstersDO来检测是否有空的item_holsters元素
            if (obj is BannerlordModEditor.Common.Models.DO.ItemHolstersDO itemHolsters)
            {
                var doc = XDocument.Parse(xml);
                var itemHolstersElement = doc.Root?.Element("item_holsters");
                itemHolsters.HasEmptyItemHolsters = itemHolstersElement != null && 
                    (itemHolstersElement.Elements().Count() == 0 || itemHolstersElement.Elements("item_holster").Count() == 0);
            }
            
            // 特殊处理CreditsDO来保持原始XML元素顺序
            if (obj is BannerlordModEditor.Common.Models.DO.CreditsDO credits)
            {
                var doc = XDocument.Parse(xml);
                // 读取原始Category元素的顺序
                var categoryElements = doc.Root?.Elements("Category").ToList();
                if (categoryElements != null)
                {
                    // 确保Categories列表按照原始XML顺序排列
                    for (int i = 0; i < categoryElements.Count && i < credits.Categories.Count; i++)
                    {
                        var categoryElement = categoryElements[i];
                        var category = credits.Categories[i];
                        
                        // 处理每个Category内部的元素顺序
                        var sectionElements = categoryElement.Elements("Section").ToList();
                        var entryElements = categoryElement.Elements("Entry").ToList();
                        var emptyLineElements = categoryElement.Elements("EmptyLine").ToList();
                        var loadFromFileElements = categoryElement.Elements("LoadFromFile").ToList();
                        var imageElements = categoryElement.Elements("Image").ToList();
                        
                        // 重新排序Category内部的元素以匹配原始XML
                        ReorderCategoryElements(category, sectionElements, entryElements, emptyLineElements, loadFromFileElements, imageElements);
                    }
                }
            }
            
            return obj;
        }
        
        // 移除复杂的SetSpecifiedProperties相关方法

        public static string Serialize<T>(T obj)
        {
            return Serialize(obj, null);
        }

        public static string Serialize<T>(T obj, string? originalXml)
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
            // 创建命名空间管理器
            var namespaces = new XmlSerializerNamespaces();
            
            // 如果提供了原始XML，则提取并保留其命名空间声明
            if (!string.IsNullOrEmpty(originalXml))
            {
                try
                {
                    var originalDoc = XDocument.Parse(originalXml);
                    if (originalDoc.Root != null)
                    {
                        foreach (var attr in originalDoc.Root.Attributes())
                        {
                            // 检查是否为命名空间声明属性
                            if (attr.IsNamespaceDeclaration)
                            {
                                // 处理默认命名空间（没有前缀的情况）
                                if (attr.Name.LocalName == "xmlns")
                                {
                                    namespaces.Add("", attr.Value);
                                }
                                else
                                {
                                    // 处理带前缀的命名空间
                                    namespaces.Add(attr.Name.LocalName, attr.Value);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // 如果解析失败，确保不添加任何命名空间
                    namespaces.Add(string.Empty, string.Empty);
                }
            }
            else
            {
                // 确保不添加任何命名空间声明
                namespaces.Add(string.Empty, string.Empty);
            }

            using var ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms, settings))
            {
                serializer.Serialize(writer, obj, namespaces);
            }
            ms.Position = 0;
            using var sr = new StreamReader(ms, Encoding.UTF8);
            
            var serialized = sr.ReadToEnd();
            
            // 后处理：移除任何自动添加的命名空间声明并标准化格式
            var doc = XDocument.Parse(serialized);
            
            // 移除命名空间声明
            RemoveNamespaceDeclarations(doc);
            
            // 对所有元素属性按名称排序
            SortAttributes(doc.Root);
            
            // 标准化自闭合标签格式
            NormalizeSelfClosingTags(doc);
            
            // 保留 XML 声明头并输出标准化后的XML
            var declaration = doc.Declaration != null ? doc.Declaration.ToString() + "\n" : "";
            return declaration + doc.Root.ToString();
        }

        public static bool AreStructurallyEqual(string xmlA, string xmlB)
        {
            var report = CompareXmlStructure(xmlA, xmlB);
            return report.IsStructurallyEqual;
        }

        // XML结构详细比较，区分属性为null与属性不存在，检测节点缺失/多余，返回详细差异报告
        public static XmlStructureDiffReport CompareXmlStructure(string xmlA, string xmlB)
        {
            // 首先解析原始XML
            var docA = XDocument.Parse(xmlA);
            var docB = XDocument.Parse(xmlB);
            
            // 移除注释
            RemoveComments(docA);
            RemoveComments(docB);
            
            // 标准化boolean属性值（将"True"转换为"true"等）
            NormalizeBooleanValues(docA);
            NormalizeBooleanValues(docB);
            
            // 标准化数值属性值，确保数值格式一致性
            NormalizeNumericValues(docA);
            NormalizeNumericValues(docB);
            
            // 对所有元素属性按名称排序，消除属性顺序影响
            SortAttributes(docA.Root);
            SortAttributes(docB.Root);
            
            // 标准化自闭合标签格式
            NormalizeSelfClosingTags(docA);
            NormalizeSelfClosingTags(docB);

            var report = new XmlStructureDiffReport();
            var rootName = docA.Root?.Name.LocalName ?? "";
            
            // 在比较前移除命名空间声明
            var contentA = RemoveNamespaceDeclarations(docA);
            var contentB = RemoveNamespaceDeclarations(docB);
            
            // 对所有元素属性按名称排序，消除属性顺序影响
            SortAttributes(contentA.Root);
            SortAttributes(contentB.Root);
            
            CompareElements(contentA.Root, contentB.Root, rootName, report);

            // 节点和属性数量统计（使用移除命名空间声明后的结果）
            int nodeCountA = contentA.Descendants().Count();
            int nodeCountB = contentB.Descendants().Count();
            int attrCountA = contentA.Descendants().Sum(e => e.Attributes().Count());
            int attrCountB = contentB.Descendants().Sum(e => e.Attributes().Count());
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
                    string pathFormat = path == "root" ? "/@{name}" : $"{path}@{name}";
                    report.MissingAttributes.Add($"{pathFormat.Replace("{name}", name)} (A缺失)");
                    continue;
                }
                if (attrB == null)
                {
                    string pathFormat = path == "root" ? "/@{name}" : $"{path}@{name}";
                    report.ExtraAttributes.Add($"{pathFormat.Replace("{name}", name)} (B缺失)");
                    continue;
                }
                // 智能比较属性值，处理布尔值和数值的差异
                if (!AreAttributeValuesEqual(attrA.Value, attrB.Value))
                {
                    string valA = attrA.Value == "" ? "空字符串" : attrA.Value ?? "null";
                    string valB = attrB.Value == "" ? "空字符串" : attrB.Value ?? "null";
                    string pathFormat = path == "root" ? "/@{name}" : $"{path}@{name}";
                    report.AttributeValueDifferences.Add($"{pathFormat.Replace("{name}", name)}: A={valA}, B={valB}");
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
                string childPath = path == "root" 
                    ? $"/{nodeName}[{i}]"
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

        // 标准化Boolean属性值，将"True"/"False"转换为"true"/"false"
        private static void NormalizeBooleanValues(XDocument doc)
        {
            foreach (var element in doc.Descendants())
            {
                foreach (var attr in element.Attributes().ToList()) // 使用ToList()避免修改集合时的异常
                {
                    var value = attr.Value;
                    // 扩展布尔值标准化，支持更多格式
                    if (CommonBooleanTrueValues.Contains(value))
                    {
                        attr.Value = "true";
                    }
                    else if (CommonBooleanFalseValues.Contains(value))
                    {
                        attr.Value = "false";
                    }
                }
            }
        }

        // 标准化数值属性值，确保数值格式一致性
        private static void NormalizeNumericValues(XDocument doc)
        {
            foreach (var element in doc.Descendants())
            {
                foreach (var attr in element.Attributes().ToList())
                {
                    var value = attr.Value;
                    // 检查是否为数值格式
                    if (double.TryParse(value, out var numericValue))
                    {
                        // 特殊处理percentage属性，保持小数格式
                        if (attr.Name == "percentage" && value.Contains('.'))
                        {
                            // 对于percentage属性，如果原始值有小数点，保留到小数点后6位
                            attr.Value = numericValue.ToString("F6").TrimEnd('0').TrimEnd('.');
                        }
                        else
                        {
                            // 如果原始值有小数点，保留原始格式，否则使用标准格式
                            if (value.Contains('.') || value.Contains(','))
                            {
                                // 保留原始小数位数，但标准化格式
                                attr.Value = numericValue.ToString("F6").TrimEnd('0').TrimEnd('.');
                            }
                            else
                            {
                                attr.Value = numericValue.ToString("F0");
                            }
                        }
                    }
                }
            }
        }

        // 智能比较属性值，处理布尔值和数值的差异
        private static bool AreAttributeValuesEqual(string? valueA, string? valueB)
        {
            // Handle null/empty cases
            if (valueA == null && valueB == null) return true;
            if (valueA == null || valueB == null) return false;
            if (valueA == valueB) return true;
            
            // Check if both values are numeric
            if (double.TryParse(valueA, out var numA) && double.TryParse(valueB, out var numB))
            {
                // Use default tolerance of 0.0001 for numeric comparison
                return Math.Abs(numA - numB) < 0.0001;
            }
            
            // Check if both values are boolean values
            if (IsBooleanValue(valueA) && IsBooleanValue(valueB))
            {
                return ParseBoolean(valueA) == ParseBoolean(valueB);
            }
            
            // Fall back to exact string comparison
            return valueA == valueB;
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

        public static bool IsBooleanValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var normalized = value.Trim().ToLowerInvariant();
            return CommonBooleanTrueValues.Contains(normalized) || CommonBooleanFalseValues.Contains(normalized);
        }

        public static bool AreBooleanValuesEqual(string value1, string value2)
        {
            return ParseBoolean(value1) == ParseBoolean(value2);
        }

        public static bool ParseBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var normalized = value.Trim().ToLowerInvariant();
            return CommonBooleanTrueValues.Contains(normalized);
        }

        public static bool IsNumericValue(string value1, string value2)
        {
            return double.TryParse(value1, out _) && double.TryParse(value2, out _);
        }

        public static bool AreXmlDocumentsLogicallyEquivalent(string xml1, string xml2, XmlComparisonOptions? options = null)
        {
            options ??= new XmlComparisonOptions();

            try
            {
                var doc1 = XDocument.Parse(xml1);
                var doc2 = XDocument.Parse(xml2);

                if (options.IgnoreComments)
                {
                    RemoveComments(doc1);
                    RemoveComments(doc2);
                }

                return AreXElementsLogicallyEquivalent(doc1.Root, doc2.Root, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML解析错误: {ex.Message}");
                return false;
            }
        }

        private static bool AreXElementsLogicallyEquivalent(XElement? elem1, XElement? elem2, XmlComparisonOptions options)
        {
            if (elem1 == null && elem2 == null) return true;
            if (elem1 == null || elem2 == null) return false;

            if (elem1.Name != elem2.Name) return false;

            // 处理文本内容
            var text1 = elem1.Value?.Trim();
            var text2 = elem2.Value?.Trim();
            if (text1 != text2)
            {
                if (!string.IsNullOrEmpty(text1) && !string.IsNullOrEmpty(text2))
                {
                    // 尝试数值比较
                    if (options.AllowNumericTolerance && IsNumericValue(text1, text2))
                    {
                        return AreNumericValuesEqual(text1, text2, options.NumericTolerance);
                    }
                    // 尝试布尔比较
                    if (options.AllowCaseInsensitiveBooleans && IsBooleanValue(text1) && IsBooleanValue(text2))
                    {
                        return AreBooleanValuesEqual(text1, text2);
                    }
                }
                return false;
            }

            // 处理属性
            var attrs1 = elem1.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
            var attrs2 = elem2.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);

            if (attrs1.Count != attrs2.Count) return false;

            foreach (var attr in attrs1)
            {
                if (!attrs2.TryGetValue(attr.Key, out var value2))
                    return false;

                // 数值比较
                if (options.AllowNumericTolerance && IsNumericValue(attr.Value, value2))
                {
                    if (!AreNumericValuesEqual(attr.Value, value2, options.NumericTolerance))
                        return false;
                }
                // 布尔比较
                else if (options.AllowCaseInsensitiveBooleans && IsBooleanValue(attr.Value) && IsBooleanValue(value2))
                {
                    if (!AreBooleanValuesEqual(attr.Value, value2))
                        return false;
                }
                else if (attr.Value != value2)
                {
                    return false;
                }
            }

            // 处理子元素
            var children1 = elem1.Elements().ToList();
            var children2 = elem2.Elements().ToList();

            if (children1.Count != children2.Count) return false;

            for (int i = 0; i < children1.Count; i++)
            {
                if (!AreXElementsLogicallyEquivalent(children1[i], children2[i], options))
                    return false;
            }

            return true;
        }

        public static void AssertXmlRoundTrip(string originalXml, string serializedXml, XmlComparisonOptions? options = null)
        {
            options ??= new XmlComparisonOptions();

            // 使用结构比较作为唯一判断标准
            var report = CompareXmlStructure(originalXml, serializedXml);
            
            if (!report.IsStructurallyEqual)
            {
                var debugPath = Path.Combine("Debug", $"xml_comparison_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(debugPath);
                
                File.WriteAllText(Path.Combine(debugPath, "original.xml"), originalXml);
                File.WriteAllText(Path.Combine(debugPath, "serialized.xml"), serializedXml);
                
                var diffReport = $"结构差异报告:\n" +
                                $"IsStructurallyEqual: {report.IsStructurallyEqual}\n" +
                                $"MissingNodes: {string.Join(", ", report.MissingNodes)}\n" +
                                $"ExtraNodes: {string.Join(", ", report.ExtraNodes)}\n" +
                                $"NodeNameDifferences: {string.Join(", ", report.NodeNameDifferences)}\n" +
                                $"MissingAttributes: {string.Join(", ", report.MissingAttributes)}\n" +
                                $"ExtraAttributes: {string.Join(", ", report.ExtraAttributes)}\n" +
                                $"AttributeValueDifferences: {string.Join(", ", report.AttributeValueDifferences)}\n" +
                                $"TextDifferences: {string.Join(", ", report.TextDifferences)}\n" +
                                $"NodeCountDifference: {report.NodeCountDifference}\n" +
                                $"AttributeCountDifference: {report.AttributeCountDifference}\n" +
                                $"保存路径: {debugPath}";
                File.WriteAllText(Path.Combine(debugPath, "diff_report.txt"), diffReport);
                
                Assert.Fail(diffReport);
            }
        }

        public static bool AreNumericValuesEqual(string value1, string value2, double tolerance)
        {
            if (double.TryParse(value1, out var d1) && double.TryParse(value2, out var d2))
            {
                return Math.Abs(d1 - d2) < tolerance;
            }
            if (int.TryParse(value1, out var i1) && int.TryParse(value2, out var i2))
            {
                return i1 == i2;
            }
            return value1 == value2;
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
        
        // 详细属性统计
        public static void DetailedAttributeCount(string xml, string tag)
        {
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            int nodeCount = 0, attrCount = 0;
            var attrDetails = new Dictionary<string, int>();
            
            foreach (var node in doc.Descendants())
            {
                nodeCount++;
                var nodeAttrCount = node.Attributes().Count();
                attrCount += nodeAttrCount;
                
                if (nodeAttrCount > 0)
                {
                    var key = $"{node.Name.LocalName}({nodeAttrCount})";
                    if (attrDetails.ContainsKey(key))
                        attrDetails[key]++;
                    else
                        attrDetails[key] = 1;
                }
            }
            
            Console.WriteLine($"=== {tag} 详细统计 ===");
            Console.WriteLine($"节点总数: {nodeCount}");
            Console.WriteLine($"属性总数: {attrCount}");
            Console.WriteLine("各节点属性分布:");
            foreach (var kvp in attrDetails.OrderBy(x => x.Key))
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value} 个节点");
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

        private static void SortAttributes(XElement? element)
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

        private static void NormalizeSelfClosingTags(XNode? node)
        {
            if (node is XElement element)
            {
                // 如果元素没有子元素且为空，将其转换为自闭合标签格式
                if (!element.HasElements && string.IsNullOrEmpty(element?.Value) && !element.IsEmpty)
                {
                    element.RemoveAll();
                }

                // 递归处理子元素
                foreach (var child in element.Elements())
                {
                    NormalizeSelfClosingTags(child);
                }
            }
        }

        private static void RemoveComments(XNode? node)
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

        private static XDocument RemoveNamespaceDeclarations(XDocument doc)
        {
            if (doc == null) return new XDocument();
            
            var clone = new XDocument(doc);
            
            // 移除所有命名空间声明
            foreach (var element in clone.Descendants())
            {
                var namespaceAttrs = element.Attributes()
                    .Where(a => a.IsNamespaceDeclaration).ToList();
                foreach (var attr in namespaceAttrs)
                {
                    attr.Remove();
                }
            }
            
            // 也移除根元素的xmlns属性
            if (clone.Root != null)
            {
                var xmlnsAttrs = clone.Root.Attributes()
                    .Where(a => a.Name.LocalName == "xmlns" || a.Name.LocalName.StartsWith("xmlns:")).ToList();
                foreach (var attr in xmlnsAttrs)
                {
                    attr.Remove();
                }
            }
            
            return clone;
        }

        
        // 重新排序Category内部的元素以匹配原始XML顺序
        private static void ReorderCategoryElements(CreditsCategoryDO category, List<XElement> sectionElements, List<XElement> entryElements, List<XElement> emptyLineElements, List<XElement> loadFromFileElements, List<XElement> imageElements)
        {
            // 创建新的列表来存储重新排序的元素
            var reorderedSections = new List<CreditsSectionDO>();
            var reorderedEntries = new List<CreditsEntryDO>();
            var reorderedEmptyLines = new List<CreditsEmptyLineDO>();
            var reorderedLoadFromFile = new List<CreditsLoadFromFileDO>();
            var reorderedImages = new List<CreditsImageDO>();
            
            // 根据原始XML中的出现顺序重新排序元素
            int sectionIndex = 0, entryIndex = 0, emptyLineIndex = 0, loadFromFileIndex = 0, imageIndex = 0;
            
            // 遍历原始Category的所有子元素
            var allChildElements = sectionElements
                .Concat(entryElements)
                .Concat(emptyLineElements)
                .Concat(loadFromFileElements)
                .Concat(imageElements)
                .OrderBy(e => e.NodesBeforeSelf().Count())
                .ToList();
            
            foreach (var element in allChildElements)
            {
                switch (element.Name.LocalName)
                {
                    case "Section" when sectionIndex < category.Sections.Count:
                        reorderedSections.Add(category.Sections[sectionIndex++]);
                        break;
                    case "Entry" when entryIndex < category.Entries.Count:
                        reorderedEntries.Add(category.Entries[entryIndex++]);
                        break;
                    case "EmptyLine" when emptyLineIndex < category.EmptyLines.Count:
                        reorderedEmptyLines.Add(category.EmptyLines[emptyLineIndex++]);
                        break;
                    case "LoadFromFile" when loadFromFileIndex < category.LoadFromFile.Count:
                        reorderedLoadFromFile.Add(category.LoadFromFile[loadFromFileIndex++]);
                        break;
                    case "Image" when imageIndex < category.Images.Count:
                        reorderedImages.Add(category.Images[imageIndex++]);
                        break;
                }
            }
            
            // 更新Category的列表
            category.Sections = reorderedSections;
            category.Entries = reorderedEntries;
            category.EmptyLines = reorderedEmptyLines;
            category.LoadFromFile = reorderedLoadFromFile;
            category.Images = reorderedImages;
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}