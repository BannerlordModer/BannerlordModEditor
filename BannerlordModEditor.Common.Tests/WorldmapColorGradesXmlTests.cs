using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class WorldmapColorGradesXmlTests
    {
        [Fact]
        public void WorldmapColorGrades_Load_ShouldSucceed()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "worldmap_color_grades.xml");
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(WorldmapColorGradesBase));

            // Act
            WorldmapColorGradesBase colorGrades;
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                colorGrades = (WorldmapColorGradesBase)serializer.Deserialize(reader)!;
            }

            // Assert
            Assert.NotNull(colorGrades);
            
            Assert.NotNull(colorGrades.ColorGradeGrid);
            Assert.Equal("worldmap_colorgrade_grid", colorGrades.ColorGradeGrid.Name);

            Assert.NotNull(colorGrades.ColorGradeDefault);
            Assert.Equal("worldmap_colorgrade_stratosphere", colorGrades.ColorGradeDefault.Name);

            Assert.NotNull(colorGrades.ColorGradeNight);
            Assert.Equal("worldmap_colorgrade_night", colorGrades.ColorGradeNight.Name);
            
            Assert.NotNull(colorGrades.ColorGradeList);
            Assert.NotEmpty(colorGrades.ColorGradeList);
            
            var desertGrade = colorGrades.ColorGradeList.FirstOrDefault(cg => cg.Name == "worldmap_colorgrade_desert");
            Assert.NotNull(desertGrade);
            Assert.Equal("20", desertGrade.Value);
        }
    }
} 