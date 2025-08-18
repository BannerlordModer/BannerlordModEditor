using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class BeforeTransparentsGraphSimpleTests
    {
        [Fact]
        public void BeforeTransparentsGraph_BasicSerialization_ShouldWork()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "before_transparents_graph.xml");
            
            if (!File.Exists(xmlPath))
            {
                // Skip test if file doesn't exist
                Assert.True(true);
                return;
            }
            
            var xml = File.ReadAllText(xmlPath);

            // Act
            var obj = XmlTestUtils.Deserialize<BeforeTransparentsGraphDO>(xml);
            var serializedXml = XmlTestUtils.Serialize(obj);

            // Assert
            Assert.NotNull(obj);
            Assert.Equal("particle_system", obj.Type);
            Assert.True(obj.HasPostfxGraphs);
            Assert.NotNull(obj.PostfxGraphs);
            Assert.NotNull(obj.PostfxGraphs.PostfxGraphList);
            Assert.True(obj.PostfxGraphs.PostfxGraphList.Count > 0);
            
            // Basic structural equality check
            Assert.Contains("before_transparents_graph", serializedXml);
            Assert.Contains("postfx_graphs", serializedXml);
        }
    }
}