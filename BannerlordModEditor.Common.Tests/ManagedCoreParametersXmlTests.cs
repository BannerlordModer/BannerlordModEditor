using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Configuration;
using Xunit;
using System.Globalization;
using System.Linq;

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
        public void ManagedCoreParameters_FromActualFile_CanDeserializeCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));
            ManagedCoreParametersRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("combat_parameters", result.Type);
            Assert.NotNull(result.ManagedCoreParameters);
            Assert.NotNull(result.ManagedCoreParameters.ManagedCoreParameter);
            
            // 验证参数数量大于50个（实际有60+个参数）
            Assert.True(result.ManagedCoreParameters.ManagedCoreParameter.Length >= 50);
            
            // 验证所有参数都有有效的ID和值
            foreach (var param in result.ManagedCoreParameters.ManagedCoreParameter)
            {
                Assert.False(string.IsNullOrWhiteSpace(param.Id));
                Assert.False(string.IsNullOrWhiteSpace(param.Value));
            }
            
            // 验证特定的重要参数存在
            var paramDict = result.ManagedCoreParameters.ManagedCoreParameter.ToDictionary(p => p.Id, p => p.Value);
            
            // 验证教程参数
            Assert.True(paramDict.ContainsKey("EnableCampaignTutorials"));
            Assert.True(paramDict["EnableCampaignTutorials"] == "0" || paramDict["EnableCampaignTutorials"] == "1");
            
            // 验证空气阻力参数
            Assert.True(paramDict.ContainsKey("AirFrictionArrow"));
            Assert.True(paramDict.ContainsKey("AirFrictionJavelin"));
            Assert.True(paramDict.ContainsKey("AirFrictionBallistaBolt"));
            Assert.True(paramDict.ContainsKey("AirFrictionBullet"));
            
            // 验证数值范围合理性
            Assert.True(double.Parse(paramDict["AirFrictionArrow"], CultureInfo.InvariantCulture) > 0);
            Assert.True(double.Parse(paramDict["AirFrictionJavelin"], CultureInfo.InvariantCulture) > 0);
            
            // 验证半径参数
            Assert.True(paramDict.ContainsKey("BipedalRadius"));
            Assert.True(paramDict.ContainsKey("QuadrupedalRadius"));
            var bipedalRadius = double.Parse(paramDict["BipedalRadius"], CultureInfo.InvariantCulture);
            var quadrupedalRadius = double.Parse(paramDict["QuadrupedalRadius"], CultureInfo.InvariantCulture);
            Assert.True(bipedalRadius > 0 && bipedalRadius < 1); // 应该是合理的人类半径
            Assert.True(quadrupedalRadius > bipedalRadius); // 四足动物半径应该更大
            
            // 验证伤害相关参数
            Assert.True(paramDict.ContainsKey("FallDamageMultiplier"));
            Assert.True(paramDict.ContainsKey("FistFightDamageMultiplier"));
            var fallDamageMultiplier = double.Parse(paramDict["FallDamageMultiplier"], CultureInfo.InvariantCulture);
            var fistFightMultiplier = double.Parse(paramDict["FistFightDamageMultiplier"], CultureInfo.InvariantCulture);
            Assert.True(fallDamageMultiplier > 0 && fallDamageMultiplier <= 1);
            Assert.True(fistFightMultiplier > 0);
            
            // 验证眩晕参数
            Assert.True(paramDict.ContainsKey("StunPeriodMax"));
            var stunPeriodMax = double.Parse(paramDict["StunPeriodMax"], CultureInfo.InvariantCulture);
            Assert.True(stunPeriodMax > 0 && stunPeriodMax <= 5); // 眩晕时间应该合理
        }
        
        [Fact]
        public void ManagedCoreParameters_AirFrictionParameters_HaveLogicalValues()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));
            ManagedCoreParametersRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            var paramDict = result.ManagedCoreParameters.ManagedCoreParameter.ToDictionary(p => p.Id, p => p.Value);
            
            // Assert - 验证空气阻力参数的逻辑关系
            var airFrictionArrow = double.Parse(paramDict["AirFrictionArrow"], CultureInfo.InvariantCulture);
            var airFrictionJavelin = double.Parse(paramDict["AirFrictionJavelin"], CultureInfo.InvariantCulture);
            var airFrictionKnife = double.Parse(paramDict["AirFrictionKnife"], CultureInfo.InvariantCulture);
            var airFrictionAxe = double.Parse(paramDict["AirFrictionAxe"], CultureInfo.InvariantCulture);
            
            // 箭矢应该比标枪有更高的空气阻力（更轻）
            Assert.True(airFrictionArrow > airFrictionJavelin);
            
            // 抛掷武器（刀和斧）应该有相对较高的空气阻力
            Assert.True(airFrictionKnife > airFrictionArrow);
            Assert.True(airFrictionAxe > airFrictionArrow);
            
            // 所有值都应该在合理范围内
            Assert.True(airFrictionArrow > 0 && airFrictionArrow < 0.1);
            Assert.True(airFrictionJavelin > 0 && airFrictionJavelin < 0.1);
        }
        
        [Fact]
        public void ManagedCoreParameters_DamageThresholdParameters_HaveConsistentValues()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));
            ManagedCoreParametersRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            var paramDict = result.ManagedCoreParameters.ManagedCoreParameter.ToDictionary(p => p.Id, p => p.Value);
            
            // Assert - 验证伤害阈值参数
            var pierceDamageThreshold = double.Parse(paramDict["DamageInterruptAttackthresholdPierce"], CultureInfo.InvariantCulture);
            var cutDamageThreshold = double.Parse(paramDict["DamageInterruptAttackthresholdCut"], CultureInfo.InvariantCulture);
            var bluntDamageThreshold = double.Parse(paramDict["DamageInterruptAttackthresholdBlunt"], CultureInfo.InvariantCulture);
            
            // 所有伤害阈值应该相等（根据XML内容）
            Assert.Equal(pierceDamageThreshold, cutDamageThreshold);
            Assert.Equal(cutDamageThreshold, bluntDamageThreshold);
            Assert.Equal(5.0, pierceDamageThreshold);
            
            // 验证其他伤害相关参数
            var rearAttackThreshold = double.Parse(paramDict["MakesRearAttackDamageThreshold"], CultureInfo.InvariantCulture);
            Assert.True(rearAttackThreshold > pierceDamageThreshold); // 背后攻击阈值应该更高
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
            
            // 验证XML格式正确
            Assert.Contains("<managed_core_parameters>", result);
            Assert.Contains("</managed_core_parameters>", result);
            Assert.Contains("<managed_core_parameter", result);
        }

        [Fact]
        public void ManagedCoreParameters_RoundTripSerialization_MaintainsData()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));
            ManagedCoreParametersRoot original;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                original = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string serializedXml;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, original);
                serializedXml = writer.ToString();
            }
            
            // Act - 重新反序列化
            ManagedCoreParametersRoot roundTripped;
            using (var reader = new StringReader(serializedXml))
            {
                roundTripped = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert
            Assert.Equal(original.Type, roundTripped.Type);
            Assert.Equal(original.ManagedCoreParameters.ManagedCoreParameter.Length, 
                        roundTripped.ManagedCoreParameters.ManagedCoreParameter.Length);
            
            // 验证每个参数都保持不变
            var originalDict = original.ManagedCoreParameters.ManagedCoreParameter.ToDictionary(p => p.Id, p => p.Value);
            var roundTrippedDict = roundTripped.ManagedCoreParameters.ManagedCoreParameter.ToDictionary(p => p.Id, p => p.Value);
            
            Assert.Equal(originalDict.Count, roundTrippedDict.Count);
            foreach (var kvp in originalDict)
            {
                Assert.True(roundTrippedDict.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, roundTrippedDict[kvp.Key]);
            }
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
            // 空的参数数组应该为null或空数组
            Assert.True(result.ManagedCoreParameters.ManagedCoreParameter == null || 
                       result.ManagedCoreParameters.ManagedCoreParameter.Length == 0);
        }
        
        [Fact]
        public void ManagedCoreParameters_ValidateParameterIdUniqueness()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));
            ManagedCoreParametersRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证参数ID的唯一性
            var ids = result.ManagedCoreParameters.ManagedCoreParameter.Select(p => p.Id).ToList();
            var uniqueIds = ids.Distinct().ToList();
            
            Assert.Equal(ids.Count, uniqueIds.Count); // 所有ID都应该是唯一的
        }
        
        [Fact]
        public void ManagedCoreParameters_ValidateNumericValuesCanBeParsed()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "managed_core_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ManagedCoreParametersRoot));
            ManagedCoreParametersRoot result;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                result = (ManagedCoreParametersRoot)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有数值都可以被解析
            foreach (var param in result.ManagedCoreParameters.ManagedCoreParameter)
            {
                // 尝试解析为双精度浮点数（使用不变文化）
                Assert.True(double.TryParse(param.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue),
                           $"Parameter '{param.Id}' has invalid numeric value: '{param.Value}'");
                           
                // 验证解析的值不是NaN或无穷大
                Assert.False(double.IsNaN(parsedValue), $"Parameter '{param.Id}' parsed to NaN");
                Assert.False(double.IsInfinity(parsedValue), $"Parameter '{param.Id}' parsed to Infinity");
            }
        }
    }
} 