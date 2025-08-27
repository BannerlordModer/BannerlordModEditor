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
using BannerlordModEditor.Common.Models.DO.Language;

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
            
            // 特殊处理TerrainMaterialsDO来检测textures、layer_flags和meshes元素
            if (obj is BannerlordModEditor.Common.Models.DO.Engine.TerrainMaterialsDO terrainMaterials)
            {
                var doc = XDocument.Parse(xml);
                foreach (var material in terrainMaterials.TerrainMaterialList)
                {
                    var materialElement = doc.Root?.Elements("terrain_material")
                        .FirstOrDefault(e => e.Attribute("name")?.Value == material.Name);
                    
                    if (materialElement != null)
                    {
                        material.HasTextures = materialElement.Element("textures") != null;
                        material.HasLayerFlags = materialElement.Element("layer_flags") != null;
                        material.HasMeshes = materialElement.Element("meshes") != null;
                        
                        var meshesElement = materialElement.Element("meshes");
                        material.HasEmptyMeshes = meshesElement != null && 
                            (meshesElement.Elements().Count() == 0 || meshesElement.Elements("mesh").Count() == 0);
                    }
                }
            }
            
            // 特殊处理LanguageBaseDO来检测是否有空元素
            if (obj is LanguageBaseDO languageBase)
            {
                var doc = XDocument.Parse(xml);
                var tagsElement = doc.Root?.Element("tags");
                languageBase.HasEmptyTags = tagsElement != null && 
                    (tagsElement.Elements().Count() == 0 || tagsElement.Elements("tag").Count() == 0);
                
                // 同时设置LanguageTagsDO的HasEmptyTags属性
                if (languageBase.Tags != null)
                {
                    languageBase.Tags.HasEmptyTags = languageBase.HasEmptyTags;
                }
            }
            
            // 特殊处理SiegeEnginesDO来检测是否有空的SiegeEngineTypes元素
            if (obj is BannerlordModEditor.Common.Models.DO.SiegeEnginesDO siegeEngines)
            {
                var doc = XDocument.Parse(xml);
                var siegeEnginesElement = doc.Root;
                siegeEngines.HasEmptySiegeEngines = siegeEnginesElement != null && 
                    (siegeEnginesElement.Elements().Count() == 0 || siegeEnginesElement.Elements("SiegeEngineType").Count() == 0);
            }
            
            // 特殊处理WaterPrefabsDO来检测是否有空的WaterPrefabs元素
            if (obj is BannerlordModEditor.Common.Models.DO.WaterPrefabsDO waterPrefabs)
            {
                var doc = XDocument.Parse(xml);
                var waterPrefabsElement = doc.Root;
                waterPrefabs.HasEmptyWaterPrefabs = waterPrefabsElement != null && 
                    (waterPrefabsElement.Elements().Count() == 0 || waterPrefabsElement.Elements("WaterPrefab").Count() == 0);
            }
            
            return obj;
        }

        public static string Serialize<T>(T obj, string? originalXml = null)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
                Encoding = new System.Text.UTF8Encoding(false)
            };

            using var stream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(stream, settings);
            
            var ns = new XmlSerializerNamespaces();
            
            // 默认添加空命名空间以避免自动生成命名空间
            ns.Add("", "");
            
            // 如果提供了原始XML，则提取并保留其命名空间声明
            if (!string.IsNullOrEmpty(originalXml))
            {
                try
                {
                    var doc = XDocument.Parse(originalXml);
                    if (doc.Root != null)
                    {
                        foreach (var attr in doc.Root.Attributes())
                        {
                            if (attr.IsNamespaceDeclaration)
                            {
                                ns.Add(attr.Name.LocalName, attr.Value);
                            }
                        }
                    }
                }
                catch
                {
                    // 如果解析失败，使用默认的空命名空间
                }
            }

            serializer.Serialize(xmlWriter, obj, ns);
            xmlWriter.Flush();
            stream.Position = 0;
            using var reader = new StreamReader(stream, new System.Text.UTF8Encoding(false));
            var result = reader.ReadToEnd();
            
            // 最终清理：移除可能存在的尾随空白和空行
            result = string.Join("\n", result.Split('\n')
                .Select(line => line.TrimEnd())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray());
            
            return result;
        }

        public static bool AreStructurallyEqual<T>(T obj1, T obj2, XmlComparisonOptions? options = null)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;

            var xml1 = Serialize(obj1);
            var xml2 = Serialize(obj2);
            
            return NormalizeXml(xml1, options) == NormalizeXml(xml2, options);
        }

        public static bool AreStructurallyEqual(string xml1, string xml2, XmlComparisonOptions? options = null)
        {
            if (string.IsNullOrEmpty(xml1) && string.IsNullOrEmpty(xml2)) return true;
            if (string.IsNullOrEmpty(xml1) || string.IsNullOrEmpty(xml2)) return false;
            
            return NormalizeXml(xml1, options) == NormalizeXml(xml2, options);
        }

        public static string NormalizeXml(string xml, XmlComparisonOptions? options = null)
        {
            options ??= new XmlComparisonOptions();
            
            // 处理空输入
            if (string.IsNullOrWhiteSpace(xml))
                return xml;
            
            try
            {
                var doc = XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
                
                // 移除XML声明（如果配置忽略）
                if (doc.Declaration != null)
                {
                    // 保存原始声明信息以便后续处理
                    var hasDeclaration = doc.Declaration != null;
                    var declarationStandalone = doc.Declaration.Standalone;
                    var declarationEncoding = doc.Declaration.Encoding;
                    var declarationVersion = doc.Declaration.Version;
                }
                
                // 移除注释（如果配置忽略）
                if (options.IgnoreComments)
                {
                    // 移除文档级别的所有注释
                    doc.Nodes().OfType<XComment>().Remove();
                    // 移除根级别的注释
                    if (doc.Root != null)
                    {
                        doc.Root.Nodes().OfType<XComment>().Remove();
                        // 移除所有后代元素中的注释
                        foreach (var element in doc.Descendants())
                        {
                            element.Nodes().OfType<XComment>().Remove();
                        }
                    }
                }
                
                // 处理空白字符
                if (options.IgnoreWhitespace)
                {
                    // 处理文档级别的空白节点（包括空行）
                    var docWhitespaceNodes = doc.Nodes()
                        .Where(n => n.NodeType == System.Xml.XmlNodeType.Whitespace)
                        .ToList();
                    foreach (var whitespaceNode in docWhitespaceNodes)
                    {
                        whitespaceNode.Remove();
                    }
                    
                    foreach (var element in doc.Descendants())
                    {
                        // 处理元素内容中的空白字符
                        if (!element.HasElements && !element.HasAttributes)
                        {
                            if (string.IsNullOrWhiteSpace(element.Value))
                            {
                                element.Value = "";
                            }
                        }
                        
                        // 处理元素之间的空白节点
                        if (element.Parent != null)
                        {
                            var whitespaceNodes = element.Parent.Nodes()
                                .Where(n => n.NodeType == System.Xml.XmlNodeType.Whitespace)
                                .ToList();
                            foreach (var whitespaceNode in whitespaceNodes)
                            {
                                whitespaceNode.Remove();
                            }
                        }
                    }
                }
                
                // 对属性进行排序
                if (options.IgnoreAttributeOrder)
                {
                    SortAttributes(doc);
                }
                
                // 处理布尔值标准化
                if (options.AllowCaseInsensitiveBooleans)
                {
                    NormalizeBooleanValues(doc);
                }
                
                // 处理自闭合标签
                NormalizeSelfClosingTags(doc);
                
                // 特殊处理base元素，确保始终使用开始/结束标签格式
                foreach (var element in doc.Descendants().Where(e => e.Name.LocalName == "base"))
                {
                    if (element.IsEmpty)
                    {
                        element.Add(""); // 强制添加空内容
                    }
                }
                
                // 生成标准化后的XML字符串
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    NewLineChars = "\n",
                    Encoding = new System.Text.UTF8Encoding(false),
                    OmitXmlDeclaration = true // 移除XML声明以便比较
                };
                
                using var stream = new MemoryStream();
                using var xmlWriter = XmlWriter.Create(stream, settings);
                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
                stream.Position = 0;
                using var reader = new StreamReader(stream, new System.Text.UTF8Encoding(false));
                var result = reader.ReadToEnd();
                
                // 最终清理：移除所有缩进，只保留内容，并确保base元素使用正确的格式
                var lines = result.Split('\n')
                    .Select(line => line.Trim())
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToList();
                
                result = string.Join("", lines.Select((line, index) => 
                {
                    // 对于XML元素，保持基本的层级结构
                    if (line.StartsWith("<") && !line.StartsWith("</"))
                    {
                        return "\n  " + line;
                    }
                    else if (line.StartsWith("</"))
                    {
                        // 确保结束标签有换行
                        return "\n" + line;
                    }
                    else
                    {
                        return "\n    " + line;
                    }
                }).ToArray()) + "\n";
                
                // 特殊处理：确保base元素使用正确的格式（开始标签和结束标签分行）
                result = result.Replace("<base", "\n<base");
                
                // 只处理base元素的自闭合标签，不要影响其他元素
                result = System.Text.RegularExpressions.Regex.Replace(
                    result, 
                    @"<base([^>]*)/>", 
                    match => $"<base{match.Groups[1].Value}></base>"
                );
                
                // 确保空base元素的格式正确 - 强制使用开始标签和结束标签分行格式
                if (result.Contains("<base") && result.Contains("</base>"))
                {
                    // 只处理真正空的base元素（不包含其他子元素的base）
                    result = System.Text.RegularExpressions.Regex.Replace(
                        result, 
                        @"<base[^>]*>\s*</base>", 
                        match => match.Value.Replace("></base>", ">\n</base>")
                    );
                    
                    // 确保最后一个</base>标签有换行
                    result = result.Replace("</functions></base>", "</functions>\n</base>");
                    result = result.Replace("</strings></base>", "</strings>\n</base>");
                    result = result.Replace("</tags></base>", "</tags>\n</base>");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                // 如果解析失败，返回原始字符串
                return xml;
            }
        }
        
        private static void SortAttributes(XDocument doc)
        {
            foreach (var element in doc.Descendants())
            {
                if (!element.HasAttributes)
                    continue;
                
                // 获取所有属性并按特定规则排序
                var sortedAttributes = element.Attributes()
                    .OrderBy(a => 
                    {
                        // 命名空间声明优先
                        if (a.IsNamespaceDeclaration) return 0;
                        
                        // 特殊属性排序规则
                        var name = a.Name.LocalName.ToLower();
                        
                        // id属性优先
                        if (name == "id") return 1;
                        
                        // name属性优先
                        if (name == "name") return 2;
                        
                        // type属性优先
                        if (name == "type") return 3;
                        
                        // 其他属性按字母顺序
                        return 4;
                    })
                    .ThenBy(a => a.Name.LocalName.ToLower())
                    .ToList();
                
                // 移除所有属性然后按顺序重新添加
                element.RemoveAttributes();
                foreach (var attr in sortedAttributes)
                {
                    element.Add(attr);
                }
            }
        }
        
        private static void NormalizeBooleanValues(XDocument doc)
        {
            foreach (var element in doc.Descendants())
            {
                foreach (var attr in element.Attributes())
                {
                    var attrName = attr.Name.LocalName.ToLower();
                    
                    // 检查是否是布尔属性
                    if (attrName.EndsWith("global") || 
                        attrName.StartsWith("is_") ||
                        attrName.Contains("enabled") ||
                        attrName.Contains("visible") ||
                        attrName.Contains("active") ||
                        attrName == "constructible" ||
                        attrName == "ranged" ||
                        attrName == "anti_personnel")
                    {
                        var value = attr.Value.ToLower();
                        if (CommonBooleanTrueValues.Contains(value, StringComparer.OrdinalIgnoreCase))
                        {
                            attr.Value = "true";
                        }
                        else if (CommonBooleanFalseValues.Contains(value, StringComparer.OrdinalIgnoreCase))
                        {
                            attr.Value = "false";
                        }
                    }
                }
            }
        }
        
        private static void NormalizeSelfClosingTags(XDocument doc)
        {
            // 将特定的自闭合标签转换为开始/结束标签格式
            foreach (var element in doc.Descendants().Where(e => e.IsEmpty && e.Name.LocalName == "base"))
            {
                element.Add(""); // 添加空内容强制使用开始/结束标签
            }
        }

        public static string GetRelativePath(string basePath, string targetPath)
        {
            var baseUri = new Uri(basePath.EndsWith(Path.DirectorySeparatorChar) ? basePath : basePath + Path.DirectorySeparatorChar);
            var targetUri = new Uri(targetPath);
            var relativeUri = baseUri.MakeRelativeUri(targetUri);
            return Uri.UnescapeDataString(relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CleanupTestFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                try
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                catch
                {
                    // 忽略删除错误
                }
            }
        }

        public static string GenerateTestXmlContent(string template, Dictionary<string, string> replacements)
        {
            var content = template;
            foreach (var replacement in replacements)
            {
                content = content.Replace($"{{{replacement.Key}}}", replacement.Value);
            }
            return content;
        }

        public static bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static XElement? FindElementByAttributeValue(XElement root, string elementName, string attributeName, string attributeValue)
        {
            return root.Descendants(elementName)
                .FirstOrDefault(e => e.Attribute(attributeName)?.Value == attributeValue);
        }

        public static IEnumerable<XElement> FindElementsByAttributeValue(XElement root, string elementName, string attributeName, string attributeValue)
        {
            return root.Descendants(elementName)
                .Where(e => e.Attribute(attributeName)?.Value == attributeValue);
        }

        public static string GetElementPath(XElement element)
        {
            var path = new List<string>();
            var current = element;
            
            while (current != null)
            {
                var index = current.Parent?.Elements(current.Name).ToList().IndexOf(current) ?? 0;
                var pathSegment = index > 0 ? $"{current.Name.LocalName}[{index}]" : current.Name.LocalName;
                path.Insert(0, pathSegment);
                current = current.Parent;
            }
            
            return string.Join("/", path);
        }

        public static int CountDescendantElements(XElement element, string elementName)
        {
            return element.Descendants(elementName).Count();
        }

        public static double GetXmlComplexityScore(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                var elements = doc.Descendants().Count();
                var attributes = doc.Descendants().Sum(e => e.Attributes().Count());
                var depth = GetMaxDepth(doc.Root);
                
                return elements * 1.0 + attributes * 0.5 + depth * 2.0;
            }
            catch
            {
                return 0;
            }
        }

        private static int GetMaxDepth(XElement? element)
        {
            if (element == null) return 0;
            
            var maxChildDepth = element.Elements().Select(GetMaxDepth).DefaultIfEmpty(0).Max();
            return maxChildDepth + 1;
        }
    }
}