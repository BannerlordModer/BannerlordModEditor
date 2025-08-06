using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BannerlordModEditor.Common.Tests.Services
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
            var docA = new XmlDocument();
            docA.LoadXml(xmlA);
            var docB = new XmlDocument();
            docB.LoadXml(xmlB);
            return XmlNodesEqual(docA.DocumentElement, docB.DocumentElement);
        }

        private static bool XmlNodesEqual(XmlNode a, XmlNode b)
        {
            if (a == null || b == null) return a == b;
            if (a.Name != b.Name) return false;
            if (a.Attributes?.Count != b.Attributes?.Count) return false;
            if (a.Attributes != null)
            {
                for (int i = 0; i < a.Attributes.Count; i++)
                {
                    var attrA = a.Attributes[i];
                    var attrB = b.Attributes[attrA.Name];
                    if (attrB == null || attrA.Value != attrB.Value)
                        return false;
                }
            }
            var aChildren = a.ChildNodes;
            var bChildren = b.ChildNodes;
            int aElementCount = 0, bElementCount = 0;
            for (int i = 0; i < aChildren.Count; i++)
                if (aChildren[i].NodeType == XmlNodeType.Element) aElementCount++;
            for (int i = 0; i < bChildren.Count; i++)
                if (bChildren[i].NodeType == XmlNodeType.Element) bElementCount++;
            if (aElementCount != bElementCount) return false;
            int ai = 0, bi = 0;
            while (ai < aChildren.Count && bi < bChildren.Count)
            {
                while (ai < aChildren.Count && aChildren[ai].NodeType != XmlNodeType.Element) ai++;
                while (bi < bChildren.Count && bChildren[bi].NodeType != XmlNodeType.Element) bi++;
                if (ai < aChildren.Count && bi < bChildren.Count)
                {
                    if (!XmlNodesEqual(aChildren[ai], bChildren[bi]))
                        return false;
                    ai++; bi++;
                }
            }
            return true;
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}