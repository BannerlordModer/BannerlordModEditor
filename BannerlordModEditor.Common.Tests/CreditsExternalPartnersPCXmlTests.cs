using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class CreditsExternalPartnersPCXmlTests
    {
        [Fact]
        public void CreditsExternalPartnersPC_DeserializesToEmptyObject()
        {
            var xmlPath = Path.Combine(TestUtils.GetSolutionRoot(), "BannerlordModEditor.Common.Tests", "TestData", "CreditsExternalPartnersPC.xml");

            // Deserialization using the existing model
            var serializer = new XmlSerializer(typeof(CreditsLegalConsole));
            CreditsLegalConsole? model;
            using (var fileStream = new FileStream(xmlPath, FileMode.Open))
            {
                model = serializer.Deserialize(fileStream) as CreditsLegalConsole;
            }

            // Assert
            Assert.NotNull(model);
            Assert.Null(model.Category); // This is sufficient to prove correctness for an empty file.
        }
    }
} 