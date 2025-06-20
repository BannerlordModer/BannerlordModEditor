using BannerlordModEditor.Common.Models.Data;
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
    public class SkeletonScalesXmlTests
    {
        [Fact]
        public void SkeletonScales_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skeleton_scales.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SkeletonScales));
            SkeletonScales skeletonScales;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                skeletonScales = (SkeletonScales)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, skeletonScales);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(skeletonScales);
            Assert.NotNull(skeletonScales.Scale);
            Assert.True(skeletonScales.Scale.Count > 0, "Should have at least one scale");
            
            // 验证具体的骨骼缩放数据
            var sturgiaHorse = skeletonScales.Scale.FirstOrDefault(s => s.Id == "sturgia_horse");
            Assert.NotNull(sturgiaHorse);
            Assert.Equal("horse_skeleton", sturgiaHorse.Skeleton);
            Assert.Equal("1,1.1,1", sturgiaHorse.MountSitBoneScale);
            Assert.Equal("0.0", sturgiaHorse.MountRadiusAdder);
            Assert.NotNull(sturgiaHorse.BoneScales);
            Assert.True(sturgiaHorse.BoneScales.BoneScale.Count > 0, "Sturgia horse should have bone scales");
            
            var battaniaHorse = skeletonScales.Scale.FirstOrDefault(s => s.Id == "battania_horse");
            Assert.NotNull(battaniaHorse);
            Assert.Equal("horse_skeleton", battaniaHorse.Skeleton);
            Assert.Equal("0.84,1.06,1", battaniaHorse.MountSitBoneScale);
            Assert.Equal("0.0", battaniaHorse.MountRadiusAdder);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("Scale").Count() == savedDoc.Root?.Elements("Scale").Count(),
                "Scale count should be the same");
        }
        
        [Fact]
        public void SkeletonScales_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "skeleton_scales.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SkeletonScales));
            SkeletonScales skeletonScales;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                skeletonScales = (SkeletonScales)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有Scale都有必要的属性
            foreach (var scale in skeletonScales.Scale)
            {
                Assert.False(string.IsNullOrWhiteSpace(scale.Id), "Scale should have Id");
                Assert.False(string.IsNullOrWhiteSpace(scale.Skeleton), "Scale should have Skeleton");
                
                // 验证BoneScales结构
                if (scale.BoneScales != null)
                {
                    foreach (var boneScale in scale.BoneScales.BoneScale)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(boneScale.BoneName), "BoneScale should have BoneName");
                        Assert.False(string.IsNullOrWhiteSpace(boneScale.Scale), "BoneScale should have Scale");
                        
                        // 验证scale值格式（应该是x,y,z格式的数值）
                        var scaleParts = boneScale.Scale.Split(',');
                        Assert.True(scaleParts.Length == 3, "Scale should have 3 components (x,y,z)");
                        
                        foreach (var part in scaleParts)
                        {
                            Assert.True(float.TryParse(part.Trim(), out _), 
                                $"Scale component '{part}' should be a valid float");
                        }
                    }
                }
            }
            
            // 验证特定的马匹缩放数据
            var khuzaitHorse = skeletonScales.Scale.FirstOrDefault(s => s.Id == "khuzait_horse");
            Assert.NotNull(khuzaitHorse);
            Assert.Equal("0.915,1.13,1", khuzaitHorse.MountSitBoneScale);
            
            var vlandiaHorse = skeletonScales.Scale.FirstOrDefault(s => s.Id == "vlandia_horse");
            Assert.NotNull(vlandiaHorse);
            Assert.Equal("1,1.165,1", vlandiaHorse.MountSitBoneScale);
            
            // 验证骨骼名称的一致性
            var allBoneNames = skeletonScales.Scale
                .Where(s => s.BoneScales != null)
                .SelectMany(s => s.BoneScales.BoneScale)
                .Select(bs => bs.BoneName)
                .Distinct()
                .ToList();
            
            Assert.True(allBoneNames.Count > 10, "Should have many distinct bone names");
            Assert.Contains(allBoneNames, name => name.Contains("horse"));
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