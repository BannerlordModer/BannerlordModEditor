using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Audio;
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
		<voice_type name=""Jump"" />
		<voice_type name=""Yell"" />
		<voice_type name=""Pain"" />
		<voice_type name=""Death"" />
	</voice_type_declarations>
	<voice_definition
		name=""male_01""
		sound_and_collision_info_class=""human""
		only_for_npcs=""true""
		min_pitch_multiplier=""0.9""
		max_pitch_multiplier=""1.1"">
		<voice
			type=""Grunt""
			path=""event:/voice/combat/male/01/grunt""
			face_anim=""grunt"" />
		<voice
			type=""Jump""
			path=""event:/voice/combat/male/01/jump""
			face_anim=""scream"" />
		<voice
			type=""Pain""
			path=""event:/voice/combat/male/01/pain""
			face_anim=""hit"" />
	</voice_definition>
	<voice_definition
		name=""horse_01""
		sound_and_collision_info_class=""horse""
		min_pitch_multiplier=""0.95""
		max_pitch_multiplier=""1.05"">
		<voice
			type=""Neigh""
			path=""event:/voice/combat/horse/01/neigh"" />
		<voice
			type=""Pain""
			path=""event:/voice/combat/horse/01/pain"" />
	</voice_definition>
</voice_definitions>";

            var serializer = new XmlSerializer(typeof(VoiceDefinitions));

            // Act
            VoiceDefinitions? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (VoiceDefinitions?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.VoiceTypeDeclarations);
            Assert.NotNull(result.VoiceTypeDeclarations.VoiceType);
            Assert.Equal(5, result.VoiceTypeDeclarations.VoiceType.Length);

            // Test voice type declarations
            var gruntType = result.VoiceTypeDeclarations.VoiceType[0];
            Assert.Equal("Grunt", gruntType.Name);
            
            var jumpType = result.VoiceTypeDeclarations.VoiceType[1];
            Assert.Equal("Jump", jumpType.Name);

            // Test voice definitions
            Assert.NotNull(result.VoiceDefinition);
            Assert.Equal(2, result.VoiceDefinition.Length);

            var maleVoice = result.VoiceDefinition[0];
            Assert.Equal("male_01", maleVoice.Name);
            Assert.Equal("human", maleVoice.SoundAndCollisionInfoClass);
            Assert.Equal("true", maleVoice.OnlyForNpcs);
            Assert.Equal("0.9", maleVoice.MinPitchMultiplier);
            Assert.Equal("1.1", maleVoice.MaxPitchMultiplier);

            // Test individual voices
            Assert.NotNull(maleVoice.Voice);
            Assert.Equal(3, maleVoice.Voice.Length);

            var gruntVoice = maleVoice.Voice[0];
            Assert.Equal("Grunt", gruntVoice.Type);
            Assert.Equal("event:/voice/combat/male/01/grunt", gruntVoice.Path);
            Assert.Equal("grunt", gruntVoice.FaceAnim);

            var jumpVoice = maleVoice.Voice[1];
            Assert.Equal("Jump", jumpVoice.Type);
            Assert.Equal("event:/voice/combat/male/01/jump", jumpVoice.Path);
            Assert.Equal("scream", jumpVoice.FaceAnim);

            // Test horse voice (without only_for_npcs)
            var horseVoice = result.VoiceDefinition[1];
            Assert.Equal("horse_01", horseVoice.Name);
            Assert.Equal("horse", horseVoice.SoundAndCollisionInfoClass);
            Assert.Null(horseVoice.OnlyForNpcs);
            Assert.Equal("0.95", horseVoice.MinPitchMultiplier);
            Assert.Equal("1.05", horseVoice.MaxPitchMultiplier);

            Assert.NotNull(horseVoice.Voice);
            Assert.Equal(2, horseVoice.Voice.Length);

            var neighVoice = horseVoice.Voice[0];
            Assert.Equal("Neigh", neighVoice.Type);
            Assert.Equal("event:/voice/combat/horse/01/neigh", neighVoice.Path);
            Assert.Null(neighVoice.FaceAnim);
        }

        [Fact]
        public void VoiceDefinitions_WithoutFaceAnim_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
	<voice_type_declarations>
		<voice_type name=""Idle"" />
	</voice_type_declarations>
	<voice_definition
		name=""sheep_01""
		sound_and_collision_info_class=""ovine"">
		<voice
			type=""Pain""
			path=""event:/mission/movement/foley/animals/sheep/pain"" />
		<voice
			type=""Idle""
			path=""event:/mission/movement/foley/animals/sheep/idle"" />
	</voice_definition>
