using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Configuration;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ManagedCoreParametersXmlTests
    {
        [Fact]
        public void ManagedCoreParameters_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
	type=""combat_parameters"">
	<managed_core_parameters>
		<managed_core_parameter
			id=""EnableCampaignTutorials""
			value=""1"" />
		<managed_core_parameter
			id=""AirFrictionJavelin""
			value=""0.002"" />
		<managed_core_parameter
			id=""AirFrictionArrow""
			value=""0.003"" />
		<managed_core_parameter
			id=""HeavyAttackMomentumMultiplier""
			value=""1.15"" />
		<managed_core_parameter
			id=""BipedalRadius""
			value=""0.38"" />
	</managed_core_parameters>
</base>";

            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));

            // Act
            ManagedCoreParametersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ManagedCoreParametersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("combat_parameters", result.Type);
            Assert.NotNull(result.ManagedCoreParameters);
            Assert.NotNull(result.ManagedCoreParameters.ManagedCoreParameter);
            Assert.Equal(5, result.ManagedCoreParameters.ManagedCoreParameter.Length);

            // Test EnableCampaignTutorials
            var enableTutorials = result.ManagedCoreParameters.ManagedCoreParameter[0];
            Assert.Equal("EnableCampaignTutorials", enableTutorials.Id);
            Assert.Equal("1", enableTutorials.Value);

            // Test AirFrictionJavelin
            var airFrictionJavelin = result.ManagedCoreParameters.ManagedCoreParameter[1];
            Assert.Equal("AirFrictionJavelin", airFrictionJavelin.Id);
            Assert.Equal("0.002", airFrictionJavelin.Value);

            // Test BipedalRadius
            var bipedalRadius = result.ManagedCoreParameters.ManagedCoreParameter[4];
            Assert.Equal("BipedalRadius", bipedalRadius.Id);
            Assert.Equal("0.38", bipedalRadius.Value);
        }

        [Fact]
        public void ManagedCoreParameters_WithCombatParameters_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""combat_parameters"">
	<managed_core_parameters>
		<managed_core_parameter
			id=""DamageInterruptAttackthresholdPierce""
			value=""5.0"" />
		<managed_core_parameter
			id=""FallDamageMultiplier""
			value=""0.575"" />
		<managed_core_parameter
			id=""FistFightDamageMultiplier""
			value=""4.0"" />
	</managed_core_parameters>
</base>";

            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));

            // Act
            ManagedCoreParametersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ManagedCoreParametersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("combat_parameters", result.Type);
            Assert.NotNull(result.ManagedCoreParameters);
            Assert.NotNull(result.ManagedCoreParameters.ManagedCoreParameter);
            Assert.Equal(3, result.ManagedCoreParameters.ManagedCoreParameter.Length);

            var damageThresholdPierce = result.ManagedCoreParameters.ManagedCoreParameter[0];
            Assert.Equal("DamageInterruptAttackthresholdPierce", damageThresholdPierce.Id);
            Assert.Equal("5.0", damageThresholdPierce.Value);

            var fallDamageMultiplier = result.ManagedCoreParameters.ManagedCoreParameter[1];
            Assert.Equal("FallDamageMultiplier", fallDamageMultiplier.Id);
            Assert.Equal("0.575", fallDamageMultiplier.Value);

            var fistFightMultiplier = result.ManagedCoreParameters.ManagedCoreParameter[2];
            Assert.Equal("FistFightDamageMultiplier", fistFightMultiplier.Id);
            Assert.Equal("4.0", fistFightMultiplier.Value);
        }

        [Fact]
        public void ManagedCoreParameters_WithStunParameters_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""combat_parameters"">
	<managed_core_parameters>
		<managed_core_parameter
			id=""StunPeriodAttackerSwing""
			value=""0.1"" />
		<managed_core_parameter
			id=""StunPeriodAttackerThrust""
			value=""0.67"" />
		<managed_core_parameter
			id=""StunMomentumTransferFactor""
			value=""0.0055"" />
		<managed_core_parameter
			id=""StunPeriodMax""
			value=""0.8"" />
	</managed_core_parameters>
</base>";

            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));

            // Act
            ManagedCoreParametersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ManagedCoreParametersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ManagedCoreParameters);
            Assert.NotNull(result.ManagedCoreParameters.ManagedCoreParameter);
            Assert.Equal(4, result.ManagedCoreParameters.ManagedCoreParameter.Length);

            var stunSwing = result.ManagedCoreParameters.ManagedCoreParameter[0];
            Assert.Equal("StunPeriodAttackerSwing", stunSwing.Id);
            Assert.Equal("0.1", stunSwing.Value);

            var stunMax = result.ManagedCoreParameters.ManagedCoreParameter[3];
            Assert.Equal("StunPeriodMax", stunMax.Id);
            Assert.Equal("0.8", stunMax.Value);
        }

        [Fact]
        public void ManagedCoreParameters_CanSerializeToXml()
        {
            // Arrange
            var managedCoreParameters = new ManagedCoreParametersRoot
            {
                Type = "combat_parameters",
                ManagedCoreParameters = new ManagedCoreParameters
                {
                    ManagedCoreParameter = new[]
                    {
                        new ManagedCoreParameter
                        {
                            Id = "TestParameter1",
                            Value = "1.0"
                        },
                        new ManagedCoreParameter
                        {
                            Id = "TestParameter2",
                            Value = "0.5"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));

            // Act
            string result;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, managedCoreParameters);
                result = writer.ToString();
            }

            // Assert
            Assert.Contains("type=\"combat_parameters\"", result);
            Assert.Contains("id=\"TestParameter1\"", result);
            Assert.Contains("value=\"1.0\"", result);
            Assert.Contains("id=\"TestParameter2\"", result);
            Assert.Contains("value=\"0.5\"", result);
        }

        [Fact]
        public void ManagedCoreParameters_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var original = new ManagedCoreParametersRoot
            {
                Type = "combat_parameters",
                ManagedCoreParameters = new ManagedCoreParameters
                {
                    ManagedCoreParameter = new[]
                    {
                        new ManagedCoreParameter
                        {
                            Id = "RoundtripTestParameter",
                            Value = "2.75"
                        }
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));

            // Act - Serialize and then deserialize
            string xmlContent;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                xmlContent = writer.ToString();
            }

            ManagedCoreParametersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ManagedCoreParametersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("combat_parameters", result.Type);
            Assert.NotNull(result.ManagedCoreParameters);
            Assert.NotNull(result.ManagedCoreParameters.ManagedCoreParameter);
            Assert.Single(result.ManagedCoreParameters.ManagedCoreParameter);

            var parameter = result.ManagedCoreParameters.ManagedCoreParameter[0];
            Assert.Equal("RoundtripTestParameter", parameter.Id);
            Assert.Equal("2.75", parameter.Value);
        }

        [Fact]
        public void ManagedCoreParameters_EmptyFile_HandlesGracefully()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base type=""combat_parameters"">
	<managed_core_parameters>
	</managed_core_parameters>
</base>";

            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));

            // Act
            ManagedCoreParametersRoot? result;
            using (var reader = new StringReader(xmlContent))
            {
                result = (ManagedCoreParametersRoot?)serializer.Deserialize(reader);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("combat_parameters", result.Type);
            Assert.NotNull(result.ManagedCoreParameters);
        }
    }
} 