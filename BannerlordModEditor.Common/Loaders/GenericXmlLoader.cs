using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace BannerlordModEditor.Common.Loaders
{
    public class GenericXmlLoader<T> where T : class
    {
        public T? Load(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new FileStream(filePath, FileMode.Open);
            return serializer.Deserialize(reader) as T;
        }

        public async Task<T?> LoadAsync(string filePath)
        {
            return await Task.Run(() => Load(filePath));
        }

        public void Save(T data, string filePath)
        {
            Save(data, filePath, null);
        }

        public void Save(T data, string filePath, string? originalXml)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n", // 使用Unix风格换行符以匹配原始XML格式
                Encoding = new System.Text.UTF8Encoding(false)
            };

            using var writer = XmlWriter.Create(filePath, settings);
            
            var ns = new XmlSerializerNamespaces();
            
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
                            // 检查是否为命名空间声明属性
                            if (attr.IsNamespaceDeclaration)
                            {
                                ns.Add(attr.Name.LocalName, attr.Value);
                            }
                        }
                    }
                }
                catch
                {
                    // 如果解析失败，回退到默认行为
                    ns.Add("", "");
                }
            }
            else
            {
                // 清空默认命名空间以避免添加额外的命名空间
                ns.Add("", "");
            }

            serializer.Serialize(writer, data, ns);
        }

        public async Task SaveAsync(T data, string filePath)
        {
            await Task.Run(() => Save(data, filePath, null));
        }

        public async Task SaveAsync(T data, string filePath, string? originalXml)
        {
            await Task.Run(() => Save(data, filePath, originalXml));
        }

        public string SaveToString(T data, string? originalXml = null)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
                Encoding = new System.Text.UTF8Encoding(false)
            };

            using var writer = new StringWriter();
            using var xmlWriter = XmlWriter.Create(writer, settings);
            
            var ns = new XmlSerializerNamespaces();
            
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
                    ns.Add("", "");
                }
            }
            else
            {
                ns.Add("", "");
            }

            serializer.Serialize(xmlWriter, data, ns);
            return writer.ToString();
        }

        public async Task<T?> LoadFromXmlStringAsync(string xml)
        {
            return await Task.Run(() => LoadFromXmlString(xml));
        }

        public T? LoadFromXmlString(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException("XML cannot be null or empty", nameof(xml));
                
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            return serializer.Deserialize(reader) as T;
        }
    }
} 