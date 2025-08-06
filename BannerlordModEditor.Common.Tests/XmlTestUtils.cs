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
                Encoding = Encoding.UTF8
            };
            using var sw = new Utf8StringWriter();
            using var writer = XmlWriter.Create(sw, settings);
            serializer.Serialize(writer, obj);
            return sw.ToString();
        }

        public static bool AreStructurallyEqual(string xmlA, string xmlB)
        {
            var docA = XDocument.Parse(xmlA);
            var docB = XDocument.Parse(xmlB);
            return XNode.DeepEquals(docA, docB);
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}