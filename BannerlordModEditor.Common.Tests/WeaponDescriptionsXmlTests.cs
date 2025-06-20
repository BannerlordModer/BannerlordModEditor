using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class WeaponDescriptionsXmlTests
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
        public void WeaponDescriptions_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "weapon_descriptions.xml");
            var serializer = new XmlSerializer(typeof(WeaponDescriptions));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as WeaponDescriptions;

            Assert.NotNull(result);
            Assert.NotNull(result.Descriptions);
            Assert.True(result.Descriptions.Any());
        }
    }
} 