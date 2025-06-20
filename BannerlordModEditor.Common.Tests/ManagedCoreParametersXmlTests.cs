using BannerlordModEditor.Common.Models.Engine;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ManagedCoreParametersXmlTests
    {
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("Solution root not found");
        }

        [Fact]
        public void ManagedCoreParameters_Load_ShouldSucceed()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ManagedCoreParametersBase));

            // Act
            ManagedCoreParametersBase coreParams;
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                coreParams = (ManagedCoreParametersBase)serializer.Deserialize(reader)!;
            }

            // Assert
            Assert.NotNull(coreParams);
            Assert.Equal("combat_parameters", coreParams.Type);
            Assert.NotNull(coreParams.ManagedCoreParameters);
            Assert.NotNull(coreParams.ManagedCoreParameters.ManagedCoreParameter);
            Assert.True(coreParams.ManagedCoreParameters.ManagedCoreParameter.Count > 0);

            var tutorialParam = coreParams.ManagedCoreParameters.ManagedCoreParameter.FirstOrDefault(p => p.Id == "EnableCampaignTutorials");
            Assert.NotNull(tutorialParam);
            Assert.Equal("1", tutorialParam.Value);

            var javelinFriction = coreParams.ManagedCoreParameters.ManagedCoreParameter.FirstOrDefault(p => p.Id == "AirFrictionJavelin");
            Assert.NotNull(javelinFriction);
            Assert.Equal("0.002", javelinFriction.Value);
        }

        [Fact]
        public void ManagedCoreParameters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // ... existing code ...
        }
    }
} 