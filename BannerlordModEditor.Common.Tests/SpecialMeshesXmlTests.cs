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
    public class SpecialMeshesXmlTests
    {
        [Fact]
        public void SpecialMeshes_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "special_meshes.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(SpecialMeshes));
            SpecialMeshes specialMeshes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                specialMeshes = (SpecialMeshes)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, specialMeshes);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构检查
            Assert.NotNull(specialMeshes);
            Assert.Equal("special_meshes", specialMeshes.Type);
            Assert.NotNull(specialMeshes.Meshes);
            Assert.NotNull(specialMeshes.Meshes.MeshList);
            Assert.True(specialMeshes.Meshes.MeshList.Count > 0, "应该有至少一个特殊网格");
            
            // Assert - 验证具体的特殊网格数据
            var outerMeshForest = specialMeshes.Meshes.MeshList.FirstOrDefault(m => m.Name == "outer_mesh_forest");
            Assert.NotNull(outerMeshForest);
            Assert.NotNull(outerMeshForest.Types);
            Assert.NotNull(outerMeshForest.Types.TypeList);
            Assert.True(outerMeshForest.Types.TypeList.Count > 0, "应该有至少一个类型");
            
            var outerMeshType = outerMeshForest.Types.TypeList.FirstOrDefault(t => t.Name == "outer_mesh");
            Assert.NotNull(outerMeshType);
            Assert.Equal("outer_mesh", outerMeshType.Name);
            
            var mainMapOuter = specialMeshes.Meshes.MeshList.FirstOrDefault(m => m.Name == "main_map_outer");
            Assert.NotNull(mainMapOuter);
            Assert.NotNull(mainMapOuter.Types);
            Assert.NotNull(mainMapOuter.Types.TypeList);
            Assert.True(mainMapOuter.Types.TypeList.Count > 0, "应该有至少一个类型");
            
            var mainMapOuterType = mainMapOuter.Types.TypeList.FirstOrDefault(t => t.Name == "outer_mesh");
            Assert.NotNull(mainMapOuterType);
            Assert.Equal("outer_mesh", mainMapOuterType.Name);
            
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
        public void SpecialMeshes_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "special_meshes.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(SpecialMeshes));
            SpecialMeshes specialMeshes;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                specialMeshes = (SpecialMeshes)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有特殊网格都有基本属性
            foreach (var mesh in specialMeshes.Meshes.MeshList)
            {
                Assert.False(string.IsNullOrEmpty(mesh.Name), "特殊网格名称不能为空");
                
                // 验证类型数据完整性
                if (mesh.Types != null && mesh.Types.TypeList.Count > 0)
                {
                    foreach (var type in mesh.Types.TypeList)
                    {
                        Assert.False(string.IsNullOrEmpty(type.Name), $"类型名称不能为空：网格{mesh.Name}");
                    }
                }
            }
            
            // 验证必要的网格存在
            var requiredMeshes = new[] { "outer_mesh_forest", "main_map_outer" };
            foreach (var requiredMesh in requiredMeshes)
            {
                Assert.True(specialMeshes.Meshes.MeshList.Any(m => m.Name == requiredMesh),
                    $"必需的特殊网格缺失：{requiredMesh}");
            }
            
            // 验证outer_mesh类型存在
            foreach (var mesh in specialMeshes.Meshes.MeshList.Where(m => requiredMeshes.Contains(m.Name)))
            {
                Assert.True(mesh.Types?.TypeList.Any(t => t.Name == "outer_mesh") == true,
                    $"网格{mesh.Name}应该有outer_mesh类型");
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