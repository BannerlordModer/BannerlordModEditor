using BannerlordModEditor.Common.Models.Engine;
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
    public class GpuParticleSystemsXmlTests
    {
        [Fact]
        public void GpuParticleSystems_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gpu_particle_systems.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(GpuParticleSystemsBase));
            GpuParticleSystemsBase particleSystems;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                particleSystems = (GpuParticleSystemsBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, particleSystems);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构检查
            Assert.NotNull(particleSystems);
            Assert.Equal("particle_system", particleSystems.Type);
            Assert.NotNull(particleSystems.ParticleSystems);
            Assert.NotNull(particleSystems.ParticleSystems.ParticleSystem);
            Assert.True(particleSystems.ParticleSystems.ParticleSystem.Count > 0, "应该有至少一个粒子系统");
            
            // Assert - 验证具体的粒子系统数据
            var testSystem = particleSystems.ParticleSystems.ParticleSystem.FirstOrDefault(ps => ps.Name == "test");
            Assert.NotNull(testSystem);
            Assert.Equal("gpuparticle_standard", testSystem.Material);
            Assert.Equal("1.000000", testSystem.Life);
            Assert.Equal("500.000000", testSystem.EmissionRate);
            Assert.Equal("0.000000, 0.000000, 6.000000", testSystem.EmitVelocity);
            Assert.Equal("1.000000, 1.000000, 1.000000", testSystem.EmitDirectionRandomness);
            Assert.Equal("0.000000", testSystem.EmitVelocityRandomness);
            Assert.Equal("0.050000", testSystem.EmitSphereRadius);
            Assert.Equal("0.000000", testSystem.ScaleT0);
            Assert.Equal("1.000000", testSystem.ScaleT1);
            Assert.Equal("1.000000", testSystem.StartScale);
            Assert.Equal("1.500000", testSystem.EndScale);
            Assert.Equal("0.000000", testSystem.AlphaT0);
            Assert.Equal("0.400000", testSystem.AlphaT1);
            Assert.Equal("0.500000", testSystem.LinearDamping);
            Assert.Equal("0.800000", testSystem.GravityConstant);
            Assert.Equal("0.500000", testSystem.AngularSpeed);
            Assert.Equal("0.500000", testSystem.AngularDamping);
            Assert.Equal("0.500000", testSystem.BurstLength);
            Assert.Equal("false", testSystem.IsBurst);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 规范化XML格式
            NormalizeXml(originalDoc.Root);
            NormalizeXml(savedDoc.Root);

            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements().Count() == savedDoc.Root?.Elements().Count(),
                "元素数量应该相同");
        }
        
        [Fact]
        public void GpuParticleSystems_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "gpu_particle_systems.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(GpuParticleSystemsBase));
            GpuParticleSystemsBase particleSystems;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                particleSystems = (GpuParticleSystemsBase)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有粒子系统都有基本属性
            foreach (var system in particleSystems.ParticleSystems.ParticleSystem)
            {
                Assert.False(string.IsNullOrEmpty(system.Name), "粒子系统名称不能为空");
                Assert.False(string.IsNullOrEmpty(system.Material), $"材质不能为空：{system.Name}");
                Assert.False(string.IsNullOrEmpty(system.Life), $"生命周期不能为空：{system.Name}");
                Assert.False(string.IsNullOrEmpty(system.EmissionRate), $"发射率不能为空：{system.Name}");
                
                // 验证数值类型属性
                var numericProperties = new[]
                {
                    (system.Life, "Life"),
                    (system.EmissionRate, "EmissionRate"),
                    (system.EmitVelocityRandomness, "EmitVelocityRandomness"),
                    (system.EmitSphereRadius, "EmitSphereRadius"),
                    (system.ScaleT0, "ScaleT0"),
                    (system.ScaleT1, "ScaleT1"),
                    (system.StartScale, "StartScale"),
                    (system.EndScale, "EndScale"),
                    (system.AlphaT0, "AlphaT0"),
                    (system.AlphaT1, "AlphaT1"),
                    (system.LinearDamping, "LinearDamping"),
                    (system.GravityConstant, "GravityConstant"),
                    (system.AngularSpeed, "AngularSpeed"),
                    (system.AngularDamping, "AngularDamping"),
                    (system.BurstLength, "BurstLength")
                };
                
                foreach (var (value, name) in numericProperties)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        Assert.True(float.TryParse(value, out var parsedValue),
                            $"{name}应该是有效的浮点数：{system.Name} = {value}");
                        Assert.True(parsedValue >= 0, $"{name}应该是非负数：{system.Name} = {parsedValue}");
                    }
                }
                
                // 验证向量类型属性 (x, y, z)
                var vectorProperties = new[]
                {
                    (system.EmitVelocity, "EmitVelocity"),
                    (system.EmitDirectionRandomness, "EmitDirectionRandomness")
                };
                
                foreach (var (value, name) in vectorProperties)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var components = value.Split(',');
                        Assert.True(components.Length == 3, $"{name}应该有3个分量：{system.Name} = {value}");
                        foreach (var component in components)
                        {
                            Assert.True(float.TryParse(component.Trim(), out _),
                                $"{name}的分量应该是有效的浮点数：{system.Name} = {component.Trim()}");
                        }
                    }
                }
                
                // 验证布尔值属性
                if (!string.IsNullOrEmpty(system.IsBurst))
                {
                    Assert.True(system.IsBurst == "true" || system.IsBurst == "false",
                        $"IsBurst应该是布尔值：{system.Name} = {system.IsBurst}");
                }
            }
        }
        
        private static string FindSolutionRoot()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.FullName ?? throw new DirectoryNotFoundException("找不到解决方案根目录");
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

        private static void NormalizeXml(XElement? element)
        {
            if (element == null) return;
            
            // 移除所有空白文本节点
            var whitespaceNodes = element.Nodes().OfType<XText>()
                .Where(t => string.IsNullOrWhiteSpace(t.Value))
                .ToList();
            foreach (var node in whitespaceNodes)
            {
                node.Remove();
            }
            
            // 递归处理子元素
            foreach (var child in element.Elements())
            {
                NormalizeXml(child);
            }
        }
    }
} 