using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BannerlordModEditor.Common.Models.DO.Testing;
using BannerlordModEditor.Common.Models.DTO.Testing;
using BannerlordModEditor.Common.Mappers.Testing;

namespace BannerlordModEditor.Common.Repositories.Testing
{
    /// <summary>
    /// 测试结果仓库
    /// 负责测试会话和结果的持久化存储
    /// </summary>
    public class TestResultRepository
    {
        private readonly string _storagePath;
        private readonly object _lock = new();
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// 初始化测试结果仓库
        /// </summary>
        public TestResultRepository(string storagePath)
        {
            _storagePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
            
            // 确保存储目录存在
            Directory.CreateDirectory(_storagePath);
            
            // 配置JSON序列化选项
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// 保存测试会话
        /// </summary>
        public async Task SaveTestSessionAsync(TestSessionDO session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var sessionDto = TestSessionMapper.ToDTO(session);
                var fileName = GetSessionFileName(session.SessionId);
                var filePath = Path.Combine(_storagePath, fileName);

                var json = JsonSerializer.Serialize(sessionDto, _jsonOptions);
                await File.WriteAllTextAsync(filePath, json);

                stopwatch.Stop();
                Console.WriteLine($"保存测试会话 {session.SessionId} 耗时: {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"保存测试会话失败: {ex.Message}");
                throw new TestResultRepositoryException($"保存测试会话失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 加载测试会话
        /// </summary>
        public async Task<TestSessionDO?> LoadTestSessionAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("会话ID不能为空", nameof(sessionId));

            var fileName = GetSessionFileName(sessionId);
            var filePath = Path.Combine(_storagePath, fileName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var sessionDto = JsonSerializer.Deserialize<TestSessionDTO>(json, _jsonOptions);
                
                return sessionDto != null ? TestSessionMapper.ToDO(sessionDto) : null;
            }
            catch (Exception ex)
            {
                throw new TestResultRepositoryException($"加载测试会话失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取所有测试会话
        /// </summary>
        public async Task<List<TestSessionDO>> GetAllTestSessionsAsync()
        {
            var sessions = new List<TestSessionDO>();

            try
            {
                var sessionFiles = Directory.GetFiles(_storagePath, "test_session_*.json");

                foreach (var file in sessionFiles)
                {
                    try
                    {
                        var json = await File.ReadAllTextAsync(file);
                        var sessionDto = JsonSerializer.Deserialize<TestSessionDTO>(json, _jsonOptions);
                        
                        if (sessionDto != null)
                        {
                            sessions.Add(TestSessionMapper.ToDO(sessionDto));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"加载会话文件 {file} 失败: {ex.Message}");
                    }
                }

                return sessions.OrderByDescending(s => s.StartTime).ToList();
            }
            catch (Exception ex)
            {
                throw new TestResultRepositoryException($"获取测试会话列表失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取最近的测试会话
        /// </summary>
        public async Task<List<TestSessionDO>> GetRecentTestSessionsAsync(int count = 10)
        {
            var allSessions = await GetAllTestSessionsAsync();
            return allSessions.Take(count).ToList();
        }

        /// <summary>
        /// 根据状态获取测试会话
        /// </summary>
        public async Task<List<TestSessionDO>> GetTestSessionsByStatusAsync(TestSessionStatus status)
        {
            var allSessions = await GetAllTestSessionsAsync();
            return allSessions.Where(s => s.SessionStatus == status).ToList();
        }

        /// <summary>
        /// 根据项目路径获取测试会话
        /// </summary>
        public async Task<List<TestSessionDO>> GetTestSessionsByProjectAsync(string projectPath)
        {
            var allSessions = await GetAllTestSessionsAsync();
            return allSessions.Where(s => s.ProjectPath.Equals(projectPath, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// 删除测试会话
        /// </summary>
        public async Task DeleteTestSessionAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("会话ID不能为空", nameof(sessionId));

            var fileName = GetSessionFileName(sessionId);
            var filePath = Path.Combine(_storagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 删除旧的测试会话
        /// </summary>
        public async Task CleanupOldSessionsAsync(DateTime cutoffDate)
        {
            try
            {
                var sessionFiles = Directory.GetFiles(_storagePath, "test_session_*.json");
                var deletedCount = 0;

                foreach (var file in sessionFiles)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.CreationTime < cutoffDate)
                    {
                        File.Delete(file);
                        deletedCount++;
                    }
                }

                Console.WriteLine($"清理了 {deletedCount} 个旧测试会话");
            }
            catch (Exception ex)
            {
                throw new TestResultRepositoryException($"清理旧会话失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取测试会话统计信息
        /// </summary>
        public async Task<TestSessionStatistics> GetSessionStatisticsAsync()
        {
            try
            {
                var allSessions = await GetAllTestSessionsAsync();
                
                return new TestSessionStatistics
                {
                    TotalSessions = allSessions.Count,
                    CompletedSessions = allSessions.Count(s => s.SessionStatus == TestSessionStatus.Completed),
                    FailedSessions = allSessions.Count(s => s.SessionStatus == TestSessionStatus.Failed),
                    CancelledSessions = allSessions.Count(s => s.SessionStatus == TestSessionStatus.Cancelled),
                    TotalTests = allSessions.Sum(s => s.TotalTests),
                    TotalPassedTests = allSessions.Sum(s => s.PassedTests),
                    TotalFailedTests = allSessions.Sum(s => s.FailedTests),
                    TotalSkippedTests = allSessions.Sum(s => s.SkippedTests),
                    AveragePassRate = allSessions.Count > 0 ? allSessions.Average(s => s.PassRate) : 0.0,
                    TotalExecutionTime = allSessions.Sum(s => s.TotalDurationMs),
                    LastSessionTime = allSessions.OrderByDescending(s => s.StartTime).FirstOrDefault()?.StartTime ?? DateTime.MinValue,
                    OldestSessionTime = allSessions.OrderBy(s => s.StartTime).FirstOrDefault()?.StartTime ?? DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                throw new TestResultRepositoryException($"获取会话统计信息失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 导出测试会话数据
        /// </summary>
        public async Task<string> ExportTestSessionsAsync(List<string> sessionIds)
        {
            var exportData = new TestSessionExport
            {
                ExportTime = DateTime.Now,
                Sessions = new List<TestSessionDO>()
            };

            foreach (var sessionId in sessionIds)
            {
                var session = await LoadTestSessionAsync(sessionId);
                if (session != null)
                {
                    exportData.Sessions.Add(session);
                }
            }

            return JsonSerializer.Serialize(exportData, _jsonOptions);
        }

        /// <summary>
        /// 导入测试会话数据
        /// </summary>
        public async Task ImportTestSessionsAsync(string jsonData)
        {
            try
            {
                var exportData = JsonSerializer.Deserialize<TestSessionExport>(jsonData, _jsonOptions);
                if (exportData?.Sessions != null)
                {
                    foreach (var session in exportData.Sessions)
                    {
                        await SaveTestSessionAsync(session);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TestResultRepositoryException($"导入测试会话失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取会话文件名
        /// </summary>
        private string GetSessionFileName(string sessionId)
        {
            return $"test_session_{sessionId}.json";
        }

        /// <summary>
        /// 获取存储空间使用情况
        /// </summary>
        public async Task<StorageUsageInfo> GetStorageUsageInfoAsync()
        {
            try
            {
                var directoryInfo = new DirectoryInfo(_storagePath);
                var files = directoryInfo.GetFiles("test_session_*.json");

                return new StorageUsageInfo
                {
                    TotalFiles = files.Length,
                    TotalSizeBytes = files.Sum(f => f.Length),
                    OldestFileTime = files.Min(f => f.CreationTime),
                    NewestFileTime = files.Max(f => f.CreationTime),
                    AverageFileSizeBytes = files.Length > 0 ? files.Average(f => f.Length) : 0
                };
            }
            catch (Exception ex)
            {
                throw new TestResultRepositoryException($"获取存储使用信息失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证存储完整性
        /// </summary>
        public async Task<StorageValidationResult> ValidateStorageIntegrityAsync()
        {
            var result = new StorageValidationResult
            {
                IsValid = true,
                Errors = new List<string>(),
                Warnings = new List<string>()
            };

            try
            {
                var sessionFiles = Directory.GetFiles(_storagePath, "test_session_*.json");

                foreach (var file in sessionFiles)
                {
                    try
                    {
                        var json = await File.ReadAllTextAsync(file);
                        var sessionDto = JsonSerializer.Deserialize<TestSessionDTO>(json, _jsonOptions);
                        
                        if (sessionDto == null)
                        {
                            result.Errors.Add($"文件 {file} 反序列化失败");
                            result.IsValid = false;
                        }
                        else if (string.IsNullOrWhiteSpace(sessionDto.SessionId))
                        {
                            result.Errors.Add($"文件 {file} 缺少会话ID");
                            result.IsValid = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"文件 {file} 验证失败: {ex.Message}");
                        result.IsValid = false;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"存储验证失败: {ex.Message}");
                result.IsValid = false;
                return result;
            }
        }
    }

    /// <summary>
    /// 测试会话统计信息
    /// </summary>
    public class TestSessionStatistics
    {
        /// <summary>
        /// 总会话数
        /// </summary>
        public int TotalSessions { get; set; }

        /// <summary>
        /// 已完成会话数
        /// </summary>
        public int CompletedSessions { get; set; }

        /// <summary>
        /// 失败会话数
        /// </summary>
        public int FailedSessions { get; set; }

        /// <summary>
        /// 取消会话数
        /// </summary>
        public int CancelledSessions { get; set; }

        /// <summary>
        /// 总测试数
        /// </summary>
        public int TotalTests { get; set; }

        /// <summary>
        /// 总通过测试数
        /// </summary>
        public int TotalPassedTests { get; set; }

        /// <summary>
        /// 总失败测试数
        /// </summary>
        public int TotalFailedTests { get; set; }

        /// <summary>
        /// 总跳过测试数
        /// </summary>
        public int TotalSkippedTests { get; set; }

        /// <summary>
        /// 平均通过率
        /// </summary>
        public double AveragePassRate { get; set; }

        /// <summary>
        /// 总执行时间
        /// </summary>
        public long TotalExecutionTime { get; set; }

        /// <summary>
        /// 最后会话时间
        /// </summary>
        public DateTime LastSessionTime { get; set; }

        /// <summary>
        /// 最旧会话时间
        /// </summary>
        public DateTime OldestSessionTime { get; set; }
    }

    /// <summary>
    /// 测试会话导出数据
    /// </summary>
    public class TestSessionExport
    {
        /// <summary>
        /// 导出时间
        /// </summary>
        public DateTime ExportTime { get; set; }

        /// <summary>
        /// 会话列表
        /// </summary>
        public List<TestSessionDO> Sessions { get; set; } = new List<TestSessionDO>();
    }

    /// <summary>
    /// 存储使用信息
    /// </summary>
    public class StorageUsageInfo
    {
        /// <summary>
        /// 总文件数
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// 总大小（字节）
        /// </summary>
        public long TotalSizeBytes { get; set; }

        /// <summary>
        /// 最旧文件时间
        /// </summary>
        public DateTime OldestFileTime { get; set; }

        /// <summary>
        /// 最新文件时间
        /// </summary>
        public DateTime NewestFileTime { get; set; }

        /// <summary>
        /// 平均文件大小（字节）
        /// </summary>
        public double AverageFileSizeBytes { get; set; }
    }

    /// <summary>
    /// 存储验证结果
    /// </summary>
    public class StorageValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// 警告列表
        /// </summary>
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// 测试结果仓库异常
    /// </summary>
    public class TestResultRepositoryException : Exception
    {
        public TestResultRepositoryException(string message) : base(message) { }

        public TestResultRepositoryException(string message, Exception innerException) : base(message, innerException) { }
    }
}