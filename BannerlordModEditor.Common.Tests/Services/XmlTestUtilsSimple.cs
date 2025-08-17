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
                IndentChars = "\t",
                Encoding = Encoding.UTF8
            };
            using var sw = new Utf8StringWriter();
            using var writer = XmlWriter.Create(sw, settings);
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(writer, obj, ns);
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

        // 重载版本：忽略命名空间和格式差异
        public static bool AreStructurallyEqualIgnoreNamespaces(string xmlA, string xmlB)
        {
            var docA = new XmlDocument();
            docA.LoadXml(xmlA);
            var docB = new XmlDocument();
            docB.LoadXml(xmlB);
            return XmlNodesEqualIgnoreNamespaces(docA.DocumentElement, docB.DocumentElement);
        }

        private static bool XmlNodesEqual(XmlNode a, XmlNode b)
        {
            if (a == null || b == null) return a == b;
            if (a.Name != b.Name) return false;
            if (a.Attributes?.Count != b.Attributes?.Count) return false;
            if (a.Attributes != null)
            {
                // 检查所有属性是否存在且值相等，不关心顺序
                foreach (XmlAttribute attrA in a.Attributes)
                {
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

        private static bool XmlNodesEqualIgnoreNamespaces(XmlNode a, XmlNode b)
        {
            if (a == null || b == null) return a == b;
            
            // 忽略命名空间前缀，只比较本地名称
            var aLocalName = a.LocalName;
            var bLocalName = b.LocalName;
            if (aLocalName != bLocalName) return false;
            
            // 过滤掉命名空间声明属性
            var aFilteredAttrs = FilterNamespaceAttributes(a.Attributes);
            var bFilteredAttrs = FilterNamespaceAttributes(b.Attributes);
            
            if (aFilteredAttrs.Count != bFilteredAttrs.Count) return false;
            
            // 检查所有属性是否存在且值相等，不关心顺序
            foreach (XmlAttribute attrA in aFilteredAttrs)
            {
                var attrB = bFilteredAttrs[attrA.Name];
                if (attrB == null || attrA.Value != attrB.Value)
                    return false;
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
                    if (!XmlNodesEqualIgnoreNamespaces(aChildren[ai], bChildren[bi]))
                        return false;
                    ai++; bi++;
                }
            }
            return true;
        }

        private static XmlAttributeCollection FilterNamespaceAttributes(XmlAttributeCollection attributes)
        {
            if (attributes == null) return null;
            
            // 创建一个新的 XmlDocument 来存放过滤后的属性
            var tempDoc = new XmlDocument();
            var filteredElement = tempDoc.CreateElement("temp");
            
            foreach (XmlAttribute attr in attributes)
            {
                // 跳过命名空间声明属性 (xmlns:*)
                if (!attr.Name.StartsWith("xmlns:") && attr.Name != "xmlns")
                {
                    var newAttr = tempDoc.CreateAttribute(attr.Name);
                    newAttr.Value = attr.Value;
                    filteredElement.Attributes.Append(newAttr);
                }
            }
            
            return filteredElement.Attributes;
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}