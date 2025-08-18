using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Configuration;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class NativeStringsXmlTests
    {
        [Fact]
        public void NativeStrings_CanDeserializeFromXml()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <strings>
    <string id=""01zQKaF3"" text=""Eye Shape"" />
    <string id=""05loYcST"" text=""Hit shield on back"" />
    <string id=""05w8Tl7n"" text=""Steppe"" />
    <string id=""0cpbEvsG"" text=""Old Face"" />
    <string id=""0DBQGaka"" text=""Delivered couched lance damage!"" />
  </strings>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<NativeStringsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            
            Assert.NotNull(result.Tags);
            Assert.NotNull(result.Tags.TagList);
            Assert.Single(result.Tags.TagList);
            
            var tag = result.Tags.TagList[0];
            Assert.Equal("English", tag.Language);
            
            Assert.NotNull(result.Strings);
            Assert.NotNull(result.Strings.StringList);
            Assert.Equal(5, result.Strings.StringList.Count);
            
            var firstString = result.Strings.StringList[0];
            Assert.Equal("01zQKaF3", firstString.Id);
            Assert.Equal("Eye Shape", firstString.Text);
            
            var secondString = result.Strings.StringList[1];
            Assert.Equal("05loYcST", secondString.Id);
            Assert.Equal("Hit shield on back", secondString.Text);
            
            var thirdString = result.Strings.StringList[2];
            Assert.Equal("05w8Tl7n", thirdString.Id);
            Assert.Equal("Steppe", thirdString.Text);
            
            var fourthString = result.Strings.StringList[3];
            Assert.Equal("0cpbEvsG", fourthString.Id);
            Assert.Equal("Old Face", fourthString.Text);
            
            var fifthString = result.Strings.StringList[4];
            Assert.Equal("0DBQGaka", fifthString.Id);
            Assert.Equal("Delivered couched lance damage!", fifthString.Text);
        }

        [Fact]
        public void NativeStrings_CanSerializeToXml()
        {
            // Arrange
            var nativeStrings = new NativeStringsDO
            {
                Type = "string",
                Tags = new NativeStringTagsDO
                {
                    TagList = new List<NativeStringTagDO>
                    {
                        new NativeStringTagDO
                        {
                            Language = "English"
                        }
                    }
                },
                Strings = new NativeStringsCollectionDO
                {
                    StringList = new List<NativeStringItemDO>
                    {
                        new NativeStringItemDO
                        {
                            Id = "01zQKaF3",
                            Text = "Eye Shape"
                        },
                        new NativeStringItemDO
                        {
                            Id = "05loYcST",
                            Text = "Hit shield on back"
                        },
                        new NativeStringItemDO
                        {
                            Id = "05w8Tl7n",
                            Text = "Steppe"
                        }
                    }
                }
            };

            // Act
            var serializedXml = XmlTestUtils.Serialize(nativeStrings);

            // Assert
            Assert.NotNull(serializedXml);
            Assert.Contains("base", serializedXml);
            Assert.Contains("type=\"string\"", serializedXml);
            Assert.Contains("tags", serializedXml);
            Assert.Contains("strings", serializedXml);
            Assert.Contains("language=\"English\"", serializedXml);
            Assert.Contains("id=\"01zQKaF3\"", serializedXml);
            Assert.Contains("text=\"Eye Shape\"", serializedXml);
            Assert.Contains("id=\"05loYcST\"", serializedXml);
            Assert.Contains("text=\"Hit shield on back\"", serializedXml);
            Assert.Contains("id=\"05w8Tl7n\"", serializedXml);
            Assert.Contains("text=\"Steppe\"", serializedXml);
        }

        [Fact]
        public void NativeStrings_RoundTripSerialization()
        {
            // Arrange
            var originalXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <strings>
    <string id=""01zQKaF3"" text=""Eye Shape"" />
    <string id=""05loYcST"" text=""Hit shield on back"" />
  </strings>
</base>";

            // Act
            var deserialized = XmlTestUtils.Deserialize<NativeStringsDO>(originalXml);
            var serializedXml = XmlTestUtils.Serialize(deserialized);

            // Assert
            Assert.True(XmlTestUtils.AreStructurallyEqual(originalXml, serializedXml));
        }

        [Fact]
        public void NativeStrings_EmptyTags()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <strings>
    <string id=""01zQKaF3"" text=""Eye Shape"" />
    <string id=""05loYcST"" text=""Hit shield on back"" />
  </strings>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<NativeStringsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.Null(result.Tags);
            
            Assert.NotNull(result.Strings);
            Assert.Equal(2, result.Strings.StringList.Count);
        }

        [Fact]
        public void NativeStrings_EmptyStrings()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <strings />
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<NativeStringsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            
            Assert.NotNull(result.Tags);
            Assert.Single(result.Tags.TagList);
            Assert.Equal("English", result.Tags.TagList[0].Language);
            
            Assert.NotNull(result.Strings);
            Assert.Empty(result.Strings.StringList);
        }

        [Fact]
        public void NativeStrings_MultipleLanguageTags()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
    <tag language=""French"" />
    <tag language=""German"" />
  </tags>
  <strings>
    <string id=""01zQKaF3"" text=""Eye Shape"" />
  </strings>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<NativeStringsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            
            Assert.NotNull(result.Tags);
            Assert.Equal(3, result.Tags.TagList.Count);
            Assert.Equal("English", result.Tags.TagList[0].Language);
            Assert.Equal("French", result.Tags.TagList[1].Language);
            Assert.Equal("German", result.Tags.TagList[2].Language);
        }

        [Fact]
        public void NativeStrings_StringWithEmptyText()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <strings>
    <string id=""empty_string"" text="""" />
    <string id=""normal_string"" text=""Normal Text"" />
  </strings>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<NativeStringsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Strings);
            Assert.Equal(2, result.Strings.StringList.Count);
            
            var emptyString = result.Strings.StringList[0];
            Assert.Equal("empty_string", emptyString.Id);
            Assert.Equal("", emptyString.Text);
            
            var normalString = result.Strings.StringList[1];
            Assert.Equal("normal_string", normalString.Id);
            Assert.Equal("Normal Text", normalString.Text);
        }

        [Fact]
        public void NativeStrings_MinimalBase()
        {
            // Arrange
            var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base />";

            // Act
            var result = XmlTestUtils.Deserialize<NativeStringsDO>(xmlContent);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Type);
            Assert.Null(result.Tags);
            Assert.Null(result.Strings);
        }
    }
}