using BannerlordModEditor.Common.Models.Engine;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class PostfxGraphsXmlTests
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
        public void PostfxGraphs_Deserialization_Works()
        {
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "before_transparents_graph.xml");
            var serializer = new XmlSerializer(typeof(PostfxGraphsBase));
            using var fileStream = new FileStream(xmlPath, FileMode.Open);
            var result = serializer.Deserialize(fileStream) as PostfxGraphsBase;

            Assert.NotNull(result);
            Assert.Equal("particle_system", result.Type);
            Assert.NotNull(result.PostfxGraphs);
            Assert.NotNull(result.PostfxGraphs.PostfxGraphList);
            Assert.True(result.PostfxGraphs.PostfxGraphList.Any());
        }
    }
} 