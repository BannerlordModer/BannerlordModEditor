using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests;

public class SkinsXmlTests
{
    [Fact]
    public void Skins_CanDeserializeFromXml()
    {
        // Arrange
        var solutionRoot = TestUtils.GetSolutionRoot();
        var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skins.xml");
        var xmlContent = File.ReadAllText(xmlPath);

        // Act
        var result = XmlTestUtils.Deserialize<SkinsDO>(xmlContent);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("skin", result.Type);
        Assert.NotNull(result.Skins);
        Assert.NotNull(result.Skins.SkinList);
        Assert.True(result.Skins.SkinList.Count > 0, "Should have at least one skin definition");

        // Test human male adult skin
        var maleSkin = result.Skins.SkinList.Find(s => s.Id == "human_male_adult");
        Assert.NotNull(maleSkin);
        Assert.Equal("Human Male Adult", maleSkin.Name);
        Assert.Equal("human", maleSkin.Race);
        Assert.Equal("male", maleSkin.Gender);
        Assert.Equal("adult", maleSkin.Age);

        // Verify skeleton
        Assert.True(maleSkin.HasSkeleton);
        Assert.NotNull(maleSkin.Skeleton);
        Assert.Equal("human_male", maleSkin.Skeleton.Name);
        Assert.Equal("1.0", maleSkin.Skeleton.Scale);

        // Verify hair meshes
        Assert.True(maleSkin.HasHairMeshes);
        Assert.NotNull(maleSkin.HairMeshes);
        Assert.True(maleSkin.HairMeshes.HasHairMeshes);
        Assert.NotNull(maleSkin.HairMeshes.HairMeshList);
        Assert.True(maleSkin.HairMeshes.HairMeshList.Count >= 3);
        
        var shortHair = maleSkin.HairMeshes.HairMeshList.Find(h => h.Id == "hair_male_01");
        Assert.NotNull(shortHair);
        Assert.Equal("Short Hair", shortHair.Name);
        Assert.Equal("hair_male_short", shortHair.Mesh);
        Assert.Equal("hair_brown", shortHair.Material);
        Assert.Equal("partial", shortHair.HairCoverType);
        Assert.Equal("male_head", shortHair.BodyName);

        // Verify beard meshes
        Assert.True(maleSkin.HasBeardMeshes);
        Assert.NotNull(maleSkin.BeardMeshes);
        Assert.True(maleSkin.BeardMeshes.HasBeardMeshes);
        Assert.NotNull(maleSkin.BeardMeshes.BeardMeshList);
        Assert.True(maleSkin.BeardMeshes.BeardMeshList.Count >= 3);
        
        var fullBeard = maleSkin.BeardMeshes.BeardMeshList.Find(b => b.Id == "beard_01");
        Assert.NotNull(fullBeard);
        Assert.Equal("Full Beard", fullBeard.Name);
        Assert.Equal("beard_full", fullBeard.Mesh);
        Assert.Equal("beard_brown", fullBeard.Material);
        Assert.Equal("male_head", fullBeard.BodyName);

        // Verify voice types
        Assert.True(maleSkin.HasVoiceTypes);
        Assert.NotNull(maleSkin.VoiceTypes);
        Assert.True(maleSkin.VoiceTypes.HasVoiceTypes);
        Assert.NotNull(maleSkin.VoiceTypes.VoiceList);
        Assert.True(maleSkin.VoiceTypes.VoiceList.Count >= 3);
        
        var deepVoice = maleSkin.VoiceTypes.VoiceList.Find(v => v.Id == "voice_male_01");
        Assert.NotNull(deepVoice);
        Assert.Equal("Deep Voice", deepVoice.Name);
        Assert.Equal("male_deep", deepVoice.SoundPrefix);
        Assert.Equal("0.9", deepVoice.Pitch);

        // Verify face textures
        Assert.True(maleSkin.HasFaceTextures);
        Assert.NotNull(maleSkin.FaceTextures);
        Assert.True(maleSkin.FaceTextures.HasFaceTextures);
        Assert.NotNull(maleSkin.FaceTextures.FaceTextureList);
        Assert.True(maleSkin.FaceTextures.FaceTextureList.Count >= 3);
        
        var fairSkin = maleSkin.FaceTextures.FaceTextureList.Find(f => f.Id == "face_01");
        Assert.NotNull(fairSkin);
        Assert.Equal("Fair Skin", fairSkin.Name);
        Assert.Equal("face_fair", fairSkin.Texture);
        Assert.Equal("face_fair_normal", fairSkin.NormalMap);
        Assert.Equal("face_fair_spec", fairSkin.SpecularMap);

        // Verify body meshes
        Assert.True(maleSkin.HasBodyMeshes);
        Assert.NotNull(maleSkin.BodyMeshes);
        Assert.True(maleSkin.BodyMeshes.HasBodyMeshes);
        Assert.NotNull(maleSkin.BodyMeshes.BodyMeshList);
        Assert.True(maleSkin.BodyMeshes.BodyMeshList.Count >= 3);
        
        var athleticBody = maleSkin.BodyMeshes.BodyMeshList.Find(b => b.Id == "body_male_01");
        Assert.NotNull(athleticBody);
        Assert.Equal("Athletic Body", athleticBody.Name);
        Assert.Equal("body_male_athletic", athleticBody.Mesh);
        Assert.Equal("skin_male", athleticBody.Material);
        Assert.Equal("torso", athleticBody.BodyPart);
        Assert.Equal("1.0", athleticBody.Weight);
        Assert.Equal("1.2", athleticBody.Build);

        // Verify tattoo materials
        Assert.True(maleSkin.HasTattooMaterials);
        Assert.NotNull(maleSkin.TattooMaterials);
        Assert.True(maleSkin.TattooMaterials.HasTattooMaterials);
        Assert.NotNull(maleSkin.TattooMaterials.TattooMaterialList);
        Assert.True(maleSkin.TattooMaterials.TattooMaterialList.Count >= 2);
        
        var tribalTattoo = maleSkin.TattooMaterials.TattooMaterialList.Find(t => t.Id == "tattoo_01");
        Assert.NotNull(tribalTattoo);
        Assert.Equal("Tribal Pattern", tribalTattoo.Name);
        Assert.Equal("tattoo_tribal", tribalTattoo.Texture);
        Assert.Equal("tattoo_tribal_mask", tribalTattoo.ColorMask);
        Assert.Equal("tattoo_tribal_alpha", tribalTattoo.AlphaTexture);

        // Test human female adult skin
        var femaleSkin = result.Skins.SkinList.Find(s => s.Id == "human_female_adult");
        Assert.NotNull(femaleSkin);
        Assert.Equal("Human Female Adult", femaleSkin.Name);
        Assert.Equal("human", femaleSkin.Race);
        Assert.Equal("female", femaleSkin.Gender);
        Assert.Equal("adult", femaleSkin.Age);

        // Verify female skeleton
        Assert.True(femaleSkin.HasSkeleton);
        Assert.NotNull(femaleSkin.Skeleton);
        Assert.Equal("human_female", femaleSkin.Skeleton.Name);
        Assert.Equal("0.95", femaleSkin.Skeleton.Scale);

        // Verify female hair meshes
        Assert.True(femaleSkin.HasHairMeshes);
        Assert.NotNull(femaleSkin.HairMeshes);
        Assert.True(femaleSkin.HairMeshes.HasHairMeshes);
        Assert.NotNull(femaleSkin.HairMeshes.HairMeshList);
        Assert.True(femaleSkin.HairMeshes.HairMeshList.Count >= 3);
        
        var longHair = femaleSkin.HairMeshes.HairMeshList.Find(h => h.Id == "hair_female_01");
        Assert.NotNull(longHair);
        Assert.Equal("Long Straight", longHair.Name);
        Assert.Equal("hair_female_long_straight", longHair.Mesh);
        Assert.Equal("hair_brown", longHair.Material);
        Assert.Equal("full", longHair.HairCoverType);
        Assert.Equal("female_head", longHair.BodyName);

        // Verify female should not have beard meshes
        Assert.False(femaleSkin.HasBeardMeshes);
        Assert.Null(femaleSkin.BeardMeshes);

        // Verify female voice types
        Assert.True(femaleSkin.HasVoiceTypes);
        Assert.NotNull(femaleSkin.VoiceTypes);
        Assert.True(femaleSkin.VoiceTypes.HasVoiceTypes);
        Assert.NotNull(femaleSkin.VoiceTypes.VoiceList);
        Assert.True(femaleSkin.VoiceTypes.VoiceList.Count >= 3);
        
        var softVoice = femaleSkin.VoiceTypes.VoiceList.Find(v => v.Id == "voice_female_01");
        Assert.NotNull(softVoice);
        Assert.Equal("Soft Voice", softVoice.Name);
        Assert.Equal("female_soft", softVoice.SoundPrefix);
        Assert.Equal("1.2", softVoice.Pitch);
    }

