using System.IO;
using Xunit;
using BannerlordModEditor.Common.Models.DO;
using System.Linq;

namespace BannerlordModEditor.Common.Tests
{
    public class ParticleSystemsHardcodedMiscDetailedDebugTest
    {
        [Fact]
        public void Debug_ParticleSystemsHardcodedMisc1_DetailedAnalysis()
        {
            var xmlPath = "TestData/particle_systems_hardcoded_misc1.xml";
            var xml = File.ReadAllText(xmlPath);

            // 反序列化
            var obj = XmlTestUtils.Deserialize<ParticleSystemsDO>(xml);

            // 再序列化（传递原始XML以保留命名空间）
            var xml2 = XmlTestUtils.Serialize(obj, xml);

            // 获取详细的差异报告
            var report = XmlTestUtils.CompareXmlStructure(xml, xml2);
            
            // 输出调试信息
            var (nodeCount1, attrCount1) = XmlTestUtils.CountNodesAndAttributes(xml);
            var (nodeCount2, attrCount2) = XmlTestUtils.CountNodesAndAttributes(xml2);
            
            // 保存调试文件
            var debugPath = Path.Combine("Debug", "particlesystems_debug");
            Directory.CreateDirectory(debugPath);
            
            File.WriteAllText(Path.Combine(debugPath, "original.xml"), xml);
            File.WriteAllText(Path.Combine(debugPath, "serialized.xml"), xml2);
            
            var diffReport = $"=== ParticleSystems 硬编码杂项调试报告 ===\n" +
                            $"原始XML节点数: {nodeCount1}, 属性数: {attrCount1}\n" +
                            $"序列化XML节点数: {nodeCount2}, 属性数: {attrCount2}\n" +
                            $"节点差异: {nodeCount1 - nodeCount2}\n" +
                            $"属性差异: {attrCount1 - attrCount2}\n" +
                            $"IsStructurallyEqual: {report.IsStructurallyEqual}\n\n" +
                            $"缺失节点: {string.Join(", ", report.MissingNodes.Take(10))}\n" +
                            $"多余节点: {string.Join(", ", report.ExtraNodes.Take(10))}\n" +
                            $"节点名差异: {string.Join(", ", report.NodeNameDifferences.Take(10))}\n" +
                            $"缺失属性: {string.Join(", ", report.MissingAttributes.Take(10))}\n" +
                            $"多余属性: {string.Join(", ", report.ExtraAttributes.Take(10))}\n" +
                            $"属性值差异: {string.Join(", ", report.AttributeValueDifferences.Take(10))}\n" +
                            $"文本差异: {string.Join(", ", report.TextDifferences.Take(10))}\n" +
                            $"节点数量差异: {report.NodeCountDifference}\n" +
                            $"属性数量差异: {report.AttributeCountDifference}\n";
            
            File.WriteAllText(Path.Combine(debugPath, "diff_report.txt"), diffReport);
            
            // 分析DecalMaterials问题
            var decalMaterialIssues = AnalyzeDecalMaterials(obj, xml);
            File.WriteAllText(Path.Combine(debugPath, "decal_materials_analysis.txt"), decalMaterialIssues);
            
            // 分析曲线元素问题
            var curveIssues = AnalyzeCurveElements(obj, xml);
            File.WriteAllText(Path.Combine(debugPath, "curve_analysis.txt"), curveIssues);
            
            // 即使测试失败，也输出调试信息
            Assert.True(report.IsStructurallyEqual, 
                $"XML结构不匹配。节点数: {nodeCount1} -> {nodeCount2}, 属性数: {attrCount1} -> {attrCount2}\n" +
                $"主要问题: {report.NodeCountDifference ?? "无"} {report.AttributeCountDifference ?? "无"}\n" +
                $"详细报告已保存到: {debugPath}");
        }
        
        private string AnalyzeDecalMaterials(ParticleSystemsDO particleSystems, string xml)
        {
            var report = "=== DecalMaterials 分析 ===\n";
            
            // 统计DO对象中的DecalMaterials
            int decalMaterialCount = 0;
            int emptyDecalMaterialCount = 0;
            
            if (particleSystems.Effects != null)
            {
                foreach (var effect in particleSystems.Effects)
                {
                    if (effect.Emitters?.EmitterList != null)
                    {
                        foreach (var emitter in effect.Emitters.EmitterList)
                        {
                            if (emitter.Parameters?.DecalMaterials != null)
                            {
                                decalMaterialCount++;
                                if (emitter.Parameters.DecalMaterials.DecalMaterialList.Count == 0)
                                {
                                    emptyDecalMaterialCount++;
                                }
                                report += $"Emitter: {emitter.Name}, DecalMaterials数量: {emitter.Parameters.DecalMaterials.DecalMaterialList.Count}\n";
                                report += $"HasDecalMaterials: {emitter.Parameters.HasDecalMaterials}\n";
                            }
                        }
                    }
                }
            }
            
            report += $"\n统计: 总共 {decalMaterialCount} 个DecalMaterials, 其中 {emptyDecalMaterialCount} 个为空\n";
            
            // 分析原始XML中的DecalMaterials
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            var decalMaterialElements = doc.Descendants("decal_materials").ToList();
            report += $"原始XML中的decal_materials元素数: {decalMaterialElements.Count}\n";
            
            foreach (var element in decalMaterialElements.Take(5))
            {
                var decalMaterialChildren = element.Elements("decal_material").Count();
                report += $"decal_materials子元素数: {decalMaterialChildren}\n";
            }
            
            return report;
        }
        
        private string AnalyzeCurveElements(ParticleSystemsDO particleSystems, string xml)
        {
            var report = "=== 曲线元素分析 ===\n";
            
            // 统计DO对象中的曲线元素
            int curveCount = 0;
            int keysCount = 0;
            int keyCount = 0;
            
            if (particleSystems.Effects != null)
            {
                foreach (var effect in particleSystems.Effects)
                {
                    if (effect.Emitters?.EmitterList != null)
                    {
                        foreach (var emitter in effect.Emitters.EmitterList)
                        {
                            if (emitter.Parameters?.ParameterList != null)
                            {
                                foreach (var parameter in emitter.Parameters.ParameterList)
                                {
                                    if (parameter.ParameterCurve != null)
                                    {
                                        curveCount++;
                                        if (parameter.ParameterCurve.Keys != null)
                                        {
                                            keysCount++;
                                            keyCount += parameter.ParameterCurve.Keys.KeyList.Count;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            report += $"统计: Curve: {curveCount}, Keys: {keysCount}, Key: {keyCount}\n";
            
            // 分析原始XML中的曲线元素
            var doc = System.Xml.Linq.XDocument.Parse(xml);
            var curveElements = doc.Descendants("curve").ToList();
            var keysElements = doc.Descendants("keys").ToList();
            var keyElements = doc.Descendants("key").ToList();
            
            report += $"原始XML中的curve元素数: {curveElements.Count}\n";
            report += $"原始XML中的keys元素数: {keysElements.Count}\n";
            report += $"原始XML中的key元素数: {keyElements.Count}\n";
            
            return report;
        }
    }
}