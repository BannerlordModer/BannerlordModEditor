using BannerlordModEditor.Common.Models.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class NativeParametersXmlTests
    {
        [Fact]
        public void NativeParameters_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "example", "ModuleData", "native_parameters.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(NativeParametersXml));
            NativeParametersXml nativeParametersXml;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                nativeParametersXml = (NativeParametersXml)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    Encoding = new UTF8Encoding(false),
                    OmitXmlDeclaration = false
                }))
                {
                    serializer.Serialize(xmlWriter, nativeParametersXml);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(nativeParametersXml);
            Assert.Equal("combat_parameters", nativeParametersXml.Type);
            Assert.NotNull(nativeParametersXml.NativeParameters);
            Assert.NotNull(nativeParametersXml.NativeParameters.NativeParameter);
            Assert.True(nativeParametersXml.NativeParameters.NativeParameter.Count > 150, 
                $"Should have many native parameters, got {nativeParametersXml.NativeParameters.NativeParameter.Count}");
            
            // 验证具体的原生参数数据
            var maxPassableSlope = nativeParametersXml.NativeParameters.NativeParameter
                .FirstOrDefault(p => p.Id == "maximum_passable_slope");
            Assert.NotNull(maxPassableSlope);
            Assert.Equal("40.0", maxPassableSlope.Value);
            
            var jumpCooldown = nativeParametersXml.NativeParameters.NativeParameter
                .FirstOrDefault(p => p.Id == "jump_cooldown");
            Assert.NotNull(jumpCooldown);
            Assert.Equal("1.6", jumpCooldown.Value);
            
            var bipedSpeedMultiplier = nativeParametersXml.NativeParameters.NativeParameter
                .FirstOrDefault(p => p.Id == "bipedal_speed_multiplier");
            Assert.NotNull(bipedSpeedMultiplier);
            Assert.Equal("6.2", bipedSpeedMultiplier.Value);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Element("native_parameters")?.Elements("native_parameter").Count(), 
                        savedDoc.Root?.Element("native_parameters")?.Elements("native_parameter").Count());
        }
        
        [Fact]
        public void NativeParameters_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "example", "ModuleData", "native_parameters.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(NativeParametersXml));
            NativeParametersXml nativeParametersXml;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                nativeParametersXml = (NativeParametersXml)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有原生参数都有必要的属性
            Assert.NotNull(nativeParametersXml.NativeParameters);
            Assert.NotNull(nativeParametersXml.NativeParameters.NativeParameter);
            
            foreach (var parameter in nativeParametersXml.NativeParameters.NativeParameter)
            {
                Assert.False(string.IsNullOrWhiteSpace(parameter.Id), "Native parameter should have Id");
                Assert.False(string.IsNullOrWhiteSpace(parameter.Value), "Native parameter should have Value");
                
                // 验证值格式 - 大多数应该是有效的数字
                Assert.True(double.TryParse(parameter.Value, out double value), 
                    $"Parameter '{parameter.Id}' value '{parameter.Value}' should be a valid number");
            }
            
            // 验证包含预期的关键参数
            var allIds = nativeParametersXml.NativeParameters.NativeParameter.Select(p => p.Id).ToList();
            
            // 移动相关参数
            Assert.Contains("maximum_passable_slope", allIds);
            Assert.Contains("jump_cooldown", allIds);
            Assert.Contains("bipedal_speed_multiplier", allIds);
            Assert.Contains("running_backward_speed_multiplier", allIds);
            
            // 战斗相关参数
            Assert.Contains("max_logical_distance_allowed_for_defend", allIds);
            Assert.Contains("heavy_attack_momentum_multiplier", allIds);
            Assert.Contains("shield_hit_ik_rotation_multiplier", allIds);
            
            // 验证参数分类覆盖
            var movementParams = nativeParametersXml.NativeParameters.NativeParameter
                .Where(p => p.Id!.Contains("speed") || p.Id.Contains("jump") || p.Id.Contains("movement")).ToList();
            var combatParams = nativeParametersXml.NativeParameters.NativeParameter
                .Where(p => p.Id!.Contains("attack") || p.Id.Contains("defend") || p.Id.Contains("combat")).ToList();
            var shieldParams = nativeParametersXml.NativeParameters.NativeParameter
                .Where(p => p.Id!.Contains("shield")).ToList();
            
            Assert.True(movementParams.Count > 5, "Should have many movement-related parameters");
            Assert.True(combatParams.Count > 10, "Should have many combat-related parameters");  
            Assert.True(shieldParams.Count > 10, "Should have many shield-related parameters");
            
            // 确保没有重复的ID
            var uniqueIds = allIds.Distinct().ToList();
            Assert.Equal(allIds.Count, uniqueIds.Count);
            
            // 验证一些具体的数值范围
            var passableSlope = nativeParametersXml.NativeParameters.NativeParameter
                .First(p => p.Id == "maximum_passable_slope");
            var slopeValue = double.Parse(passableSlope.Value!);
            Assert.True(slopeValue > 0 && slopeValue < 90, "Passable slope should be a reasonable angle");
            
            var speedMultiplier = nativeParametersXml.NativeParameters.NativeParameter
                .First(p => p.Id == "bipedal_speed_multiplier");
            var speedValue = double.Parse(speedMultiplier.Value!);
            Assert.True(speedValue > 0, "Speed multiplier should be positive");
        }

        private static void RemoveWhitespaceNodes(XElement? element)
        {
            if (element == null) return;
            
            var textNodes = element.Nodes().OfType<XText>().Where(t => string.IsNullOrWhiteSpace(t.Value)).ToList();
            foreach (var node in textNodes)
            {
                node.Remove();
            }
            
            foreach (var child in element.Elements())
            {
                RemoveWhitespaceNodes(child);
            }
        }
    }
} 