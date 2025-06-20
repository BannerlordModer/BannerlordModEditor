using BannerlordModEditor.Models;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Tests
{
    public class CraftingTemplatesXmlTests
    {
        [Fact]
        public void DeserializationTest()
        {
            var serializer = new XmlSerializer(typeof(CraftingTemplates));
            var path = Path.Combine("..", "..", "..", "TestData", "crafting_templates.xml");
            using (var stream = File.OpenRead(path))
            {
                var obj = serializer.Deserialize(stream);
                Assert.NotNull(obj);
            }
        }
    }
} 