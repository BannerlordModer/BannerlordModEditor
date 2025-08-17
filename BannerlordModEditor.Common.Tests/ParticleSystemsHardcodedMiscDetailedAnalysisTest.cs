using System.IO;
using System.Xml.Linq;
using System.Linq;
using Xunit;
using BannerlordModEditor.Common.Models.DO;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsHardcodedMiscDetailedAnalysisTest
    {
        [Fact]
        public void DetailedAnalysis_ParticleSystemsHardcodedMisc1()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);
            
            // 分析原始XML
            var doc = XDocument.Parse(xml);
            var originalStats = AnalyzeXmlStructure(doc);
            
            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);
            
            // 再序列化
            var xml2 = XmlTestUtils.Serialize(obj, xml);
            
            // 分析序列化后的XML
            var doc2 = XDocument.Parse(xml2);
            var serializedStats = AnalyzeXmlStructure(doc2);
            
            // 比较差异
            var diffReport = GenerateDifferenceReport(originalStats, serializedStats);
            
            // 保存差异报告
            File.WriteAllText("TestData/detailed_analysis_report.txt", diffReport);
            
            // 检查是否有未处理的元素类型
            var missingElements = FindMissingElementTypes(doc, doc2);
            if (missingElements.Count > 0)
            {
                var missingReport = "未处理的元素类型:\n";
                foreach (var element in missingElements)
                {
                    missingReport += $"  - {element}\n";
                }
                File.WriteAllText("TestData/missing_elements.txt", missingReport);
            }
            
            // 最终验证
            var areEqual = XmlTestUtils.AreStructurallyEqual(xml, xml2);
            
            Assert.True(areEqual, $"XML结构化对比失败。详细差异:\n{diffReport}");
        }
        
        private XmlStructureStats AnalyzeXmlStructure(XDocument doc)
        {
            var stats = new XmlStructureStats();
            
            stats.TotalElements = doc.Descendants().Count();
            stats.TotalAttributes = doc.Descendants().Sum(e => e.Attributes().Count());
            
            stats.ElementTypes = doc.Descendants()
                .GroupBy(e => e.Name.LocalName)
                .ToDictionary(g => g.Key, g => g.Count());
                
            stats.EffectCount = doc.Root.Elements("effect").Count();
            
            var firstEffect = doc.Root.Elements("effect").FirstOrDefault();
            if (firstEffect != null)
            {
                stats.FirstEffectHasDecalMaterials = firstEffect.Element("decal_materials") != null;
                
                var emitters = firstEffect.Element("emitters");
                if (emitters != null)
                {
                    stats.FirstEffectEmitterCount = emitters.Elements("emitter").Count();
                    
                    var firstEmitter = emitters.Elements("emitter").FirstOrDefault();
                    if (firstEmitter != null)
                    {
                        stats.FirstEmitterHasChildren = firstEmitter.Element("children") != null;
                        stats.FirstEmitterHasFlags = firstEmitter.Element("flags") != null;
                        stats.FirstEmitterHasParameters = firstEmitter.Element("parameters") != null;
                    }
                }
            }
            
            // 统计特定的复杂元素
            stats.DecalMaterialCount = doc.Descendants("decal_material").Count();
            stats.DecalMaterialsCount = doc.Descendants("decal_materials").Count();
            
            return stats;
        }
        
        private string GenerateDifferenceReport(XmlStructureStats original, XmlStructureStats serialized)
        {
            var report = "=== XML结构差异分析 ===\n\n";
            
            report += $"基本统计:\n";
            report += $"  原始元素数: {original.TotalElements}\n";
            report += $"  序列化元素数: {serialized.TotalElements}\n";
            report += $"  元素差异: {original.TotalElements - serialized.TotalElements}\n\n";
            
            report += $"  原始属性数: {original.TotalAttributes}\n";
            report += $"  序列化属性数: {serialized.TotalAttributes}\n";
            report += $"  属性差异: {original.TotalAttributes - serialized.TotalAttributes}\n\n";
            
            report += $"Effect统计:\n";
            report += $"  原始Effect数: {original.EffectCount}\n";
            report += $"  序列化Effect数: {serialized.EffectCount}\n\n";
            
            report += $"DecalMaterial统计:\n";
            report += $"  原始decal_material数: {original.DecalMaterialCount}\n";
            report += $"  序列化decal_material数: {serialized.DecalMaterialCount}\n";
            report += $"  原始decal_materials数: {original.DecalMaterialsCount}\n";
            report += $"  序列化decal_materials数: {serialized.DecalMaterialsCount}\n\n";
            
            report += "元素类型差异:\n";
            var allElementTypes = original.ElementTypes.Keys.Union(serialized.ElementTypes.Keys).OrderBy(x => x);
            foreach (var elementType in allElementTypes)
            {
                var originalCount = original.ElementTypes.ContainsKey(elementType) ? original.ElementTypes[elementType] : 0;
                var serializedCount = serialized.ElementTypes.ContainsKey(elementType) ? serialized.ElementTypes[elementType] : 0;
                
                if (originalCount != serializedCount)
                {
                    report += $"  {elementType}: {originalCount} -> {serializedCount} (差异: {originalCount - serializedCount})\n";
                }
            }
            
            return report;
        }
        
        private System.Collections.Generic.List<string> FindMissingElementTypes(XDocument original, XDocument serialized)
        {
            var missing = new System.Collections.Generic.List<string>();
            
            var originalElements = original.Descendants().Select(e => e.Name.LocalName).Distinct().ToList();
            var serializedElements = serialized.Descendants().Select(e => e.Name.LocalName).Distinct().ToList();
            
            foreach (var element in originalElements)
            {
                if (!serializedElements.Contains(element))
                {
                    missing.Add(element);
                }
            }
            
            return missing;
        }
        
        private class XmlStructureStats
        {
            public int TotalElements { get; set; }
            public int TotalAttributes { get; set; }
            public System.Collections.Generic.Dictionary<string, int> ElementTypes { get; set; } = new System.Collections.Generic.Dictionary<string, int>();
            public int EffectCount { get; set; }
            public bool FirstEffectHasDecalMaterials { get; set; }
            public int FirstEffectEmitterCount { get; set; }
            public bool FirstEmitterHasChildren { get; set; }
            public bool FirstEmitterHasFlags { get; set; }
            public bool FirstEmitterHasParameters { get; set; }
            public int DecalMaterialCount { get; set; }
            public int DecalMaterialsCount { get; set; }
        }
    }
}