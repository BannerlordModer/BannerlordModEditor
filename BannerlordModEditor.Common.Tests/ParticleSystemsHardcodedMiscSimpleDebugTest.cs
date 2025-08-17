using System.IO;
using System.Xml.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsHardcodedMiscSimpleDebugTest
    {
        [Fact]
        public void ParticleSystemsHardcodedMisc1_Simple_Debug()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            // 临时输出结果以便查看
            File.WriteAllText("TestData/debug_original.xml", xml);
            File.WriteAllText("TestData/debug_serialized.xml", xml2);
            
            if (!areEqual)
            {
                // 详细差异分析
                var doc1 = XDocument.Parse(xml);
                var doc2 = XDocument.Parse(xml2);
                
                var elements1 = doc1.Descendants().Count();
                var elements2 = doc2.Descendants().Count();
                
                var attributes1 = doc1.Descendants().Sum(e => e.Attributes().Count());
                var attributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
                
                // 输出差异信息到文件
                var diffInfo = $"元素数量差异: {elements1} -> {elements2}\n";
                diffInfo += $"属性数量差异: {attributes1} -> {attributes2}\n";
                
                File.WriteAllText("TestData/debug_diff.txt", diffInfo);
            }
            
            Assert.True(areEqual, "XML结构化对比失败");
        }

        [Fact]
        public void ParticleSystemsHardcodedMisc2_Simple_Debug()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc2.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            // 临时输出结果以便查看
            File.WriteAllText("TestData/debug_original2.xml", xml);
            File.WriteAllText("TestData/debug_serialized2.xml", xml2);
            
            if (!areEqual)
            {
                // 详细差异分析
                var doc1 = XDocument.Parse(xml);
                var doc2 = XDocument.Parse(xml2);
                
                var elements1 = doc1.Descendants().Count();
                var elements2 = doc2.Descendants().Count();
                
                var attributes1 = doc1.Descendants().Sum(e => e.Attributes().Count());
                var attributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
                
                // 输出差异信息到文件
                var diffInfo = $"元素数量差异: {elements1} -> {elements2}\n";
                diffInfo += $"属性数量差异: {attributes1} -> {attributes2}\n";
                
                File.WriteAllText("TestData/debug_diff2.txt", diffInfo);
            }
            
            Assert.True(areEqual, "XML结构化对比失败");
        }
    }
}