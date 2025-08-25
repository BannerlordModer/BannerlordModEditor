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
                languageBase.HasEmptyTags = doc.Root?.Element("tags") != null && 
                    (doc.Root.Element("tags")?.Elements().Count() == 0 || doc.Root.Element("tags")?.Elements("tag").Count() == 0);
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
            return reader.ReadToEnd();
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
            
            var doc = XDocument.Parse(xml);
            
            if (options.IgnoreComments)
            {
                doc.Descendants().OfType<XComment>().Remove();
            }
            
            if (options.IgnoreWhitespace)
            {
                foreach (var element in doc.Descendants())
                {
                    if (element.IsEmpty) continue;
                    if (string.IsNullOrWhiteSpace(element.Value))
                    {
                        element.Value = "";
                    }
                }
            }
            
            // 对所有元素的属性进行排序以确保一致的输出
            foreach (var element in doc.Descendants())
            {
                var sortedAttributes = element.Attributes()
                    .OrderBy(a => a.IsNamespaceDeclaration ? 0 : 1)
                    .ThenBy(a => a.Name.NamespaceName)
                    .ThenBy(a => a.Name.LocalName)
                    .ToList();
                
                // 移除所有属性然后按顺序重新添加
                element.RemoveAttributes();
                foreach (var attr in sortedAttributes)
                {
                    element.Add(attr);
                }
            }
            
            // 特殊处理：将自闭合标签转换为开始/结束标签格式以保持一致性
            foreach (var element in doc.Descendants().Where(e => e.IsEmpty && e.Name.LocalName == "base"))
            {
                element.Add(""); // 添加空内容强制使用开始/结束标签
            }
            
            return doc.ToString();
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