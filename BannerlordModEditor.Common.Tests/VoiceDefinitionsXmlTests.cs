using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Audio;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class VoiceDefinitionsXmlTests
    {
        [Fact]
        public void VoiceDefinitions_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
    <voice_type_declarations>
        <voice_type name=""Grunt"" />
        <voice_type name=""Yell"" />
        <voice_type name=""Pain"" />
    </voice_type_declarations>
    <voice_definition
        name=""male_01""
        sound_and_collision_info_class=""human""
        only_for_npcs=""true""
        min_pitch_multiplier=""0.9""
        max_pitch_multiplier=""1.1"">
        <voice type=""Grunt"" path=""event:/voice/combat/male/01/grunt"" face_anim=""grunt"" />
        <voice type=""Yell"" path=""event:/voice/combat/male/01/yell"" face_anim=""yell"" />
    </voice_definition>
</voice_definitions>";

            var result = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasVoiceTypeDeclarations);
            Assert.NotNull(result.VoiceTypeDeclarations);
            Assert.Equal(3, result.VoiceTypeDeclarations.VoiceTypes.Count);
            
            var voiceType = result.VoiceTypeDeclarations.VoiceTypes[0];
            Assert.Equal("Grunt", voiceType.Name);
            
            Assert.NotNull(result.VoiceDefinitions);
            Assert.Single(result.VoiceDefinitions);
            
            var voiceDefinition = result.VoiceDefinitions[0];
            Assert.Equal("male_01", voiceDefinition.Name);
            Assert.Equal("human", voiceDefinition.SoundAndCollisionInfoClass);
            Assert.Equal("true", voiceDefinition.OnlyForNpcs);
            Assert.Equal("0.9", voiceDefinition.MinPitchMultiplier);
            Assert.Equal("1.1", voiceDefinition.MaxPitchMultiplier);
            
            Assert.NotNull(voiceDefinition.Voices);
            Assert.Equal(2, voiceDefinition.Voices.Count);
            
            var voice = voiceDefinition.Voices[0];
            Assert.Equal("Grunt", voice.Type);
            Assert.Equal("event:/voice/combat/male/01/grunt", voice.Path);
            Assert.Equal("grunt", voice.FaceAnim);
        }

        [Fact]
        public void VoiceDefinitions_CanSerializeToXml()
        {
            // Arrange
            var model = new VoiceDefinitionsDO
            {
                HasVoiceTypeDeclarations = true,
                VoiceTypeDeclarations = new VoiceTypeDeclarationsDO
                {
                    VoiceTypes = new List<VoiceTypeDO>
                    {
                        new VoiceTypeDO
                        {
                            Name = "Grunt"
                        },
                        new VoiceTypeDO
                        {
                            Name = "Yell"
                        }
                    }
                },
                VoiceDefinitions = new List<VoiceDefinitionDO>
                {
                    new VoiceDefinitionDO
                    {
                        Name = "male_01",
                        SoundAndCollisionInfoClass = "human",
                        OnlyForNpcs = "true",
                        MinPitchMultiplier = "0.9",
                        MaxPitchMultiplier = "1.1",
                        Voices = new List<VoiceDO>
                        {
                            new VoiceDO
                            {
                                Type = "Grunt",
                                Path = "event:/voice/combat/male/01/grunt",
                                FaceAnim = "grunt"
                            }
                        }
                    }
                }
            };

            var xml = XmlTestUtils.Serialize(model);

            // Assert
            Assert.Contains("male_01", xml);
            Assert.Contains("Grunt", xml);
            Assert.Contains("voice_type", xml);
            Assert.Contains("voice_definition", xml);
        }

        [Fact]
        public void VoiceDefinitions_RoundTripPreservesData()
        {
            // Arrange
            var originalModel = new VoiceDefinitionsDO
            {
                HasVoiceTypeDeclarations = true,
                VoiceTypeDeclarations = new VoiceTypeDeclarationsDO
                {
                    VoiceTypes = new List<VoiceTypeDO>
                    {
                        new VoiceTypeDO { Name = "Grunt" },
                        new VoiceTypeDO { Name = "Yell" },
                        new VoiceTypeDO { Name = "Pain" }
                    }
                },
                VoiceDefinitions = new List<VoiceDefinitionDO>
                {
                    new VoiceDefinitionDO
                    {
                        Name = "male_01",
                        SoundAndCollisionInfoClass = "human",
                        OnlyForNpcs = "true",
                        MinPitchMultiplier = "0.9",
                        MaxPitchMultiplier = "1.1",
                        Voices = new List<VoiceDO>
                        {
                            new VoiceDO
                            {
                                Type = "Grunt",
                                Path = "event:/voice/combat/male/01/grunt",
                                FaceAnim = "grunt"
                            },
                            new VoiceDO
                            {
                                Type = "Yell",
                                Path = "event:/voice/combat/male/01/yell",
                                FaceAnim = "yell"
                            }
                        }
                    }
                }
            };

            // Act - Serialize
            var xml = XmlTestUtils.Serialize(originalModel);

            // Act - Deserialize
            var deserializedModel = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xml);

            // Assert
            Assert.NotNull(deserializedModel);
            Assert.Equal(originalModel.HasVoiceTypeDeclarations, deserializedModel.HasVoiceTypeDeclarations);
            
            Assert.Equal(originalModel.VoiceTypeDeclarations.VoiceTypes.Count, 
                        deserializedModel.VoiceTypeDeclarations.VoiceTypes.Count);
            
            var originalVoiceType = originalModel.VoiceTypeDeclarations.VoiceTypes[0];
            var deserializedVoiceType = deserializedModel.VoiceTypeDeclarations.VoiceTypes[0];
            
            Assert.Equal(originalVoiceType.Name, deserializedVoiceType.Name);
            
            Assert.Equal(originalModel.VoiceDefinitions.Count, 
                        deserializedModel.VoiceDefinitions.Count);
            
            var originalDefinition = originalModel.VoiceDefinitions[0];
            var deserializedDefinition = deserializedModel.VoiceDefinitions[0];
            
            Assert.Equal(originalDefinition.Name, deserializedDefinition.Name);
            Assert.Equal(originalDefinition.SoundAndCollisionInfoClass, deserializedDefinition.SoundAndCollisionInfoClass);
            Assert.Equal(originalDefinition.OnlyForNpcs, deserializedDefinition.OnlyForNpcs);
            Assert.Equal(originalDefinition.MinPitchMultiplier, deserializedDefinition.MinPitchMultiplier);
            Assert.Equal(originalDefinition.MaxPitchMultiplier, deserializedDefinition.MaxPitchMultiplier);
            
            Assert.Equal(originalDefinition.Voices.Count, deserializedDefinition.Voices.Count);
            
            var originalVoice = originalDefinition.Voices[0];
            var deserializedVoice = deserializedDefinition.Voices[0];
            
            Assert.Equal(originalVoice.Type, deserializedVoice.Type);
            Assert.Equal(originalVoice.Path, deserializedVoice.Path);
            Assert.Equal(originalVoice.FaceAnim, deserializedVoice.FaceAnim);
        }

        [Fact]
        public void VoiceDefinitions_EmptyXmlHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<voice_definitions></voice_definitions>";

            var result = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasVoiceTypeDeclarations);
            Assert.NotNull(result.VoiceTypeDeclarations);
            Assert.Empty(result.VoiceTypeDeclarations.VoiceTypes);
            Assert.NotNull(result.VoiceDefinitions);
            Assert.Empty(result.VoiceDefinitions);
        }

        [Fact]
        public void VoiceDefinitions_OnlyVoiceTypeDeclarationsHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
    <voice_type_declarations>
        <voice_type name=""Grunt"" />
        <voice_type name=""Yell"" />
        <voice_type name=""Pain"" />
    </voice_type_declarations>
