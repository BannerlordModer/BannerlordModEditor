using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace BannerlordModEditor.Common.Services
{
    public interface IXsdSchemaProvider
    {
        string? GetXsdPathForXml(string xmlFilePath, string gameDirectory);
        List<string> GetAllXsdFiles(string gameDirectory);
        XmlSchemaSet LoadSchemaSet(string xsdPath);
        bool ValidateXmlAgainstXsd(string xmlFilePath, string xsdPath, out List<string> errors);
    }

    public class XsdSchemaProvider : IXsdSchemaProvider
    {
        private const string XsdDirectoryRelativePath = "XmlSchemas";
        private const string ModuleDataRelativePath = "Modules";
        private const string NativeModuleRelativePath = "Modules\\Native\\ModuleData";

        public string? GetXsdPathForXml(string xmlFilePath, string gameDirectory)
        {
            if (string.IsNullOrEmpty(xmlFilePath) || string.IsNullOrEmpty(gameDirectory))
                return null;

            var xmlFileName = Path.GetFileNameWithoutExtension(xmlFilePath);
            
            var xsdPaths = new[]
            {
                Path.Combine(gameDirectory, XsdDirectoryRelativePath, $"{xmlFileName}.xsd"),
                Path.Combine(gameDirectory, XsdDirectoryRelativePath, "Native", $"{xmlFileName}.xsd"),
                Path.Combine(gameDirectory, NativeModuleRelativePath, $"{xmlFileName}.xsd")
            };

            foreach (var xsdPath in xsdPaths)
            {
                if (File.Exists(xsdPath))
                    return xsdPath;
            }

            return null;
        }

        public List<string> GetAllXsdFiles(string gameDirectory)
        {
            var xsdFiles = new List<string>();

            if (string.IsNullOrEmpty(gameDirectory) || !Directory.Exists(gameDirectory))
                return xsdFiles;

            var searchPaths = new[]
            {
                Path.Combine(gameDirectory, XsdDirectoryRelativePath),
                Path.Combine(gameDirectory, NativeModuleRelativePath)
            };

            foreach (var searchPath in searchPaths)
            {
                if (Directory.Exists(searchPath))
                {
                    try
                    {
                        xsdFiles.AddRange(Directory.GetFiles(searchPath, "*.xsd", SearchOption.AllDirectories));
                    }
                    catch
                    {
                    }
                }
            }

            return xsdFiles.Distinct().ToList();
        }

        public XmlSchemaSet LoadSchemaSet(string xsdPath)
        {
            var schemaSet = new XmlSchemaSet();

            if (!File.Exists(xsdPath))
                return schemaSet;

            try
            {
                using var stream = new FileStream(xsdPath, FileMode.Open, FileAccess.Read);
                var schema = XmlSchema.Read(stream, null);
                if (schema != null)
                {
                    schemaSet.Add(schema);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load XSD schema from {xsdPath}: {ex.Message}", ex);
            }

            return schemaSet;
        }

        public bool ValidateXmlAgainstXsd(string xmlFilePath, string xsdPath, out List<string> errors)
        {
            errors = new List<string>();

            if (!File.Exists(xmlFilePath))
            {
                errors.Add($"XML file not found: {xmlFilePath}");
                return false;
            }

            if (!File.Exists(xsdPath))
            {
                errors.Add($"XSD schema not found: {xsdPath}");
                return false;
            }

            try
            {
                var schemaSet = LoadSchemaSet(xsdPath);
                var validationErrors = new List<string>();
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemaSet
                };

                settings.ValidationEventHandler += (sender, e) =>
                {
                    var severity = e.Severity == XmlSeverityType.Error ? "Error" : "Warning";
                    validationErrors.Add($"{severity}: {e.Message}");
                };

                using var reader = XmlReader.Create(xmlFilePath, settings);
                while (reader.Read())
                {
                }

                errors = validationErrors;
                return errors.Count == 0;
            }
            catch (Exception ex)
            {
                errors.Add($"Validation error: {ex.Message}");
                return false;
            }
        }
    }
}
