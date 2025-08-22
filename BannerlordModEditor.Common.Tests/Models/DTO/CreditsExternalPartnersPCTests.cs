using Xunit;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class CreditsExternalPartnersPCTests
{
    [Fact]
    public void TestXmlSerialization()
    {
        var dto = new CreditsExternalPartnersPCDTO
        {
            Categories = new List<CreditsCategoryDTO>
            {
                new CreditsCategoryDTO
                {
                    Text = "PC Partners",
                    Entries = new List<CreditsEntryDTO>
                    {
                        new CreditsEntryDTO { Text = "Steam" },
                        new CreditsEntryDTO { Text = "GOG" }
                    }
                }
            }
        };

        var xml = XmlTestUtils.Serialize(dto);
        var deserialized = XmlTestUtils.Deserialize<CreditsExternalPartnersPCDTO>(xml);

        Assert.NotNull(deserialized);
        Assert.Equal(dto.Categories.Count, deserialized.Categories.Count);
        Assert.Equal(dto.Categories[0].Text, deserialized.Categories[0].Text);
        Assert.Equal(dto.Categories[0].Entries.Count, deserialized.Categories[0].Entries.Count);
    }

    [Fact]
    public void TestEmptyXml()
    {
        var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Credits></Credits>";

        var dto = XmlTestUtils.Deserialize<CreditsExternalPartnersPCDTO>(xml);
        var serialized = XmlTestUtils.Serialize(dto, xml);

        // 验证序列化后的XML不为空
        Assert.NotNull(serialized);
        Assert.Contains("<Credits", serialized);
        
        // 验证反序列化的对象
        Assert.NotNull(dto);
        Assert.Empty(dto.Categories);
        Assert.Empty(dto.LoadFromFile);
    }

    [Fact]
    public void TestRoundTripWithTestData()
    {
        var testDataPath = Path.Combine("TestData", "CreditsExternalPartnersPC.xml");
        if (File.Exists(testDataPath))
        {
            var xml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<CreditsExternalPartnersPCDTO>(xml);
            var serialized = XmlTestUtils.Serialize(dto, xml);

            // 验证序列化后的XML不为空
            Assert.NotNull(serialized);
            Assert.Contains("<Credits", serialized);
            
            // 验证反序列化的对象
            Assert.NotNull(dto);
            
            // 验证往返序列化后的对象仍然正确
            var reDeserialized = XmlTestUtils.Deserialize<CreditsExternalPartnersPCDTO>(serialized);
            Assert.NotNull(reDeserialized);
            Assert.Equal(dto.Categories.Count, reDeserialized.Categories.Count);
            Assert.Equal(dto.LoadFromFile.Count, reDeserialized.LoadFromFile.Count);
        }
    }
}