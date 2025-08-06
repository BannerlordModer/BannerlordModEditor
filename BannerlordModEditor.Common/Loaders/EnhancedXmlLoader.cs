using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using BannerlordModEditor.Common.Models;

namespace BannerlordModEditor.Common.Loaders
{
    /// <summary>
    /// 增强的XML加载器，能够正确区分字段为空和字段不存在的情况
    /// </summary>
    public class EnhancedXmlLoader<T> where T : class
    {
        private readonly XmlSerializer _serializer;

        public EnhancedXmlLoader()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        /// <summary>
        /// 加载XML文件并跟踪字段存在性
        /// </summary>
        public T? Load(string filePath)
        {
            // 首先读取XML文档以分析哪些字段存在
            var xdoc = XDocument.Load(filePath);
            var existingProperties = AnalyzeExistingProperties(xdoc);

            // 使用标准序列化器加载对象
            T? result;
            using (var reader = new FileStream(filePath, FileMode.Open))
            {
                result = _serializer.Deserialize(reader) as T;
            }

            // 如果结果是XmlModelBase的实例，标记存在的属性
            if (result is XmlModelBase modelBase)
            {
                foreach (var prop in existingProperties)
                {
                    // 简化实现：对于复杂嵌套结构，我们需要更复杂的逻辑
                    // 这里我们只标记顶层属性
                    var propName = prop.Split('.').Last().Split('@').Last();
                    modelBase.MarkPropertyExists(propName);
                }
            }

            return result;
        }

        /// <summary>
        /// 异步加载XML文件
        /// </summary>
        public async Task<T?> LoadAsync(string filePath)
        {
            return await Task.Run(() => Load(filePath));
        }

        /// <summary>
        /// 保存对象到XML文件，正确处理字段存在性
        /// </summary>
        public void Save(T data, string filePath)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
                Encoding = new UTF8Encoding(false)
            };

            using var writer = XmlWriter.Create(filePath, settings);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            _serializer.Serialize(writer, data, ns);
        }

        /// <summary>
        /// 异步保存对象到XML文件
        /// </summary>
        public async Task SaveAsync(T data, string filePath)
        {
            await Task.Run(() => Save(data, filePath));
        }

        /// <summary>
        /// 分析XML文档中存在的属性
        /// </summary>
        private HashSet<string> AnalyzeExistingProperties(XDocument xdoc)
        {
            var existingProperties = new HashSet<string>();
            
            if (xdoc.Root == null)
                return existingProperties;

            // 分析所有元素和属性
            AnalyzeElement(xdoc.Root, existingProperties);

            return existingProperties;
        }

        /// <summary>
        /// 递归分析元素及其子元素
        /// </summary>
        private void AnalyzeElement(XElement element, HashSet<string> existingProperties, string parentPath = "")
        {
            // 添加元素名称
            var elementPath = string.IsNullOrEmpty(parentPath) ? element.Name.LocalName : $"{parentPath}.{element.Name.LocalName}";
            existingProperties.Add(elementPath);

            // 添加所有属性
            foreach (var attr in element.Attributes())
            {
                var attrPath = $"{elementPath}@{attr.Name.LocalName}";
                existingProperties.Add(attrPath);
            }

            // 递归处理子元素
            foreach (var child in element.Elements())
            {
                AnalyzeElement(child, existingProperties, elementPath);
            }
        }

        /// <summary>
        /// 验证两个XML文档的逻辑等价性
        /// </summary>
        public static bool AreXmlDocumentsLogicallyEquivalent(XDocument doc1, XDocument doc2)
        {
            if (doc1.Root == null && doc2.Root == null) return true;
            if (doc1.Root == null || doc2.Root == null) return false;

            return AreXmlElementsLogicallyEquivalent(doc1.Root, doc2.Root);
        }

        /// <summary>
        /// 验证两个XML元素的逻辑等价性（复用MpItemsSubsetTests.cs中的逻辑）
        /// </summary>
        private static bool AreXmlElementsLogicallyEquivalent(XElement? original, XElement? generated)
        {
            if (original == null && generated == null) return true;
            if (original == null || generated == null) return false;

            // 比较元素名称
            if (original.Name != generated.Name) return false;

            // 比较属性（忽略顺序）
            var originalAttrs = original.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
            var generatedAttrs = generated.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);

            if (originalAttrs.Count != generatedAttrs.Count) return false;

            foreach (var attr in originalAttrs)
            {
                if (!generatedAttrs.TryGetValue(attr.Key, out var generatedValue))
                    return false;

                // 对于数值类型，进行宽松比较（例如 1.0 == 1）
                if (IsNumericValue(attr.Value, generatedValue))
                {
                    if (!AreNumericValuesEqual(attr.Value, generatedValue))
                        return false;
                }
                else if (attr.Value != generatedValue)
                {
                    return false;
                }
            }

            // 比较子元素（保持顺序，因为这对于列表类很重要）
            var originalChildren = original.Elements().ToList();
            var generatedChildren = generated.Elements().ToList();

            if (originalChildren.Count != generatedChildren.Count) return false;

            for (int i = 0; i < originalChildren.Count; i++)
            {
                if (!AreXmlElementsLogicallyEquivalent(originalChildren[i], generatedChildren[i]))
                    return false;
            }

            // 比较文本内容
            return original.Value == generated.Value;
        }

        private static bool IsNumericValue(string value1, string value2)
        {
            return (double.TryParse(value1, out _) && double.TryParse(value2, out _)) ||
                   (int.TryParse(value1, out _) && int.TryParse(value2, out _));
        }

        private static bool AreNumericValuesEqual(string value1, string value2)
        {
            if (double.TryParse(value1, out var d1) && double.TryParse(value2, out var d2))
            {
                return Math.Abs(d1 - d2) < 0.0001; // 允许小的浮点误差
            }
            return value1 == value2;
        }
    }
}