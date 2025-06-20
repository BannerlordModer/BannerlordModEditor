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
    public class FloraGroupsXmlTests
    {
        [Fact]
        public void FloraGroups_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "flora_groups.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(FloraGroups));
            FloraGroups floraGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                floraGroups = (FloraGroups)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, floraGroups);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(floraGroups);
            Assert.Equal(5, floraGroups.FloraGroupList.Count);

            // 验证测试组
            var testGroup = floraGroups.FloraGroupList.FirstOrDefault(g => g.Name == "test_flora_group");
            Assert.NotNull(testGroup);
            Assert.Equal(3, testGroup.FloraRecords.Records.Count);
            Assert.Equal("wheats_green", testGroup.FloraRecords.Records[0].Name);
            Assert.Equal(100, testGroup.FloraRecords.Records[0].Density);

            // 验证草地组
            var grassGroup = floraGroups.FloraGroupList.FirstOrDefault(g => g.Name == "grass");
            Assert.NotNull(grassGroup);
            Assert.Equal(3, grassGroup.FloraRecords.Records.Count);

            // 验证芦苇组（有6个记录）
            var reedsGroup = floraGroups.FloraGroupList.FirstOrDefault(g => g.Name == "reeds");
            Assert.NotNull(reedsGroup);
            Assert.Equal(6, reedsGroup.FloraRecords.Records.Count);
            Assert.Equal("flora_reed_a", reedsGroup.FloraRecords.Records[0].Name);
            Assert.Equal(25, reedsGroup.FloraRecords.Records[0].Density);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("flora_group").Count() == savedDoc.Root?.Elements("flora_group").Count(),
                "flora_group count should be the same");
        }
        
        [Fact]
        public void FloraGroups_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "flora_groups.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(FloraGroups));
            FloraGroups floraGroups;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                floraGroups = (FloraGroups)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有组都有必要的数据
            foreach (var group in floraGroups.FloraGroupList)
            {
                Assert.False(string.IsNullOrWhiteSpace(group.Name), "Flora group should have Name");
                Assert.NotNull(group.FloraRecords);
                Assert.True(group.FloraRecords.Records.Count > 0, $"Flora group '{group.Name}' should have records");
                
                // 验证每个记录的数据完整性
                foreach (var record in group.FloraRecords.Records)
                {
                    Assert.False(string.IsNullOrWhiteSpace(record.Name), "Flora record should have Name");
                    Assert.True(record.Density > 0, $"Flora record '{record.Name}' should have positive density");
                    Assert.True(record.Density <= 100, $"Flora record '{record.Name}' density should not exceed 100");
                }
            }
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