using BannerlordModEditor.Common.Models.Engine;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class CombatParametersXmlTests
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
        public void CombatParameters_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "combat_parameters.xml");
            var serializer = new XmlSerializer(typeof(CombatParametersBase));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as CombatParametersBase;

            Assert.NotNull(result);
            Assert.Equal("combat_parameters", result.Type);
            Assert.NotNull(result.Definitions);
            Assert.NotNull(result.Definitions.Defs);
            Assert.True(result.Definitions.Defs.Any());
            Assert.NotNull(result.CombatParameters);
            Assert.NotNull(result.CombatParameters.CombatParameterList);
            Assert.True(result.CombatParameters.CombatParameterList.Any());
        }
    }
} 