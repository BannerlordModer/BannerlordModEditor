using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using BannerlordModEditor.Common.Services;

namespace BannerlordModEditor.Common.Tests.Services
{
    public class XsdSchemaProviderTests
    {
        private readonly XsdSchemaProvider _provider;

        public XsdSchemaProviderTests()
        {
            _provider = new XsdSchemaProvider();
        }

        [Fact]
        public void GetXsdPathForXml_ReturnsNullForNonExistentGameDirectory()
        {
            var result = _provider.GetXsdPathForXml("test.xml", "/nonexistent/path");
            Assert.Null(result);
        }

        [Fact]
        public void GetXsdPathForXml_ReturnsNullForEmptyPaths()
        {
            Assert.Null(_provider.GetXsdPathForXml("", ""));
            Assert.Null(_provider.GetXsdPathForXml(null!, null!));
        }

        [Fact]
        public void GetXsdPathForXml_FindsXsdInXmlSchemasDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var xsdDir = Path.Combine(tempDir, "XmlSchemas");
            Directory.CreateDirectory(xsdDir);

            try
            {
                var xsdPath = Path.Combine(xsdDir, "attributes.xsd");
                File.WriteAllText(xsdPath, "<xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"></xs:schema>");

                var result = _provider.GetXsdPathForXml("attributes.xml", tempDir);
                Assert.Equal(xsdPath, result);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void GetAllXsdFiles_ReturnsEmptyForNonExistentDirectory()
        {
            var result = _provider.GetAllXsdFiles("/nonexistent/path");
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllXsdFiles_FindsAllXsdFiles()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var xsdDir = Path.Combine(tempDir, "XmlSchemas");
            Directory.CreateDirectory(xsdDir);

            try
            {
                File.WriteAllText(Path.Combine(xsdDir, "file1.xsd"), "");
                File.WriteAllText(Path.Combine(xsdDir, "file2.xsd"), "");

                var result = _provider.GetAllXsdFiles(tempDir);
                Assert.Equal(2, result.Count);
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void LoadSchemaSet_ReturnsEmptyForNonExistentFile()
        {
            var result = _provider.LoadSchemaSet("/nonexistent/schema.xsd");
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void LoadSchemaSet_LoadsValidXsd()
        {
            var tempFile = Path.GetTempFileName();
            var xsd = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Test"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Name"" type=""xs:string""/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";

            try
            {
                File.WriteAllText(tempFile, xsd);

                var result = _provider.LoadSchemaSet(tempFile);
                Assert.NotNull(result);
                Assert.Equal(1, result.Count);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ValidateXmlAgainstXsd_ReturnsFalseForNonExistentXml()
        {
            var result = _provider.ValidateXmlAgainstXsd("/nonexistent.xml", "/schema.xsd", out var errors);
            Assert.False(result);
            Assert.Contains("XML file not found", errors[0]);
        }

        [Fact]
        public void ValidateXmlAgainstXsd_ReturnsFalseForNonExistentXsd()
        {
            var tempXml = Path.GetTempFileName();
            File.WriteAllText(tempXml, "<Test></Test>");

            try
            {
                var result = _provider.ValidateXmlAgainstXsd(tempXml, "/nonexistent.xsd", out var errors);
                Assert.False(result);
                Assert.Contains("XSD schema not found", errors[0]);
            }
            finally
            {
                File.Delete(tempXml);
            }
        }
    }
}
