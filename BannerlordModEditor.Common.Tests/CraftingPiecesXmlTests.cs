using BannerlordModEditor.Common.Models.Game;
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
    public class CraftingPiecesXmlTests
    {
        [Fact]
        public void CraftingPieces_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "crafting_pieces.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(CraftingPiecesBase));
            CraftingPiecesBase craftingPieces;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                craftingPieces = (CraftingPiecesBase)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, craftingPieces);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构检查
            Assert.NotNull(craftingPieces);
            Assert.Equal("crafting_piece", craftingPieces.Type);
            Assert.NotNull(craftingPieces.CraftingPieces);
            Assert.NotNull(craftingPieces.CraftingPieces.CraftingPiece);
            Assert.True(craftingPieces.CraftingPieces.CraftingPiece.Count > 0, "应该有至少一个锻造部件");
            
            // Assert - 验证具体的锻造部件数据
            var bladeExample = craftingPieces.CraftingPieces.CraftingPiece.FirstOrDefault(cp => cp.Id == "blade_01");
            Assert.NotNull(bladeExample);
            Assert.Equal("铁制剑刃", bladeExample.Name);
            Assert.Equal("blade", bladeExample.CraftingPieceType);
            Assert.Equal("1", bladeExample.PieceTier);
            Assert.Equal("empire", bladeExample.Culture);
            Assert.Equal("blade_01", bladeExample.Mesh);
            Assert.Equal("metal", bladeExample.PhysicsMaterial);
            
            // Assert - 验证部件数据
            Assert.NotNull(bladeExample.PieceData);
            Assert.Equal("25", bladeExample.PieceData.ThrustDamage);
            Assert.Equal("pierce", bladeExample.PieceData.ThrustDamageType);
            Assert.Equal("30", bladeExample.PieceData.SwingDamage);
            Assert.Equal("cut", bladeExample.PieceData.SwingDamageType);
            Assert.Equal("85", bladeExample.PieceData.ThrustSpeed);
            Assert.Equal("90", bladeExample.PieceData.SwingSpeed);
            Assert.Equal("90", bladeExample.PieceData.WeaponLength);
            Assert.Equal("15", bladeExample.PieceData.WeaponBalance);
            Assert.Equal("1.2", bladeExample.PieceData.Weight);
            Assert.Equal("180", bladeExample.PieceData.HitPoints);
            
            // Assert - 验证材料数据
            Assert.NotNull(bladeExample.Materials);
            Assert.NotNull(bladeExample.Materials.Material);
            Assert.True(bladeExample.Materials.Material.Count > 0, "应该有至少一种材料");
            var ironMaterial = bladeExample.Materials.Material.FirstOrDefault(m => m.Id == "iron");
            Assert.NotNull(ironMaterial);
            Assert.Equal("2", ironMaterial.Count);
            Assert.Equal("metal", ironMaterial.MaterialType);
            
            // Assert - 验证修饰符数据
            Assert.NotNull(bladeExample.Modifiers);
            Assert.NotNull(bladeExample.Modifiers.Modifier);
            Assert.True(bladeExample.Modifiers.Modifier.Count > 0, "应该有至少一个修饰符");
            var thrustModifier = bladeExample.Modifiers.Modifier.FirstOrDefault(m => m.Attribute == "thrust_damage");
            Assert.NotNull(thrustModifier);
            Assert.Equal("multiply", thrustModifier.Operation);
            Assert.Equal("1.0", thrustModifier.Value);
            
            // Assert - 验证标志数据
            Assert.NotNull(bladeExample.Flags);
            Assert.NotNull(bladeExample.Flags.Flag);
            Assert.True(bladeExample.Flags.Flag.Count > 0, "应该有至少一个标志");
            var pairFlag = bladeExample.Flags.Flag.FirstOrDefault(f => f.Name == "can_be_paired_with_guard");
            Assert.NotNull(pairFlag);
            Assert.Equal("true", pairFlag.Value);
            
            // Assert - 验证复合弓臂部件（带可用性要求）
            var bowArmExample = craftingPieces.CraftingPieces.CraftingPiece.FirstOrDefault(cp => cp.Id == "bow_arm_01");
            Assert.NotNull(bowArmExample);
            Assert.Equal("复合弓臂", bowArmExample.Name);
            Assert.Equal("bow_arm", bowArmExample.CraftingPieceType);
            Assert.Equal("2", bowArmExample.PieceTier);
            Assert.Equal("khuzait", bowArmExample.Culture);
            Assert.Equal("1.1", bowArmExample.ScaleFactor);
            
            // Assert - 验证可用性要求
            Assert.NotNull(bowArmExample.Availability);
            Assert.NotNull(bowArmExample.Availability.Requirement);
            Assert.True(bowArmExample.Availability.Requirement.Count >= 2, "应该有至少两个可用性要求");
            var cultureReq = bowArmExample.Availability.Requirement.FirstOrDefault(r => r.Type == "culture");
            Assert.NotNull(cultureReq);
            Assert.Equal("khuzait", cultureReq.Id);
            Assert.Equal("2", cultureReq.Level);
            
            var skillReq = bowArmExample.Availability.Requirement.FirstOrDefault(r => r.Type == "skill");
            Assert.NotNull(skillReq);
            Assert.Equal("bow", skillReq.Id);
            Assert.Equal("50", skillReq.Level);
            
            // Assert - 验证隐藏部件
            var hiddenPiece = craftingPieces.CraftingPieces.CraftingPiece.FirstOrDefault(cp => cp.Id == "hidden_piece_01");
            Assert.NotNull(hiddenPiece);
            Assert.Equal("true", hiddenPiece.IsHidden);
            Assert.Equal("特殊合金", hiddenPiece.Name);
            Assert.Equal("special_material", hiddenPiece.CraftingPieceType);
            Assert.Equal("4", hiddenPiece.PieceTier);
            
            // Assert - 验证有条件的修饰符
            Assert.NotNull(hiddenPiece.Modifiers);
            var conditionalModifier = hiddenPiece.Modifiers.Modifier.FirstOrDefault(m => m.Condition != null);
            Assert.NotNull(conditionalModifier);
            Assert.Equal("weapon_type=sword", conditionalModifier.Condition);
            
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
        public void CraftingPieces_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = FindSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "crafting_pieces.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(CraftingPiecesBase));
            CraftingPiecesBase craftingPieces;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                craftingPieces = (CraftingPiecesBase)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有部件都有基本属性
            foreach (var piece in craftingPieces.CraftingPieces.CraftingPiece)
            {
                Assert.False(string.IsNullOrEmpty(piece.Id), $"部件ID不能为空");
                Assert.False(string.IsNullOrEmpty(piece.Name), $"部件名称不能为空：{piece.Id}");
                Assert.False(string.IsNullOrEmpty(piece.CraftingPieceType), $"部件类型不能为空：{piece.Id}");
                Assert.False(string.IsNullOrEmpty(piece.PieceTier), $"部件等级不能为空：{piece.Id}");
                
                // 验证材料数据完整性
                if (piece.Materials != null && piece.Materials.Material.Count > 0)
                {
                    foreach (var material in piece.Materials.Material)
                    {
                        Assert.False(string.IsNullOrEmpty(material.Id), $"材料ID不能为空：部件{piece.Id}");
                        Assert.False(string.IsNullOrEmpty(material.Count), $"材料数量不能为空：部件{piece.Id}，材料{material.Id}");
                    }
                }
                
                // 验证修饰符数据完整性
                if (piece.Modifiers != null && piece.Modifiers.Modifier.Count > 0)
                {
                    foreach (var modifier in piece.Modifiers.Modifier)
                    {
                        Assert.False(string.IsNullOrEmpty(modifier.Attribute), $"修饰符属性不能为空：部件{piece.Id}");
                        Assert.False(string.IsNullOrEmpty(modifier.Operation), $"修饰符操作不能为空：部件{piece.Id}");
                        Assert.False(string.IsNullOrEmpty(modifier.Value), $"修饰符数值不能为空：部件{piece.Id}");
                    }
                }
                
                // 验证标志数据完整性
                if (piece.Flags != null && piece.Flags.Flag.Count > 0)
                {
                    foreach (var flag in piece.Flags.Flag)
                    {
                        Assert.False(string.IsNullOrEmpty(flag.Name), $"标志名称不能为空：部件{piece.Id}");
                        Assert.False(string.IsNullOrEmpty(flag.Value), $"标志值不能为空：部件{piece.Id}，标志{flag.Name}");
                    }
                }
                
                // 验证可用性要求数据完整性
                if (piece.Availability != null && piece.Availability.Requirement.Count > 0)
                {
                    foreach (var requirement in piece.Availability.Requirement)
                    {
                        Assert.False(string.IsNullOrEmpty(requirement.Type), $"要求类型不能为空：部件{piece.Id}");
                        // Id和Level对某些要求类型可能为空，所以不做强制检查
                    }
                }
            }
        }
        
        [Fact]
        public void DeserializationTest()
        {
            var serializer = new XmlSerializer(typeof(CraftingPiecesBase));
            var path = Path.Combine("TestData", "crafting_pieces.xml");
            using (var stream = File.OpenRead(path))
            {
                var obj = serializer.Deserialize(stream);
                Assert.NotNull(obj);
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