using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Data;
using Xunit;

namespace BannerlordModEditor.Common.Tests;

public class SkinsXmlTests
{
    [Fact]
    public void Skins_LoadAndSave_ShouldBeLogicallyIdentical()
    {
        // Arrange
        var solutionRoot = TestUtils.GetSolutionRoot();
        var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skins.xml");
        
        // Act & Assert - Load the XML
        var serializer = new XmlSerializer(typeof(SkinsBase));
        SkinsBase skins;
        using (var reader = new StreamReader(xmlPath))
        {
            skins = (SkinsBase)serializer.Deserialize(reader)!;
        }

        // Verify basic structure
        Assert.NotNull(skins);
        Assert.NotNull(skins.Skins);
        Assert.NotNull(skins.Skins.Skin);
        Assert.Equal("skin", skins.Type);
        Assert.True(skins.Skins.Skin.Count > 0, "Should have at least one skin definition");

        // Test human male adult skin
        var maleSkin = skins.Skins.Skin.Find(s => s.Id == "human_male_adult");
        Assert.NotNull(maleSkin);
        Assert.Equal("Human Male Adult", maleSkin.Name);
        Assert.Equal("human", maleSkin.Race);
        Assert.Equal("male", maleSkin.Gender);
        Assert.Equal("adult", maleSkin.Age);

        // Verify skeleton
        Assert.NotNull(maleSkin.Skeleton);
        Assert.Equal("human_male", maleSkin.Skeleton.Name);
        Assert.Equal("1.0", maleSkin.Skeleton.Scale);

        // Verify hair meshes
        Assert.NotNull(maleSkin.HairMeshes);
        Assert.NotNull(maleSkin.HairMeshes.HairMesh);
        Assert.True(maleSkin.HairMeshes.HairMesh.Count >= 3);
        
        var shortHair = maleSkin.HairMeshes.HairMesh.Find(h => h.Id == "hair_male_01");
        Assert.NotNull(shortHair);
        Assert.Equal("Short Hair", shortHair.Name);
        Assert.Equal("hair_male_short", shortHair.Mesh);
        Assert.Equal("hair_brown", shortHair.Material);
        Assert.Equal("partial", shortHair.HairCoverType);
        Assert.Equal("male_head", shortHair.BodyName);

        // Verify beard meshes
        Assert.NotNull(maleSkin.BeardMeshes);
        Assert.NotNull(maleSkin.BeardMeshes.BeardMesh);
        Assert.True(maleSkin.BeardMeshes.BeardMesh.Count >= 3);
        
        var fullBeard = maleSkin.BeardMeshes.BeardMesh.Find(b => b.Id == "beard_01");
        Assert.NotNull(fullBeard);
        Assert.Equal("Full Beard", fullBeard.Name);
        Assert.Equal("beard_full", fullBeard.Mesh);
        Assert.Equal("beard_brown", fullBeard.Material);
        Assert.Equal("male_head", fullBeard.BodyName);

        // Verify voice types
        Assert.NotNull(maleSkin.VoiceTypes);
        Assert.NotNull(maleSkin.VoiceTypes.Voice);
        Assert.True(maleSkin.VoiceTypes.Voice.Count >= 3);
        
        var deepVoice = maleSkin.VoiceTypes.Voice.Find(v => v.Id == "voice_male_01");
        Assert.NotNull(deepVoice);
        Assert.Equal("Deep Voice", deepVoice.Name);
        Assert.Equal("male_deep", deepVoice.SoundPrefix);
        Assert.Equal("0.9", deepVoice.Pitch);

        // Verify face textures
        Assert.NotNull(maleSkin.FaceTextures);
        Assert.NotNull(maleSkin.FaceTextures.FaceTexture);
        Assert.True(maleSkin.FaceTextures.FaceTexture.Count >= 3);
        
        var fairSkin = maleSkin.FaceTextures.FaceTexture.Find(f => f.Id == "face_01");
        Assert.NotNull(fairSkin);
        Assert.Equal("Fair Skin", fairSkin.Name);
        Assert.Equal("face_fair", fairSkin.Texture);
        Assert.Equal("face_fair_normal", fairSkin.NormalMap);
        Assert.Equal("face_fair_spec", fairSkin.SpecularMap);

        // Verify body meshes
        Assert.NotNull(maleSkin.BodyMeshes);
        Assert.NotNull(maleSkin.BodyMeshes.BodyMesh);
        Assert.True(maleSkin.BodyMeshes.BodyMesh.Count >= 3);
        
        var athleticBody = maleSkin.BodyMeshes.BodyMesh.Find(b => b.Id == "body_male_01");
        Assert.NotNull(athleticBody);
        Assert.Equal("Athletic Body", athleticBody.Name);
        Assert.Equal("body_male_athletic", athleticBody.Mesh);
        Assert.Equal("skin_male", athleticBody.Material);
        Assert.Equal("torso", athleticBody.BodyPart);
        Assert.Equal("1.0", athleticBody.Weight);
        Assert.Equal("1.2", athleticBody.Build);

        // Verify tattoo materials
        Assert.NotNull(maleSkin.TattooMaterials);
        Assert.NotNull(maleSkin.TattooMaterials.TattooMaterial);
        Assert.True(maleSkin.TattooMaterials.TattooMaterial.Count >= 2);
        
        var tribalTattoo = maleSkin.TattooMaterials.TattooMaterial.Find(t => t.Id == "tattoo_01");
        Assert.NotNull(tribalTattoo);
        Assert.Equal("Tribal Pattern", tribalTattoo.Name);
        Assert.Equal("tattoo_tribal", tribalTattoo.Texture);
        Assert.Equal("tattoo_tribal_mask", tribalTattoo.ColorMask);
        Assert.Equal("tattoo_tribal_alpha", tribalTattoo.AlphaTexture);

        // Test human female adult skin
        var femaleSkin = skins.Skins.Skin.Find(s => s.Id == "human_female_adult");
        Assert.NotNull(femaleSkin);
        Assert.Equal("Human Female Adult", femaleSkin.Name);
        Assert.Equal("human", femaleSkin.Race);
        Assert.Equal("female", femaleSkin.Gender);
        Assert.Equal("adult", femaleSkin.Age);

        // Verify female skeleton
        Assert.NotNull(femaleSkin.Skeleton);
        Assert.Equal("human_female", femaleSkin.Skeleton.Name);
        Assert.Equal("0.95", femaleSkin.Skeleton.Scale);

        // Verify female hair meshes
        Assert.NotNull(femaleSkin.HairMeshes);
        Assert.NotNull(femaleSkin.HairMeshes.HairMesh);
        Assert.True(femaleSkin.HairMeshes.HairMesh.Count >= 3);
        
        var longHair = femaleSkin.HairMeshes.HairMesh.Find(h => h.Id == "hair_female_01");
        Assert.NotNull(longHair);
        Assert.Equal("Long Straight", longHair.Name);
        Assert.Equal("hair_female_long_straight", longHair.Mesh);
        Assert.Equal("hair_brown", longHair.Material);
        Assert.Equal("full", longHair.HairCoverType);
        Assert.Equal("female_head", longHair.BodyName);

        // Verify female should not have beard meshes
        Assert.Null(femaleSkin.BeardMeshes);

        // Verify female voice types
        Assert.NotNull(femaleSkin.VoiceTypes);
        Assert.NotNull(femaleSkin.VoiceTypes.Voice);
        Assert.True(femaleSkin.VoiceTypes.Voice.Count >= 3);
        
        var softVoice = femaleSkin.VoiceTypes.Voice.Find(v => v.Id == "voice_female_01");
        Assert.NotNull(softVoice);
        Assert.Equal("Soft Voice", softVoice.Name);
        Assert.Equal("female_soft", softVoice.SoundPrefix);
        Assert.Equal("1.2", softVoice.Pitch);

        // Test serialization
        using (var stringWriter = new StringWriter())
        {
            using (var xmlWriter = new StringWriter())
            {
                serializer.Serialize(xmlWriter, skins);
                var serializedXml = xmlWriter.ToString();
                Assert.Contains("skin", serializedXml);
                Assert.Contains("human_male_adult", serializedXml);
                Assert.Contains("human_female_adult", serializedXml);
            }
        }
    }

