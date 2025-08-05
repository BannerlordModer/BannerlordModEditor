using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
            ns.Add("", "");

            serializer.Serialize(writer, data, ns);
        }

        public async Task SaveAsync(T data, string filePath)
        {
            await Task.Run(() => Save(data, filePath));
        }
    }
} 