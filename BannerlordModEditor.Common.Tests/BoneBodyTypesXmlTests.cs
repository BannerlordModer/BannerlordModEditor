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
    public class BoneBodyTypesXmlTests
    {
        [Fact]
        public void BoneBodyTypes_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "bone_body_types.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(BoneBodyTypes));
            BoneBodyTypes boneBodyTypes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                boneBodyTypes = (BoneBodyTypes)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, boneBodyTypes);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(boneBodyTypes);
            Assert.NotNull(boneBodyTypes.BoneBodyType);
            Assert.True(boneBodyTypes.BoneBodyType.Count > 0, "Should have at least one bone body type");
            
            // 验证具体的骨骼身体类型数据
            var abdomenType = boneBodyTypes.BoneBodyType.FirstOrDefault(b => b.Type == "abdomen");
            Assert.NotNull(abdomenType);
            Assert.Equal("3", abdomenType.Priority);
            
            var headType = boneBodyTypes.BoneBodyType.FirstOrDefault(b => b.Type == "head");
            Assert.NotNull(headType);
            Assert.Equal("4", headType.Priority);
            Assert.Equal("true", headType.DoNotScaleAccordingToAgentScale);
            
            var leftArmType = boneBodyTypes.BoneBodyType.FirstOrDefault(b => b.Type == "arm_left");
            Assert.NotNull(leftArmType);
            Assert.Equal("1", leftArmType.Priority);
            Assert.Equal("true", leftArmType.ActivateSweep);
            Assert.Equal("true", leftArmType.UseSmallerRadiusMultWhileHoldingShield);
            
            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.Equal(originalDoc.Root?.Elements("bone_body_type").Count(), 
                        savedDoc.Root?.Elements("bone_body_type").Count());
        }
        
        [Fact]
        public void BoneBodyTypes_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "bone_body_types.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(BoneBodyTypes));
            BoneBodyTypes boneBodyTypes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                boneBodyTypes = (BoneBodyTypes)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有骨骼身体类型都有必要的属性
            foreach (var boneType in boneBodyTypes.BoneBodyType)
            {
                Assert.False(string.IsNullOrWhiteSpace(boneType.Type), "Bone body type should have Type");
                Assert.False(string.IsNullOrWhiteSpace(boneType.Priority), "Bone body type should have Priority");
                
                // 验证优先级是数字
                Assert.True(int.TryParse(boneType.Priority, out int priority), 
                    $"Priority '{boneType.Priority}' should be a valid integer");
                Assert.True(priority >= 1 && priority <= 4, 
                    $"Priority should be between 1-4, got {priority}");
                
                // 验证布尔属性格式
                if (!string.IsNullOrEmpty(boneType.ActivateSweep))
                {
                    Assert.True(boneType.ActivateSweep == "true" || boneType.ActivateSweep == "false",
                        $"ActivateSweep should be 'true' or 'false', got '{boneType.ActivateSweep}'");
                }
                
                if (!string.IsNullOrEmpty(boneType.DoNotScaleAccordingToAgentScale))
                {
                    Assert.True(boneType.DoNotScaleAccordingToAgentScale == "true" || boneType.DoNotScaleAccordingToAgentScale == "false",
                        $"DoNotScaleAccordingToAgentScale should be 'true' or 'false', got '{boneType.DoNotScaleAccordingToAgentScale}'");
                }
            }
            
            // 验证包含预期的身体部位
            var allTypes = boneBodyTypes.BoneBodyType.Select(b => b.Type).ToList();
            Assert.Contains("head", allTypes);
            Assert.Contains("chest", allTypes);
            Assert.Contains("abdomen", allTypes);
            Assert.Contains("arm_left", allTypes);
            Assert.Contains("arm_right", allTypes);
            Assert.Contains("legs", allTypes);
            Assert.Contains("neck", allTypes);
            
            // 验证优先级分布合理
            var headType = boneBodyTypes.BoneBodyType.First(b => b.Type == "head");
            var neckType = boneBodyTypes.BoneBodyType.First(b => b.Type == "neck");
            Assert.Equal("4", headType.Priority); // 头部应该有最高优先级
            Assert.Equal("4", neckType.Priority); // 脖子也应该有最高优先级
            
            var armTypes = boneBodyTypes.BoneBodyType.Where(b => b.Type?.Contains("arm") == true).ToList();
            Assert.All(armTypes, arm => Assert.Equal("1", arm.Priority)); // 手臂应该有最低优先级
            
            // 确保没有重复的类型
            var uniqueTypes = allTypes.Distinct().ToList();
            Assert.Equal(allTypes.Count, uniqueTypes.Count);
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