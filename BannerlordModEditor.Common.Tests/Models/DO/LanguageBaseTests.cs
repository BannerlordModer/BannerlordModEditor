using System.IO;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Language;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests.Models.DO
{
    public class LanguageBaseTests
    {
        [Fact]
        public void LanguageBaseDO_Deserialize_SimpleFunctions()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
   <functions>
	<function functionName=""MAX"" functionBody=""{?$0>$1}{$0}{?}{$1}{?}""/>
	<function functionName=""POW2"" functionBody=""{?$0==0}1{?}{2*POW2($0 - 1)}{?}""/>
	<function functionName=""ADDITION"" functionBody=""$0+$1""/>
  </functions>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Tags);
            Assert.Single(result.Tags.Tags);
            Assert.Equal("English", result.Tags.Tags[0].Language);
            Assert.NotNull(result.Functions);
            Assert.Equal(3, result.Functions.Count);
            Assert.Equal("MAX", result.Functions[0].FunctionName);
            Assert.Equal("{?$0>$1}{$0}{?}{$1}{?}", result.Functions[0].FunctionBody);
        }

        [Fact]
        public void LanguageBaseDO_Deserialize_Strings()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <strings>
    <string id=""test1"" text=""Hello World"" />
    <string id=""test2"" text=""Goodbye"" />
  </strings>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Tags);
            Assert.Single(result.Tags.Tags);
            Assert.Equal("English", result.Tags.Tags[0].Language);
            Assert.NotNull(result.Strings);
            Assert.Equal(2, result.Strings.Count);
            Assert.Equal("test1", result.Strings[0].Id);
            Assert.Equal("Hello World", result.Strings[0].Text);
            Assert.Equal("test2", result.Strings[1].Id);
            Assert.Equal("Goodbye", result.Strings[1].Text);
        }

        [Fact]
        public void LanguageBaseDO_Deserialize_BothFunctionsAndStrings()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
   <functions>
	<function functionName=""PLURAL"" functionBody=""{?$0.Plural}{$0.Plural}{?}{$0}{.s}{?}""/>
  </functions>
  <strings>
    <string id=""hello"" text=""Hello"" />
    <string id=""world"" text=""World"" />
  </strings>
</base>";

            // Act
            var result = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("string", result.Type);
            Assert.NotNull(result.Tags);
            Assert.Single(result.Tags.Tags);
            Assert.Equal("English", result.Tags.Tags[0].Language);
            Assert.NotNull(result.Functions);
            Assert.Single(result.Functions);
            Assert.Equal("PLURAL", result.Functions[0].FunctionName);
            Assert.NotNull(result.Strings);
            Assert.Equal(2, result.Strings.Count);
            Assert.Equal("hello", result.Strings[0].Id);
            Assert.Equal("Hello", result.Strings[0].Text);
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_FunctionsOnly()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
   <functions>
	<function functionName=""MAX"" functionBody=""{?$0>$1}{$0}{?}{$1}{?}""/>
	<function functionName=""POW2"" functionBody=""{?$0==0}1{?}{2*POW2($0 - 1)}{?}""/>
  </functions>
</base>";

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_StringsOnly()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
  <strings>
    <string id=""test1"" text=""Hello World"" />
    <string id=""test2"" text=""Goodbye"" />
  </strings>
</base>";

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_Complex()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
    <tag language=""English"" />
  </tags>
   <functions>
	<function functionName=""PLURAL"" functionBody=""{?$0.Plural}{$0.Plural}{?}{$0}{.s}{?}""/>
	<function functionName=""IS_PLURAL"" functionBody=""{?$0!.P}1{:?$0.Plural}1{:}0{?}""/>
  </functions>
  <strings>
    <string id=""hello"" text=""Hello"" />
    <string id=""world"" text=""World"" />
    <string id=""test"" text=""{PLUS_OR_MINUS}{BONUSEFFECT}"" />
  </strings>
</base>";

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_EmptyTags()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
  <tags>
  </tags>
</base>";

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_NoTags()
        {
            // Arrange
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<base xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" type=""string"">
</base>";

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_RealFunctionFile()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "std_functions.xml");
            if (!File.Exists(xmlPath))
            {
                // Skip test if file doesn't exist
                return;
            }
            
            string xml = await File.ReadAllTextAsync(xmlPath);

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }

        [Fact]
        public async Task LanguageBaseDO_XmlSerialization_ShouldBeRoundTripValid_RealStringFile()
        {
            // Arrange
            string xmlPath = Path.Combine("TestData", "std_TaleWorlds_Core.xml");
            if (!File.Exists(xmlPath))
            {
                // Skip test if file doesn't exist
                return;
            }
            
            string xml = await File.ReadAllTextAsync(xmlPath);

            // Act - Round Trip Test
            var deserialized = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
            var serialized = XmlTestUtils.Serialize(deserialized, xml);
            var roundTripResult = XmlTestUtils.Deserialize<LanguageBaseDO>(serialized);

            // Assert
            Assert.NotNull(deserialized);
            Assert.NotNull(roundTripResult);
            Assert.True(XmlTestUtils.AreStructurallyEqual(xml, serialized));
        }
    }
}