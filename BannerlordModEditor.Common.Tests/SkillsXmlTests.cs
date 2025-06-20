using BannerlordModEditor.Common.Models.Data;
using System.IO;
using System.Linq;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class SkillsXmlTests
    {
        [Fact]
        public void Skills_Load_ShouldSucceed()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skills.xml");
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ArrayOfSkillData));

            // Act
            ArrayOfSkillData skills;
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                skills = (ArrayOfSkillData)serializer.Deserialize(reader)!;
            }

            // Assert
            Assert.NotNull(skills);
            Assert.NotNull(skills.SkillDataList);
            Assert.True(skills.SkillDataList.Count > 0);

            var ironFlesh1 = skills.SkillDataList.FirstOrDefault(s => s.Id == "IronFlesh1");
            Assert.NotNull(ironFlesh1);
            Assert.Equal("Iron Flesh 1", ironFlesh1.Name);
            Assert.NotNull(ironFlesh1.Modifiers);
            Assert.NotNull(ironFlesh1.Modifiers.AttributeModifierList);
            var modifier = ironFlesh1.Modifiers.AttributeModifierList.First();
            Assert.Equal("AgentHitPoints", modifier.AttribCode);
            Assert.Equal("Multiply", modifier.Modification);
            Assert.Equal("1.01", modifier.Value);
            Assert.Equal("Iron flesh increases hit points", ironFlesh1.Documentation);
        }
    }
} 