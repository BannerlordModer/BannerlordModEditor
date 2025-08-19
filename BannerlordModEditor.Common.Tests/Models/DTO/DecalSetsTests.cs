using Xunit;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class DecalSetsTests
{
    [Fact]
    public void TestXmlSerialization()
    {
        var dto = new DecalSetsDTO
        {
            Type = "decal_set",
            DecalSets = new DecalSetsContainerDTO
            {
                Items = new List<DecalSetDTO>
                {
                    new DecalSetDTO
                    {
                        Name = "editor_set",
                        TotalDecalLifeBase = "999999999999.000000",
                        VisibleDecalLifeBase = "999999999999.000000",
                        MaximumDecalCountPerGrid = "0",
                        MinVisibilityArea = "0.0, 0.0",
                        AdaptiveTimeLimit = "false",
                        FadeOutDelete = "false"
                    },
                    new DecalSetDTO
                    {
                        Name = "default_set",
                        TotalDecalLifeBase = "240.000000",
                        VisibleDecalLifeBase = "60.000000",
                        MaximumDecalCountPerGrid = "0",
                        MinVisibilityArea = "0.0, 0.0",
                        AdaptiveTimeLimit = "true",
                        FadeOutDelete = "true"
                    }
                }
            }
        };

        var xml = XmlTestUtils.Serialize(dto);
        var deserialized = XmlTestUtils.Deserialize<DecalSetsDTO>(xml);

        Assert.NotNull(deserialized);
        Assert.Equal(dto.Type, deserialized.Type);
        Assert.Equal(dto.DecalSets.Items.Count, deserialized.DecalSets.Items.Count);
        Assert.Equal("editor_set", deserialized.DecalSets.Items[0].Name);
        Assert.Equal("default_set", deserialized.DecalSets.Items[1].Name);
    }

    [Fact]
    public void TestRoundTripWithTestData()
    {
        var testDataPath = Path.Combine("TestData", "decal_sets.xml");
        if (File.Exists(testDataPath))
        {
            var xml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<DecalSetsDTO>(xml);
            var serialized = XmlTestUtils.Serialize(dto, xml);

            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}