    [Fact]
    public void Skins_CanSerializeToXml()
    {
        // Arrange
        var skins = new SkinsDO
        {
            Type = "skin",
            HasSkins = true, // 重要：需要设置HasSkins为true才能序列化skins容器
            Skins = new SkinsContainerDO
            {
                SkinList = new List<SkinDO>
                {
                    new SkinDO
                    {
                        Id = "test_skin",
                        Name = "Test Skin",
                        Race = "human",
                        Gender = "male",
                        Age = "adult",
                        HasSkeleton = true,
                        Skeleton = new SkinSkeletonDO
                        {
                            Name = "human_male",
                            Scale = "1.0"
                        },
                        HasHairMeshes = true,
                        HairMeshes = new HairMeshesDO
                        {
                            HasHairMeshes = true,
                            HairMeshList = new List<HairMeshDO>
                            {
                                new HairMeshDO
                                {
                                    Id = "hair_test_01",
                                    Name = "Test Hair",
                                    Mesh = "hair_test",
                                    Material = "hair_brown",
                                    HairCoverType = "partial",
                                    BodyName = "male_head"
                                }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var serializedXml = XmlTestUtils.Serialize(skins);

        // Assert
        Assert.NotNull(serializedXml);
        Assert.Contains("<base type=\"skin\">", serializedXml);
        Assert.Contains("id=\"test_skin\"", serializedXml);
        Assert.Contains("name=\"Test Skin\"", serializedXml);
        Assert.Contains("<skeleton name=\"human_male\" scale=\"1.0\" />", serializedXml);
        Assert.Contains("hair_test_01", serializedXml);
    }

    [Fact]
    public void Skins_RoundTripSerialization()
    {
        // Arrange
        var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""skin"">
  <skins>
    <skin id=""human_male_adult"" name=""Human Male Adult"" race=""human"" gender=""male"" age=""adult"">
      <skeleton name=""human_male"" scale=""1.0"" />
      <hair_meshes>
        <hair_mesh id=""hair_male_01"" name=""Short Hair"" mesh=""hair_male_short"" material=""hair_brown"" hair_cover_type=""partial"" body_name=""male_head"" />
      </hair_meshes>
      <voice_types>
        <voice id=""voice_male_01"" name=""Deep Voice"" sound_prefix=""male_deep"" pitch=""0.9"" />
      </voice_types>
    </skin>
  </skins>
</base>";

        // Act
        var deserialized = XmlTestUtils.Deserialize<SkinsDO>(originalXml);
        var serializedXml = XmlTestUtils.Serialize(deserialized);

        // Assert
        Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
    }

    [Fact]
    public void Skins_EmptySkinList()
    {
        // Arrange
        var xmlContent = @"<base type=""skin"" />";

        // Act
        var result = XmlTestUtils.Deserialize<SkinsDO>(xmlContent);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Skins);
        Assert.NotNull(result.Skins.SkinList);
        Assert.Empty(result.Skins.SkinList);
    }

    [Fact]
    public void Skins_SkinWithMinimalData()
    {
        // Arrange
        var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""skin"">
  <skins>
    <skin id=""minimal_skin"" name=""Minimal Skin"" race=""human"" gender=""male"" age=""adult"" />
  </skins>
</base>";

        // Act
        var result = XmlTestUtils.Deserialize<SkinsDO>(xmlContent);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Skins.SkinList);
        
        var skin = result.Skins.SkinList[0];
        Assert.Equal("minimal_skin", skin.Id);
        Assert.Equal("Minimal Skin", skin.Name);
        Assert.Equal("human", skin.Race);
        Assert.Equal("male", skin.Gender);
        Assert.Equal("adult", skin.Age);
        Assert.False(skin.HasSkeleton);
        Assert.False(skin.HasHairMeshes);
        Assert.False(skin.HasBeardMeshes);
        Assert.False(skin.HasVoiceTypes);
        Assert.False(skin.HasFaceTextures);
        Assert.False(skin.HasBodyMeshes);
        Assert.False(skin.HasTattooMaterials);
    }

    [Fact]
    public void Skins_SerializationControl_HandlesEmptyCollections()
    {
        // Arrange
        var skin = new SkinDO
        {
            Id = "test_skin",
            Name = "Test Skin",
            HasHairMeshes = true,
            HairMeshes = new HairMeshesDO
            {
                HasEmptyHairMeshes = true, // Empty collection
                HairMeshList = new List<HairMeshDO>()
            },
            HasVoiceTypes = true,
            VoiceTypes = new VoiceTypesDO
            {
                HasEmptyVoiceTypes = true, // Empty collection
                VoiceList = new List<VoiceDO>()
            }
        };

        var skins = new SkinsDO
        {
            HasSkins = true, // 重要：需要设置HasSkins为true才能序列化skins容器
            Skins = new SkinsContainerDO
            {
                SkinList = new List<SkinDO> { skin }
            }
        };

        // Act
        var serializedXml = XmlTestUtils.Serialize(skins);

        // Assert
        Assert.NotNull(serializedXml);
        // 当HasXxx为true但集合为空时，应该序列化空元素
        Assert.Contains("<hair_meshes />", serializedXml);
        Assert.Contains("<voice_types />", serializedXml);
        
        // 输出调试信息
        System.Console.WriteLine("Serialized XML:");
        System.Console.WriteLine(serializedXml);
    }

    [Fact]
    public void Skins_SerializationControl_SkipsEmptyCollectionsWhenHasIsFalse()
    {
        // Arrange
        var skin = new SkinDO
        {
            Id = "test_skin",
            Name = "Test Skin",
            HasHairMeshes = false, // Don't serialize
            HairMeshes = new HairMeshesDO
            {
                HairMeshList = new List<HairMeshDO>
                {
                    new HairMeshDO
                    {
                        Id = "hair_test",
                        Name = "Test Hair"
                    }
                }
            },
            HasVoiceTypes = false // Don't serialize
        };

        var skins = new SkinsDO
        {
            Skins = new SkinsContainerDO
            {
                SkinList = new List<SkinDO> { skin }
            }
        };

        // Act
        var serializedXml = XmlTestUtils.Serialize(skins);

        // Assert
        Assert.NotNull(serializedXml);
        // 当HasXxx为false时，即使有数据也不应该序列化
        Assert.DoesNotContain("<hair_meshes", serializedXml);
        Assert.DoesNotContain("<voice_types", serializedXml);
        Assert.DoesNotContain("hair_test", serializedXml);
    }
} 