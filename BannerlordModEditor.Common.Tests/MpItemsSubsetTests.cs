using BannerlordModEditor.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class MpItemsSubsetTests
    {
        public static IEnumerable<object[]> GetItemFiles()
        {
            var solutionRoot = TestUtils.GetSolutionRoot();
            var subsetDirectory = Path.Combine(solutionRoot, "BannerlordModEditor.Common.Tests", "TestSubsets", "MpItems");
            if (!Directory.Exists(subsetDirectory))
            {
                return Enumerable.Empty<object[]>();
            }
            return Directory.GetFiles(subsetDirectory, "*.xml").Select(file => new object[] { file });
        }

        [Theory]
        [MemberData(nameof(GetItemFiles))]
        public void SingleItem_LoadAndSave_ShouldBeLogicallyIdentical(string itemFilePath)
        {
            // Arrange
            var serializer = new XmlSerializer(typeof(Item));
            Item? item;

            // Act
            using (var reader = new FileStream(itemFilePath, FileMode.Open))
            {
                item = serializer.Deserialize(reader) as Item;
            }
            Assert.NotNull(item);
            
            var memoryStream = new MemoryStream();
            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                Encoding = new UTF8Encoding(false)
            };
            using (var writer = XmlWriter.Create(memoryStream, writerSettings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, item, ns);
            }
            memoryStream.Position = 0;
            var savedXml = new StreamReader(memoryStream).ReadToEnd();

            // Assert
            var originalDoc = XDocument.Load(itemFilePath, LoadOptions.None);
            var savedDoc = XDocument.Parse(savedXml, LoadOptions.None);

            Assert.True(AreXmlElementsLogicallyEqual(originalDoc.Root, savedDoc.Root),
                $"File '{Path.GetFileName(itemFilePath)}' is not logically equivalent.\n\nOriginal:\n{originalDoc.Root}\n\nGenerated:\n{savedDoc.Root}");
        }

        private static bool AreXmlElementsLogicallyEqual(XElement? original, XElement? generated)
        {
            if (original == null && generated == null) return true;
            if (original == null || generated == null) return false;

            // 比较元素名称
            if (original.Name != generated.Name) return false;

            // 比较属性（忽略顺序）
            var originalAttrs = original.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);
            var generatedAttrs = generated.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value);

            if (originalAttrs.Count != generatedAttrs.Count) return false;

            foreach (var attr in originalAttrs)
            {
                if (!generatedAttrs.TryGetValue(attr.Key, out var generatedValue))
                    return false;
                
                // 对于数值类型，进行宽松比较（例如 1.0 == 1）
                if (IsNumericValue(attr.Value, generatedValue))
                {
                    if (!AreNumericValuesEqual(attr.Value, generatedValue))
                        return false;
                }
                else if (attr.Value != generatedValue)
                {
                    return false;
                }
            }

            // 比较子元素（保持顺序，因为这对于列表类很重要）
            var originalChildren = original.Elements().ToList();
            var generatedChildren = generated.Elements().ToList();

            if (originalChildren.Count != generatedChildren.Count) return false;

            for (int i = 0; i < originalChildren.Count; i++)
            {
                if (!AreXmlElementsLogicallyEqual(originalChildren[i], generatedChildren[i]))
                    return false;
            }

            // 比较文本内容
            return original.Value == generated.Value;
        }

        private static bool IsNumericValue(string value1, string value2)
        {
            return (double.TryParse(value1, out _) && double.TryParse(value2, out _)) ||
                   (int.TryParse(value1, out _) && int.TryParse(value2, out _));
        }

        private static bool AreNumericValuesEqual(string value1, string value2)
        {
            if (double.TryParse(value1, out var d1) && double.TryParse(value2, out var d2))
            {
                return Math.Abs(d1 - d2) < 0.0001; // 允许小的浮点误差
            }
            return value1 == value2;
        }
    }
}