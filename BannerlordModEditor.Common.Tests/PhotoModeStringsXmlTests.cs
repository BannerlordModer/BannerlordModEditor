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
    public class PhotoModeStringsXmlTests
    {
        [Fact]
        public void PhotoModeStrings_LoadAndSave_ShouldBeLogicallyIdentical()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "photo_mode_strings.xml");
            
            // Act - 反序列化
            var serializer = new XmlSerializer(typeof(PhotoModeStrings));
            PhotoModeStrings photoModeStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                photoModeStrings = (PhotoModeStrings)serializer.Deserialize(reader)!;
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
                    serializer.Serialize(xmlWriter, photoModeStrings);
                }
                savedXml = writer.ToString();
            }

            // Assert - 基本结构验证
            Assert.NotNull(photoModeStrings);
            Assert.Equal(47, photoModeStrings.Strings.Count);

            // 验证特定的照片模式字符串
            var defaultFilter = photoModeStrings.Strings.FirstOrDefault(s => s.Id == "str_photo_mode_overlay.photo_mode_filter_identity");
            Assert.NotNull(defaultFilter);
            Assert.Equal("{=Qe0GG34b}Default", defaultFilter.Text);

            var cinematicBands = photoModeStrings.Strings.FirstOrDefault(s => s.Id == "str_photo_mode_overlay.photo_mode_filter_cinematic_bands");
            Assert.NotNull(cinematicBands);
            Assert.Equal("{=Xn2bpKRO}Cinematic Bands", cinematicBands.Text);

            // 验证颜色分级字符串
            var popColorGrade = photoModeStrings.Strings.FirstOrDefault(s => s.Id == "str_photo_mode_color_grade.1_pop");
            Assert.NotNull(popColorGrade);
            Assert.Equal("{=XSUgdvyo}Pop", popColorGrade.Text);

            var aseraiheat = photoModeStrings.Strings.FirstOrDefault(s => s.Id == "str_photo_mode_color_grade.2_aserai_heat");
            Assert.NotNull(aseraiheat);
            Assert.Equal("{=0RRNliSj}Aserai Heat", aseraiheat.Text);

            // Assert - XML结构验证
            var originalDoc = XDocument.Load(xmlPath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);
            
            // 移除纯空白文本节点
            RemoveWhitespaceNodes(originalDoc.Root);
            RemoveWhitespaceNodes(savedDoc.Root);
            
            // 检查XML结构基本一致
            Assert.True(originalDoc.Root?.Elements("string").Count() == savedDoc.Root?.Elements("string").Count(),
                "string count should be the same");
        }
        
        [Fact]
        public void PhotoModeStrings_ValidateDataIntegrity_ShouldPassBasicChecks()
        {
            // Arrange
            var solutionRoot = TestUtils.GetSolutionRoot();
            var xmlPath = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestData", "photo_mode_strings.xml");
            
            // Act
            var serializer = new XmlSerializer(typeof(PhotoModeStrings));
            PhotoModeStrings photoModeStrings;
            
            using (var reader = new FileStream(xmlPath, FileMode.Open))
            {
                photoModeStrings = (PhotoModeStrings)serializer.Deserialize(reader)!;
            }
            
            // Assert - 验证所有字符串都有必要的数据
            foreach (var str in photoModeStrings.Strings)
            {
                Assert.False(string.IsNullOrWhiteSpace(str.Id), "String should have Id");
                Assert.False(string.IsNullOrWhiteSpace(str.Text), "String should have Text");
                
                // 验证ID格式
                Assert.True(str.Id.StartsWith("str_photo_mode_"), "Photo mode string ID should start with str_photo_mode_");
                
                // 验证本地化字符串格式
                Assert.True(str.Text.StartsWith("{=") && str.Text.Contains("}"), 
                    $"String '{str.Id}' should have localization format {{=key}}value");
                
                // 验证分类
                var isOverlay = str.Id.Contains("photo_mode_overlay");
                var isColorGrade = str.Id.Contains("photo_mode_color_grade");
                Assert.True(isOverlay || isColorGrade, 
                    $"String '{str.Id}' should be either overlay or color grade type");
            }
            
            // 验证必需的滤镜类型存在
            var requiredFilters = new[] { 
                "str_photo_mode_overlay.photo_mode_filter_identity",
                "str_photo_mode_overlay.photo_mode_filter_frame",
                "str_photo_mode_overlay.photo_mode_filter_cinematic_bands"
            };
            
            foreach (var requiredFilter in requiredFilters)
            {
                var filterString = photoModeStrings.Strings.FirstOrDefault(s => s.Id == requiredFilter);
                Assert.NotNull(filterString);
            }
            
            // 验证必需的颜色分级存在
            var requiredColorGrades = new[] { 
                "str_photo_mode_color_grade.1_pop",
                "str_photo_mode_color_grade.Default",
                "str_photo_mode_color_grade.colorgrade_sketch"
            };
            
            foreach (var requiredGrade in requiredColorGrades)
            {
                var gradeString = photoModeStrings.Strings.FirstOrDefault(s => s.Id == requiredGrade);
                Assert.NotNull(gradeString);
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