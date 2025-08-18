using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Services;

class XmlAnalyzer
{
    static void Main()
    {
        Console.WriteLine("=== Bannerlord XML 文件适配分析 ===");
        
        var discoveryService = new FileDiscoveryService();
        var unadaptedFiles = discoveryService.FindUnadaptedFilesAsync().Result;
        
        Console.WriteLine($"\n发现 {unadaptedFiles.Count} 个未适配的 XML 文件:");
        Console.WriteLine(new string('-', 60));
        
        foreach (var file in unadaptedFiles)
        {
            Console.WriteLine($"文件名: {file.FileName}");
            Console.WriteLine($"期望的模型名: {file.ExpectedModelName}");
            Console.WriteLine($"文件大小: {file.FileSize / 1024.0:F2} KB");
            Console.WriteLine($"复杂度: {file.Complexity}");
            Console.WriteLine($"建议的命名空间: {NamingConventionMapper.GetSuggestedNamespace(file.FileName)}");
            Console.WriteLine(new string('-', 40));
        }
        
        // 按复杂度分组统计
        var complexityGroups = unadaptedFiles.GroupBy(f => f.Complexity);
        Console.WriteLine("\n按复杂度分组统计:");
        foreach (var group in complexityGroups)
        {
            Console.WriteLine($"{group.Key}: {group.Count()} 个文件");
        }
        
        // 按命名空间分组统计
        var namespaceGroups = unadaptedFiles.GroupBy(f => NamingConventionMapper.GetSuggestedNamespace(f.FileName));
        Console.WriteLine("\n按命名空间分组统计:");
        foreach (var group in namespaceGroups)
        {
            Console.WriteLine($"{group.Key}: {group.Count()} 个文件");
        }
        
        // 按文件大小排序，显示最大的文件
        Console.WriteLine("\n最大的 10 个文件:");
        var largestFiles = unadaptedFiles.OrderByDescending(f => f.FileSize).Take(10);
        foreach (var file in largestFiles)
        {
            Console.WriteLine($"{file.FileName}: {file.FileSize / 1024.0:F2} KB");
        }
    }
}