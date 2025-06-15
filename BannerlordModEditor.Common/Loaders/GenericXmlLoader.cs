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

        public void Save(string filePath, T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                Encoding = new System.Text.UTF8Encoding(false)
            };

            using var writer = XmlWriter.Create(filePath, settings);
            
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            serializer.Serialize(writer, data, ns);
        }
    }
} 