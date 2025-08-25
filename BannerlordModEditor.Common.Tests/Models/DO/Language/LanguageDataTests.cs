using BannerlordModEditor.Common.Models.DO.Language;
using BannerlordModEditor.Common.Tests;

namespace BannerlordModEditor.Common.Tests.Models.DO.Language;

public class LanguageDataTests
{
    [Fact]
    public void LanguageData_RoundTripTest()
    {
        // Arrange
        var xmlPath = "TestData/language_data.xml";
        var xml = File.ReadAllText(xmlPath);

        // Act
        var result = XmlTestUtils.Deserialize<LanguageDataDO>(xml);
        var serializedXml = XmlTestUtils.Serialize(result, xml);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("English", result.Id);
        Assert.Equal("English", result.Name);
        Assert.Equal("en-GB", result.SubtitleExtension);
        Assert.Equal("TaleWorlds.Localization.TextProcessor.LanguageProcessors.EnglishTextProcessor", result.TextProcessor);
        Assert.False(result.UnderDevelopment);
        
        // 验证往返序列化结果
        var areEqual = XmlTestUtils.AreStructurallyEqual(xml, serializedXml);
        Assert.True(areEqual, $"往返序列化结果不匹配。原始XML长度: {xml.Length}, 序列化XML长度: {serializedXml.Length}");
    }

    [Fact]
    public void LanguageBase_Strings_RoundTripTest()
    {
        // Arrange
        var xmlPath = "TestData/std_common_strings_xml.xml";
        var xml = File.ReadAllText(xmlPath);

        // Act
        var result = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
        var serializedXml = XmlTestUtils.Serialize(result, xml);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("string", result.Type);
        Assert.NotNull(result.Tags);
        Assert.NotNull(result.Strings);
        Assert.True(result.Strings.Count > 0);
        
        // 验证第一个字符串项
        var firstString = result.Strings.FirstOrDefault();
        Assert.NotNull(firstString);
        Assert.Equal("028iCb8B", firstString.Id);
        Assert.Equal("Jythea", firstString.Text);
        
        // 验证往返序列化结果
        var areEqual = XmlTestUtils.AreStructurallyEqual(xml, serializedXml);
        Assert.True(areEqual, $"往返序列化结果不匹配。原始XML长度: {xml.Length}, 序列化XML长度: {serializedXml.Length}");
    }

    [Fact]
    public void LanguageBase_Functions_RoundTripTest()
    {
        // Arrange
        var xmlPath = "TestData/std_functions.xml";
        var xml = File.ReadAllText(xmlPath);

        // Act
        var result = XmlTestUtils.Deserialize<LanguageBaseDO>(xml);
        var serializedXml = XmlTestUtils.Serialize(result, xml);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("string", result.Type);
        Assert.NotNull(result.Tags);
        Assert.NotNull(result.Functions);
        Assert.True(result.Functions.Count > 0);
        
        // 验证第一个函数项
        var firstFunction = result.Functions.FirstOrDefault();
        Assert.NotNull(firstFunction);
        Assert.Equal("MAX", firstFunction.FunctionName);
        Assert.Equal("{?$0>$1}{$0}{?}{$1}{\\?}", firstFunction.FunctionBody);
        
        // 验证往返序列化结果
        var areEqual = XmlTestUtils.AreStructurallyEqual(xml, serializedXml);
        Assert.True(areEqual, $"往返序列化结果不匹配。原始XML长度: {xml.Length}, 序列化XML长度: {serializedXml.Length}");
    }
}