</voice_definitions>";

            var serializer = new XmlSerializer(typeof(VoiceDefinitions));

            // Act
            VoiceDefinitions? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (VoiceDefinitions?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.VoiceDefinition);
            Assert.Single(result.VoiceDefinition);

            var sheepVoice = result.VoiceDefinition[0];
            Assert.Equal("sheep_01", sheepVoice.Name);
            Assert.Equal("ovine", sheepVoice.SoundAndCollisionInfoClass);
            Assert.Null(sheepVoice.OnlyForNpcs);
            Assert.Null(sheepVoice.MinPitchMultiplier);
            Assert.Null(sheepVoice.MaxPitchMultiplier);

            Assert.NotNull(sheepVoice.Voice);
            Assert.Equal(2, sheepVoice.Voice.Length);

            var painVoice = sheepVoice.Voice[0];
            Assert.Equal("Pain", painVoice.Type);
            Assert.Equal("event:/mission/movement/foley/animals/sheep/pain", painVoice.Path);
            Assert.Null(painVoice.FaceAnim);
        }

        [Fact]
        public void VoiceDefinitions_CanSerializeToXml()
        {
            // Arrange
            var voiceDefinitions = new VoiceDefinitions
            {
                VoiceTypeDeclarations = new VoiceTypeDeclarations
                {
                    VoiceType = new[]
                    {
                        new VoiceType { Name = "TestGrunt" },
                        new VoiceType { Name = "TestPain" }
                    }
                },
                VoiceDefinition = new[]
                {
                    new VoiceDefinition
                    {
                        Name = "test_voice",
                        SoundAndCollisionInfoClass = "human",
                        OnlyForNpcs = "false",
                        MinPitchMultiplier = "0.8",
                        MaxPitchMultiplier = "1.2",
                        Voice = new[]
                        {
                            new Voice
                            {
                                Type = "TestGrunt",
                                Path = "event:/test/grunt",
                                FaceAnim = "test_grunt"
                            },
                            new Voice
                            {
                                Type = "TestPain",
                                Path = "event:/test/pain",
                                FaceAnim = "test_pain"
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(VoiceDefinitions));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, voiceDefinitions);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("name=\"TestGrunt\"", result);
            Assert.Contains("name=\"test_voice\"", result);
            Assert.Contains("sound_and_collision_info_class=\"human\"", result);
            Assert.Contains("only_for_npcs=\"false\"", result);
            Assert.Contains("min_pitch_multiplier=\"0.8\"", result);
            Assert.Contains("type=\"TestGrunt\"", result);
            Assert.Contains("path=\"event:/test/grunt\"", result);
            Assert.Contains("face_anim=\"test_grunt\"", result);
        }

        [Fact]
        public void VoiceDefinitions_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new VoiceDefinitions
            {
                VoiceTypeDeclarations = new VoiceTypeDeclarations
                {
                    VoiceType = new[]
                    {
                        new VoiceType { Name = "RoundTripTest" }
                    }
                },
                VoiceDefinition = new[]
                {
                    new VoiceDefinition
                    {
                        Name = "roundtrip_test",
                        SoundAndCollisionInfoClass = "test_class",
                        MinPitchMultiplier = "0.95",
                        MaxPitchMultiplier = "1.05",
                        Voice = new[]
                        {
                            new Voice
                            {
                                Type = "RoundTripTest",
                                Path = "event:/test/roundtrip",
                                FaceAnim = "test_anim"
                            }
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(VoiceDefinitions));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            VoiceDefinitions? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (VoiceDefinitions?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.VoiceTypeDeclarations);
            Assert.NotNull(result.VoiceTypeDeclarations.VoiceType);
            Assert.Single(result.VoiceTypeDeclarations.VoiceType);
            Assert.Equal("RoundTripTest", result.VoiceTypeDeclarations.VoiceType[0].Name);

            Assert.NotNull(result.VoiceDefinition);
            Assert.Single(result.VoiceDefinition);

            var voiceDef = result.VoiceDefinition[0];
            Assert.Equal("roundtrip_test", voiceDef.Name);
            Assert.Equal("test_class", voiceDef.SoundAndCollisionInfoClass);
            Assert.Equal("0.95", voiceDef.MinPitchMultiplier);
            Assert.Equal("1.05", voiceDef.MaxPitchMultiplier);

            Assert.NotNull(voiceDef.Voice);
            Assert.Single(voiceDef.Voice);

            var voice = voiceDef.Voice[0];
            Assert.Equal("RoundTripTest", voice.Type);
            Assert.Equal("event:/test/roundtrip", voice.Path);
            Assert.Equal("test_anim", voice.FaceAnim);
        }

        [Fact]
        public void VoiceDefinitions_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
	<voice_type_declarations>
	</voice_type_declarations>
</voice_definitions>";

            var serializer = new XmlSerializer(typeof(VoiceDefinitions));

            // Act
            VoiceDefinitions? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (VoiceDefinitions?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.VoiceTypeDeclarations);
        }
    }
} 