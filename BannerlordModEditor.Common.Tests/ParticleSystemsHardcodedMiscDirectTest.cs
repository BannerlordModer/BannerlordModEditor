using System.IO;
using System.Xml.Linq;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsHardcodedMiscDirectTest
    {
        [Fact]
        public void Analyze_ParticleSystemsHardcodedMisc1_Structure()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 直接分析XML结构
            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            
            // 记录基本统计信息
            var effectElements = root.Elements("effect").ToList();
            var totalElements = doc.Descendants().Count();
            var totalAttributes = doc.Descendants().Sum(e => e.Attributes().Count());
            
            // 分析第一个effect的详细结构
            if (effectElements.Count > 0)
            {
                var firstEffect = effectElements[0];
                var emittersElement = firstEffect.Element("emitters");
                
                if (emittersElement != null)
                {
                    var emitterElements = emittersElement.Elements("emitter").ToList();
                    
                    if (emitterElements.Count > 0)
                    {
                        var firstEmitter = emitterElements[0];
                        var childrenElement = firstEmitter.Element("children");
                        var flagsElement = firstEmitter.Element("flags");
                        var parametersElement = firstEmitter.Element("parameters");
                        
                        // 检查children元素的结构
                        if (childrenElement != null)
                        {
                            var childEmitters = childrenElement.Elements("emitter").ToList();
                        }
                    }
                }
            }
            
            // 尝试反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的结构
            var doc2 = XDocument.Parse(xml2);
            var totalElements2 = doc2.Descendants().Count();
            var totalAttributes2 = doc2.Descendants().Sum(e => e.Attributes().Count());
            
            // 比较差异
            var elementsDiff = totalElements - totalElements2;
            var attributesDiff = totalAttributes - totalAttributes2;
            
            // 如果有差异，记录详细信息
            if (elementsDiff != 0 || attributesDiff != 0)
            {
                // 创建详细的差异报告
                var diffReport = $"原始XML: {totalElements} 元素, {totalAttributes} 属性\n";
                diffReport += $"序列化后: {totalElements2} 元素, {totalAttributes2} 属性\n";
                diffReport += $"差异: {elementsDiff} 元素, {attributesDiff} 属性\n";
                
                // 检查特定元素类型的数量差异
                var originalFlags = doc.Descendants("flag").Count();
                var serializedFlags = doc2.Descendants("flag").Count();
                diffReport += $"Flag元素: {originalFlags} -> {serializedFlags}\n";
                
                var originalParameters = doc.Descendants("parameter").Count();
                var serializedParameters = doc2.Descendants("parameter").Count();
                diffReport += $"Parameter元素: {originalParameters} -> {serializedParameters}\n";
                
                var originalEmitters = doc.Descendants("emitter").Count();
                var serializedEmitters = doc2.Descendants("emitter").Count();
                diffReport += $"Emitter元素: {originalEmitters} -> {serializedEmitters}\n";
                
                // 保存差异报告
                File.WriteAllText("TestData/analysis_report.txt", diffReport);
            }
            
            // 最终的结构化对比
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            Assert.True(areEqual, $"XML结构化对比失败。元素差异: {elementsDiff}, 属性差异: {attributesDiff}");
        }
    }
}