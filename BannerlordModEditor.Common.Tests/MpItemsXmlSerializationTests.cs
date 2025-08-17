using BannerlordModEditor.Common.Mappers;
using BannerlordModEditor.Common.Models.DO;
using BannerlordModEditor.Common.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace BannerlordModEditor.Common.Tests
{
    public class MpItemsXmlSerializationTests
    {
        private readonly ITestOutputHelper _output;

        public MpItemsXmlSerializationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void MpItemsDO_XmlSerialization_ShouldPreserveNamespaceAndStructure()
        {
            // Arrange
            var doItem = new MpItemsDO
            {
                Items = new List<MpItemDO>
                {
                    new MpItemDO
                    {
                        Id = "test_item_1",
                        Name = "Test Item 1",
                        MultiplayerItem = "true",
                        Type = "Weapon",
                        UsingTableau = "false",
                        IsMerchandise = "true",
                        RecalculateBody = "true",
                        HasLowerHolsterPriority = "false",
                        Value = "100"
                    }
                },
                CraftedItems = new List<CraftedItemDO>
                {
                    new CraftedItemDO
                    {
                        Id = "crafted_item_1",
                        Name = "Crafted Item 1",
                        CraftingTemplate = "template_1",
                        MultiplayerItem = "false",
                        IsMerchandise = "false",
                        Value = "200"
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(MpItemsDO));
            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                Encoding = new UTF8Encoding(false)
            };

            // Act
            var memoryStream = new MemoryStream();
            using (var writer = XmlWriter.Create(memoryStream, writerSettings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Remove default namespace
                serializer.Serialize(writer, doItem, ns);
            }

            memoryStream.Position = 0;
            var xmlResult = new StreamReader(memoryStream).ReadToEnd();

            // Assert
            Assert.Contains("<Items>", xmlResult);
            Assert.Contains("<Item", xmlResult);
            Assert.Contains("id=\"test_item_1\"", xmlResult);
            Assert.Contains("multiplayer_item=\"true\"", xmlResult);
            Assert.Contains("<CraftedItem", xmlResult);
            Assert.Contains("id=\"crafted_item_1\"", xmlResult);
            Assert.Contains("multiplayer_item=\"false\"", xmlResult);

            // Verify no namespace declarations
            Assert.DoesNotContain("xmlns", xmlResult);

            _output.WriteLine("Serialized XML:");
            _output.WriteLine(xmlResult);
        }

        [Fact]
        public void MpItemsDO_ToDTO_ToDO_XmlSerialization_ShouldBeConsistent()
        {
            // Arrange
            var originalDo = new MpItemsDO
            {
                Items = new List<MpItemDO>
                {
                    new MpItemDO
                    {
                        Id = "test_item_1",
                        Name = "Test Item 1",
                        MultiplayerItem = "true",
                        Type = "Weapon",
                        UsingTableau = "false",
                        IsMerchandise = "1",
                        RecalculateBody = "yes",
                        HasLowerHolsterPriority = "0",
                        Value = "100"
                    }
                }
            };

            // Act - Full round trip
            var dto = MpItemsMapper.ToDTO(originalDo);
            var resultDo = MpItemsMapper.ToDO(dto);

            // Serialize both
            var originalSerializer = new XmlSerializer(typeof(MpItemsDO));
            var resultSerializer = new XmlSerializer(typeof(MpItemsDO));
            
            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                Encoding = new UTF8Encoding(false)
            };

            var originalXml = SerializeToXml(originalDo, originalSerializer, writerSettings);
            var resultXml = SerializeToXml(resultDo, resultSerializer, writerSettings);

            // Assert
            Assert.NotNull(originalXml);
            Assert.NotNull(resultXml);
            Assert.Contains("multiplayer_item=\"true\"", originalXml);
            Assert.Contains("multiplayer_item=\"true\"", resultXml); // Should be standardized
            Assert.Contains("is_merchandise=\"true\"", resultXml); // Should be standardized from "1"
            Assert.Contains("recalculate_body=\"true\"", resultXml); // Should be standardized from "yes"
            Assert.Contains("has_lower_holster_priority=\"false\"", resultXml); // Should be standardized from "0"

            _output.WriteLine("Original XML:");
            _output.WriteLine(originalXml);
            _output.WriteLine("\nResult XML:");
            _output.WriteLine(resultXml);
        }

        private string SerializeToXml(MpItemsDO obj, XmlSerializer serializer, XmlWriterSettings settings)
        {
            var memoryStream = new MemoryStream();
            using (var writer = XmlWriter.Create(memoryStream, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Remove default namespace
                serializer.Serialize(writer, obj, ns);
            }

            memoryStream.Position = 0;
            return new StreamReader(memoryStream).ReadToEnd();
        }
    }
}