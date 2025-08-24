using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 文件发现服务的实现
    /// </summary>
    public class FileDiscoveryService : IFileDiscoveryService
    {
        private readonly string _xmlDirectory;
        private readonly string _modelsRootDirectory;

        public FileDiscoveryService(string xmlDirectory = "example/ModuleData", 
                                  string modelsRootDirectory = "BannerlordModEditor.Common/Models")
        {
            _xmlDirectory = xmlDirectory;
            _modelsRootDirectory = modelsRootDirectory;
        }

        public async Task<List<UnadaptedFile>> FindUnadaptedFilesAsync()
        {
            return await Task.Run(() =>
            {
                var unadaptedFiles = new List<UnadaptedFile>();

                if (!Directory.Exists(_xmlDirectory))
                {
                    throw new DirectoryNotFoundException($"XML 目录不存在: {_xmlDirectory}");
                }

                var searchDirectories = GetModelSearchDirectories();
                var xmlFiles = Directory.GetFiles(_xmlDirectory, "*.xml", SearchOption.AllDirectories);

                foreach (var xmlFile in xmlFiles)
                {
                    var fileName = Path.GetFileName(xmlFile);
                    var baseName = Path.GetFileNameWithoutExtension(fileName);
                    var expectedModelName = ConvertToModelName(baseName);

                    if (!ModelExists(expectedModelName, searchDirectories))
                    {
                        var fileInfo = new FileInfo(xmlFile);
                        var complexity = DetermineComplexity(fileInfo);

                        unadaptedFiles.Add(new UnadaptedFile
                        {
                            FileName = fileName,
                            FullPath = xmlFile,
                            FileSize = fileInfo.Length,
                            ExpectedModelName = expectedModelName,
                            Complexity = complexity
                        });
                    }
                }

                return unadaptedFiles.OrderBy(f => f.Complexity).ThenBy(f => f.FileName).ToList();
            });
        }

        public string ConvertToModelName(string xmlFileName)
        {
            if (string.IsNullOrEmpty(xmlFileName))
                return string.Empty;

            // 移除 .xml 扩展名（如果存在）
            var baseName = xmlFileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) 
                ? xmlFileName[..^4] 
                : xmlFileName;

            // 使用命名约定映射器进行转换
            return NamingConventionMapper.GetMappedClassName(baseName);
        }

        public bool ModelExists(string modelName, string[] searchDirectories)
        {
            var expectedFileName = $"{modelName}.cs";
            var pluralFileName = $"{modelName}s.cs";

            foreach (var directory in searchDirectories)
            {
                if (!Directory.Exists(directory))
                    continue;

                // 检查标准名称
                if (File.Exists(Path.Combine(directory, expectedFileName)))
                    return true;

                // 检查复数形式
                if (File.Exists(Path.Combine(directory, pluralFileName)))
                    return true;
            }

            return false;
        }

        public bool IsFileAdapted(string xmlFileName)
        {
            if (string.IsNullOrEmpty(xmlFileName))
                return false;

            var baseName = Path.GetFileNameWithoutExtension(xmlFileName);
            var expectedModelName = ConvertToModelName(baseName);
            var searchDirectories = GetModelSearchDirectories();

            return ModelExists(expectedModelName, searchDirectories);
        }

        private string[] GetModelSearchDirectories()
        {
            var directories = new List<string> { _modelsRootDirectory };

            if (Directory.Exists(_modelsRootDirectory))
            {
                var subdirectories = Directory.GetDirectories(_modelsRootDirectory, "*", SearchOption.AllDirectories)
                    .Where(d => !Path.GetFileName(d).Equals("bin", StringComparison.OrdinalIgnoreCase) &&
                               !Path.GetFileName(d).Equals("obj", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                directories.AddRange(subdirectories);
            }

            return directories.ToArray();
        }

        private static string ToPascalCase(string snakeCaseString)
        {
            if (string.IsNullOrEmpty(snakeCaseString))
                return string.Empty;

            return string.Join("", snakeCaseString.Split('_')
                .Select(word => char.ToUpperInvariant(word[0]) + word[1..].ToLowerInvariant()));
        }

        private static AdaptationComplexity DetermineComplexity(FileInfo fileInfo)
        {
            // 基于文件大小的简单启发式判断
            var sizeInKB = fileInfo.Length / 1024.0;

            return sizeInKB switch
            {
                > 1024 => AdaptationComplexity.Large,  // > 1MB
                > 100 => AdaptationComplexity.Complex,  // > 100KB
                > 10 => AdaptationComplexity.Medium,    // > 10KB
                _ => AdaptationComplexity.Simple        // <= 10KB
            };
        }
    }
}