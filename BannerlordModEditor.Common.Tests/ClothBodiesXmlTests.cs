using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.Engine;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class ClothBodiesXmlTests
    {
        [Fact]
        public void ClothBodies_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "cloth_bodies.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(ClothBodies));
            ClothBodies clothBodies;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                clothBodies = (ClothBodies)serializer.Deserialize(reader)!;
            }
            
            // Act - 序列化
            string savedXml;
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings 
            { 
                Indent = true, 
                IndentChars = "\t",
                OmitXmlDeclaration = false,
                Encoding = Encoding.UTF8
            }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(xmlWriter, clothBodies, ns);
                savedXml = stringWriter.ToString();
            }
            
            // Assert - 基本结构验证
            Assert.NotNull(clothBodies);
            Assert.NotNull(clothBodies.Body);
            Assert.True(clothBodies.Body.Count > 0, "Should have at least one cloth body");
            
            // 验证具体的布料体数据
            var newBody = clothBodies.Body.FirstOrDefault(b => b.Name == "New body");
            Assert.NotNull(newBody);
            Assert.Null(newBody.OwnerSkeleton); // 这个body没有owner_skeleton
            Assert.NotNull(newBody.Capsules);
            Assert.NotNull(newBody.Capsules.Capsule);
            Assert.True(newBody.Capsules.Capsule.Count > 0, "Should have at least one capsule");
            
            var newCapsule = newBody.Capsules.Capsule.First();
            Assert.Equal("New capsule", newCapsule.Name);
            Assert.Equal("0.200", newCapsule.Length);
            Assert.Equal("0.000, 0.000, 0.000", newCapsule.Origin);
            Assert.NotNull(newCapsule.Points);
            Assert.NotNull(newCapsule.Points.Point);
            Assert.Equal(2, newCapsule.Points.Point.Count);
            
            // 验证point数据
            var firstPoint = newCapsule.Points.Point.First();
            Assert.Equal("0.100", firstPoint.Radius);
            Assert.NotNull(firstPoint.Bones);
            Assert.NotNull(firstPoint.Bones.Bone);
            Assert.Empty(firstPoint.Bones.Bone); // 这个point没有bones
            
            // 验证有owner_skeleton的body
            var armsBody = clothBodies.Body.FirstOrDefault(b => b.Name == "arms");
            Assert.NotNull(armsBody);
            Assert.Equal("human_skeleton", armsBody.OwnerSkeleton);
            Assert.NotNull(armsBody.Capsules);
            Assert.NotNull(armsBody.Capsules.Capsule);
            Assert.True(armsBody.Capsules.Capsule.Count > 0);
            
            // 验证有bones的capsule
            var lArmCapsule = armsBody.Capsules.Capsule.FirstOrDefault(c => c.Name == "l_arm");
            Assert.NotNull(lArmCapsule);
            Assert.Equal("0.200", lArmCapsule.Length);
            Assert.NotNull(lArmCapsule.Points);
            Assert.NotNull(lArmCapsule.Points.Point);
            Assert.True(lArmCapsule.Points.Point.Count > 0);
            
            var pointWithBone = lArmCapsule.Points.Point.First();
            Assert.Equal("0.130", pointWithBone.Radius);
            Assert.NotNull(pointWithBone.Bones);
            Assert.NotNull(pointWithBone.Bones.Bone);
            Assert.True(pointWithBone.Bones.Bone.Count > 0);
            
            var bone = pointWithBone.Bones.Bone.First();
            Assert.Equal("l_upperarm_twist", bone.Name);
            Assert.Equal("1.000", bone.Weight);
            
            // 验证序列化的XML不包含null值属性
            Assert.DoesNotContain("owner_skeleton=\"\"", savedXml);
            
            // 验证序列化包含必要的属性
            Assert.Contains("name=\"New body\"", savedXml);
            Assert.Contains("name=\"arms\"", savedXml);
            Assert.Contains("owner_skeleton=\"human_skeleton\"", savedXml);
            Assert.Contains("<capsule", savedXml);
            Assert.Contains("<point", savedXml);
            Assert.Contains("<bone", savedXml);
        }
        
        [Fact]
        public void ClothBodies_ShouldHandleOptionalOwnerSkeleton()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "cloth_bodies.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ClothBodies));
            ClothBodies clothBodies;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                clothBodies = (ClothBodies)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证有owner_skeleton和没有owner_skeleton的body
            var bodiesWithOwner = clothBodies.Body.Where(b => !string.IsNullOrEmpty(b.OwnerSkeleton)).ToList();
            var bodiesWithoutOwner = clothBodies.Body.Where(b => string.IsNullOrEmpty(b.OwnerSkeleton)).ToList();
            
            Assert.True(bodiesWithOwner.Count > 0, "Should have bodies with owner_skeleton");
            Assert.True(bodiesWithoutOwner.Count > 0, "Should have bodies without owner_skeleton");
            
            // 验证有owner_skeleton的body
            var armsBody = bodiesWithOwner.FirstOrDefault(b => b.Name == "arms");
            Assert.NotNull(armsBody);
            Assert.Equal("human_skeleton", armsBody.OwnerSkeleton);
            
            // 验证没有owner_skeleton的body
            var newBody = bodiesWithoutOwner.FirstOrDefault(b => b.Name == "New body");
            Assert.NotNull(newBody);
            Assert.Null(newBody.OwnerSkeleton);
        }

        [Fact]
        public void ClothBodies_ShouldHandleEmptyBones()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "cloth_bodies.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ClothBodies));
            ClothBodies clothBodies;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                clothBodies = (ClothBodies)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证空bones和有内容的bones都能正确处理
            var allPoints = clothBodies.Body
                .SelectMany(b => b.Capsules?.Capsule ?? new List<ClothCapsule>())
                .SelectMany(c => c.Points?.Point ?? new List<ClothPoint>())
                .ToList();
            
            var pointsWithEmptyBones = allPoints.Where(p => p.Bones?.Bone.Count == 0).ToList();
            var pointsWithBones = allPoints.Where(p => p.Bones?.Bone.Count > 0).ToList();
            
            Assert.True(pointsWithEmptyBones.Count > 0, "Should have points with empty bones");
            Assert.True(pointsWithBones.Count > 0, "Should have points with bones");
            
            // 验证空bones的point
            var emptyBonesPoint = pointsWithEmptyBones.First();
            Assert.NotNull(emptyBonesPoint.Bones);
            Assert.NotNull(emptyBonesPoint.Bones.Bone);
            Assert.Empty(emptyBonesPoint.Bones.Bone);
            
            // 验证有bones的point
            var bonesPoint = pointsWithBones.First();
            Assert.NotNull(bonesPoint.Bones);
            Assert.NotNull(bonesPoint.Bones.Bone);
            Assert.True(bonesPoint.Bones.Bone.Count > 0);
            
            var firstBone = bonesPoint.Bones.Bone.First();
            Assert.NotNull(firstBone.Name);
            Assert.NotNull(firstBone.Weight);
        }

        [Fact]
        public void ClothBodies_ShouldPreserveNumericPrecision()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "cloth_bodies.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ClothBodies));
            ClothBodies clothBodies;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                clothBodies = (ClothBodies)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证数值精度保持
            var armsBody = clothBodies.Body.FirstOrDefault(b => b.Name == "arms");
            Assert.NotNull(armsBody);
            
            var lArmCapsule = armsBody.Capsules?.Capsule.FirstOrDefault(c => c.Name == "l_arm");
            Assert.NotNull(lArmCapsule);
            Assert.Equal("0.200", lArmCapsule.Length);
            Assert.Equal("0.000, 0.000, 0.000", lArmCapsule.Origin);
            
            // 验证复杂的frame矩阵值
            Assert.Contains("-0.834051", lArmCapsule.Frame);
            Assert.Contains("-0.035514", lArmCapsule.Frame);
            Assert.Contains("-0.550543", lArmCapsule.Frame);
            
            // 验证point的radius精度
            var firstPoint = lArmCapsule.Points?.Point.FirstOrDefault();
            Assert.NotNull(firstPoint);
            Assert.Equal("0.130", firstPoint.Radius);
        }

        [Fact]
        public void ClothBodies_ShouldSerializeWithoutNullAttributes()
        {
            // Arrange - 创建一个只有必要属性的布料体
            var clothBodies = new ClothBodies();
            var body = new ClothBody
            {
                Name = "test_body",
                // 故意不设置OwnerSkeleton，它应该保持null
                OwnerSkeleton = null,
                Capsules = new CapsulesContainer
                {
                    Capsule = new List<ClothCapsule>
                    {
                        new ClothCapsule
                        {
                            Name = "test_capsule",
                            Length = "1.000",
                            Points = new PointsContainer
                            {
                                Point = new List<ClothPoint>
                                {
                                    new ClothPoint
                                    {
                                        Radius = "0.050",
                                        Bones = new BonesContainer
                                        {
                                            Bone = new List<ClothBone>()
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            clothBodies.Body.Add(body);
            
            // Act - 序列化
            var serializer = new XmlSerializer(typeof(ClothBodies));
            string serializedXml;
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings 
            { 
                Indent = true, 
                OmitXmlDeclaration = false
            }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(xmlWriter, clothBodies, ns);
                serializedXml = stringWriter.ToString();
            }
            
            // Assert - 验证null属性不会出现在XML中
            Assert.DoesNotContain("owner_skeleton=", serializedXml);
            
            // 验证必要属性存在
            Assert.Contains("name=\"test_body\"", serializedXml);
            Assert.Contains("name=\"test_capsule\"", serializedXml);
            Assert.Contains("length=\"1.000\"", serializedXml);
            Assert.Contains("radius=\"0.050\"", serializedXml);
        }

        [Fact]
        public void ClothBodies_ShouldHandleComplexFrameMatrices()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "cloth_bodies.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(ClothBodies));
            ClothBodies clothBodies;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                clothBodies = (ClothBodies)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证复杂的frame矩阵能正确处理
            var armsBody = clothBodies.Body.FirstOrDefault(b => b.Name == "arms");
            Assert.NotNull(armsBody);
            
            var rArmCapsule = armsBody.Capsules?.Capsule.FirstOrDefault(c => c.Name == "r_arm");
            Assert.NotNull(rArmCapsule);
            
            // 验证frame矩阵包含16个浮点数值
            var frameValues = rArmCapsule.Frame.Split(',').Select(s => s.Trim()).ToList();
            Assert.Equal(16, frameValues.Count);
            
            // 验证最后一个值是1.000000（表示这是一个齐次坐标变换矩阵）
            Assert.Equal("1.000000", frameValues.Last());
            
            // 验证包含负数值
            Assert.Contains(frameValues, v => v.StartsWith("-"));
        }
    }
} 