using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Tests
{
    public static class XmlTestUtils
    {
        public static T Deserialize<T>(string xml) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(xml);
            return (T)serializer.Deserialize(reader);
        }

        public static string Serialize<T>(T obj) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = new UTF8Encoding(false)
            };

            using var writer = XmlWriter.Create(sb, settings);
            
            var ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");

            serializer.Serialize(writer, obj, ns);
            return sb.ToString();
        }
        
        public static bool AreStructurallyEqual(string xml1, string xml2)
        {
            try
            {
                var settings = new XmlReaderSettings { IgnoreWhitespace = true };

                using var sr1 = new StringReader(xml1);
                using var xr1 = XmlReader.Create(sr1, settings);
                var doc1 = XDocument.Load(xr1);

                using var sr2 = new StringReader(xml2);
                using var xr2 = XmlReader.Create(sr2, settings);
                var doc2 = XDocument.Load(xr2);
                
                return XNode.DeepEquals(doc1, doc2);
            }
            catch (XmlException)
            {
                return false;
            }
        }
    }
} 