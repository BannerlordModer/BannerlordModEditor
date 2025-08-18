using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsDetailedAnalysis
    {
        [Fact]
        public void AnalyzeDetailedDifferences()
        {
            Console.WriteLine("=== FloraKinds 详细差异分析 ===");
            
            var originalPath = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common.Tests/bin/Debug/net9.0/flora_kinds_original.xml";
            var serializedPath = "/root/WorkSpace/CSharp/BannerlordModEditor/BannerlordModEditor.Common.Tests/bin/Debug/net9.0/flora_kinds_serialized.xml";
            
            var originalDoc = XDocument.Load(originalPath);
            var serializedDoc = XDocument.Load(serializedPath);
            
            // 统计属性数量
            var originalAttrs = originalDoc.Descendants().SelectMany(e => e.Attributes()).ToList();
            var serializedAttrs = serializedDoc.Descendants().SelectMany(e => e.Attributes()).ToList();
            
            Console.WriteLine($"原始属性数量: {originalAttrs.Count}");
            Console.WriteLine($"序列化属性数量: {serializedAttrs.Count}");
            
            // 找出缺失的属性
            var originalAttrSet = new HashSet<string>();
            foreach (var attr in originalAttrs)
            {
                originalAttrSet.Add($"{attr.Parent.Name.LocalName}.{attr.Name}={attr.Value}");
            }
            
            var serializedAttrSet = new HashSet<string>();
            foreach (var attr in serializedAttrs)
            {
                serializedAttrSet.Add($"{attr.Parent.Name.LocalName}.{attr.Name}={attr.Value}");
            }
            
            var missingAttrs = originalAttrSet.Except(serializedAttrSet).ToList();
            var extraAttrs = serializedAttrSet.Except(originalAttrSet).ToList();
            
            Console.WriteLine($"缺失属性数量: {missingAttrs.Count}");
            Console.WriteLine($"多余属性数量: {extraAttrs.Count}");
            
            if (missingAttrs.Count > 0)
            {
                Console.WriteLine("缺失的属性:");
                foreach (var attr in missingAttrs.Take(10))
                {
                    Console.WriteLine($"  - {attr}");
                }
            }
            
            if (extraAttrs.Count > 0)
            {
                Console.WriteLine("多余的属性:");
                foreach (var attr in extraAttrs.Take(10))
                {
                    Console.WriteLine($"  - {attr}");
                }
            }
            
            // 检查特定的flora_kind元素
            var originalFloraKinds = originalDoc.Root.Elements("flora_kind").ToList();
            var serializedFloraKinds = serializedDoc.Root.Elements("flora_kind").ToList();
            
            Console.WriteLine($"原始flora_kind元素数量: {originalFloraKinds.Count}");
            Console.WriteLine($"序列化flora_kind元素数量: {serializedFloraKinds.Count}");
            
            // 找出属性数量不同的元素
            for (int i = 0; i < Math.Min(originalFloraKinds.Count, serializedFloraKinds.Count); i++)
            {
                var original = originalFloraKinds[i];
                var serialized = serializedFloraKinds[i];
                
                var originalAttrsInElement = original.Attributes().ToList();
                var serializedAttrsInElement = serialized.Attributes().ToList();
                
                if (originalAttrsInElement.Count != serializedAttrsInElement.Count)
                {
                    var name = original.Attribute("name")?.Value ?? "unnamed";
                    Console.WriteLine($"发现差异元素 {i} (name={name}):");
                    Console.WriteLine($"  原始属性数量: {originalAttrsInElement.Count}");
                    Console.WriteLine($"  序列化属性数量: {serializedAttrsInElement.Count}");
                    
                    // 找出具体差异的属性
                    var originalAttrNames = originalAttrsInElement.Select(a => a.Name.LocalName).ToHashSet();
                    var serializedAttrNames = serializedAttrsInElement.Select(a => a.Name.LocalName).ToHashSet();
                    
                    var missingInElement = originalAttrNames.Except(serializedAttrNames).ToList();
                    var extraInElement = serializedAttrNames.Except(originalAttrNames).ToList();
                    
                    if (missingInElement.Count > 0)
                    {
                        Console.WriteLine($"  缺失属性: {string.Join(", ", missingInElement)}");
                    }
                    
                    if (extraInElement.Count > 0)
                    {
                        Console.WriteLine($"  多余属性: {string.Join(", ", extraInElement)}");
                    }
                    
                    break;
                }
            }
            
            // 保存详细分析结果
            File.WriteAllText("flora_kinds_detailed_analysis.txt", 
                $"原始属性数量: {originalAttrs.Count}\n" +
                $"序列化属性数量: {serializedAttrs.Count}\n" +
                $"缺失属性数量: {missingAttrs.Count}\n" +
                $"多余属性数量: {extraAttrs.Count}\n" +
                $"缺失属性: {string.Join(", ", missingAttrs.Take(5))}\n" +
                $"多余属性: {string.Join(", ", extraAttrs.Take(5))}");
        }
    }
}