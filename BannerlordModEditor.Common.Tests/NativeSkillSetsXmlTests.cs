using BannerlordModEditor.Common.Models.Data;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class NativeSkillSetsXmlTests
    {
        [Fact]
        public void NativeSkillSets_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "native_skill_sets.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(NativeSkillSets));
            NativeSkillSets nativeSkillSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                nativeSkillSets = (NativeSkillSets)serializer.Deserialize(reader);
            }
            
            // 验证反序列化的数据
            Assert.NotNull(nativeSkillSets);
            Assert.NotNull(nativeSkillSets.SkillSets);
            
            // 这个文件当前是空的，所以列表应该为空
            Assert.Empty(nativeSkillSets.SkillSets);
            
            // Act - 序列化回字符串
            string serializedXml;
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    OmitXmlDeclaration = false,
                    Encoding = Encoding.UTF8
                }))
                {
                    serializer.Serialize(xmlWriter, nativeSkillSets);
                }
                serializedXml = stringWriter.ToString();
            }
            
            // Assert - 结构化比较
            var originalXml = File.ReadAllText(xmlPath);
            
            var originalDoc = XDocument.Parse(originalXml);
            var serializedDoc = XDocument.Parse(serializedXml);
            
            // 验证根节点
            Assert.Equal(originalDoc.Root.Name.LocalName, serializedDoc.Root.Name.LocalName);
            
            // 验证空内容
            Assert.Empty(originalDoc.Root.Elements());
            Assert.Empty(serializedDoc.Root.Elements());
            
            // 验证序列化后的结构一致性
            Assert.Equal("SkillSets", serializedDoc.Root.Name.LocalName);
        }

        [Fact]
        public void NativeSkillSets_EmptyFile_HandlesCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "native_skill_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(NativeSkillSets));
            NativeSkillSets nativeSkillSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                nativeSkillSets = (NativeSkillSets)serializer.Deserialize(reader);
            }
            
            // Assert
            Assert.NotNull(nativeSkillSets);
            Assert.NotNull(nativeSkillSets.SkillSets);
            Assert.Empty(nativeSkillSets.SkillSets);
            
            // 验证可以正确处理空的技能集合
            Assert.Equal(0, nativeSkillSets.SkillSets.Count);
        }
    }
} 