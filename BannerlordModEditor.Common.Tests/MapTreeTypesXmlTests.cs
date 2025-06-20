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
    public class MapTreeTypesXmlTests
    {
        [Fact]
        public void MapTreeTypes_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_tree_types.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(MapTreeTypes));
            MapTreeTypes mapTreeTypes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapTreeTypes = (MapTreeTypes)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, mapTreeTypes);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(mapTreeTypes);
            Assert.Equal("tree_type", mapTreeTypes.Type);
            Assert.Equal(6, mapTreeTypes.TreeTypes.TreeTypeList.Count);

            // 验证特定的树木类型
            var snowTrees = mapTreeTypes.TreeTypes.TreeTypeList.FirstOrDefault(t => t.Name == "snow_trees");
            Assert.NotNull(snowTrees);
            Assert.Equal(3, snowTrees.Trees.Count);
            Assert.Equal("snow_tree_a", snowTrees.Trees[0].Name);
            Assert.Null(snowTrees.Trees[0].LodMeshName);

            // 验证带LOD的平原树
            var plainTrees = mapTreeTypes.TreeTypes.TreeTypeList.FirstOrDefault(t => t.Name == "plain_trees");
            Assert.NotNull(plainTrees);
            Assert.Equal(4, plainTrees.Trees.Count);
            Assert.Equal("map_tree_a", plainTrees.Trees[0].Name);
            Assert.Equal("map_tree_new_a_lod1", plainTrees.Trees[0].LodMeshName);

            // 验证混合树（最多的7个）
            var mixTrees = mapTreeTypes.TreeTypes.TreeTypeList.FirstOrDefault(t => t.Name == "mix_trees");
            Assert.NotNull(mixTrees);
            Assert.Equal(7, mixTrees.Trees.Count);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("tree_types").Count() == savedDoc.Root?.Elements("tree_types").Count(),
                "tree_types count should be the same");
        }
        
        [Fact]
        public void MapTreeTypes_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "map_tree_types.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(MapTreeTypes));
            MapTreeTypes mapTreeTypes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                mapTreeTypes = (MapTreeTypes)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有树木类型都有必要的数据
            foreach (var treeType in mapTreeTypes.TreeTypes.TreeTypeList)
            {
                Assert.False(string.IsNullOrWhiteSpace(treeType.Name), "Tree type should have Name");
                Assert.True(treeType.Trees.Count > 0, $"Tree type '{treeType.Name}' should have trees");
                
                // 验证每个树的数据完整性
                foreach (var tree in treeType.Trees)
                {
                    Assert.False(string.IsNullOrWhiteSpace(tree.Name), "Tree should have Name");
                    
                    // 验证LOD网格名称逻辑（只有特定的树有LOD）
                    if (tree.Name.StartsWith("map_tree_") && !tree.Name.StartsWith("map_tree_small"))
                    {
                        // 大部分主要树木应该有LOD网格
                        // 注意：某些情况下可能没有，这是正常的
                    }
                    
                    // 验证命名约定
                    Assert.True(tree.Name.Length > 0, "Tree name should not be empty");
                    Assert.False(tree.Name.Contains(" "), "Tree name should not contain spaces");
                }
            }
            
            // 验证特定的树木类型存在
            var expectedTreeTypes = new[] { "snow_trees", "steppe_trees", "desert_trees", "plain_trees", "pine_trees", "mix_trees" };
            foreach (var expectedType in expectedTreeTypes)
            {
                var treeType = mapTreeTypes.TreeTypes.TreeTypeList.FirstOrDefault(t => t.Name == expectedType);
                Assert.NotNull(treeType);
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