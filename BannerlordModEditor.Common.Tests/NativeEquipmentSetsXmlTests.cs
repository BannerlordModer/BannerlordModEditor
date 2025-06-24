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
    public class NativeEquipmentSetsXmlTests
    {
        [Fact]
        public void NativeEquipmentSets_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "native_equipment_sets.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(NativeEquipmentSets));
            NativeEquipmentSets nativeEquipmentSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                nativeEquipmentSets = (NativeEquipmentSets)serializer.Deserialize(reader);
            }
            
            // 验证反序列化的数据
            Assert.NotNull(nativeEquipmentSets);
            Assert.NotNull(nativeEquipmentSets.EquipmentRosters);
            
            // 这个文件当前是空的，所以列表应该为空
            Assert.Empty(nativeEquipmentSets.EquipmentRosters);
            
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
                    serializer.Serialize(xmlWriter, nativeEquipmentSets);
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
            Assert.Equal("EquipmentRosters", serializedDoc.Root.Name.LocalName);
        }

        [Fact]
        public void NativeEquipmentSets_EmptyFile_HandlesCorrectly()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "native_equipment_sets.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(NativeEquipmentSets));
            NativeEquipmentSets nativeEquipmentSets;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                nativeEquipmentSets = (NativeEquipmentSets)serializer.Deserialize(reader);
            }
            
            // Assert
            Assert.NotNull(nativeEquipmentSets);
            Assert.NotNull(nativeEquipmentSets.EquipmentRosters);
            Assert.Empty(nativeEquipmentSets.EquipmentRosters);
            
            // 验证可以正确处理空的设备集合
            Assert.Equal(0, nativeEquipmentSets.EquipmentRosters.Count);
        }
    }
}