using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class VoicesXmlTests
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
        public void Voices_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "voices.xml");
            var serializer = new XmlSerializer(typeof(VoicesBase));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as VoicesBase;

            Assert.NotNull(result);
            Assert.NotNull(result.FaceAnimationRecords);
            Assert.NotNull(result.FaceAnimationRecords.FaceAnimationRecordList);
            Assert.True(result.FaceAnimationRecords.FaceAnimationRecordList.Any());
        }
    }
} 