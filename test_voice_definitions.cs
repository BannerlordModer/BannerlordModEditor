using System;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Audio;
using BannerlordModEditor.Common.Tests;

class TestVoiceDefinitions
{
    static void Main()
    {
        Console.WriteLine("Testing VoiceDefinitionsDO...");
        
        // Test case 1: Empty voice_type_declarations
        var xml1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
    <voice_type_declarations>
    </voice_type_declarations>
    <voice_definition name=""test"" />
</voice_definitions>";

        try
        {
            var result1 = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xml1);
            Console.WriteLine($"Test 1 - HasVoiceTypeDeclarations: {result1.HasVoiceTypeDeclarations}");
            Console.WriteLine($"Test 1 - VoiceTypes count: {result1.VoiceTypeDeclarations.VoiceTypes.Count}");
            
            var serialized1 = XmlTestUtils.Serialize(result1, xml1);
            Console.WriteLine($"Test 1 - Serialized XML contains voice_type_declarations: {serialized1.Contains("voice_type_declarations")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test 1 failed: {ex.Message}");
        }
        
        // Test case 2: No voice_type_declarations
        var xml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<voice_definitions>
    <voice_definition name=""test"" />
</voice_definitions>";

        try
        {
            var result2 = XmlTestUtils.Deserialize<VoiceDefinitionsDO>(xml2);
            Console.WriteLine($"Test 2 - HasVoiceTypeDeclarations: {result2.HasVoiceTypeDeclarations}");
            
            var serialized2 = XmlTestUtils.Serialize(result2, xml2);
            Console.WriteLine($"Test 2 - Serialized XML contains voice_type_declarations: {serialized2.Contains("voice_type_declarations")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test 2 failed: {ex.Message}");
        }
        
        Console.WriteLine("Testing complete.");
    }
}