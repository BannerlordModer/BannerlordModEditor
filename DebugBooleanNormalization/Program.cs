using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;

class Program
{
    static void Main()
    {
        // Read the test XML file directly
        var xmlPath = "../BannerlordModEditor.Common.Tests/TestData/taunt_usage_sets.xml";
        if (File.Exists(xmlPath))
        {
            var originalXml = File.ReadAllText(xmlPath);
            Console.WriteLine("=== Original XML (first 500 chars) ===");
            Console.WriteLine(originalXml.Substring(0, Math.Min(500, originalXml.Length)) + "...");
            
            try
            {
                // Deserialize
                var serializer = new XmlSerializer(typeof(TauntUsageSets));
                using var stringReader = new StringReader(originalXml);
                var model = (TauntUsageSets)serializer.Deserialize(stringReader);
                
                Console.WriteLine("\n=== Deserialized Successfully ===");
                
                // Serialize back
                using var stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, model);
                var serializedXml = stringWriter.ToString();
                
                Console.WriteLine("\n=== Serialized XML (first 500 chars) ===");
                Console.WriteLine(serializedXml.Substring(0, Math.Min(500, serializedXml.Length)) + "...");
                
                // Compare structure
                var areEqual = AreStructurallyEqual(originalXml, serializedXml);
                Console.WriteLine($"\n=== Structural Equality Result: {areEqual} ===");
                
                if (!areEqual)
                {
                    Console.WriteLine("\n=== Detailed Comparison ===");
                    CompareXmlDocuments(originalXml, serializedXml);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
        else
        {
            Console.WriteLine("File not found: " + xmlPath);
        }
    }
    
    static bool AreStructurallyEqual(string xmlA, string xmlB)
    {
        try
        {
            var docA = XDocument.Parse(xmlA);
            var docB = XDocument.Parse(xmlB);
            
            NormalizeBooleanValues(docA);
            NormalizeBooleanValues(docB);
            
            return XNode.DeepEquals(docA, docB);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AreStructurallyEqual: {ex.Message}");
            return false;
        }
    }
    
    static void NormalizeBooleanValues(XDocument doc)
    {
        foreach (var element in doc.Descendants())
        {
            foreach (var attr in element.Attributes().ToList())
            {
                var value = attr.Value;
                if (string.Equals(value, "True", StringComparison.OrdinalIgnoreCase))
                {
                    attr.Value = "true";
                }
                else if (string.Equals(value, "False", StringComparison.OrdinalIgnoreCase))
                {
                    attr.Value = "false";
                }
            }
        }
    }
    
    static void CompareXmlDocuments(string xmlA, string xmlB)
    {
        try
        {
            var docA = XDocument.Parse(xmlA);
            var docB = XDocument.Parse(xmlB);
            
            Console.WriteLine($"Document A root: {docA.Root?.Name}");
            Console.WriteLine($"Document B root: {docB.Root?.Name}");
            
            if (docA.Root != null && docB.Root != null)
            {
                CompareElements(docA.Root, docB.Root, "");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CompareXmlDocuments: {ex.Message}");
        }
    }
    
    static void CompareElements(XElement elemA, XElement elemB, string path)
    {
        if (elemA.Name != elemB.Name)
        {
            Console.WriteLine($"Element name mismatch at {path}: {elemA.Name} vs {elemB.Name}");
            return;
        }
        
        // Compare attributes
        var attrsA = elemA.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
        var attrsB = elemB.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
        
        foreach (var attr in attrsA)
        {
            if (!attrsB.ContainsKey(attr.Key))
            {
                Console.WriteLine($"Missing attribute in B at {path}/{elemA.Name.LocalName}: {attr.Key}={attr.Value}");
            }
            else if (attrsB[attr.Key] != attr.Value)
            {
                Console.WriteLine($"Attribute value mismatch at {path}/{elemA.Name.LocalName}.{attr.Key}: '{attr.Value}' vs '{attrsB[attr.Key]}'");
            }
        }
        
        foreach (var attr in attrsB)
        {
            if (!attrsA.ContainsKey(attr.Key))
            {
                Console.WriteLine($"Extra attribute in B at {path}/{elemA.Name.LocalName}: {attr.Key}={attr.Value}");
            }
        }
        
        // Compare child elements
        var childrenA = elemA.Elements().ToList();
        var childrenB = elemB.Elements().ToList();
        
        if (childrenA.Count != childrenB.Count)
        {
            Console.WriteLine($"Child count mismatch at {path}/{elemA.Name.LocalName}: {childrenA.Count} vs {childrenB.Count}");
        }
        else
        {
            for (int i = 0; i < childrenA.Count; i++)
            {
                CompareElements(childrenA[i], childrenB[i], $"{path}/{elemA.Name.LocalName}");
            }
        }
    }
}