</voice_definitions>";

            var result = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasVoiceTypeDeclarations);
            Assert.Equal(3, result.VoiceTypeDeclarations.VoiceTypes.Count);
            
            var firstVoice = result.VoiceTypeDeclarations.VoiceTypes[0];
            Assert.Equal("Grunt", firstVoice.Name);
            
            var secondVoice = result.VoiceTypeDeclarations.VoiceTypes[1];
            Assert.Equal("Yell", secondVoice.Name);
            
            var thirdVoice = result.VoiceTypeDeclarations.VoiceTypes[2];
            Assert.Equal("Pain", thirdVoice.Name);
            
            Assert.NotNull(result.VoiceDefinitions);
            Assert.Empty(result.VoiceDefinitions);
        }

        [Fact]
        public void VoiceDefinitions_OnlyVoiceDefinitionsHandledCorrectly()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
    <voice_definition
        name=""male_01""
        sound_and_collision_info_class=""human""
        only_for_npcs=""true""
        min_pitch_multiplier=""0.9""
        max_pitch_multiplier=""1.1"">
        <voice type=""Grunt"" path=""event:/voice/combat/male/01/grunt"" face_anim=""grunt"" />
    </voice_definition>
    <voice_definition
        name=""male_02""
        sound_and_collision_info_class=""human""
        only_for_npcs=""false""
        min_pitch_multiplier=""0.8""
        max_pitch_multiplier=""1.2"">
        <voice type=""Yell"" path=""event:/voice/combat/male/02/yell"" face_anim=""yell"" />
    </voice_definition>
</voice_definitions>";

            var result = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasVoiceTypeDeclarations);
            Assert.NotNull(result.VoiceTypeDeclarations);
            Assert.Empty(result.VoiceTypeDeclarations.VoiceTypes);
            
            Assert.NotNull(result.VoiceDefinitions);
            Assert.Equal(2, result.VoiceDefinitions.Count);
            
            var firstDefinition = result.VoiceDefinitions[0];
            Assert.Equal("male_01", firstDefinition.Name);
            Assert.Equal("human", firstDefinition.SoundAndCollisionInfoClass);
            Assert.Equal("true", firstDefinition.OnlyForNpcs);
            Assert.Equal("0.9", firstDefinition.MinPitchMultiplier);
            Assert.Equal("1.1", firstDefinition.MaxPitchMultiplier);
            Assert.Single(firstDefinition.Voices);
            
            var secondDefinition = result.VoiceDefinitions[1];
            Assert.Equal("male_02", secondDefinition.Name);
            Assert.Equal("human", secondDefinition.SoundAndCollisionInfoClass);
            Assert.Equal("false", secondDefinition.OnlyForNpcs);
            Assert.Equal("0.8", secondDefinition.MinPitchMultiplier);
            Assert.Equal("1.2", secondDefinition.MaxPitchMultiplier);
            Assert.Single(secondDefinition.Voices);
        }
    }
}