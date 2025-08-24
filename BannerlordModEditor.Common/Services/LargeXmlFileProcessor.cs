using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BannerlordModEditor.Common.Models.DO.Engine;
using BannerlordModEditor.Common.Models.DO.Multiplayer;

namespace BannerlordModEditor.Common.Services
{
    /// <summary>
    /// 大型XML文件处理器
    /// 专门用于处理大型XML文件的高效加载和处理
    /// 支持分块处理、内存优化和并行处理
    /// </summary>
    public class LargeXmlFileProcessor
    {
        private const int DefaultChunkSize = 1000;
        private const int MaxMemoryUsageMB = 500;
        
        /// <summary>
        /// 处理大型地形材质XML文件
        /// </summary>
        public async Task<TerrainMaterialsDO> ProcessLargeTerrainMaterialsFileAsync(string filePath)
        {
            return await ProcessLargeFileAsync<TerrainMaterialsDO, TerrainMaterialDO>(
                filePath,
                "terrain_material",
                (materials, chunk) =>
                {
                    materials.TerrainMaterialList.AddRange(chunk);
                    materials.ProcessedChunks++;
                },
                DefaultChunkSize
            );
        }
        
        /// <summary>
        /// 处理大型多人游戏分类XML文件
        /// </summary>
        public async Task<MPClassDivisionsDO> ProcessLargeMPClassDivisionsFileAsync(string filePath)
        {
            return await ProcessLargeFileAsync<MPClassDivisionsDO, MPClassDivisionDO>(
                filePath,
                "MPClassDivision",
                (divisions, chunk) =>
                {
                    divisions.MPClassDivisions.AddRange(chunk);
                    divisions.ProcessedChunks++;
                    
                    // 增量更新索引
                    foreach (var division in chunk)
                    {
                        var culture = divisions.ExtractCultureFromId(division.Id);
                        if (!divisions.ClassDivisionsByCulture.ContainsKey(culture))
                        {
                            divisions.ClassDivisionsByCulture[culture] = new List<MPClassDivisionDO>();
                        }
                        divisions.ClassDivisionsByCulture[culture].Add(division);

                        foreach (var perk in division.Perks?.PerksList ?? new List<PerkDO>())
                        {
                            var gameMode = perk.GameMode;
                            if (!string.IsNullOrEmpty(gameMode))
                            {
                                if (!divisions.ClassDivisionsByGameMode.ContainsKey(gameMode))
                                {
                                    divisions.ClassDivisionsByGameMode[gameMode] = new List<MPClassDivisionDO>();
                                }
                                if (!divisions.ClassDivisionsByGameMode[gameMode].Contains(division))
                                {
                                    divisions.ClassDivisionsByGameMode[gameMode].Add(division);
                                }
                            }
                        }
                    }
                },
                DefaultChunkSize
            );
        }
        
        /// <summary>
        /// 通用大型文件处理方法
        /// </summary>
        private async Task<T> ProcessLargeFileAsync<T, TElement>(
            string filePath,
            string elementName,
            Action<T, List<TElement>> chunkProcessor,
            int chunkSize) where T : new()
        {
            var result = new T();
            var fileInfo = new FileInfo(filePath);
            
            // 检查文件大小
            if (fileInfo.Length < 10 * 1024 * 1024) // 小于10MB，直接加载
            {
                return await LoadSmallFileAsync<T>(filePath);
            }
            
            // 标记为大型文件
            if (result is TerrainMaterialsDO terrainMaterials)
            {
                terrainMaterials.IsLargeFile = true;
            }
            else if (result is MPClassDivisionsDO mpDivisions)
            {
                mpDivisions.IsLargeFile = true;
            }
            
            // 分块处理
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var currentChunk = new List<TElement>();
                var lineCount = 0;
                
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    lineCount++;
                    
                    if (line.Contains($"<{elementName}"))
                    {
                        // 开始一个新元素
                        var elementContent = await ExtractElementContent(reader, elementName, line);
                        var element = DeserializeElement<TElement>(elementContent, elementName);
                        
                        if (element != null)
                        {
                            currentChunk.Add(element);
                            
                            if (currentChunk.Count >= chunkSize)
                            {
                                chunkProcessor(result, currentChunk);
                                currentChunk.Clear();
                                
                                // 内存检查
                                if (lineCount % 10000 == 0)
                                {
                                    await CheckMemoryUsageAsync();
                                }
                            }
                        }
                    }
                }
                
