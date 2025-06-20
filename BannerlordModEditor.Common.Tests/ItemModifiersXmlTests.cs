using BannerlordModEditor.Common.Models.Game;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ItemModifiersXmlTests
    {
        [Fact]
        public void ItemModifiers_Load_ShouldSucceed()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "item_modifiers.xml");
            var serializer = new XmlSerializer(typeof(ItemModifiers));

            // Act
            ItemModifiers itemModifiers;
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                itemModifiers = (ItemModifiers)serializer.Deserialize(reader)!;
            }

            // Assert
            Assert.NotNull(itemModifiers);
            Assert.NotEmpty(itemModifiers.ItemModifier);

            var lordlyPlate = itemModifiers.ItemModifier.FirstOrDefault(im => im.Id == "lordly_plate");
            Assert.NotNull(lordlyPlate);
            Assert.Equal("{=w3jn7Ahz}Lordly {ITEMNAME}", lordlyPlate.Name);
            Assert.Equal("1.5", lordlyPlate.PriceFactor);
            Assert.Equal("masterwork", lordlyPlate.Quality);
            Assert.Equal("8", lordlyPlate.Armor);
        }
    }
} 