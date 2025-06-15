using System.IO;
using System.Xml.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class GenerateTestSubsets
    {
        [Fact]
        public void SplitMpItemsXmlIntoIndividualFiles()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var sourceFilePath = Path.Combine(solutionRoot, "example", "ModuleData", "mpitems.xml");
            var outputDirectory = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestSubsets", "MpItems");

            // Create output directory if it doesn't exist
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var doc = XDocument.Load(sourceFilePath);
            var items = doc.Root?.Elements("Item");

            if (items is null) return;

            int count = 0;
            foreach (var item in items)
            {
                count++;
                // Use a sanitized ID or an index for the filename
                var id = item.Attribute("id")?.Value ?? $"item_{count}";
                var sanitizedId = string.Join("_", id.Split(Path.GetInvalidFileNameChars()));
                var outputFilePath = Path.Combine(outputDirectory, $"{sanitizedId}.xml");
                
                // Skip if file already exists to preserve existing test files
                if (File.Exists(outputFilePath))
                {
                    continue;
                }
                
                // Save the single <Item> element to its own file
                item.Save(outputFilePath);
            }
        }
    }
} 