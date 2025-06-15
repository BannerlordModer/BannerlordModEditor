using BannerlordModEditor.Common.Loaders;
using BannerlordModEditor.Common.Models;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SkillsXmlTests
    {
        private readonly string _testFilePath;
        private readonly string _outputFilePath;

        public SkillsXmlTests()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            _testFilePath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skills.xml");
            _outputFilePath = Path.Combine(Path.GetTempPath(), "skills.output.xml");
        }

        [Fact]
        public void SkillsXml_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var loader = new GenericXmlLoader<SkillsWrapper>();
            
            // Act
            var skills = loader.Load(_testFilePath);
            Assert.NotNull(skills);
            loader.Save(_outputFilePath, skills);

            // Assert
            var originalDoc = XDocument.Load(_testFilePath, LoadOptions.None);
            var savedDoc = XDocument.Load(_outputFilePath, LoadOptions.None);

            // Remove artifacts that the serializer doesn't (and shouldn't) produce
            originalDoc.DescendantNodes().OfType<XComment>().Remove();
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            originalDoc.Root?.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name == xsi + "noNamespaceSchemaLocation").Remove();

            Assert.True(XNode.DeepEquals(originalDoc.Root, savedDoc.Root), 
                $"Files are not logically equivalent. Check the output at '{_outputFilePath}'");
        }
    }
} 