using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using BannerlordModEditor.Common.Tests;
using BannerlordModEditor.Common.Models.DO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🧪 开始运行ParticleSystems测试...");
        
        try
        {
            // 测试简化的ParticleSystems功能
            var testXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<particle_effects>
    <effect name=""test_effect"" guid=""12345"">
        <emitters>
            <emitter name=""test_emitter"">
                <parameters>
                    <parameter name=""test_param"" value=""1.0"" />
                    <decal_materials>
                        <decal_material value=""test_material"" />
                    </decal_materials>
                </parameters>
            </emitter>
        </emitters>
    </effect>
</particle_effects>";

            // 测试反序列化
            var particleSystems = XmlTestUtils.Deserialize<ParticleSystemsDO>(testXml);
            Console.WriteLine("✅ 反序列化成功");
            
            // 测试序列化
            var serialized = XmlTestUtils.Serialize(particleSystems, testXml);
            Console.WriteLine("✅ 序列化成功");
            
            // 验证结构一致性
            var isStructurallyEqual = XmlTestUtils.AreStructurallyEqual(testXml, serialized);
            Console.WriteLine($"📊 结构一致性测试: {(isStructurallyEqual ? "✅ 通过" : "❌ 失败")}");
            
            if (!isStructurallyEqual)
            {
                Console.WriteLine("❌ 测试失败：XML结构不一致");
                Console.WriteLine("原始XML:");
                Console.WriteLine(testXml);
                Console.WriteLine("序列化后XML:");
                Console.WriteLine(serialized);
            }
            else
            {
                Console.WriteLine("🎉 简化测试通过！");
            }
            
            // 尝试运行大型测试文件（如果存在）
            var largeXmlPath = "BannerlordModEditor.Common.Tests/TestData/particle_systems_hardcoded_misc1.xml";
            if (File.Exists(largeXmlPath))
            {
                Console.WriteLine("🔄 运行大型XML文件测试...");
                var largeXml = File.ReadAllText(largeXmlPath);
                
                try
                {
                    var largeParticleSystems = XmlTestUtils.Deserialize<ParticleSystemsDO>(largeXml);
                    var largeSerialized = XmlTestUtils.Serialize(largeParticleSystems, largeXml);
                    var largeIsEqual = XmlTestUtils.AreStructurallyEqual(largeXml, largeSerialized);
                    
                    Console.WriteLine($"📊 大型文件结构一致性测试: {(largeIsEqual ? "✅ 通过" : "❌ 失败")}");
                    
                    if (largeIsEqual)
                    {
                        Console.WriteLine("🎉 大型XML文件测试通过！修复成功！");
                    }
                    else
                    {
                        Console.WriteLine("❌ 大型XML文件测试仍然失败，需要进一步调查");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ 大型XML文件测试出错: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("⚠️ 大型测试文件不存在，跳过大型文件测试");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ 测试运行失败: {ex.Message}");
            Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
        }
    }
}