                // 处理剩余的元素
                if (currentChunk.Count > 0)
                {
                    chunkProcessor(result, currentChunk);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// 提取单个元素的内容
        /// </summary>
        private async Task<string> ExtractElementContent(StreamReader reader, string elementName, string startLine)
        {
            var content = startLine;
            var depth = 1;
            
            while (!reader.EndOfStream && depth > 0)
            {
                var line = await reader.ReadLineAsync();
                content += Environment.NewLine + line;
                
                if (line.Contains($"<{elementName}"))
                {
                    depth++;
                }
                else if (line.Contains($"</{elementName}>"))
                {
                    depth--;
                }
            }
            
            return content;
        }
        
        /// <summary>
        /// 反序列化单个元素
        /// </summary>
        private TElement DeserializeElement<TElement>(string elementContent, string elementName)
        {
            try
            {
                if (typeof(TElement) == typeof(TerrainMaterialDO))
                {
                    var terrainSerializer = new XmlSerializer(typeof(TerrainMaterialDO));
                    using (var stringReader = new StringReader(elementContent))
                    {
                        return (TElement)(object)terrainSerializer.Deserialize(stringReader)!;
                    }
                }
                else if (typeof(TElement) == typeof(MPClassDivisionDO))
                {
                    var mpSerializer = new XmlSerializer(typeof(MPClassDivisionDO));
                    using (var stringReader = new StringReader(elementContent))
                    {
                        return (TElement)(object)mpSerializer.Deserialize(stringReader)!;
                    }
                }
                else
                {
                    return default(TElement);
                }
            }
            catch
            {
                // 记录错误但继续处理
                return default(TElement);
            }
        }
        
        /// <summary>
        /// 加载小型文件
        /// </summary>
        private Task<T> LoadSmallFileAsync<T>(string filePath) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            
            return Task.Run(() =>
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    return (T)serializer.Deserialize(stream)!;
                }
            });
        }
        
        /// <summary>
        /// 检查内存使用情况
        /// </summary>
        private async Task CheckMemoryUsageAsync()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var memoryUsedMB = currentProcess.WorkingSet64 / (1024 * 1024);
            
            if (memoryUsedMB > MaxMemoryUsageMB)
            {
                // 内存使用过高，触发垃圾回收
                await Task.Run(() => GC.Collect());
                await Task.Delay(100); // 给垃圾回收一些时间
            }
        }
        
        /// <summary>
        /// 获取文件大小信息
        /// </summary>
        public FileSizeInfo GetFileSizeInfo(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            
            return new FileSizeInfo
            {
                FilePath = filePath,
                SizeBytes = fileInfo.Length,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                IsLargeFile = fileInfo.Length > 10 * 1024 * 1024, // 10MB
                IsVeryLargeFile = fileInfo.Length > 100 * 1024 * 1024 // 100MB
            };
        }
        
        /// <summary>
        /// 优化处理参数
        /// </summary>
        public ProcessingParameters GetOptimalProcessingParameters(string filePath)
        {
            var sizeInfo = GetFileSizeInfo(filePath);
            
            return new ProcessingParameters
            {
                ChunkSize = sizeInfo.IsVeryLargeFile ? 500 : DefaultChunkSize,
                UseParallelProcessing = sizeInfo.IsVeryLargeFile,
                EnableMemoryOptimization = sizeInfo.IsLargeFile,
                MaxConcurrentOperations = sizeInfo.IsVeryLargeFile ? 2 : 1
            };
        }
    }
    
    /// <summary>
    /// 文件大小信息
    /// </summary>
    public class FileSizeInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public double SizeMB { get; set; }
        public bool IsLargeFile { get; set; }
        public bool IsVeryLargeFile { get; set; }
    }
    
    /// <summary>
    /// 处理参数
    /// </summary>
    public class ProcessingParameters
    {
        public int ChunkSize { get; set; } = 1000;
        public bool UseParallelProcessing { get; set; }
        public bool EnableMemoryOptimization { get; set; }
        public int MaxConcurrentOperations { get; set; } = 1;
    }
}