    [Fact]
    public void Skins_ShouldHandleOptionalFields()
    {
        // Arrange
        var skin = new Skin
        {
            Id = "test_skin",
            Name = "Test Skin",
            Race = "human",
            Gender = "male",
            Age = "adult"
        };

        var skins = new SkinsBase
        {
            Skins = new SkinsContainer
            {
                Skin = new List<Skin> { skin }
            }
        };

        // Act & Assert - Serialize with minimal data
        var serializer = new XmlSerializer(typeof(SkinsBase));
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, skins);
            var xml = stringWriter.ToString();
            
            // Should not contain null fields
            Assert.DoesNotContain("skeleton", xml.ToLower());
            Assert.DoesNotContain("hair_meshes", xml.ToLower());
            Assert.DoesNotContain("beard_meshes", xml.ToLower());
        }
    }

    [Fact] 
    public void Skins_ShouldHandleEmptyCollections()
    {
        // Arrange
        var skin = new Skin
        {
            Id = "test_skin",
            HairMeshes = new HairMeshes(), // Empty collection
            VoiceTypes = new VoiceTypes()  // Empty collection
        };

        var skins = new SkinsBase
        {
            Skins = new SkinsContainer
            {
                Skin = new List<Skin> { skin }
            }
        };

        // Act & Assert
        var serializer = new XmlSerializer(typeof(SkinsBase));
        using (var stringWriter = new StringWriter())
        {
            serializer.Serialize(stringWriter, skins);
            var xml = stringWriter.ToString();
            
            // Empty collections should still serialize properly
            Assert.Contains("<hair_meshes", xml);
            Assert.Contains("<voice_types", xml);
        }
    }
} 