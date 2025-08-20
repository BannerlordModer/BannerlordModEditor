using Xunit;
using System.IO;
using BannerlordModEditor.Common.Models.DTO;

namespace BannerlordModEditor.Common.Tests.Models.DTO;

public class CreditsExternalPartnersXBoxTests
{
    [Fact]
    public void TestXmlSerialization()
    {
        var dto = new CreditsExternalPartnersXBoxDTO
        {
            Categories = new List<CreditsCategoryDTO>
            {
                new CreditsCategoryDTO
                {
                    Text = "XBox Partners",
                    Entries = new List<CreditsEntryDTO>
                    {
                        new CreditsEntryDTO { Text = "Microsoft" },
                        new CreditsEntryDTO { Text = "Platform Holder" }
                    }
                }
            }
        };

        var xml = XmlTestUtils.Serialize(dto);
        var deserialized = XmlTestUtils.Deserialize<CreditsExternalPartnersXBoxDTO>(xml);

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

        var dto = XmlTestUtils.Deserialize<CreditsExternalPartnersXBoxDTO>(xml);
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
        var testDataPath = Path.Combine("TestData", "CreditsExternalPartnersXBox.xml");
        if (File.Exists(testDataPath))
        {
            var xml = File.ReadAllText(testDataPath);
            var dto = XmlTestUtils.Deserialize<CreditsExternalPartnersXBoxDTO>(xml);
            var serialized = XmlTestUtils.Serialize(dto, xml);

            // 验证序列化后的XML不为空
            Assert.NotNull(serialized);
            Assert.Contains("<Credits", serialized);
            
            // 验证反序列化的对象
            Assert.NotNull(dto);
            
            // 验证往返序列化后的对象仍然正确
            var reDeserialized = XmlTestUtils.Deserialize<CreditsExternalPartnersXBoxDTO>(serialized);
            Assert.NotNull(reDeserialized);
            Assert.Equal(dto.Categories.Count, reDeserialized.Categories.Count);
            Assert.Equal(dto.LoadFromFile.Count, reDeserialized.LoadFromFile.Count);
        }
    }
}